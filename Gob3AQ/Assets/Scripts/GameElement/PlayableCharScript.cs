using Gob3AQ.FixedConfig;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gob3AQ.GameElement.PlayableChar
{
 
    [System.Serializable]
    public class PlayableCharScript : GameElementClass
    {
        /* Fields */
        [SerializeField]
        private CharacterType charType;

        public CharacterType CharType => charType;
     


        #region "Services"

        public void LockRequest(bool enablelock)
        {
            if (enablelock)
            {
                if (IsAvailable)
                {
                    /* Lock the character */
                    physicalstate = PhysicalState.PHYSICAL_STATE_LOCKED;
                }
            }
            else
            {
                if(physicalstate == PhysicalState.PHYSICAL_STATE_LOCKED)
                {
                    /* Unlock the character */
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                }
            }
        }

#endregion



        protected override void Awake()
        {
            base.Awake();

            topParent = transform.parent.gameObject;
            topParentTransform = topParent.transform;
            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();
            myAnimator = topParent.GetComponent<Animator>();

            SetVisible_Internal(false);
        }

        protected override void Start()
        {
            base.Start();

            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            SetAvailable(true);

            PlayerMasterClass.SetPlayerLoadPresent(CharType);

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);

            /* Start loading coroutine */
            _ = StartCoroutine(Execute_Loading_Coroutine());
        }

        protected override void Update()
        {
            base.Update();

            /* Script will only be enabled in play mode */
            Game_Status gstatus = VARMAP_PlayerMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    Execute_Play();
                    break;
            }
            
        }

        protected override void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)(topParentTransform.position.y * 1000);
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
        }


        #region "Private Methods "

        protected override void ReachedWaypointFunction(int wp_index)
        {
            VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, wp_index);
            exposedWaypoint = wp_index;
        }

        private IEnumerator Execute_Loading_Coroutine()
        {
            bool loadOk = false;

            while (!loadOk)
            {
                loadOk = Execute_Loading_Action();
                yield return ResourceAtlasClass.WaitForNextFrame;
            }
        }

        private bool Execute_Loading_Action()
        {
            VARMAP_PlayerMaster.IS_MODULE_LOADED(GameModules.MODULE_LevelMaster, out bool loadOk);

            if (!loadOk) return false;
            
            int wpStartIndex = VARMAP_PlayerMaster.GET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)charType);


            actualWaypoint = wpStartIndex == -1 ? 0 : wpStartIndex;
            exposedWaypoint = actualWaypoint;

            topParentTransform.position = waypoints_infos[actualWaypoint].Position;

            PresetProgrammedPathStruct(actualWaypoint);
            SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

            VARMAP_PlayerMaster.PLAYER_WAYPOINT_UPDATE(charType, actualWaypoint);

            mySpriteRenderer.flipX = waypoints_infos[actualWaypoint].FlipXForAction;

            SetVisible_Internal(true);
            PlayerMasterClass.SetPlayerLoaded(CharType);

            PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE, null, null);
            ExecuteQueuedTrigger();

            UpdateSortingOrder();


            return true;
        }

        private void Execute_Play()
        {
            UpdateSortingOrder();
            SetAvailable((physicalstate == PhysicalState.PHYSICAL_STATE_STANDING) || (physicalstate == PhysicalState.PHYSICAL_STATE_WALKING));
        }
#endregion

    }

}
