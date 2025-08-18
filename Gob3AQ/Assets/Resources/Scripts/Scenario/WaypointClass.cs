using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.Waypoint.Types;
using Gob3AQ.Waypoint.Network;
using Gob3AQ.VARMAP.LevelMaster;
using System.Threading.Tasks;
using Gob3AQ.LevelMaster;

namespace Gob3AQ.Waypoint
{

    [System.Serializable]
    public class WaypointClass : MonoBehaviour
    {
        public List<WaypointPreloadConnection> PreloadConnections;

        public WaypointType PreloadType;

        public bool CreateNetwork = false;

        [Tooltip("Do not change by this in inspector, this is only the feedback, do it with PreloadType")]
        [SerializeField]
        private WaypointType type;

        public WaypointType Type => type;

        [SerializeField]
        private WaypointNetwork network;
        public WaypointNetwork Network => network;


        [SerializeField]
        private int indexInNetwork;

        public int IndexInNetwork => indexInNetwork;

        [SerializeField]
        [Tooltip("Do not modify this, this will be the result from adapting the preset network")]
        private List<WaypointConnection> connections;

        public List<WaypointConnection> Connections => connections;




        void Awake()
        {
            connections = new List<WaypointConnection>();

            indexInNetwork = -1;

            network = null;
        }

        void Start()
        {
            /* Make connection of preloaded connections */
            if ((PreloadConnections != null) && (PreloadConnections.Count > 0))
            {
                for (int i = 0; i < PreloadConnections.Count; i++)
                {
                    ConnectPoint(PreloadConnections[i].withWaypoint, PreloadConnections[i].type);
                }
            }

            /* Subscribe only if this WP will create the whole Network */
            if (CreateNetwork)
            {
                VARMAP_LevelMaster.LATE_START_SUBSCRIPTION(LateStart, true);
            }


            type = PreloadType;

            VARMAP_LevelMaster.WP_REGISTER(this, true);
        }


        public async void LateStart()
        {
            await Task.Run(ThreadedCalculatePaths);
            LevelMasterClass.DeclareAllWaypointsLoaded();
        }

        private void ThreadedCalculatePaths()
        {
            DateTime before = DateTime.Now;

            if (network == null)
            {
                /* This will set network variable */
                WaypointNetwork wpnet = new WaypointNetwork();
                wpnet.AddWaypointAndItsBranch(this);
            }

            /* This could also be valid just after above assign */
            network?.CalculatePaths();

            DateTime after = DateTime.Now;

            TimeSpan duration = after.Subtract(before);

            Debug.LogWarning("Dijkstra operation lasted: " + duration.Milliseconds + "ms");
        }


        void OnDestroy()
        {
            VARMAP_LevelMaster.LATE_START_SUBSCRIPTION(LateStart, false);
        }

        public void ResetNetoworkAndConnections()
        {
            network = null;
            indexInNetwork = -1;

            for (int i = 0; i < connections.Count; i++)
            {
                if (DisconnectConnection(connections[i]))
                {
                    i--;
                }
            }
            connections.Clear();
        }

        public void DisconnectPoint(WaypointClass waypoint)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                WaypointConnection conn = connections[i];

