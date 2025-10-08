using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Gob3AQ.GameElement.Item
{
    public class ItemClass : GameElement
    {
        protected SpriteRenderer _sprRenderer;
        protected Collider2D _collider;
        protected Rigidbody2D _rigidbody;

        protected bool registered;

        void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _sprRenderer.enabled = false;
            _collider.enabled = false;

            gameElementFamily = GameItemFamily.ITEM_FAMILY_TYPE_OBJECT;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
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
                _collider.enabled = true;
                _sprRenderer.enabled = true;
                registered = true;
                isAvailable = true;

                /* Execute on next Update */
                _ = StartCoroutine(Execute_Loading());
            }
            else
            {
                gameObject.SetActive(false);
                registered = false;
            }
        }

        protected virtual void OnDisable()
        {
            if(registered)
            {
                VARMAP_ItemMaster.ITEM_REGISTER(false, this);
            }
        }


        protected void OnMouseEnter()
        {
            MouseEnterAction(true);
        }

        protected void OnMouseExit()
        {
            MouseEnterAction(false);
        }

        private void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            LevelElemInfo info = new((int)itemID, gameElementFamily, actualWaypoint, enter & isAvailable);
            VARMAP_ItemMaster.GAME_ELEMENT_OVER(in info);
        }


        protected virtual IEnumerator Execute_Loading()
        {
            yield return new WaitForNextFrameUnit();
            VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint);
        }
    }
}
