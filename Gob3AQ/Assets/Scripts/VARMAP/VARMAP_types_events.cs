

namespace Gob3AQ.VARMAP.Types
{

    public enum GameEvent
    {
        /* > ATG 1 START < */
        EVENT_NONE = -1, 
        EVENT_FIRST, 
        EVENT_OBSERVE_VICTIM, 
        EVENT_AWARE_CHASE_1, 
        EVENT_AWARE_CHASE_2, 
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
