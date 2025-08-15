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
        public const float MILLISECONDS_TO_SECONDS = 1000f;
        public const float KEY_REFRESH_TIME_SECONDS = 0.05f;
        public const float CHARACTER_NORMAL_SPEED = 5.0f;
        public const float MOVE_CAMERA_SPEED = 6.0f;

        /* HUD */
        public const float DISTANCE_MOUSE_FURTHEST_WP = 1.5f;


        /* Graphics - Mouse */
        public const float MENU_TOP_SCREEN_HEIGHT_PERCENT = 0.05f;
        public const float GAME_ZONE_HEIGHT_PERCENT = 1.0f - MENU_TOP_SCREEN_HEIGHT_PERCENT;
        public const float GAME_ZONE_HEIGHT_FACTOR = 1.0f / GAME_ZONE_HEIGHT_PERCENT;
        public const float GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR = 0.1f;
        public const float GAME_ZONE_CURSOR_MOVE_CAMERA_1MFACTOR = 1.0f - GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR;
        

        /* Performance */
        public const int MAX_CACHED_PHRASES = 64;
        public const int MAX_POOLED_ENEMIES = 50;
        public const int MAX_LEVEL_PLAYABLE_CHARACTERS = 3;
        public const int MAX_BUFFERED_EVENTS = 16;
        public const int MAX_LEVEL_WAYPOINTS = 128;
        public const int MAX_PICKED_ITEMS = 64;
        public const int MAX_DISPLAYED_PICKED_ITEMS = 16;
        public const int MAX_DISPLAYED_HOR_PICKED_ITEMS = 6;


        /* File routes */
        public static readonly string LOADSAVE_FILEPATH = Application.persistentDataPath + "/savedat.dat";


        /* Scenes */
        public const int MAX_SCENE_DOORS = 2;
        public const string ROOM_MAINMENU = "Boot";
        private static readonly string[] _ROOM_TO_SCENE_NAME =
        {
            "SampleScene",
            "House1"
        };

        
    }
}
