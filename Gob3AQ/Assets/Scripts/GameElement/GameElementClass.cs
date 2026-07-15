using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Cards;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Network;
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
        private struct WaypointProgrammedPath
        {
            public int target_index;
            public int final_index;
            public float wp_distance;
            public float wp_distance3D;
            public float initial_size;
            public float delta_size;
        }

        protected enum PhysicalState
        {
            PHYSICAL_STATE_STANDING = 0x0,
            PHYSICAL_STATE_WALKING = 0x1,
            PHYSICAL_STATE_LOCKED = 0x2,
        }


        [SerializeField]
        protected GameItem itemID;

        [SerializeField]
        [Tooltip("Real waypoint (for movements")]
        protected WaypointClass startingWaypoint;

        [SerializeField]
        [Tooltip("Used Waypoint for character positioning when interacting with this element")]
        protected WaypointClass startingExposedWaypoint;

        [SerializeField]
        protected int hoverPriority;

        [SerializeField]
        protected bool reverseFlipX;


        public GameItem ItemID => itemID;

        public Collider2D My2DCollider => myCollider;
        public Bounds Bounds => mySpriteRenderer.bounds;
        public int Waypoint => actualWaypoint;
        public int ExposedWaypoint => exposedWaypoint;

        public bool IsAvailable => isAvailable;

        protected IReadOnlyList<WaypointInfo> waypoints_infos;

        protected GameItemFamily gameElementFamily;
        protected int actualWaypoint;
        protected int exposedWaypoint; /* For character positioning when interacting */
        protected LevelElemInfo hoverInfo;
        protected GameObject topParent;
        protected Transform topParentTransform;
        protected Collider2D myCollider;
        protected SpriteRenderer mySpriteRenderer;
        protected Rigidbody2D myRigidbody;
        protected Animator myAnimator;
        protected SpriteMask spriteMask;
        protected Action animationStartCallback;
        protected Action animationEndCallback;
        protected bool? storedPendingFlipX;
        protected int? storedPendingSteadyTrigger;
        protected int pendingStateCrossings;
        protected bool pendingZeroStateCross;
        protected AnimationTrigger actualAnimationTrigger;
        protected AnimationTrigger autoSteadyTrigger;
        protected AnimationTrigger queuedTrigger;
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

        protected PhysicalState physicalstate;
        private WaypointProgrammedPath actualProgrammedPath;
        private float baseSize;

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

            hoverInfo = new(itemID, gameElementFamily, exposedWaypoint, hoverPriority, false);
        }

        protected virtual void Start()
        {
            loaded = false;

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            gameElementFamily = itemInfo.family;

            actualAnimationTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE;
            autoSteadyTrigger = AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE;
            queuedTrigger = AnimationTrigger.ANIMATION_TRIGGER_ZERO;
            prevAnimationNormalizedTime = 0f;

            /* Register item as Level element (to be clicked and able to interact) */
            VARMAP_ItemMaster.ITEM_REGISTER(true, this);

            registered = true;
            baseSize = topParentTransform.localScale.x;

            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);
            VARMAP_ItemMaster.GET_WP_LIST(out waypoints_infos);

            /* Children actions (Clickable, Hoverable) will be executed afterward */
        }

        protected virtual void Update()
        {
            Game_Status gstatus = VARMAP_ItemMaster.GET_GAMESTATUS();
            if (gstatus is Game_Status.GAME_STATUS_PLAY or Game_Status.GAME_STATUS_PLAY_ANIMATION)
            {
                switch (physicalstate)
                {
                    case PhysicalState.PHYSICAL_STATE_WALKING:
                        Execute_Walk();
                        break;
                }
            }
        }


        protected virtual void OnDestroy()
        {
            VirtualDestroy();
        }

        public void PerformAnimation(AnimationTrigger trigger, Action startCallback, Action endCallback, bool storeSteadyOnly, bool? doFlipX, bool immediate)
        {
            if ((!myAnimator.runtimeAnimatorController) || (trigger == AnimationTrigger.ANIMATION_TRIGGER_ZERO)) return;

            if(storeSteadyOnly)
            {
                if (ResourceAnimationsAtlasClass.IsTriggerSteady(trigger))
                {
                    autoSteadyTrigger = trigger;
                    storedPendingSteadyTrigger = (int)trigger;
                }
                return;
            }


            if ((queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_ZERO) && ((animationStartCallback != null) || (animationEndCallback != null)))
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
            
            AnimationTrigger usedTrigger = trigger;

            if (ResourceAnimationsAtlasClass.IsTriggerSteady(trigger))
            {
                autoSteadyTrigger = usedTrigger;
                myAnimator.SetInteger(ResourceAnimationsAtlasClass.STEADY_INDEX_HASH, (int)trigger);
            }
            else if ((trigger == AnimationTrigger.ANIMATION_TRIGGER_AUTO_STEADY))
            {
                usedTrigger = autoSteadyTrigger;
            }

            /* If animation is yet present, for example Talking -> Talking */
            if (actualAnimationTrigger == usedTrigger)
            {
                queuedTrigger = AnimationTrigger.ANIMATION_TRIGGER_ZERO;
                startCallback?.Invoke();
                animationStartCallback = null;
                animationEndCallback = endCallback;
                pendingStateCrossings = 1;
                if(doFlipX != null)
                {
                    mySpriteRenderer.flipX = doFlipX.Value;
                }
            }
            else
            {
                queuedTrigger = usedTrigger;
                storedPendingFlipX = doFlipX;
                pendingStateCrossings = 0;
                pendingZeroStateCross = false;
                animationStartCallback = startCallback;
                animationEndCallback = endCallback;


                if (immediate || ResourceAnimationsAtlasClass.IsTriggerWalking(actualAnimationTrigger) || ResourceAnimationsAtlasClass.IsTriggerSteady(actualAnimationTrigger))
                {
                    ActivateTrigger(queuedTrigger);
                }
            }
        }

        public bool ActionRequest(int destWp_index)
        {
            bool requestAccepted;

            /* Interact only if not talking or doing an action */
            if (IsAvailable)
            {
                int target_index = waypoints_infos[actualWaypoint].Solution.TravelTo[destWp_index];


                if (target_index != -1)
                {
                    actualProgrammedPath.final_index = destWp_index;

                    /* If already walking, complete its actual segment. If stopped, start with first inteded segment of new path from actual waypoint */
                    if ((physicalstate != PhysicalState.PHYSICAL_STATE_WALKING) && (actualWaypoint != destWp_index))
                    {
                        Walk_StartNextSegment();
                    }
                    physicalstate = PhysicalState.PHYSICAL_STATE_WALKING;

                    requestAccepted = true;
                }
                else
                {
                    requestAccepted = false;
                }

                
            }
            else
            {
                requestAccepted = false;
            }

            

            return requestAccepted;
        }

        public void SetUnspawned(bool unspawned)
        {
            isUnspawned = unspawned;
        }


        public void SetSprite(GameSprite newSprite)
        {
            if (!loaded) return;
            
            mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(newSprite);

            if(spriteMask)
            {
                spriteMask.sprite = mySpriteRenderer.sprite;
            }
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

            if(spriteMask)
            {
                spriteMask.enabled = enable;
            }

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

            hoverInfo = new(itemID, gameElementFamily, exposedWaypoint, hoverPriority, compound);           
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
            AnimationTrigger prevAnimationTrigger = actualAnimationTrigger;
            
            actualAnimationTrigger = ResourceAnimationsAtlasClass.STATE_HASH_TO_TRIGGER.GetValueOrDefault(stateInfo.tagHash, AnimationTrigger.ANIMATION_TRIGGER_ZERO);
            
            prevAnimationNormalizedTime = 0f;

            bool decremented;

            if ((prevAnimationTrigger == AnimationTrigger.ANIMATION_TRIGGER_ZERO) && pendingZeroStateCross)
            {
                pendingZeroStateCross = false;
                --pendingStateCrossings;

                decremented = true;
            }
            else if ((pendingStateCrossings > 1) && !pendingZeroStateCross)
            {
                --pendingStateCrossings;
                decremented = true;
            }
            else
            {
                decremented = false;
            }

            if (decremented && (pendingStateCrossings == 1))
            {
                if (storedPendingFlipX.HasValue)
                {
                    mySpriteRenderer.flipX = storedPendingFlipX.Value;
                    storedPendingFlipX = null;
                }

                animationStartCallback?.Invoke();
                animationStartCallback = null;
            }
            
            /* Start animation also triggers queued animation */
            if (queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_ZERO)
            {
                ExecuteQueuedTrigger();
            }
        }

        public virtual void OnAnimationUpdate(AnimatorStateInfo stateInfo)
        {
            float normTime = stateInfo.normalizedTime % 1f;

            if ((normTime < prevAnimationNormalizedTime))
            {
                if (pendingStateCrossings == 1)
                {
                    animationEndCallback?.Invoke();
                    animationEndCallback = null;
                
                    --pendingStateCrossings;
                }
                
                if (queuedTrigger != AnimationTrigger.ANIMATION_TRIGGER_ZERO)
                {
                    ExecuteQueuedTrigger();
                }
            }
            else
            {
                prevAnimationNormalizedTime = normTime;
            }
        }

        public virtual void OnAnimationEnd(AnimatorStateInfo stateInfo)
        {
            if ((actualAnimationTrigger != AnimationTrigger.ANIMATION_TRIGGER_ZERO) && (pendingStateCrossings == 1))
            {                
                animationEndCallback?.Invoke();
                animationEndCallback = null;
                
                --pendingStateCrossings;
            }
        }

        private void ActivateTrigger(AnimationTrigger trigger)
        {
            if (storedPendingSteadyTrigger.HasValue)
            {
                myAnimator.SetInteger(ResourceAnimationsAtlasClass.STEADY_INDEX_HASH, storedPendingSteadyTrigger.Value);
                storedPendingSteadyTrigger = null;
            }

            myAnimator.ResetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
            myAnimator.ResetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_EXT_HASH);
            myAnimator.SetInteger(ResourceAnimationsAtlasClass.ANIMATION_INDEX_HASH, (int)trigger);
            myAnimator.SetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
            myAnimator.SetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_EXT_HASH);
        }

        protected void ExecuteQueuedTrigger()
        {
            ActivateTrigger(queuedTrigger);

            bool isCycled = ResourceAnimationsAtlasClass.IsTriggerCycled(queuedTrigger);
            
            queuedTrigger = AnimationTrigger.ANIMATION_TRIGGER_ZERO;

            pendingStateCrossings = isCycled ? 3 : 2;
            pendingZeroStateCross = true;
        }

        private void Execute_Walk()
        {
            Vector3 target_pos = waypoints_infos[actualProgrammedPath.target_index].Position;
            Vector3 orig_pos = waypoints_infos[actualWaypoint].Position;
            Vector2 deltaPos = topParentTransform.position - orig_pos;
            float crossed_distance = deltaPos.magnitude;
            float distance_clamped = Mathf.Min(crossed_distance, actualProgrammedPath.wp_distance);
            float interp_size;

            if (actualProgrammedPath.wp_distance == 0f)
            {
                interp_size = actualProgrammedPath.initial_size;
            }
            else
            {
                interp_size = actualProgrammedPath.initial_size + (actualProgrammedPath.delta_size * (distance_clamped / actualProgrammedPath.wp_distance));
            }

            SetSize(interp_size);

            if (crossed_distance >= actualProgrammedPath.wp_distance)
            {
                /* Store WP Index */
                ReachedWaypointFunction(actualProgrammedPath.target_index);

                /* If last segment or action triggered or next waypoint or really unreachable */
                if ((actualProgrammedPath.target_index == actualProgrammedPath.final_index))
                {
                    actualWaypoint = actualProgrammedPath.target_index;
                    PresetProgrammedPathStruct(actualWaypoint);
                    topParentTransform.position = target_pos;
                    physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
                    myRigidbody.linearVelocity = Vector2.zero;
                    SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

                    PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE, null, null, false, null, true);
                    ExecuteQueuedTrigger();
                    mySpriteRenderer.flipX = waypoints_infos[actualWaypoint].FlipXForAction ^ reverseFlipX;

                    StartBufferedInteraction();
                }
                else
                {
                    Walk_StartNextSegment();
                }
            }
        }

        protected virtual void ReachedWaypointFunction(int wp_index)
        {
            /* Fill in children */
        }

        private void Walk_StartNextSegment()
        {
            Vector2 delta;
            float speed_reduction_factor = 1f;


            actualWaypoint = actualProgrammedPath.target_index;
            topParentTransform.position = waypoints_infos[actualWaypoint].Position;
            actualProgrammedPath.target_index = waypoints_infos[actualWaypoint].Solution.TravelTo[actualProgrammedPath.final_index];

            actualProgrammedPath.wp_distance = ((Vector2)waypoints_infos[actualProgrammedPath.target_index].Position - (Vector2)waypoints_infos[actualWaypoint].Position).magnitude;
            actualProgrammedPath.wp_distance3D = (waypoints_infos[actualProgrammedPath.target_index].Position - waypoints_infos[actualWaypoint].Position).magnitude;
            actualProgrammedPath.initial_size = waypoints_infos[actualWaypoint].CharacterSizeFactor;
            actualProgrammedPath.delta_size = waypoints_infos[actualProgrammedPath.target_index].CharacterSizeFactor - waypoints_infos[actualWaypoint].CharacterSizeFactor;
            SetSize(waypoints_infos[actualWaypoint].CharacterSizeFactor);

            delta = ((Vector2)waypoints_infos[actualProgrammedPath.target_index].Position - (Vector2)waypoints_infos[actualWaypoint].Position).normalized;

            if (actualProgrammedPath.wp_distance3D != 0f)
            {
                speed_reduction_factor = actualProgrammedPath.wp_distance / actualProgrammedPath.wp_distance3D;
            }

            float absDeltaX = Mathf.Abs(delta.x);
            float absDeltaY = Mathf.Abs(delta.y);
            AnimationTrigger walkdirTrigger;

            if (absDeltaX >= 0.985f)
            {
                walkdirTrigger = AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE;

                mySpriteRenderer.flipX = (delta.x > 0) ^ reverseFlipX;
            }
            else if (absDeltaX >= 0.173f)
            {
                walkdirTrigger = delta.y >= 0f ? AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERBACK : AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERFRONT;

                mySpriteRenderer.flipX = (delta.x > 0) ^ reverseFlipX;
            }
            else
            {
                walkdirTrigger = delta.y >= 0f ? AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK : AnimationTrigger.ANIMATION_TRIGGER_WALK_FRONT;

                mySpriteRenderer.flipX = reverseFlipX;
            }

            PerformAnimation(walkdirTrigger, null, null, false, null, true);
            ExecuteQueuedTrigger();

            /* Remove after debugging */
            speed_reduction_factor = 1f;

            myRigidbody.linearVelocity = GameFixedConfig.CHARACTER_NORMAL_SPEED * speed_reduction_factor * delta;
        }

        protected void PresetProgrammedPathStruct(int waypoint_index)
        {
            actualProgrammedPath.target_index = waypoint_index;
            actualProgrammedPath.final_index = waypoint_index;
            actualProgrammedPath.wp_distance = 0f;
            actualProgrammedPath.wp_distance3D = 0f;
            actualProgrammedPath.initial_size = waypoints_infos[waypoint_index].CharacterSizeFactor;
            actualProgrammedPath.delta_size = 0f;
        }

        private void StartBufferedInteraction()
        {
            VARMAP_ItemMaster.ITEM_REACHED_WAYPOINT(itemID);
        }

        protected void SetSize(float size)
        {
            Vector3 scale = size * baseSize * Vector3.one;
            topParentTransform.localScale = scale;
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
                    if (((newval != Game_Status.GAME_STATUS_PLAY_DIALOG) && (newval != Game_Status.GAME_STATUS_PLAY_ANIMATION)) || !myAnimator)
                    {
                        SetActive(false);
                    }

                    SetClickable(false);
                    SetMotion(newval == Game_Status.GAME_STATUS_PLAY_ANIMATION);

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