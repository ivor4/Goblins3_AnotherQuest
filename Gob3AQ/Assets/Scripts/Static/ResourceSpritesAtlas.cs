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
            new("SPRITE_ATLAS_UI_0[SPRITE_CURSOR_NORMAL]"), /* SPRITE_CURSOR_NORMAL */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_CURSOR_USING]"), /* SPRITE_CURSOR_USING */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_CURSOR_DRAG]"), /* SPRITE_CURSOR_DRAG */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_INVENTORY]"), /* SPRITE_INVENTORY */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_ACTION_TAKE]"), /* SPRITE_UI_TAKE */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_ACTION_TALK]"), /* SPRITE_UI_TALK */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_ACTION_OBSERVE]"), /* SPRITE_UI_OBSERVE */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_MOUSE_MOVE]"), /* SPRITE_UI_MOUSE_MOVE */ 
            new("SPRITE_ATLAS_PICKABLES_0[SPRITE_ITEM_POTION]"), /* SPRITE_POTION_RED */ 
            new("SPRITE_ATLAS_PICKABLES_0[SPRITE_ITEM_POTION_BLUE]"), /* SPRITE_POTION_BLUE */ 
            new("SPRITE_ATLAS_ROOM_FIRST_0[SPRITE_ITEM_FOUNTAIN]"), /* SPRITE_FOUNTAIN */ 
            new("SPRITE_ATLAS_ROOM_FIRST_0[SPRITE_ITEM_FOUNTAIN_FULL]"), /* SPRITE_FOUNTAIN_FULL */ 
            new("SPRITE_ATLAS_ROOM_FIRST_0[SPRITE_ITEM_NPC_MILITO]"), /* SPRITE_NPC_MILITO */ 
            new("SPRITE_ATLAS_ROOM_FIRST_0[SPRITE_BACKGROUND_ROOM_FIRST]"), /* BACKGROUND_ROOM_FIRST */ 
            new("SPRITE_LAST"), /* SPRITE_LAST */ 
            /* > ATG 1 END < */
        };


    }
}