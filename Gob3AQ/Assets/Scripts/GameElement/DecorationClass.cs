using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;
using System.Collections;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;

namespace Gob3AQ.GameElement.Decoration
{

    [System.Serializable]
    public class DecorationClass : MonoBehaviour
    {
        [SerializeField] private GameSprite sprite;

        [SerializeField] private AnimationTrigger startingTrigger;
        
        private Animator myAnimator;

        private SpriteRenderer mySpriteRenderer;

        private void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            myAnimator = GetComponent<Animator>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            StartCoroutine(Load_Coroutine());
        }

        private IEnumerator Load_Coroutine()
        {
            bool sprites_loaded = false;

            while (!sprites_loaded)
            {
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out sprites_loaded);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            if (myAnimator.runtimeAnimatorController)
            {
                if (startingTrigger != AnimationTrigger.ANIMATION_TRIGGER_ZERO)
                {
                    if (ResourceAnimationsAtlasClass.IsTriggerSteady(startingTrigger))
                    {
                        myAnimator.SetInteger(ResourceAnimationsAtlasClass.STEADY_INDEX_HASH, (int)startingTrigger);
                    }
                    
                    myAnimator.ResetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
                    myAnimator.ResetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_EXT_HASH);
                    myAnimator.SetInteger(ResourceAnimationsAtlasClass.ANIMATION_INDEX_HASH, (int)startingTrigger);
                    myAnimator.SetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_HASH);
                    myAnimator.SetTrigger(ResourceAnimationsAtlasClass.TRANSITION_TRIGGER_EXT_HASH);
                }
            }
            else
            {
                mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(sprite);
            }

            UpdateSortingOrder();
        }

        private void UpdateSortingOrder()
        {
            /* Set sorting order based on its actual Y */
            mySpriteRenderer.sortingOrder = -(int)(mySpriteRenderer.bounds.min.y * 1000);
        }
    }
}