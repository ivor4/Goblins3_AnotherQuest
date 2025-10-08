using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.Waypoint;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.GameElement
{
    [System.Serializable]
    public class GameElement : MonoBehaviour
    {
        [SerializeField]
        protected GameItem itemID;

        public GameItem ItemID => itemID;

        public GameItemFamily GetGameItemFamily => gameElementFamily;
        public WaypointClass Waypoint => actualWaypoint;

        public bool IsAvailable => isAvailable;

        protected GameItemFamily gameElementFamily;
        protected WaypointClass actualWaypoint;
        protected bool isAvailable;
    }
}