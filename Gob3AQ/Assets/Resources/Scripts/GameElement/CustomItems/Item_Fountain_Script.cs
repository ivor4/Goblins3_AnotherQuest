using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.ResourceAtlas;

namespace Gob3AQ.GameElement.Item.Fountain
{
    public class Item_Fountain_Script : ItemClass
    {
        private Sprite _sprite_full;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            if (registered)
            {
                _sprite_full = ResourceAtlasClass.GetSprite(SpriteEnum.SPRITE_FOUNTAIN_FULL);

                VARMAP_ItemMaster.IS_EVENT_OCCURRED(GameEvent.EVENT_FOUNTAIN_FULL, out bool occurred);

                if (occurred)
                {
                    _sprRenderer.sprite = _sprite_full;
                }
                else
                {
                    VARMAP_ItemMaster.EVENT_SUBSCRIPTION(GameEvent.EVENT_FOUNTAIN_FULL, _Fountain_Filled, true);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (registered)
            {
                VARMAP_ItemMaster.EVENT_SUBSCRIPTION(GameEvent.EVENT_FOUNTAIN_FULL, _Fountain_Filled, false);
            }
        }



        private void _Fountain_Filled(bool _)
        {
            _sprRenderer.sprite = _sprite_full;

            /* Unsubscribe */
            VARMAP_ItemMaster.EVENT_SUBSCRIPTION(GameEvent.EVENT_FOUNTAIN_FULL, _Fountain_Filled, false);
        }
        
    }
}
