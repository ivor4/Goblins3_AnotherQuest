

namespace Gob3AQ.VARMAP.Types
{
    public enum GameAnimation
    {
        /* > ATG 1 START < */
        ANIMATION_NONE = -1, 
        ANIMATION_REME_TEST, 
        ANIMATION_LAST, 
        
        ANIMATION_TOTAL
        /* > ATG 1 END < */
    }


    public enum AnimationSrcTrigger
    {
        SRC_TRIGGER_TIME_FROM_PREV,
        SRC_TRIGGER_CALLBACK,
        SRC_TRIGGER_ANIM_END
    }

    public enum AnimationTrigger
    {
        TRIGGER_ONE,
        TRIGGER_TWO,
        TRIGGER_THREE,
        TRIGGER_FOUR,
        TRIGGER_FIVE,
        TRIGGER_SIX,

        TRIGGER_SPECIAL_ONE,
        TRIGGER_SPECIAL_TWO,

        TRIGGER_STEADY,

        TRIGGER_TALK,
        TRIGGER_WALK_FRONT,
        TRIGGER_WALK_BACK,
        TRIGGER_WALK_CORNERFRONT,
        TRIGGER_WALK_CORNERBACK,
        TRIGGER_WALK_SIDE,

        TRIGGER_TOTAL
    }
}
