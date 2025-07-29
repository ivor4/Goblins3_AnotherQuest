using UnityEngine;
using UnityEditor;
using Gob3AQ.PlayerMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Types;
using Gob3AQ.Waypoint.ProgrammedPath;
using Gob3AQ.Libs.Arith;
using Gob3AQ.Brain.ItemsInteraction;
using System.Collections.Generic;


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

    public struct BufferedData
    {
        public bool pending;
        public InteractionUsage usage;
    }


    [System.Serializable]
    public class PlayableCharScript : MonoBehaviour
    {
        /* Fields */
        [SerializeField]
        private CharacterType charType;

        public CharacterType CharType => charType;

        public Collider2D Collider => _collider;

        public bool IsSteady => (physicalstate == PhysicalState.PHYSICAL_STATE_STANDING) && (!bufferedData.pending);

        public WaypointClass Waypoint => actualWaypoint;

        /* GameObject components */
        private SpriteRenderer _sprRenderer;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        /* Status */
        private PhysicalState physicalstate;
        private float actTimeout;

        private bool selected;
        private bool loaded;
        private WaypointClass actualWaypoint;
        private WaypointProgrammedPath actualProgrammedPath;
        private BufferedData bufferedData;

        /// <summary>
        /// This is a preallocated list to avoid unnecessary allocs when asking for a calculated solution
        /// </summary>
        private List<WaypointClass> wpsolutionlist;


        #region "Services"

        public void LockRequest(bool enablelock)
        {
            if (enablelock)
            {
                if (IsSteady)
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





        public void ActionRequest(in InteractionUsage usage)
        {
            /* Interact only if not talking or doing an action */
            if ((physicalstate & (PhysicalState.PHYSICAL_STATE_LOCKED | PhysicalState.PHYSICAL_STATE_TALKING | PhysicalState.PHYSICAL_STATE_ACTING)) == 0)
            {
                WaypointSolution solution = usage.destWaypoint.Network.GetWaypointSolution(actualWaypoint, usage.destWaypoint,
                    WaypointSkillType.WAYPOINT_SKILL_NORMAL, wpsolutionlist);

                if (solution.totalDistance == float.PositiveInfinity)
                {
                    Debug.LogError("Point is not reachable from actual waypoint");
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                }
                else
                {
                    bufferedData.usage = usage;
                    bufferedData.pending = true;


                    actualProgrammedPath = new WaypointProgrammedPath(solution);
                    physicalstate = PhysicalState.PHYSICAL_STATE_WALKING; 

                    Walk_StartNextSegment(false);
                }
            }
        }


        #endregion



        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _sprRenderer.enabled = false;

            wpsolutionlist = new List<WaypointClass>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
        }

        private void Start()
        {
            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            selected = false;
            loaded = false;
            bufferedData.pending = false;
            actTimeout = 0f;

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.REG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
        }


        private void Update()
        {
            Game_Status gstatus = VARMAP_PlayerMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading();
                    break;

                case Game_Status.GAME_STATUS_PLAY:
                    Execute_Play();
                    break;

                default:
                    break;
            }
            
        }

        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
            VARMAP_PlayerMaster.UNREG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
        }

        #region "Private Methods "

        private void Execute_Loading()
        {
            if(!loaded)
            {
                int wpStartIndex = VARMAP_PlayerMaster.GET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)charType);
                VARMAP_PlayerMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out WaypointClass nearestWp);

                if (wpStartIndex == -1)
                {
                    actualWaypoint = nearestWp;
                }
                else
                {
                    actualWaypoint = nearestWp.Network.WaypointList[wpStartIndex];
                }

                transform.position = actualWaypoint.transform.position;
                _sprRenderer.enabled = true;
            }

            loaded = true;
        }

        private void Execute_Play()
        {
            switch(physicalstate)
            {
                case PhysicalState.PHYSICAL_STATE_WALKING:
                    Execute_Walk();
                    break;
                case PhysicalState.PHYSICAL_STATE_ACTING:
                    Execute_Act();
                    break;

                default:
                    break;
            }
        }

        private void Execute_Walk()
        {
            List<WaypointClass> wplist = actualProgrammedPath.originalSolution.waypointTrace;
            int seg_index = actualProgrammedPath.crossedWaypointIndex;
            WaypointClass target_wp = wplist[seg_index];
            Vector3 target_pos = target_wp.transform.position;
            Vector2 deltaPos = target_pos - transform.position;

            /* If delta vector of positions and original velocity vector lose their cos(0deg)=1,
             * it means character crossed the point */
            float dot = Vector2.Dot(deltaPos, _rigidbody.linearVelocity);

            if(dot <= 0f)
            {
                /* Store WP Index */
                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, target_wp.IndexInNetwork);

                /* If last segment */
                if(actualProgrammedPath.crossedWaypointIndex == (wplist.Count - 1))
                {
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                    transform.position = target_pos;
                    _rigidbody.linearVelocity = Vector2.zero;

                    StartBufferedInteraction();
                }
                else
                {
                    Walk_StartNextSegment(true);
                }
            }
        }

        private void Execute_Act()
        {
            actTimeout -= Time.deltaTime;

            if(actTimeout <= 0f)
            {
                actTimeout = 0f;
                physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;

                if(selected)
                {
                    _sprRenderer.color = Color.red;
                }
                else
                {
                    _sprRenderer.color = Color.white;
                }
            }
        }

        private void Walk_StartNextSegment(bool reached)
        {
            List<WaypointClass> wplist = actualProgrammedPath.originalSolution.waypointTrace;
            WaypointClass target_wp;
            Vector2 delta;

            if (reached)
            {
                actualProgrammedPath.crossedWaypointIndex++;
                target_wp = wplist[actualProgrammedPath.crossedWaypointIndex];
                delta = (target_wp.transform.position - actualWaypoint.transform.position).normalized;
                transform.position = actualWaypoint.transform.position;
            }
            else
            {
                target_wp = wplist[actualProgrammedPath.crossedWaypointIndex];
                delta = (target_wp.transform.position - transform.position).normalized;
            }

            _rigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * delta;
            
            actualWaypoint = target_wp;
        }

        private void StartBufferedInteraction()
        {
            bool validTransaction;

            /* Now interact if buffered */
            if (bufferedData.pending)
            {
                switch (bufferedData.usage.type)
                {
                    case InteractionType.PLAYER_WITH_ITEM:
                    case InteractionType.ITEM_WITH_ITEM:
                    case InteractionType.ITEM_WITH_PLAYER:
                    case InteractionType.ITEM_WITH_NPC:

                        

                        if(bufferedData.usage.type == InteractionType.ITEM_WITH_PLAYER)
                        {
                            validTransaction = PlayerMasterClass.IsPlayerInSameState(bufferedData.usage.playerDest,
                                bufferedData.usage.playerTransactionId, actualWaypoint);
                        }
                        else
                        {
                            validTransaction = true;
                        }

                        if (validTransaction)
                        {
                            /* Use Item is also Take Item */
                            VARMAP_PlayerMaster.USE_ITEM(in bufferedData.usage, out ItemInteractionType permitted,
                                out CharacterAnimation animation);

                            /* If action is valid */
                            if (permitted != ItemInteractionType.INTERACTION_NONE)
                            {
                                ActAnimationRequest(animation);
                            }
                        }

                        break;

                    case InteractionType.PLAYER_WITH_NPC:
                        /* Talk */
                        VARMAP_PlayerMaster.INTERACT_PLAYER_NPC(charType, bufferedData.usage.destListIndex);
                        break;

                    case InteractionType.PLAYER_WITH_DOOR:
                        VARMAP_PlayerMaster.CROSS_DOOR(charType, bufferedData.usage.destListIndex);
                        break;

                    default:
                        break;
                
                }

                /* Clear */
                bufferedData.pending = false;
            }
        }

        private void ActAnimationRequest(CharacterAnimation animation)
        {
            _ = animation;

            physicalstate = PhysicalState.PHYSICAL_STATE_ACTING;
            _sprRenderer.color = Color.blue;
            actTimeout = 1f;
        }

        #endregion


        #region "Events"
        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, ref CharacterType oldval, ref CharacterType newval)
        {
            if(newval == charType)
            {
                _sprRenderer.color = Color.red;
                selected = true;
            }
            else
            {
                _sprRenderer.color = Color.white;
                selected = false;
            }
        }

        #endregion


    }

}
