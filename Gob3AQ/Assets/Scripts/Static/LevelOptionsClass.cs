using System.Collections;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;

namespace Gob3AQ.Brain.LevelOptions
{
    public static class LevelOptionsClass
    {
        public static int GetLevelDoorToWaypoint(Room room, int doorIndex)
        {
            int waypointIndex = _Level_Door2WaypointIndex[(int)room][doorIndex];
            return waypointIndex;
        }

        /// <summary>
        /// Use -1 for default appear position
        /// </summary>
        private static readonly int[][] _Level_Door2WaypointIndex = new int[(int)Room.ROOMS_TOTAL][]
        {
            /* ROOM1 */
            new int[GameFixedConfig.MAX_SCENE_DOORS]
            {
                -1, -1
            },
            /* ROOM1 KITCHEN */
            new int[GameFixedConfig.MAX_SCENE_DOORS]
            {
                -1, -1
            },
            /* ROOM LAST */
            new int[GameFixedConfig.MAX_SCENE_DOORS]
            {
                -1, -1
            }
        };
    }
}



