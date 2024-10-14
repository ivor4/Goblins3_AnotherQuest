using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.Item;
using Gob3AQ.Brain.ItemsInteraction;
using System;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;
        private static bool loaded;

        public static void GetItemInteractionService(in ItemUsage usage, out InteractionItemType interaction)
        {
            interaction = ItemsInteractionsClass.GetItemInteraction(usage);
        }

        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        public static void TakeItemService(GamePickableItem item, CharacterType character)
        {
            /* Get item gameObjects registered on this scenario (physically) */
            VARMAP_ItemMaster.GET_SCENARIO_ITEM_LIST(out ReadOnlyList<ItemClass> itemlist);

            /* Remove it, in case it has been taken "from the street" */
            /* Should this be moved to LevelMaster ? */
            int totalItems = itemlist.Count;

            for(int i=0; i< totalItems; i++)
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
            VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)item, character);
        }


        public static void SelectPickableItemService(GamePickableItem item)
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(item);
        }

        public static void CancelPickableItemService()
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(GamePickableItem.ITEM_PICK_NONE);
        }

        
        public static void UseItemService(in ItemUsage usage)
        {
            InteractionItemType interaction;
            CharacterType activePlayer = VARMAP_ItemMaster.GET_PLAYER_SELECTED();

            if (activePlayer != CharacterType.CHARACTER_NONE)
            {
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemSource];
                interaction = ItemsInteractionsClass.GetItemInteraction(usage);

                /* Pickable items which are already stored in inventory */
                if (pickable != GamePickableItem.ITEM_PICK_NONE)
                {
                    CharacterType owner = VARMAP_ItemMaster.GET_ELEM_PICKABLE_ITEM_OWNER((int)pickable);

                    if ((owner == activePlayer)&&(interaction == InteractionItemType.INTERACTION_USE))
                    {
                        /* Lose item */
                        VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);
                    }
                }

                /* Common part */
                if(interaction == InteractionItemType.INTERACTION_USE)
                {
                    /* ... */
                }
            }
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


    }
}