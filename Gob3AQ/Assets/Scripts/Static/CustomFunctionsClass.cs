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
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_DETERGENT,  Custom_Lab_Add_Detergent},
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_INSECTICIDE,  Custom_Lab_Add_Insecticide},
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_VARNISH,  Custom_Lab_Add_Varnish},
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_RUST,  Custom_Lab_Add_Rust},
            {CustomFunction.CUSTOM_FUNCTION_LAB_POST_CHECK, Custom_Lab_Post_Check },
            {CustomFunction.CUSTOM_FUNCTION_SERVICE_JAR_INIT_STATE, Custom_Service_Jar_Init_State }
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

        private static void Custom_Lab_Add_Detergent()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_DETERGENT);
        }

        private static void Custom_Lab_Add_Insecticide()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_INSECTICIDE);
        }

        private static void Custom_Lab_Add_Varnish()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_VARNISH);
        }

        private static void Custom_Lab_Add_Rust()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_RUST);
        }

        private static void Custom_Lab_Add_Liquid(LabLiquid liquid)
        {
            Span<GameEventCombi> conditions = stackalloc GameEventCombi[1];
            Span<GameAction> actionGroup = stackalloc GameAction[5];

            ulong uval = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS);
            NumberPack npack = new(long1: (long)uval);
            LabLiquidConf liquidConf = new(in npack);

            conditions[0] = new(GameEvent.EVENT_LAB_PERFECT_MIX, false);

            VARMAP_GameEventMaster.IS_EVENT_COMBI_OCCURRED(conditions, out bool perfectMix);


            bool canMix = liquid != LabLiquid.LAB_LIQUID_NONE;
            canMix &= !perfectMix;
            canMix &= !liquidConf.isValid;
            canMix &= (liquid != LabLiquid.LAB_LIQUID_RUST) || ((liquidConf.nRust < 1) && (liquidConf.height == LabLiquidConf.MAX_HEIGHT_NR));
            canMix &= (liquid != LabLiquid.LAB_LIQUID_DETERGENT) || (liquidConf.nDetergent < 2);
            canMix &= (liquid != LabLiquid.LAB_LIQUID_VARNISH) || (liquidConf.nVarnish < 2);
            canMix &= (liquid is not (LabLiquid.LAB_LIQUID_FLOORWASHER or LabLiquid.LAB_LIQUID_INSECTICIDE)) || (liquidConf.height < LabLiquidConf.MAX_HEIGHT_NR);

            if (canMix)
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
                        animationAction = GameAction.ACTION_ANIMATION_DETERGENT_JUG;
                        break;
                    case LabLiquid.LAB_LIQUID_INSECTICIDE:
                        liquidConf.nInsecticide++;
                        animationAction = GameAction.ACTION_ANIMATION_INSECTICIDE_JUG;
                        break;
                    case LabLiquid.LAB_LIQUID_VARNISH:
                        liquidConf.nVarnish++;
                        animationAction = GameAction.ACTION_ANIMATION_VARNISH_JUG;
                        break;
                    default:
                        liquidConf.nRust++;
                        animationAction = GameAction.ACTION_ANIMATION_RUST_JUG;
                        break;
                }

                liquidConf.nTotal++;

                /* Sum height of liquids */
                liquidConf.height = liquidConf.nFloorwasher + liquidConf.nInsecticide;

                /* Powders can only increase height in first shot */
                if(liquidConf.height == 0)
                {
                    liquidConf.height += Mathf.Min(1, liquidConf.nDetergent + liquidConf.nVarnish);
                }

                /* Check if mixture is finally valid */
                liquidConf.isValid = liquidConf.nDetergent == 1;
                liquidConf.isValid &= liquidConf.nVarnish == 0;
                liquidConf.isValid &= liquidConf.nFloorwasher == 3;
                liquidConf.isValid &= liquidConf.nInsecticide == 2;
                liquidConf.isValid &= liquidConf.height == LabLiquidConf.MAX_HEIGHT_NR;
                liquidConf.isValid &= liquidConf.nRust == 1;

                npack = liquidConf.ToNumberPack(false);

                VARMAP_GameEventMaster.SET_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS, (ulong)npack.long1);

                actionGroup[0] = GameAction.ACTION_CUSTOM_UPDATE_JUG_LIQUID_VALUE;   /* With delay of 2.0s */
                actionGroup[1] = animationAction;
                actionGroup[2] = GameAction.ACTION_CUSTOM_LAB_POST_CHECK_VALUE;
                actionGroup[3] = GameAction.ACTION_EVENT_LAB_STARTED_MIX;
                actionGroup[4] = GameAction.ACTION_LOSE_RUST_POWDER;

                if(liquid != LabLiquid.LAB_LIQUID_RUST)
                {
                    actionGroup = actionGroup[..4];
                }

                VARMAP_GameEventMaster.PERFORM_ACTION(actionGroup, null);
            }
            else if(liquidConf.isValid || perfectMix)
            {
                actionGroup = actionGroup[..1];
                actionGroup[0] = GameAction.ACTION_DIALOGUE_TRY_USE_WITH_COMPLETE_JUG_MIX;

                VARMAP_GameEventMaster.PERFORM_ACTION(actionGroup, null);
            }
            else if((liquidConf.nRust == 0) && (liquid == LabLiquid.LAB_LIQUID_RUST))
            {
                actionGroup = actionGroup[..1];
                actionGroup[0] = GameAction.ACTION_DIALOGUE_MAINCHAR_NONSENSE_NOTYET;

                VARMAP_GameEventMaster.PERFORM_ACTION(actionGroup, null);
            }
            else
            {
                actionGroup = actionGroup[..1];
                actionGroup[0] = GameAction.ACTION_DIALOGUE_MAINCHAR_NONSENSE_TOO_MUCH;

                VARMAP_GameEventMaster.PERFORM_ACTION(actionGroup, null);
            }
        }

        private static void Custom_Lab_Post_Check()
        {
            Span<GameAction> actions = stackalloc GameAction[4];

            ulong uval = VARMAP_GameEventMaster.GET_SHADOW_ELEM_MISC_VALUES((int)MiscValuesIndex.MISC_VALUE_INDEX_LAB_PORTION_VALS);
            NumberPack npack = new(long1: (long)uval);
            LabLiquidConf liquidConf = new(in npack);

            if(liquidConf.isValid)
            {
                Custom_Reset_Lab_Misc_Values();

                actions[0] = GameAction.ACTION_EVENT_LAB_PERFECT_MIX;
                actions[1] = GameAction.ACTION_PLAY_SOUND_MIX_JAR_BUBBLES_LOOP;
                actions[2] = GameAction.ACTION_MEMENTO_HIVE_LAB_3;
                actions[3] = GameAction.ACTION_DIALOGUE_COMMENT_PERFECT_MIX_LAB;

                VARMAP_GameEventMaster.PERFORM_ACTION(actions, null);
            }
        }

        private static void Custom_Service_Jar_Init_State()
        {
            Span<GameEventCombi> conditions = stackalloc GameEventCombi[1];
            Span<GameAction> actions = stackalloc GameAction[3];

            conditions[0] = new(GameEvent.EVENT_LAB_STARTED_MIX, false);
            VARMAP_GameEventMaster.IS_EVENT_COMBI_OCCURRED(conditions, out bool startedMix);
            conditions[0] = new(GameEvent.EVENT_MIX_JAR_IN_INVENTORY, false);
            VARMAP_GameEventMaster.IS_EVENT_COMBI_OCCURRED(conditions, out bool jarInInventory);
            conditions[0] = new(GameEvent.EVENT_LAB_PERFECT_MIX, false);
            VARMAP_GameEventMaster.IS_EVENT_COMBI_OCCURRED(conditions, out bool perfectMix);

            if (perfectMix)
            {
                Span<GameAction> action = actions[..1];
                action[0] = GameAction.ACTION_DESTROY_LAB_WORKDESK;
                VARMAP_GameEventMaster.PERFORM_ACTION(action, null);
            }

            if (jarInInventory)
            {
                actions[0] = GameAction.ACTION_DESPAWN_MIX_JAR;
                actions[1] = GameAction.ACTION_DESPAWN_IMPERFECT_MIX_JAR;
                actions[2] = GameAction.ACTION_DESPAWN_PERFECT_MIX_JAR;
            }
            else if(perfectMix)
            {
                actions[0] = GameAction.ACTION_DESPAWN_MIX_JAR;
                actions[1] = GameAction.ACTION_DESPAWN_IMPERFECT_MIX_JAR;
                actions[2] = GameAction.ACTION_PLAY_SOUND_MIX_JAR_BUBBLES_LOOP;
            }
            else if(startedMix)
            {
                actions = actions[..2];
                actions[0] = GameAction.ACTION_DESPAWN_MIX_JAR;
                actions[1] = GameAction.ACTION_DESPAWN_PERFECT_MIX_JAR;
            }
            else
            {
                /* Empty jar on its place */
                actions = actions[..2];
                actions[0] = GameAction.ACTION_DESPAWN_IMPERFECT_MIX_JAR;
                actions[1] = GameAction.ACTION_DESPAWN_PERFECT_MIX_JAR;
            }

            VARMAP_GameEventMaster.PERFORM_ACTION(actions, null);
        }
    }
}
