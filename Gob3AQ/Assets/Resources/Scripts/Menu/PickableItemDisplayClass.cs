using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.ResourceAtlas;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gob3AQ.ResourceSprites;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.Brain.ItemsInteraction;

namespace Gob3AQ.GameMenu.PickableItemDisplay
{
    public delegate void DISPLAYED_ITEM_CLICK(GameItem item);

    public class PickableItemDisplayClass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Image _spr;
        private GameItem _item;
        private DISPLAYED_ITEM_CLICK _call;
        private GameObject _glow;
        private Image _sprglow;
        private GameObject _parent;

        public void SetCallFunction(DISPLAYED_ITEM_CLICK fn)
        {
            _call = fn;
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
            pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)item];
            sprID = ResourceSpritesAtlasClass.PickableItemToSpriteAvatar[(int)pickable];
            _spr.sprite = ResourceSpritesClass.GetSprite(sprID);
            _sprglow.sprite = _spr.sprite;
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _glow.SetActive(false);
            eventData.Use();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _call?.Invoke(_item);
                eventData.Use();
            }
        }
    }
}
