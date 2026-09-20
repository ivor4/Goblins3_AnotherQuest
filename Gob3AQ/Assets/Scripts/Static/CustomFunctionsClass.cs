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
            {CustomFunction.CUSTOM_FUNCTION_RESET_LAB_MISC_VALUES, Custom_Reset_Lab_Misc_Values },
            {CustomFunction.CUSTOM_FUNCTION_RECOVER_LAB_MISC_VALUES, Custom_Lab_Recover_Values },
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_FLOORWASHER,  Custom_Lab_Add_Florwasher}
        };

        private static void Custom_Reset_Lab_Misc_Values()
        {
            for(int i = (int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_POURED_PORTIONS; i <= (int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS; ++i)
            {
                VARMAP_GameEventMaster.SET_ELEM_MISC_VALUES(i, 0UL);
            }
        }

        private static void Custom_Lab_Recover_Values()
        {
            ulong poured_nr = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_POURED_PORTIONS);

            poured_nr = Math.Clamp(poured_nr, 0, 4);

            /* TODO */
        }

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
