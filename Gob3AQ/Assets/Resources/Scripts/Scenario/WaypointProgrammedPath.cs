using UnityEngine;

using Gob3AQ.Waypoint.Types;
using Gob3AQ.Waypoint.Network;
using System.Collections.Generic;

namespace Gob3AQ.Waypoint.ProgrammedPath
{
    [System.Serializable]
    public struct WaypointProgrammedPath
    {
        public WaypointSolution originalSolution;
        public float totalDistance;
        public float remainingDistance;
        public int crossedWaypointIndex;

        public static WaypointProgrammedPath DEFAULT => new WaypointProgrammedPath()
        {
            originalSolution = WaypointSolution.DEFAULT,
            totalDistance = float.PositiveInfinity,
            remainingDistance = float.PositiveInfinity,
            crossedWaypointIndex = -1
        };

        public WaypointProgrammedPath(WaypointSolution solution)
        {
            originalSolution = solution;
            totalDistance = solution.totalDistance;
            remainingDistance = totalDistance;
            crossedWaypointIndex = 0;
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

    }
}
