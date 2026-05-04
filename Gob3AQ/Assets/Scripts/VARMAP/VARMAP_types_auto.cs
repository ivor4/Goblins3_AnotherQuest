

using System;

namespace Gob3AQ.VARMAP.Types
{
    
    public enum CharacterType
    {
        /* > ATG 1 START < */
        CHARACTER_NONE = -1, 
        CHARACTER_MAIN, 
        CHARACTER_PARROT, 
        CHARACTER_SNAKE, 
        
        CHARACTER_TOTAL
        /* > ATG 1 END < */
    }



    public enum CharacterAnimation
    {
        /* > ATG 2 START < */
        ITEM_USE_ANIMATION_NONE = -1, 
        ITEM_USE_ANIMATION_NORMAL, 
        ITEM_USE_ANIMATION_TAKE, 
        ITEM_USE_ANIMATION_CONFUSE, 
        ITEM_USE_ANIMATION_STARE_SCREEN, 
        ITEM_USE_ANIMATION_POUR, 
        
        ITEM_USE_ANIMATION_TOTAL
        /* > ATG 2 END < */
    }

    public enum ItemInteractionType
    {
        /* > ATG 3 START < */
        INTERACTION_NONE = -1, 
        INTERACTION_MOVE, 
        INTERACTION_USE, 
        INTERACTION_TAKE, 
        INTERACTION_OBSERVE, 
        INTERACTION_TALK, 
        INTERACTION_COMBINE, 
        INTERACTION_CROSS_DOOR, 
        
        INTERACTION_TOTAL
        /* > ATG 3 END < */
    }

    public enum ActionType
    {
        /* > ATG 4 START < */
        ACTION_TYPE_NONE = -1, 
        ACTION_TYPE_SPAWN, 
        ACTION_TYPE_DESPAWN, 
        ACTION_TYPE_DESTROY, 
        ACTION_TYPE_UNCLICKABLE, 
        ACTION_TYPE_SET_SPRITE, 
        ACTION_TYPE_TRIGGER_ITEM_ANIMATION, 
        ACTION_TYPE_EARN_ITEM, 
        ACTION_TYPE_LOSE_ITEM, 
        ACTION_TYPE_EVENT, 
        ACTION_TYPE_NOTIF, 
        ACTION_TYPE_MEMENTO, 
        ACTION_TYPE_DECISION, 
        ACTION_TYPE_START_DIALOGUE, 
        ACTION_TYPE_START_DIALOGUE_BCKG, 
        ACTION_TYPE_START_ANIMATION, 
        ACTION_TYPE_START_CARD_GAME, 
        ACTION_TYPE_CHANGE_MOMENT_DAY, 
        
        ACTION_TYPE_TOTAL
        /* > ATG 4 END < */
    }


    public enum AnimationTrigger
    {
        /* > ATG 5 START < */
        ANIMATION_TRIGGER_NONE = -1, 
        ANIMATION_TRIGGER_ONE, 
        ANIMATION_TRIGGER_TWO, 
        ANIMATION_TRIGGER_THREE, 
        ANIMATION_TRIGGER_FOUR, 
        ANIMATION_TRIGGER_FIVE, 
        ANIMATION_TRIGGER_SIX, 
        ANIMATION_TRIGGER_SEVEN, 
        ANIMATION_TRIGGER_EIGHT, 
        ANIMATION_TRIGGER_CYCLE_ONE, 
        ANIMATION_TRIGGER_CYCLE_TWO, 
        ANIMATION_TRIGGER_CYCLE_THREE, 
        ANIMATION_TRIGGER_CYCLE_FOUR, 
        ANIMATION_TRIGGER_STEADY, 
        ANIMATION_TRIGGER_TALK, 
        ANIMATION_TRIGGER_WALK_FRONT, 
        ANIMATION_TRIGGER_WALK_BACK, 
        ANIMATION_TRIGGER_WALK_CORNERBACK, 
        ANIMATION_TRIGGER_WALK_CORNERFRONT, 
        ANIMATION_TRIGGER_WALK_SIDE, 
        
        DIALOG_ANIMATION_TOTAL
        /* > ATG 5 END < */
    }

    public enum GameItemFamily
    {
        /* > ATG 6 START < */
        ITEM_FAMILY_TYPE_NONE = -1, 
        ITEM_FAMILY_TYPE_PLAYER, 
        ITEM_FAMILY_TYPE_DOOR, 
        ITEM_FAMILY_TYPE_NPC, 
        ITEM_FAMILY_TYPE_OBJECT, 
        
        ITEM_FAMILY_TYPE_TOTAL
        /* > ATG 6 END < */
    }

    public enum MomentType
    {
        /* > ATG 7 START < */
        MOMENT_MORNING, 
        MOMENT_NIGHT, 
        MOMENT_ANY, 
        
        MOMENT_TOTAL
        /* > ATG 7 END < */

    }


}
