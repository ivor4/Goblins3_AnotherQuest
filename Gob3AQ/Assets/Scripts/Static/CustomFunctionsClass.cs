using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gob3AQ.Brain.CustomFunctions
{
    public static class CustomFunctionsClass
    {
        public static readonly IReadOnlyDictionary<CustomFunction, Action> CUSTOM_FN_DICT = new Dictionary<CustomFunction, Action>()
        {
            {CustomFunction.CUSTOM_FUNCTION_NONE, null },
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_FLOORWASHER,  Custom_Lab_Add_Florwasher}
        };

        private static void Custom_Lab_Add_Florwasher()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_FLOORWASHER);
        }

        private static void Custom_Lab_Add_Liquid(LabLiquid liquid)
        {
            GameAction gameAction = GameAction.ACTION_ANIMATION_FLOORWASHER_JUG;

            Span<GameAction> actions = MemoryMarshal.CreateSpan(ref gameAction, 1);

            VARMAP_GameEventMaster.PERFORM_ACTION(actions, null);
        }
    }
}
