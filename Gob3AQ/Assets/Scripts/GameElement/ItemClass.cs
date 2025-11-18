using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System.Collections;
using UnityEngine;



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

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            GameElementClickable clickable = topParent.GetComponent<GameElementClickable>();

            clickable.SetOnHoverAction(MouseEnterAction);

            /* Register item as Level element (to be clicked and able to iteract) */
            VARMAP_ItemMaster.ITEM_REGISTER(true, this, clickable);

            registered = true;

            /* Execute on next Update */
            _ = StartCoroutine(Execute_Loading());
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
            if (startingWaypoint == null)
            {
                VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint, out _);
            }
            else
            {
                actualWaypoint = startingWaypoint.ID_in_Network;
            }

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            /* Set to default sprite */
            mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(actualSprite);

            SetVisible_Internal(true);
            SetClickable_Internal(true);
            SetAvailable(true);

            UpdateSortingOrder();

            loaded = true;
        }
    }
}
