using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ResourceAnimationsAtlas
{
    public static class ResourceAnimationsAtlasClass
    {
        public static readonly IReadOnlyDictionary<AnimationTrigger, int> ANIM_TRIGGER_TO_STR = new Dictionary<AnimationTrigger, int>()
        {
            { AnimationTrigger.TRIGGER_ONE, Animator.StringToHash("Tr_1") },
            { AnimationTrigger.TRIGGER_TWO, Animator.StringToHash("Tr_2") },
            { AnimationTrigger.TRIGGER_THREE, Animator.StringToHash("Tr_3") },
            { AnimationTrigger.TRIGGER_FOUR, Animator.StringToHash("Tr_4") },
            { AnimationTrigger.TRIGGER_FIVE, Animator.StringToHash("Tr_5") },
            { AnimationTrigger.TRIGGER_SIX, Animator.StringToHash("Tr_6") },

            {AnimationTrigger.TRIGGER_SPECIAL_ONE, Animator.StringToHash("Tr_Special1") },
            {AnimationTrigger.TRIGGER_SPECIAL_TWO, Animator.StringToHash("Tr_Special2") },

            { AnimationTrigger.TRIGGER_STEADY, Animator.StringToHash("Tr_Steady") },

            { AnimationTrigger.TRIGGER_TALK, Animator.StringToHash("Tr_Talk") },
            { AnimationTrigger.TRIGGER_WALK_FRONT, Animator.StringToHash("Tr_WalkFront") },
            { AnimationTrigger.TRIGGER_WALK_BACK, Animator.StringToHash("Tr_WalkBack") },
            { AnimationTrigger.TRIGGER_WALK_CORNERFRONT, Animator.StringToHash("Tr_WalkCornerFront") },
            { AnimationTrigger.TRIGGER_WALK_CORNERBACK, Animator.StringToHash("Tr_WalkCornerBack") },
            { AnimationTrigger.TRIGGER_WALK_SIDE, Animator.StringToHash("Tr_WalkSide") },
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
                        new(noAction, GameItem.ITEM_HIVE1_NPC_REME, AnimationTrigger.TRIGGER_SPECIAL_ONE, GameSound.SOUND_NONE),   /* Action 1 */
                    }
                ),
                /* Milestone 2 */
                new(AnimationSrcTrigger.SRC_TRIGGER_CALLBACK, 0f,
                    new AnimationActionConfig[]
                    {
                        new(noAction, GameItem.ITEM_NONE, AnimationTrigger.TRIGGER_ONE, GameSound.SOUND_NONE),   /* Action 1 */
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
                        new(noAction, GameItem.ITEM_PLAYER_MAIN, AnimationTrigger.TRIGGER_ONE, GameSound.SOUND_NONE),   /* Action 1 */
                    }
                ),
            }),
        };

        
    }
}