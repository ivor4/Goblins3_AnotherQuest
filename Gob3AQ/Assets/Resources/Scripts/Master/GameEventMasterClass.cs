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
        private static GameEventMasterClass _singleton;
        private static EVENT_SUBSCRIPTION_CALL_DELEGATE[] _event_subscription;

        /// <summary>
        /// Will be used for subscriptions only
        /// </summary>
        private static Queue<int> _bufferedEvents;


        public static void IsEventOccurredService(GameEvent ev, out bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 1 event, which is placed at the beginning */

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            ref readonly MultiBitFieldStruct mbfs = ref VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            occurred = mbfs.GetIndividualBool(itembit);
        }

        public static void CommitEventService(GameEvent ev, bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 1 event, which is placed at the beginning */

            /* Set in VARMAP */
            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_SHADOW_ELEM_EVENTS_OCCURRED(arraypos);

            mbfs.SetIndividualBool(itembit, occurred);

            VARMAP_GameEventMaster.SET_ELEM_EVENTS_OCCURRED(arraypos, mbfs);

            if (!_bufferedEvents.Contains(evIndex))
            {
                _bufferedEvents.Enqueue(evIndex);
            }
        }

        public static void ItemObtainPickableEventService(GamePickableItem item)
        {
            int evIndex = (int)item;

            /* Set in VARMAP */
            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_SHADOW_ELEM_EVENTS_OCCURRED(arraypos);

            mbfs.SetIndividualBool(itembit, true);

            VARMAP_GameEventMaster.SET_ELEM_EVENTS_OCCURRED(arraypos, mbfs);

            if (!_bufferedEvents.Contains(evIndex))
            {
                _bufferedEvents.Enqueue(evIndex);
            }
        }


        public static void IsItemTakenFromSceneService(GamePickableItem item, out bool taken)
        {
            int evIndex = (int)item;
            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);
            ref readonly MultiBitFieldStruct mbfs = ref VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);
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

        void Start()
        {
            VARMAP_GameEventMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameEventMaster);
        }

        void Update()
        {
            Game_Status gstatus = VARMAP_GameEventMaster.GET_GAMESTATUS();
            
            switch(gstatus)
            {
                /* In this way, triggering of events would wait for next cycle in order not to overload previous one */
                case Game_Status.GAME_STATUS_PLAY:
                    while(_bufferedEvents.Count > 0)
                    {
                        /* Get first event */
                        int evIndex = _bufferedEvents.Dequeue();
                        /* Get from VARMAP */
                        GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);
                        ref readonly MultiBitFieldStruct mbfs = ref VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);
                        /* Get individual bit */
                        bool active = mbfs.GetIndividualBool(itembit);
                        /* Invoke subscribers of this event */
                        _event_subscription[evIndex]?.Invoke(active);
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