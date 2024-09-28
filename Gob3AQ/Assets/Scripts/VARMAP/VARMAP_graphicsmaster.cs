using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;
using UnityEngine;

namespace MVerse.VARMAP.GraphicsMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GraphicsMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_LIFE_TOTAL = _GET_LIFE_TOTAL;
            REG_LIFE_TOTAL = _REG_LIFE_TOTAL;
            UNREG_LIFE_TOTAL = _UNREG_LIFE_TOTAL;
            GET_LIFE_ACTUAL = _GET_LIFE_ACTUAL;
            REG_LIFE_ACTUAL = _REG_LIFE_ACTUAL;
            UNREG_LIFE_ACTUAL = _UNREG_LIFE_ACTUAL;
            GET_ELEM_SELECTED_CHARMS = _GET_ELEM_SELECTED_CHARMS;
            GET_SIZE_SELECTED_CHARMS = _GET_SIZE_SELECTED_CHARMS;
            GET_ARRAY_SELECTED_CHARMS = _GET_ARRAY_SELECTED_CHARMS;
            GET_OTHER_WORLD = _GET_OTHER_WORLD;
            REG_OTHER_WORLD = _REG_OTHER_WORLD;
            UNREG_OTHER_WORLD = _UNREG_OTHER_WORLD;
            GET_OTHER_WORLD_TRANSITION_ACTIVE = _GET_OTHER_WORLD_TRANSITION_ACTIVE;
            GET_OTHER_WORLD_TRANSITION_PROGRESS = _GET_OTHER_WORLD_TRANSITION_PROGRESS;
            GET_OTHER_WORLD_MODE = _GET_OTHER_WORLD_MODE;
            REG_OTHER_WORLD_MODE = _REG_OTHER_WORLD_MODE;
            UNREG_OTHER_WORLD_MODE = _UNREG_OTHER_WORLD_MODE;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<byte> GET_LIFE_TOTAL;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> REG_LIFE_TOTAL;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> UNREG_LIFE_TOTAL;
        public static GetVARMAPValueDelegate<byte> GET_LIFE_ACTUAL;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> REG_LIFE_ACTUAL;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> UNREG_LIFE_ACTUAL;
        public static GetVARMAPArrayElemValueDelegate<byte> GET_ELEM_SELECTED_CHARMS;
        public static GetVARMAPArraySizeDelegate GET_SIZE_SELECTED_CHARMS;
        public static GetVARMAPArrayDelegate<byte> GET_ARRAY_SELECTED_CHARMS;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<bool> REG_OTHER_WORLD;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<bool> UNREG_OTHER_WORLD;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD_TRANSITION_ACTIVE;
        public static GetVARMAPValueDelegate<float> GET_OTHER_WORLD_TRANSITION_PROGRESS;
        public static GetVARMAPValueDelegate<OtherWorldMode> GET_OTHER_WORLD_MODE;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> REG_OTHER_WORLD_MODE;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> UNREG_OTHER_WORLD_MODE;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
