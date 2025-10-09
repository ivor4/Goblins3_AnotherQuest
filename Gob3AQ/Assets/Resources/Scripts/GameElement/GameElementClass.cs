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

        public GameItem ItemID => itemID;

        public GameItemFamily GetGameItemFamily => gameElementFamily;
        public WaypointClass Waypoint => actualWaypoint;

        public bool IsAvailable => isAvailable;

        protected GameItemFamily gameElementFamily;
        protected WaypointClass actualWaypoint;
        protected bool isHovered;
        protected GameObject topParent;
        protected Collider2D myCollider;
        protected SpriteRenderer mySpriteRenderer;
        protected Rigidbody2D myRigidbody;
        protected bool registered;
        private bool isAvailable;
        private bool isActive_int;
        private bool isActive_ext;
        private bool isVisible_int;
        private bool isVisible_ext;
        private bool isClickable_int;
        private bool isClickable_ext;
        private bool isMotion_int;
        private bool isMotion_ext;

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
        }

        protected virtual void Start()
        {
            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);
        }


        protected virtual void OnDestroy()
        {
            VirtualDestroy();
            
        }

        protected void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            isHovered = enter;
            LevelElemInfo info = new((int)itemID, gameElementFamily, actualWaypoint, enter & isAvailable);
            VARMAP_ItemMaster.GAME_ELEMENT_OVER(in info);
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
                VARMAP_ItemMaster.ITEM_REGISTER(false, this);
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
                        SetActive(true);
                        SetClickable(true);
                        SetMotion(true);
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