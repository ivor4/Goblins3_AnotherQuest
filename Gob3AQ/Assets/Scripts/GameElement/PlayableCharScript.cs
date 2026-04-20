using Gob3AQ.FixedConfig;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint.Network;
using System;
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

        private WaypointProgrammedPath actualProgrammedPath;

        private IReadOnlyList<WaypointInfo> waypoints_infos;

        private struct WaypointProgrammedPath
        {
            public int target_index;
            public int final_index;
            public float wp_distance;
            public float wp_distance3D;
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

                /* If already walking, complete its actual segment. If stopped, start with first inteded segment of new path from actual waypoint */
                if((physicalstate != PhysicalState.PHYSICAL_STATE_WALKING)&&(actualWaypoint != destWp_index))
                {
                    Walk_StartNextSegment();
                }
                physicalstate = PhysicalState.PHYSICAL_STATE_WALKING;

                SetActive_Internal(true);
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
            myAnimator = topParent.GetComponent<Animator>();
            myAnimatorBehavior = myAnimator.GetBehaviour<GenericAnimBehavior>();
            myAnimatorBehavior.SetOnStartEndCallback(OnAnimationStart, OnAnimationEnd);

            SetVisible_Internal(false);
        }

        protected override void Start()
        {
            base.Start();

            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            actTimeout = 0f;
            SetAvailable(true);

            PlayerMasterClass.SetPlayerLoadPresent(CharType);

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.GET_WP_LIST(out waypoints_infos);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }

        private void Update()
        {
            /* Script will only be enabled in play mode */
            Execute_Play();
        }

        protected override void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)(_parentTransform.position.y * 1000);
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
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
                    actualWaypoint = 0;
                    
                }
                else
                {
                    actualWaypoint = wpStartIndex;
                }

                _parentTransform.position = waypoints_infos[actualWaypoint].Position;

                PresetProgrammedPathStruct(actualWaypoint);
                SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualWaypoint);

                mySpriteRenderer.flipX = waypoints_infos[actualWaypoint].FlipXForAction;

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
            Vector3 target_pos = waypoints_infos[actualProgrammedPath.target_index].Position;
            Vector3 orig_pos = waypoints_infos[actualWaypoint].Position;
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

                bool neededEventOk;

                if (waypoints_infos[actualProgrammedPath.target_index].Reachability == WaypointReachability.REACHABLE_WHEN_COMBI)
                {
                    Span<GameEventCombi> eventCombi = stackalloc GameEventCombi[1];
                    GameEventCombi_prv eventCombiPrv = waypoints_infos[actualProgrammedPath.target_index].NeededEvent;
                    eventCombi[0] = new(eventCombiPrv.ev, eventCombiPrv.not);
                    VARMAP_ItemMaster.IS_EVENT_COMBI_OCCURRED(eventCombi, out neededEventOk);
                }
                else
                {
                    neededEventOk = true;
                }


                /* If last segment or action triggered or next waypoint or really unreachable */
                if((actualProgrammedPath.target_index == actualProgrammedPath.final_index)||
                    (waypoints_infos[actualProgrammedPath.target_index].ActionWhenCross != GameAction.ACTION_NONE)||
                    (!neededEventOk))
                {
                    actualWaypoint = actualProgrammedPath.target_index;
                    PresetProgrammedPathStruct(actualWaypoint);
                    _parentTransform.position = target_pos;
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                    myRigidbody.linearVelocity = Vector2.zero;
                    SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

                    ResetWalkTriggers();
                    myAnimator.SetTrigger(ResourceAnimationsAtlasClass.ANIM_TRIGGER_TO_HASH[AnimationTrigger.ANIMATION_TRIGGER_STEADY]);
                    mySpriteRenderer.flipX = waypoints_infos[actualWaypoint].FlipXForAction;

                    continueOp = StartBufferedInteraction();
                }
                else
                {
                    Walk_StartNextSegment();
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
            }
            else
            {
                continueOp = true;
            }

            return continueOp;
        }


        private void Walk_StartNextSegment()
        {
            Vector2 delta;
            float speed_reduction_factor = 1f;


            actualWaypoint = actualProgrammedPath.target_index;
            _parentTransform.position = waypoints_infos[actualWaypoint].Position;
            actualProgrammedPath.target_index = waypoints_infos[actualWaypoint].Solution.TravelTo[actualProgrammedPath.final_index];

            actualProgrammedPath.wp_distance = ((Vector2)waypoints_infos[actualProgrammedPath.target_index].Position - (Vector2)waypoints_infos[actualWaypoint].Position).magnitude;
            actualProgrammedPath.wp_distance3D = (waypoints_infos[actualProgrammedPath.target_index].Position - waypoints_infos[actualWaypoint].Position).magnitude;
            actualProgrammedPath.initial_size = waypoints_infos[actualWaypoint].CharacterSizeFactor;
            actualProgrammedPath.delta_size = waypoints_infos[actualProgrammedPath.target_index].CharacterSizeFactor - waypoints_infos[actualWaypoint].CharacterSizeFactor;
            SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

            delta = ((Vector2)waypoints_infos[actualProgrammedPath.target_index].Position - (Vector2)waypoints_infos[actualWaypoint].Position).normalized;

            if (actualProgrammedPath.wp_distance3D != 0f)
            {
                speed_reduction_factor = actualProgrammedPath.wp_distance / actualProgrammedPath.wp_distance3D;
            }

            float absDeltaX = Mathf.Abs(delta.x);
            float absDeltaY = Mathf.Abs(delta.y);
            AnimationTrigger walkdirTrigger;

            if(absDeltaX >= 0.985f)
            {
                walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE;

                mySpriteRenderer.flipX = delta.x > 0;
            }
            else if(absDeltaX >= 0.173f)
            {
                if (delta.y >= 0f)
                {
                    walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERBACK;
                }
                else
                {
                    walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERFRONT;
                }

                mySpriteRenderer.flipX = delta.x > 0;
            }
            else
            {
                if (delta.y >= 0f)
                {
                    walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK;
                }
                else
                {
                    walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_FRONT;
                }

                mySpriteRenderer.flipX = false;
            }


            ResetWalkTriggers();
            myAnimator.SetTrigger(ResourceAnimationsAtlasClass.ANIM_TRIGGER_TO_HASH[walkdirTrigger]);

            /* Remove after debugging */
            speed_reduction_factor = 1f;

            myRigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * speed_reduction_factor * delta;
        }

        private bool StartBufferedInteraction()
        {
            bool continueOp = false;

            VARMAP_PlayerMaster.PLAYER_REACHED_WAYPOINT(charType);

            return continueOp;
        }

        private void SetSize(float size)
        {
            Vector3 scale = size * Vector3.one;
            _parentTransform.localScale = scale;
        }

        private void PresetProgrammedPathStruct(int waypoint_index)
        {
            actualProgrammedPath.target_index = waypoint_index;
            actualProgrammedPath.final_index = waypoint_index;
            actualProgrammedPath.wp_distance = 0f;
            actualProgrammedPath.wp_distance3D = 0f;
            actualProgrammedPath.initial_size = waypoints_infos[waypoint_index].CharacterSizeFactor;
            actualProgrammedPath.delta_size = 0f;
        }

        


#endregion


#region "Events"

#endregion


    }

}
