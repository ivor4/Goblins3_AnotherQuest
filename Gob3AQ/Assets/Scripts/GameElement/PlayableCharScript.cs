using Gob3AQ.FixedConfig;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
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

        private Vector3 sceneOrigSize;

        private IReadOnlyList<Vector2> waypoints_pos;
        private IReadOnlyList<float> waypoints_sizes;
        private IReadOnlyList<WaypointSolution> solutions;

        private struct WaypointProgrammedPath
        {
            public int target_index;
            public int final_index;
            public float wp_distance;
            public float initial_size;
            public float delta_size;
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

            topParent = transform.parent.gameObject;
            _parentTransform = topParent.transform;
            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();

            sceneOrigSize = _parentTransform.localScale;

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

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.REG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
            VARMAP_PlayerMaster.GET_WP_LIST(out waypoints_pos, out waypoints_sizes, out solutions);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }

        private void Update()
        {
            /* Script will only be enabled in play mode */
            Execute_Play();
        }



        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

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


                if (wpStartIndex == -1)
                {
                    if (startingWaypoint == null)
                    {
                        VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint, out _);
                    }
                    else
                    {
                        actualWaypoint = startingWaypoint.ID_in_Network;
                    }
                }
                else
                {
                    actualWaypoint = wpStartIndex;
                }

                _parentTransform.position = waypoints_pos[actualWaypoint];

                PresetProgrammedPathStruct(actualWaypoint);
                SetSize(waypoints_sizes[actualWaypoint]);

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
            Vector3 target_pos = waypoints_pos[actualProgrammedPath.target_index];
            Vector3 orig_pos = waypoints_pos[actualWaypoint];
            Vector2 deltaPos = _parentTransform.position - orig_pos;
            float crossed_distance = deltaPos.magnitude;
            float distance_clamped = Mathf.Min(crossed_distance, actualProgrammedPath.wp_distance);
            float interp_size;

            if(actualProgrammedPath.wp_distance == 0f)
            {
                interp_size = actualProgrammedPath.initial_size;
            }
            else
            {
                interp_size = actualProgrammedPath.initial_size + (actualProgrammedPath.delta_size * (distance_clamped / actualProgrammedPath.wp_distance));
            }
                
            SetSize(interp_size);

            if (crossed_distance >= actualProgrammedPath.wp_distance)
            {
                /* Store WP Index */
                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualProgrammedPath.target_index);

                /* If last segment */
                if(actualProgrammedPath.target_index == actualProgrammedPath.final_index)
                {
                    actualWaypoint = actualProgrammedPath.target_index;
                    PresetProgrammedPathStruct(actualWaypoint);
                    _parentTransform.position = target_pos;
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                    myRigidbody.linearVelocity = Vector2.zero;
                    SetSize(waypoints_sizes[actualWaypoint]);

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
                _parentTransform.position = waypoints_pos[actualWaypoint];
                actualProgrammedPath.target_index = solutions[actualWaypoint].TravelTo[actualProgrammedPath.final_index];

                actualProgrammedPath.wp_distance = (waypoints_pos[actualProgrammedPath.target_index] - waypoints_pos[actualWaypoint]).magnitude;
                actualProgrammedPath.initial_size = waypoints_sizes[actualWaypoint];
                actualProgrammedPath.delta_size = waypoints_sizes[actualProgrammedPath.target_index] - waypoints_sizes[actualWaypoint];
                SetSize(waypoints_sizes[actualWaypoint]);
            }

            delta = (waypoints_pos[actualProgrammedPath.target_index] - waypoints_pos[actualWaypoint]).normalized;

            myRigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * delta;
        }

        private bool StartBufferedInteraction()
        {
            bool continueOp = false;

            VARMAP_PlayerMaster.PLAYER_REACHED_WAYPOINT(charType);

            return continueOp;
        }

        private void SetSize(float size)
        {
            Vector3 scale = size * sceneOrigSize;
            _parentTransform.localScale = scale;
        }

        private void PresetProgrammedPathStruct(int waypoint_index)
        {
            actualProgrammedPath.target_index = waypoint_index;
            actualProgrammedPath.final_index = waypoint_index;
            actualProgrammedPath.wp_distance = 0f;
            actualProgrammedPath.initial_size = waypoints_sizes[waypoint_index];
            actualProgrammedPath.delta_size = 0f;
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
