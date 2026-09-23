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
            {CustomFunction.CUSTOM_FUNCTION_UPDATE_JUG_LIQUID, Custom_Lab_Update_Values },
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_FLOORWASHER,  Custom_Lab_Add_Florwasher},
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_VARNISH,  Custom_Lab_Add_Varnish}
        };

        private static void Custom_Reset_Lab_Misc_Values()
        {
            for(int i = (int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS; i <= (int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS; ++i)
            {
                VARMAP_GameEventMaster.SET_ELEM_MISC_VALUES(i, 0UL);
            }
        }

        private static void Custom_Lab_Recover_Values()
        {
            ulong uval = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS);

            NumberPack npack = new(true, long1: (long)uval);

            VARMAP_GameEventMaster.EXECUTE_ITEM_EXT_FUNCTION(ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID, in npack);
        }

        private static void Custom_Lab_Update_Values()
        {
            ulong uval = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS);

            NumberPack npack = new(false, long1: (long)uval);

            VARMAP_GameEventMaster.EXECUTE_ITEM_EXT_FUNCTION(ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID, in npack);
        }

        private static void Custom_Lab_Add_Florwasher()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_FLOORWASHER);
        }

        private static void Custom_Lab_Add_Varnish()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_VARNISH);
        }

        private static void Custom_Lab_Add_Liquid(LabLiquid liquid)
        {
            Span<GameAction> twoActions = stackalloc GameAction[2];

            ulong uval = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS);
            NumberPack npack = new(long1: (long)uval);
            LabLiquidConf liquidConf = new(in npack);
            

            if ((liquidConf.nTotal < LabLiquidConf.MAX_DOSE_NR) && (liquid != LabLiquid.LAB_LIQUID_NONE))
            {
                GameAction animationAction;

                switch (liquid)
                {
                    case LabLiquid.LAB_LIQUID_FLOORWASHER:
                        liquidConf.nFloorwasher++;
                        animationAction = GameAction.ACTION_ANIMATION_FLOORWASHER_JUG;
                        break;
                    case LabLiquid.LAB_LIQUID_DETERGENT:
                        liquidConf.nDetergent++;
                        animationAction = GameAction.ACTION_ANIMATION_FLOORWASHER_JUG;
                        break;
                    case LabLiquid.LAB_LIQUID_INSECTICIDE:
                        liquidConf.nInsecticide++;
                        animationAction = GameAction.ACTION_ANIMATION_FLOORWASHER_JUG;
                        break;
                    case LabLiquid.LAB_LIQUID_VARNISH:
                        liquidConf.nVarnish++;
                        animationAction = GameAction.ACTION_ANIMATION_VARNISH_JUG;
                        break;
                    default:
                        liquidConf.nRust++;
                        animationAction = GameAction.ACTION_ANIMATION_FLOORWASHER_JUG;
                        break;
                }

                liquidConf.nTotal++;

                npack = liquidConf.ToNumberPack(false);

                VARMAP_GameEventMaster.SET_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS, (ulong)npack.long1);

                twoActions[0] = GameAction.ACTION_CUSTOM_UPDATE_JUG_LIQUID_VALUE;   /* With delay of 2.0s */
                twoActions[1] = animationAction;
                

                VARMAP_GameEventMaster.PERFORM_ACTION(twoActions, null);
            }
        }
    }
}
