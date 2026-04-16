using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_INCLUDE_TESTS
namespace Gob3AQ.GameEventMaster
{
    public partial class GameEventMasterClass : MonoBehaviour
    {
        public IReadOnlyDictionary<GameEvent, HashSet<UnchainConditions>> Test_PendingUnchainDict => _pendingUnchainDict;

        public void Test_UpdateStep()
        {
            Update();
        }
    }
}

#endif
