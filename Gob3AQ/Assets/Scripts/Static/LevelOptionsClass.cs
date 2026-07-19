using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.Brain.LevelOptions
{
    public static class LevelOptionsClass
    {
        public static ReadOnlySpan<string> ROOM_TO_SCENE_NAME => _ROOM_TO_SCENE_NAME;
        public static IReadOnlyDictionary<Room, bool> IS_ROOM_FADE_OUT_LONG => _IS_ROOM_FADE_OUT_LONG;
        public static IReadOnlyDictionary<Room, IReadOnlyList<PrefabEnum>> CHARACTERS_TO_LOAD_PER_SCENE => _CHARACTERS_TO_LOAD_PER_SCENE;
        public static IReadOnlyDictionary<int, Tuple<string, NameType>> CHAPTER_TO_TITLE => _CHAPTER_TO_TITLE;
        public static IReadOnlyDictionary<int, Tuple<Room, int>> CHAPTER_TO_ROOM_AND_INIT_WP => _CHAPTER_TO_ROOM_AND_INIT_WP;

        public static ReadOnlySpan<InitialWalkInfo> GetInitialWalkInfo(Room room)
        {
            InitialWalkInfo[] retVal = _RoomInitialWaypointWalk.GetValueOrDefault(room, _DefaultInitialWalkInfo);

            return retVal;
        }


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
            "SCENE_CHAPTER_SHOW",
            "SCENE_DREAM_1_CORRIDOR",
            "SCENE_DREAM_1_FRAMEWORK",
            "SCENE_DREAM_1_KITCHEN",
            "SCENE_DREAM_1_BEDROOM",
            "SCENE_DREAM_1_BEDROOM_NIGHT",
            "SCENE_DREAM_1_CORRIDOR_NIGHT",
            "SCENE_DREAM_1_KITCHEN_NIGHT",
            ""
        };

        private static readonly Dictionary<Room, bool> _IS_ROOM_FADE_OUT_LONG = new Dictionary<Room, bool>()
        {
            {Room.CITY1_EXTRAPERLO3_2, true},
            {Room.CHAPTER_SHOW, true},
            {Room.DREAM_1_BEDROOM_NIGHT, true},
        };

        private static readonly Dictionary<Room, IReadOnlyList<PrefabEnum>> _CHARACTERS_TO_LOAD_PER_SCENE = new Dictionary<Room, IReadOnlyList<PrefabEnum>>()
        {
            {Room.CITY1_EXTRAPERLO3_2, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_SEATED }},
            {Room.DREAM_1_CORRIDOR, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
            {Room.DREAM_1_FRAMEWORK, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
            {Room.DREAM_1_KITCHEN, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
            {Room.DREAM_1_BEDROOM, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
            {Room.DREAM_1_BEDROOM_NIGHT, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_BED }},
            {Room.DREAM_1_CORRIDOR_NIGHT, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
            {Room.DREAM_1_KITCHEN_NIGHT, new List<PrefabEnum>(){ PrefabEnum.PREFAB_MAINCHARACTER_DREAM }},
        };

        private static readonly InitialWalkInfo[] _DefaultInitialWalkInfo = new InitialWalkInfo[1]
        {
            new(0,0)
        };

        private static readonly Dictionary<Room, InitialWalkInfo[]> _RoomInitialWaypointWalk = new Dictionary<Room, InitialWalkInfo[]>
        {
            {Room.CITY1_STREET_1, new InitialWalkInfo[]{new(1,5), new(7,8), new(9,6), new(0,4) } }, /* CITY1_STREET_1 */
            {Room.HIVE1_BACKALLEY, new InitialWalkInfo[]{new(2,4) } }, /* HIVE1_BACKALLEY */
            {Room.CITY1_SOUTH_STREET_1, new InitialWalkInfo[]{new(6,0), new(3,5) } }, /* CITY1_SOUTH_STREET_1 */
            {Room.CITY1_SOUTH_STREET_2, new InitialWalkInfo[]{new(3,5), new(8,1) } }, /* CITY1_SOUTH_STREET_2 */
            {Room.CITY1_EXTRAPERLO2, new InitialWalkInfo[]{new(1,3) } }, /* CITY1_EXTRAPERLO2 */
        };

        private static readonly Dictionary<int, Tuple<string, NameType>> _CHAPTER_TO_TITLE = new Dictionary<int, Tuple<string, NameType>>()
        {
            {1, new Tuple<string, NameType>("I", NameType.NAME_DENIAL) }
        };

        private static readonly Dictionary<int, Tuple<Room, int>> _CHAPTER_TO_ROOM_AND_INIT_WP = new Dictionary<int, Tuple<Room, int>>()
        {
            {1, new Tuple<Room, int>(Room.DREAM_1_CORRIDOR, 0) }
        };
    }
}



