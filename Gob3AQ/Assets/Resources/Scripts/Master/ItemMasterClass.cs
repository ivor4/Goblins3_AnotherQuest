using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.Item;
using System;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        

        private static ItemMasterClass _singleton;
        private static List<PickableItemAndOwner> _actualPickedItems;
        private static bool loaded;


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        public static void TakeItemService(GamePickableItem item, CharacterType character)
        {
            ReadOnlyList<ItemClass> itemlist = new(null);

            /* Get item gameObjects registered on this scenario (physically) */
            VARMAP_ItemMaster.GET_SCENARIO_ITEM_LIST(ref itemlist);

            /* Remove it, in case it has been taken "from the street" */
            /* Should this be moved to LevelMaster ? */
            for(int i=0; i< itemlist.Count; i++)
            {
                ItemClass iclass = itemlist[i];

                if(iclass.Pickable == item)
                {
                    iclass.gameObject.SetActive(false);
                    break;
                }
            }

            /* Officially take it and set corresponding events.
             * If it was not in scene, at least an event will be set (which could be useful) */
            VARMAP_ItemMaster.TAKE_ITEM_FROM_SCENE_EVENT(item);

            /* Set item owner in VARMAP */
            ModifyOwnedItem(item, character);
        }

        public static void IsItemOwnedService(GamePickableItem item, out CharacterType owner)
        {
            owner = GetItemOwner(item);
        }


        public static void GetPickedItemListService(ref ReadOnlyList<PickableItemAndOwner> list)
        {
            list.SetRefList(_actualPickedItems);
        }

        public static void SelectPickableItemService(GamePickableItem item)
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(item);
        }

        public static void CancelPickableItemService()
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(GamePickableItem.ITEM_PICK_NONE);
        }



        void Awake()
        {
            if(_singleton)
            {
                Destroy(this);
            }
            else
            {
                _singleton = this;
                DontDestroyOnLoad(gameObject);
                _actualPickedItems = new List<PickableItemAndOwner>(GameFixedConfig.MAX_PICKED_ITEMS);
                loaded = false;
            }
        }

        void Update()
        {
            Game_Status gstatus = VARMAP_ItemMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    if(!loaded)
                    {
                        BuildPickedItemList();
                        loaded = true;
                    }
                    break;
            }
        }


        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }


        private static void BuildPickedItemList()
        {
            for(GamePickableItem pickable = 0; pickable < GamePickableItem.ITEM_PICK_TOTAL; pickable++)
            {
                CharacterType owner = GetItemOwner(pickable);

                if (owner != CharacterType.CHARACTER_NONE)
                {
                    PickableItemAndOwner elem;
                    elem.character = owner;
                    elem.item = pickable;
                    _actualPickedItems.Add(elem);
                }
            }
            
        }

        private static CharacterType GetItemOwner(GamePickableItem item)
        {
            GetArrayIndexAndPos(item, out int arrayIndex, out int offset);

            MultiBitFieldStruct st = VARMAP_ItemMaster.GET_ELEM_PICKABLE_ITEM_OWNER(arrayIndex);
            CharacterType owner = (CharacterType)(st.GetValueFromOffset(offset) & 0xF);

            return owner;
        }

        private static void ModifyOwnedItem(GamePickableItem item, CharacterType character)
        {
            CharacterType prevOwner = GetItemOwner(item);


            /* This array must be changed here - BAD CODE*/

            PickableItemAndOwner elem;
            elem.character = character;
            elem.item = item;

            /* No need to update whole list again */
            _actualPickedItems.Add(elem);

            /**/


            GetArrayIndexAndPos(item, out int arrayIndex, out int offset);

            MultiBitFieldStruct st = VARMAP_ItemMaster.GET_ELEM_PICKABLE_ITEM_OWNER(arrayIndex);

            st.SetValueFromOffset(offset, (ulong)character, 0xF);

            VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER(arrayIndex, st);
        }

        /// <summary>
        /// Based on 64 bit bitfield, gets decomposition of array element and bit position.
        /// Every element consumes 4 bits
        /// </summary>
        /// <param name="item">Item (do not use ITEM_NONE)</param>
        /// <param name="arrayIndex">index of array where to look owner of item</param>
        /// <param name="offset">offset of bits to find 4bits which have the owner</param>
        private static void GetArrayIndexAndPos(GamePickableItem item, out int arrayIndex, out int offset)
        {
            int item_int = (int)item;

            /* Only 16 owner per array element (64 / 4) */
            arrayIndex = item_int >> 4;
            offset = (item_int & 0xF) << 2;
        }

    }
}