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
            new("Sprites/potion_64"), /* SPRITE_POTION_RED */ 
            new("Sprites/potionblue_64"), /* SPRITE_POTION_BLUE */ 
            new("Sprites/spr_fountain_256"), /* SPRITE_FOUNTAIN */ 
            new("Sprites/spr_fountain_full_256"), /* SPRITE_FOUNTAIN_FULL */ 
            new("Sprites/spr_milito_1"), /* SPRITE_NPC_MILITO */ 
            new(""), /* SPRITE_LAST */ 
            /* > ATG 1 END < */
        };


    }
}