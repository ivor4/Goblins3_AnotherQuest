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

            {AnimationTrigger.TRIGGER_SPECIAL_ONE, "Tr_Special1" },
            {AnimationTrigger.TRIGGER_SPECIAL_TWO, "Tr_Special2" },

            { AnimationTrigger.TRIGGER_TALK, "Tr_Talk" },
            { AnimationTrigger.TRIGGER_WALK, "Tr_Walk" },
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

        private static readonly GameAction[] noAction = new GameAction[0];


        private static readonly AnimationConfig[] _AnimationConfig = new AnimationConfig[(int)GameAnimation.ANIMATION_TOTAL]
        {
            /* ANIMATION_REME TEST */
            new(new AnimationMilestoneConfig[]
            {
                /* Milestone 1 */
                new(AnimationSrcTrigger.SRC_TRIGGER_ANIM_END, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noAction, GameItem.ITEM_HIVE1_NPC_REME, AnimationTrigger.TRIGGER_SPECIAL_ONE),   /* Action 1 */
                    }
                ),
                /* Milestone 2 */
                new(AnimationSrcTrigger.SRC_TRIGGER_CALLBACK, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noAction, GameItem.ITEM_NONE, AnimationTrigger.TRIGGER_ONE),   /* Action 1 */
                    }
                ),
            }),

            /* ANIMATION_LAST */
            new(new AnimationMilestoneConfig[]
            {
                /* Milestone 1 */
                new(AnimationSrcTrigger.SRC_TRIGGER_ANIM_END, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noAction, GameItem.ITEM_PLAYER_MAIN, AnimationTrigger.TRIGGER_ONE),   /* Action 1 */
                    }
                ),
            }),
        };

        
    }
}