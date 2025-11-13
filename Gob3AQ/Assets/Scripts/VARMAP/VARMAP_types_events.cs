

namespace Gob3AQ.VARMAP.Types
{

    public enum GameEvent
    {
        /* > ATG 1 START < */
        EVENT_NONE = -1, 
        EVENT_RED_POTION_TOOK, 
        EVENT_BLUE_POTION_TOOK, 
        EVENT_FOUNTAIN_OBSERVED, 
        EVENT_FOUNTAIN_THOUGHT_MEMENTO, 
        EVENT_FOUNTAIN_FULL, 
        EVENT_LAST, 
        
        EVENT_TOTAL
        /* > ATG 1 END < */
    }

    public enum MementoParent
    {
        /* > ATG 2 START < */
        MEMENTO_PARENT_NONE = -1, 
        MEMENTO_PARENT_RED_POTION, 
        MEMENTO_PARENT_FOUNTAIN, 
        MEMENTO_PARENT_LAST, 
        
        MEMENTO_PARENT_TOTAL
        /* > ATG 2 END < */
    }

    public enum Memento
    {
        /* > ATG 3 START < */
        MEMENTO_NONE = -1, 
        MEMENTO_RED_POTION_1, 
        MEMENTO_RED_POTION_2, 
        MEMENTO_FOUNTAIN_1, 
        MEMENTO_FOUNTAIN_2, 
        MEMENTO_FOUNTAIN_3, 
        MEMENTO_LAST, 
        
        MEMENTO_TOTAL
        /* > ATG 3 END < */
    }

    public enum MementoCombi
    {
        /* > ATG 4 START < */
        MEMENTO_COMBI_NONE = -1, 
        MEMENTO_COMBI_POTION_FOUNTAIN, 
        
        MEMENTO_COMBI_TOTAL
        /* > ATG 4 END < */
    }

}
