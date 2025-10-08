using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.PlayerMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.ProgrammedPath;
using Gob3AQ.Waypoint.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


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
    public class PlayableCharScript : GameElement
    {
        /* Fields */
        [SerializeField]
        private CharacterType charType;

        public CharacterType CharType => charType;

        public Collider2D Collider => _collider;


        /* GameObject components */
        private GameObject _parent;
        private Transform _parentTransform;
        private SpriteRenderer _sprRenderer;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        /* Status */
        private PhysicalState physicalstate;
        private float actTimeout;

        private bool selected;
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
                if (isAvailable)
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
                    gameObject.SetActive(true);
                }
            }
        }


        #endregion



        private void Awake()
        {
            gameElementFamily = GameItemFamily.ITEM_FAMILY_TYPE_PLAYER;

            _parent = transform.parent.gameObject;
            _parentTransform = _parent.transform;
            _sprRenderer = _parent.GetComponent<SpriteRenderer>();
            _collider = _parent.GetComponent<Collider2D>();
            _rigidbody = _parent.GetComponent<Rigidbody2D>();

            _sprRenderer.enabled = false;

            wpsolutionlist = new List<WaypointClass>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
        }

        private void Start()
        {
            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            selected = false;
            bufferedData.pending = false;
            actTimeout = 0f;
            isAvailable = true;

            PlayerMasterClass.SetPlayerLoadPresent(CharType);

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
            VARMAP_PlayerMaster.REG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
            VARMAP_PlayerMaster.REG_GAMESTATUS(ChangedGameStatus);

            _parent.GetComponent<GameElementClickable>().SetOnClickAction(MouseEnterAction);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }

        private void Update()
        {
            /* Script will only be enabled in play mode */
            Execute_Play();
        }



        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
            VARMAP_PlayerMaster.UNREG_PLAYER_SELECTED(ChangedSelectedPlayerEvent);
            VARMAP_PlayerMaster.UNREG_GAMESTATUS(ChangedGameStatus);
        }

        private void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            LevelElemInfo info = new((int)charType, gameElementFamily, actualWaypoint, enter & isAvailable);
            VARMAP_PlayerMaster.GAME_ELEMENT_OVER(in info);
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
            VARMAP_PlayerMaster.GET_NEAREST_WP(_parentTransform.position, float.MaxValue, out WaypointClass nearestWp);

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

                _parentTransform.position = actualWaypoint.transform.position;
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

            if (!continueLoop)
            {
                gameObject.SetActive(false);
            }

            isAvailable = (physicalstate == PhysicalState.PHYSICAL_STATE_STANDING) && (!bufferedData.pending);
        }

        private bool Execute_Walk()
        {
            bool continueOp;
            List<WaypointClass> wplist = actualProgrammedPath.originalSolution.waypointTrace;
            int seg_index = actualProgrammedPath.crossedWaypointIndex;
            WaypointClass target_wp = wplist[seg_index];
            Vector3 target_pos = target_wp.transform.position;
            Vector2 deltaPos = target_pos - _parentTransform.position;

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
                    _parentTransform.position = target_pos;
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
                _parentTransform.position = actualWaypoint.transform.position;
            }
            else
            {
                target_wp = wplist[actualProgrammedPath.crossedWaypointIndex];
                delta = (target_wp.transform.position - _parentTransform.position).normalized;
            }

            _rigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * delta;
            
            actualWaypoint = target_wp;
        }

        private bool StartBufferedInteraction()
        {
            bool continueOp = false;

            /* Generate stack array of 2 talkers, player and dst */
            Span<GameItem> talkers = stackalloc GameItem[2];

            /* Now interact if buffered */
            if (bufferedData.pending && (bufferedData.usage.type == InteractionType.PLAYER_WITH_ITEM))
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(bufferedData.usage.itemDest);

                switch(itemInfo.family)
                {
                    case GameItemFamily.ITEM_FAMILY_TYPE_DOOR:
                        VARMAP_PlayerMaster.CROSS_DOOR(charType, bufferedData.usage.destListIndex);
                        break;
                    default:
                        VARMAP_PlayerMaster.IS_ITEM_AVAILABLE(bufferedData.usage.itemDest, out bool validTransaction);

                        if (validTransaction)
                        {
                            /* Use Item is also Take Item */
                            VARMAP_PlayerMaster.USE_ITEM(in bufferedData.usage, out InteractionUsageOutcome outcome);

                            if (outcome.dialogType != DialogType.DIALOG_NONE)
                            {
                                /* Default talkers are own player and itemDest */
                                talkers[0] = itemID;
                                talkers[1] = bufferedData.usage.itemDest;

                                VARMAP_PlayerMaster.ENABLE_DIALOGUE(true, talkers, outcome.dialogType, outcome.dialogPhrase);
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
                }
            }

            /* Clear */
            bufferedData.pending = false;

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
            _ = eventType;

            if (oldval != newval)
            {
                switch(newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        _rigidbody.simulated = true;

                        /* Activate script only if there is pending action */
                        gameObject.SetActive(physicalstate != PhysicalState.PHYSICAL_STATE_STANDING);
                        break;
                }

                switch(oldval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        _rigidbody.simulated = false;
                        gameObject.SetActive(false);
                        break;
                }
            }
        }

        #endregion


    }

}
