using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
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

        private SpriteRenderer _sprRenderer;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private WaypointClass actualWaypoint;
        private GamePickableItem pickable;

        private bool registered;
        private bool loaded;

        void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _sprRenderer.enabled = false;
            _collider.enabled = false;

            
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
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
            }
            else
            {
                gameObject.SetActive(false);
                registered = false;
            }

            loaded = false;
        }

        // Update is called once per frame
        void Update()
        {
            Game_Status gstatus = VARMAP_ItemMaster.GET_GAMESTATUS();

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading();
                    break;

                default:
                    break;
            }
        }

        void OnDisable()
        {
            if(registered)
            {
                VARMAP_ItemMaster.ITEM_REGISTER(false, this);
            }
        }


        private void Execute_Loading()
        {
            if (!loaded)
            {
                VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint);
                loaded = true;
            }
        }
    }
}
