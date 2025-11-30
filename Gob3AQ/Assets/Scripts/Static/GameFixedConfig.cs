using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.FixedConfig
{
    public static class GameFixedConfig
    {
        /* Read only fields */
        public static ReadOnlySpan<byte> GAME_VERSION => _GAME_VERSION;
        public static ReadOnlySpan<byte> LOAD_SAVE_FILE_FORMAT_VERSION => _LOAD_SAVE_FILE_FORMAT_VERSION;
        public static ReadOnlySpan<string> ROOM_TO_SCENE_NAME => _ROOM_TO_SCENE_NAME;

        /* Release data */
        public const bool PERIPH_PC = true;
        private static readonly byte[] _GAME_VERSION = { 0, 0, 1 };
        private static readonly byte[] _LOAD_SAVE_FILE_FORMAT_VERSION = { 0, 0, 1 };


        /* Time and timeouts */
        public const float SECONDS_TO_MILLISECONDS = 1000f;
        public const float KEY_REFRESH_TIME_SECONDS = 0.05f;
        public const float CHARACTER_NORMAL_SPEED = 5.0f;
        public const float MOVE_CAMERA_SPEED = 20.0f;
        public const float USER_INTERACTION_CHANGE_ANIMATION_TIME = 0.5f;
        public const ulong DOUBLE_CLICK_MS = 400;

        /* HUD */
        public const float DISTANCE_MOUSE_FURTHEST_WP = 1.5f;
        public const float MIN_CAMERA_ORTHO_SIZE = 0.5f;


        /* Graphics - Mouse */
        public const float MENU_TOP_SCREEN_HEIGHT_PERCENT = 64f/768f;
        public const float GAME_ZONE_HEIGHT_PERCENT = 1.0f - MENU_TOP_SCREEN_HEIGHT_PERCENT;
        public const float GAME_ZONE_HEIGHT_FACTOR = 1.0f / GAME_ZONE_HEIGHT_PERCENT;
        public const float GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR = 0.1f;
        public const float GAME_ZONE_CURSOR_MOVE_CAMERA_1MFACTOR = 1.0f - GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR;
        public const float GAME_ZONE_CURSOR_PERCENT_MAX_SPEED = 0.25f;
        public const float GAME_ZONE_CURSOR_PERCENT_MAX_SPEED_DIV = 1f/GAME_ZONE_CURSOR_PERCENT_MAX_SPEED;


        /* Performance */
        public const int MAX_FIXED_SPRITES_TO_LOAD = (int)GamePickableItem.ITEM_PICK_TOTAL + 16;
        public const int MAX_FIXED_NAMES_TO_LOAD = (int)GamePickableItem.ITEM_PICK_TOTAL + (int)MementoParent.MEMENTO_PARENT_TOTAL + 16;
        public const int MAX_FIXED_PHRASES_TO_LOAD = (int)Memento.MEMENTO_TOTAL + 16;
        public const int MAX_CACHED_SPRITES = 64 + (int)GamePickableItem.ITEM_PICK_TOTAL + (int)Memento.MEMENTO_TOTAL;
        public const int MAX_CACHED_PHRASES = 64;
        public const int MAX_CACHED_PREFABS = 16;
        public const int MAX_DIALOG_OPTIONS = 6;
        public const int MAX_POOLED_ITEMS = 32;
        public const int MAX_BUFFERED_EVENTS = 16;
        public const int MAX_PENDING_UNCHAINERS = 128;
        public const int MAX_UNCHAINER_CONDITIONS = 4;
        public const int MAX_RAYCASTED_ITEMS = 4;
        public const int MAX_LEVEL_WAYPOINTS = 128;
        public const int MAX_DIALOG_TALKERS = 4;
        public const int MAX_DISPLAYED_PICKED_ITEMS = 16;
        public const int MAX_DISPLAYED_HOR_PICKED_ITEMS = 4;
        public const int MAX_SUBSCRIBED_EVENTS_PER_ITEM = 8;


        /* File routes */
        public static readonly string LOADSAVE_FILEPATH = Application.persistentDataPath + "/savedat.dat";


        /* Scenes */
        public const int MAX_SCENE_DOORS = 4;
        public const string ROOM_MAINMENU = "Boot";
        public const string ROOM_BASE = "SCENE_BASE";
        private static readonly string[] _ROOM_TO_SCENE_NAME =
        {
            "SCENE_ROOM1",
            "SCENE_ROOM1_KITCHEN",
            "SCENE_ROOM1_GARDEN",
            "SCENE_ROOM1_CAR"
        };
        
    }
}
