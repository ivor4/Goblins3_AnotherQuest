using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using System;
using TMPro;
using UnityEngine;

namespace Gob3AQ.GameMenu
{

    [System.Serializable]
    public class GameMenuClass : MonoBehaviour
    {
        [SerializeField]
        private GameObject UICanvas;

        private static GameMenuClass _singleton;
        private static GameObject _itemMenu;
        private static bool _prevItemMenuOpened;
        private static bool _pendingRefresh;
        private static PickableItemDisplayClass[] _displayItemArray;
        private static Camera _mainCamera;
        private static Rect _upperGameMenuRect;
        
        private static string[] _gameMenuToolbarStrings;

        private static CharacterType dialog_sender;
        private static DialogType dialog_currentDialog;
        private static int dialog_currentPhraseIndex;
        private static bool dialog_optionPending;


        private static GameObject UICanvas_dialogObj;
        private static TMP_Text UICanvas_dialogObj_sender;
        private static TMP_Text UICanvas_dialogObj_msg;
        private static GameObject UICanvas_dialogOptions;
        private static DialogOptionButtonClass[] UICanvas_dialogOptionButtons;


        public static void ShowDialogueService(CharacterType charType, DialogType dialog, DialogPhrase phrase)
        {
            int selectableOptions;

            Span<DialogPhrase> phraseOptions = stackalloc DialogPhrase[GameFixedConfig.MAX_DIALOG_OPTIONS];

            if (dialog == DialogType.DIALOG_SIMPLE)
            {
                phraseOptions[0] = phrase;
                selectableOptions = 1;
            }
            else
            {
                selectableOptions = 0;

                ReadOnlySpan<DialogOptionConfig> dialogOptionConfigs = ResourceDialogsAtlasClass.DialogOptionConfigs;
                ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.DialogConfigs[(int)dialog];
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                for (int i = 0; i < dialogOptions.Length; i++)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref dialogOptionConfigs[(int)dialogOptions[i]];
                    bool valid;

                    if (dialogOptionConfig.conditionEvent == GameEvent.EVENT_NONE)
                    {
                        valid = true;
                    }
                    else
                    {
                        VARMAP_GameMenu.IS_EVENT_OCCURRED(dialogOptionConfig.conditionEvent, out valid);
                    }

                    if (valid)
                    {
                        phraseOptions[selectableOptions] = dialogOptionConfig.phrases[0];
                        ++selectableOptions;
                    }
                }
            }

            /* Enshorten only up to total selectableOptions */
            phraseOptions = phraseOptions[..selectableOptions];



            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectableOptions > 1)
            {
                UICanvas_dialogObj_msg.gameObject.SetActive(false);
                UICanvas_dialogObj_sender.gameObject.SetActive(false);
                UICanvas_dialogOptions.SetActive(true);

                dialog_sender = charType;
                dialog_currentDialog = dialog;
                dialog_optionPending = true;

                for (int i = 0; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
                {
                    if (i < selectableOptions)
                    {
                        ref readonly PhraseContent optionPhraseContent = ref ResourceDialogsClass.GetPhraseContent(phraseOptions[i]);

                        UICanvas_dialogOptionButtons[i].SetOptionText(in optionPhraseContent.message);
                        UICanvas_dialogOptionButtons[i].SetActive(true);
                    }
                    else
                    {
                        UICanvas_dialogOptionButtons[i].SetActive(false);
                    }
                }
                
            }
            else if (selectableOptions == 1)
            {
                dialog_optionPending = false;
                StartPhrase(charType, phraseOptions[0]);
            }
            else
            {
                dialog_optionPending = false;
                Debug.LogError("GraphicsMasterClass.ShowDialogueService: No valid dialog options found for dialog " + dialog.ToString());
            }
        }

        private static void _DialogOptionSelected(int optionIndex)
        {
            if ((VARMAP_GameMenu.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_DIALOG) && dialog_optionPending)
            {
                ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.DialogConfigs[(int)dialog_currentDialog];
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;
                ReadOnlySpan<DialogOptionConfig> dialogOptionConfigs = ResourceDialogsAtlasClass.DialogOptionConfigs;

                StartPhrase(dialog_sender, dialogOptionConfigs[(int)dialogOptions[optionIndex]].phrases[dialog_currentPhraseIndex]);
                dialog_optionPending = false;
            }
        }

        private static void StartPhrase(CharacterType charType, DialogPhrase phrase)
        {
            UICanvas_dialogObj_msg.gameObject.SetActive(true);
            UICanvas_dialogObj_sender.gameObject.SetActive(true);
            UICanvas_dialogOptions.SetActive(false);

            ref readonly PhraseContent content = ref ResourceDialogsClass.GetPhraseContent(phrase);


            if (content.senderName.Length == 1)
            {
                UICanvas_dialogObj_sender.text = CharacterNames.GetCharacterName(charType);
            }
            else
            {
                UICanvas_dialogObj_sender.text = content.senderName;
            }

            UICanvas_dialogObj_msg.text = content.message;
        }


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

                UICanvas_dialogObj = UICanvas.transform.Find("DialogObj").gameObject;
                UICanvas_dialogObj_sender = UICanvas_dialogObj.transform.Find("DialogSender").GetComponent<TMP_Text>();
                UICanvas_dialogObj_msg = UICanvas_dialogObj.transform.Find("DialogMsg").GetComponent<TMP_Text>();
                UICanvas_dialogOptions = UICanvas_dialogObj.transform.Find("DialogOptions").gameObject;

                UICanvas_dialogOptionButtons = new DialogOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];

                for(int i=0;i<GameFixedConfig.MAX_DIALOG_OPTIONS;++i)
                {
                    Transform btnTransf = UICanvas_dialogOptions.transform.Find("DialogOption" + (i + 1).ToString());
                    UICanvas_dialogOptionButtons[i] = btnTransf.Find("ActiveArea").gameObject.GetComponent<DialogOptionButtonClass>();
                    UICanvas_dialogOptionButtons[i].SetClickDelegate(_DialogOptionSelected);
                }

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