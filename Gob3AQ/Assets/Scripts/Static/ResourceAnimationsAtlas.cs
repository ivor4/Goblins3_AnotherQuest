using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceAnimationsAtlas
{
    public static class ResourceAnimationsAtlasClass
    {
        public static ref readonly AnimationConfig GetAnimationConfig(GameAnimation animation)
        {
            if ((uint)animation < (uint)GameAnimation.ANIMATION_TOTAL)
            {
                return ref _AnimationConfig[(int)animation];
            }
            else
            {
                Debug.LogError($"Trying to get AnimationConfig for invalid animation {animation}");
                return ref AnimationConfig.EMPTY;
            }
        }

        private static readonly GameEventCombi[] noCombi = new GameEventCombi[0];


        private static readonly AnimationConfig[] _AnimationConfig = new AnimationConfig[(int)GameAnimation.ANIMATION_TOTAL]
        {
            /* ANIMATION_ONE */
            new(new AnimationMilestoneConfig[]
            {
                /* Milestone 1 */
                new(AnimationSrcTrigger.SRC_TRIGGER_PREV_END, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noCombi, GameItem.ITEM_PLAYER_MAIN, null),   /* Action 1 */
                    }
                ),
            }), 
        };
    }
}