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
                { Animator.StringToHash("Steady1"), AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE }, 
                { Animator.StringToHash("Steady2"), AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO }, 
                { Animator.StringToHash("Group_Cycle1"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE },
                { Animator.StringToHash("Group_Cycle2"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_TWO },
                { Animator.StringToHash("Group_Cycle3"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_THREE },
                { Animator.StringToHash("Group_Cycle4"), AnimationTrigger.ANIMATION_TRIGGER_CYCLE_FOUR },
                { Animator.StringToHash("Group_Talk"), AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE },
                { Animator.StringToHash("Group_Talk2"), AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO },
                { Animator.StringToHash("Group_Talk3"), AnimationTrigger.ANIMATION_TRIGGER_TALK_THREE },
                { Animator.StringToHash("Group_Talk4"), AnimationTrigger.ANIMATION_TRIGGER_TALK_FOUR },
                { Animator.StringToHash("Action1"), AnimationTrigger.ANIMATION_TRIGGER_ONE },
                { Animator.StringToHash("Action2"), AnimationTrigger.ANIMATION_TRIGGER_TWO },
                { Animator.StringToHash("Action3"), AnimationTrigger.ANIMATION_TRIGGER_THREE },
                { Animator.StringToHash("Action4"), AnimationTrigger.ANIMATION_TRIGGER_FOUR },
                { Animator.StringToHash("Action5"), AnimationTrigger.ANIMATION_TRIGGER_FIVE },
                { Animator.StringToHash("Action6"), AnimationTrigger.ANIMATION_TRIGGER_SIX },
                { Animator.StringToHash("Action7"), AnimationTrigger.ANIMATION_TRIGGER_SEVEN },
                { Animator.StringToHash("Action8"), AnimationTrigger.ANIMATION_TRIGGER_EIGHT },
                { Animator.StringToHash("Group_WalkBack"), AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK },
                { Animator.StringToHash("Group_WalkFront"), AnimationTrigger.ANIMATION_TRIGGER_WALK_FRONT },
                { Animator.StringToHash("Group_WalkCornerBack"), AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERBACK },
                { Animator.StringToHash("Group_WalkCornerFront"), AnimationTrigger.ANIMATION_TRIGGER_WALK_CORNERFRONT },
                { Animator.StringToHash("Group_WalkSide"), AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE }
            };
        public static readonly int TRANSITION_TRIGGER_HASH = Animator.StringToHash("TransitionTrigger");
        public static readonly int TRANSITION_TRIGGER_EXT_HASH = Animator.StringToHash("TransitionTriggerOutside");
        public static readonly int ANIMATION_INDEX_HASH = Animator.StringToHash("AnimationIndex");
        public static readonly int STEADY_INDEX_HASH = Animator.StringToHash("SteadyIndex");

        public static bool IsTriggerSteady(AnimationTrigger trigger)
        {
            return trigger
                is >= AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE 
                and <= AnimationTrigger.ANIMATION_TRIGGER_STEADY_FOUR; 
        }
        
        public static bool IsTriggerWalking(AnimationTrigger trigger)
        {
            return trigger is 
                >= AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK
                and <= AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE;
        }

        public static bool IsTriggerCycled(AnimationTrigger trigger)
        {
            return trigger is 
                (
                >= AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE
                and <= AnimationTrigger.ANIMATION_TRIGGER_CYCLE_FOUR
                )
                or
                (
                >= AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE
                and <= AnimationTrigger.ANIMATION_TRIGGER_TALK_FOUR
                )
                or
                (
                >= AnimationTrigger.ANIMATION_TRIGGER_WALK_BACK
                and <= AnimationTrigger.ANIMATION_TRIGGER_WALK_SIDE
                );
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