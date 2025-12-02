

namespace Gob3AQ.VARMAP.Types
{

    public enum GameEvent
    {
        /* > ATG 1 START < */
        EVENT_NONE = -1, 
        EVENT_FIRST, 
        EVENT_ROOM1_OBSERVE_VICTIM, 
        EVENT_ROOM1_AWARE_CHASE_1, 
        EVENT_ROOM1_AWARE_CHASE_2, 
        EVENT_ROOM1_NEED_FLEE, 
        EVENT_ROOM1_SPOON_TAKEN, 
        EVENT_ROOM1_SPOON_WITH_POISON_TAKEN, 
        EVENT_ROOM1_OLD_KEY_TAKEN, 
        EVENT_ROOM1_WINDOW_OPENED, 
        EVENT_ROOM1_TROWEL_TAKEN, 
        EVENT_ROOM1_CAR_OPENED, 
        EVENT_ROOM1_MATCHES_TAKEN, 
        EVENT_ROOM1_CANDLE_FIRE, 
        EVENT_ROOM1_TAKE_LOCKPICK, 
        EVENT_ROOM1_DRAWER_OPENED, 
        EVENT_ROOM1_TAKE_CONTRACT, 
        EVENT_ROOM1_TAKE_JAR, 
        EVENT_ROOM1_FOUNTAIN_FULL, 
        EVENT_LAST, 
        
        EVENT_TOTAL
        /* > ATG 1 END < */
    }

    public enum MementoParent
    {
        /* > ATG 2 START < */
        MEMENTO_PARENT_NONE = -1, 
        MEMENTO_PARENT_VICTIM_CASE, 
        MEMENTO_PARENT_CHASE, 
        MEMENTO_PARENT_LAST, 
        
        MEMENTO_PARENT_TOTAL
        /* > ATG 2 END < */
    }

    public enum Memento
    {
        /* > ATG 3 START < */
        MEMENTO_NONE = -1, 
        MEMENTO_VICTIM_CASE_0, 
        MEMENTO_VICTIM_CASE_1, 
        MEMENTO_VICTIM_CASE_2, 
        MEMENTO_CHASE_0, 
        MEMENTO_CHASE_1, 
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
