using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.GameEventMaster
{

    public class GameEventMasterClass : MonoBehaviour
    {
        private struct ActionOrder
        {
            public GameAction action;
            public Action callback;
            public bool performedAndWaiting;

            public ActionOrder(GameAction action, Action callback)
            {
                this.action = action;
                this.callback = callback;
                performedAndWaiting = false;
            }
        }

        private static GameEventMasterClass _singleton;

        /// <summary>
        /// Events which were modified on last cycle
        /// </summary>
        private HashSet<GameEvent> _bufferedEvents;
        /// <summary>
        /// Entries to remove after processing (auxiliar)
        /// </summary>
        private HashSet<UnchainConditions> _removePendingHash;
        /// <summary>
        /// Keys to remove after processing (auxiliar)
        /// </summary>
        private HashSet<GameEvent> _removePendingKey;
        /// <summary>
        /// Unchain conditions which need to be reviewed on each Scene load
        /// </summary>
        private HashSet<UnchainConditions> _itemRelatedUnchainers;
        /// <summary>
        /// Compound conditions of whole game which are still pending and items of this scene
        /// </summary>
        private Dictionary<GameEvent, HashSet<UnchainConditions>> _pendingUnchainDict;
        /// <summary>
        /// Reverse dictionary of pending unchainers to their needed events
        /// </summary>
        private Dictionary<UnchainConditions, HashSet<GameEvent>> _reversePendingUnchainDict;
        /// <summary>
        /// Timestamp for periodic background tasks execution
        /// </summary>
        private ulong _bckgActionsTimestamp;
        /// <summary>
        /// Iterator for every period to be executed, to avoid execute all on the same frame when there are more than one
        /// </summary>
        private int _bckgActionsIndex;
        private List<ActionOrder> _pendingActions;
        private bool _actionEndedFlag;


        public static void IsMementoUnlockedService(Memento memento, out bool occurred, out bool unwatched)
        {
            GetArrayIndexAndPos((int)memento, out int arraypos, out int elembit);
            ref readonly MultiBitFieldStruct mbfs = ref VARMAP_GameEventMaster.GET_ELEM_UNLOCKED_MEMENTO(arraypos);
            occurred = mbfs.GetIndividualBool(elembit);

            ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(memento);
            GetArrayIndexAndPos((int)memInfo.parent, out arraypos, out elembit);

            mbfs = ref VARMAP_GameEventMaster.GET_ELEM_UNWATCHED_PARENT_MEMENTO(arraypos);
            unwatched = mbfs.GetIndividualBool(elembit);
        }


        public static void IsEventCombiOccurredService(ReadOnlySpan<GameEventCombi> combi, out bool occurred)
        {
            occurred = true;

            if (combi[0].eventType != GameEvent.EVENT_NONE)
            {
                for (int i = 0; i < combi.Length && occurred; i++)
                {
                    IsEventOccurred(combi[i].eventType, out bool evOccurred);
                    evOccurred ^= combi[i].eventNOT; // If condition is NOT, invert result
                    occurred &= evOccurred;
                }
            }
        }

        public static void CommitEventService(ReadOnlySpan<GameEventCombi> combi)
        {
            if (_singleton != null)
            {
                ReadOnlySpan<MultiBitFieldStruct> events_in = VARMAP_GameEventMaster.GET_SHADOW_ARRAY_EVENTS_OCCURRED();
                Span<MultiBitFieldStruct> events_out = stackalloc MultiBitFieldStruct[events_in.Length];
                events_in.CopyTo(events_out);
                bool somethingChanged = false;

                for (int i = 0; i < combi.Length; ++i)
                {
                    ref readonly GameEventCombi combiInfo = ref combi[i];

                    if (combiInfo.eventType != GameEvent.EVENT_NONE)
                    {
                        int evIndex = (int)combiInfo.eventType;

                        /* Set in VARMAP */
                        GetArrayIndexAndPos(evIndex, out int arraypos, out int elembit);

                        ref MultiBitFieldStruct mbfs = ref events_out[arraypos];

                        bool prevValue = mbfs.GetIndividualBool(elembit);
                        bool newValue = !combiInfo.eventNOT;

                        mbfs.SetIndividualBool(elembit, newValue);

                        if (newValue != prevValue)
                        {
                            _singleton._bufferedEvents.Add(combiInfo.eventType);
                            somethingChanged = true;
                        }
                    }
                }

                if (somethingChanged)
                {
                    VARMAP_GameEventMaster.SET_ARRAY_EVENTS_OCCURRED(events_out);
                    BusyState busyState = VARMAP_GameEventMaster.GET_SHADOW_BUSY_STATE();
                    busyState = UpdateBusyState(busyState, true, false);
                    VARMAP_GameEventMaster.SET_BUSY_STATE(busyState);
                }
            }
        }

        public static void MementoParentWatchedService(MementoParent mementoParent)
        {
            GetArrayIndexAndPos((int)mementoParent, out int arraypos, out int elembit);
            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO(arraypos);
            bool prevValue = mbfs.GetIndividualBool(elembit);
            mbfs.SetIndividualBool(elembit, false);

            if (prevValue)
            {
                VARMAP_GameEventMaster.SET_ELEM_UNWATCHED_PARENT_MEMENTO(arraypos, in mbfs);
                BusyState busyState = VARMAP_GameEventMaster.GET_SHADOW_BUSY_STATE();
                busyState = UpdateBusyState(busyState, true, false);
                VARMAP_GameEventMaster.SET_BUSY_STATE(busyState);
            }
        }


        private static void CommitMementoService(Memento memento)
        {
            GetArrayIndexAndPos((int)memento, out int arraypos, out int elembit);
            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_SHADOW_ELEM_UNLOCKED_MEMENTO(arraypos);
            bool prevValue = mbfs.GetIndividualBool(elembit);
            mbfs.SetIndividualBool(elembit, true);

            if (!prevValue)
            {
                VARMAP_GameEventMaster.SET_ELEM_UNLOCKED_MEMENTO(arraypos, in mbfs);

                ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(memento);
                GetArrayIndexAndPos((int)memInfo.parent, out arraypos, out elembit);

                mbfs = VARMAP_GameEventMaster.GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO(arraypos);
                mbfs.SetIndividualBool(elembit, true);

                VARMAP_GameEventMaster.SET_ELEM_UNWATCHED_PARENT_MEMENTO(arraypos, in mbfs);
                VARMAP_GameEventMaster.COMMIT_MEMENTO_NOTIF(memento);

                BusyState busyState = VARMAP_GameEventMaster.GET_SHADOW_BUSY_STATE();
                busyState = UpdateBusyState(busyState, true, false);
                VARMAP_GameEventMaster.SET_BUSY_STATE(busyState);
            }
        }

        public static void PerformActionService(ReadOnlySpan<GameAction> actions, Action callback)
        {
            if (_singleton != null)
            {
                for(int i=0; i < actions.Length; ++i)
                {
                    if (actions[i] != GameAction.ACTION_NONE)
                    {
                        Action usedCallback;

                        /* Only execute callback on last action, to avoid multiple calls when there are more than one action */
                        if (i < (actions.Length - 1))
                        {
                            usedCallback = null; 
                        }
                        else
                        {
                            usedCallback = callback;
                        }

                        _singleton._pendingActions.Add(new ActionOrder(actions[i], usedCallback));
                    }
                }

                BusyState busyState = VARMAP_GameEventMaster.GET_SHADOW_BUSY_STATE();
                busyState = UpdateBusyState(busyState, true, false);
                VARMAP_GameEventMaster.SET_BUSY_STATE(busyState);
            }
        }

        public static void NotifyEndedActionService()
        {
            if(_singleton != null)
            {
                _singleton._actionEndedFlag = true;
            }
        }


        /// <summary>
        /// Based on 64 bit bitfield, gets decomposition of array element and bit position
        /// </summary>
        /// <param name="index">total index [0, ARRAY_TOTAL*64)</param>
        /// <param name="arrayIndex">index/64 operation</param>
        /// <param name="bitPos">index % 64 operation</param>
        private static void GetArrayIndexAndPos(int index, out int arrayIndex, out int bitPos)
        {
            arrayIndex = index >> 6;
            bitPos = index & 0x3F;
        }

        private static void IsEventOccurred(GameEvent ev, out bool occurred)
        {
            int evIndex = (int)ev;

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            ref readonly MultiBitFieldStruct mbfs = ref VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            occurred = mbfs.GetIndividualBool(itembit);
        }

        private static BusyState UpdateBusyState(BusyState actualState, bool newEvents, bool newActions)
        {
            BusyState retState = actualState;

            retState |= newEvents ? BusyState.GAME_PROCESSING_EVENTS : BusyState.GAME_NOT_BUSY;
            retState |= newActions ? BusyState.GAME_PROCESSING_ACTIONS : BusyState.GAME_NOT_BUSY;

            return retState;
        }

        void Awake()
        {
            if (_singleton != null)
            {
                Destroy(this);
                return;
            }
            else
            {
                _singleton = this;

                _pendingUnchainDict = new(GameFixedConfig.MAX_PENDING_UNCHAINERS);
                _reversePendingUnchainDict = new(GameFixedConfig.MAX_PENDING_UNCHAINERS);

                _removePendingKey = new HashSet<GameEvent>(GameFixedConfig.MAX_PENDING_UNCHAINERS);
                _removePendingHash = new HashSet<UnchainConditions>(GameFixedConfig.MAX_PENDING_UNCHAINERS);

                _bufferedEvents = new(GameFixedConfig.MAX_BUFFERED_EVENTS);

                _itemRelatedUnchainers = new(GameFixedConfig.MAX_PENDING_UNCHAINERS);
                _pendingActions = new(GameFixedConfig.MAX_BUFFERED_EVENTS);

                _actionEndedFlag = false;
            }
        }

        void Start()
        {
            if (_singleton != null)
            {
                VARMAP_GameEventMaster.REG_GAMESTATUS(_GameStatusChanged);
                Room room = VARMAP_GameEventMaster.GET_ACTUAL_ROOM();
                StartCoroutine(Initial_Loading_Task_Coroutine());
            }
        }

        /* This module is executed just after GameMaster (which will not interfere by services or variable with this module) */
        /* Therefore here in Update are processed changed events from last cycle (which is desirable scenario) */
        private void Update()
        {
            bool processingEvents;
            bool processingActions;
            BusyState prevBusyState;
            BusyState actualBusyState;

            ExecuteBackgroundActions();

            processingEvents = _bufferedEvents.Count != 0;

            prevBusyState = VARMAP_GameEventMaster.GET_SHADOW_BUSY_STATE();
            actualBusyState = BusyState.GAME_NOT_BUSY;

            ProcessPendingEvents(ref processingEvents);
            processingActions = ProcessPendingActions();

            actualBusyState = UpdateBusyState(actualBusyState, processingEvents, processingActions);

            if (prevBusyState != actualBusyState)
            {
                VARMAP_GameEventMaster.SET_BUSY_STATE(actualBusyState);
            }
        }


        void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
                VARMAP_GameEventMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private IEnumerator Initial_Loading_Task_Coroutine()
        {
            bool completed = false;
            int unchainer_index = 0;


            while (!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameEventMaster.IS_MODULE_LOADED(GameModules.MODULE_ItemMaster, out completed);
            }

            completed = false;

            while (!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameEventMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMenu, out completed);
            }

            completed = false;

            while (!completed)
            {
                completed = Initial_Loading_Task_Cycle(unchainer_index);
                ++unchainer_index;
            }

            VARMAP_GameEventMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameEventMaster);
        }

        private bool Initial_Loading_Task_Cycle(int index)
        {
            bool ended;

            /* Retrieve all Unchainers */
            Span<GameEventCombi> ignoreIfCondition = stackalloc GameEventCombi[1];

            if (index < (int)UnchainConditions.UNCHAIN_TOTAL)
            {
                UnchainConditions unchainer = (UnchainConditions)index;
                ref readonly UnchainInfo unchainer_info = ref ItemsInteractionsClass.GetUnchainInfo(unchainer);
                bool pending;

                /* Check if ignoreif condition comply (NONE means never ignore) */
                if (unchainer_info.ignoreif.eventType != GameEvent.EVENT_NONE)
                {
                    ignoreIfCondition[0] = unchainer_info.ignoreif;
                    IsEventCombiOccurredService(ignoreIfCondition, out bool occurred);

                    pending = !occurred;
                }
                else
                {
                    pending = true;
                }

                if (pending)
                {
                    /* In case this points to an item, check if item is involved in room */
                    if (!unchainer_info.isOneShot)
                    {
                        /* Store for Scene Load (not in initial load) */
                        _itemRelatedUnchainers.Add(unchainer);
                    }
                    else
                    {
                        AddUnchainerEventsToPending(unchainer, in unchainer_info);
                    }
                }

                ended = false;
            }
            else
            {
                ended = true;
            }

            return ended;
        }

        

        private IEnumerator Scene_Loading_Task_Coroutine(Room room)
        {
            bool completed = false;

            while (!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameEventMaster.IS_MODULE_LOADED(GameModules.MODULE_ItemMaster, out completed);
            }

            HashSet<UnchainConditions> _itemRelatedUnchainersToRemove = new(GameFixedConfig.MAX_PENDING_UNCHAINERS);

            foreach(UnchainConditions unchainer in _itemRelatedUnchainers)
            {
                Scene_Loading_Task_ItemUnchainers_Cycle(unchainer, room, _itemRelatedUnchainersToRemove);
            }
            yield return ResourceAtlasClass.WaitForNextFrame;

            foreach(UnchainConditions unchainerToRemove in _itemRelatedUnchainersToRemove)
            {
                _itemRelatedUnchainers.Remove(unchainerToRemove);
            }

            /* Now it's time to check all pending ones to execute */
            /* Don't insert yields between this for and remove operation, as Update() may interfere */
            foreach (KeyValuePair<UnchainConditions, HashSet<GameEvent>> kvp in _reversePendingUnchainDict)
            {
                UnchainInfo unchainer_info = ItemsInteractionsClass.GetUnchainInfo(kvp.Key);

                if (TryUnchainAction(in unchainer_info))
                {
                    if (!unchainer_info.repeat)
                    {
                        _removePendingHash.Add(kvp.Key);
                    }
                }
            }

            foreach(UnchainConditions unchainer in _removePendingHash)
            {
                RemoveUnchainerEventsFromPending(unchainer);
            }
            _removePendingHash.Clear();

            _bckgActionsIndex = 0;

            VARMAP_GameEventMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameEventMaster);
        }

        private void Scene_Loading_Task_ItemUnchainers_Cycle(UnchainConditions unchainer, Room room, HashSet<UnchainConditions> itemRemoveSet)
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ref readonly UnchainInfo unchainer_info = ref ItemsInteractionsClass.GetUnchainInfo(unchainer);
            bool pending;

            /* Retrieve all Unchainers */
            Span<GameEventCombi> ignoreIfCondition = stackalloc GameEventCombi[1];

            /* Check if ignoreif condition comply (NONE means never ignore) */
            if (unchainer_info.ignoreif.eventType != GameEvent.EVENT_NONE)
            {
                ignoreIfCondition[0] = unchainer_info.ignoreif;
                IsEventCombiOccurredService(ignoreIfCondition, out bool occurred);

                pending = !occurred;
            }
            else
            {
                pending = true;
            }

            if(!pending)
            {
                itemRemoveSet.Add(unchainer);
            }

            bool hasItemsInRoom = false;

            if (pending)
            {
                foreach (GameAction action in unchainer_info.UnchainActions)
                {
                    ref readonly ActionInfo actionInfo = ref ItemsInteractionsClass.GetActionInfo(action);
                    if ((actionInfo.targetItem != GameItem.ITEM_NONE) && roomInfo.items[actionInfo.targetItem])
                    {
                        hasItemsInRoom = true;

                        /* If it needs to spawn, make it invisible by the moment */
                        if (actionInfo.type == ActionType.ACTION_TYPE_SPAWN)
                        {
                            ActionInfo pre_despawn_action = ActionInfo.CreateDespawnAction(actionInfo.targetItem);
                            VARMAP_GameEventMaster.ACTION_TO_ITEM(in pre_despawn_action);
                        }
                    }
                }
            }

            if (hasItemsInRoom && pending)
            {
                AddUnchainerEventsToPending(unchainer, in unchainer_info);
            }
            else
            {
                /* It may be still pending from last scene */
                RemoveUnchainerEventsFromPending(unchainer);
            }
        }

        private void ProcessPendingEvents(ref bool processingEvents)
        {
            /* Iterate in buffered changes to call invokes, based on dictionary which has been working until now */
            /* Whatever these invoke do, will work on the alternate dictionary (which is the official new for this cycle) */
            /* >Only one per cycle< */
            if (processingEvents)
            {
                GameEvent eventWithChanges = GameEvent.EVENT_NONE;
                foreach (GameEvent gameEvent in _bufferedEvents)
                {
                    eventWithChanges = gameEvent;
                    break;
                }
                _bufferedEvents.Remove(eventWithChanges);

                if (_pendingUnchainDict.TryGetValue(eventWithChanges, out HashSet<UnchainConditions> unchainers))
                {
                    /* Try unchain for related unchainers to this event */
                    foreach (UnchainConditions unchainer in unchainers)
                    {
                        ref readonly UnchainInfo unchainerInfo = ref ItemsInteractionsClass.GetUnchainInfo(unchainer);

                        /* If it is completed, add to remove list */
                        if (TryUnchainAction(in unchainerInfo))
                        {
                            if (!unchainerInfo.repeat)
                            {
                                _removePendingHash.Add(unchainer);
                            }
                        }
                    }

                    /* Remove outside previous foreach loop */
                    foreach (UnchainConditions unchainerToRemove in _removePendingHash)
                    {
                        RemoveUnchainerEventsFromPending(unchainerToRemove);
                    }
                    _removePendingHash.Clear();
                }
            }
            else
            {
                /* Master pending events (when there is no pending work) */
                Span<GameEventCombi> stackCheck = stackalloc GameEventCombi[2];

                stackCheck[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_MOMENT_DAY, false);
                IsEventCombiOccurredService(stackCheck[..1], out bool aftermath_change_day);
                stackCheck[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_ROOM, false);
                IsEventCombiOccurredService(stackCheck[..1], out bool aftermath_change_room);

                if ((aftermath_change_day || aftermath_change_room) && (VARMAP_GameEventMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY))
                {
                    stackCheck[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_MOMENT_DAY, true);
                    stackCheck[1] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_ROOM, true);
                    CommitEventService(stackCheck);

                    processingEvents = true;
                }
            }
        }

        private bool ProcessPendingActions()
        {
            bool stop = false;
            bool processingActions = false;

            while ((_pendingActions.Count > 0) && (!stop))
            {
                ActionOrder actionOrder = _pendingActions[0];
                bool endedAction = false;
                processingActions = true;

                if (!actionOrder.performedAndWaiting)
                {
                    _actionEndedFlag = false;
                    stop = ExecuteAction(actionOrder.action);
                    endedAction = !stop;

                    actionOrder.performedAndWaiting = true;
                    _pendingActions[0] = actionOrder;
                }
                else
                {
                    endedAction = _actionEndedFlag;
                    stop = !endedAction;
                }

                /* If action was not required to be waited or is an action which does not need wait */
                if (endedAction)
                {
                    Debug.Log("Ended action: " + actionOrder.action);
                    _pendingActions.RemoveAt(0);

                    /* If there is a callback, execute it and stop processing more actions until next cycle, to avoid multiple calls in same frame */
                    actionOrder.callback?.Invoke();
                }
            }

            return processingActions;
        }

        private void AddUnchainerEventsToPending(UnchainConditions unchainer, in UnchainInfo unchainer_info)
        {
            if (!_reversePendingUnchainDict.ContainsKey(unchainer))
            {
                foreach (GameEventCombi eventCombi in unchainer_info.NeededEvents)
                {
                    /* If key is already there, add to HashSet (won't duplicate) */
                    if (_pendingUnchainDict.TryGetValue(eventCombi.eventType, out HashSet<UnchainConditions> hash))
                    {
                        hash.Add(unchainer);
                    }
                    else
                    {
                        _pendingUnchainDict[eventCombi.eventType] = new(GameFixedConfig.MAX_PENDING_UNCHAINERS)
                            { unchainer };
                    }

                    if (_reversePendingUnchainDict.TryGetValue(unchainer, out HashSet<GameEvent> reverseHash))
                    {
                        reverseHash.Add(eventCombi.eventType);
                    }
                    else
                    {
                        _reversePendingUnchainDict[unchainer] = new(GameFixedConfig.MAX_PENDING_UNCHAINERS)
                            { eventCombi.eventType};
                    }
                }
            }
        }

        private void RemoveUnchainerEventsFromPending(UnchainConditions unchainer)
        {
            /* Remove from pending, as item is not in this room */
            if (_reversePendingUnchainDict.TryGetValue(unchainer, out HashSet<GameEvent> reverseHash))
            {
                /* Remove outside previous foreach loop */
                foreach (GameEvent unchainerNeededEvent in reverseHash)
                {
                    _pendingUnchainDict[unchainerNeededEvent].Remove(unchainer);
                    if (_pendingUnchainDict[unchainerNeededEvent].Count == 0)
                    {
                        _removePendingKey.Add(unchainerNeededEvent);
                    }
                }
                _reversePendingUnchainDict.Remove(unchainer);

                foreach (GameEvent gameEventToRemove in _removePendingKey)
                {
                    _pendingUnchainDict.Remove(gameEventToRemove);
                }
                _removePendingKey.Clear();
            }
        }

        private bool TryUnchainAction(in UnchainInfo info)
        {
            ReadOnlySpan<GameEventCombi> neededEvents = info.NeededEvents;
            IsEventCombiOccurredService(neededEvents, out bool occurred);
            occurred &= IsMomentValid(info.momentType);

            /* If occurred, execute it */
            if (occurred)
            {
                PerformActionService(info.UnchainActions, null);
            }

            return occurred;
        }

        private bool ExecuteAction(GameAction action)
        {
            bool mustWait = false;

            if (action != GameAction.ACTION_NONE)
            {
                ref readonly ActionInfo info = ref ItemsInteractionsClass.GetActionInfo(action);
                bool error;

                ActionType actionTypeOverride = info.type;

                /* Downgrade dialog to background dialog when it has been called from item menu */
                if((actionTypeOverride == ActionType.ACTION_TYPE_START_DIALOGUE) && (VARMAP_GameEventMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_ITEM_MENU))
                {
                    actionTypeOverride = ActionType.ACTION_TYPE_START_DIALOGUE_BCKG;
                }

                switch (actionTypeOverride)
                {
                    case ActionType.ACTION_TYPE_EVENT:
                        CommitEventService(info.TargetEvents);
                        break;
                    case ActionType.ACTION_TYPE_MEMENTO:
                        CommitMementoService(info.targetMemento);
                        break;
                    case ActionType.ACTION_TYPE_CHANGE_MOMENT_DAY:
                        VARMAP_GameEventMaster.CHANGE_DAY_MOMENT(info.targetMomentOfDay);
                        break;
                    case ActionType.ACTION_TYPE_DECISION:
                        VARMAP_GameEventMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_DECISION, out error);
                        if (!error)
                        {
                            VARMAP_GameEventMaster.SHOW_DECISION(info.targetDecision);
                        }
                        break;
                    case ActionType.ACTION_TYPE_START_DIALOGUE:
                        VARMAP_GameEventMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_DIALOG, out error);
                        if (!error)
                        {
                            mustWait = info.waitForEnd;
                            VARMAP_GameEventMaster.SHOW_DIALOGUE(info.targetDialog, info.targetPhrase, false);
                        }
                        break;
                    case ActionType.ACTION_TYPE_START_DIALOGUE_BCKG:
                        mustWait = info.waitForEnd;
                        VARMAP_GameEventMaster.SHOW_DIALOGUE(info.targetDialog, info.targetPhrase, true);
                        break;
                    case ActionType.ACTION_TYPE_START_ANIMATION:
                        mustWait = info.waitForEnd;
                        break;
                    default:
                        VARMAP_GameEventMaster.ACTION_TO_ITEM(in info);
                        break;
                }
            }

            return mustWait;
        }

        private void ExecuteBackgroundActions()
        {
            ulong elapsedTime = VARMAP_GameEventMaster.GET_ELAPSED_TIME_MS();
            VARMAP_GameEventMaster.IS_DIALOG_ACTIVE(out bool dialogActive);

            /* Reset timestamp when not in play mode or there is an active background dialog */
            if ((VARMAP_GameEventMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY) && (!dialogActive))
            {
                if (elapsedTime - _bckgActionsTimestamp >= GameFixedConfig.BACKGROUND_ITEM_ACTIONS_MS)
                {
                    MomentType currentMoment = VARMAP_GameEventMaster.GET_DAY_MOMENT();
                    ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(VARMAP_GameEventMaster.GET_ACTUAL_ROOM());

                    if (roomInfo.ActionConditions.Length > 0)
                    {
                        ActionConditions action = roomInfo.ActionConditions[_bckgActionsIndex];
                        _bckgActionsIndex = (_bckgActionsIndex + 1) % roomInfo.ActionConditions.Length;

                        ref readonly ActionConditionsInfo actionInfo = ref ItemsInteractionsClass.GetActionConditionsInfo(action);
                        bool valid = actionInfo.actionCondType == ItemInteractionType.INTERACTION_AUTO_6s;
                        valid &= IsMomentValid(actionInfo.momentType);

                        IsEventCombiOccurredService(actionInfo.NeededEvents, out bool occurred);
                        valid &= occurred;

                        if (valid)
                        {
                            PerformActionService(actionInfo.UnchainActions, null);
                        }
                    }

                    _bckgActionsTimestamp = elapsedTime;
                }
            }
            else
            {
                _bckgActionsTimestamp = elapsedTime;
            }
        }

        private bool IsMomentValid(MomentType momentType)
        {
            bool valid;
            MomentType currentMoment = VARMAP_GameEventMaster.GET_DAY_MOMENT();
            if (momentType == MomentType.MOMENT_ANY)
            {
                valid = true;
            }
            else
            {
                valid = momentType == currentMoment;
            }
            return valid;
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        StartCoroutine(Scene_Loading_Task_Coroutine(VARMAP_GameEventMaster.GET_ACTUAL_ROOM()));
                        break;

                    default:
                        break;
                }
            }
        }
    }
}