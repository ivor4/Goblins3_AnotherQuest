using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.ResourceSpritesAtlas;
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
        private static readonly Color hoverColor = Color.white;
        private static readonly Color notClearedColor = Color.black;

        private int index;
        private bool cleared;
        private RectTransform rectTransform;
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

        public void SetPositionAndFunction(int index, MEMENTO_ITEM_CLICK_DELEGATE onClickFunction)
        {
            this.index = index;
            this.onClickFunction = onClickFunction;

            Vector2 anchPos = rectTransform.anchoredPosition;
            anchPos.y = -index * rectTransform.sizeDelta.y;
            rectTransform.anchoredPosition = anchPos;
        }

        public void SetMementoParent(MementoParent parent, bool totallyCleared)
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

            }
            else
            {
                image.sprite = null;
                text.text = string.Empty;
            }
        }

        void Awake()
        {
            image = transform.Find("Icon").GetComponent<Image>();
            text = transform.Find("Name").GetComponent<TMP_Text>();
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            image.color = idleColor;
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (cleared)
            {
                image.color = idleColor;
            }
        }
    }
}
