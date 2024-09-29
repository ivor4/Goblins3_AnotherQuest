using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;


namespace Gob3AQ.VARMAP.GameMaster
{

    public abstract class VARMAP_GameMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            SET_ELAPSED_TIME_MS = _SET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            SET_ACTUAL_ROOM = _SET_ACTUAL_ROOM;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            SET_GAMESTATUS = _SET_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            START_GAME = _START_GAME;
            LOAD_ROOM = _LOAD_ROOM;
            EXIT_GAME = _EXIT_GAME;
            LOADING_COMPLETED = _LOADING_COMPLETED;
            FREEZE_PLAY = _FREEZE_PLAY;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static SetVARMAPValueDelegate<ulong> SET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static SetVARMAPValueDelegate<Room> SET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static SetVARMAPValueDelegate<Game_Status> SET_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static START_GAME_DELEGATE START_GAME;
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
        public static EXIT_GAME_DELEGATE EXIT_GAME;
        public static LODING_COMPLETED_DELEGATE LOADING_COMPLETED;
        public static FREEZE_PLAY_DELEGATE FREEZE_PLAY;
        /* > ATG 3 END */
    }
}
