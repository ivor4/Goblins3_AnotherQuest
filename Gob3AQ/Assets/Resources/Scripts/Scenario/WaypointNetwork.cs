using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.Waypoint.Types;

namespace Gob3AQ.Waypoint.Network
{
    [System.Serializable]
    public class WaypointNetwork
    {
        public const float IMPOSSIBLEDISTANCE = 1000000f;

        [SerializeField]
        private List<WaypointClass> waypointList;

        public List<WaypointClass> WaypointList => waypointList;

        /// <summary>
        /// Solutions for normal hability
        /// </summary>
        [SerializeField]
        private List<WaypointSolutions> resolvedSolutions_normal;


        private Dictionary<WaypointSkillType, List<WaypointSolutions>> resolvedSolutionsByType;

        private bool dirty;

        public bool IsDirty => dirty;

        public WaypointNetwork()
        {
            waypointList = new List<WaypointClass>();
            resolvedSolutions_normal = new List<WaypointSolutions>();

            resolvedSolutionsByType = new Dictionary<WaypointSkillType, List<WaypointSolutions>>();

            resolvedSolutionsByType[WaypointSkillType.WAYPOINT_SKILL_NORMAL] = resolvedSolutions_normal;

            dirty = false;
        }





        public void AddIsolatedWaypoint(WaypointClass wp)
        {
            waypointList.Add(wp);
            dirty = true;
        }

        /// <summary>
        /// Adds a Waypoint to a network, connected to another Waypoint which is already in network
        /// </summary>
        /// <param name="wp">New waypoint to connect (its network and connections will be reseted)</param>
        /// <param name="connectedWith">Already existing waypoint in Network</param>
        public void AddWayPointConnectedTo(WaypointClass wp, WaypointClass connectedWith, WaypointConnectionType type = WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL)
        {
            if (connectedWith.Network == this)
            {
                wp.ResetNetoworkAndConnections();
                waypointList.Add(wp);
                wp.SetNetwork(this, waypointList.Count - 1);
                connectedWith.ConnectPoint(wp, type);

                dirty = true;
            }
            else
            {
                Debug.LogError("Waypoint to connect with doesn't belong to network");
            }
        }

        /// <summary>
        /// Insertes a Waypoint in the middle of a connection, breaking that original connection and becomming an intermediate (not geometrically)
        /// </summary>
        /// <param name="wp">New waypoint to add (its original network and connections will be reseted)</param>
        /// <param name="connection">Connection to break to insert between</param>
        /// <param name="type">New connection type for two new connections</param>
        public void InsertWaypointBetweenConnection(WaypointClass wp, WaypointConnection connection, WaypointConnectionType type = WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL)
        {
            WaypointClass originWp = connection.ownerWaypoint;
            WaypointClass destWp = connection.destWaypoint;

            if (originWp.Network == this)
            {
                wp.ResetNetoworkAndConnections();
                waypointList.Add(wp);
                wp.SetNetwork(this, waypointList.Count - 1);

                originWp.DisconnectConnection(connection);

                originWp.ConnectPoint(wp, type);
                wp.ConnectPoint(destWp, type);

                dirty = true;
            }
            else
            {
                Debug.LogError("One of waypoints of original connection doesn't belong to Network");
            }
        }

