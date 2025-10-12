using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP;
using Gob3AQ.VARMAP.Variable;


namespace Gob3AQ.VARMAP.Debug
{
    public sealed class VARMAP_Debug_Master : VARMAP
    {
#if UNITY_EDITOR

        public static VARMAP_Variable_Indexable[] GetRef()
        {
            return DATA;
        }
#endif
    }
}

