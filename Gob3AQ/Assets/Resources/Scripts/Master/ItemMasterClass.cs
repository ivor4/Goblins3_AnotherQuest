using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.Item;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;
        private static List<GamePickableItem> _actualPickedItems;
        private static bool loaded;


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        public static void TakeItemObjectService(GamePickableItem item)
        {
            ReadOnlyList<ItemClass> itemlist = new(null);

            /* Get item gameObjects registered on this scenario (physically) */
            VARMAP_ItemMaster.GET_SCENARIO_ITEM_LIST(ref itemlist);

            /* Remove it, in case it has been taken "from the street" */
            for(int i=0; i< itemlist.Count; i++)
            {
                ItemClass iclass = itemlist[i];

                if(iclass.Pickable == item)
                {
                    iclass.gameObject.SetActive(false);
                    break;
                }
            }

            /* Officially take it and set corresponding events */
            VARMAP_ItemMaster.TAKE_ITEM_EVENT(item);

            /* No need to update whole list again */
            _actualPickedItems.Add(item);
        }


        public static void GetPickedItemListService(ref ReadOnlyList<GamePickableItem> list)
        {
            list.SetRefList(_actualPickedItems);
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
                _actualPickedItems = new List<GamePickableItem>(GameFixedConfig.MAX_PICKED_ITEMS);
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
                bool picked;

                VARMAP_ItemMaster.IS_ITEM_TAKEN(pickable, out picked);

                if(picked)
                {
                    _actualPickedItems.Add(pickable);
                }
            }
            
        }

    }
}