using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.LevelMaster
{
    public abstract class VARMAP_LevelMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_ELEM_ITEMS_COLLECTED = _GET_ELEM_ITEMS_COLLECTED;
            GET_SIZE_ITEMS_COLLECTED = _GET_SIZE_ITEMS_COLLECTED;
            GET_ARRAY_ITEMS_COLLECTED = _GET_ARRAY_ITEMS_COLLECTED;
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            LOAD_ROOM = _LOAD_ROOM;
            LOADING_COMPLETED = _LOADING_COMPLETED;
            FREEZE_PLAY = _FREEZE_PLAY;
            NPC_REGISTER = _NPC_REGISTER;
            MONO_REGISTER = _MONO_REGISTER;
            MOVE_PLAYER = _MOVE_PLAYER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_ITEMS_COLLECTED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_ITEMS_COLLECTED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_ITEMS_COLLECTED;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
        public static LODING_COMPLETED_DELEGATE LOADING_COMPLETED;
        public static FREEZE_PLAY_DELEGATE FREEZE_PLAY;
        public static NPC_REGISTER_SERVICE NPC_REGISTER;
        public static MONO_REGISTER_SERVICE MONO_REGISTER;
        public static MOVE_PLAYER_SERVICE MOVE_PLAYER;
        /* > ATG 3 END */
    }
}
