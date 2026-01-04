using UnityEngine;

namespace Gob3AQ.VARMAP.Types
{
    public enum GameItem
    {
        /* > ATG 1 START < */
        ITEM_NONE = -1, 
        ITEM_PLAYER_MAIN, 
        ITEM_HIVE1_CHEST, 
        ITEM_CARDS_PICKABLE, 
        ITEM_LAST, 
        
        ITEM_TOTAL
        /* > ATG 1 END < */
    }

    public enum GamePickableItem
    {
        /* > ATG 2 START < */
        ITEM_PICK_NONE = -1,
        ITEM_PICK_CARDS_PICKABLE, 
        
        ITEM_PICK_TOTAL
        /* > ATG 2 END < */
    }

    public enum ActionConditions
    {
        /* > ATG 3 START < */
        COND_OK, 
        COND_OPEN_CHEST, 
        COND_LAST, 
        
        COND_TOTAL
        /* > ATG 3 END < */
    }

    public enum UnchainConditions
    {
        /* > ATG 4 START < */
        UNCHAIN_ROOM1_INITIAL_MEMENTO, 
        UNCHAIN_HIVE1_OPEN_CHEST_1, 
        UNCHAIN_HIVE1_OPEN_CHEST_2, 
        
        UNCHAIN_TOTAL
        /* > ATG 4 END < */
    }
}
