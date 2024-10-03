using UnityEngine;

using Gob3AQ.Waypoint.Types;
using Gob3AQ.Waypoint.Network;
using System.Collections.Generic;

namespace Gob3AQ.Waypoint.ProgrammedPath
{
    [System.Serializable]
    public struct WaypointProgrammedPath
    {
        public WaypointNetwork network;
        public WaypointSkillType skill;
        public WaypointSolution originalSolution;
        public float totalDistance;
        public float remainingDistance;
        public bool endIsWaypoint;
        public WaypointConnection actualConnection;
        public WaypointConnection lastConnection;
        public int lastDirection;
        public bool climbing;
        public bool useClimbLineColliders;
        public int lastClimbDirection;
        public List<WaypointConnection> actualGroundConnectionsEvent;
        public WaypointOverGroundState? changeGroundStateBufferedEvent;
        public bool adjacentGroundConnectionEvent;
        public int crossedWaypointIndex;

        public static WaypointProgrammedPath DEFAULT => new WaypointProgrammedPath()
        {
            network = null,
            originalSolution = WaypointSolution.DEFAULT,
            totalDistance = float.PositiveInfinity,
            skill = WaypointSkillType.WAYPOINT_SKILL_UNDEFINED,
            remainingDistance = float.PositiveInfinity,
            endIsWaypoint = true,
            actualConnection = null,
            lastConnection = null,
            lastDirection = 0,
            climbing = false,
            useClimbLineColliders = false,
            lastClimbDirection = 0,
            actualGroundConnectionsEvent = null,
            changeGroundStateBufferedEvent = null,
            adjacentGroundConnectionEvent = false,
            crossedWaypointIndex = -1
        };

        /// <summary>
        /// Gets index of solution path when identifying newConnection on it
        /// </summary>
        /// <param name="newConnection">Connection to identify in path</param>
        /// <returns>Index in solution path list. -1 if not found</returns>
        public int GetSegmentOfPathFromNewConnection(WaypointConnection newConnection)
        {
            int retVal = -1;

            if ((newConnection != null) && (originalSolution.path != null))
            {
                retVal = originalSolution.path.IndexOf(newConnection);
            }

            return retVal;
        }

        public int GetSegmentOfPathFromCrossedWaypoint(WaypointClass crossedWaypoint)
        {
            int retVal = -1;

            if ((originalSolution.waypointTrace != null) && (crossedWaypoint != null))
            {
                retVal = originalSolution.waypointTrace.IndexOf(crossedWaypoint);
            }

            return retVal;
        }

        public void ClearEvents()
        {
            adjacentGroundConnectionEvent = false;
            actualGroundConnectionsEvent = null;
            changeGroundStateBufferedEvent = null;
            crossedWaypointIndex = -1;
        }
    }
}
