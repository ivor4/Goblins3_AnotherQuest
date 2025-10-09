using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.Waypoint;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.GameElement
{
    [System.Serializable]
    public class GameElementClass : MonoBehaviour
    {
        [SerializeField]
        protected GameItem itemID;

        public GameItem ItemID => itemID;

        public GameItemFamily GetGameItemFamily => gameElementFamily;
        public WaypointClass Waypoint => actualWaypoint;

        public bool IsAvailable => isAvailable;

        protected GameItemFamily gameElementFamily;
        protected WaypointClass actualWaypoint;
        protected bool isHovered;
        private bool isAvailable;


        protected void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            isHovered = enter;
            LevelElemInfo info = new((int)itemID, gameElementFamily, actualWaypoint, enter & isAvailable);
            VARMAP_ItemMaster.GAME_ELEMENT_OVER(in info);
        }

        protected void SetAvailable(bool available)
        {
            /* Change in availability */
            if (available ^ IsAvailable)
            {
                isAvailable = available;

                if(isHovered)
                {
                    /* Update hover state */
                    MouseEnterAction(isHovered);
                }
            }
        }
    }
}