        /// <summary>
        /// Also valid to explore all connected points in a network
        /// </summary>
        /// <param name="wp">One of the waypoints of the network</param>
        public void AddWaypointAndItsBranch(WaypointClass wp)
        {
            List<WaypointClass> visitedWaypoints = new List<WaypointClass>();
            Stack<WaypointClass> waypointStack = new Stack<WaypointClass>();

            waypointList.Add(wp);
            visitedWaypoints.Add(wp);

            wp.SetNetwork(this, waypointList.Count - 1);

            WaypointClass inspectedWaypoint = wp;
            waypointStack.Push(wp);
            do
            {
                List<WaypointConnection> inspectedConnections = inspectedWaypoint.Connections;

                bool newPush = false;

                for (int i = 0; i < inspectedConnections.Count; i++)
                {
                    WaypointClass newPushCandidate = inspectedConnections[i].destWaypoint;

                    if (!waypointList.Contains(newPushCandidate))
                    {
                        waypointList.Add(newPushCandidate);
                        newPushCandidate.SetNetwork(this, waypointList.Count - 1);
                    }

                    if (!visitedWaypoints.Contains(newPushCandidate))
                    {
                        newPush = true;
                        visitedWaypoints.Add(newPushCandidate);
                        waypointStack.Push(newPushCandidate);
                        inspectedWaypoint = newPushCandidate;
                        break;
                    }
                }

                if (!newPush)
                {
                    _ = waypointStack.Pop();
                    if (waypointStack.Count > 0)
                    {
                        inspectedWaypoint = waypointStack.Peek();
                    }
                }
            } while (waypointStack.Count > 0);

            dirty = true;
        }

        public bool IsWaypointOnNetwork(WaypointClass wp)
        {
            return waypointList.Contains(wp);
        }


        public WaypointSolution GetWaypointSolution(WaypointClass source, WaypointClass dest, WaypointSkillType availableSkill, List<WaypointClass> waypointTrace)
        {
            if (waypointList.Contains(source) && waypointList.Contains(dest))
            {
                int wp1Index = source.IndexInNetwork;
                int wp2Index = dest.IndexInNetwork;

                List<WaypointSolutions> solutions = resolvedSolutionsByType[availableSkill];

                WaypointSolutions sourceSolutions = solutions[wp1Index];

                List<WaypointConnection> solutionConnections = sourceSolutions.shortestPathTo[wp2Index].stackedConnectionList;
                float solutionDistance = sourceSolutions.totalDistanceTo[wp2Index];

                waypointTrace.Clear();
                WaypointClass lastWpTrace = source;

                waypointTrace.Add(lastWpTrace);

                /* Extract waypoint trace */
                for (int i = 0; i < solutionConnections.Count; i++)
                {
                    WaypointConnection inspectedConnection = solutionConnections[i];

                    if (lastWpTrace != inspectedConnection.ownerWaypoint)
                    {
                        waypointTrace.Add(inspectedConnection.ownerWaypoint);
                        lastWpTrace = inspectedConnection.ownerWaypoint;
                    }
                    else
                    {
                        waypointTrace.Add(inspectedConnection.destWaypoint);
                        lastWpTrace = inspectedConnection.destWaypoint;
                    }
                }

                return new WaypointSolution() { totalDistance = solutionDistance, waypointTrace = waypointTrace };
            }
            else
            {
                Debug.LogError("Point wp1 or wp2 doesn't belong to WaypointNetwork");
                return WaypointSolution.DEFAULT;
            }
        }

        public void CalculatePaths()
        {
            resolvedSolutions_normal.Clear();

            /* Calculate for all points */
            for (int i = 0; i < waypointList.Count; i++)
            {
                WaypointSolutions solution;

                WaypointClass genesisWaypoint = waypointList[i];

                solution = CalculateWaypointPath(genesisWaypoint, (ushort)WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL);
                resolvedSolutions_normal.Add(solution);
            }

            dirty = false;
        }

