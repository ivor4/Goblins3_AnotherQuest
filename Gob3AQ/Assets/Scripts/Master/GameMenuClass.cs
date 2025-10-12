using Gob3AQ.Brain.ItemsInteraction;
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
using Unity.VisualScripting;
using UnityEngine;

namespace Gob3AQ.GameMenu
{

    [System.Serializable]
    public class GameMenuClass : MonoBehaviour
    {
        [SerializeField]
        private GameObject UICanvas;

        private static GameObject _UICanvas;

        private static GameMenuClass _singleton;
        private static bool _menuOpened;
        private static PickableItemDisplayClass[] _displayItemArray;
        private static Camera _mainCamera;
        private static Rect _upperGameMenuRect;
        
        private static string[] _gameMenuToolbarStrings;

        private static GameItem[] dialog_current_talkers;
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

        private static GameObject UICanvas_itemMenuObj;

        /// <summary>
        /// Displays a dialogue interface based on the specified character type, dialogue type, and initial phrase.
        /// </summary>
        /// <remarks>This method configures and displays a dialogue interface based on the provided
        /// parameters. For simple dialogues,  the specified phrase is displayed directly. For multi-choice dialogues,
        /// the available options are dynamically  populated based on the active dialogue configurations. If no valid
        /// options are found, an error is logged.</remarks>
        /// <param name="defaultTalkers">Default list of talkers (Player and ItemDest)</param>
        /// <param name="dialog">The type of dialogue to display, which determines the structure and options available.</param>
        /// <param name="phrase">The initial phrase to display if the dialogue type is simple.</param>
        public static void ShowDialogueService(ReadOnlySpan<GameItem> defaultTalkers, DialogType dialog, DialogPhrase phrase)
        {
            int selectablePhrases;

            DialogPhrase uniquePhrase = DialogPhrase.PHRASE_NONE;
            DialogOption uniqueOption = DialogOption.DIALOG_OPTION_NONE;

            ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogConfig(dialog);


            if (dialog == DialogType.DIALOG_SIMPLE)
            {
                uniquePhrase = phrase;
                uniqueOption = DialogOption.DIALOG_OPTION_SIMPLE;
                selectablePhrases = 1;
            }
            else
            {
                selectablePhrases = 0;
                
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                for (int i = 0; i < dialogOptions.Length; i++)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[i]);

                    VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);

                    if (valid)
                    {
                        ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;

                        ref readonly PhraseContent optionPhraseContent = ref ResourceDialogsClass.GetPhraseContent(dialogPhrases[0]);

                        UICanvas_dialogOptionButtons[selectablePhrases].SetOptionText(in optionPhraseContent.message);
                        UICanvas_dialogOptionButtons[selectablePhrases].SetDialogOption(dialogOptions[i]);
                        UICanvas_dialogOptionButtons[selectablePhrases].SetActive(true);

                        uniquePhrase = dialogPhrases[0];
                        uniqueOption = dialogOptions[i];
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

            

            /* Chose between default talkers or imposed */

            if (dialogConfig.Talkers[0] != GameItem.ITEM_NONE)
            {
                dialogConfig.Talkers.CopyTo(dialog_current_talkers);
            }
            else
            {
                defaultTalkers.CopyTo(dialog_current_talkers);
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectablePhrases > 1)
            {
                UICanvas_dialogObj_msg.gameObject.SetActive(false);
                UICanvas_dialogObj_sender.gameObject.SetActive(false);
                UICanvas_dialogOptions.SetActive(true);

                dialog_optionPending = true;
                dialog_tellingInProgress = false;
            }
            else if (selectablePhrases == 1)
            {
                /* Initialize phrase index */
                dialog_optionPending = false;

                StartDialogue(uniqueOption, 1);
                StartPhrase(uniquePhrase);
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
                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(option);

                dialog_optionPending = false;

                /* If option is permitted, show it */
                VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);
                if (valid)
                {
                    ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                    StartDialogue(option, dialogPhrases.Length);
                    StartPhrase(dialogPhrases[0]);
                }
            }
        }

        private static void StartDialogue(DialogOption option, int totalPhrases)
        {
            dialog_optionPhrases = option;
            dialog_totalPhrases = totalPhrases;
            dialog_currentPhraseIndex = 0;
        }

        private static void StartPhrase(DialogPhrase phrase)
        {
            UICanvas_dialogObj_msg.gameObject.SetActive(true);
            UICanvas_dialogObj_sender.gameObject.SetActive(true);
            UICanvas_dialogOptions.SetActive(false);

            /* Preset */
            dialog_tellingInProgress = true;


            ref readonly PhraseContent content = ref ResourceDialogsClass.GetPhraseContent(phrase);
            GameItem talkerItem = dialog_current_talkers[content.config.talkerIndex];
            ref readonly ItemInfo talkerInfo = ref ItemsInteractionsClass.GetItemInfo(talkerItem);

            /* Set sender name */
            UICanvas_dialogObj_sender.text = ResourceDialogsClass.GetName(talkerInfo.name);
            UICanvas_dialogObj_msg.text = content.message;

            dialog_coroutine = _singleton.StartCoroutine(EndPhrase(2f));
        }


