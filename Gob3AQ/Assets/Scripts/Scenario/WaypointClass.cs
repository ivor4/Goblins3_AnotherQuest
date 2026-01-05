


using Gob3AQ.Waypoint.Network;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.Waypoint
{

    [System.Serializable]
    public class WaypointClass : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Assign Manually or by Tool")]
        private int id_in_network;

        public int ID_in_Network => id_in_network;

        [SerializeField]
        private List<WaypointClass> connectedWaypoints;

        public IReadOnlyList<WaypointClass> ConnectedWaypoints => connectedWaypoints;

        [SerializeField]
        [Tooltip("Assign Manually or by Tool")]
        private WaypointNetwork network;

        [SerializeField]
        private float characterSizeFactor;

        public WaypointNetwork Network => network;

        public float CharacterSizeFactor => characterSizeFactor;


#if UNITY_EDITOR
        public void SetID(int id)
        {
            id_in_network = id;
        }

        public void SetNetwork(WaypointNetwork nw)
        {
            network = nw;
        }

        public void ConnectWith(WaypointClass w)
        {
            if(!connectedWaypoints.Contains(w))
            {
                connectedWaypoints.Add(w);
            }
        }

        public void DisconnectWith(WaypointClass w)
        {
            connectedWaypoints.Remove(w);
        }

        private void OnDrawGizmos()
        {
            foreach (WaypointClass dest in ConnectedWaypoints)
            {
                Gizmos.DrawLine(transform.position, dest.transform.position);
            }
        }
#endif
    }
}