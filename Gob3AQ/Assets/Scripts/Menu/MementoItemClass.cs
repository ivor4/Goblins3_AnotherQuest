using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.Types;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gob3AQ.GameMenu.MementoItem
{
    public delegate void MEMENTO_ITEM_CLICK_DELEGATE(MementoParent mementoParent);

    public class MementoItemClass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private static readonly Color color_item_idle = new(0.5f, 0.5f, 0.5f, 0.5f);
        private static readonly Color color_text_idle = new(0.85f, 0.85f, 0.85f, 1.0f);
        private static readonly Color color_all_hover = Color.white;
        private static readonly Color color_item_uncleared = Color.black;

        private static readonly Color color_transparent = new(1.0f, 1.0f, 1.0f, 0.0f);
        private static readonly Color color_background_selected = new(0.42f, 0.7f, 1.0f, 1.0f);

        private Color text_color_idle;
        private Color text_color_hover;
        private Color background_color_idle;
        private Color background_color_hover;
        private Color item_color_idle;
        private Color item_color_hover;
        private Color item_color_reload_idle;
        private Color item_color_reload_hover;

        private GameObject newsObj;
        private RectTransform rectTransform;
        private Image backgroundImage;
        private Image itemImage;
        private TMP_Text text;
        private MementoParent parent;
        private MEMENTO_ITEM_CLICK_DELEGATE onClickFunction;

        public Vector2 GetSize => rectTransform.sizeDelta;

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);
        }

        public void Select(bool select, bool doubleClick)
        {
            if(select)
            {
                background_color_idle = color_background_selected;
                background_color_hover = color_background_selected;
                item_color_idle = item_color_reload_hover;
                text_color_idle = color_all_hover;

                Refresh_Hover();

                newsObj.SetActive(false);
            }
            else
            {
                ResetVisual();
            }
        }

        public void InitialSetup(int index, MEMENTO_ITEM_CLICK_DELEGATE onClickFunction, string name)
        {
            this.onClickFunction = onClickFunction;
            gameObject.name = name;

            Vector2 anchPos = rectTransform.anchoredPosition;
            anchPos.y = -index * rectTransform.sizeDelta.y;
            rectTransform.anchoredPosition = anchPos;
        }

        public void SetMementoParent(MementoParent parent, bool totallyCleared, bool unwatched)
        {
            this.parent = parent;

            if (totallyCleared)
            {
                item_color_reload_idle = color_item_idle;
                item_color_reload_hover = color_all_hover;
            }
            else
            {
                item_color_reload_idle = color_item_uncleared;
                item_color_reload_hover = color_item_uncleared;
            }

            ResetVisual();

            if (parent != MementoParent.MEMENTO_PARENT_NONE)
            {
                ref readonly MementoParentInfo memparinfo = ref ItemsInteractionsClass.GetMementoParentInfo(parent);

                Sprite sprite = ResourceSpritesClass.GetSprite(memparinfo.sprite);
                itemImage.sprite = sprite;

                string name = ResourceDialogsClass.GetName(memparinfo.name);
                text.text = name;

                newsObj.SetActive(unwatched);
            }
            else
            {
                itemImage.sprite = null;
                text.text = string.Empty;
            }
        }

        void Awake()
        {
            backgroundImage = GetComponent<Image>();
            itemImage = transform.Find("Icon").GetComponent<Image>();
            newsObj = itemImage.transform.Find("News").gameObject;
            text = transform.Find("Name").GetComponent<TMP_Text>();
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            item_color_idle = color_item_uncleared;
            item_color_hover = color_item_uncleared;
            ResetVisual();
        }

        void OnEnable()
        {
            ResetVisual();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (parent != MementoParent.MEMENTO_PARENT_NONE)
            {
                onClickFunction?.Invoke(parent);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Refresh_Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Refresh_Idle();
        }

        private void ResetVisual()
        {
            background_color_idle = color_transparent;
            background_color_hover = color_transparent;
            text_color_idle = color_text_idle;
            text_color_hover = color_all_hover;
            item_color_idle = item_color_reload_idle;
            item_color_hover = item_color_reload_hover;

            Refresh_Idle();
        }

        private void Refresh_Idle()
        {
            itemImage.color = item_color_idle;
            text.color = text_color_idle;
            backgroundImage.color = background_color_idle;
        }

        private void Refresh_Hover()
        {
            itemImage.color = item_color_hover;
            text.color = text_color_hover;
            backgroundImage.color = background_color_hover;
        }
    }
}
