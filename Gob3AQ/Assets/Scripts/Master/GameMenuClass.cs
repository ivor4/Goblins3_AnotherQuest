﻿using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Gob3AQ.GameMenu
{

    [System.Serializable]
    public class GameMenuClass : MonoBehaviour
    {
        private enum DialogCoroutineTaskType
        {
            DIALOG_TASK_NONE,
            DIALOG_TASK_START,
            DIALOG_TASK_ENDPHRASE
        }

        [SerializeField]
        private GameObject UICanvas;

        private static GameMenuClass _singleton;
        private bool _menuOpened;
        
        private Rect _upperGameMenuRect;
        
        private string[] _gameMenuToolbarStrings;

        private UICanvasClass _uicanvas_cls;

        private GameItem[] dialog_input_talkers;
        private DialogType dialog_input_type;
        private DialogPhrase dialog_input_phrase;

        private int dialog_currentPhraseIndex;
        private int dialog_totalPhrases;
        private DialogOption dialog_optionPhrases;
        private bool dialog_optionPending;
        private bool dialog_tellingInProgress;
        private Coroutine dialog_main_coroutine;
        private WaitUntil yield_custom;
        private DialogCoroutineTaskType dialog_actualTaskType;
        private WaitForSeconds yield_2s;


        public static void CancelPickableItemService()
        {
            VARMAP_GameMenu.SET_PICKABLE_ITEM_CHOSEN(GameItem.ITEM_NONE);
        }

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
           if(_singleton != null)
           {
                /* Copy default talkers to array */
                defaultTalkers.CopyTo(_singleton.dialog_input_talkers);
                _singleton.dialog_input_type = dialog;
                _singleton.dialog_input_phrase = phrase;
                _singleton.dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_START;
           }
        }

        private void ShowDialogueExec(DialogType dialog, DialogPhrase phrase)
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

                        _uicanvas_cls.ActivateDialogOption(selectablePhrases, true, dialogOptions[i], optionPhraseContent.message);

                        uniquePhrase = dialogPhrases[0];
                        uniqueOption = dialogOptions[i];
                        ++selectablePhrases;
                    }
                }

                /* Clear previous usage data and deactivate */
                for (int i = selectablePhrases; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
                {
                    _uicanvas_cls.ActivateDialogOption(i, false, DialogOption.DIALOG_OPTION_NONE, string.Empty);
                }
            }



            /* Chose between default talkers or imposed */

            if (dialogConfig.Talkers[0] != GameItem.ITEM_NONE)
            {
                dialogConfig.Talkers.CopyTo(dialog_input_talkers);
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectablePhrases > 1)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_OPTIONS, string.Empty, string.Empty);

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
                Debug.LogError("GameMenuClass.ShowDialogueService: No valid dialog options found for dialog " + dialog.ToString());
            }
        }

        private void OnDialogOptionClick(DialogOption option)
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

        private void OnInventoryItemClick(GameItem item)
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

                    VARMAP_GameMenu.SET_PICKABLE_ITEM_CHOSEN(item);
                }
            }
        }

        private void StartDialogue(DialogOption option, int totalPhrases)
        {
            dialog_optionPhrases = option;
            dialog_totalPhrases = totalPhrases;
            dialog_currentPhraseIndex = 0;
        }

        private void StartPhrase(DialogPhrase phrase)
        {
            /* Preset */
            dialog_tellingInProgress = true;

            ref readonly PhraseContent content = ref ResourceDialogsClass.GetPhraseContent(phrase);
            GameItem talkerItem = dialog_input_talkers[content.config.talkerIndex];
            ref readonly ItemInfo talkerInfo = ref ItemsInteractionsClass.GetItemInfo(talkerItem);

            string sender = ResourceDialogsClass.GetName(talkerInfo.name);
            string msg = content.message;

            _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_PHRASE, sender, msg);

            dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_ENDPHRASE;
        }



        private void EndPhrase_Action()
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
                    _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_NONE, string.Empty, string.Empty);

                    /* If end of conversation triggers an event */
                    if (dialogConfig.triggeredEvent != GameEvent.EVENT_NONE)
                    {
                        VARMAP_GameMenu.COMMIT_EVENT(dialogConfig.triggeredEvent, true);
                    }

                    VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
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

                float menuHeight = Screen.safeArea.height * GameFixedConfig.MENU_TOP_SCREEN_HEIGHT_PERCENT;
                _upperGameMenuRect = new Rect(0, 0, Screen.safeArea.width, menuHeight);
                _gameMenuToolbarStrings = new string[] { "Save Game", "Exit Game" };
                dialog_input_talkers = new GameItem[GameFixedConfig.MAX_DIALOG_TALKERS];

                yield_custom = new WaitUntil(WaitUntilCondition);
                yield_2s = new WaitForSeconds(2f);
                dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_NONE;
            }
        }

        

        void Start()
        {
            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
            VARMAP_GameMenu.REG_GAMESTATUS(_OnGameStatusChanged);

            _uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _ = StartCoroutine(LoadCoroutine());
        }

        private IEnumerator LoadCoroutine()
        {
            Coroutine uicoroutine = StartCoroutine(_uicanvas_cls.Execute_Load_Coroutine(OnDialogOptionClick, OnInventoryItemClick));
            yield return uicoroutine;

            /* This is as a deferred Update function */
            dialog_main_coroutine = StartCoroutine(UpdateDialog_Coroutine());

            VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
        }

        private bool WaitUntilCondition()
        {
            return dialog_actualTaskType != DialogCoroutineTaskType.DIALOG_TASK_NONE;
        }

        /// <summary>
        /// Periodic Task, but interrupted when no active dialog
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateDialog_Coroutine()
        {
            while(true)
            {
                switch(dialog_actualTaskType)
                {
                    case DialogCoroutineTaskType.DIALOG_TASK_START:
                        dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_NONE;
                        ShowDialogueExec(dialog_input_type, dialog_input_phrase);
                        break;

                    case DialogCoroutineTaskType.DIALOG_TASK_ENDPHRASE:
                        dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_NONE;
                        yield return yield_2s;
                        EndPhrase_Action();
                        break;

                    default:
                        dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_NONE;
                        break;
                }

                yield return yield_custom;
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

                if (dialog_main_coroutine != null)
                {
                    StopCoroutine(dialog_main_coroutine);
                }

                VARMAP_GameMenu.UNREG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
                VARMAP_GameMenu.UNREG_GAMESTATUS(_OnGameStatusChanged);
            }
        }

        




        private void RefreshItemMenuElements()
        {
            ReadOnlySpan<CharacterType> item_owner = VARMAP_GameMenu.GET_ARRAY_PICKABLE_ITEM_OWNER();
            CharacterType selectedChar = VARMAP_GameMenu.GET_PLAYER_SELECTED();


            int totalarrayItems = item_owner.Length;
            int lastFoundItemIndex = 0;


            /* Fill all spots with first available item */
            for(int i = 0; i < GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS; i++)
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
                            _uicanvas_cls.ActivateInventoryItem(i, true, gitem);
                            found = true;
                        }
                    }
                }

                if(!found)
                {
                    /* Otherwise keep hidden */
                    _uicanvas_cls.ActivateInventoryItem(i, false, GameItem.ITEM_NONE);
                }
                
            }
        }


        private void _OnItemOwnerChanged(ChangedEventType evtype, in CharacterType oldVal, in CharacterType newVal)
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
  

        private void _OnGameStatusChanged(ChangedEventType evtype, in Game_Status oldVal, in Game_Status newVal)
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
                        dialog_actualTaskType = DialogCoroutineTaskType.DIALOG_TASK_NONE;
                        dialog_tellingInProgress = false;
                        dialog_optionPending = false;
                        break;
                }
            }
        }

        

    }
}