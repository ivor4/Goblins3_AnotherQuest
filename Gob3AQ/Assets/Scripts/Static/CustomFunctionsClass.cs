using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Gob3AQ.Brain.CustomFunctions
{
    public static class CustomFunctionsClass
    {
        public static readonly IReadOnlyDictionary<CustomFunction, Action> CUSTOM_FN_DICT = new Dictionary<CustomFunction, Action>()
        {
            {CustomFunction.CUSTOM_FUNCTION_LAB_ADD_FLOORWASHER,  Custom_Lab_Add_Florwasher}
        };

        private static void Custom_Lab_Add_Florwasher()
        {
            Custom_Lab_Add_Liquid(LabLiquid.LAB_LIQUID_FLOORWASHER);
        }

        private static void Custom_Lab_Add_Liquid(LabLiquid liquid)
        {
            PlayableDirector director = GameObject.Find("VarnishDirector").GetComponent<PlayableDirector>();
            director.Play();
        }
    }
}
