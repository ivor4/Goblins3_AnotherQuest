using UnityEngine;
using UnityEditor;
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
        PHYSICAL_STATE_WALKING = 0x4
    }

    public enum BufferedInteraction
    {
        BUFFERED_INT_NONE,
        BUFFERED_INT_ITEM,
        BUFFERED_INT_TALK
    }


    [System.Serializable]
    public class PlayableCharScript : MonoBehaviour
    {
        /* Fields */
        [SerializeField]
        public CharacterType charType;

        public Collider2D Collider
        {
            get
            {
                return _collider;
            }
        }

        /* GameObject components */
        private SpriteRenderer _sprRenderer;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        /* Status */
        private PhysicalState physicalstate;

        private byte playerID;
        private bool selected;
        private bool loaded;
        private WaypointClass actualWaypoint;
        private WaypointProgrammedPath actualProgrammedPath;
        private GameItem bufferedInteractionItem;
        private BufferedInteraction bufferedInteraction;

        /// <summary>
        /// This is a preallocated list to avoid unnecessary allocs when asking for a calculated solution
        /// </summary>
        private List<WaypointClass> wpsolutionlist;


        #region "Services"

        public void MoveRequest(WaypointClass wp)
        {
            /* Move only if not talking or doing an action */
            if((physicalstate & (PhysicalState.PHYSICAL_STATE_TALKING | PhysicalState.PHYSICAL_STATE_ACTING)) == 0)
            {
                WaypointSolution solution = wp.Network.GetWaypointSolution(actualWaypoint, wp, WaypointSkillType.WAYPOINT_SKILL_NORMAL, wpsolutionlist);

                if(solution.totalDistance == float.PositiveInfinity)
                {
                    Debug.LogError("Point is not reachable from actual waypoint");
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                }
                else
                {
                    actualProgrammedPath = new WaypointProgrammedPath(solution);
                    physicalstate = PhysicalState.PHYSICAL_STATE_WALKING;
                    Walk_StartNextSegment(false);
                }

                /* Cancel interaction which could be ongoing */
                bufferedInteraction = BufferedInteraction.BUFFERED_INT_NONE;
            }
        }

        public void ItemInteractRequest(GameItem item, WaypointClass itemwp)
        {
            /* Interact only if not talking or doing an action */
            if ((physicalstate & (PhysicalState.PHYSICAL_STATE_TALKING | PhysicalState.PHYSICAL_STATE_ACTING)) == 0)
            {
                WaypointSolution solution = itemwp.Network.GetWaypointSolution(actualWaypoint, itemwp, WaypointSkillType.WAYPOINT_SKILL_NORMAL, wpsolutionlist);

                if (solution.totalDistance == float.PositiveInfinity)
                {
                    Debug.LogError("Point is not reachable from actual waypoint");
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                }
                else
                {
                    bufferedInteractionItem = item;
                    bufferedInteraction = BufferedInteraction.BUFFERED_INT_ITEM;

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
            bufferedInteraction = BufferedInteraction.BUFFERED_INT_NONE;

            VARMAP_PlayerMaster.MONO_REGISTER(this, true, out playerID);
            VARMAP_PlayerMaster.REG_PLAYER_ID_SELECTED(ChangedSelectedPlayerEvent);
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
            VARMAP_PlayerMaster.MONO_REGISTER(this, false, out _);
            VARMAP_PlayerMaster.UNREG_PLAYER_ID_SELECTED(ChangedSelectedPlayerEvent);
        }

        #region "Private Methods "

        private void Execute_Loading()
        {
            if(!loaded)
            {
                VARMAP_PlayerMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint);
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

            /* If delta vector of positions and original velocity vector lose their cos(0deg)=1, it means character crossed the point */
            float dot = Vector2.Dot(deltaPos, _rigidbody.linearVelocity);

            if(dot <= 0f)
            {
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

            /* Now interact if buffered */
            switch (bufferedInteraction)
            {
                case BufferedInteraction.BUFFERED_INT_ITEM:
                    InteractionItemType permittedInteraction;
                    VARMAP_PlayerMaster.GET_ITEM_INTERACTION(charType, bufferedInteractionItem, out permittedInteraction);

                    if(permittedInteraction == InteractionItemType.INTERACTION_TAKE)
                    {
                        GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)bufferedInteractionItem];
                        VARMAP_PlayerMaster.TAKE_ITEM_OBJECT(pickable);
                    }

                    break;

                default:
                    break;
            }

            /* Clear */
            bufferedInteraction = BufferedInteraction.BUFFERED_INT_NONE;
        }

        #endregion


        #region "Events"
        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, ref byte oldval, ref byte newval)
        {
            if(newval == playerID)
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
