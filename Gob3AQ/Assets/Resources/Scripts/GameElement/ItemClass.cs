using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types.Items;
using Gob3AQ.Waypoint;
using Gob3AQ.Brain.ItemsInteraction;



namespace Gob3AQ.GameElement.Item
{
    [System.Serializable]
    public class ItemClass : MonoBehaviour
    {
        [SerializeField]
        public GameItem itemID;

        public GameItem ItemID
        {
            get
            {
                return itemID;
            }
        }

        public Collider2D Collider
        {
            get
            {
                return _collider;
            }
        }

        public WaypointClass Waypoint
        {
            get
            {
                return actualWaypoint;
            }
        }

        public GamePickableItem Pickable
        {
            get
            {
                return pickable;
            }
        }

        protected SpriteRenderer _sprRenderer;
        protected Collider2D _collider;
        protected Rigidbody2D _rigidbody;
        protected WaypointClass actualWaypoint;
        protected GamePickableItem pickable;

        protected bool registered;

        void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _sprRenderer.enabled = false;
            _collider.enabled = false;

            
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            bool taken;
            pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)itemID];

            /* If it is a pickable item, may have been picked before */
            if (pickable != GamePickableItem.ITEM_PICK_NONE)
            {
                VARMAP_ItemMaster.IS_ITEM_TAKEN_FROM_SCENE(pickable, out taken);
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

                VARMAP_ItemMaster.LATE_START_SUBSCRIPTION(Execute_Loading, true);
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
                VARMAP_ItemMaster.LATE_START_SUBSCRIPTION(Execute_Loading, false);
            }
        }


        protected virtual void Execute_Loading()
        {
            VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint);
        }
    }
}
