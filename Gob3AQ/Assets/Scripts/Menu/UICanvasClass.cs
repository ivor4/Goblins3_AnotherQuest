using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.MementoItem;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
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
        DISPLAY_MODE_NONE,
        DISPLAY_MODE_INVENTORY,
        DISPLAY_MODE_DIALOG,
        DISPLAY_MODE_MEMENTO
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
        MENU_BUTTON_MEMENTO,
        MENU_BUTTON_TAKE,
        MENU_BUTTON_TALK,
        MENU_BUTTON_OBSERVE
    }

    public class UICanvasClass : MonoBehaviour
    {
        private const string SEPARATOR = "\n\n_____________________________\n\n";

        private GameObject UICanvas_loadingObj;
        private GameObject UICanvas_dialogObj;
        private GameObject UICanvas_mementoObj;
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
        private RectTransform cursor_rect;

        private Image cursor_spr;
        private Image cursor_subobj_spr;
        private TMP_Text cursor_textobj_text;

        private GameObject UICanvas_uppertoolbarObj;
        private Button tool_saveButton;
        private Button tool_exitButton;
        private Button tool_mementoButton;
        private Button tool_takeButton;
        private Button tool_talkButton;
        private Button tool_observeButton;
        private Image tool_takeButton_img;
        private Image tool_talkButton_img;
        private Image tool_observeButton_img;

        private GameObject memento_ContentObj;
        private TMP_Text memento_descrText;
        private RectTransform memento_ContentRectTransform;
        private MementoParent memento_selectedItem;
        private HashSet<MementoParent> memento_combinedItems;
        private MementoItemClass[] memento_itemClass;
        private List<MementoParent> memento_unlocked_parents_list;
        private HashSet<MementoParent> memento_unlocked_parents;
        private Dictionary<MementoParent, MementoItemClass> memento_parent_dict;
        private HashSet<MementoParent> memento_totallyCleared;
        private HashSet<MementoParent> memento_unwatched;
        private HashSet<Memento> memento_unlocked;

        private StringBuilder stringBuilder;



        private void Awake()
        {
            UICanvas_dialogOptionButtons = new DialogOptionButtonClass[GameFixedConfig.MAX_DIALOG_OPTIONS];
            UICanvas_inventoryButtons = new PickableItemDisplayClass[GameFixedConfig.MAX_DISPLAYED_PICKED_ITEMS];
            raycaster = GetComponent<GraphicRaycaster>();

            UICanvas_loadingObj = transform.Find("LoadingObj").gameObject;
            UICanvas_dialogObj = transform.Find("DialogObj").gameObject;
            UICanvas_mementoObj = transform.Find("MementoObj").gameObject;
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
            tool_saveButton = UICanvas_uppertoolbarObj.transform.Find("SaveButton").GetComponent<Button>();
            tool_exitButton = UICanvas_uppertoolbarObj.transform.Find("ExitButton").GetComponent<Button>();
            tool_mementoButton = UICanvas_uppertoolbarObj.transform.Find("MementoButton").GetComponent<Button>();
            tool_takeButton = UICanvas_uppertoolbarObj.transform.Find("TakeButton").GetComponent<Button>();
            tool_talkButton = UICanvas_uppertoolbarObj.transform.Find("TalkButton").GetComponent<Button>();
            tool_observeButton = UICanvas_uppertoolbarObj.transform.Find("ObserveButton").GetComponent<Button>();
            tool_takeButton_img = tool_takeButton.gameObject.GetComponent<Image>();
            tool_talkButton_img = tool_talkButton.gameObject.GetComponent<Image>();
            tool_observeButton_img = tool_observeButton.gameObject.GetComponent<Image>();

            memento_ContentObj = UICanvas_mementoObj.transform.Find("MementoList/Viewport/Content").gameObject;
            memento_descrText = UICanvas_mementoObj.transform.Find("MementoDescr").GetComponent<TMP_Text>();
            memento_ContentRectTransform = memento_ContentObj.GetComponent<RectTransform>();
            memento_unlocked_parents_list = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_unlocked_parents = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_parent_dict = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_totallyCleared = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_unlocked = new((int)Memento.MEMENTO_TOTAL);
            memento_unwatched = new((int)MementoParent.MEMENTO_PARENT_TOTAL);
            memento_itemClass = new MementoItemClass[(int)MementoParent.MEMENTO_PARENT_TOTAL];
            memento_combinedItems = new(2);

            /* Will be enabled at the end of Loading (new display mode) */
            raycaster.enabled = false;

            memento_selectedItem = MementoParent.MEMENTO_PARENT_NONE;

            stringBuilder = new(512);
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
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(true);

                    break;

                case DisplayMode.DISPLAY_MODE_DIALOG:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(true);
                    UICanvas_mementoObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;

                case DisplayMode.DISPLAY_MODE_MEMENTO:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(true);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;

                default:
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(false);
                    UICanvas_mementoObj.SetActive(false);
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

        public void SetCursorLabel(GameItem item, in ItemInfo itemInfo)
        {
            if (item == GameItem.ITEM_NONE)
            {
                cursor_textobj.SetActive(false);
            }
            else
            {
                
                cursor_textobj_text.text = ResourceDialogsClass.GetName(itemInfo.name);
                cursor_textobj.SetActive(true);
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

        public void MementoMenuActivated()
        {
            memento_descrText.text = string.Empty;

            ClearCombinedMementos();

            if (memento_selectedItem != MementoParent.MEMENTO_PARENT_NONE)
            {
                memento_parent_dict[memento_selectedItem].Select(false, false);
                memento_selectedItem = MementoParent.MEMENTO_PARENT_NONE;
            }
        }


        public void NewMementoUnlocked(Memento memento, bool unwatched, bool sortAndResize)
        {
            ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(memento);
            memento_unlocked.Add(memento);

            if(unwatched)
            {
                memento_unwatched.Add(memInfo.parent);
            }

            /* Assumed only one initial per parent Memento */
            if (!memento_unlocked_parents.Contains(memInfo.parent))
            {
                memento_unlocked_parents_list.Add(memInfo.parent);
                memento_unlocked_parents.Add(memInfo.parent);
            }

            if (memInfo.final)
            {
                memento_totallyCleared.Add(memInfo.parent);
            }

            if(sortAndResize)
            {
                MementoSortAndResizeAll();
            }
        }

        public void MementoParentClicked(MementoParent parent, bool doubleClick, out ReadOnlyHashSet<MementoParent> combinedMementos)
        {
            memento_unwatched.Remove(parent);
            MementoItemClass itemClass = memento_parent_dict[parent];

            MementoParent prevSelected = memento_selectedItem;
            if ((memento_selectedItem != MementoParent.MEMENTO_PARENT_NONE) && (!memento_combinedItems.Contains(memento_selectedItem)))
            {
                memento_parent_dict[memento_selectedItem].Select(false, false);
                memento_selectedItem = MementoParent.MEMENTO_PARENT_NONE;
            }

            bool mementoCombinedFull = memento_combinedItems.Count >= 2;

            if (doubleClick && (!mementoCombinedFull))
            {
                if (memento_combinedItems.Contains(parent) || (prevSelected != parent))
                {
                    memento_combinedItems.Remove(parent);
                    itemClass.Select(true, false);
                    memento_selectedItem = parent;
                }
                else
                {
                    memento_combinedItems.Add(parent);
                    itemClass.Select(true, true);
                }
            }
            else
            {
                if (mementoCombinedFull || memento_combinedItems.Contains(parent))
                {
                    ClearCombinedMementos(); 
                }

                itemClass.Select(true, false);
                memento_selectedItem = parent;
            }

            combinedMementos = new(memento_combinedItems);
            

            ref readonly MementoParentInfo memParInfo = ref ItemsInteractionsClass.GetMementoParentInfo(parent);
            ReadOnlySpan<Memento> children = memParInfo.Children;

            stringBuilder.Clear();
            bool addedElement = false;

            for (int i = 0; i < children.Length; ++i)
            {
                Memento memento = children[i];

                if (memento_unlocked.Contains(memento))
                {
                    if (addedElement && (i > 0))
                    {
                        stringBuilder.Append(SEPARATOR);
                    }

                    ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(memento);
                    ResourceDialogsClass.GetPhraseContent(memInfo.phrase, out PhraseContent phraseContent);
                    stringBuilder.Append(phraseContent.message);
                    addedElement = true;
                }
            }

            memento_descrText.text = stringBuilder.ToString();
        }

        private void MementoSortAndResizeAll()
        {
            /* Fit content to size */
            Vector2 sizeDelta = memento_ContentRectTransform.sizeDelta;
            sizeDelta.y = memento_unlocked_parents_list.Count * memento_itemClass[0].GetSize.y;
            memento_ContentRectTransform.sizeDelta = sizeDelta;

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
                    memento_parent_dict[parent] = instance;

                    instance.SetMementoParent(parent,
                        memento_totallyCleared.Contains(parent),
                        memento_unwatched.Contains(parent));
                    instance.Activate(true);
                    
                }
                /* Deactivated ones */
                else
                {
                    instance.Activate(false);
                }
            }
        }

        private void ClearCombinedMementos()
        {
            foreach (MementoParent mementoParent in memento_combinedItems)
            {
                memento_parent_dict[mementoParent].Select(false, false);
            }

            memento_combinedItems.Clear();
        }

        public IEnumerator Execute_Load_Coroutine(DIALOG_OPTION_CLICK_DELEGATE OnDialogOptionClick,
            DISPLAYED_ITEM_CLICK OnItemDisplayClick,
            DISPLAYED_ITEM_HOVER OnHover,
            MENU_BUTTON_CLICK_DELEGATE OnMenuButtonClick,
            MEMENTO_ITEM_CLICK_DELEGATE OnMementoItemClick
            )
        {
            bool sprites_loaded = false;

            tool_saveButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_SAVE));
            tool_exitButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_EXIT));
            tool_mementoButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_MEMENTO));
            tool_takeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TAKE));
            tool_talkButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_TALK));
            tool_observeButton.onClick.AddListener(() => OnMenuButtonClick(MenuButtonType.MENU_BUTTON_OBSERVE));


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
                VARMAP_GameMenu.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out sprites_loaded);
            }

            UICanvas_itemMenuObj.GetComponent<Image>().sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_INVENTORY);
            tool_takeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TAKE);
            tool_talkButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TALK);
            tool_observeButton.image.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_OBSERVE);

            cursor_userInteraction_cls.LoadTask();
            yield return ResourceAtlasClass.WaitForNextFrame;

            /* Fill memento loaded list */
            
 
            /* Load memento */
            GameObject memento_item_prefab = ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_MEMENTO_ITEM);

            AsyncInstantiateOperation<GameObject> handle = InstantiateAsync<GameObject>(memento_item_prefab, (int)MementoParent.MEMENTO_PARENT_TOTAL);
            yield return handle;
            GameObject[] memento_itemObj = handle.Result;
            
            /* Keep them ready for usage */
            for (int i=0; i < memento_itemObj.Length; ++i)
            {
                memento_itemObj[i].transform.SetParent(memento_ContentObj.transform, false);
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

            /* Update lists */
            /* Check for all active mementos */
            for (int i = 0; i < (int)Memento.MEMENTO_TOTAL; ++i)
            {
                Memento memento = (Memento)i;

                VARMAP_GameMenu.IS_MEMENTO_UNLOCKED(memento, out bool unlocked, out bool unwatched);

                if (unlocked)
                {
                    NewMementoUnlocked(memento, unwatched, false);
                }
            }
            yield return ResourceAtlasClass.WaitForNextFrame;

            MementoSortAndResizeAll();
        }

 

        private static int MementoParentSortMethod(MementoParent a, MementoParent b)
        {
            return (int)a - (int)b;
        }
    }
}
