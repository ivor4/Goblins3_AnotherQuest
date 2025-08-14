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
        private static bool _pendingRefresh;
        private static PickableItemDisplayClass[] _displayItemArray;
        private static Camera _mainCamera;
        private static Rect _upperGameMenuRect;
        
        private static string[] _gameMenuToolbarStrings;




        void Awake()
        {
            if(_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;

                Transform child = transform.Find("ItemMenu");
                _itemMenu = child.gameObject;

                _displayItemArray = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];

                _pendingRefresh = false;

                float menuHeight = Screen.safeArea.height * GameFixedConfig.MENU_TOP_SCREEN_HEIGHT_PERCENT;
                _upperGameMenuRect = new Rect(0, 0, Screen.safeArea.width, menuHeight);

                _gameMenuToolbarStrings = new string[] { "Save Game", "Exit Game" };
            }
        }

        void Start()
        {
            _prevItemMenuOpened = false;

            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);

            Execute_Load_Async();
        }

        

        void Update()
        {
            Game_Status gstatus = VARMAP_GameMenu.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    Execute_Play();
                    break;

                default:
                    break;
            }

 
        }

        void OnGUI()
        {
            GUI.backgroundColor = Color.blue;

            GUILayout.BeginArea(_upperGameMenuRect);


            
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            int selected = GUILayout.Toolbar(-1, _gameMenuToolbarStrings, GUILayout.Height(_upperGameMenuRect.height));

            switch(selected)
            {
                case 0:
                    VARMAP_GameMenu.SAVE_GAME();
                    break;

                case 1:
                    VARMAP_GameMenu.EXIT_GAME(out _);
                    break;

                default:
                    break;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            

            GUILayout.EndArea();
        }




        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
                VARMAP_GameMenu.UNREG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
            }
        }

        private static async void Execute_Load_Async()
        {
            for(int i=0;i<GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS;++i)
            {
                AsyncInstantiateOperation asyncop = InstantiateAsync(ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_MENU_PICKABLE_ITEM));
                await asyncop;

                GameObject newObj = asyncop.Result[0] as GameObject;
                Transform newTrns = newObj.transform;

                newObj.name = "PickableItemDisplay" + i;
                newTrns.SetParent(_itemMenu.transform, false);

                float x = -0.825f + 0.333f * (i % GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);
                float y = 0.825f - 0.333f * (i / GameFixedConfig.MAX_DISPLAYED_HOR_PICKED_ITEMS);

                newTrns.localPosition = new Vector3(x, y);

                _displayItemArray[i] = newObj.GetComponent<PickableItemDisplayClass>();
                _displayItemArray[i].SetCallFunction(OnItemDisplayClick);
            }

            _mainCamera = Camera.main;


            VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
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


        private static void _OnItemOwnerChanged(ChangedEventType evtype, in CharacterType oldVal, in CharacterType newVal)
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