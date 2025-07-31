using UnityEngine;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Brain.ItemsInteraction;
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
        SPRITE_NONE,

        SPRITE_POTION,
        SPRITE_POTIONBLUE,
        SPRITE_FOUNTAIN_DRY,
        SPRITE_FOUNTAIN_FULL,

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

        public static Sprite GetAvatarSpriteFromPickableItem(GamePickableItem item)
        {
            return GetSprite(_PickableItemToSpriteId[(int)item]);
        }

        public static Sprite GetPickableAvatarSpriteFromItem(GameItem item)
        {
            return GetSprite(_PickableItemToSpriteId[(int)ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)item]]);
        }

        private static readonly SpriteEnum[] _PickableItemToSpriteId = new SpriteEnum[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            SpriteEnum.SPRITE_POTION,     /* PICKABLE_POTION */
            SpriteEnum.SPRITE_POTIONBLUE    /* PICKABLE_POTION_BLUE */
        };

        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "Prefabs/Waypoint",
            "Prefabs/PickableItemDisplay"
        };

        private static readonly string[] _SpriteList = new string[(int)SpriteEnum.SPRITE_TOTAL]
        {
            "",
            "Sprites/potion_64",
            "Sprites/potionblue_64",
            "Sprites/spr_fountain_256",
            "Sprites/spr_fountain_full_256",
        };
    }
}
