using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gob3AQ.GameMenu.UICanvas
{
    public delegate void MENU_BUTTON_CLICK_DELEGATE(MenuButtonType type);
    public enum DisplayMode
    {
        DISPLAY_MODE_NONE,
        DISPLAY_MODE_INVENTORY,
        DISPLAY_MODE_DIALOG
    }

    public enum DialogMode
    {
        DIALOG_MODE_NONE,
        DIALOG_MODE_OPTIONS,
        DIALOG_MODE_PHRASE
    }

    public enum MenuButtonType
    {
        MENU_BUTTON_NONE,
        MENU_BUTTON_SAVE,
        MENU_BUTTON_EXIT,
        MENU_BUTTON_TAKE,
        MENU_BUTTON_TALK,
        MENU_BUTTON_OBSERVE
    }

    public class UICanvasClass : MonoBehaviour
    {
        private GameObject UICanvas_loadingObj;
        private GameObject UICanvas_dialogObj;
        private GameObject UICanvas_itemMenuObj;

        private TMP_Text UICanvas_dialogObj_sender;
        private TMP_Text UICanvas_dialogObj_msg;
        private GameObject UICanvas_dialogOptions;
        private DialogOptionButtonClass[] UICanvas_dialogOptionButtons;
        private PickableItemDisplayClass[] UICanvas_inventoryButtons;
        private GraphicRaycaster raycaster;

        private GameObject cursor;
        private GameObject cursor_subobj;
        private GameObject cursor_textobj;
        private GameObject cursor_userInteractionSel;
        private UIUserInteractionSelClass cursor_userInteraction_cls;
        RectTransform cursor_rect;

        private Image cursor_spr;
        private Image cursor_subobj_spr;
        private TMP_Text cursor_textobj_text;

        private GameObject UICanvas_uppertoolbarObj;
        private Button saveButton;
        private Button exitButton;
        private Button takeButton;
        private Button talkButton;
        private Button observeButton;
        private Image takeButton_img;
        private Image talkButton_img;
        private Image observeButton_img;


        private void Awake()
        {
            UICanvas_dialogOptionButtons = new DialogOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];
            UICanvas_inventoryButtons = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];
            raycaster = GetComponent<GraphicRaycaster>();

            UICanvas_loadingObj = transform.Find("LoadingObj").gameObject;
            UICanvas_dialogObj = transform.Find("DialogObj").gameObject;
            UICanvas_itemMenuObj = transform.Find("ItemMenuObj").gameObject;

            UICanvas_dialogObj_sender = UICanvas_dialogObj.transform.Find("DialogSender").GetComponent<TMP_Text>();
            UICanvas_dialogObj_msg = UICanvas_dialogObj.transform.Find("DialogMsg").GetComponent<TMP_Text>();
            UICanvas_dialogOptions = UICanvas_dialogObj.transform.Find("DialogOptions").gameObject;

            cursor = transform.Find("Cursor").gameObject;
            cursor_spr = cursor.GetComponent<Image>();
            cursor_rect = cursor.GetComponent<RectTransform>();
            cursor_subobj = cursor.transform.Find("CursorObject").gameObject;
            cursor_subobj_spr = cursor_subobj.GetComponent<Image>();
            cursor_textobj = cursor.transform.Find("CursorText").gameObject;
            cursor_textobj_text = cursor_textobj.transform.Find("Text").gameObject.GetComponent<TMP_Text>();
            cursor_userInteractionSel = cursor.transform.Find("UserInteractionSel").gameObject;
            cursor_userInteraction_cls = cursor_userInteractionSel.GetComponent<UIUserInteractionSelClass>();

            UICanvas_uppertoolbarObj = transform.Find("UpperToolbar").gameObject;
            saveButton = UICanvas_uppertoolbarObj.transform.Find("SaveButton").GetComponent<Button>();
            exitButton = UICanvas_uppertoolbarObj.transform.Find("ExitButton").GetComponent<Button>();
            takeButton = UICanvas_uppertoolbarObj.transform.Find("TakeButton").GetComponent<Button>();
            talkButton = UICanvas_uppertoolbarObj.transform.Find("TalkButton").GetComponent<Button>();
            observeButton = UICanvas_uppertoolbarObj.transform.Find("ObserveButton").GetComponent<Button>();
            takeButton_img = takeButton.gameObject.GetComponent<Image>();
            talkButton_img = talkButton.gameObject.GetComponent<Image>();
            observeButton_img = observeButton.gameObject.GetComponent<Image>();

            /* Will be enabled at the end of Loading (new display mode) */
            raycaster.enabled = false;
        }

        public void SetDisplayMode(DisplayMode mode)
        {
            /* On every change of dispaly mode, abort any animation of User Interaction change and hide related objects */
            cursor_userInteraction_cls.Disable();
            raycaster.enabled = true;

            switch (mode)
            {
                case DisplayMode.DISPLAY_MODE_INVENTORY:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(true);

                    break;

                case DisplayMode.DISPLAY_MODE_DIALOG:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(true);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;

                default:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;
            }
        }

        public void SetDialogMode(DialogMode mode, string sender, string msg)
        {
            switch (mode)
            {
                case DialogMode.DIALOG_MODE_OPTIONS:
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(true);
                    break;
                case DialogMode.DIALOG_MODE_PHRASE:
                    UICanvas_dialogObj_sender.gameObject.SetActive(true);
                    UICanvas_dialogObj_msg.gameObject.SetActive(true);
                    UICanvas_dialogOptions.SetActive(false);

                    UICanvas_dialogObj_sender.text = sender;
                    UICanvas_dialogObj_msg.text = msg;
                    break;

                default:
                    UICanvas_dialogObj_sender.gameObject.SetActive(false);
                    UICanvas_dialogObj_msg.gameObject.SetActive(false);
                    UICanvas_dialogOptions.SetActive(false);
                    break;
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
            if (spriteID == GameSprite.SPRITE_CURSOR_DRAG)
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

        public void SetCursorLabel(GameItem item)
        {
            if (item == GameItem.ITEM_NONE)
            {
                cursor_textobj.SetActive(false);
            }
            else
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);
                cursor_textobj_text.text = ResourceDialogsClass.GetName(itemInfo.name);
                cursor_textobj.SetActive(true);
            }
        }

        public void SetUserInteraction(UserInputInteraction interaction)
        {
            switch(interaction)
            {
                case UserInputInteraction.INPUT_INTERACTION_TAKE:
                    takeButton_img.color = Color.white;
                    talkButton_img.color = Color.gray;
                    observeButton_img.color = Color.gray;
                    break;
                case UserInputInteraction.INPUT_INTERACTION_TALK:
                    takeButton_img.color = Color.gray;
                    talkButton_img.color = Color.white;
                    observeButton_img.color = Color.gray;
                    break;
                default:
                    takeButton_img.color = Color.gray;
                    talkButton_img.color = Color.gray;
                    observeButton_img.color = Color.white;
                    break;
            }
        }

        public void ActivateDialogOption(int index, bool activate, DialogOption option, string text)
        {
            ref readonly DialogOptionButtonClass button = ref UICanvas_dialogOptionButtons[index];

            if (activate)
            {
                button.SetDialogOption(option);
                button.SetOptionText(in text);
            }
            else
            {
                button.SetDialogOption(DialogOption.DIALOG_OPTION_NONE);
                button.SetOptionText(in string.Empty);
            }

            button.SetActive(activate);
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

        public void AnimateNewUserInteraction(UserInputInteraction interaction)
        {
            /* Passthrough */
            cursor_userInteraction_cls.AnimateNewUserInteraction(interaction);
        }

        public IEnumerator Execute_Load_Coroutine(DIALOG_OPTION_CLICK_DELEGATE OnDialogOptionClick,
            DISPLAYED_ITEM_CLICK OnItemDisplayClick,
            DISPLAYED_ITEM_HOVER OnHover,
            MENU_BUTTON_CLICK_DELEGATE OnMenuButtonClick
            )
        {
            bool sprites_loaded = false;

            saveButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_SAVE));
            exitButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_EXIT));
            takeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TAKE));
            talkButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TALK));
            observeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_OBSERVE));


            for (int i = 0; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
            {
                Transform btnTransf = UICanvas_dialogOptions.transform.Find("DialogOption" + (i + 1).ToString());
                UICanvas_dialogOptionButtons[i] = btnTransf.Find("ActiveArea").gameObject.GetComponent<DialogOptionButtonClass>();
                UICanvas_dialogOptionButtons[i].SetClickDelegate(OnDialogOptionClick);
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
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out sprites_loaded);
            }

            UICanvas_itemMenuObj.GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY);
            takeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TAKE);
            talkButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TALK);
            observeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_OBSERVE);

            cursor_userInteraction_cls.LoadTask();
        }

    }
}
