using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.GameMenu;
using Gob3AQ.InputMaster;


namespace Gob3AQ.VARMAP.LevelMaster
{
    public sealed class VARMAP_LevelMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_ELEM_PLAYER_ACTUAL_WAYPOINT = _GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
            GET_SHADOW_ELEM_PLAYER_ACTUAL_WAYPOINT = _GET_SHADOW_ELEM_PLAYER_ACTUAL_WAYPOINT;
            SET_ELEM_PLAYER_ACTUAL_WAYPOINT = _SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
            GET_SIZE_PLAYER_ACTUAL_WAYPOINT = _GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
            GET_ARRAY_PLAYER_ACTUAL_WAYPOINT = _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            GET_SHADOW_ARRAY_PLAYER_ACTUAL_WAYPOINT = _GET_SHADOW_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            SET_ARRAY_PLAYER_ACTUAL_WAYPOINT = _SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            GET_ITEM_HOVER = _GET_ITEM_HOVER;
            GET_SHADOW_ITEM_HOVER = _GET_SHADOW_ITEM_HOVER;
            SET_ITEM_HOVER = _SET_ITEM_HOVER;
            SAVE_GAME = _SAVE_GAME;
            LOAD_ROOM = _LOAD_ROOM;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            ITEM_REGISTER = _ITEM_REGISTER;
            ITEM_OBTAIN_PICKABLE = _ITEM_OBTAIN_PICKABLE;
            MONO_REGISTER = _MONO_REGISTER;
            WP_REGISTER = _WP_REGISTER;
            DOOR_REGISTER = _DOOR_REGISTER;
            PLAYER_WAYPOINT_UPDATE = _PLAYER_WAYPOINT_UPDATE;
            GAME_ELEMENT_OVER = _GAME_ELEMENT_OVER;
            SELECT_PLAYER = _SELECT_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            INTERACT_PLAYER = _INTERACT_PLAYER;
            PLAYER_REACHED_WAYPOINT = _PLAYER_REACHED_WAYPOINT;
            USE_ITEM = _USE_ITEM;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            LOCK_PLAYER = _LOCK_PLAYER;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            SHOW_DIALOGUE = _SHOW_DIALOGUE;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayElemValueDelegate<int> GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArrayElemValueDelegate<int> GET_SHADOW_ELEM_PLAYER_ACTUAL_WAYPOINT;
        public static SetVARMAPArrayElemValueDelegate<int> SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArrayDelegate<int> GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArrayDelegate<int> GET_SHADOW_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static SetVARMAPArrayDelegate<int> SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static GetVARMAPValueDelegate<GameItem> GET_ITEM_HOVER;
        public static GetVARMAPValueDelegate<GameItem> GET_SHADOW_ITEM_HOVER;
        public static SetVARMAPValueDelegate<GameItem> SET_ITEM_HOVER;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// Saves game at any moment
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.SaveGameService"/> </para> 
        /// </summary>
        public static SAVE_GAME_DELEGATE SAVE_GAME;
        /// <summary> 
        /// Loads a room (for example when crossing a door)
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadRoomService"/> </para> 
        /// </summary>
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
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
        /// Registers an item in system
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ItemRegisterService"/> </para> 
        /// </summary>
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        /// <summary> 
        /// Removes an item from level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ItemObtainPickableService"/> </para> 
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_DELEGATE ITEM_OBTAIN_PICKABLE;
        /// <summary> 
        /// Registers a player in scene
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.MonoRegisterService"/> </para> 
        /// </summary>
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        /// <summary> 
        /// Registers a Waypoint in level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors:  </para> 
        /// <para> Method: <see cref="LevelMasterClass.WPRegisterService"/> </para> 
        /// </summary>
        public static WP_REGISTER_DELEGATE WP_REGISTER;
        /// <summary> 
        /// Registers a door in level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors:  </para> 
        /// <para> Method: <see cref="LevelMasterClass.DoorRegisterService"/> </para> 
        /// </summary>
        public static DOOR_REGISTER_DELEGATE DOOR_REGISTER;
        /// <summary> 
        /// Updates actual player waypoint when crossing or stopping on it
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.PlayerWaypointUpdateService"/> </para> 
        /// </summary>
        public static PLAYER_WAYPOINT_UPDATE_DELEGATE PLAYER_WAYPOINT_UPDATE;
        /// <summary> 
        /// Any of Game Elements (Player or Item or Door) will call with essential info
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GameElementOverService"/> </para> 
        /// </summary>
        public static GAME_ELEMENT_OVER_DELEGATE GAME_ELEMENT_OVER;
        /// <summary> 
        /// Selects player
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.SelectPlayerService"/> </para> 
        /// </summary>
        public static SELECT_PLAYER_DELEGATE SELECT_PLAYER;
        /// <summary> 
        /// Gets a list of actual players
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: GraphicsMaster, PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetPlayerListService"/> </para> 
        /// </summary>
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetNearestWPService"/> </para> 
        /// </summary>
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        /// <summary> 
        /// Tells if an event is occurred
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        /// <summary> 
        /// Same as IsEventOccurredService but for array of combos with possible negation
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventCombiOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_COMBI_OCCURRED_DELEGATE IS_EVENT_COMBI_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Makes player interact with usage data
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.InteractPlayerService"/> </para> 
        /// </summary>
        public static INTERACT_PLAYER_DELEGATE INTERACT_PLAYER;
        /// <summary> 
        /// Tells LevelMaster that player reached Waypoint to start action in case
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.PlayerReachedWaypointService"/> </para> 
        /// </summary>
        public static PLAYER_REACHED_WAYPOINT PLAYER_REACHED_WAYPOINT;
        /// <summary> 
        /// Uses an item with something
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.UseItemService"/> </para> 
        /// </summary>
        public static USE_ITEM_DELEGATE USE_ITEM;
        /// <summary> 
        /// Tells if item is taken from scene
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsItemTakenFromSceneService"/> </para> 
        /// </summary>
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        /// <summary> 
        /// Cancels selected item
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameMaster, LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.CancelPickableItemService"/> </para> 
        /// </summary>
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.EventSubscriptionService"/> </para> 
        /// </summary>
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /// <summary> 
        /// Locks player so it cannot act until an action over it has been done (or removes lock)
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.LockPlayerService"/> </para> 
        /// </summary>
        public static LOCK_PLAYER_DELEGATE LOCK_PLAYER;
        /// <summary> 
        /// Asks Game Master to set game mode
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeGameModeService"/> </para> 
        /// </summary>
        public static CHANGE_GAME_MODE_DELEGATE CHANGE_GAME_MODE;
        /// <summary> 
        /// Second part of start dialogue. Tells Game Menu to prepare menu elements
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.ShowDialogueService"/> </para> 
        /// </summary>
        public static SHOW_DIALOGUE_DELEGATE SHOW_DIALOGUE;
        /* > ATG 3 END */
    }
}
