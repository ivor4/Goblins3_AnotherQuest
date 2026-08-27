using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Decision;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.MementoItem;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gob3AQ.GameMenu.UICanvas
{
    public delegate void MENU_BUTTON_CLICK_DELEGATE(MenuButtonType type);
    public enum DisplayMode
    {
        DISPLAY_MODE_NONE = -1,
        DISPLAY_MODE_INVENTORY,
        DISPLAY_MODE_DIALOG,
        DISPLAY_MODE_DECISION,
        DISPLAY_MODE_MEMENTO,
        DISPLAY_MODE_DETAIL,
        DISPLAY_MODE_CARDS,
        DISPLAY_MODE_CHAPTER,
        DISPLAY_MODE_LOADING
    }

    public enum DialogMode
    {
        DIALOG_MODE_NONE = -1,
        DIALOG_MODE_OPTIONS,
        DIALOG_MODE_PHRASE,
        DIALOG_MODE_BACKGROUND
    }

    public enum MenuButtonType
    {
        MENU_BUTTON_NONE = -1,
        MENU_BUTTON_SAVE,
        MENU_BUTTON_EXIT,
        MENU_BUTTON_MEMENTO,
        MENU_BUTTON_GIVEUP,
        MENU_BUTTON_TAKE,
        MENU_BUTTON_TALK,
        MENU_BUTTON_OBSERVE,
        MENU_BUTTON_DETAIL_RETURN
    }

    public class UICanvasClass : MonoBehaviour
    {
        private const string SEPARATOR = "\n\n_____________________________\n\n";
        private static readonly Color CLEAR_WHITE = new Color(1f, 1f, 1f, 0f);

        private GameObject UICanvas_loadingObj;
        private GameObject UICanvas_dialogObj;
        private GameObject UICanvas_decisionObj;
        private GameObject UICanvas_mementoObj;
        private GameObject UICanvas_itemMenuObj;
        private GameObject UICanvas_detailObj;
        private GameObject UICanvas_cardObj;
        private GameObject UICanvas_chapterObj;

        private Image UICanvas_dialogObj_background;
        private TMP_Text UICanvas_dialogObj_sender;
        private TMP_Text UICanvas_dialogObj_msg;
        private GameObject UICanvas_dialogOptions;
        private DialogOptionButtonClass[] UICanvas_dialogOptionButtons;
        private PickableItemDisplayClass[] UICanvas_inventoryButtons;

        private TMP_Text UICanvas_chapterObj_chapterNo;
        private TMP_Text UICanvas_chapterObj_chapterName;

        private Image UICanvas_loadingObj_image;
        private Sprite UICanvas_loadingObj_image_initial;


        private GameObject UICanvas_decisionOptions;
        private DecisionOptionButtonClass[] UICanvas_decisionOptionButtons;
        private RectTransform UICanvas_deicsionObj_rect;

        private GraphicRaycaster raycaster;

        private GameObject cursor;
        private GameObject cursor_subobj;
        private GameObject cursor_textobj;
        private RectTransform cursor_text_rect;
        private float cursor_text_rect_initialX;
        private GameObject cursor_userInteractionSel;
        private UIUserInteractionSelClass cursor_userInteraction_cls;
        private RectTransform cursor_rect;

        private Image cursor_spr;
        private Image cursor_subobj_spr;
        private TMP_Text cursor_textobj_text;

        private GameObject UICanvas_uppertoolbarObj;
        private Button tool_saveButton;
        private Button tool_exitButton;
        private Button tool_mementoButton;
        private GameObject tool_giveUpButtonMono;
        private Button tool_giveUpButton;
        private Button tool_takeButton;
        private Button tool_talkButton;
        private Button tool_observeButton;
        private Image tool_takeButton_img;
        private Image tool_talkButton_img;
        private Image tool_observeButton_img;

        private Button inventory_itemsTabButton;
        private Button inventory_ideasTabButton;
        private Button inventory_rightButton;
        private Button inventory_leftButton;


        private GameObject memento_itemsContentObj;
        private Image memento_largeIcon;
        private GameObject memento_largeIconTick;
        private TMP_Text memento_descrText;
        private RectTransform memento_itemsContentRectTransform;
        private MementoParent memento_selectedItem;
        private MementoItemClass[] memento_itemClass;
        private List<MementoParent> memento_unlocked_parents_list;
        private Dictionary<MementoParent, MementoItemClass> memento_parent_dict;

        private GameObject detailObj_instance;
        private Button detail_returnButton;

        private StringBuilder stringBuilder;



        private void Awake()
        {
            UICanvas_dialogOptionButtons = new DialogOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];
            UICanvas_decisionOptionButtons = new DecisionOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];
            UICanvas_inventoryButtons = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];
            raycaster = GetComponent<GraphicRaycaster>();

            UICanvas_loadingObj = transform.Find("LoadingObj").gameObject;
            UICanvas_dialogObj = transform.Find("DialogObj").gameObject;
            UICanvas_decisionObj = transform.Find("DecisionObj").gameObject;
            UICanvas_mementoObj = transform.Find("MementoObj").gameObject;
            UICanvas_itemMenuObj = transform.Find("ItemMenuObj").gameObject;
            UICanvas_detailObj = transform.Find("DetailObj").gameObject;
            UICanvas_cardObj = transform.Find("CardObj").gameObject;
            UICanvas_chapterObj = transform.Find("ChapterObj").gameObject;

            UICanvas_chapterObj_chapterNo = UICanvas_chapterObj.transform.Find("ChapterText").GetComponent<TMP_Text>();
            UICanvas_chapterObj_chapterName = UICanvas_chapterObj.transform.Find("ChapterTitleText").GetComponent<TMP_Text>();

            UICanvas_loadingObj_image = UICanvas_loadingObj.GetComponent<Image>();
            UICanvas_loadingObj_image_initial = UICanvas_loadingObj_image.sprite;

            UICanvas_dialogObj_background = UICanvas_dialogObj.GetComponent<Image>();
            UICanvas_dialogObj_sender = UICanvas_dialogObj.transform.Find("DialogSender").GetComponent<TMP_Text>();
            UICanvas_dialogObj_msg = UICanvas_dialogObj.transform.Find("DialogMsg").GetComponent<TMP_Text>();
            UICanvas_dialogOptions = UICanvas_dialogObj.transform.Find("DialogOptions").gameObject;

            UICanvas_decisionOptions = UICanvas_decisionObj.transform.Find("DecisionOptions").gameObject;
            UICanvas_deicsionObj_rect = UICanvas_decisionObj.GetComponent<RectTransform>();

            cursor = transform.Find("Cursor").gameObject;
            cursor_spr = cursor.GetComponent<Image>();
            cursor_rect = cursor.GetComponent<RectTransform>();
            cursor_subobj = cursor.transform.Find("CursorObject").gameObject;
            cursor_subobj_spr = cursor_subobj.GetComponent<Image>();
            cursor_textobj = cursor.transform.Find("CursorText").gameObject;
            cursor_text_rect = cursor_textobj.GetComponent<RectTransform>();
            cursor_text_rect_initialX = cursor_text_rect.localPosition.x;
            cursor_textobj_text = cursor_textobj.transform.Find("Text").gameObject.GetComponent<TMP_Text>();
            cursor_userInteractionSel = cursor.transform.Find("UserInteractionSel").gameObject;
            cursor_userInteraction_cls = cursor_userInteractionSel.GetComponent<UIUserInteractionSelClass>();

            UICanvas_uppertoolbarObj = transform.Find("UpperToolbar").gameObject;
            tool_saveButton = UICanvas_uppertoolbarObj.transform.Find("SaveButton").GetComponent<Button>();
            tool_exitButton = UICanvas_uppertoolbarObj.transform.Find("ExitButton").GetComponent<Button>();
            tool_mementoButton = UICanvas_uppertoolbarObj.transform.Find("MementoButton").GetComponent<Button>();
            tool_giveUpButtonMono = UICanvas_uppertoolbarObj.transform.Find("GiveUpButton").gameObject;
            tool_giveUpButton = tool_giveUpButtonMono.GetComponent<Button>();
            tool_takeButton = UICanvas_uppertoolbarObj.transform.Find("TakeButton").GetComponent<Button>();
            tool_talkButton = UICanvas_uppertoolbarObj.transform.Find("TalkButton").GetComponent<Button>();
            tool_observeButton = UICanvas_uppertoolbarObj.transform.Find("ObserveButton").GetComponent<Button>();
            tool_takeButton_img = tool_takeButton.gameObject.GetComponent<Image>();
            tool_talkButton_img = tool_talkButton.gameObject.GetComponent<Image>();
            tool_observeButton_img = tool_observeButton.gameObject.GetComponent<Image>();

            inventory_itemsTabButton = UICanvas_itemMenuObj.transform.Find("ItemTabButton").GetComponent<Button>();
            inventory_ideasTabButton = UICanvas_itemMenuObj.transform.Find("IdeaTabButton").GetComponent<Button>();
            inventory_rightButton = UICanvas_itemMenuObj.transform.Find("RightButton").GetComponent<Button>();
            inventory_leftButton = UICanvas_itemMenuObj.transform.Find("LeftButton").GetComponent<Button>();

            memento_itemsContentObj = UICanvas_mementoObj.transform.Find("MementoList/Viewport/Content").gameObject;
            memento_descrText = UICanvas_mementoObj.transform.Find("MementoDescr/Viewport/Content/DescrText").GetComponent<TMP_Text>();
            memento_largeIcon = UICanvas_mementoObj.transform.Find("MementoDescr/Viewport/Content/Icon").GetComponent<Image>();
            memento_largeIconTick = UICanvas_mementoObj.transform.Find("MementoDescr/Viewport/Content/Icon/Completed").gameObject;
            memento_largeIconTick.GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_TICK);
            memento_itemsContentRectTransform = memento_itemsContentObj.GetComponent<RectTransform>();
            memento_unlocked_parents_list = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_parent_dict = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_itemClass = new MementoItemClass[(int)MementoParent.MEMENTO_PARENT_TOTAL];

            detail_returnButton = UICanvas_detailObj.transform.Find("ReturnButton").GetComponent<Button>();

            /* Will be enabled at the end of Loading (new display mode) */
            raycaster.enabled = false;

            memento_selectedItem = MementoParent.MEMENTO_PARENT_NONE;

            stringBuilder = new(512);

            memento_largeIconTick.SetActive(false);
            memento_largeIcon.gameObject.SetActive(false);
        }

        public void Show_Hide_Toolbar(bool show)
        {
            UICanvas_uppertoolbarObj.SetActive(show);
        }

        public void SetDisplayMode(DisplayMode mode)
        {
            /* On every change of dispaly mode, abort any animation of User Interaction change and hide related objects */
            cursor_userInteraction_cls.Disable();
            raycaster.enabled = true;
            DestroyDetailPrefab();

            switch (mode)
            {
                case DisplayMode.DISPLAY_MODE_INVENTORY:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(true);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);

                    break;

                case DisplayMode.DISPLAY_MODE_DIALOG:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_DECISION:
                    UICanvas_decisionObj.SetActive(true);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_MEMENTO:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(true);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_DETAIL:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(true);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_LOADING:
                    UICanvas_loadingObj.SetActive(true);
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_CARDS:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(true);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(true);
                    break;

                case DisplayMode.DISPLAY_MODE_CHAPTER:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(true);
                    tool_giveUpButtonMono.SetActive(false);
                    break;

                default:
                    UICanvas_decisionObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    UICanvas_detailObj.SetActive(false);
                    UICanvas_cardObj.SetActive(false);
                    UICanvas_chapterObj.SetActive(false);
                    tool_giveUpButtonMono.SetActive(false);
                    break;
            }
        }

        public void SetDialogMode(DialogMode mode, string sender, string msg)
        {
            switch (mode)
            {
                case DialogMode.DIALOG_MODE_OPTIONS:
                    UICanvas_dialogObj_background.enabled = false;
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(true);
                    break;
                case DialogMode.DIALOG_MODE_PHRASE:
                    UICanvas_dialogObj_background.enabled = false;
                    UICanvas_dialogObj_sender.gameObject.SetActive(msg.Length > 0);
                    UICanvas_dialogObj_msg.gameObject.SetActive(msg.Length > 0);
                    UICanvas_dialogOptions.SetActive(false);

                    UICanvas_dialogObj_sender.text = sender;
                    UICanvas_dialogObj_msg.text = msg;
                    break;

                case DialogMode.DIALOG_MODE_BACKGROUND:
                    UICanvas_dialogObj_background.enabled = false;
                    UICanvas_dialogObj_sender.gameObject.SetActive(msg.Length > 0);
                    UICanvas_dialogObj_msg.gameObject.SetActive(msg.Length > 0);
                    UICanvas_dialogOptions.SetActive(false);

                    UICanvas_dialogObj_sender.text = sender;
                    UICanvas_dialogObj_msg.text = msg;
                    break;

                default:
                    UICanvas_dialogObj_background.enabled = false;
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(false);
                    break;
            }
        }

        public GameObject SetDetailPrefab(GameObject prefab)
        {
            GameObject retVal = null;

            DestroyDetailPrefab();

            if (prefab != null)
            {
                detailObj_instance = Instantiate(prefab, UICanvas_detailObj.transform, false);
                retVal = detailObj_instance;
            }

            return retVal;
        }


        private void DestroyDetailPrefab()
        {
            if(detailObj_instance != null)
            {
                Destroy(detailObj_instance);
                detailObj_instance = null;
            }
        }

        public void ShowCursor(bool show)
        {
            cursor.SetActive(show);
        }

        public void MoveCursor(Vector2 pos)
        {
            cursor.transform.position = pos;
        }

        public void SetCursorBaseSprite(GameSprite spriteID)
        {
            if ((spriteID == GameSprite.SPRITE_CURSOR_DRAG) || (spriteID == GameSprite.SPRITE_UI_CURSOR_DOOR))
            {
                cursor_rect.pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                cursor_rect.pivot = new Vector2(0f, 1f);
            }

            cursor_spr.sprite = ResourceSpritesClass.GetSprite(spriteID);
        }

        public void SetCursorItem(GameItem item)
        {
            if (item == GameItem.ITEM_NONE)
            {
                cursor_subobj.SetActive(false);
                cursor_subobj_spr.sprite = null;
            }
            else
            {
                ref readonly ItemInfo info = ref ItemsInteractionsClass.GetItemInfo(item);
                GameSprite sprID;
                sprID = info.pickableSprite;

                cursor_subobj_spr.sprite = ResourceSpritesClass.GetSprite(sprID);
                cursor_subobj.SetActive(true);
            }
        }

        public void SetCursorLabel(bool active, string labelName)
        {
            ref readonly MousePropertiesStruct mouseProps = ref VARMAP_GameMenu.GET_MOUSE_PROPERTIES();

            if (!active)
            {
                cursor_textobj.SetActive(false);
            }
            else
            {
                cursor_textobj_text.text = labelName;
                cursor_textobj.SetActive(true);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(cursor_text_rect);

            if((mouseProps.posPixels.x + cursor_text_rect.sizeDelta.x + 100) > Screen.width)
            {
                cursor_text_rect.localPosition = new Vector2(-cursor_text_rect_initialX*2f, cursor_text_rect.localPosition.y);
            }
            else
            {
                cursor_text_rect.localPosition = new Vector2(cursor_text_rect_initialX, cursor_text_rect.localPosition.y);
            }
        }

        public void SetUserInteraction(UserInputInteraction interaction)
        {
            switch(interaction)
            {
                case UserInputInteraction.INPUT_INTERACTION_TAKE:
                    tool_takeButton_img.color = Color.white;
                    tool_talkButton_img.color = Color.gray;
                    tool_observeButton_img.color = Color.gray;
                    break;
                case UserInputInteraction.INPUT_INTERACTION_TALK:
                    tool_takeButton_img.color = Color.gray;
                    tool_talkButton_img.color = Color.white;
                    tool_observeButton_img.color = Color.gray;
                    break;
                default:
                    tool_takeButton_img.color = Color.gray;
                    tool_talkButton_img.color = Color.gray;
                    tool_observeButton_img.color = Color.white;
                    break;
            }
        }

        public void SetLoadingSprite(Sprite sprite, bool stablish, Color fadetint)
        {
            if (stablish)
            {
                UICanvas_loadingObj_image.sprite = sprite;
                UICanvas_loadingObj_image.color = fadetint;
            }
            else
            {
                UICanvas_loadingObj_image.sprite = UICanvas_loadingObj_image_initial;
                UICanvas_loadingObj_image.color = Color.black;
            }
        }

        public void HideLoadingObj()
        {
            UICanvas_loadingObj.SetActive(false);
        }

        public void SetChapterNoAndTitle(string chapterNo, string chapterTitle)
        {
            UICanvas_chapterObj_chapterNo.text = chapterNo;
            UICanvas_chapterObj_chapterName.text = chapterTitle;
        }

        public void ActivateDialogOption(int index, bool activate, DialogOption option, DialogPhrase phrase, string text)
        {
            ref readonly DialogOptionButtonClass button = ref UICanvas_dialogOptionButtons[index];

            if (activate)
            {
                button.SetDialogOption(option);
                button.SetDialogPhrase(phrase, text);
            }
            else
            {
                button.SetDialogOption(DialogOption.DIALOG_OPTION_NONE);
                button.SetDialogPhrase(DialogPhrase.PHRASE_NONE, string.Empty);
            }

            button.SetActive(activate);
        }

        public void ActivateDecisionOption(int index, bool activate, DecisionOption option, string text)
        {
            ref readonly DecisionOptionButtonClass button = ref UICanvas_decisionOptionButtons[index];
            if (activate)
            {
                button.SetDecisionOption(option);
                button.SetOptionText(in text);
            }
            else
            {
                button.SetDecisionOption(DecisionOption.DECISION_OPTION_NONE);
                button.SetOptionText(in string.Empty);
            }
            button.SetActive(activate);
        }

        public void SetDecisionNumElems(int numElems)
        {
            UICanvas_deicsionObj_rect.sizeDelta = new Vector2(UICanvas_deicsionObj_rect.sizeDelta.x,
                numElems * 50f);
        }


        public void ActivateInventoryItem(int index, bool activate, GameItem item)
        {
            ref readonly PickableItemDisplayClass inventory_obj = ref UICanvas_inventoryButtons[index];

            if (activate)
            {
                inventory_obj.SetDisplayedItem(item);
            }
            else
            {
                inventory_obj.SetDisplayedItem(GameItem.ITEM_NONE);
            }

            inventory_obj.Enable(activate);
        }

        public void SetInventoryTab(InventoryTabType tabType)
        {
            ColorBlock cblock;

            switch (tabType)
            {
                case InventoryTabType.INVENTORY_TAB_ITEMS:
                    cblock = inventory_itemsTabButton.colors;
                    cblock.normalColor = Color.white;
                    inventory_itemsTabButton.colors = cblock;

                    cblock = inventory_ideasTabButton.colors;
                    cblock.normalColor = CLEAR_WHITE;
                    inventory_ideasTabButton.colors = cblock;
                    break;

                default:
                    cblock = inventory_ideasTabButton.colors;
                    cblock.normalColor = Color.white;
                    inventory_ideasTabButton.colors = cblock;

                    cblock = inventory_itemsTabButton.colors;
                    cblock.normalColor = CLEAR_WHITE;
                    inventory_itemsTabButton.colors = cblock;
                    break;
            }
        }

        public void ActivateInventoryArrows(bool leftArrow, bool rightArrow)
        {
            inventory_leftButton.interactable = leftArrow;
            inventory_rightButton.interactable = rightArrow;
        }

        public void AnimateNewUserInteraction(UserInputInteraction interaction)
        {
            /* Passthrough */
            cursor_userInteraction_cls.AnimateNewUserInteraction(interaction);
        }

        public void MementoMenuActivated(ReadOnlySpan<MementoStatus> mementoParentStatus)
        {
            /* Clear previous icon and description */
            memento_descrText.text = string.Empty;
            memento_largeIconTick.SetActive(false);
            memento_largeIcon.gameObject.SetActive(false);

            /* Clear previous selection */
            if (memento_selectedItem != MementoParent.MEMENTO_PARENT_NONE)
            {
                memento_parent_dict[memento_selectedItem].Select(false);
                memento_selectedItem = MementoParent.MEMENTO_PARENT_NONE;
            }

            /* Refill unlocked list */
            memento_unlocked_parents_list.Clear();

            for(int i=0; i < mementoParentStatus.Length; ++i)
            {
                ref readonly MementoStatus parentStatus = ref mementoParentStatus[i];

                if(parentStatus.unlocked)
                {
                    memento_unlocked_parents_list.Add((MementoParent)i);
                }
            }

            /* Fit content to size */
            Vector2 sizeDelta = memento_itemsContentRectTransform.sizeDelta;
            sizeDelta.y = memento_unlocked_parents_list.Count * memento_itemClass[0].GetSize.y;
            memento_itemsContentRectTransform.sizeDelta = sizeDelta;

            /* Sort active parent list (the ones with a higher ID are supposed to be unlocked laster in game)
            * Therefore, later unlocked events should appear first */
            memento_unlocked_parents_list.Sort(MementoParentSortMethod);

            memento_parent_dict.Clear();

            /* Activate and give shape to items */
            for (int i = 0; i < memento_itemClass.Length; i++)
            {
                MementoItemClass instance = memento_itemClass[i];

                /* Active ones */
                if (i < memento_unlocked_parents_list.Count)
                {
                    MementoParent parent = memento_unlocked_parents_list[i];
                    ref readonly MementoStatus parentStatus = ref mementoParentStatus[(int)parent];
                    memento_parent_dict[parent] = instance;

                    instance.SetMementoParent(parent, parentStatus.completed, parentStatus.unwatched);
                    instance.Activate(true);
                }
                /* Deactivated ones */
                else
                {
                    instance.Activate(false);
                }
            }
        }



        public void MementoParentClicked(MementoParent parent, ReadOnlySpan<MementoStatus> mementoStatus, ReadOnlySpan<MementoStatus> mementoParentStatus)
        {
            MementoItemClass itemClass = memento_parent_dict[parent];
            ref readonly MementoStatus parentStatus = ref mementoParentStatus[(int)parent];

            if(memento_selectedItem != MementoParent.MEMENTO_PARENT_NONE)
            {
                memento_parent_dict[memento_selectedItem].Select(false);
            }

            itemClass.Select(true);
            memento_selectedItem = parent;

            ref readonly MementoParentInfo memParInfo = ref ItemsInteractionsClass.GetMementoParentInfo(parent);
            ReadOnlySpan<Memento> children = memParInfo.Children;

            stringBuilder.Clear();
            bool addedElement = false;
            bool completed = false;

            for (int i = 0; i < children.Length; ++i)
            {
                Memento memento = children[i];
                ref readonly MementoStatus childStatus = ref mementoStatus[(int)memento];

                if (childStatus.unlocked)
                {
                    if (addedElement && (i > 0))
                    {
                        stringBuilder.Append(SEPARATOR);
                    }

                    ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(memento);
                    completed |= memInfo.final;
                    ResourceDialogsClass.GetPhraseContent(memInfo.phrase, out PhraseContent phraseContent);
                    stringBuilder.Append(phraseContent.message);
                    addedElement = true;
                }
            }

            memento_descrText.text = stringBuilder.ToString();
            memento_largeIcon.sprite = ResourceSpritesClass.GetSprite(memParInfo.sprite);
            memento_largeIcon.gameObject.SetActive(true);
            memento_largeIconTick.SetActive(completed);
        }





        public IEnumerator Execute_Load_Coroutine(DIALOG_OPTION_CLICK_DELEGATE OnDialogOptionClick,
            DECISION_OPTION_CLICK_DELEGATE OnDecisionOptionClick,
            DISPLAYED_ITEM_CLICK OnItemDisplayClick,
            DISPLAYED_ITEM_HOVER OnHover,
            INVENTORY_TAB_CLICK OnInventoryTabClick,
            INVENTORY_ARROW_CLICK OnInventoryArrowClick,
            MENU_BUTTON_CLICK_DELEGATE OnMenuButtonClick,
            MEMENTO_ITEM_CLICK_DELEGATE OnMementoItemClick
            )
        {
            bool sprites_loaded = false;

            tool_saveButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_SAVE));
            tool_exitButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_EXIT));
            tool_mementoButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_MEMENTO));
            tool_giveUpButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_GIVEUP));
            tool_takeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TAKE));
            tool_talkButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TALK));
            tool_observeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_OBSERVE));
            inventory_itemsTabButton.onClick.AddListener(() => OnInventoryTabClick(InventoryTabType.INVENTORY_TAB_ITEMS));
            inventory_ideasTabButton.onClick.AddListener(() => OnInventoryTabClick(InventoryTabType.INVENTORY_TAB_IDEAS));
            inventory_leftButton.onClick.AddListener(() => OnInventoryArrowClick(true));
            inventory_rightButton.onClick.AddListener(() => OnInventoryArrowClick(false));
            detail_returnButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_DETAIL_RETURN));


            for (int i = 0; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
            {
                Transform btnTransf = UICanvas_dialogOptions.transform.Find("DialogOption" + (i + 1).ToString());
                UICanvas_dialogOptionButtons[i] = btnTransf.Find("ActiveArea").gameObject.GetComponent<DialogOptionButtonClass>();
                UICanvas_dialogOptionButtons[i].SetClickDelegate(OnDialogOptionClick);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            for (int i = 0; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
            {
                Transform btnTransf = UICanvas_decisionOptions.transform.Find("DecisionOption" + (i + 1).ToString());
                UICanvas_decisionOptionButtons[i] = btnTransf.Find("ActiveArea").gameObject.GetComponent<DecisionOptionButtonClass>();
                UICanvas_decisionOptionButtons[i].SetClickDelegate(OnDecisionOptionClick);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }


            for (int i = 0; i < GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS; ++i)
            {
                GameObject itemObj = UICanvas_itemMenuObj.transform.Find("Item" + (i + 1)).Find("Item").gameObject;
                UICanvas_inventoryButtons[i] = itemObj.GetComponent<PickableItemDisplayClass>();
                UICanvas_inventoryButtons[i].SetOnClickCallFunction(OnItemDisplayClick);
                UICanvas_inventoryButtons[i].SetHoverCallFunction(OnHover);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            /* Wait for GameMaster Load */
            while (!sprites_loaded)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_GameMenu.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out sprites_loaded);
            }

            UICanvas_itemMenuObj.GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY);

            inventory_itemsTabButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY_SELECTED_TAB);
            inventory_ideasTabButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY_SELECTED_TAB);
            inventory_rightButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY_ARROW);
            inventory_leftButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY_ARROW);

            inventory_itemsTabButton.transform.Find("Icon").GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_INVENTORY_ITEM);
            inventory_ideasTabButton.transform.Find("Icon").GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_INVENTORY_IDEA);

            SetInventoryTab(InventoryTabType.INVENTORY_TAB_ITEMS);

            tool_takeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TAKE);
            tool_talkButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TALK);
            tool_observeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_OBSERVE);

            cursor_userInteraction_cls.LoadTask();
            yield return ResourceAtlasClass.WaitForNextFrame;

            /* Fill memento loaded list */


            /* Load memento */
            UICanvas_mementoObj.GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_BOOK_MEMENTOS);

            GameObject memento_item_prefab = ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_MEMENTO_ITEM);

            AsyncInstantiateOperation<GameObject> handle = InstantiateAsync<GameObject>(memento_item_prefab, (int)MementoParent.MEMENTO_PARENT_TOTAL);
            yield return handle;

            GameObject[] memento_itemObj = handle.Result;
            
            /* Keep them ready for usage */
            for (int i=0; i < memento_itemObj.Length; ++i)
            {
                memento_itemObj[i].transform.SetParent(memento_itemsContentObj.transform, false);
                memento_itemClass[i] = memento_itemObj[i].GetComponent<MementoItemClass>();
                MementoItemClass itemClass = memento_itemClass[i];

                stringBuilder.Clear();
                stringBuilder.Append("item");
                stringBuilder.Append(i);

                itemClass.InitialSetup(i, OnMementoItemClick, stringBuilder.ToString());

                if ((i & 0xF) == 0xF)
                {
                    yield return ResourceAtlasClass.WaitForNextFrame;
                }
            }
        }

        private static int MementoParentSortMethod(MementoParent a, MementoParent b)
        {
            return (int)a - (int)b;
        }

    }
}
