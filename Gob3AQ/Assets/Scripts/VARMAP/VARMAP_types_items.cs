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
        ITEM_VICTIM, 
        ITEM_WITNESS1, 
        ITEM_WITNESS2, 
        ITEM_WITNESS3, 
        ITEM_ROOM1_DOOR_KITCHEN, 
        ITEM_ROOM1_DOOR_MAIN, 
        ITEM_LAST, 
        
        ITEM_TOTAL
        /* > ATG 1 END < */
    }

    public enum GamePickableItem
    {
        /* > ATG 2 START < */
        ITEM_PICK_NONE = -1,
        
        ITEM_PICK_TOTAL
        /* > ATG 2 END < */
    }

    public enum ActionConditions
    {
        /* > ATG 3 START < */
        COND_OK, 
        COND_ROOM1_OBSERVE_VICTIM, 
        COND_ROOM1_TALK_WITNESS1, 
        COND_ROOM1_TALK_WITNESS2, 
        COND_ROOM1_NOT_CROSS_DOOR, 
        COND_ROOM1_NOT_CROSS_MAIN_DOOR, 
        COND_LAST, 
        
        COND_TOTAL
        /* > ATG 3 END < */
    }

    public enum UnchainConditions
    {
        /* > ATG 4 START < */
        UNCHAIN_ROOM1_INITIAL_MEMENTO, 
        UNCHAIN_ROOM1_INITIAL_EVENT, 
        UNCHAIN_ROOM1_OBSERVE_VICTIM, 
        UNCHAIN_ROOM1_MEMENTO_CHASE_0, 
        UNCHAIN_ROOM1_MEMENTO_CHASE_0_bis, 
        UNCHAIN_ROOM1_NEED_FLEE, 
        UNCHAIN_ROOM1_MEMENTO_CHASE_1, 
        
        UNCHAIN_TOTAL
        /* > ATG 4 END < */
    }
}
