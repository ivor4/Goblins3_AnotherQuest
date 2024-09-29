using System.Collections;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.DefaultValues;

namespace Gob3AQ.GameOptions
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
