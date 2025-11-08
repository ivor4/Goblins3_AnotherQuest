using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gob3AQ.GameElement.PlayableChar
{
    public enum PhysicalState
    {
        PHYSICAL_STATE_STANDING = 0x0,
        PHYSICAL_STATE_TALKING = 0x1,
        PHYSICAL_STATE_ACTING = 0x2,
        PHYSICAL_STATE_WALKING = 0x4,
        PHYSICAL_STATE_LOCKED = 0x8,
    }

    


    [System.Serializable]
    public class PlayableCharScript : GameElementClass
    {
        /* Fields */
        [SerializeField]
        private CharacterType charType;

        public CharacterType CharType => charType;

        /* GameObject components */
        private Transform _parentTransform;

        /* Status */
        private PhysicalState physicalstate;
        private float actTimeout;

        private bool selected;
        private WaypointProgrammedPath actualProgrammedPath;

        private IReadOnlyList<Vector2> waypoints;
        private IReadOnlyList<WaypointSolution> solutions;

        private struct WaypointProgrammedPath
        {
            public int target_index;
            public int final_index;

            public WaypointProgrammedPath(int target, int final)
            {
                target_index = target;
                final_index = final;
            }
        }


        #region "Services"

        public void LockRequest(bool enablelock)
        {
            if (enablelock)
            {
                if (IsAvailable)
                {
                    /* Lock the character */
                    physicalstate = PhysicalState.PHYSICAL_STATE_LOCKED;
                }
            }
            else
            {
                if(physicalstate == PhysicalState.PHYSICAL_STATE_LOCKED)
                {
                    /* Unlock the character */
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                }
            }
        }


        public bool ActionRequest(int destWp_index)
        {
            bool requestAccepted;

            /* Interact only if not talking or doing an action */
            if (IsAvailable)
            {
                actualProgrammedPath.final_index = destWp_index;
                physicalstate = PhysicalState.PHYSICAL_STATE_WALKING;

                SetActive_Internal(true);
                Walk_StartNextSegment(false);

                requestAccepted = true;
            }
            else
            {
                requestAccepted = false;
            }

            return requestAccepted;
        }


#endregion



        protected override void Awake()
        {
            base.Awake();

            gameElementFamily = GameItemFamily.ITEM_FAMILY_TYPE_PLAYER;

            topParent = transform.parent.gameObject;
            _parentTransform = topParent.transform;
            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();

            SetVisible_Internal(false);
        }

        protected override void Start()
        {
            base.Start();

            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            selected = false;
            actTimeout = 0f;
            SetAvailable(true);

            PlayerMasterClass.SetPlayerLoadPresent(CharType);

            GameElementClickable clickable = topParent.GetComponent<GameElementClickable>();

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.ITEM_REGISTER(true, this, clickable);
            VARMAP_PlayerMaster.REG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);

            clickable.SetOnHoverAction(MouseEnterAction);

            registered = true;

            VARMAP_PlayerMaster.GET_WP_LIST(out waypoints, out solutions);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }

        private void Update()
        {
            /* Script will only be enabled in play mode */
            Execute_Play();
        }



        protected override void OnDestroy()
        {
            base.OnDestroy();

            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
            VARMAP_PlayerMaster.UNREG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
        }


        #region "Private Methods "

        private IEnumerator Execute_Loading_Coroutine()
        {
            bool loadOk = false;

            while (!loadOk)
            {
                loadOk = Execute_Loading_Action();
                yield return ResourceAtlasClass.WaitForNextFrame;
            }
        }

        private bool Execute_Loading_Action()
        {
            bool loadOk;

            VARMAP_PlayerMaster.IS_MODULE_LOADED(GameModules.MODULE_LevelMaster, out loadOk);

            if (loadOk)
            {
                int wpStartIndex = VARMAP_PlayerMaster.GET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)charType);
                VARMAP_PlayerMaster.GET_NEAREST_WP(_parentTransform.position, float.MaxValue, out int nearestWp_index, out _);


                if (wpStartIndex == -1)
                {
                    actualWaypoint = nearestWp_index;
                }
                else
                {
                    actualWaypoint = wpStartIndex;
                }

                _parentTransform.position = waypoints[actualWaypoint];

                actualProgrammedPath.target_index = actualWaypoint;
                actualProgrammedPath.final_index = actualWaypoint;

                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualWaypoint);

                SetVisible_Internal(true);
                PlayerMasterClass.SetPlayerLoaded(CharType);

                UpdateSortingOrder();
            }


            return loadOk;
        }

        private void Execute_Play()
        {
            bool continueLoop = true;

            Game_Status gstatus = VARMAP_PlayerMaster.GET_GAMESTATUS();
            if (gstatus == Game_Status.GAME_STATUS_PLAY)
            {
                switch (physicalstate)
                {
                    case PhysicalState.PHYSICAL_STATE_WALKING:
                        continueLoop = Execute_Walk();
                        break;
                    case PhysicalState.PHYSICAL_STATE_ACTING:
                        continueLoop = Execute_Act();
                        break;
                    default:
                        continueLoop = false;
                        break;
                }
            }
            else
            {
                continueLoop = true;
            }

            UpdateSortingOrder();
            SetActive_Internal(continueLoop);
            SetAvailable((physicalstate == PhysicalState.PHYSICAL_STATE_STANDING) || (physicalstate == PhysicalState.PHYSICAL_STATE_WALKING));
        }

        private bool Execute_Walk()
        {
            bool continueOp;
            Vector3 target_pos = waypoints[actualProgrammedPath.target_index];
            Vector2 deltaPos = target_pos - _parentTransform.position;

            /* If delta vector of positions and original velocity vector lose their cos(0deg)=1,
             * it means character crossed the point */
            float dot = Vector2.Dot(deltaPos, myRigidbody.linearVelocity);

            if(dot <= 0f)
            {
                /* Store WP Index */
                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualProgrammedPath.target_index);

                /* If last segment */
                if(actualProgrammedPath.target_index == actualProgrammedPath.final_index)
                {
                    actualWaypoint = actualProgrammedPath.target_index;
                    _parentTransform.position = target_pos;
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                    myRigidbody.linearVelocity = Vector2.zero;

                    continueOp = StartBufferedInteraction();
                }
                else
                {
                    Walk_StartNextSegment(true);
                    continueOp = true;
                }
            }
            else
            {
                continueOp = true;
            }

            return continueOp;
        }

        private bool Execute_Act()
        {
            bool continueOp;
            actTimeout -= Time.deltaTime;

            if(actTimeout <= 0f)
            {
                actTimeout = 0f;
                physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                continueOp = false;

                if(selected)
                {
                    mySpriteRenderer.color = Color.red;
                }
                else
                {
                    mySpriteRenderer.color = Color.white;
                }
            }
            else
            {
                continueOp = true;
            }

            return continueOp;
        }


        private void Walk_StartNextSegment(bool reached)
        {
            Vector2 delta;

            if(reached)
            {
                actualWaypoint = actualProgrammedPath.target_index;
                _parentTransform.position = waypoints[actualWaypoint];
                actualProgrammedPath.target_index = solutions[actualWaypoint].TravelTo[actualProgrammedPath.final_index];
            }

            delta = (waypoints[actualProgrammedPath.target_index] - waypoints[actualWaypoint]).normalized;

            myRigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * delta;
        }

        private bool StartBufferedInteraction()
        {
            bool continueOp = false;

            VARMAP_PlayerMaster.PLAYER_REACHED_WAYPOINT(charType);

            return continueOp;
        }

        private void ActAnimationRequest(CharacterAnimation animation)
        {
            _ = animation;

            physicalstate = PhysicalState.PHYSICAL_STATE_ACTING;
            mySpriteRenderer.color = Color.blue;
            actTimeout = 1f;
        }

#endregion


        #region "Events"
        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, in CharacterType oldval, in CharacterType newval)
        {
            if(newval == charType)
            {
                mySpriteRenderer.color = Color.red;
                selected = true;
            }
            else
            {
                mySpriteRenderer.color = Color.white;
                selected = false;
            }
        }

        #endregion


    }

}
