

namespace Gob3AQ.VARMAP.Types
{

    public enum GameEvent
    {
        /* > ATG 1 START < */
        EVENT_NONE = -1, 
        EVENT_MASTER_CHANGE_ROOM, 
        EVENT_MASTER_CHANGE_MOMENT_DAY, 
        EVENT_MASTER_PENDING_SLEEP_NAP, 
        EVENT_MASTER_PENDING_SLEEP_LONG, 
        EVENT_FIRST, 
        EVENT_HIVE1_CHEST_OPENED, 
        EVENT_CARDS_PICKABLE_TAKEN, 
        EVENT_HIVE1_WARDROBE_OPENED, 
        EVENT_HIVE1_AD_BOARD_OBSERVED_1, 
        EVENT_HIVE1_USED_PERFUME, 
        EVENT_HIVE1_USED_BASIN, 
        EVENT_SOAP_PICKABLE_TAKEN, 
        EVENT_LAUNCH_SLEEP_DECISION, 
        EVENT_CAN_GO_SOUTH_NEIGH, 
        EVENT_PHARMACY_EMPTY, 
        EVENT_MANYO_REFUSED_WORK, 
        EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, 
        EVENT_UMBRELLA_PICKABLE_TAKEN, 
        EVENT_POOR_MAN_WC_NEED_WATER, 
        EVENT_COCKROACH_CHEATED, 
        EVENT_PIPE_OBSERVATION_1, 
        EVENT_COCKROACH_SCARED, 
        EVENT_LAST, 
        
        EVENT_TOTAL
        /* > ATG 1 END < */
    }

    public enum MementoParent
    {
        /* > ATG 2 START < */
        MEMENTO_PARENT_NONE = -1, 
        MEMENTO_PARENT_JOB_FIND_1, 
        MEMENTO_PARENT_RECIPE_MISSION, 
        MEMENTO_PARENT_POOR_MAN_WC, 
        MEMENTO_PARENT_LAST, 
        
        MEMENTO_PARENT_TOTAL
        /* > ATG 2 END < */
    }

    public enum Memento
    {
        /* > ATG 3 START < */
        MEMENTO_NONE = -1, 
        MEMENTO_JOB_FIND_1_1, 
        MEMENTO_JOB_FIND_1_2, 
        MEMENTO_RECIPE_MISSION_1, 
        MEMENTO_POOR_MAN_WC_1, 
        MEMENTO_POOR_MAN_WC_2, 
        MEMENTO_POOR_MAN_WC_3, 
        MEMENTO_LAST, 
        
        MEMENTO_TOTAL
        /* > ATG 3 END < */
    }

    public enum MementoCombi
    {
        /* > ATG 4 START < */
        MEMENTO_COMBI_NONE = -1, 
        
        MEMENTO_COMBI_TOTAL
        /* > ATG 4 END < */
    }

}
