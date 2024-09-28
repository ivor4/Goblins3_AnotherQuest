using System.Collections;
using System.Collections.Generic;
using MVerse.VARMAP.Types;
using MVerse.VARMAP.DefaultValues;

namespace MVerse.GameOptions
{
    
    public static class GameOptionsClass
    {
        public static GameOptionsStruct GetDefaults(out bool getoptionsfromlevel_error)
        {
            getoptionsfromlevel_error = false;

            return VARMAP_DefaultValues.GameOptionsStruct_Default;
        }

        public static GameOptionsStruct GetSavedValues(out bool getoptionsfromlevel_error)
        {
            getoptionsfromlevel_error = false;
            return default(GameOptionsStruct);
        }
    }
}
