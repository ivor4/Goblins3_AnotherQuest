using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.Waypoint.Types
{
    public enum WaypointType
    {
        WAYPOINT_TYPE_GROUND,
        WAYPOINT_TYPE_UNREACHABLE
    }

    public enum WaypointConnectionType
    {
        WAYPOINT_CONNECTION_DISCONNECTED = 0x0,
        WAYPOINT_CONNECTION_NORMAL = 0x1,
    }

    public enum WaypointSkillType
    {
        WAYPOINT_SKILL_UNDEFINED,
        WAYPOINT_SKILL_NORMAL,
        WAYPOINT_SKILL_JUMP, // AKA CLIMB
        WAYPOINT_SKILL_AIR,
        WAYPOINT_SKILL_SWIM,
        WAYPOINT_SKILL_SWIM_JUMP,
        WAYPOINT_SKILL_ALL
    }

    public enum WaypointProgrammedPathState
    {
        WAYPOINTPATH_STATE_UNSTARTED,
        WAYPOINTPATH_STATE_WAYPOINT_REACHED,
        WAYPOINTPATH_STATE_CROSSING_CONNECTION,
        WAYPOINTPATH_STATE_ENDED,
        WAYPOINTPATH_STATE_STUCK
    }

    

    [System.Serializable]
    public class WaypointConnection
    {
        public string name;
        public WaypointClass ownerWaypoint;
        public WaypointClass destWaypoint;
        public WaypointConnectionType type;
        public float distance;
        public bool isReciprocal;
        public WaypointConnection reciprocal;
    }

    /// <summary>
    /// This enum shows state of Player respect Waypoint allocation
    /// </summary>
    public enum WaypointOverGroundState
    {
        /// <summary>
        /// A check must be made on terrain Waypoints (hard CPU)
        /// </summary>
        NO_WAYPOINT_ACKNOWLEDGED,
        /// <summary>
        /// Only a slight update must be made (soft CPU)
        /// </summary>
        ON_GROUND_AND_ACKNOWLEDGED,
        /// <summary>
        /// Trust-in-the-process, if enters in ground and already known Waypoints are not involved in connection, would be like NO_WAYPOINT_ACKNOWLEDGED
        /// </summary>
        FALLING_AND_ACKNOWLEDGED,
        /// <summary>
        /// Trust-in-the-process, not touching ground but climbing on a known path
        /// </summary>
        CLIMBING_AND_ACKNOWLEDGED
    }

    [System.Serializable]
    public struct WaypointSituation
    {
        public List<WaypointConnection> activeConnections;
        public WaypointOverGroundState overGroundState;
        public bool lastTickManagedByCollision;

        public static WaypointSituation DEFAULT => new WaypointSituation()
        {
            activeConnections = new List<WaypointConnection>(),
            overGroundState = WaypointOverGroundState.NO_WAYPOINT_ACKNOWLEDGED,
            lastTickManagedByCollision = false
        };
    }

    [System.Serializable]
    public struct WaypointPreloadConnection
    {
        public WaypointClass withWaypoint;
        public WaypointConnectionType type;
        public int connectedPath;
        public int connectedPoint;
    }


    [System.Serializable]
    public struct WaypointSolutions
    {
        /// <summary>
        /// Dictionary has as Key the destiny Waypoint, and a list of connections to reach it
        /// </summary>
        public List<WaypointPath> shortestPathTo;

        /// <summary>
        /// Dictionary has as Key the destiny Waypoint, and total distance to reach it
        /// </summary>
        public List<float> totalDistanceTo;
    }

    [System.Serializable]
    public struct WaypointSolution
    {
        public List<WaypointConnection> path;
        public List<WaypointClass> waypointTrace;
        public float totalDistance;

        public static WaypointSolution DEFAULT => new WaypointSolution() { path = null, totalDistance = float.PositiveInfinity, waypointTrace = null };
    }

    [System.Serializable]
    public struct WaypointPath
    {
        /// <summary>
        /// List of connections from origin point to dest, but in reverse order as it was stacked. First element is the last connection remaining to reach dest, and so on
        /// </summary>
        public List<WaypointConnection> stackedConnectionList;
    }
}
