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
        ITEM_HIVE1_WARDROBE, 
        ITEM_HIVE1_WARDROBE_OPENED, 
        ITEM_GENERIC_DOOR1, 
        ITEM_HIVE1_NPC_REME, 
        ITEM_GENERIC_DOOR2, 
        ITEM_GENERIC_DOOR3, 
        ITEM_HIVE1_PERFUME, 
        ITEM_HIVE1_AD_BOARD, 
        ITEM_HIVE1_EXIT_DOOR, 
        ITEM_HIVE1_BASIN, 
        ITEM_SOAP_PICKABLE, 
        ITEM_LAST, 
        
        ITEM_TOTAL
        /* > ATG 1 END < */
    }

    public enum GamePickableItem
    {
        /* > ATG 2 START < */
        ITEM_PICK_NONE = -1,
        ITEM_PICK_CARDS_PICKABLE, 
        ITEM_PICK_SOAP_PICKABLE, 
        
        ITEM_PICK_TOTAL
        /* > ATG 2 END < */
    }

    public enum ActionConditions
    {
        /* > ATG 3 START < */
        COND_OK, 
        COND_OPEN_CHEST, 
        COND_CLOSE_CHEST, 
        COND_TAKE_CARDS, 
        COND_OPEN_HIVE1_WARDROBE, 
        COND_CLOSE_HIVE1_WARDROBE, 
        COND_TALK_REME_1, 
        COND_OBSERVE_HIVE1_AD_BOARD_1, 
        COND_EXIT_HIVE1_HALL_1, 
        COND_EXIT_HIVE1_HALL_2, 
        COND_EXIT_HIVE1_HALL_3, 
        COND_USE_HIVE1_BASIN_NO_SOAP, 
        COND_USE_HIVE1_BASIN_W_SOAP, 
        COND_USE_HIVE1_BASIN_W_SOAP_REPEAT, 
        COND_TAKE_SOAP, 
        COND_LAST, 
        
        COND_TOTAL
        /* > ATG 3 END < */
    }

    public enum UnchainConditions
    {
        /* > ATG 4 START < */
        UNCHAIN_ROOM1_INITIAL_MEMENTO_1, 
        UNCHAIN_ROOM1_INITIAL_MEMENTO_2, 
        UNCHAIN_HIVE1_OPEN_CHEST_1, 
        UNCHAIN_HIVE1_OPEN_CHEST_2, 
        UNCHAIN_HIVE1_CLOSE_CHEST_1, 
        UNCHAIN_HIVE1_CLOSE_CHEST_2, 
        UNCHAIN_CARDS_PICKABLE_TAKE_1, 
        UNCHAIN_CARDS_PICKABLE_TAKE_2, 
        UNCHAIN_HIVE1_OPEN_WARDROBE_1, 
        UNCHAIN_HIVE1_OPEN_WARDROBE_2, 
        UNCHAIN_HIVE1_OPEN_WARDROBE_3, 
        UNCHAIN_HIVE1_CLOSE_WARDROBE_1, 
        UNCHAIN_HIVE1_CLOSE_WARDROBE_2, 
        UNCHAIN_HIVE1_CLOSE_WARDROBE_3, 
        UNCHAIN_OBSERVE_HIVE1_AD_BOARD_1, 
        UNCHAIN_SOAP_PICKABLE_TAKE_1, 
        UNCHAIN_SOAP_PICKABLE_TAKE_2, 
        
        UNCHAIN_TOTAL
        /* > ATG 4 END < */
    }
}