        private static IEnumerator EndPhrase(float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);

            /* Erase this coroutine reference */
            dialog_coroutine = null;

            EndPhrase_Action();
        }

        private static void EndPhrase_Action()
        {
            if (dialog_tellingInProgress)
            {
                dialog_tellingInProgress = false;
                ++dialog_currentPhraseIndex;
                ref readonly DialogOptionConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);

                if (dialog_totalPhrases > dialog_currentPhraseIndex)
                {
                    /* More phrases to say, wait for user interaction */
                    StartPhrase(dialogConfig.Phrases[dialog_currentPhraseIndex]);
                }
                else
                {
                    /* End of dialog */
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(false);

                    /* If end of conversation triggers an event */
                    if (dialogConfig.triggeredEvent != GameEvent.EVENT_NONE)
                    {
                        VARMAP_GameMenu.COMMIT_EVENT(dialogConfig.triggeredEvent, true);
                    }

                    VARMAP_GameMenu.ENABLE_DIALOGUE(false, null, DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE);
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
                _UICanvas = UICanvas;

                float menuHeight = Screen.safeArea.height * GameFixedConfig.MENU_TOP_SCREEN_HEIGHT_PERCENT;
                _upperGameMenuRect = new Rect(0, 0, Screen.safeArea.width, menuHeight);
                _gameMenuToolbarStrings = new string[] { "Save Game", "Exit Game" };

                UICanvas_dialogOptionButtons = new DialogOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];
                _displayItemArray = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];

                dialog_current_talkers = new GameItem[GameFixedConfig.MAX_DIALOG_TALKERS];
            }
        }

        

        void Start()
        {
            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
            VARMAP_GameMenu.REG_GAMESTATUS(_OnGameStatusChanged);

            _mainCamera = Camera.main;

            _ = StartCoroutine(Execute_Load_Coroutine());
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
                VARMAP_GameMenu.UNREG_GAMESTATUS(_OnGameStatusChanged);
            }
        }

        private static IEnumerator Execute_Load_Coroutine()
        {
            UICanvas_dialogObj = _UICanvas.transform.Find("DialogObj").gameObject;
            UICanvas_dialogObj_sender = UICanvas_dialogObj.transform.Find("DialogSender").GetComponent<TMP_Text>();
            UICanvas_dialogObj_msg = UICanvas_dialogObj.transform.Find("DialogMsg").GetComponent<TMP_Text>();
            UICanvas_dialogOptions = UICanvas_dialogObj.transform.Find("DialogOptions").gameObject;

            yield return new WaitForNextFrameUnit();

            for (int i = 0; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
            {
                Transform btnTransf = UICanvas_dialogOptions.transform.Find("DialogOption" + (i + 1).ToString());
                UICanvas_dialogOptionButtons[i] = btnTransf.Find("ActiveArea").gameObject.GetComponent<DialogOptionButtonClass>();
                UICanvas_dialogOptionButtons[i].SetClickDelegate(_DialogOptionSelected);
                yield return new WaitForNextFrameUnit();
            }


            UICanvas_itemMenuObj = _UICanvas.transform.Find("ItemMenuObj").gameObject;

            for (int i = 0; i < GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS; ++i)
            {
                GameObject itemObj = UICanvas_itemMenuObj.transform.Find("Item" + (i + 1)).Find("Item").gameObject;
                _displayItemArray[i] = itemObj.GetComponent<PickableItemDisplayClass>();
                _displayItemArray[i].SetCallFunction(OnItemDisplayClick);
                yield return new WaitForNextFrameUnit();
            }

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
                    for (; (lastFoundItemIndex < totalarrayItems) && (!found); lastFoundItemIndex++)
                    {
                        /* If this element has to show a picked item */
                        if (item_owner[lastFoundItemIndex] == selectedChar)
                        {
                            GameItem gitem = ItemsInteractionsClass.GetItemFromPickable((GamePickableItem)lastFoundItemIndex);

                            _displayItemArray[i].Enable(true);
                            _displayItemArray[i].SetDisplayedItem(gitem);
                            found = true;
                        }
                    }
                }

                if(!found)
                {
                    /* Otherwise keep hidden */
                    _displayItemArray[i].Enable(false);
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
  

        private static void _OnGameStatusChanged(ChangedEventType evtype, in Game_Status oldVal, in Game_Status newVal)
        {
            _ = evtype;

            if (newVal != oldVal)
            {
                switch(newVal)
                {
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        /* Populate menu */
                        RefreshItemMenuElements();
                        _menuOpened = true;
                        break;
                    default:
                        break;
                }

                switch(oldVal)
                {
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        _menuOpened = false;
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DIALOG:
                        /* If dialog was in progress, stop it */
                        if (dialog_coroutine != null)
                        {
                            _singleton.StopCoroutine(dialog_coroutine);    
                        }

                        dialog_coroutine = null;
                        dialog_tellingInProgress = false;
                        dialog_optionPending = false;
                        break;
                }
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