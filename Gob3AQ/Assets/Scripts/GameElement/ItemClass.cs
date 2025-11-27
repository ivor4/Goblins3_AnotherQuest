using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ItemMaster;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System.Collections;
using UnityEngine;



namespace Gob3AQ.GameElement.Item
{
    [System.Serializable]
    public class ItemClass : GameElementClass
    {
        [SerializeField]
        private bool needsZoom;

        [SerializeField]
        private float maxZoomLevel;

        private float actualZoomLevel;

        private Color transparent_color;
        private Color original_color;

        protected override void Awake()
        {
            base.Awake();

            topParent = transform.parent.gameObject;

            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();

            original_color = mySpriteRenderer.color;
            transparent_color = mySpriteRenderer.color;
            transparent_color.a *= 0.2f;

            SetVisible_Internal(false);
            SetClickable_Internal(false);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            if(needsZoom)
            {
                VARMAP_ItemMaster.ZOOM_SUBSCRIPTION(true, _OnZoomChanged);
                actualZoomLevel = 100f;
            }

            ItemMasterClass.AddOneItemToLoad();

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
            SetAvailable(true);

            if ((!needsZoom) || (actualZoomLevel <= maxZoomLevel))
            {
                SetClickable_Internal(true);
            }
            else
            {
                mySpriteRenderer.color = transparent_color;
                SetClickable_Internal(false);
            }

            UpdateSortingOrder();

            ItemMasterClass.AddOneItemLoaded();

            loaded = true;
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            if (needsZoom)
            {
                VARMAP_ItemMaster.ZOOM_SUBSCRIPTION(false, _OnZoomChanged);
            }
        }

        private void _OnZoomChanged(float newZoonLevel)
        {
            actualZoomLevel = newZoonLevel;

            if (newZoonLevel > maxZoomLevel)
            {
                mySpriteRenderer.color = transparent_color;
                SetClickable_Internal(false);
            }
            else
            {
                mySpriteRenderer.color = original_color;
                SetClickable_Internal(true);
            }
        }
    }
}
