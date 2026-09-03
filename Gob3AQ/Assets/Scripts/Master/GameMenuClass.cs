using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.DetailActiveElem;
using Gob3AQ.GameMenu.UICanvas;
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

        private InventoryTabType inventory_tabType;
        private List<GamePickableItem> inventory_availableItems;
        private byte inventory_pageIndex;
        private byte inventory_lastPageIndex;

        private bool prevShowToolbarCommand;

        private DetailType detail_loaded;

        


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

        public static void ShowPushNotificationService(DialogPhrase notifPhrase, PushNotificationType notifType)
        {
            if (!_singleton) return;

            ResourceDialogsClass.GetPhraseContent(notifPhrase, out PhraseContent phraseContent);

            PushNotificationInfo info = new(notifType, phraseContent.message);

            _singleton._uicanvas_cls.ShowPushNotification(in info);
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
                VARMAP_GameMenu.PERFORM_ACTION(decisionOptionConfig.TriggeredActions, null);

                decision_actualTaskType = DecisionTaskType.DECISION_TASK_DECIDING;
            }
        }

        public void OnInventoryItemClick(GameItem item)
        {
            BusyState busyState = VARMAP_GameMenu.GET_BUSY_STATE();

            if (_itemMenuOpened && (busyState == BusyState.GAME_NOT_BUSY))
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
                            if ((itemInfo.detailType != DetailType.PREFAB_NONE) && itemInfo.isPrefabDetail && (prevChoosen != GameItem.ITEM_NONE))
                            {
                                CreateDetail(itemInfo.detailType);
                            }
                            else if(itemInfo.isPickable && (prevChoosen == GameItem.ITEM_NONE))
                            {
                                VARMAP_GameMenu.SET_PICKABLE_ITEM_CHOSEN(item);
                            }
                            else if(itemInfo.isPickable && (prevChoosen != GameItem.ITEM_NONE))
                            {
                                CharacterType playerSelected = VARMAP_GameMenu.GET_PLAYER_SELECTED();
                                InteractionUsage usage = InteractionUsage.CreateCombineItems(playerSelected, prevChoosen, item, -1);
                                VARMAP_GameMenu.USE_ITEM(in usage, out _);
                                VARMAP_GameMenu.CANCEL_PICKABLE_ITEM();
                            }
                            /* Detail element to manipulate with Take */
                            else
                            {
                                CharacterType playerSelected = VARMAP_GameMenu.GET_PLAYER_SELECTED();
                                InteractionUsage usage = InteractionUsage.CreateTakeItem(playerSelected, item, -1);
                                VARMAP_GameMenu.USE_ITEM(in usage, out _);
                            }
                        }
                        break;

                    case UserInputInteraction.INPUT_INTERACTION_OBSERVE:
                        /* Observe in detail */
                        if (itemInfo.detailType != DetailType.PREFAB_NONE)
                        {
                            VARMAP_GameMenu.CANCEL_PICKABLE_ITEM();
                            CreateDetail(itemInfo.detailType);
                        }
                        /* Simple observation phrase */
                        else
                        {
                            CharacterType playerSelected = VARMAP_GameMenu.GET_PLAYER_SELECTED();
                            InteractionUsage usage = InteractionUsage.CreateObserveItem(playerSelected, item, -1);
                            VARMAP_GameMenu.USE_ITEM(in usage, out _);
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

        private void OnInventoryTabClick(InventoryTabType tabType)
        {
            if ((inventory_tabType != tabType) && _itemMenuOpened)
            {
                inventory_tabType = tabType;
                inventory_pageIndex = 0;

                RefreshItemMenuElements();
            }
        }

        private void OnInventoryArrowClick(bool left)
        {
            if(_itemMenuOpened)
            {
                byte newPageIndex;


                if (left)
                {
                    newPageIndex = (byte)Math.Clamp(inventory_pageIndex - 1, 0, inventory_lastPageIndex);
                }
                else
                {
                    newPageIndex = (byte)Math.Clamp(inventory_pageIndex + 1, 0, inventory_lastPageIndex);
                }

                if (newPageIndex != inventory_pageIndex)
                {
                    inventory_pageIndex = newPageIndex;
                    RefreshItemMenuElements();
                }
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
                case MenuButtonType.MENU_BUTTON_GIVEUP:
                    if (gstatus == Game_Status.GAME_STATUS_PLAY_CARDS)
                    {
                        VARMAP_GameMenu.GIVE_UP_CARD_GAME();
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

            VARMAP_GameMenu.GET_MEMENTO_STATUS(out ReadOnlySpan<MementoStatus> mementoStatus, out ReadOnlySpan<MementoStatus> mementoParentStatus);

            /* Display */
            _uicanvas_cls.MementoParentClicked(mementoParent, mementoStatus, mementoParentStatus);
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
            if (detail_loaded != DetailType.PREFAB_NONE)
            {
                ref readonly DetailInfo dinfo = ref ItemsInteractionsClass.GetDetailInfo(detail_loaded);
                VARMAP_GameMenu.LOAD_ADDITIONAL_PREFAB(false, dinfo.prefabPath, null);
            }

            detail_loaded = DetailType.PREFAB_NONE;
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

                detail_loaded = DetailType.PREFAB_NONE;

                inventory_tabType = InventoryTabType.INVENTORY_TAB_ITEMS;

                inventory_availableItems = new((int)GamePickableItem.ITEM_PICK_TOTAL);
            }
        }

        

        void Start()
        {
            VARMAP_GameMenu.REG_GAMESTATUS(_OnGameStatusChanged);
            VARMAP_GameMenu.KEY_SUBSCRIPTION(KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION, _OnKeyPressedChanged, true);

            _uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _ = StartCoroutine(LoadCoroutine());

            _lastClickTimestamp = Time.time;

            prevShowToolbarCommand = true;


        }

        private IEnumerator LoadCoroutine()
        {
            Coroutine uicoroutine = StartCoroutine(_uicanvas_cls.Execute_Load_Coroutine(OnDialogOptionClick,
                OnDecisionOptionClick, OnInventoryItemClick, OnInventoryItemHover, OnInventoryTabClick, OnInventoryArrowClick, OnMenuButtonClick, OnMementoItemClick));
            yield return uicoroutine;

            /* Preset with actual value */
            UserInputInteraction interaction = VARMAP_GameMenu.GET_SHADOW_USER_INPUT_INTERACTION();
            _uicanvas_cls.SetUserInteraction(interaction);

            VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
        }




        private void Update()
        {
            ref readonly MousePropertiesStruct mouseProps = ref VARMAP_GameMenu.GET_MOUSE_PROPERTIES();

            bool showToolbarCommand = mouseProps.posPixels.y >= GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT * Screen.safeArea.height;

            if (showToolbarCommand != prevShowToolbarCommand)
            {
                _uicanvas_cls.Show_Hide_Toolbar(showToolbarCommand);
            }

            prevShowToolbarCommand = showToolbarCommand;

            switch (decision_actualTaskType)
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

                VARMAP_GameMenu.UNREG_GAMESTATUS(_OnGameStatusChanged);
                VARMAP_GameMenu.KEY_SUBSCRIPTION(KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION, _OnKeyPressedChanged, false);
            }
        }


        

        private void RefreshItemMenuElements()
        {
            CharacterType selectedChar = VARMAP_GameMenu.GET_PLAYER_SELECTED();
            

            _uicanvas_cls.SetInventoryTab(inventory_tabType);

            inventory_availableItems.Clear();

            /* Fill all spots with first available item */
            if (inventory_tabType == InventoryTabType.INVENTORY_TAB_ITEMS)
            {
                ReadOnlySpan<CharacterType> item_owner = VARMAP_GameMenu.GET_ARRAY_PICKABLE_ITEM_OWNER();

                for (int i = 0; i < item_owner.Length; ++i)
                {
                    if (item_owner[i] == selectedChar)
                    {
                        inventory_availableItems.Add((GamePickableItem)i);
                    }
                }
            }
            else
            {
                VARMAP_GameMenu.GET_MEMENTO_STATUS(out _, out ReadOnlySpan<MementoStatus> parentStatus);

                for (int i = 0; i < parentStatus.Length; ++i)
                {
                    ref readonly MementoStatus parentStatusInfo = ref parentStatus[i];
                    ref readonly MementoParentInfo parentInfo = ref ItemsInteractionsClass.GetMementoParentInfo((MementoParent)i);

                    if ((parentInfo.associatedItem != GameItem.ITEM_NONE) && (parentInfo.associatedChar == selectedChar) && parentStatusInfo.unlocked)
                    {
                        ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(parentInfo.associatedItem);

                        if (itemInfo.isPickable && itemInfo.isIdea)
                        {
                            inventory_availableItems.Add(itemInfo.pickableItem);
                        }
                    }
                }
            }

            int startIndex = inventory_pageIndex * GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS;

            /* for (int i = startIndex; (i < endIndex) && (i < inventory_availableItems.Count); ++i) */

            for (int i = 0; i < GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS; ++i)
            {
                int i_warp = startIndex + i;

                if ((selectedChar != CharacterType.CHARACTER_NONE) && (i_warp < inventory_availableItems.Count))
                {
                    GameItem gitem = ItemsInteractionsClass.GetItemFromPickable(inventory_availableItems[i_warp]);
                    _uicanvas_cls.ActivateInventoryItem(i, true, gitem);
                }
                else
                {
                    /* Otherwise keep hidden */
                    _uicanvas_cls.ActivateInventoryItem(i, false, GameItem.ITEM_NONE);
                }
            }

            int lastPageIndex = (inventory_availableItems.Count + (GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS - 1)) / GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS;
            lastPageIndex = Math.Max(0, lastPageIndex - 1);

            inventory_lastPageIndex = (byte)lastPageIndex;

            _uicanvas_cls.ActivateInventoryArrows(inventory_pageIndex > 0, inventory_pageIndex < inventory_lastPageIndex);
        }

        private void RefreshMementoElements()
        {
            VARMAP_GameMenu.GET_MEMENTO_STATUS(out _, out ReadOnlySpan<MementoStatus> mementoParentStatus);

            _uicanvas_cls.MementoMenuActivated(mementoParentStatus);
        }

        private void SetUserInteraction(UserInputInteraction interaction)
        {
            _uicanvas_cls.SetUserInteraction(interaction);
            VARMAP_GameMenu.SET_USER_INPUT_INTERACTION(interaction);
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
                switch (newVal)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_NONE);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_INVENTORY);
                        /* Populate menu */
                        SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                        inventory_tabType = InventoryTabType.INVENTORY_TAB_ITEMS;
                        inventory_pageIndex = 0;
                        RefreshItemMenuElements();
                        _itemMenuOpened = true;
                        break;
                    case Game_Status.GAME_STATUS_PLAY_MEMENTO:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_MEMENTO);
                        RefreshMementoElements();
                        break;
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_LOADING);
                        detail_loaded = DetailType.PREFAB_NONE;
                        SetUserInteraction(UserInputInteraction.INPUT_INTERACTION_TAKE);
                        _lastClickTimestamp = Time.time;
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_GameMenu.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMenu);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DECISION:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_DECISION);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DIALOG:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_DIALOG);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_CARDS:
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_CARDS);
                        break;
                    case Game_Status.GAME_STATUS_CHAPTER_SHOW:
                        var chapter_title = LevelOptionsClass.CHAPTER_TO_TITLE.GetValueOrDefault(VARMAP_GameMenu.GET_CHAPTER_SHOW_NR(), new Tuple<string, NameType>("UNK", NameType.NAME_CHAR_MAIN));
                        _uicanvas_cls.SetChapterNoAndTitle(chapter_title.Item1, ResourceDialogsClass.GetName(chapter_title.Item2));
                        _uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_CHAPTER);
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