using UnityEngine;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;

namespace Gob3AQ.ResourceAtlas
{
    public enum PrefabEnum
    {
        PREFAB_WAYPOINT,
        PREFAB_MENU_PICKABLE_ITEM,

        PREFAB_TOTAL
    }

    public static class ResourceAtlasClass
    {
        public static GameObject GetPrefab(PrefabEnum prefabId)
        {
            return Resources.Load<GameObject>(_PrefabList[(int)prefabId]);
        }

        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "Prefabs/Waypoint",
            "Prefabs/PickableItemDisplay"
        };
    }
}
