using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVerse.FixedConfig
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
        public const float KEY_COMBO_TIMEOUT_SECONDS = 0.75f;
        public const float KEY_DOUBLE_PUSH_MAX_SECONDS = 0.5f;
        public const float OTHER_WORLD_TRANSITION_TIME_SECONDS = 2f;
        public const float CLIMB_WALL_TIMEOUT = 0.75f;
        public const float RECEIVE_DAMAGE_INVINCIBLE_TIMEOUT = 1.5f;
        public const float OBSERVE_OTHER_WORLD_TIMEOUT = 10f;

        /* Physics */
        public const float COLLISION_STAND_MIN_NORMAL_Y = 0.15f;
        public const float CLIMB_WALL_MIN_NORMAL_X = 0.95f;
        public const float PLAYER_NORMAL_MOVEMENT_SPEED = 6.0f;
        public const float PLAYER_JUMP_SPEED_REDUCTION_SHORT_PRESS = 1f / 3f;
        public static readonly Vector3 PLAYER_NORMAL_MOVE_LEFT_VECTOR = Vector3.left * PLAYER_NORMAL_MOVEMENT_SPEED;
        public static readonly Vector3 PLAYER_NORMAL_MOVE_RIGHT_VECTOR = Vector3.right * PLAYER_NORMAL_MOVEMENT_SPEED;

        /* Colors */
        public static readonly Color SUN_COLOR_NORMAL = new Color(255f / 255f, 244f / 255f, 214f / 255f);
        public static readonly Color SUN_COLOR_OTHERWORLD = Color.blue;

        /* Graphics */
        public static readonly float HEART_PERCENT_OF_SCREEN_WIDTH = 1f / 100f;

        /* Performance */
        public const int MAX_POOLED_ENEMIES = 50;

        /* Power ups */
        public const int MAX_POSSIBLE_LIFE = 6;


        /* Keys */
        public const int COMBO_MAX_KEYS = 4;    /* Needs to be a power of 2 */
        public const int COMBO_MAX_MASK = COMBO_MAX_KEYS - 1;

        /* File routes */
        public static readonly string LOADSAVE_FILEPATH = Application.persistentDataPath + "/savedat.dat";


        /* Scenes */
        public const string ROOM_MAINMENU = "MainMenu";
        private static readonly string[] _ROOM_TO_SCENE_NAME =
        {
            "",
            "Scene1",
            "SceneSpaceMama",
            ""
        };

        
    }
}
