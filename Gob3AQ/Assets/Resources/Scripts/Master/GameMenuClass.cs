using UnityEngine;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.Libs.Arith;
using System;

namespace Gob3AQ.GameMenu
{

    public class GameMenuClass : MonoBehaviour
    {

        private static GameMenuClass _singleton;
        private static GameObject _itemMenu;
        private static bool _prevItemMenuOpened;
        private static bool _loaded;
        private static bool _levelLoaded;
        private static bool _pendingRefresh;
        private static PickableItemDisplayClass[] _displayItemArray;
        private static Camera _mainCamera;

        

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

                _displayItemArray = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];

                _loaded = false;
                _levelLoaded = false;
                _pendingRefresh = false;
            }
        }

        void Start()
        {
            _prevItemMenuOpened = false;

            VARMAP_GameMenu.REG_ACTUAL_ROOM(_OnLevelChanged);
            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
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
                VARMAP_GameMenu.UNREG_ACTUAL_ROOM(_OnLevelChanged);
                VARMAP_GameMenu.UNREG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
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

                    float x = -0.825f + 0.333f * (i % GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);
                    float y = 0.825f - 0.333f * (i / GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);

                    newTrns.localPosition = new Vector3(x, y);

                    _displayItemArray[i] = newObj.GetComponent<PickableItemDisplayClass>();
                    _displayItemArray[i].SetCallFunction(OnItemDisplayClick);
                }

                _loaded = true;
            }

            if(!_levelLoaded)
            {
                _mainCamera = Camera.main;
                _levelLoaded = true;
            }
        }

        private static void Execute_Play()
        {
            bool itemMenuOpened = VARMAP_GameMenu.GET_ITEM_MENU_ACTIVE();

            if (itemMenuOpened)
            {
                if ((!_prevItemMenuOpened)||_pendingRefresh)
                {
                    /* Activate family */
                    _itemMenu.SetActive(true);

                    /* Place Item Menu just in center of camera (World coordinates) */
                    _itemMenu.transform.position = (Vector2)_mainCamera.transform.position;

                    /* Populate menu */
                    RefreshItemMenuElements();
                }
            }
            else if (_prevItemMenuOpened)
            {
                /* Deactivate family */
                _itemMenu.SetActive(false);
            }
            else
            {
                /**/
            }



            _prevItemMenuOpened = itemMenuOpened;
        }

        private static void RefreshItemMenuElements()
        {
            ReadOnlySpan<CharacterType> item_owner = VARMAP_GameMenu.GET_ARRAY_PICKABLE_ITEM_OWNER();
            CharacterType selectedChar = VARMAP_GameMenu.GET_PLAYER_SELECTED();


            int totalarrayItems = item_owner.Length;
            int lastFoundItemIndex = 0;


            /* Fill all spots with first available item */
            for(int i = 0; i < _displayItemArray.Length; i++)
            {
                bool found = false;
                if (selectedChar != CharacterType.CHARACTER_NONE)
                {
                    for (; (lastFoundItemIndex < totalarrayItems)&&(!found); lastFoundItemIndex++)
                    {
                        /* If this element has to show a picked item */
                        if (item_owner[lastFoundItemIndex] == selectedChar)
                        {
                            _displayItemArray[i].gameObject.SetActive(true);
                            _displayItemArray[i].SetDisplayedItem((GameItem)lastFoundItemIndex);
                            found = true;
                        }
                    }
                }

                if(!found)
                {
                    /* Otherwise keep hidden */
                    _displayItemArray[i].gameObject.SetActive(false);
                }
                
            }

            _pendingRefresh = false;
        }

        private static void _OnLevelChanged(ChangedEventType evtype, ref Room oldval, ref Room newval)
        {
            _ = evtype;
            _ = oldval;
            _ = newval;

            /* It is needed to take new Camera from this level, as this object survives between levels */
            _levelLoaded = false;
        }

        private static void _OnItemOwnerChanged(ChangedEventType evtype, ref CharacterType oldVal, ref CharacterType newVal)
        {
            _ = evtype;
            _ = oldVal;
            _ = newVal;

            _pendingRefresh = true;
        }

        private static void OnItemDisplayClick(GameItem item)
        {
            GameItem prevChoosen = VARMAP_GameMenu.GET_PICKABLE_ITEM_CHOSEN();

            
            if (prevChoosen == item)
            {
                VARMAP_GameMenu.CANCEL_PICKABLE_ITEM();
            }
            else
            {
                /* Possible interaction of item combine ? */
                _ = prevChoosen;

                VARMAP_GameMenu.SELECT_PICKABLE_ITEM(item);
            }
        }

    }
}