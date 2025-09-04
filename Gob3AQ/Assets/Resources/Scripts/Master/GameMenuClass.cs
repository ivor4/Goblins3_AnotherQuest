using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
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
        private static bool _menuOpened;
        private static PickableItemDisplayClass[] _displayItemArray;
        private static Camera _mainCamera;
        private static Rect _upperGameMenuRect;
        
        private static string[] _gameMenuToolbarStrings;

        private static CharacterType dialog_sender;
        private static int dialog_currentPhraseIndex;
        private static int dialog_totalPhrases;
        private static DialogOption dialog_optionPhrases;
        private static bool dialog_optionPending;
        private static bool dialog_tellingInProgress;
        private static Coroutine dialog_coroutine;


        private static GameObject UICanvas_dialogObj;
        private static TMP_Text UICanvas_dialogObj_sender;
        private static TMP_Text UICanvas_dialogObj_msg;
        private static GameObject UICanvas_dialogOptions;
        private static DialogOptionButtonClass[] UICanvas_dialogOptionButtons;

        /// <summary>
        /// Displays a dialogue interface based on the specified character type, dialogue type, and initial phrase.
        /// </summary>
        /// <remarks>This method configures and displays a dialogue interface based on the provided
        /// parameters. For simple dialogues,  the specified phrase is displayed directly. For multi-choice dialogues,
        /// the available options are dynamically  populated based on the active dialogue configurations. If no valid
        /// options are found, an error is logged.</remarks>
        /// <param name="charType">The type of the character initiating the dialogue.</param>
        /// <param name="dialog">The type of dialogue to display, which determines the structure and options available.</param>
        /// <param name="phrase">The initial phrase to display if the dialogue type is simple.</param>
        public static void ShowDialogueService(CharacterType charType, DialogType dialog, DialogPhrase phrase)
        {
            int selectablePhrases;

            DialogPhrase uniquePhrase = DialogPhrase.PHRASE_NONE;

            if (dialog == DialogType.DIALOG_SIMPLE)
            {
                uniquePhrase = phrase;
                selectablePhrases = 1;
            }
            else
            {
                selectablePhrases = 0;

                ReadOnlySpan<DialogOptionConfig> dialogOptionConfigs = ResourceDialogsAtlasClass.DialogOptionConfigs;
                ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.DialogConfigs[(int)dialog];
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                for (int i = 0; i < dialogOptions.Length; i++)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref dialogOptionConfigs[(int)dialogOptions[i]];

                    if (IsDialogOptionActive(in dialogOptionConfig))
                    {
                        ref readonly PhraseContent optionPhraseContent = ref ResourceDialogsClass.GetPhraseContent(dialogOptionConfig.phrases[0]);

                        UICanvas_dialogOptionButtons[selectablePhrases].SetOptionText(in optionPhraseContent.message);
                        UICanvas_dialogOptionButtons[selectablePhrases].SetDialogOption(dialogOptions[i]);
                        UICanvas_dialogOptionButtons[selectablePhrases].SetActive(true);

                        uniquePhrase = dialogOptionConfig.phrases[0];
                        ++selectablePhrases;
                    }
                }

                /* Clear previous usage data and deactivate */
                for (int i=selectablePhrases;i<GameFixedConfig.MAX_DIALOG_OPTIONS;++i)
                {
                    UICanvas_dialogOptionButtons[i].SetOptionText(in string.Empty);
                    UICanvas_dialogOptionButtons[i].SetDialogOption(DialogOption.DIALOG_OPTION_NONE);
                    UICanvas_dialogOptionButtons[i].SetActive(false);
                }
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectablePhrases > 1)
            {
                UICanvas_dialogObj_msg.gameObject.SetActive(false);
                UICanvas_dialogObj_sender.gameObject.SetActive(false);
                UICanvas_dialogOptions.SetActive(true);

                dialog_sender = charType;
                dialog_optionPending = true;
                dialog_tellingInProgress = false;
            }
            else if (selectablePhrases == 1)
            {
                /* Initialize phrase index */
                dialog_optionPending = false;
                dialog_currentPhraseIndex = 0;
                dialog_totalPhrases = 1;
                dialog_optionPhrases = DialogOption.DIALOG_OPTION_NONE;

                StartPhrase(charType, uniquePhrase);
            }
            else
            {
                dialog_optionPending = false;
                dialog_tellingInProgress = false;
                Debug.LogError("GraphicsMasterClass.ShowDialogueService: No valid dialog options found for dialog " + dialog.ToString());
            }
        }

        private static void _DialogOptionSelected(DialogOption option)
        {
            if ((VARMAP_GameMenu.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_DIALOG) && dialog_optionPending)
            {
                ReadOnlySpan<DialogOptionConfig> dialogOptionConfigs = ResourceDialogsAtlasClass.DialogOptionConfigs;
                ref readonly DialogOptionConfig dialogOptionConfig = ref dialogOptionConfigs[(int)option];

                /* If option is permitted, show it */
                if (IsDialogOptionActive(in dialogOptionConfig))
                {
                    dialog_currentPhraseIndex = 0;
                    dialog_totalPhrases = dialogOptionConfig.phrases.Length;
                    dialog_optionPhrases = option;

                    StartPhrase(dialog_sender, dialogOptionConfigs[(int)option].phrases[0]);
                }
            }

            dialog_optionPending = false;
        }

        private static void StartPhrase(CharacterType charType, DialogPhrase phrase)
        {
            UICanvas_dialogObj_msg.gameObject.SetActive(true);
            UICanvas_dialogObj_sender.gameObject.SetActive(true);
            UICanvas_dialogOptions.SetActive(false);

            /* Get current timestamp */
            dialog_tellingInProgress = true;

            ref readonly PhraseContent content = ref ResourceDialogsClass.GetPhraseContent(phrase);

            /* Set sender name */
            if (content.senderName.Length == 1)
            {
                UICanvas_dialogObj_sender.text = CharacterNames.GetCharacterName(charType);
            }
            else
            {
                UICanvas_dialogObj_sender.text = content.senderName;
            }

            UICanvas_dialogObj_msg.text = content.message;

            dialog_coroutine = _singleton.StartCoroutine(EndPhrase(2f));
        }

        private static bool IsDialogOptionActive(in DialogOptionConfig config)
        {
            bool valid;

            if (config.conditionEvent == GameEvent.EVENT_NONE)
            {
                valid = true;
            }
            else
            {
                VARMAP_GameMenu.IS_EVENT_OCCURRED(config.conditionEvent, out valid);
                valid ^= config.conditionNotOccurred;
            }

            return valid;
        }

        private static IEnumerator EndPhrase(float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);

            if (dialog_tellingInProgress)
            {
                dialog_tellingInProgress = false;
                ++dialog_currentPhraseIndex;

                if(dialog_totalPhrases > dialog_currentPhraseIndex)
                {
                    /* More phrases to say, wait for user interaction */
                    ReadOnlySpan<DialogOptionConfig> dialogOptionConfigs = ResourceDialogsAtlasClass.DialogOptionConfigs;
                    StartPhrase(dialog_sender, dialogOptionConfigs[(int)dialog_optionPhrases].phrases[dialog_currentPhraseIndex]);
                }
                else
                {
                    /* End of dialog */
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(false);

                    VARMAP_GameMenu.END_DIALOGUE();
                }
            }
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
            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
            VARMAP_GameMenu.REG_ITEM_MENU_ACTIVE(_OnItemMenuActiveChanged);
            VARMAP_GameMenu.REG_GAMESTATUS(_OnGameStatusChanged);

            Execute_Load_Async();
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
                VARMAP_GameMenu.UNREG_ITEM_MENU_ACTIVE(_OnItemMenuActiveChanged);
                VARMAP_GameMenu.UNREG_GAMESTATUS(_OnGameStatusChanged);
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
        }


        private static void _OnItemOwnerChanged(ChangedEventType evtype, in CharacterType oldVal, in CharacterType newVal)
        {
            _ = evtype;
            _ = oldVal;
            _ = newVal;

            /* Populate menu */

            if (_menuOpened)
            {
                RefreshItemMenuElements();
            }
        }

        private static void _OnItemMenuActiveChanged(ChangedEventType evtype, in bool oldVal, in bool newVal)
        {
            _ = evtype;

            if(newVal && !oldVal)
            {
                /* Activate family */
                _itemMenu.SetActive(true);

                /* Place Item Menu just in center of camera (World coordinates) */
                _itemMenu.transform.position = (Vector2)_mainCamera.transform.position;

                /* Populate menu */
                RefreshItemMenuElements();
            }
            else if(!newVal && oldVal)
            {
                /* Deactivate family */
                _itemMenu.SetActive(false);
            }
            else
            {
                /**/
            }

            _menuOpened = newVal;
        }   

        private static void _OnGameStatusChanged(ChangedEventType evtype, in Game_Status oldVal, in Game_Status newVal)
        {
            _ = evtype;
            _ = newVal;

            /* If game status changed from pause to play, restore previous status */
            if ((oldVal == Game_Status.GAME_STATUS_PLAY_DIALOG) && (newVal != Game_Status.GAME_STATUS_PAUSE))
            {
                /* If dialog was in progress, stop it */
                if (dialog_coroutine != null)
                {
                    _singleton.StopCoroutine(dialog_coroutine);
                    dialog_coroutine = null;
                }

                dialog_tellingInProgress = false;
                dialog_optionPending = false;
                UICanvas_dialogObj_msg.gameObject.SetActive(false);
                UICanvas_dialogObj_sender.gameObject.SetActive(false);
                UICanvas_dialogOptions.SetActive(false);
            }
        }

        private static void OnItemDisplayClick(GameItem item)
        {
            if (_menuOpened)
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
}