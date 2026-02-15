using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.GraphicsMaster;
using Gob3AQ.GameMenu;


namespace Gob3AQ.VARMAP.GameMaster
{

    public sealed class VARMAP_GameMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_SHADOW_ELAPSED_TIME_MS = _GET_SHADOW_ELAPSED_TIME_MS;
            SET_ELAPSED_TIME_MS = _SET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_SHADOW_ACTUAL_ROOM = _GET_SHADOW_ACTUAL_ROOM;
            SET_ACTUAL_ROOM = _SET_ACTUAL_ROOM;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            GET_SHADOW_GAMESTATUS = _GET_SHADOW_GAMESTATUS;
            SET_GAMESTATUS = _SET_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_EVENTS_BEING_PROCESSED = _GET_EVENTS_BEING_PROCESSED;
            GET_DAY_MOMENT = _GET_DAY_MOMENT;
            GET_SHADOW_DAY_MOMENT = _GET_SHADOW_DAY_MOMENT;
            SET_DAY_MOMENT = _SET_DAY_MOMENT;
            GET_LAST_VARMAP_VAL = _GET_LAST_VARMAP_VAL;
            GET_SHADOW_LAST_VARMAP_VAL = _GET_SHADOW_LAST_VARMAP_VAL;
            SET_LAST_VARMAP_VAL = _SET_LAST_VARMAP_VAL;
            START_GAME = _START_GAME;
            SAVE_GAME = _SAVE_GAME;
            LOAD_GAME = _LOAD_GAME;
            LOAD_ROOM = _LOAD_ROOM;
            EXIT_GAME = _EXIT_GAME;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            COMMIT_EVENT = _COMMIT_EVENT;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            CHANGE_DAY_MOMENT = _CHANGE_DAY_MOMENT;
            LOAD_ADDITIONAL_RESOURCES = _LOAD_ADDITIONAL_RESOURCES;
            LAST_SERVICE = _LAST_SERVICE;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<ulong> GET_SHADOW_ELAPSED_TIME_MS;
        public static SetVARMAPValueDelegate<ulong> SET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<Room> GET_SHADOW_ACTUAL_ROOM;
        public static SetVARMAPValueDelegate<Room> SET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static GetVARMAPValueDelegate<Game_Status> GET_SHADOW_GAMESTATUS;
        public static SetVARMAPValueDelegate<Game_Status> SET_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<bool> GET_EVENTS_BEING_PROCESSED;
        public static GetVARMAPValueDelegate<MomentType> GET_DAY_MOMENT;
        public static GetVARMAPValueDelegate<MomentType> GET_SHADOW_DAY_MOMENT;
        public static SetVARMAPValueDelegate<MomentType> SET_DAY_MOMENT;
        public static GetVARMAPValueDelegate<bool> GET_LAST_VARMAP_VAL;
        public static GetVARMAPValueDelegate<bool> GET_SHADOW_LAST_VARMAP_VAL;
        public static SetVARMAPValueDelegate<bool> SET_LAST_VARMAP_VAL;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// Starts game from main menu
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.StartGameService"/> </para> 
        /// </summary>
        public static START_GAME_DELEGATE START_GAME;
        /// <summary> 
        /// Saves game at any moment
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.SaveGameService"/> </para> 
        /// </summary>
        public static SAVE_GAME_DELEGATE SAVE_GAME;
        /// <summary> 
        /// Loads game from any moment
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadGameService"/> </para> 
        /// </summary>
        public static LOAD_GAME_DELEGATE LOAD_GAME;
        /// <summary> 
        /// Loads a room (for example when crossing a door)
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadRoomService"/> </para> 
        /// </summary>
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
        /// <summary> 
        /// Exits games to OS
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ExitGameService"/> </para> 
        /// </summary>
        public static EXIT_GAME_DELEGATE EXIT_GAME;
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        public static LODING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMaster, LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Asks Game Master to set game mode
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeGameModeService"/> </para> 
        /// </summary>
        public static CHANGE_GAME_MODE_DELEGATE CHANGE_GAME_MODE;
        /// <summary> 
        /// Requests to change moment of day to a given one
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeDayMomentService"/> </para> 
        /// </summary>
        public static CHANGE_DAY_MOMENT_DELEGATE CHANGE_DAY_MOMENT;
        /// <summary> 
        /// Loads/Unloads a set of names and Phrases
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadAdditionalResourcesService"/> </para> 
        /// </summary>
        public static LOAD_ADDITIONAL_RESOURCES LOAD_ADDITIONAL_RESOURCES;
        /// <summary> 
        /// Last service
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors:  </para> 
        /// <para> Method: <see cref="GameMasterClass.ExitGameService"/> </para> 
        /// </summary>
        public static EXIT_GAME_DELEGATE LAST_SERVICE;
        /* > ATG 3 END */
    }
}