        /// <summary>
        /// Dikjstra algorithm
        /// </summary>
        /// <param name="genesisWaypoint">Point to calculate its relative connections</param>
        /// <param name="availabletype">Power to consider to cross paths</param>
        /// <returns></returns>
        private WaypointSolutions CalculateWaypointPath(WaypointClass genesisWaypoint, ushort availableConnectionTypeBitField)
        {
            WaypointSolutions newSolution = new WaypointSolutions();

            newSolution.totalDistanceTo = new List<float>();
            newSolution.shortestPathTo = new List<WaypointPath>();

            /* Pre-fill lists */
            for (int i = 0; i < waypointList.Count; i++)
            {
                WaypointPath newPath = new WaypointPath();
                newPath.stackedConnectionList = new List<WaypointConnection>();

                if (i == genesisWaypoint.IndexInNetwork)
                {
                    newSolution.totalDistanceTo.Add(0);
                }
                else
                {
                    newSolution.totalDistanceTo.Add(float.PositiveInfinity);
                }

                newSolution.shortestPathTo.Add(newPath);
            }

            /* Create stacks of connection and distance - Must be operated together */
            Stack<float> distanceStack = new Stack<float>();
            Stack<WaypointConnection> connectionStack = new Stack<WaypointConnection>();

            /* Waypoint stack goes one element ahead (the starting point) */
            Stack<WaypointClass> waypointStack = new Stack<WaypointClass>();

            /* Define starting point with distance to its own - 0 */
            WaypointClass inspectedWaypoint = genesisWaypoint;
            float accumulatedDistance = 0f;

            /* First element in stack relative to genesis point */
            waypointStack.Push(inspectedWaypoint);

            do
            {
                List<WaypointConnection> inspectedConnections = inspectedWaypoint.Connections;

                bool newPush = false;

                for (int i = 0; i < inspectedConnections.Count; i++)
                {
                    WaypointConnection inspectedConnection = inspectedConnections[i];

                    bool pass;

                    /* Check if some of given skills matched type of connection */
                    pass = ((ushort)inspectedConnection.type & availableConnectionTypeBitField) > 0;


                    /* Get new candidate, even if it was already visited */
                    WaypointClass newPushCandidate = inspectedConnection.destWaypoint;
                    int indexOfCandidate = newPushCandidate.IndexInNetwork;

                    /* Solution distance starts being Infinity, and goes correction by correction in a way to its minimum */
                    float solutionDistance = newSolution.totalDistanceTo[indexOfCandidate];

                    /* Proposed distance for this branching */
                    float proposedDistance = accumulatedDistance + inspectedConnections[i].distance;

                    /* If it is not possible to pass, connection still exists, but won't be the best, and when reaching the point, if it's the only way, Player should stop */
                    if (!pass)
                    {
                        proposedDistance += IMPOSSIBLEDISTANCE;
                    }

                    /* If proposed distance is < solution distance, then this path will become temporally in official one, and branch will advance through it */
                    if (proposedDistance < solutionDistance)
                    {
                        newSolution.totalDistanceTo[indexOfCandidate] = proposedDistance;
                        List<WaypointConnection> connList = newSolution.shortestPathTo[indexOfCandidate].stackedConnectionList;

                        /* Prepare next cycle */
                        newPush = true;
                        inspectedWaypoint = newPushCandidate;
                        accumulatedDistance = proposedDistance;
                        waypointStack.Push(newPushCandidate);
                        connectionStack.Push(inspectedConnection);
                        distanceStack.Push(proposedDistance);

                        /* Update connList with actual path from genesis to point */
                        connList.Clear();
                        connList.AddRange(connectionStack);

                        break;
                    }
                }

                /* Go back one step before in branching */
                if (!newPush)
                {
                    /* Discard actual values, as connections and distances have always one element less than waypointStack, checking is mandatory */
                    if (connectionStack.Count > 0)
                    {
                        _ = connectionStack.Pop();
                        _ = distanceStack.Pop();

                        if (distanceStack.Count > 0)
                        {
                            accumulatedDistance = distanceStack.Peek();
                        }
                        else
                        {
                            accumulatedDistance = 0f;
                        }
                    }
                    else
                    {
                        accumulatedDistance = 0f;
                    }

                    /* Discard this value which gave no new pushed elements */
                    _ = waypointStack.Pop();

                    /* If not ended, take new values without removing them */
                    if (waypointStack.Count > 0)
                    {
                        inspectedWaypoint = waypointStack.Peek();
                    }
                }
            } while (waypointStack.Count > 0);

            /* Reverse */
            foreach(WaypointPath wpath in newSolution.shortestPathTo)
            {
                wpath.stackedConnectionList.Reverse();
            }

            return newSolution;
        }
    }
}