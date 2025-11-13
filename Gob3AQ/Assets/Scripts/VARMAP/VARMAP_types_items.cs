using UnityEngine;

namespace Gob3AQ.VARMAP.Types
{
    public enum GameItem
    {
        /* > ATG 1 START < */
        ITEM_NONE = -1, 
        ITEM_PLAYER_MAIN, 
        ITEM_PLAYER_PARROT, 
        ITEM_PLAYER_SNAKE, 
        ITEM_POTION, 
        ITEM_POTION_BLUE, 
        ITEM_FOUNTAIN, 
        ITEM_NPC_MILITO, 
        ITEM_LAST, 
        
        ITEM_TOTAL
        /* > ATG 1 END < */
    }

    public enum GamePickableItem
    {
        /* > ATG 2 START < */
        ITEM_PICK_NONE = -1,
        ITEM_PICK_POTION, 
        ITEM_PICK_POTION_BLUE, 
        
        ITEM_PICK_TOTAL
        /* > ATG 2 END < */
    }

    public enum ActionConditions
    {
        /* > ATG 3 START < */
        COND_OK, 
        COND_TAKE_POTION, 
        COND_OBSERVE_POTION, 
        COND_OBSERVE_FOUNTAIN, 
        COND_TAKE_POTION_BLUE, 
        COND_FOUNTAIN_THOUGHT, 
        COND_FOUNTAIN_NOT_THOUGHT, 
        COND_TALK_MILITO, 
        COND_LAST, 
        
        COND_TOTAL
        /* > ATG 3 END < */
    }

    public enum UnchainConditions
    {
        /* > ATG 4 START < */
        UNCHAIN_RED_POTION_TOOK, 
        UNCHAIN_RED_POTION_TOOK_2, 
        UNCHAIN_RED_POTION_TOOK_3, 
        UNCHAIN_BLUE_POTION_TOOK, 
        UNCHAIN_BLUE_POTION_TOOK_2, 
        UNCHAIN_FOUNTAIN_OBSERVED, 
        UNCHAIN_FOUNTAIN_THOUGHT_MEMENTO, 
        UNCHAIN_FOUNTAIN_FULL, 
        UNCHAIN_FOUNTAIN_FULL_2, 
        UNCHAIN_FOUNTAIN_FULL_3, 
        UNCHAIN_FOUNTAIN_FULL_4, 
        
        UNCHAIN_TOTAL
        /* > ATG 4 END < */
    }
}
