using UnityEngine;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using System;
using Gob3AQ.Brain.ItemsInteraction;
using System.Collections;
using Gob3AQ.ResourceAtlas;

namespace Gob3AQ.GameEventMaster
{
    
    public class GameEventMasterClass : MonoBehaviour
    {
        private static GameEventMasterClass _singleton;

        /// <summary>
        /// Events which were modified on last cycle
        /// </summary>
        private HashSet<GameEvent>[] _bufferedEvents;
        private HashSet<UnchainConditions> _removePendingHash;
        private HashSet<GameEvent> _removePendingKey;
        private int _actualUsedBufferedHashSetIndex;

        /* Ok, this is gross. Here goes something which could make you think in opposite directions <noob<->expert> */
        /* One of (maybe) multiple events necessary for one or multiple unchainers. So several keys could point to same unchainer(s) */
        /* It is responsability of designer to set correctly "ignoreif" events for events which could happen way further in game */
        private Dictionary<GameEvent, HashSet<UnchainConditions>> _pendingUnchainDict;


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

                        if ((newValue != prevValue) && (_singleton != null))
                        {
                            _singleton._bufferedEvents[_singleton._actualUsedBufferedHashSetIndex].Add(combiInfo.eventType);
                        }
                    }
                }

                VARMAP_GameEventMaster.SET_ARRAY_EVENTS_OCCURRED(events_out);
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

                _pendingUnchainDict = new Dictionary<GameEvent, HashSet<UnchainConditions>>(GameFixedConfig.MAX_PENDING_UNCHAINERS);
                _removePendingKey = new HashSet<GameEvent>(GameFixedConfig.MAX_PENDING_UNCHAINERS);
                _removePendingHash = new HashSet<UnchainConditions>(GameFixedConfig.MAX_UNCHAINER_CONDITIONS);

                _bufferedEvents = new HashSet<GameEvent>[2];
                _bufferedEvents[0] = new(GameFixedConfig.MAX_BUFFERED_EVENTS);
                _bufferedEvents[1] = new(GameFixedConfig.MAX_BUFFERED_EVENTS);

                _actualUsedBufferedHashSetIndex = 0;
            }
        }

        void Start()
        {
            if (_singleton != null)
            {
                Room room = VARMAP_GameEventMaster.GET_ACTUAL_ROOM();
                StartCoroutine(Loading_Task_Coroutine(room));
            }
        }

        /* This module is executed just after GameMaster (which will not interfere by services or variable with this module) */
        /* Therefore here in Update are processed changed events from last cycle (which is desirable scenario) */
        private void Update()
        {
            int lastCycleHashSetIndex;
            int nextCycleHashSetIndex;

            /* First of all, move selector to the other alternate dictionary, as invokes may trigger new events */
            lastCycleHashSetIndex = _actualUsedBufferedHashSetIndex;
            nextCycleHashSetIndex = (lastCycleHashSetIndex + 1) & 0x1;
            _actualUsedBufferedHashSetIndex = nextCycleHashSetIndex;

            /* Iterate in buffered changes to call invokes, based on dictionary which has been working until now */
            /* Whatever these invoke do, will work on the alternate dictionary (which is the official new for this cycle) */
            foreach (GameEvent gameEvent in _bufferedEvents[lastCycleHashSetIndex])
            {
                if (_pendingUnchainDict.TryGetValue(gameEvent, out HashSet<UnchainConditions> unchainers))
                {
                    /* Try unchain for related unchainers to this event */
                    foreach (UnchainConditions unchainer in unchainers)
                    {
                        ref readonly UnchainInfo unchainerInfo = ref ItemsInteractionsClass.GetUnchainInfo(unchainer);

                        /* If it is completed, add to remove list */
                        if(TryUnchainAction(in unchainerInfo))
                        {
                            _removePendingHash.Add(unchainer);
                        }
                    }

                    /* Remove outside previous foreach loop */
                    foreach(UnchainConditions unchainerToRemove in _removePendingHash)
                    {
                        foreach(KeyValuePair<GameEvent, HashSet<UnchainConditions>> keypair in _pendingUnchainDict)
                        {
                            keypair.Value.Remove(unchainerToRemove);

                            if(keypair.Value.Count == 0)
                            {
                                _removePendingKey.Add(keypair.Key);
                            }
                        }
                    }
                    _removePendingHash.Clear();

                    foreach(GameEvent gameEventToRemove in _removePendingKey)
                    {
                        _pendingUnchainDict.Remove(gameEventToRemove);
                    }
                    _removePendingKey.Clear();
                }
            }

            /* Clear old dictionary */
            _bufferedEvents[lastCycleHashSetIndex].Clear();
        }


        void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
            }
        }

        private IEnumerator Loading_Task_Coroutine(Room room)
        {
            bool completed = false;
            int unchainer_index = 0;


            while (!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameEventMaster.IS_MODULE_LOADED(GameModules.MODULE_ItemMaster, out completed);
            }

            completed = false;

            while(!completed)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                completed = Loading_Task_Cycle(room, unchainer_index);
                ++unchainer_index;
            }

            VARMAP_GameEventMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameEventMaster);
        }

        private bool Loading_Task_Cycle(Room room, int index)
        {
            bool ended;

            /* Retrieve all Unchainers */
            ReadOnlySpan<UnchainInfo> all_game_unchainers = ItemsInteractionsClass.GET_UNCHAINERS;
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            Span<GameEventCombi> ignoreIfCondition = stackalloc GameEventCombi[1];

            if(index < all_game_unchainers.Length)
            {
                UnchainConditions unchainer = (UnchainConditions)index;
                ref readonly UnchainInfo unchainer_info = ref all_game_unchainers[index];
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
                    if (unchainer_info.type != UnchainType.UNCHAIN_TYPE_EVENT)
                    {
                        if (!roomInfo.items[unchainer_info.targetItem])
                        {
                            pending = false;
                        }
                    }
                }

                /* Now check if all of its necessary events are complied to execute it */
                if (pending)
                {
                    bool occurred = TryUnchainAction(in unchainer_info);
    
                    if(!occurred)
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
                                _pendingUnchainDict[eventCombi.eventType] = new HashSet<UnchainConditions>(GameFixedConfig.MAX_UNCHAINER_CONDITIONS)
                                { unchainer };
                            }
                        }
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

        private bool TryUnchainAction(in UnchainInfo info)
        {
            ReadOnlySpan<GameEventCombi> neededEvents = info.NeededEvents;
            IsEventCombiOccurredService(neededEvents, out bool occurred);

            /* If occurred, execute it and don't add it to pending */
            if (occurred)
            {
                switch(info.type)
                {
                    case UnchainType.UNCHAIN_TYPE_EVENT:
                        CommitEventService(info.TargetEvents);
                        break;
                    default:
                        VARMAP_GameEventMaster.UNCHAIN_TO_ITEM(in info);
                        break;
                }
            }

            return occurred;
        }

    }
}