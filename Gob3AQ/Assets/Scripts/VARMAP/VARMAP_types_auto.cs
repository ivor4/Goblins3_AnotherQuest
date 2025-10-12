

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
        INTERACTION_TAKE, 
        INTERACTION_USE, 
        INTERACTION_RECEIVE, 
        INTERACTION_TAKE_AND_RECEIVE, 
        INTERACTION_TALK, 
        
        INTERACTION_TOTAL
        /* > ATG 3 END < */
    }


    public enum DialogAnimation
    {
        /* > ATG 4 START < */
        DIALOG_ANIMATION_NONE = -1, 
        DIALOG_ANIMATION_TALK, 
        
        DIALOG_ANIMATION_TOTAL
        /* > ATG 4 END < */
    }

    public enum GameItemFamily
    {
        /* > ATG 5 START < */
        ITEM_FAMILY_TYPE_NONE = -1, 
        ITEM_FAMILY_TYPE_DOOR, 
        ITEM_FAMILY_TYPE_OBJECT, 
        ITEM_FAMILY_TYPE_NPC, 
        ITEM_FAMILY_TYPE_PLAYER, 
        
        ITEM_FAMILY_TYPE_TOTAL
        /* > ATG 5 END < */
    }


}
