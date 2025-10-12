using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Gob3AQ.VARMAP.Config
{
    public static class VARMAP_Config
    {
        /* Margin must be at least 1 unit higher than  2 * safe variables. */
        public const uint VARMAP_SAFE_VARIABLES = 30;
        public const uint VARMAP_SAFE_SLOTS = 2 * VARMAP_SAFE_VARIABLES;

        public const uint VARMAP_SAFE_RUBISH_BIN_MARGIN = 128 - VARMAP_SAFE_SLOTS;

        public const uint VARMAP_SAFE_RUBISH_BIN_SIZE = VARMAP_SAFE_RUBISH_BIN_MARGIN + VARMAP_SAFE_SLOTS;
    }
}


