using UnityEngine;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.FixedConfig;

namespace Gob3AQ.GameMenu
{
    public class GameMenuClass : MonoBehaviour
    {
        private static GameMenuClass _singleton;
        private static GameObject _itemMenu;
        private static SpriteRenderer _itemMenuspr;
        private static bool _prevItemMenuOpened;
        private static bool _loaded;
        private static GameObject[] _displayItemArray;

        void Awake()
        {
            if(_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                DontDestroyOnLoad(gameObject);

                Transform child = transform.Find("ItemMenu");
                _itemMenu = child.gameObject;
                _itemMenuspr = _itemMenu.GetComponent<SpriteRenderer>();

                _displayItemArray = new GameObject[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];

                _loaded = false;
            }
        }

        void Start()
        {
            _prevItemMenuOpened = false;
        }

        void Update()
        {
            Game_Status gstatus = VARMAP_GameMenu.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Load();
                    break;

                case Game_Status.GAME_STATUS_PLAY:
                    Execute_Play();
                    break;

                default:
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

        private static void Execute_Load()
        {
            if(!_loaded)
            {
                for(int i=0;i<GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS;++i)
                {
                    GameObject newObj = Instantiate(ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_MENU_PICKABLE_ITEM));
                    Transform newTrns = newObj.transform;

                    newObj.name = "PickableItemDisplay" + i;
                    newTrns.SetParent(_itemMenu.transform, false);

                    float x = -0.825f + 0.5f * (i % GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);
                    float y = 0.825f - 0.5f * (i / GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);

                    newTrns.localPosition = new Vector3(x, y);
                    

                    _displayItemArray[i] = newObj;
                }

                _loaded = true;
            }
        }

        private static void Execute_Play()
        {
            bool itemMenuOpened = VARMAP_GameMenu.GET_ITEM_MENU_ACTIVE();

            if (itemMenuOpened)
            {
                if (!_prevItemMenuOpened)
                {
                    _itemMenu.SetActive(true);

                    /* Populate menu */
                }

            }
            else if (_prevItemMenuOpened)
            {
                _itemMenu.SetActive(false);
            }
            else
            {
                /**/
            }



            _prevItemMenuOpened = itemMenuOpened;
        }
    }
}