using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.DetailActiveElem;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceDecisionsAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.GameMenu
{

    [System.Serializable]
    public class GameMenuClass : MonoBehaviour
    {
        private enum DialogTaskType
        {
            DIALOG_STATE_NONE,
            DIALOG_STATE_STARTING,
            DIALOG_STATE_SAYING,
            DIALOG_STATE_DEAD_TIME,
            DIALOG_STATE_WAITING_EVENTS
        }

        private enum DecisionTaskType
        {
            DECISION_TASK_NONE,
            DECISION_TASK_STARTING,
            DECISION_TASK_DECIDING,
            DECISION_TASK_ENDING
        }

        [SerializeField]
        private GameObject UICanvas;

        private static GameMenuClass _singleton;
        private bool _itemMenuOpened;
        private float _lastClickTimestamp;
        

        private UICanvasClass _uicanvas_cls;

        private GameItem[] dialog_input_talkers;
        private DialogType dialog_input_type;
        private DialogPhrase dialog_input_phrase;
        private bool dialog_input_backgroundDialog;

        private int dialog_currentPhraseIndex;
        private int dialog_totalPhrases;
        private DialogOption dialog_optionPhrases;
        private bool dialog_optionPending;
        private bool dialog_tellingInProgress;
        private bool dialog_background;

        private bool decision_optionPending;
        private DecisionType decision_input_type;

        private DialogTaskType dialog_actualTaskType;
        private DecisionTaskType decision_actualTaskType;

        private HashSet<MementoCombi> memento_combi_union;
        private HashSet<MementoCombi> memento_combi_intersection;

        private DetailType detail_loaded;

        private ulong dialog_timestamp;
        private Dictionary<DialogOption, List<byte>> dialog_randomized_left_indexes;


        public static void CommitMementoNotifService(Memento memento)
        {
            if(_singleton != null)
            {
                _singleton._uicanvas_cls.NewMementoUnlocked(memento, true, true);
            }
        }


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
        /// <param name="backgroundDialog">If background does not need user interaction (as a background conversation).</param>
        public static void ShowDialogueService(ReadOnlySpan<GameItem> defaultTalkers, DialogType dialog, DialogPhrase phrase, bool backgroundDialog)
        {
           if(_singleton != null)
           {
                /* Copy default talkers to array */
                defaultTalkers.CopyTo(_singleton.dialog_input_talkers);
                _singleton.dialog_input_type = dialog;
                _singleton.dialog_input_phrase = phrase;
                _singleton.dialog_input_backgroundDialog = backgroundDialog;
                _singleton.dialog_actualTaskType = DialogTaskType.DIALOG_STATE_STARTING;
           }
        }

        public static void ShowDecisionService(DecisionType decision)
        {
            if (_singleton != null)
            {
                _singleton.decision_input_type = decision;
                _singleton.decision_actualTaskType = DecisionTaskType.DECISION_TASK_STARTING;
            }
        }

        private void ShowDecisionExec(DecisionType decision)
        {
            ref readonly DecisionConfig decisionConfig = ref ResourceDecisionsAtlasClass.GetDecisionConfig(decision);

            for(int i = 0; i < decisionConfig.Options.Length; i++)
            {
                DecisionOption option = decisionConfig.Options[i];
                ref readonly DecisionOptionConfig decisionOptionConfig = ref ResourceDecisionsAtlasClass.GetDecisionOptionConfig(option);

                ResourceDialogsClass.GetPhraseContent(decisionOptionConfig.phrase, out PhraseContent optionPhraseContent);
                _uicanvas_cls.ActivateDecisionOption(i, true, option, optionPhraseContent.message);
            }

            /* Clear previous usage data and deactivate */
            for (int i = decisionConfig.Options.Length; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
            {
                _uicanvas_cls.ActivateDecisionOption(i, false, DecisionOption.DECISION_OPTION_NONE, string.Empty);
            }

            _uicanvas_cls.SetDecisionNumElems(decisionConfig.Options.Length);

            decision_optionPending = true;
        }

        private void ShowDialogueExec(DialogType dialog, DialogPhrase phrase, bool background)
        {
            int selectableOptions;

            DialogPhrase uniquePhrase = DialogPhrase.PHRASE_NONE;
            DialogOption uniqueOption = DialogOption.DIALOG_OPTION_NONE;
            int uniqueNumPhrases = 0;
            MomentType currentMoment = VARMAP_GameMenu.GET_DAY_MOMENT();

            ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogConfig(dialog);


            if (dialog == DialogType.DIALOG_SIMPLE)
            {
                uniquePhrase = phrase;
                uniqueOption = DialogOption.DIALOG_OPTION_SIMPLE;
                selectableOptions = 1;
                uniqueNumPhrases = 1;
            }
            /* A dialog with only one option should be given. But, in case, take always first option for background mode */
            /* This is inconditional, even if option would not be available due to Needed Events */
            else if(background)
            {
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;
                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[0]);

                ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                DialogPhrase headPhrase;

                if (dialogOptionConfig.randomized)
                {
                    int randomIndex = GetRandomizedOption(dialogOptions[0], in dialogOptionConfig);
                    headPhrase = dialogPhrases[randomIndex];
                    uniqueNumPhrases = 1;
                }
                else
                {
                    headPhrase = dialogPhrases[0];
                    uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                }

                uniquePhrase = headPhrase;
                uniqueOption = dialogOptions[0];

                selectableOptions = 1;
            }
            else
            {
                selectableOptions = 0;

                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                for (int i = 0; i < dialogOptions.Length; i++)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[i]);

                    VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);

                    if (valid && ((currentMoment == dialogOptionConfig.momentType) || (dialogOptionConfig.momentType == MomentType.MOMENT_ANY)))
                    {
                        ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                        DialogPhrase headPhrase;

                        if (dialogOptionConfig.randomized)
                        {
                            int randomIndex = GetRandomizedOption(dialogOptions[i], in dialogOptionConfig);
                            headPhrase = dialogPhrases[randomIndex];
                            uniqueNumPhrases = 1;
                        }
                        else
                        {
                            headPhrase = dialogPhrases[0];
                            uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                        }

                        uniquePhrase = headPhrase;
                        uniqueOption = dialogOptions[i];

                        ResourceDialogsClass.GetPhraseContent(headPhrase, out PhraseContent optionPhraseContent);
                        _uicanvas_cls.ActivateDialogOption(selectableOptions, true, dialogOptions[i], headPhrase, optionPhraseContent.message);

                        ++selectableOptions;
                    }
                }

                /* Clear previous usage data and deactivate */
                for (int i = selectableOptions; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
                {
                    _uicanvas_cls.ActivateDialogOption(i, false, DialogOption.DIALOG_OPTION_NONE, DialogPhrase.PHRASE_NONE, string.Empty);
                }
            }



            /* Chose between default talkers or imposed */

            if (dialogConfig.Talkers[0] != GameItem.ITEM_NONE)
            {
                dialogConfig.Talkers.CopyTo(dialog_input_talkers);
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectableOptions > 1)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_OPTIONS, string.Empty, string.Empty);

                dialog_optionPending = true;
                dialog_tellingInProgress = false;
            }
            else if (selectableOptions == 1)
            {
                /* Initialize phrase index */
                dialog_optionPending = false;

                StartDialogue(uniqueOption, uniqueNumPhrases, background);
                StartPhrase(uniquePhrase);
            }
            else
            {
                dialog_optionPending = false;
                dialog_tellingInProgress = false;
                Debug.LogError("GameMenuClass.ShowDialogueService: No valid dialog options found for dialog " + dialog.ToString());
            }
        }

        private void OnDialogOptionClick(DialogOption option, DialogPhrase phrase)
        {
            if ((VARMAP_GameMenu.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_DIALOG) && dialog_optionPending)
            {
                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(option);

                dialog_optionPending = false;

                /* If option is permitted, show it */
                VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);
                MomentType currentMoment = VARMAP_GameMenu.GET_DAY_MOMENT();


                if (valid && ((currentMoment == dialogOptionConfig.momentType) || (dialogOptionConfig.momentType == MomentType.MOMENT_ANY)))
                {
                    ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                    int length = dialogOptionConfig.randomized ? 1 : dialogPhrases.Length;

                    StartDialogue(option, length, false);
                    StartPhrase(phrase);
                }
            }
        }

        private void OnDecisionOptionClick(DecisionOption option)
        {
            if ((VARMAP_GameMenu.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_DECISION) && decision_optionPending)
            {
                ref readonly DecisionOptionConfig decisionOptionConfig = ref ResourceDecisionsAtlasClass.GetDecisionOptionConfig(option);
                decision_optionPending = false;

                /* Trigger linked events */
                VARMAP_GameMenu.COMMIT_EVENT(decisionOptionConfig.TriggeredEvents);

                decision_actualTaskType = DecisionTaskType.DECISION_TASK_DECIDING;
            }
        }

        public void OnInventoryItemClick(GameItem item)
        {
            if (_itemMenuOpened)
            {
                UserInputInteraction currentInteraction = VARMAP_GameMenu.GET_USER_INPUT_INTERACTION();
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);

                switch (currentInteraction)
                {
                    case UserInputInteraction.INPUT_INTERACTION_TAKE:
                        GameItem prevChoosen = VARMAP_GameMenu.GET_PICKABLE_ITEM_CHOSEN();

                        if (prevChoosen == item)
                        {
                            VARMAP_GameMenu.CANCEL_PICKABLE_ITEM();
                        }
                        else
                        {
                            if ((itemInfo.detailType != DetailType.DETAIL_NONE) && (prevChoosen != GameItem.ITEM_NONE))
                            {
                                CreateDetail(itemInfo.detailType);
                            }
                            else if(itemInfo.isPickable)
                            {
                                VARMAP_GameMenu.SET_PICKABLE_ITEM_CHOSEN(item);
                            }
                        }
                        break;

                    case UserInputInteraction.INPUT_INTERACTION_OBSERVE:
                        /* Observe in detail */
                        if (itemInfo.detailType != DetailType.DETAIL_NONE)
                        {
                            VARMAP_GameMenu.CANCEL_PICKABLE_ITEM();
                            CreateDetail(itemInfo.detailType);
                        }
                        /* Simple observation phrase */
                        else
                        {
                            CharacterType playerSelected = VARMAP_GameMenu.GET_PLAYER_SELECTED();
                            InteractionUsage usage = InteractionUsage.CreateObserveItem(playerSelected, item, -1);
                            VARMAP_GameMenu.USE_ITEM(in usage, out InteractionUsageOutcome outcome);

                            if (outcome.ok && (outcome.dialogType != DialogType.DIALOG_NONE))
                            {
                                Span<GameItem> talkers = stackalloc GameItem[2];
                                talkers[0] = (GameItem)playerSelected;
                                talkers[1] = (GameItem)playerSelected;
                                VARMAP_GameMenu.SHOW_DIALOGUE(talkers, outcome.dialogType, outcome.dialogPhrase, true);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void OnInventoryItemHover(GameItem item, bool hover)
        {
            if(_itemMenuOpened && hover)
            {
                VARMAP_GameMenu.SET_ITEM_MENU_HOVER(item);
            }
            else
            {
                VARMAP_GameMenu.SET_ITEM_MENU_HOVER(GameItem.ITEM_NONE);
            }
        }

        private void OnMenuButtonClick(MenuButtonType type)
        {
            Game_Status gstatus = VARMAP_GameMenu.GET_GAMESTATUS();

            switch(type)
            {
                case MenuButtonType.MENU_BUTTON_SAVE:
                    VARMAP_GameMenu.SAVE_GAME();
                    break;
                case MenuButtonType.MENU_BUTTON_EXIT:
                    VARMAP_GameMenu.EXIT_GAME(out _);
                    break;
                case MenuButtonType.MENU_BUTTON_MEMENTO:
                    if (gstatus == Game_Status.GAME_STATUS_PLAY)
                    {
                        VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_MEMENTO, out _);
                    }
                    else if(gstatus == Game_Status.GAME_STATUS_PLAY_MEMENTO)
                    {
                        VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                    }
                    else
                    {
                        /**/
                    }
                    break;
                case MenuButtonType.MENU_BUTTON_TAKE:
                    SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                    break;
                case MenuButtonType.MENU_BUTTON_TALK:
                    SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TALK);
                    break;
                case MenuButtonType.MENU_BUTTON_OBSERVE:
                    SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_OBSERVE);
                    break;
                case MenuButtonType.MENU_BUTTON_DETAIL_RETURN:
                    if(_itemMenuOpened)
                    {
                        DestroyLoadedDetail();
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_INVENTORY);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnMementoItemClick(MementoParent mementoParent)
        {
            /* Set as watched */
            ref readonly MementoParentInfo memParInfo = ref ItemsInteractionsClass.GetMementoParentInfo(mementoParent);
            VARMAP_GameMenu.MEMENTO_PARENT_WATCHED(mementoParent);

            /* Double click */
            float timestamp_ms = Time.time;
            bool doubleClick = DoubleClickDetect(timestamp_ms);

            /* Display and get Combined */
            _uicanvas_cls.MementoParentClicked(mementoParent, doubleClick, out ReadOnlyHashSet<MementoParent> combinedMementos);

            /* Get one of both, when 2 and check for combis */
            CheckMementoCombination(combinedMementos);
        }

        private void CreateDetail(DetailType detailType)
        {
            DestroyLoadedDetail();
            ref readonly DetailInfo dinfo = ref ItemsInteractionsClass.GetDetailInfo(detailType);
            detail_loaded = detailType;
            VARMAP_GameMenu.LOAD_ADDITIONAL_RESOURCES(true, dinfo.prefabPath, DetailLoaded);
        }

        private void DestroyLoadedDetail()
        {
            if (detail_loaded != DetailType.DETAIL_NONE)
            {
                ref readonly DetailInfo dinfo = ref ItemsInteractionsClass.GetDetailInfo(detail_loaded);
                VARMAP_GameMenu.LOAD_ADDITIONAL_RESOURCES(false, dinfo.prefabPath, null);
            }

            detail_loaded = DetailType.DETAIL_NONE;
        }

        private void DetailLoaded(GameObject prefab)
        {
            _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_DETAIL);

            GameObject createdInstance = _uicanvas_cls.SetDetailPrefab(prefab);
            IDetailScript scr = createdInstance.GetComponent<IDetailScript>();
            scr.SetItemClickAction(OnInventoryItemClick);
            scr.SetItemHoverAction(OnInventoryItemHover);

            VARMAP_GameMenu.SET_ITEM_MENU_HOVER(GameItem.ITEM_NONE);
        }

        private void StartDialogue(DialogOption option, int totalPhrases, bool background)
        {
            dialog_optionPhrases = option;
            dialog_totalPhrases = totalPhrases;
            dialog_currentPhraseIndex = 0;
            dialog_background = background;
        }

        private void StartPhrase(DialogPhrase phrase)
        {
            /* Preset */
            dialog_tellingInProgress = true;

            ResourceDialogsClass.GetPhraseContent(phrase, out PhraseContent content);
            GameItem talkerItem = dialog_input_talkers[content.config.talkerIndex];
            ItemInfo talkerItemInfo = ItemsInteractionsClass.GetItemInfo(talkerItem);
            NameType talkerName = talkerItemInfo.name;

            string sender = ResourceDialogsClass.GetName(talkerName);
            string msg = content.message;

            if(dialog_background)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_BACKGROUND, sender, msg);
            }
            else
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_PHRASE, sender, msg);
            }

            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_SAYING;
        }



        private void EndPhrase_Action()
        {
            if (dialog_tellingInProgress)
            {
                dialog_tellingInProgress = false;
                ++dialog_currentPhraseIndex;
                DialogOptionConfig dialogConfig = ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);

                if (dialog_totalPhrases > dialog_currentPhraseIndex)
                {
                    /* More phrases to say, wait for user interaction */
                    StartPhrase(dialogConfig.Phrases[dialog_currentPhraseIndex]);
                }
                else
                {
                    /* If end of conversation triggers an event */
                    VARMAP_GameMenu.COMMIT_EVENT(dialogConfig.TriggeredEvents);

                    if (dialogConfig.dialogTriggered != DialogType.DIALOG_NONE)
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_WAITING_EVENTS;
                    }
                    else
                    {
                        /* End of dialog */
                        _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_NONE, string.Empty, string.Empty);

                        if (!dialog_background)
                        {
                            VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                        }
                    }
                }
            }
        }

        private void CheckMementoCombination(ReadOnlyHashSet<MementoParent> combinedParents)
        {
            /* In theory, both should be reciprocally related, no matter which of both is examined against the other */
            MementoParent firstInvolved = MementoParent.MEMENTO_PARENT_NONE;
            MementoParent secondInvolved = MementoParent.MEMENTO_PARENT_NONE;

            if(combinedParents.Count == 2)
            {
                foreach(MementoParent parent in combinedParents)
                {
                    if (firstInvolved == MementoParent.MEMENTO_PARENT_NONE)
                    {
                        firstInvolved = parent;
                    }
                    else
                    {
                        secondInvolved = parent;
                    }
                }

                ref readonly MementoParentInfo memParInfoFirst = ref ItemsInteractionsClass.GetMementoParentInfo(firstInvolved);
                ref readonly MementoParentInfo memParInfoSecond = ref ItemsInteractionsClass.GetMementoParentInfo(secondInvolved);

                memento_combi_union.Clear();
                memento_combi_intersection.Clear();

                foreach(Memento child in memParInfoFirst.Children)
                {
                    VARMAP_GameMenu.IS_MEMENTO_UNLOCKED(child, out bool occurred, out _);

                    if(occurred)
                    {
                        ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(child);
                        memento_combi_intersection.UnionWith(memInfo.combinations);
                    }
                }

                foreach (Memento child in memParInfoSecond.Children)
                {
                    VARMAP_GameMenu.IS_MEMENTO_UNLOCKED(child, out bool occurred, out _);

                    if (occurred)
                    {
                        ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(child);
                        memento_combi_union.UnionWith(memInfo.combinations);
                    }
                }

                memento_combi_intersection.IntersectWith(memento_combi_union);
                memento_combi_intersection.Remove(MementoCombi.MEMENTO_COMBI_NONE);

                /* Take first in intersection (There should be only one, in case) */
                MementoCombi intersected = MementoCombi.MEMENTO_COMBI_NONE;

                foreach(MementoCombi combiCommon in memento_combi_intersection)
                {
                    intersected = combiCommon;
                    break;
                }

                /* How to get Char item of actual player from here */
                Span<GameItem> talkers = stackalloc GameItem[2];
                talkers[0] = GameItem.ITEM_PLAYER_MAIN;
                talkers[1] = GameItem.ITEM_PLAYER_MAIN;

                if (intersected != MementoCombi.MEMENTO_COMBI_NONE)
                {
                    /* Check if triggered event is not already triggered */
                    ref readonly MementoCombiInfo memCombiInfo = ref ItemsInteractionsClass.GetMementoCombiInfo(intersected);

                    Span<GameEventCombi> one_event = stackalloc GameEventCombi[1];
                    one_event[0] = new(memCombiInfo.triggeredEvent, false);
                    VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(one_event, out bool occurred);

                    /* Then commit */
                    if(!occurred)
                    {
                        VARMAP_GameMenu.COMMIT_EVENT(one_event);

                        ShowDialogueService(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_GREAT_IDEA_COMBI, false);
                    }
                    else
                    {
                        ShowDialogueService(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_ALREADY_COMBI, false);
                    }
                }
                else
                {
                    ShowDialogueService(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_NONSENSE_COMBI, false);
                }

                VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_DIALOG, out _);
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
                dialog_input_talkers = new GameItem[GameFixedConfig.MAX_DIALOG_TALKERS];


                dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;

                decision_optionPending = false;
                decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;

                memento_combi_intersection = new(8);
                memento_combi_union = new(8);

                dialog_randomized_left_indexes = new();

                detail_loaded = DetailType.DETAIL_NONE;
            }
        }

        

        void Start()
        {
            VARMAP_GameMenu.REG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
            VARMAP_GameMenu.REG_GAMESTATUS(_OnGameStatusChanged);
            VARMAP_GameMenu.KEY_SUBSCRIPTION(KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION, _OnKeyPressedChanged, true);

            _uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _ = StartCoroutine(LoadCoroutine());

            _lastClickTimestamp = Time.time;
            
        }

        private IEnumerator LoadCoroutine()
        {
            Coroutine uicoroutine = StartCoroutine(_uicanvas_cls.Execute_Load_Coroutine(OnDialogOptionClick,
                OnDecisionOptionClick, OnInventoryItemClick, OnInventoryItemHover, OnMenuButtonClick, OnMementoItemClick));
            yield return uicoroutine;

            /* Preset with actual value */
            UserInputInteraction interaction = VARMAP_GameMenu.GET_SHADOW_USER_INPUT_INTERACTION();
            _uicanvas_cls.SetUserInteraction(interaction);

            VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
        }




        private void Update()
        {
            ulong actualTimestamp = VARMAP_GameMenu.GET_ELAPSED_TIME_MS();

            switch (dialog_actualTaskType)
            {
                case DialogTaskType.DIALOG_STATE_STARTING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                    ShowDialogueExec(dialog_input_type, dialog_input_phrase, dialog_input_backgroundDialog);
                    break;

                case DialogTaskType.DIALOG_STATE_SAYING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_DEAD_TIME;
                    dialog_timestamp = actualTimestamp;
                    break;

                case DialogTaskType.DIALOG_STATE_DEAD_TIME:
                    if ((actualTimestamp - dialog_timestamp) >= 2000)
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        EndPhrase_Action();
                    }
                    break;

                case DialogTaskType.DIALOG_STATE_WAITING_EVENTS:
                    if(!VARMAP_GameMenu.GET_EVENTS_BEING_PROCESSED())
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        DialogOptionConfig dialogConfig = ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);
                        ShowDialogueExec(dialogConfig.dialogTriggered, DialogPhrase.PHRASE_NONE, dialog_background);
                    }
                    break;

                default:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                    break;
            }

            switch(decision_actualTaskType)
            {
                case DecisionTaskType.DECISION_TASK_STARTING:
                    decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;
                    ShowDecisionExec(decision_input_type);
                    break;
                case DecisionTaskType.DECISION_TASK_DECIDING:
                    decision_actualTaskType = DecisionTaskType.DECISION_TASK_ENDING;
                    /* This avoid this click itself is used in next playing game cycle
                     * (user clicks this option and room object behind) */
                    break;
                case DecisionTaskType.DECISION_TASK_ENDING:
                    VARMAP_GameMenu.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                    decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;
                    break;
                default:
                    decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;
                    break;
            }
        }




        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;

                VARMAP_GameMenu.UNREG_PICKABLE_ITEM_OWNER(_OnItemOwnerChanged);
                VARMAP_GameMenu.UNREG_GAMESTATUS(_OnGameStatusChanged);
                VARMAP_GameMenu.KEY_SUBSCRIPTION(KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION, _OnKeyPressedChanged, false);
            }
        }


        private int GetRandomizedOption(DialogOption option, in DialogOptionConfig dialogOptionConfig)
        {
            int optionIndex;

            if(!dialog_randomized_left_indexes.TryGetValue(option, out List<byte> leftIndexes))
            {
                List<byte> newList = new(dialogOptionConfig.Phrases.Length);
                dialog_randomized_left_indexes[option] = newList;
                leftIndexes = newList;
            }

            if (leftIndexes.Count == 0)
            {
                /* Refill and reshuffle */
                for (byte i = 0; i < dialogOptionConfig.Phrases.Length; i++)
                {
                    int j = UnityEngine.Random.Range(0, 2);

                    if(j == 0)
                    {
                        leftIndexes.Insert(0, i);
                    }
                    else
                    {
                        leftIndexes.Add(i);
                    }
                }
            }
            optionIndex = leftIndexes[0];
            leftIndexes.RemoveAt(0);

            return optionIndex;
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

        private void SetUserInteraction(UserInputInteraction interaction)
        {
            _uicanvas_cls.SetUserInteraction(interaction);
            VARMAP_GameMenu.SET_USER_INPUT_INTERACTION(interaction);
        }


        private void _OnItemOwnerChanged(ChangedEventType evtype, in CharacterType oldVal, in CharacterType newVal)
        {
            _ = evtype;
            _ = oldVal;
            _ = newVal;

            /* Populate menu */

            if (_itemMenuOpened)
            {
                RefreshItemMenuElements();
            }
        }

        private void _OnKeyPressedChanged(KeyFunctionsIndex key, bool isPressed)
        {
            switch(key)
            {
                case KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION:
                    if(isPressed)
                    {
                        UserInputInteraction interaction = VARMAP_GameMenu.GET_SHADOW_USER_INPUT_INTERACTION();
                        int intinteraction = ((int)interaction + 1) % (int)UserInputInteraction.INPUT_INTERACTION_TOTAL;

                        interaction = (UserInputInteraction)intinteraction;

                        SetUserInteraction(interaction);
                    }
                    break;

                default:
                    break;
            }
        }

        private bool DoubleClickDetect(float timestamp_ms)
        {
            bool doubleClick;

            /* Double click */
            if (((timestamp_ms - _lastClickTimestamp) * 1000) < GameFixedConfig.DOUBLE_CLICK_MS)
            {
                doubleClick = true;

                /* Annulate posterior clicks for that timestamp - Take that further in time */
                _lastClickTimestamp -= GameFixedConfig.DOUBLE_CLICK_MS;
            }
            else
            {
                doubleClick = false;
                _lastClickTimestamp = timestamp_ms;
            }

            return doubleClick;
        }
  

        private void _OnGameStatusChanged(ChangedEventType evtype, in Game_Status oldVal, in Game_Status newVal)
        {
            _ = evtype;

            if (newVal != oldVal)
            {
                /* Set to default action (most common) */
                

                switch(newVal)
                {
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        /* Populate menu */
                        SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                        RefreshItemMenuElements();
                        _itemMenuOpened = true;
                        break;
                    case Game_Status.GAME_STATUS_PLAY_MEMENTO:
                        _uicanvas_cls.MementoMenuActivated();
                        break;
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        detail_loaded = DetailType.DETAIL_NONE;
                        SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                        _lastClickTimestamp = Time.time;
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
                        break;
                    default:
                        break;
                }

                switch(oldVal)
                {
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        DestroyLoadedDetail();
                        SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                        _itemMenuOpened = false;
                        VARMAP_GameMenu.SET_ITEM_MENU_HOVER(GameItem.ITEM_NONE);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DIALOG:
                        /* If dialog was in progress, stop it */
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        dialog_tellingInProgress = false;
                        dialog_optionPending = false;
                        break;

                    case Game_Status.GAME_STATUS_PLAY_DECISION:
                        decision_optionPending = false;
                        decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;
                        break;
                }
            }
        }

        

    }
}