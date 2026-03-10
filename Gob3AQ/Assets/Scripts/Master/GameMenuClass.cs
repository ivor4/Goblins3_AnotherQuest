using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.DetailActiveElem;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceDecisionsAtlas;
using Gob3AQ.ResourceDialogs;
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

        private bool decision_optionPending;
        private DecisionType decision_input_type;
        private DecisionTaskType decision_actualTaskType;

        private HashSet<MementoCombi> memento_combi_union;
        private HashSet<MementoCombi> memento_combi_intersection;

        private DetailType detail_loaded;

        


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

                            /* If observation brings a dialogue, unchain it if not already in a dialogue */
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

        private void OnDialogOptionClick(DialogOption option, DialogPhrase phrase)
        {
            VARMAP_GameMenu.DIALOGUE_SELECT_OPTION(option, phrase);
        }

        private void CreateDetail(DetailType detailType)
        {
            DestroyLoadedDetail();
            ref readonly DetailInfo dinfo = ref ItemsInteractionsClass.GetDetailInfo(detailType);
            detail_loaded = detailType;
            VARMAP_GameMenu.LOAD_ADDITIONAL_PREFAB(true, dinfo.prefabPath, DetailLoaded);
        }

        private void DestroyLoadedDetail()
        {
            if (detail_loaded != DetailType.DETAIL_NONE)
            {
                ref readonly DetailInfo dinfo = ref ItemsInteractionsClass.GetDetailInfo(detail_loaded);
                VARMAP_GameMenu.LOAD_ADDITIONAL_PREFAB(false, dinfo.prefabPath, null);
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

                        VARMAP_GameMenu.SHOW_DIALOGUE(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_GREAT_IDEA_COMBI, false);
                    }
                    else
                    {
                        VARMAP_GameMenu.SHOW_DIALOGUE(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_ALREADY_COMBI, false);
                    }
                }
                else
                {
                    VARMAP_GameMenu.SHOW_DIALOGUE(talkers, DialogType.DIALOG_SIMPLE, DialogPhrase.PHRASE_NONSENSE_COMBI, false);
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

                decision_optionPending = false;
                decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;

                memento_combi_intersection = new(8);
                memento_combi_union = new(8);

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

                    case Game_Status.GAME_STATUS_PLAY_DECISION:
                        decision_optionPending = false;
                        decision_actualTaskType = DecisionTaskType.DECISION_TASK_NONE;
                        break;
                }
            }
        }

        

    }
}