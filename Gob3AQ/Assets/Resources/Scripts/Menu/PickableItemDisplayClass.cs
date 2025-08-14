using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.ResourceAtlas;

namespace Gob3AQ.GameMenu.PickableItemDisplay
{
    public delegate void DISPLAYED_ITEM_CLICK(GameItem item);

    public class PickableItemDisplayClass : MonoBehaviour
    {
        private SpriteRenderer _spr;
        private GameItem _item;
        private DISPLAYED_ITEM_CLICK _call;
        private GameObject _glow;
        private SpriteRenderer _sprglow;

        public void SetCallFunction(DISPLAYED_ITEM_CLICK fn)
        {
            _call = fn;
        }

        public void SetDisplayedItem(GameItem item)
        {
            _item = item;
            _spr.sprite = ResourceAtlasClass.GetPickableAvatarSpriteFromItem(item);
            _sprglow.sprite = _spr.sprite;
        }

        void Awake()
        {
            _item = GameItem.ITEM_NONE;
            _spr = GetComponent<SpriteRenderer>();
            _glow = transform.Find("Glow").gameObject;
            _sprglow = _glow.GetComponent<SpriteRenderer>();
        }

        void OnMouseEnter()
        {
            _glow.SetActive(true);
        }

        void OnMouseExit()
        {
            _glow.SetActive(false);    
        }

        void OnDisable()
        {
            _glow.SetActive(false);
        }

        void OnMouseDown()
        {
            _call?.Invoke(_item);
        }
    }
}
