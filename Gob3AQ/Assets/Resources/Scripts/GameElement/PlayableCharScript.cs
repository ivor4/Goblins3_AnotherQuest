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
using System.Collections;
using Unity.VisualScripting;


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
        private WaypointClass actualWaypoint;
        private WaypointProgrammedPath actualProgrammedPath;
        private BufferedData bufferedData;
        private Coroutine _playCoroutine;

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

                    /* Activate coroutine if not yet active */
                    if (_playCoroutine == null)
                    {
                        _playCoroutine = StartCoroutine(Execute_Play_Coroutine());
                    }
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
            bufferedData.pending = false;
            actTimeout = 0f;

            PlayerMasterClass.SetPlayerAvailable(CharType);

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.REG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
            VARMAP_PlayerMaster.REG_GAMESTATUS(ChangedGameStatus);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }




        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
            VARMAP_PlayerMaster.UNREG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
            VARMAP_PlayerMaster.UNREG_GAMESTATUS(ChangedGameStatus);

            if(_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
                _playCoroutine = null;
            }
        }

        #region "Private Methods "

        private IEnumerator Execute_Loading_Coroutine()
        {
            bool loadOk = false;

            while (!loadOk)
            {
                loadOk = Execute_Loading_Action();
                yield return new WaitForNextFrameUnit();
            }
        }

        private bool Execute_Loading_Action()
        {
            bool loadOk;
            int wpStartIndex = VARMAP_PlayerMaster.GET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)charType);
            VARMAP_PlayerMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out WaypointClass nearestWp);

            /* Wait until Waypoints have loaded their network */
            if ((nearestWp != null) && (nearestWp.Network != null) && (nearestWp.Network.IsCalculated))
            {
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
                VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualWaypoint.IndexInNetwork);

                PlayerMasterClass.SetPlayerLoaded(CharType);
                loadOk = true;
            }
            else
            {
                loadOk = false;
            }

            return loadOk;
        }

        private IEnumerator Execute_Play_Coroutine()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
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

                yield return new WaitForNextFrameUnit();
            }

            _playCoroutine = null;
        }

        private bool Execute_Walk()
        {
            bool continueOp;
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
                    _sprRenderer.color = Color.red;
                }
                else
                {
                    _sprRenderer.color = Color.white;
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

        private bool StartBufferedInteraction()
        {
            bool validTransaction;
            bool continueOp = false;

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
                            VARMAP_PlayerMaster.USE_ITEM(in bufferedData.usage, out InteractionUsageOutcome outcome);

                            if(outcome.dialogType != DialogType.DIALOG_NONE)
                            {
                                VARMAP_PlayerMaster.ENABLE_DIALOGUE(true, CharType, outcome.dialogType, outcome.dialogPhrase);
                            }
                            else if (outcome.animation != CharacterAnimation.ITEM_USE_ANIMATION_NONE)
                            {
                                ActAnimationRequest(outcome.animation);
                                continueOp = true;
                            }
                            else
                            {
                                /**/
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

            return continueOp;
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
        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, in CharacterType oldval, in CharacterType newval)
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

        private void ChangedGameStatus(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
        {
            if(oldval != newval)
            {
                switch(newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        _rigidbody.simulated = true;
                        break;
                }

                switch(oldval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        _rigidbody.simulated = false;
                        break;
                }
            }
        }

        #endregion


    }

}
