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
        private static readonly Color idleColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        private static readonly Color textIdleColor = new(0.85f, 0.85f, 0.85f, 1.0f);
        private static readonly Color hoverColor = Color.white;
        private static readonly Color notClearedColor = Color.black;

        private static readonly Color transparentColor = new(1.0f, 1.0f, 1.0f, 0.0f);
        private static readonly Color selectedBckgColor = new(0.42f, 0.7f, 1.0f, 1.0f);

        private bool cleared;
        private bool selected;
        private GameObject newsObj;
        private RectTransform rectTransform;
        private Image backgroundImage;
        private Image image;
        private TMP_Text text;
        private MementoParent parent;
        private MEMENTO_ITEM_CLICK_DELEGATE onClickFunction;

        public Vector2 GetSize => rectTransform.sizeDelta;

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }

        public void Select(bool select)
        {
            selected = select;

            if(select)
            {
                if(cleared)
                {
                    image.color = hoverColor;
                }

                text.color = hoverColor;
                backgroundImage.color = selectedBckgColor;
                newsObj.SetActive(false);
            }
            else
            {
                if(cleared)
                {
                    image.color = idleColor;
                }

                backgroundImage.color = transparentColor;
                text.color = textIdleColor;
            }
        }

        public void SetPositionAndFunction(int index, MEMENTO_ITEM_CLICK_DELEGATE onClickFunction)
        {
            this.onClickFunction = onClickFunction;

            Vector2 anchPos = rectTransform.anchoredPosition;
            anchPos.y = -index * rectTransform.sizeDelta.y;
            rectTransform.anchoredPosition = anchPos;
        }

        public void SetMementoParent(MementoParent parent, bool totallyCleared, bool unwatched)
        {
            this.parent = parent;
            this.cleared = totallyCleared;

            if (parent != MementoParent.MEMENTO_PARENT_NONE)
            {
                ref readonly MementoParentInfo memparinfo = ref ItemsInteractionsClass.GetMementoParentInfo(parent);

                Sprite sprite = ResourceSpritesClass.GetSprite(memparinfo.sprite);
                image.sprite = sprite;

                string name = ResourceDialogsClass.GetName(memparinfo.name);
                text.text = name;

                if(totallyCleared)
                {
                    image.color = idleColor;
                }
                else
                {
                    image.color = notClearedColor;
                }

                text.color = textIdleColor;
                newsObj.SetActive(unwatched);
            }
            else
            {
                image.sprite = null;
                text.text = string.Empty;
            }

            backgroundImage.color = transparentColor;
        }

        void Awake()
        {
            backgroundImage = GetComponent<Image>();
            image = transform.Find("Icon").GetComponent<Image>();
            newsObj = image.transform.Find("News").gameObject;
            text = transform.Find("Name").GetComponent<TMP_Text>();
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            backgroundImage.color = transparentColor;
            image.color = idleColor;
            text.color = textIdleColor;
            selected = false;
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
            if (cleared)
            {
                image.color = hoverColor;
            }

            text.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!selected)
            {
                if (cleared)
                {
                    image.color = idleColor;
                }

                text.color = textIdleColor;
            }
        }
    }
}
