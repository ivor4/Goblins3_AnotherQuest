using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MVerse.VARMAP.Config
{
    public static class VARMAP_Config
    {
        /* Margin must be at least 2 units higher than safe variables. Both summed must be exactly a power of 2 */
        public const uint VARMAP_SAFE_VARIABLES = 15;
        public const uint VARMAP_SAFE_RUBISH_BIN_MARGIN = 128- VARMAP_SAFE_VARIABLES;

        public const uint VARMAP_SAFE_RUBISH_BIN_SIZE = VARMAP_SAFE_RUBISH_BIN_MARGIN + VARMAP_SAFE_VARIABLES;

        public const uint VARMAP_SAFE_RUBISH_BIN_SIZE_MASK = VARMAP_SAFE_RUBISH_BIN_SIZE - 1;
    }
}