                WaypointClass otherWp = conn.destWaypoint;
                if (otherWp == waypoint)
                {
                    otherWp.DetachConnection(conn.reciprocal);
                    DetachConnection(conn);
                    break;
                }
            }
        }

        public bool DisconnectConnection(WaypointConnection wpconn)
        {
            if (connections.Contains(wpconn))
            {
                WaypointClass otherWp = wpconn.destWaypoint;
                otherWp.DetachConnection(wpconn.reciprocal);
                DetachConnection(wpconn);
                return true;
            }
            return false;
        }

        public void SetNetwork(WaypointNetwork wpnet, int index)
        {
            network = wpnet;

            indexInNetwork = index;
        }

        public void ConnectPoint(WaypointClass destWaypoint, WaypointConnectionType type)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                WaypointConnection inspectedConnection = connections[i];

                if (inspectedConnection.destWaypoint == destWaypoint)
                {
                    /* Duplicated connection (made by another node) */
                    return;
                }
            }
            WaypointConnection newConn = new WaypointConnection();
            WaypointConnection reciprocalConn = new WaypointConnection();

            float distance = Vector2.Distance(destWaypoint.transform.position, transform.position);

            newConn.name = name + "-" + destWaypoint.name;
            newConn.ownerWaypoint = this;
            newConn.destWaypoint = destWaypoint;
            newConn.distance = distance;
            newConn.isReciprocal = false;
            newConn.type = type;

            reciprocalConn.name = destWaypoint.name + "-" + name;
            reciprocalConn.ownerWaypoint = destWaypoint;
            reciprocalConn.destWaypoint = this;
            reciprocalConn.distance = distance;
            reciprocalConn.isReciprocal = true;
            reciprocalConn.type = type;


            newConn.reciprocal = reciprocalConn;
            reciprocalConn.reciprocal = newConn;

            AttachConnection(newConn);
            destWaypoint.AttachConnection(reciprocalConn);

        }

        public static void PreloadWaypointConnection(WaypointClass wp1, WaypointClass wp2, WaypointConnectionType type = WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL)
        {
            WaypointPreloadConnection prelo = new WaypointPreloadConnection();
            prelo.type = type;
            prelo.withWaypoint = wp2;
            wp1.PreloadConnections.Add(prelo);

            prelo.withWaypoint = wp1;
            wp2.PreloadConnections.Add(prelo);
        }

        public static void PreloadWaypointDisconnection(WaypointClass wp1, WaypointClass wp2)
        {
            for (int i = 0; i < wp1.PreloadConnections.Count; i++)
            {
                if (wp1.PreloadConnections[i].withWaypoint == wp2)
                {
                    wp1.PreloadConnections.RemoveAt(i);
                    break;
                }
            }


            for (int i = 0; i < wp2.PreloadConnections.Count; i++)
            {
                if (wp2.PreloadConnections[i].withWaypoint == wp1)
                {
                    wp2.PreloadConnections.RemoveAt(i);
                    break;
                }
            }
        }

        public static bool IsPreloadConnectedWith(WaypointClass wp1, WaypointClass wp2)
        {
            bool retVal = false;

            for (int i = 0; i < wp1.PreloadConnections.Count; i++)
            {
                if (wp1.PreloadConnections[i].withWaypoint == wp2)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the common joint Waypoint between two connections in case they are directly connected
        /// </summary>
        /// <param name="conn1">Waypoint connection A</param>
        /// <param name="conn2">Waypoint connection B</param>
        /// <returns></returns>
        public static WaypointClass GetConnectionWaypoint(WaypointConnection conn1, WaypointConnection conn2)
        {
            WaypointClass common;

            if (conn1.reciprocal == conn2)
            {
                common = null;
            }
            else
            {
                if (conn1.ownerWaypoint == conn2.ownerWaypoint)
                {
                    common = conn1.ownerWaypoint;
                }
                else if (conn1.ownerWaypoint == conn2.destWaypoint)
                {
                    common = conn1.ownerWaypoint;
                }
                else if (conn1.destWaypoint == conn2.ownerWaypoint)
                {
                    common = conn1.destWaypoint;
                }
                else if (conn1.destWaypoint == conn2.destWaypoint)
                {
                    common = conn1.destWaypoint;
                }
                else
                {
                    common = null;
                }
            }

            return common;
        }

        /// <summary>
        /// Low level function. This doesn't check if connection is already present
        /// </summary>
        /// <param name="connection">Connection instance</param>
        private void AttachConnection(WaypointConnection connection)
        {
            connections.Add(connection);
        }

        private void DetachConnection(WaypointConnection connection)
        {
            connections.Remove(connection);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            if (Application.isPlaying)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    if (!connections[i].isReciprocal)
                    {
                        switch (connections[i].type)
                        {
                            case WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL:
                                Gizmos.color = Color.white;
                                break;
                            case WaypointConnectionType.WAYPOINT_CONNECTION_DISCONNECTED:
                                Gizmos.color = Color.red;
                                break;
                            default:
                                Gizmos.color = Color.yellow;
                                break;
                        }
                        /* Avoid repeated lines */
                        Gizmos.DrawLine(transform.position, connections[i].destWaypoint.transform.position);
                    }
                }
            }
            else
            {
                for (int i = 0; i < PreloadConnections.Count; i++)
                {
                    switch (PreloadConnections[i].type)
                    {
                        case WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL:
                            Gizmos.color = Color.white;
                            break;
                        case WaypointConnectionType.WAYPOINT_CONNECTION_DISCONNECTED:
                            Gizmos.color = Color.red;
                            break;
                        default:
                            Gizmos.color = Color.yellow;
                            break;
                    }
                    /* Avoid repeated lines */
                    Gizmos.DrawLine(transform.position, PreloadConnections[i].withWaypoint.transform.position);
                }
            }
        }
#endif

        

        
    }



    
}