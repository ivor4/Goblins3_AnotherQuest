using UnityEngine;

using Gob3AQ.Waypoint.Types;
using Gob3AQ.Waypoint.Network;
using System.Collections.Generic;

namespace Gob3AQ.Waypoint.ProgrammedPath
{
    [System.Serializable]
    public struct WaypointProgrammedPath
    {
        public readonly WaypointSolution originalSolution;
        public readonly float totalDistance;
        public int crossedWaypointIndex;



        public WaypointProgrammedPath(WaypointSolution solution)
        {
            originalSolution = solution;
            totalDistance = solution.totalDistance;
            crossedWaypointIndex = 0;
        }


        public readonly int GetSegmentOfPathFromCrossedWaypoint(WaypointClass crossedWaypoint)
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
