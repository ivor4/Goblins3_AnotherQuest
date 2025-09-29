using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceSpritesAtlas
{
    public static class ResourceSpritesAtlasClass
    {
        public static ReadOnlySpan<SpriteConfig> SpriteConfigs => _SpriteConfig;
        public static ReadOnlySpan<GameSprite> PickableItemToSpriteAvatar => _PickableItemToSpriteAvatar;


        private static readonly SpriteConfig[] _SpriteConfig = new SpriteConfig[(int)GameSprite.SPRITE_TOTAL]
        {
            /* > ATG 1 START < */
            new("Sprites/potion_64", GameItem.ITEM_POTION, Room.ROOM_FIRST), /* SPRITE_POTION_RED */ 
            new("Sprites/potionblue_64", GameItem.ITEM_POTION_BLUE, Room.ROOM_FIRST), /* SPRITE_POTION_BLUE */ 
            new("Sprites/spr_fountain_256", GameItem.ITEM_NONE, Room.ROOM_FIRST), /* SPRITE_FOUNTAIN */ 
            new("Sprites/spr_fountain_full_256", GameItem.ITEM_NONE, Room.ROOM_FIRST), /* SPRITE_FOUNTAIN_FULL */ 
            new("", GameItem.ITEM_NONE, Room.ROOM_NONE), /* SPRITE_LAST */ 
            /* > ATG 1 END < */
        };

        private static readonly GameSprite[] _PickableItemToSpriteAvatar = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 2 START < */ 
            GameSprite.SPRITE_POTION_RED,	 /* ITEM_PICK_POTION */ 
            GameSprite.SPRITE_POTION_BLUE,	 /* ITEM_PICK_POTION_BLUE */ 
            /* > ATG 2 END < */
        };

    }
}