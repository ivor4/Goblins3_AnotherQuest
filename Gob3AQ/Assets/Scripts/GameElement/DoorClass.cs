using Gob3AQ.VARMAP.Types;
using UnityEngine;

namespace Gob3AQ.GameElement.Item.Door
{
    [System.Serializable]
    public class DoorClass : ItemClass
    {
        [SerializeField]
        private Room _roomLeadTo;

        public Room RoomLead => _roomLeadTo;

        [SerializeField]
        private int _waypointLeadTo;

        public int RoomAppearPosition => _waypointLeadTo;

        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
        }

    }
}
