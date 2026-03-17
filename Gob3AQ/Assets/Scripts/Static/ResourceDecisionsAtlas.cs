using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceDecisionsAtlas
{
    public static class ResourceDecisionsAtlasClass
    {
        public static ref readonly DecisionConfig GetDecisionConfig(DecisionType decision)
        {
            if((uint)decision < (uint)DecisionType.DECISION_TOTAL)
            {
                return ref _DecisionConfig[(int)decision];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDecisionConfig: Invalid decision {decision}");
                return ref DecisionConfig.EMPTY;
            }
        }

        public static ref readonly DecisionOptionConfig GetDecisionOptionConfig(DecisionOption option)
        {
            if((uint)option < (uint)DecisionOption.DECISION_OPTION_TOTAL)
            {
                return ref _DecisionOptionConfig[(int)option];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDecisionOptionConfig: Invalid decision option {option}");
                return ref DecisionOptionConfig.EMPTY;
            }
        }



        private static readonly DecisionConfig[] _DecisionConfig = new DecisionConfig[(int)DecisionType.DECISION_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* DECISION_SLEEP_1 */
            new DecisionOption[3]{DecisionOption.DECISION_OPTION_NOT_SLEEP,DecisionOption.DECISION_OPTION_SLEEP_DAY,DecisionOption.DECISION_OPTION_SLEEP_NIGHT,}),
            new( /* DECISION_ID_LAST */
            new DecisionOption[1]{DecisionOption.DECISION_OPTION_LAST,}),
            /* > ATG 1 END < */
        };

        private static readonly DecisionOptionConfig[] _DecisionOptionConfig = new DecisionOptionConfig[(int)DecisionOption.DECISION_OPTION_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* DECISION_OPTION_NOT_SLEEP */
            DialogPhrase.PHRASE_DECISION_NOT_SLEEP,
            new GameAction[1]{GameAction.ACTION_NONE, }),
            new( /* DECISION_OPTION_SLEEP_DAY */
            DialogPhrase.PHRASE_DECISION_SLEEP_DAY,
            new GameAction[1]{GameAction.ACTION_DAY_MOMENT_DAY, }),
            new( /* DECISION_OPTION_SLEEP_NIGHT */
            DialogPhrase.PHRASE_DECISION_SLEEP_NIGHT,
            new GameAction[1]{GameAction.ACTION_DAY_MOMENT_NIGHT, }),
            new( /* DECISION_OPTION_LAST */
            DialogPhrase.PHRASE_NONE,
            new GameAction[1]{GameAction.ACTION_NONE, }),
            /* > ATG 2 END < */
        };
    }
}