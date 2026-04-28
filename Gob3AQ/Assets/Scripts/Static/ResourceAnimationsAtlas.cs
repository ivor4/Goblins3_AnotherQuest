using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ResourceAnimationsAtlas
{
    public static class ResourceAnimationsAtlasClass
    {
        public static readonly IReadOnlyDictionary<AnimationTrigger, int> ANIM_TRIGGER_TO_HASH = new Dictionary<AnimationTrigger, int>()
        {
            { AnimationTrigger.ANIMATION_TRIGGER_ONE, Animator.StringToHash("Tr_1") },
            { AnimationTrigger.ANIMATION_TRIGGER_TWO, Animator.StringToHash("Tr_2") },
            { AnimationTrigger.ANIMATION_TRIGGER_THREE, Animator.StringToHash("Tr_3") },
            { AnimationTrigger.ANIMATION_TRIGGER_FOUR, Animator.StringToHash("Tr_4") },
            { AnimationTrigger.ANIMATION_TRIGGER_FIVE, Animator.StringToHash("Tr_5") },
            { AnimationTrigger.ANIMATION_TRIGGER_SIX, Animator.StringToHash("Tr_6") },
            { AnimationTrigger.ANIMATION_TRIGGER_SEVEN, Animator.StringToHash("Tr_7") },
            { AnimationTrigger.ANIMATION_TRIGGER_EIGHT, Animator.StringToHash("Tr_8") },

            { AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE, Animator.StringToHash("Tr_Cycle1") },
            { AnimationTrigger.ANIMATION_TRIGGER_CYCLE_TWO, Animator.StringToHash("Tr_Cycle2") },
            { AnimationTrigger.ANIMATION_TRIGGER_CYCLE_THREE, Animator.StringToHash("Tr_Cycle3") },
            { AnimationTrigger.ANIMATION_TRIGGER_CYCLE_FOUR, Animator.StringToHash("Tr_Cycle4") },

            { AnimationTrigger.ANIMATION_TRIGGER_STEADY, Animator.StringToHash("Tr_Steady") },

            { AnimationTrigger.ANIMATION_TRIGGER_TALK, Animator.StringToHash("Tr_Talk") },

            { AnimationTrigger.ANIMATION_TRIGGER_WALK_FRONT, Animator.StringToHash("Tr_WalkFront") },
            { AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK, Animator.StringToHash("Tr_WalkBack") },
            { AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERFRONT, Animator.StringToHash("Tr_WalkCornerFront") },
            { AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERBACK, Animator.StringToHash("Tr_WalkCornerBack") },
            { AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE, Animator.StringToHash("Tr_WalkSide") },
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
                        new(noAction, GameItem.ITEM_HIVE1_NPC_REME, AnimationTrigger.ANIMATION_TRIGGER_TWO, GameSound.SOUND_NONE),   /* Action 1 */
                    }
                ),
                /* Milestone 2 */
                new(AnimationSrcTrigger.SRC_TRIGGER_CALLBACK, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noAction, GameItem.ITEM_NONE, AnimationTrigger.ANIMATION_TRIGGER_ONE, GameSound.SOUND_NONE),   /* Action 1 */
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
                        new(noAction, GameItem.ITEM_PLAYER_MAIN, AnimationTrigger.ANIMATION_TRIGGER_ONE, GameSound.SOUND_NONE),   /* Action 1 */
                    }
                ),
            }),
        };

        
    }
}