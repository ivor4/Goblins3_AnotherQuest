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
            new("SPRITE_ATLAS_ROOM1_0[ROOM1_BACKGROUND]"), /* BACKGROUND_ROOM1 */ 
            new("SPRITE_ATLAS_ROOM1_1[SPRITE_ROOM1_VICTIM]"), /* SPRITE_ROOM1_VICTIM */ 
            new("SPRITE_ATLAS_ROOM1_1[SPRITE_ROOM1_WITNESS1]"), /* SPRITE_ROOM1_WITNESS1 */ 
            new("SPRITE_ATLAS_ROOM1_1[SPRITE_ROOM1_WITNESS2]"), /* SPRITE_ROOM1_WITNESS2 */ 
            new("SPRITE_ATLAS_ROOM1_1[SPRITE_ROOM1_WITNESS3]"), /* SPRITE_ROOM1_WITNESS3 */ 
            new("SPRITE_ATLAS_ROOM1_1[SPRITE_ROOM1_TABLE]"), /* SPRITE_ROOM1_DECO_TABLE */ 
            new("SPRITE_ATLAS_PICKABLES_0[SPRITE_MEMENTO_CHASE]"), /* SPRITE_MEMENTO_CHASE */ 
            new("SPRITE_ATLAS_UI_0[SPRITE_CURSOR_DOOR]"), /* SPRITE_UI_CURSOR_DOOR */ 
            new("SPRITE_ATLAS_ROOM1_2[BACKGROUND_ROOM1_KITCHEN]"), /* BACKGROUND_ROOM1_KITCHEN */ 
            new("SPRITE_ATLAS_ROOM1_2[SPRITE_ROOM1_KITCHEN_BASIN]"), /* SPRITE_ROOM1_KITCHEN_BASIN */ 
            new("SPRITE_ATLAS_ROOM1_2[SPRITE_ROOM1_SPOON]"), /* SPRITE_ROOM1_SPOON */ 
            new("SPRITE_ATLAS_ROOM1_2[SPRITE_ROOM1_DRAWER]"), /* SPRITE_ROOM1_DRAWER */ 
            new("SPRITE_ATLAS_ROOM1_2[SPRITE_ROOM1_WINDOW_CLOSED]"), /* SPRITE_ROOM1_WINDOW_CLOSED */ 
            new("SPRITE_ATLAS_ROOM1_2[SPRITE_ROOM1_JAM]"), /* SPRITE_ROOM1_JAM */ 
            new("SPRITE_ATLAS_PICKABLES_0[SPRITE_PICKABLE_SPOON]"), /* SPRITE_PICKABLE_SPOON */ 
            new("SPRITE_LAST"), /* SPRITE_LAST */ 
            /* > ATG 1 END < */
        };


    }
}