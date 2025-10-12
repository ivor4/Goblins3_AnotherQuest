using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using Gob3AQ.VARMAP.LevelMaster;

namespace Gob3AQ.GameElement.Door
{
    [System.Serializable]
    public class DoorClass : MonoBehaviour
    {
        [SerializeField]
        private WaypointClass _waypoint;

        [SerializeField]
        private GameEvent _neededEvent;

        [SerializeField]
        private Room _roomLead;

        [SerializeField]
        private int _roomAppearPosition;

        private Collider2D _collider;
        
        private SpriteRenderer _sprrend;
        private Rigidbody2D _rigidbody;

        private bool _subscribed;
        private bool _registered;

        public Room RoomLead => _roomLead;
        public int RoomAppearPosition => _roomAppearPosition;

        public Collider2D Collider => _collider;

        public WaypointClass Waypoint => _waypoint;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _sprrend = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _collider.enabled = false;
            _sprrend.enabled = false;

            _subscribed = false;
            _registered = false;
        }

        private void Start()
        {
            if (_neededEvent != GameEvent.EVENT_NONE)
            {
                VARMAP_LevelMaster.IS_EVENT_OCCURRED(_neededEvent, out bool occurred);

                if(occurred)
                {
                    _ActivateDoor();
                }
                else
                {
                    VARMAP_LevelMaster.EVENT_SUBSCRIPTION(_neededEvent, _NeededEventChanged, true);
                    _subscribed = true;
                }
            }
        }

        private void OnDestroy()
        {
            if(_subscribed)
            {
                VARMAP_LevelMaster.EVENT_SUBSCRIPTION(_neededEvent, _NeededEventChanged, false);
            }

            if(_registered)
            {
                VARMAP_LevelMaster.DOOR_REGISTER(this, false);
            }
        }

        private void _NeededEventChanged(bool status)
        {
            if(status)
            {
                _ActivateDoor();
            }
        }

        private void _ActivateDoor()
        {
            _sprrend.enabled = true;
            _collider.enabled = true;

            VARMAP_LevelMaster.DOOR_REGISTER(this, true);

            _registered = true;
        }
    }
}
