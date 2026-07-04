using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gob3AQ.Brain.LevelOptions
{
    public static class LevelOptionsClass
    {
        public static ReadOnlySpan<string> ROOM_TO_SCENE_NAME => _ROOM_TO_SCENE_NAME;
        public static IReadOnlyDictionary<Room, bool> IS_ROOM_FADE_OUT_LONG => _IS_ROOM_FADE_OUT_LONG;
        public static IReadOnlyDictionary<Room, IReadOnlyList<PrefabEnum>> CHARACTERS_TO_LOAD_PER_SCENE => _CHARACTERS_TO_LOAD_PER_SCENE;
        public static IReadOnlyDictionary<int, Tuple<string, NameType>> CHAPTER_TO_TITLE => _CHAPTER_TO_TITLE;
        public static IReadOnlyDictionary<int, Tuple<Room, int>> CHAPTER_TO_ROOM_AND_INIT_WP => _CHAPTER_TO_ROOM_AND_INIT_WP;


        private static readonly string[] _ROOM_TO_SCENE_NAME = new string[(int)Room.ROOMS_TOTAL]
        {
            "SCENE_HIVE1_ROOM_1",
            "SCENE_HIVE1_CORRIDOR_1",
            "SCENE_HIVE1_HALL_1",
            "SCENE_HIVE1_WC_1",
            "SCENE_CITY1_STREET_1",
            "SCENE_CITY1_STREET_2",
            "SCENE_PHARMACY1",
            "SCENE_MANYO1",
            "SCENE_HIVE1_BACKALLEY",
            "SCENE_CITY1_SOUTH_STREET_1",
            "SCENE_CITY1_SOUTH_STREET_2",
            "SCENE_EXTRAPERLO",
            "SCENE_EXTRAPERLO2",
            "SCENE_EXTRAPERLO3",
            "SCENE_EXTRAPERLO3_2",
            "SCENE_DREAM_1",
            "SCENE_CHAPTER_SHOW",
            ""
        };

        private static readonly Dictionary<Room, bool> _IS_ROOM_FADE_OUT_LONG = new Dictionary<Room, bool>()
        {
            {Room.CITY1_EXTRAPERLO3_2, true},
            {Room.CHAPTER_SHOW, true},
        };

        private static readonly Dictionary<Room, IReadOnlyList<PrefabEnum>> _CHARACTERS_TO_LOAD_PER_SCENE = new Dictionary<Room, IReadOnlyList<PrefabEnum>>()
        {
            {Room.CITY1_EXTRAPERLO3_2, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_SEATED }},
            {Room.DREAM_1, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
        };

        private static readonly Dictionary<int, Tuple<string, NameType>> _CHAPTER_TO_TITLE = new Dictionary<int, Tuple<string, NameType>>()
        {
            {1, new Tuple<string, NameType>("I", NameType.NAME_DENIAL) }
        };

        private static readonly Dictionary<int, Tuple<Room, int>> _CHAPTER_TO_ROOM_AND_INIT_WP = new Dictionary<int, Tuple<Room, int>>()
        {
            {1, new Tuple<Room, int>(Room.DREAM_1, 0) }
        };
    }
}



