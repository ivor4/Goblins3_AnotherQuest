using System;
using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ItemMaster;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Gob3AQ.GameElement.Item
{
    [System.Serializable]
    public struct ItemProgrammedAnimation
    {
        public ulong everyMs;
        public AnimationTrigger conditionActualTrigger;
        public AnimationTrigger destTrigger;
    }

    public struct ItemProgrammedAnimationRuntime
    {
        public ItemProgrammedAnimation progAnim;
        public ulong lastTimestampMs;

        public ItemProgrammedAnimationRuntime(ItemProgrammedAnimation p, ulong actualTimestamp)
        {
            progAnim = p;
            lastTimestampMs = actualTimestamp;
        }
    }
    [System.Serializable]
    public class ItemClass : GameElementClass
    {
        [SerializeField]
        private bool needsZoom;

        [SerializeField]
        private float maxZoomLevel;

        [SerializeField]
        private Color nightColor;

        [SerializeField]
        private List<ItemProgrammedAnimation> programmedAnimations;

        [SerializeField]
        private AnimationTrigger startingTrigger;

        private float actualZoomLevel;

        private Color transparent_color;
        private Color original_color;

        private ItemProgrammedAnimationRuntime[] programmedAnimationsRt;

        protected override void Awake()
        {
            base.Awake();

            topParent = transform.parent.gameObject;

            mySpriteRenderer = topParent.GetComponent<SpriteRenderer>();
            myCollider = topParent.GetComponent<Collider2D>();
            myRigidbody = topParent.GetComponent<Rigidbody2D>();
            myAnimator = topParent.GetComponent<Animator>();

            if (myAnimator)
            {
                ulong actualTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
                programmedAnimationsRt = new ItemProgrammedAnimationRuntime[programmedAnimations.Count];
                for (int i = 0; i < programmedAnimations.Count; i++) { 
                    programmedAnimationsRt[i] = new ItemProgrammedAnimationRuntime(programmedAnimations[i],
                        actualTimestamp - (ulong)Random.Range(0, (int)programmedAnimations[i].everyMs));
                }
                
                myAnimatorBehavior = myAnimator.GetBehaviour<GenericAnimBehavior>();
                if (myAnimatorBehavior)
                {
                    myAnimatorBehavior.SetOnStartEndCallback(OnAnimationStart, OnAnimationEnd);
                }

                if (startingTrigger != AnimationTrigger.ANIMATION_TRIGGER_NONE)
                {
                    PerformAnimation(startingTrigger, null, null);
                }
            }

            if (VARMAP_ItemMaster.GET_DAY_MOMENT() == MomentType.MOMENT_MORNING)
            {
                original_color = mySpriteRenderer.color;
            }
            else
            {
                original_color = mySpriteRenderer.color * nightColor;
            }

            transparent_color = original_color;
            transparent_color.a *= 0.2f;

            SetVisible_Internal(false);
            SetClickable_Internal(false);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            if(needsZoom)
            {
                VARMAP_ItemMaster.ZOOM_SUBSCRIPTION(true, _OnZoomChanged);
                actualZoomLevel = 100f;
            }

            ItemMasterClass.AddOneItemToLoad();

            /* Execute on next Update */
            _ = StartCoroutine(Execute_Loading());
        }

        protected void Update()
        {
            if ((!myAnimator.runtimeAnimatorController) || (programmedAnimationsRt.Length <= 0)) return;
            
            ulong actualTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
            for (int i = 0; i < programmedAnimationsRt.Length; i++)
            {
                ref ItemProgrammedAnimationRuntime rt = ref programmedAnimationsRt[i];

                if (rt.progAnim.conditionActualTrigger != actualAnimationTrigger)
                {
                    rt.lastTimestampMs = actualTimestamp;
                    continue;
                }
                    
                if ((actualTimestamp - rt.lastTimestampMs) < rt.progAnim.everyMs) continue;
                    
                rt.lastTimestampMs = actualTimestamp;
                
                PerformAnimation(rt.progAnim.destTrigger, null, null);
            }

            animationJustStarted = false;
        }


        protected virtual IEnumerator Execute_Loading()
        {
            bool _loaded = false;

            while (!_loaded)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out _loaded);
            }

            Loading_Task();
        }

        protected virtual void Loading_Task()
        {
            if (!startingWaypoint)
            {
                VARMAP_ItemMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out actualWaypoint, out _);
            }
            else
            {
                actualWaypoint = startingWaypoint.ID_in_Network;
            }

            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(itemID);

            loaded = true;

            /* Set to default sprite if no animator */
            if (!myAnimator.runtimeAnimatorController)
            {
                SetSprite(itemInfo.defaultSprite);
            }

            SetVisible_Internal(true);
            SetAvailable(true);

            if ((!needsZoom) || (actualZoomLevel <= maxZoomLevel))
            {
                mySpriteRenderer.color = original_color;
                SetClickable_Internal(true);
            }
            else
            {
                mySpriteRenderer.color = transparent_color;
                SetClickable_Internal(false);
            }

            UpdateSortingOrder();

            ItemMasterClass.AddOneItemLoaded();

            
        }

        public override void VirtualDestroy()
        {
            base.VirtualDestroy();

            if (needsZoom)
            {
                VARMAP_ItemMaster.ZOOM_SUBSCRIPTION(false, _OnZoomChanged);
            }
        }

        private void _OnZoomChanged(float newZoonLevel)
        {
            actualZoomLevel = newZoonLevel;

            if (newZoonLevel > maxZoomLevel)
            {
                mySpriteRenderer.color = transparent_color;
                SetClickable_Internal(false);
            }
            else
            {
                mySpriteRenderer.color = original_color;
                SetClickable_Internal(true);
            }
        }
    }
}
