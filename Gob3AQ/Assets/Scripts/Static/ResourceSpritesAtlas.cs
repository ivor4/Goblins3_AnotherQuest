using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceSpritesAtlas
{
    public static class ResourceSpritesAtlasClass
    {
        public static ref readonly SpriteConfig GetSpriteConfig(GameSprite sprite)
        {
            if ((uint)sprite < (uint)GameSprite.SPRITE_TOTAL)
            {
                return ref _SpriteConfig[(int)sprite];
            }
            else
            {
                Debug.LogError($"Trying to get SpriteConfig for invalid sprite {sprite}");
                return ref SpriteConfig.EMPTY;
            }
        }


        private static readonly SpriteConfig[] _SpriteConfig = new SpriteConfig[(int)GameSprite.SPRITE_TOTAL]
        {
            /* > ATG 1 START < */
            new("SPRITE_CURSOR_NORMAL"), /* SPRITE_CURSOR_NORMAL */ 
            new("SPRITE_INVENTORY"), /* SPRITE_INVENTORY */ 
            new("SPRITE_ITEM_POTION"), /* SPRITE_POTION_RED */ 
            new("SPRITE_ITEM_POTION_BLUE"), /* SPRITE_POTION_BLUE */ 
            new("SPRITE_ITEM_FOUNTAIN"), /* SPRITE_FOUNTAIN */ 
            new("SPRITE_ITEM_FOUNTAIN_FULL"), /* SPRITE_FOUNTAIN_FULL */ 
            new("SPRITE_NPC_MILITO"), /* SPRITE_NPC_MILITO */ 
            new("BACKGROUND_ROOM_FIRST"), /* BACKGROUND_ROOM_FIRST */ 
            new("SPRITE_LAST"), /* SPRITE_LAST */ 
            /* > ATG 1 END < */
        };


    }
}