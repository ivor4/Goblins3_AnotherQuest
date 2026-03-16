using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ResourceAnimationsAtlas
{
    public static class ResourceAnimationsAtlasClass
    {
        public static readonly IReadOnlyDictionary<AnimationTrigger, string> ANIM_TRIGGER_TO_STR = new Dictionary<AnimationTrigger, string>()
        {
            { AnimationTrigger.TRIGGER_ONE, "Tr_1" },
            { AnimationTrigger.TRIGGER_TWO, "Tr_2" },
            { AnimationTrigger.TRIGGER_THREE, "Tr_3" },
            { AnimationTrigger.TRIGGER_FOUR, "Tr_4" },
            { AnimationTrigger.TRIGGER_FIVE, "Tr_5" },
            { AnimationTrigger.TRIGGER_SIX, "Tr_6" },
        };

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
                new(AnimationSrcTrigger.SRC_TRIGGER_ANIM_END, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noCombi, GameItem.ITEM_PLAYER_MAIN, null),   /* Action 1 */
                    }
                ),
            }), 
        };

        
    }
}