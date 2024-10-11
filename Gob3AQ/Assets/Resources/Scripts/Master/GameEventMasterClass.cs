using UnityEngine;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Waypoint;
using Gob3AQ.Brain.ItemsInteraction;
using System;

namespace Gob3AQ.GameEventMaster
{

    public class GameEventMasterClass : MonoBehaviour
    {
        private static GameEventMasterClass _singleton;


        public static void IsEventOccurredService(GameEvent ev, out bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 2 events, which are placed at the beginning */

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            occurred = mbfs.GetIndividualBool(itembit);
        }

        public static void CommitEventService(GameEvent ev, bool occurred)
        {
            int evIndex = (int)ev;
            evIndex += (int)GamePickableItem.ITEM_PICK_TOTAL;    /* Every item has 2 events, which are placed at the beginning */

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            mbfs.SetIndividualBool(itembit, occurred);

            VARMAP_GameEventMaster.SET_ELEM_EVENTS_OCCURRED(arraypos, mbfs);
        }

        public static void TakeItemFromSceneEventService(GamePickableItem item)
        {
            int evIndex = (int)item;

            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);

            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);

            mbfs.SetIndividualBool(itembit, true);

            VARMAP_GameEventMaster.SET_ELEM_EVENTS_OCCURRED(arraypos, mbfs);
        }


        public static void IsItemTakenFromSceneService(GamePickableItem item, out bool taken)
        {
            int evIndex = (int)item;
            GetArrayIndexAndPos(evIndex, out int arraypos, out int itembit);
            MultiBitFieldStruct mbfs = VARMAP_GameEventMaster.GET_ELEM_EVENTS_OCCURRED(arraypos);
            taken = mbfs.GetIndividualBool(itembit);
        }


        public static void GetItemInteractionService(CharacterType character, GameItem item, out InteractionItemType interaction)
        {
            interaction = ItemsInteractionsClass.GetItemInteraction(character, item);
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

        private void Awake()
        {
            if (_singleton)
            {
                Destroy(this);
                return;
            }
            else
            {
                _singleton = this;
            }
        }



        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}