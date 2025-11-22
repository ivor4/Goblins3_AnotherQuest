using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System.Collections;
using UnityEngine;

namespace Gob3AQ.GameElement.Item.Door
{
    

    [System.Serializable]
    public class DoorClass : GameElementClass
    {
        [SerializeField]
        private Room _roomLeadTo;

        [SerializeField]
        private int _waypointLeadTo;

        private DoorInfo _doorInfo;

        protected override void Awake()
        {
            base.Awake();

            topParent = transform.parent.gameObject;

            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();

            _doorInfo = new DoorInfo(_roomLeadTo, _waypointLeadTo);

            SetVisible_Internal(false);
            SetClickable_Internal(false);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            VARMAP_ItemMaster.DOOR_REGISTER(itemID, true, in _doorInfo);

            /* Execute on next Update */
            _ = StartCoroutine(Execute_Loading());
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            VARMAP_ItemMaster.DOOR_REGISTER(itemID, false, in _doorInfo);
        }


        protected virtual IEnumerator Execute_Loading()
        {
            bool loaded = false;

            while (!loaded)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out loaded);
            }

            Loading_Task();
        }

        protected virtual void Loading_Task()
        {
            if (startingWaypoint == null)
            {
                VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint, out _);
            }
            else
            {
                actualWaypoint = startingWaypoint.ID_in_Network;
            }

            /* Set to default sprite */
            mySpriteRenderer.sprite = null;

            SetVisible_Internal(false);
            SetClickable_Internal(true);
            SetAvailable(true);

            loaded = true;
        }

    }
}
