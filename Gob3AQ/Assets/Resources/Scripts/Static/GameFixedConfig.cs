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

        /* HUD */
        public const float DISTANCE_MOUSE_FURTHEST_WP = 1.5f;

        /* Colors */
        public static readonly Color SUN_COLOR_NORMAL = new Color(255f / 255f, 244f / 255f, 214f / 255f);
        public static readonly Color SUN_COLOR_OTHERWORLD = Color.blue;

        /* Graphics - Mouse */
        public const float MENU_TOP_SCREEN_HEIGHT_PERCENT = 0.1f;
        public const float GAME_ZONE_HEIGHT_PERCENT = 1.0f - MENU_TOP_SCREEN_HEIGHT_PERCENT;
        public const float GAME_ZONE_HEIGHT_FACTOR = 1.0f / GAME_ZONE_HEIGHT_PERCENT;

        /* Performance */
        public const int MAX_POOLED_ENEMIES = 50;
        public const int MAX_LEVEL_PLAYABLE_CHARACTERS = 3;
        public const int MAX_LEVEL_WAYPOINTS = 128;


        /* File routes */
        public static readonly string LOADSAVE_FILEPATH = Application.persistentDataPath + "/savedat.dat";


        /* Scenes */
        public const string ROOM_MAINMENU = "MainMenu";
        private static readonly string[] _ROOM_TO_SCENE_NAME =
        {
            "",
            "SampleScene",
            ""
        };

        
    }
}
