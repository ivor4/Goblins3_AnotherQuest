using UnityEngine;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;
using System;

namespace Gob3AQ.GameEventMaster
{
    
    public class GameEventMasterClass : MonoBehaviour
    {
        private readonly struct BufferedEvent
        {
            public readonly int evIndex;
            public readonly bool active;

            public BufferedEvent(int evIndex, bool active)
            {
                this.evIndex = evIndex;
                this.active = active;
            }
        }

        private static GameEventMasterClass _singleton;
        private static EVENT_SUBSCRIPTION_CALL_DELEGATE[] _event_subscription;
        private static GrowingStaticStackArray<BufferedEvent> _bufferedEvents;

        public static void IsEventOccurredService(GameEvent ev, out bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 1 event, which is placed at the beginning */

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            occurred = mbfs.GetIndividualBool(itembit);
        }

        public static void CommitEventService(GameEvent ev, bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 1 event, which is placed at the beginning */

            BufferedEvent newEv = new BufferedEvent(evIndex, occurred);

            _bufferedEvents.Add(newEv);
        }

        public static void ItemObtainPickableEventService(GamePickableItem item)
        {
            int evIndex = (int)item;

            BufferedEvent newEv = new(evIndex, true);

            _bufferedEvents.Add(newEv);
        }


        public static void IsItemTakenFromSceneService(GamePickableItem item, out bool taken)
        {
            int evIndex = (int)item;
            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);
            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);
            taken = mbfs.GetIndividualBool(itembit);
        }


        public static void EventSubscriptionService(GameEvent gevent, EVENT_SUBSCRIPTION_CALL_DELEGATE callable, bool add)
        {
            int evIndex = (int)gevent;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 1 event, which is placed at the beginning */

            if (add)
            {
                _event_subscription[evIndex] += callable;
            }
            else
            {
                _event_subscription[evIndex] -= callable;
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

        void Awake()
        {
            if (_singleton)
            {
                Destroy(this);
                return;
            }
            else
            {
                _singleton = this;
                _event_subscription = new EVENT_SUBSCRIPTION_CALL_DELEGATE[(int)GamePickableItem.ITEM_PICK_TOTAL + (int)GameEvent.EVENT_TOTAL];
                _bufferedEvents = new(GameFixedConfig.MAX_BUFFERED_EVENTS);
            }
        }

        void Update()
        {
            Game_Status gstatus = VARMAP_GameEventMaster.GET_GAMESTATUS();
            
            switch(gstatus)
            {
                /* In this way, triggering of events would wait for next cycle in order not to overload previous one */
                case Game_Status.GAME_STATUS_PLAY:
                    BufferedEvent be;
                    int bufferedEventsCount = _bufferedEvents.Count;

                    /* Declare a copy in stack - Some events could trigger other events. That explains why copying */
                    Span<BufferedEvent> thisCycleEventsSpan = stackalloc BufferedEvent[bufferedEventsCount];
                    _bufferedEvents.GetReadOnlySpan.Slice(0, bufferedEventsCount).CopyTo(thisCycleEventsSpan);

                    /* Officially set to 0, to be able to buffer even here from this point for next cycle */
                    _bufferedEvents.ResetCount();

                    for(int i=0;i<bufferedEventsCount;i++)
                    {
                        be = thisCycleEventsSpan[i];

                        /* Set in VARMAP */
                        GetArrayIndexAndPos(be.evIndex, out int arraypos, out int itembit);

                        MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

                        mbfs.SetIndividualBool(itembit, be.active);

                        VARMAP_GameEventMaster.SET_ELEM_EVENTS_OCCURRED(arraypos, mbfs);

                        /* Invoke subscribers of this event */
                        _event_subscription[be.evIndex]?.Invoke(be.active);
                    }
                    break;

                default:
                    break;
            }
        }


        void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}