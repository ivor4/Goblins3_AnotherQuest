using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        protected GameSprite actualSprite;
        protected HashSet<GameEvent> subscribedEvents;
        protected GameItemFamily gameElementFamily;
        protected WaypointClass actualWaypoint;
        protected bool isHovered;
        protected GameObject topParent;
        protected Collider2D myCollider;
        protected SpriteRenderer mySpriteRenderer;
        protected Rigidbody2D myRigidbody;
        protected bool registered;
        protected bool loaded;
        private bool isAvailable;
        private bool isActive_int;
        private bool isActive_ext;
        private bool isVisible_int;
        private bool isVisible_ext;
        private bool isClickable_int;
        private bool isClickable_ext;
        private bool isMotion_int;
        private bool isMotion_ext;

        protected virtual void Awake()
        {
            isActive_int = true;
            isActive_ext = true;
            isVisible_int = true;
            isVisible_ext = true;
            isClickable_int = true;
            isClickable_ext = true;
            isMotion_int = true;
            isMotion_ext = true;
        }

        protected virtual void Start()
        {
            loaded = false;

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            actualSprite = itemInfo.Sprites[0];

            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);

            UpdateSortingOrder();
        }


        protected virtual void OnDestroy()
        {
            VirtualDestroy();
            
        }

        protected void SetSprite(GameSprite newSprite)
        {
            actualSprite = newSprite;

            if (loaded)
            {
                mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(actualSprite);
            }
        }

        protected bool CheckSpawnConditions(bool register)
        {
            bool despawn = false;
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);
            ReadOnlySpan<SpawnConditions> spawnConditions = itemInfo.SpawnConditions;

            for (int i = 0; (i < spawnConditions.Length) && (!despawn); i++)
            {
                SpawnConditions spawnCondition = spawnConditions[i];

                if (spawnCondition != SpawnConditions.SPAWN_COND_NONE)
                {
                    ref readonly SpawnConditionInfo conditionInfo = ref ItemsInteractionsClass.GetSpawnConditionInfo(spawnCondition);
                    ReadOnlySpan<GameEventCombi> neededEvents = conditionInfo.Events;

                    /* First, check if already complied */
                    VARMAP_ItemMaster.IS_EVENT_COMBI_OCCURRED(neededEvents, out bool complied);

                    if (complied)
                    {
                        despawn = SpawnConditionComplied(in conditionInfo);
                    }
                    else if (register)
                    {
                        /* Register event for late actions */
                        for (int j = 0; j < neededEvents.Length; j++)
                        {
                            ref readonly GameEventCombi eventCombi = ref conditionInfo.Events[j];

                            if (eventCombi.eventType != GameEvent.EVENT_NONE)
                            {
                                if (!subscribedEvents.Contains(eventCombi.eventType))
                                {
                                    subscribedEvents.Add(eventCombi.eventType);
                                    VARMAP_ItemMaster.EVENT_SUBSCRIPTION(eventCombi.eventType, _OnSpawnConditionEvent, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        /* Not completed */
                    }
                }
            }

            return despawn;
        }

        protected bool SpawnConditionComplied(in SpawnConditionInfo conditionInfo)
        {
            if (conditionInfo.spawn)
            {
                /**/
            }

            if (conditionInfo.changeSprite)
            {
                SetSprite(conditionInfo.targetSprite);
            }

            return conditionInfo.despawn;
        }

        protected void MouseEnterAction(bool enter)
        {
            /* Prepare LevelInfo struct */
            isHovered = enter;
            LevelElemInfo info = new(itemID, gameElementFamily, actualWaypoint, enter & isAvailable);
            VARMAP_ItemMaster.GAME_ELEMENT_OVER(in info);
        }

        protected void SetAvailable(bool available)
        {
            isAvailable = available;
            _Hover_Refresh();
        }

        /// <summary>
        /// Sets active externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetActive(bool active)
        {
            isActive_ext = active;
            _SetActive_Refresh();
        }

        protected void SetActive_Internal(bool active)
        {
            isActive_int = active;
            _SetActive_Refresh();
        }

        private void _SetActive_Refresh()
        {
            bool enable = isActive_int & isActive_ext;

            gameObject.SetActive(enable);
        }

        /// <summary>
        /// Sets visible externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetVisible(bool active)
        {
            isVisible_ext = active;
            _SetVisible_Refresh();
        }

        protected void SetVisible_Internal(bool active)
        {
            isVisible_int = active;
            _SetVisible_Refresh();
        }

        private void _SetVisible_Refresh()
        {
            bool enable = isVisible_int & isVisible_ext;
            mySpriteRenderer.enabled = enable;
            _Hover_Refresh();
        }

        /// <summary>
        /// Sets motion externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetMotion(bool active)
        {
            isMotion_ext = active;
            _SetMotion_Refresh();
        }

        protected void SetMotion_Internal(bool active)
        {
            isMotion_int = active;
            _SetMotion_Refresh();
        }

        private void _SetMotion_Refresh()
        {
            bool enable = isMotion_int & isMotion_ext;
            myRigidbody.simulated = enable;
        }

        /// <summary>
        /// Sets clickable externally
        /// </summary>
        /// <param name="active">true or false</param>
        public void SetClickable(bool active)
        {
            isClickable_ext = active;
            _SetClickable_Refresh();
        }

        protected void SetClickable_Internal(bool active)
        {
            isClickable_int = active;
            _SetClickable_Refresh();
        }

        private void _SetClickable_Refresh()
        {
            bool enable = isClickable_ext & isClickable_int;
            myCollider.enabled = enable;
            _Hover_Refresh();
        }

        private void _Hover_Refresh()
        {
            bool compound;

            compound = isAvailable;
            compound &= isClickable_ext & isClickable_int;

            if (isHovered && !compound)
            {
                /* Update hover state */
                MouseEnterAction(false);
                isHovered = false;
            }
        }

        protected void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)mySpriteRenderer.bounds.min.y * 1000;
        }



        public virtual void VirtualDestroy()
        {
            SetAvailable(false);
            SetClickable(false);
            SetMotion(false);
            SetVisible(false);

            _Hover_Refresh();

            topParent.SetActive(false);

            VARMAP_ItemMaster.UNREG_GAMESTATUS(ChangedGameStatus);

            if (registered)
            {
                VARMAP_ItemMaster.ITEM_REGISTER(false, this);
            }

            if (subscribedEvents != null)
            {
                foreach (GameEvent gameEvent in subscribedEvents)
                {
                    VARMAP_ItemMaster.EVENT_SUBSCRIPTION(gameEvent, _OnSpawnConditionEvent, false);
                }
                subscribedEvents.Clear();
                subscribedEvents = null;
            }
        }


        private void ChangedGameStatus(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
        {
            _ = eventType;

            if (oldval != newval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        SetActive(true);
                        SetClickable(true);
                        SetMotion(true);
                        break;
                }

                switch (oldval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        SetActive(false);
                        SetClickable(false);
                        SetMotion(false);
                        break;
                }
            }
        }

        private void _OnSpawnConditionEvent(bool eventStatus)
        {
            _ = eventStatus;
            bool despawn = CheckSpawnConditions(false);

            if (despawn)
            {
                VirtualDestroy();
            }
        }
    }
}