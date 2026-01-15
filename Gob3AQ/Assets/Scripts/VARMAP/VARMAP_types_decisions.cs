using UnityEngine;

namespace Gob3AQ.VARMAP.Types
{
    public enum DecisionType
    {
        /* > ATG 1 START < */
        DECISION_NONE = -1, 
        DECISION_SLEEP_1, 
        DECISION_ID_LAST, 
        
        DECISION_TOTAL
        /* > ATG 1 END < */
    }

    public enum DecisionOption
    {
        /* > ATG 2 START < */
        DECISION_OPTION_NONE = -1, 
        DECISION_OPTION_NOT_SLEEP, 
        DECISION_OPTION_SLEEP_NAP, 
        DECISION_OPTION_SLEEP_LONG, 
        DECISION_OPTION_LAST, 
        
        DECISION_OPTION_TOTAL
        /* > ATG 2 END < */
    }
}
