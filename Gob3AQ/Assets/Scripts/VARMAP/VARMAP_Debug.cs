using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP;
using Gob3AQ.VARMAP.Variable;


namespace Gob3AQ.VARMAP.Debug
{
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
    public sealed class VARMAP_Debug : VARMAP
    {
        public static VARMAP_Variable_Indexable[] GetRef()
        {
            return DATA;
        }
    }
#endif
}

