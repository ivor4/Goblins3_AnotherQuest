using UnityEngine;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.ResourceAtlas
{
    public enum PrefabEnum
    {
        PREFAB_WAYPOINT,
        PREFAB_MENU_PICKABLE_ITEM,

        PREFAB_TOTAL
    }

    public enum SpriteEnum
    {
        SPRITE_POTION,

        SPRITE_TOTAL
    }

    public static class ResourceAtlasClass
    {

        public static GameObject GetPrefab(PrefabEnum prefabId)
        {
            return Resources.Load<GameObject>(_PrefabList[(int)prefabId]);
        }

        public static Sprite GetSprite(SpriteEnum spriteId)
        {
            return Resources.Load<Sprite>(_SpriteList[(int)spriteId]);
        }

        public static Sprite GetSpriteFromPickableItem(GamePickableItem item)
        {
            return GetSprite(_PickableItemToSpriteId[(int)item]);
        }

        private static readonly SpriteEnum[] _PickableItemToSpriteId = new SpriteEnum[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            SpriteEnum.SPRITE_POTION
        };

        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "Prefabs/Waypoint",
            "Prefabs/PickableItemDisplay"
        };

        private static readonly string[] _SpriteList = new string[(int)SpriteEnum.SPRITE_TOTAL]
        {
            "Sprites/potion_64"
        };
    }
}
