using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Gob3AQ.GameElement.Item
{
    public class ItemClass : GameElementClass
    {

        protected override void Awake()
        {
            base.Awake();

            topParent = transform.parent.gameObject;

            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();


            gameElementFamily = GameItemFamily.ITEM_FAMILY_TYPE_OBJECT;

            SetVisible_Internal(false);
            SetClickable_Internal(false);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            bool taken;
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            /* If it is a pickable item, may have been picked before */
            if (itemInfo.isPickable)
            {
                VARMAP_ItemMaster.IS_ITEM_TAKEN_FROM_SCENE(itemInfo.pickableItem, out taken);
            }
            else
            {
                taken = false;
            }

            if(!taken)
            {
                /* Register item as Level element (to be clicked and able to iteract) */
                VARMAP_ItemMaster.ITEM_REGISTER(true, this);

                topParent.GetComponent<GameElementClickable>().SetOnClickAction(MouseEnterAction);

                registered = true;

                /* Execute on next Update */
                _ = StartCoroutine(Execute_Loading());
            }
            else
            {
                registered = false;
                VirtualDestroy();
            }
        }


        protected virtual IEnumerator Execute_Loading()
        {
            bool loaded = false;

            while (!loaded)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out loaded);
            }

            Loading_Task();
        }

        protected virtual void Loading_Task()
        {
            VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint);

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            /* Set to default sprite */
            mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(itemInfo.Sprites[0]);

            SetVisible_Internal(true);
            SetClickable_Internal(true);
            SetAvailable(true);
        }
    }
}
