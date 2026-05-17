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
                if (startingTrigger != AnimationTrigger.ANIMATION_TRIGGER_NONE)
                {
                    myAnimator.SetTrigger(ResourceAnimationsAtlasClass.ANIM_TRIGGER_TO_HASH[startingTrigger]);
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