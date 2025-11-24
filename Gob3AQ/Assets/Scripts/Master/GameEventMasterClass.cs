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
                    VARMAP_GameEventMaster.SET_EVENTS_BEING_PROCESSED(true);
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
                VARMAP_GameEventMaster.SET_EVENTS_BEING_PROCESSED(true);
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
                VARMAP_GameEventMaster.SET_EVENTS_BEING_PROCESSED(true);
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
            bool prevProcessingEvents;

            processingEvents = _bufferedEvents.Count != 0;
            prevProcessingEvents = VARMAP_GameEventMaster.GET_SHADOW_EVENTS_BEING_PROCESSED();

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
                            _removePendingHash.Add(unchainer);
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

            if (prevProcessingEvents != processingEvents)
            {
                VARMAP_GameEventMaster.SET_EVENTS_BEING_PROCESSED(processingEvents);
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
                yield return ResourceAtlasClass.WaitForNextFrame;
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
                    if ((unchainer_info.type == UnchainType.UNCHAIN_TYPE_SET_SPRITE) ||
                        (unchainer_info.type == UnchainType.UNCHAIN_TYPE_SPAWN) ||
                        (unchainer_info.type == UnchainType.UNCHAIN_TYPE_DESPAWN)
                        )
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

            completed = false;

            while (!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameEventMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMenu, out completed);
            }

            foreach(UnchainConditions unchainer in _itemRelatedUnchainers)
            {
                Scene_Loading_Task_ItemUnchainers_Cycle(unchainer, room);
            }
            yield return ResourceAtlasClass.WaitForNextFrame;

            /* Now it's time to check all pending ones to execute */
            foreach (KeyValuePair<UnchainConditions, HashSet<GameEvent>> kvp in _reversePendingUnchainDict)
            {
                if (TryUnchainAction(in ItemsInteractionsClass.GetUnchainInfo(kvp.Key)))
                {
                    _removePendingHash.Add(kvp.Key);
                }
            }

            foreach(UnchainConditions unchainer in _removePendingHash)
            {
                RemoveUnchainerEventsFromPending(unchainer);
            }
            _removePendingHash.Clear();

            VARMAP_GameEventMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameEventMaster);
        }

        private void Scene_Loading_Task_ItemUnchainers_Cycle(UnchainConditions unchainer, Room room)
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ref readonly UnchainInfo unchainer_info = ref ItemsInteractionsClass.GetUnchainInfo(unchainer);

            if (roomInfo.items[unchainer_info.targetItem])
            {
                /* If it needs to spawn, make it invisible by the moment */
                if (unchainer_info.type == UnchainType.UNCHAIN_TYPE_SPAWN)
                {
                    Debug.Log("Pre-Disappear for posterior Spawn unchainer " + unchainer_info.targetItem);
                    VARMAP_GameEventMaster.UNCHAIN_TO_ITEM(in UnchainInfo.EMPTY, true);
                }

                AddUnchainerEventsToPending(unchainer, in unchainer_info);
            }
            else
            {
                RemoveUnchainerEventsFromPending(unchainer);
            }
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

            /* If occurred, execute it and don't add it to pending */
            if (occurred)
            {
                switch (info.type)
                {
                    case UnchainType.UNCHAIN_TYPE_EVENT:
                        Debug.Log("Unchaining event " + info.TargetEvents[0].eventType);
                        CommitEventService(info.TargetEvents);
                        break;
                    case UnchainType.UNCHAIN_TYPE_MEMENTO:
                        Debug.Log("Unchaining Memento " + info.targetMemento);
                        CommitMementoService(info.targetMemento);
                        break;
                    default:
                        VARMAP_GameEventMaster.UNCHAIN_TO_ITEM(in info, false);
                        break;
                }
            }

            return occurred;
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