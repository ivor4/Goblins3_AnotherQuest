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



        public static void TakeItemObjectService(GamePickableItem item)
        {
            ReadOnlyList<ItemClass> itemlist = new(null);
            VARMAP_ItemMaster.GET_ITEM_LIST(ref itemlist);

            for(int i=0; i< itemlist.Count; i++)
            {
                ItemClass iclass = itemlist[i];

                if(iclass.Pickable == item)
                {
                    Destroy(iclass.gameObject);
                    break;
                }
            }

            VARMAP_ItemMaster.TAKE_ITEM_EVENT(item);
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