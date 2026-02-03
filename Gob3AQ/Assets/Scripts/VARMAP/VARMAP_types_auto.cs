

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
        INTERACTION_AUTO_6s, 
        INTERACTION_AUTO_CROSS_WAYPOINT, 
        INTERACTION_CROSS_DOOR, 
        
        INTERACTION_TOTAL
        /* > ATG 3 END < */
    }

    public enum UnchainType
    {
        /* > ATG 4 START < */
        UNCHAIN_TYPE_SPAWN, 
        UNCHAIN_TYPE_DESPAWN, 
        UNCHAIN_TYPE_DESTROY, 
        UNCHAIN_TYPE_UNCLICKABLE, 
        UNCHAIN_TYPE_SET_SPRITE, 
        UNCHAIN_TYPE_EARN_ITEM, 
        UNCHAIN_TYPE_LOSE_ITEM, 
        UNCHAIN_TYPE_EVENT, 
        UNCHAIN_TYPE_NOTIF, 
        UNCHAIN_TYPE_MEMENTO, 
        UNCHAIN_TYPE_DECISION, 
        UNCHAIN_TYPE_CHANGE_MOMENT_DAY, 
        
        UNCHAIN_TYPE_TOTAL
        /* > ATG 4 END < */
    }


    public enum DialogAnimation
    {
        /* > ATG 5 START < */
        DIALOG_ANIMATION_NONE = -1, 
        DIALOG_ANIMATION_TALK, 
        
        DIALOG_ANIMATION_TOTAL
        /* > ATG 5 END < */
    }

    public enum GameItemFamily
    {
        /* > ATG 6 START < */
        ITEM_FAMILY_TYPE_NONE = -1, 
        ITEM_FAMILY_TYPE_DOOR, 
        ITEM_FAMILY_TYPE_OBJECT, 
        ITEM_FAMILY_TYPE_NPC, 
        ITEM_FAMILY_TYPE_PLAYER, 
        
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
