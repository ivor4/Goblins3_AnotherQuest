using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Item;
using Gob3AQ.ItemMaster;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.GameElement.MapPoint
{

    [System.Serializable]
    public class MapPointClass : GameElementClass
    {
        [SerializeField]
        private Room _roomLeadTo;

        [SerializeField]
        private string _waypointLeadTo;

        private DoorInfo _doorInfo;




        protected override void Awake()
        {
            base.Awake();

            _doorInfo = new DoorInfo(_roomLeadTo, _waypointLeadTo);

            topParent = gameObject;
            topParentTransform = topParent.transform;

            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out int nearestWaypointIndex, out _);

            if (startingWaypoint && startingExposedWaypoint)
            {
                actualWaypoint = startingWaypoint.ID_in_Network;
                exposedWaypoint = startingExposedWaypoint.ID_in_Network;
            }
            else if (!startingWaypoint && !startingExposedWaypoint)
            {
                actualWaypoint = nearestWaypointIndex;
                exposedWaypoint = actualWaypoint;
            }
            else if (startingWaypoint)
            {
                actualWaypoint = startingWaypoint.ID_in_Network;
                exposedWaypoint = actualWaypoint;
            }
            else
            {
                exposedWaypoint = startingExposedWaypoint.ID_in_Network;
                actualWaypoint = exposedWaypoint;
            }

            VARMAP_ItemMaster.DOOR_REGISTER(itemID, true, in _doorInfo);

            InteractionUsage usage = InteractionUsage.CreateCrossDoor(CharacterType.CHARACTER_BLANK, itemID, actualWaypoint);
            VARMAP_ItemMaster.PEEK_ITEM(in usage, out InteractionUsageOutcome outcome);

            /* Doors work in inverse way */
            if (!outcome.ok)
            {
                SetVisible_Internal(false);
                SetClickable_Internal(true);
            }
            else
            {
                SetVisible_Internal(true);
                SetClickable_Internal(false);
            }

            SetAvailable(true);
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            VARMAP_ItemMaster.DOOR_REGISTER(itemID, false, in _doorInfo);
        }
    }
}