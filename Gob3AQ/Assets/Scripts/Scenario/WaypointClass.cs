using Gob3AQ.VARMAP.Types;
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

        [SerializeField]
        private List<WaypointClass> connectedWaypoints;

        [SerializeField]
        [Tooltip("Assign Manually or by Tool")]
        private WaypointNetwork network;

        [SerializeField]
        private float characterSizeFactor;

        [SerializeField]
        private WaypointReachability reachability;

        [SerializeField]
        private GameEventCombi_prv neededEvent;

        [SerializeField]
        private GameAction actionWhenCross;

        [SerializeField]
        private bool flipXForAction;

        public float CharacterSizeFactor => characterSizeFactor;
        public int ID_in_Network => id_in_network;
        public IReadOnlyList<WaypointClass> ConnectedWaypoints => connectedWaypoints;
        public WaypointReachability Reachability => reachability;
        public GameEventCombi_prv NeededEvent => neededEvent;
        public GameAction ActionWhenCross => actionWhenCross;
        public bool FlipXForAction => flipXForAction;


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