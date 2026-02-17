using UnityEngine;
using Gob3AQ.VARMAP.Types;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gob3AQ.ResourceSprites;
using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.GameMenu.PickableItemDisplay
{
   
    public class PickableItemDisplayClass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Image _spr;
        private GameItem _item;
        private DISPLAYED_ITEM_CLICK _call_click;
        private DISPLAYED_ITEM_HOVER _call_hover;
        private GameObject _glow;
        private Image _sprglow;
        private GameObject _parent;

        public void SetOnClickCallFunction(DISPLAYED_ITEM_CLICK fn)
        {
            _call_click = fn;
        }

        public void SetHoverCallFunction(DISPLAYED_ITEM_HOVER fn)
        {
            _call_hover = fn;
        }

        public void Enable(bool enable)
        {
            _parent.SetActive(enable);
        }

        public void SetDisplayedItem(GameItem item)
        {
            GamePickableItem pickable;
            GameSprite sprID;

            _item = item;

            if (item != GameItem.ITEM_NONE)
            {
                ref readonly ItemInfo info = ref ItemsInteractionsClass.GetItemInfo(item);
                pickable = info.pickableItem;
                sprID = info.pickableSprite;
                Sprite sprRes = ResourceSpritesClass.GetSprite(sprID);
                _spr.sprite = sprRes;
                _sprglow.sprite = sprRes;
            }
            else
            {
                _spr.sprite = null;
                _sprglow.sprite = null;
            }
        }

        void Awake()
        {
            _item = GameItem.ITEM_NONE;
            _spr = GetComponent<Image>();
            _glow = transform.parent.Find("Glow").gameObject;
            _sprglow = _glow.GetComponent<Image>();
            _parent = transform.parent.gameObject;
        }



        void OnDisable()
        {
            _glow.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _glow.SetActive(true);
            eventData.Use();

            _call_hover?.Invoke(_item, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _glow.SetActive(false);
            eventData.Use();

            _call_hover?.Invoke(_item, false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _call_click?.Invoke(_item);
                eventData.Use();
            }
        }
    }
}
