using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ResourceAnimationsAtlas
{
    public static class ResourceAnimationsAtlasClass
    {
        public static readonly IReadOnlyDictionary<int, AnimationTrigger> STATE_HASH_TO_TRIGGER =
            new Dictionary<int, AnimationTrigger>()
            {
                { Animator.StringToHash("Steady"), AnimationTrigger.ANIMATION_TRIGGER_STEADY }, 
                { Animator.StringToHash("Steady2"), AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO }, 
                { Animator.StringToHash("SpecialCycleAction1"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE },
                { Animator.StringToHash("SpecialCycleAction2"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_TWO },
                { Animator.StringToHash("SpecialCycleAction3"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_THREE },
                { Animator.StringToHash("SpecialCycleAction4"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_FOUR },
                { Animator.StringToHash("Talking"), AnimationTrigger.ANIMATION_TRIGGER_TALK },
                { Animator.StringToHash("SpecialAction1"), AnimationTrigger.ANIMATION_TRIGGER_ONE },
                { Animator.StringToHash("SpecialAction2"), AnimationTrigger.ANIMATION_TRIGGER_TWO },
                { Animator.StringToHash("SpecialAction3"), AnimationTrigger.ANIMATION_TRIGGER_THREE },
                { Animator.StringToHash("SpecialAction4"), AnimationTrigger.ANIMATION_TRIGGER_FOUR },
                { Animator.StringToHash("SpecialAction5"), AnimationTrigger.ANIMATION_TRIGGER_FIVE },
                { Animator.StringToHash("SpecialAction6"), AnimationTrigger.ANIMATION_TRIGGER_SIX },
                { Animator.StringToHash("SpecialAction7"), AnimationTrigger.ANIMATION_TRIGGER_SEVEN },
                { Animator.StringToHash("SpecialAction8"), AnimationTrigger.ANIMATION_TRIGGER_EIGHT },

            };
        public static readonly int TRANSITION_TRIGGER_HASH = Animator.StringToHash("TransitionTrigger");
        public static readonly int ANIMATION_INDEX_HASH = Animator.StringToHash("AnimationIndex");

        public static bool IsTriggerSteady(AnimationTrigger trigger)
        {
            return trigger == AnimationTrigger.ANIMATION_TRIGGER_STEADY || 
                   trigger == AnimationTrigger.ANIMATION_TRIGGER_AUTO_STEADY || 
                   trigger == AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO;
        }
            
        

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

        private static readonly GameAction[] noAction = Array.Empty<GameAction>();


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