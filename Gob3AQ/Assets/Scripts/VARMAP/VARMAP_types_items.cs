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
        COND_TAKE_POTION_BLUE, 
        COND_FOUNTAIN, 
        COND_FOUNTAIN2, 
        COND_TALK_MILITO, 
        COND_LAST, 
        
        COND_TOTAL
        /* > ATG 3 END < */
    }

    public enum SpawnConditions
    {
        /* > ATG 4 START < */
        SPAWN_COND_NONE, 
        SPAWN_COND_FOUNTAIN, 
        
        SPAWN_COND_TOTAL
        /* > ATG 4 END < */
    }
}
