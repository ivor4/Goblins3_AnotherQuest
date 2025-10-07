using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.Waypoint;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.GameElement
{
    public class GameElement : MonoBehaviour
    {
        public GameElementType GetGameElementType => gameElementType;
        public WaypointClass Waypoint => actualWaypoint;

        protected GameElementType gameElementType;
        protected WaypointClass actualWaypoint;
    }
}