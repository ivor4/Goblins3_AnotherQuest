using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.GameElement
{
    public interface IAnimatorListener
    {
        public void OnAnimationStart(AnimatorStateInfo stateInfo);
        public void OnAnimationUpdate(AnimatorStateInfo stateInfo);
        public void OnAnimationEnd(AnimatorStateInfo stateInfo);
    }
    
    [System.Serializable]
    public class GameElementClass : MonoBehaviour, IGameObjectHoverable, IAnimatorListener
    {
        
        
        [SerializeField]
        protected GameItem itemID;

        [SerializeField]
        protected WaypointClass startingWaypoint;

        [SerializeField]
        protected int hoverPriority;


        public GameItem ItemID => itemID;

        public Collider2D My2DCollider => myCollider;
        public Bounds Bounds => mySpriteRenderer.bounds;
        public int Waypoint => actualWaypoint;

        public bool IsAvailable => isAvailable;


        protected GameItemFamily gameElementFamily;
        protected int actualWaypoint;
        protected LevelElemInfo hoverInfo;
        protected GameObject topParent;
        protected Collider2D myCollider;
        protected SpriteRenderer mySpriteRenderer;
        protected Rigidbody2D myRigidbody;
        protected Animator myAnimator;
        protected Action animationStartCallback;
        protected Action animationEndCallback;
        protected int animationStartedNewState;
        protected AnimationTrigger actualAnimationTrigger;
        protected AnimationTrigger autoSteadyTrigger;
        protected AnimationTrigger queuedTrigger;
        protected bool ignoreAnimationEventEnter;
        protected bool ignoreAnimationEventUpdate;
        protected bool ignoreAnimationEventEnd;
        protected float prevAnimationNormalizedTime;
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

        private bool isUnspawned;
        private bool isUnclickable;

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
            isUnspawned = false;
            isUnclickable = false;

            hoverInfo = new(itemID, gameElementFamily, actualWaypoint, hoverPriority, false);
        }

        protected virtual void Start()
        {
            loaded = false;

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            gameElementFamily = itemInfo.family;

            actualAnimationTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY;
            autoSteadyTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY;
            queuedTrigger = AnimationTrigger.ANIMATION_TRIGGER_NONE;
            prevAnimationNormalizedTime = 0f;

            /* Register item as Level element (to be clicked and able to interact) */
            VARMAP_ItemMaster.ITEM_REGISTER(true, this);

            registered = true;

            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);

            /* Children actions (Clickable, Hoverable) will be executed afterward */
        }


        protected virtual void OnDestroy()
        {
            VirtualDestroy();
        }

        public void PerformAnimation(AnimationTrigger trigger, Action startCallback, Action endCallback)
        {
            if ((!myAnimator.runtimeAnimatorController) || (trigger == AnimationTrigger.ANIMATION_TRIGGER_NONE)) return;

            if ((queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_NONE) && ((animationStartCallback != null) || (animationEndCallback != null)))
            {
                Debug.LogError($"Trying to queue an animation when animator has already one enqueued ({queuedTrigger}) and new {trigger}");
            }
            else if ((animationStartCallback != null) || (animationEndCallback != null))
            {
                Debug.LogError($"Calling animationEndCallback before it has ended {actualAnimationTrigger}");
            }

            /* Emergency call to avoid stuck actions - However, this shall never happen ;-) */
            animationStartCallback?.Invoke();
            animationEndCallback?.Invoke();
            
            animationStartedNewState = 0;
            animationStartCallback = startCallback;
            animationEndCallback = endCallback;
            AnimationTrigger usedTrigger = trigger;

            switch (trigger)
            {
                case AnimationTrigger.ANIMATION_TRIGGER_TRANSITION_ONE:
                    autoSteadyTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY;
                    break;
                case AnimationTrigger.ANIMATION_TRIGGER_TRANSITION_TWO:
                    autoSteadyTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO;
                    break;
                case AnimationTrigger.ANIMATION_TRIGGER_AUTO_STEADY:
                    usedTrigger = autoSteadyTrigger;
                    break;
            }
            
            queuedTrigger = usedTrigger;
        }

        public void SetUnspawned(bool unspawned)
        {
            isUnspawned = unspawned;
        }

        public void SetSprite(GameSprite newSprite)
        {
            if (!loaded) return;
            
            mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(newSprite);
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

        public void SetUnclickable(bool unclickable)
        {
            isUnclickable = unclickable;
            SetClickable(!isUnclickable);
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
            var compound = isAvailable;
            compound &= isClickable_ext & isClickable_int;

            hoverInfo = new(itemID, gameElementFamily, actualWaypoint, hoverPriority, compound);           
        }

        protected virtual void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)(mySpriteRenderer.bounds.min.y * 1000);
        }



        public virtual void VirtualDestroy()
        {
            SetAvailable(false);
            SetClickable(false);
            SetMotion(false);
            SetVisible(false);

            topParent.SetActive(false);

            VARMAP_ItemMaster.UNREG_GAMESTATUS(ChangedGameStatus);

            if (registered)
            {
                VARMAP_ItemMaster.ITEM_REGISTER(false, this);
            }
        }

        public virtual void OnAnimationStart(AnimatorStateInfo stateInfo)
        {
            if (ignoreAnimationEventEnter) return;
            ignoreAnimationEventEnter = true;
            
            actualAnimationTrigger = ResourceAnimationsAtlasClass.STATE_HASH_TO_TRIGGER.GetValueOrDefault(stateInfo.shortNameHash, AnimationTrigger.ANIMATION_TRIGGER_NONE);
            prevAnimationNormalizedTime = 0f;

            if (animationStartedNewState == 1)
            {
                animationStartedNewState = 2;
            }
            
            /* Start animation also triggers queued animation */
            if (queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_NONE)
            {
                ExecuteQueuedTrigger();
            }

            animationStartCallback?.Invoke();
            animationStartCallback = null;
        }

        public virtual void OnAnimationUpdate(AnimatorStateInfo stateInfo)
        {
            if (ignoreAnimationEventUpdate) return;
            ignoreAnimationEventUpdate = true;
            
            float normTime = stateInfo.normalizedTime % 1f;

            if ((normTime < prevAnimationNormalizedTime) && (queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_NONE))
            {
                ExecuteQueuedTrigger();
            }
            else
            {
                prevAnimationNormalizedTime = normTime;
            }
        }

        public virtual void OnAnimationEnd(AnimatorStateInfo stateInfo)
        {
            if (ignoreAnimationEventEnd) return;
            ignoreAnimationEventEnd = true;
            
            if (animationStartedNewState == 2)
            {
                animationStartedNewState = 0;
                animationEndCallback?.Invoke();
                animationEndCallback = null;
            }
        }

        protected void ActivateTrigger(AnimationTrigger trigger)
        {
            myAnimator.ResetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
            myAnimator.SetInteger(ResourceAnimationsAtlasClass.ANIMATION_INDEX_HASH, (int)trigger);
            myAnimator.SetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
            
            animationStartedNewState = 1;
        }

        private void ExecuteQueuedTrigger()
        {
            ActivateTrigger(queuedTrigger);

            actualAnimationTrigger = queuedTrigger;
            prevAnimationNormalizedTime = 0f;
            
            queuedTrigger = AnimationTrigger.ANIMATION_TRIGGER_NONE;
                
            animationStartCallback?.Invoke();
            animationStartCallback = null;
        }

        private void ChangedGameStatus(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
        {
            _ = eventType;

            if (oldval == newval) return;
            
            switch (newval)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    SetActive(!isUnspawned);
                    SetClickable(!isUnspawned && !isUnclickable);
                    SetMotion(!isUnspawned);
                    if (myAnimator)
                    {
                        myAnimator.enabled = true;
                    }
                    break;
            }

            switch (oldval)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    if ((newval != Game_Status.GAME_STATUS_PLAY_DIALOG) || !myAnimator)
                    {
                        SetActive(false);
                    }

                    SetClickable(false);
                    SetMotion(false);

                    if (((newval == Game_Status.GAME_STATUS_PLAY_MEMENTO) || (newval == Game_Status.GAME_STATUS_PLAY_ITEM_MENU) || (newval == Game_Status.GAME_STATUS_PLAY_DECISION)) && myAnimator)
                    {
                        myAnimator.enabled = false;
                    }
                    break;
            }
        }

        public ref readonly LevelElemInfo GetHoverableLevelElemInfo()
        {
            return ref hoverInfo;
        }
    }
}