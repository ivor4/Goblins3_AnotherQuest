using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using UnityEngine;

namespace Gob3AQ.GameElement
{
    [System.Serializable]
    public class GameElementClass : MonoBehaviour
    {
        [SerializeField]
        protected GameItem itemID;

        [SerializeField]
        protected WaypointClass startingWaypoint;

        [SerializeField]
        protected int hoverPriority;


        public GameItem ItemID => itemID;

        public Collider2D My2DCollider => myCollider;
        public int Waypoint => actualWaypoint;

        public bool IsAvailable => isAvailable;

        protected GameSprite actualSprite;
        protected GameItemFamily gameElementFamily;
        protected int actualWaypoint;
        protected bool isHovered;
        protected GameObject topParent;
        protected Collider2D myCollider;
        protected SpriteRenderer mySpriteRenderer;
        protected Rigidbody2D myRigidbody;
        protected bool registered;
        protected bool loaded;
        private bool isAvailable;
        private bool isActive_int;
        private bool isActive_ext;
        private bool isVisible_int;
        private bool isVisible_ext;
        private bool isClickable_int;
        private bool isClickable_ext;
        private bool isMotion_int;
        private bool isMotion_ext;

        private bool isUnspawned;
        private bool isUnclickable;

        protected virtual void Awake()
        {
            isActive_int = true;
            isActive_ext = true;
            isVisible_int = true;
            isVisible_ext = true;
            isClickable_int = true;
            isClickable_ext = true;
            isMotion_int = true;
            isMotion_ext = true;
            isUnspawned = false;
            isUnclickable = false;
        }

        protected virtual void Start()
        {
            loaded = false;

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            actualSprite = itemInfo.defaultSprite;

            gameElementFamily = itemInfo.family;

            GameElementClickable clickable = topParent.GetComponent<GameElementClickable>();

            clickable.SetOnHoverAction(MouseEnterAction);

            /* Register item as Level element (to be clicked and able to iteract) */
            VARMAP_ItemMaster.ITEM_REGISTER(true, this, clickable);

            registered = true;

            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);

            /* Update sorting order here has no sense, sprite is not yet loaded */
        }


        protected virtual void OnDestroy()
        {
            VirtualDestroy();
            
        }

        public void SetUnspawned(bool unspawned)
        {
            isUnspawned = unspawned;
        }

        public void SetSprite(GameSprite newSprite)
        {
            actualSprite = newSprite;

            if (loaded)
            {
                mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(actualSprite);
            }
        }


        protected void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            isHovered = enter;
            LevelElemInfo info = new(itemID, gameElementFamily, actualWaypoint, hoverPriority, enter & isAvailable);
            VARMAP_ItemMaster.GAME_ELEMENT_HOVER(in info);
        }

        protected void SetAvailable(bool available)
        {
            isAvailable = available;
            _Hover_Refresh();
        }

        /// <summary>
        /// Sets active externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetActive(bool active)
        {
            isActive_ext = active;
            _SetActive_Refresh();
        }

        protected void SetActive_Internal(bool active)
        {
            isActive_int = active;
            _SetActive_Refresh();
        }

        private void _SetActive_Refresh()
        {
            bool enable = isActive_int & isActive_ext;

            gameObject.SetActive(enable);
        }

        /// <summary>
        /// Sets visible externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetVisible(bool active)
        {
            isVisible_ext = active;
            _SetVisible_Refresh();
        }

        protected void SetVisible_Internal(bool active)
        {
            isVisible_int = active;
            _SetVisible_Refresh();
        }

        private void _SetVisible_Refresh()
        {
            bool enable = isVisible_int & isVisible_ext;
            mySpriteRenderer.enabled = enable;
            _Hover_Refresh();
        }

        /// <summary>
        /// Sets motion externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetMotion(bool active)
        {
            isMotion_ext = active;
            _SetMotion_Refresh();
        }

        protected void SetMotion_Internal(bool active)
        {
            isMotion_int = active;
            _SetMotion_Refresh();
        }

        private void _SetMotion_Refresh()
        {
            bool enable = isMotion_int & isMotion_ext;
            myRigidbody.simulated = enable;
        }

        public void SetUnclickable(bool unclickable)
        {
            isUnclickable = unclickable;
            SetClickable(!isUnclickable);
        }

        /// <summary>
        /// Sets clickable externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetClickable(bool active)
        {
            isClickable_ext = active;
            _SetClickable_Refresh();
        }

        protected void SetClickable_Internal(bool active)
        {
            isClickable_int = active;
            _SetClickable_Refresh();
        }

        private void _SetClickable_Refresh()
        {
            bool enable = isClickable_ext & isClickable_int;
            myCollider.enabled = enable;
            _Hover_Refresh();
        }

        private void _Hover_Refresh()
        {
            bool compound;

            compound = isAvailable;
            compound &= isClickable_ext & isClickable_int;

            if (isHovered && !compound)
            {
                /* Update hover state */
                MouseEnterAction(false);
                isHovered = false;
            }
        }

        protected void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)(mySpriteRenderer.bounds.min.y * 1000);
        }



        public virtual void VirtualDestroy()
        {
            SetAvailable(false);
            SetClickable(false);
            SetMotion(false);
            SetVisible(false);

            _Hover_Refresh();

            topParent.SetActive(false);

            VARMAP_ItemMaster.UNREG_GAMESTATUS(ChangedGameStatus);

            if (registered)
            {
                VARMAP_ItemMaster.ITEM_REGISTER(false, this, null);
            }
        }


        private void ChangedGameStatus(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
        {
            _ = eventType;

            if (oldval != newval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        SetActive(!isUnspawned);
                        SetClickable(!isUnspawned && !isUnclickable);
                        SetMotion(!isUnspawned);
                        break;
                }

                switch (oldval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        SetActive(false);
                        SetClickable(false);
                        SetMotion(false);
                        break;
                }
            }
        }

    }
}