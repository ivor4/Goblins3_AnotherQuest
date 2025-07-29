using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Items;
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
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_ELEM_PLAYER_ACTUAL_WAYPOINT = _GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
            SET_ELEM_PLAYER_ACTUAL_WAYPOINT = _SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
            GET_SIZE_PLAYER_ACTUAL_WAYPOINT = _GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
            GET_ARRAY_PLAYER_ACTUAL_WAYPOINT = _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            SET_ARRAY_PLAYER_ACTUAL_WAYPOINT = _SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_ITEM_MENU_ACTIVE = _GET_ITEM_MENU_ACTIVE;
            SET_ITEM_MENU_ACTIVE = _SET_ITEM_MENU_ACTIVE;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            GET_ELEM_PLAYER_TRANSACTION = _GET_ELEM_PLAYER_TRANSACTION;
            GET_SIZE_PLAYER_TRANSACTION = _GET_SIZE_PLAYER_TRANSACTION;
            GET_ARRAY_PLAYER_TRANSACTION = _GET_ARRAY_PLAYER_TRANSACTION;
            SAVE_GAME = _SAVE_GAME;
            LOAD_ROOM = _LOAD_ROOM;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            LOADING_COMPLETED = _LOADING_COMPLETED;
            FREEZE_PLAY = _FREEZE_PLAY;
            NPC_REGISTER = _NPC_REGISTER;
            ITEM_REGISTER = _ITEM_REGISTER;
            ITEM_OBTAIN_PICKABLE = _ITEM_OBTAIN_PICKABLE;
            MONO_REGISTER = _MONO_REGISTER;
            WP_REGISTER = _WP_REGISTER;
            DOOR_REGISTER = _DOOR_REGISTER;
            PLAYER_WAYPOINT_UPDATE = _PLAYER_WAYPOINT_UPDATE;
            SELECT_PLAYER = _SELECT_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            GET_NPC_LIST = _GET_NPC_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            INTERACT_PLAYER = _INTERACT_PLAYER;
            GET_SCENARIO_ITEM_LIST = _GET_SCENARIO_ITEM_LIST;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            CROSS_DOOR = _CROSS_DOOR;
            LOCK_PLAYER = _LOCK_PLAYER;
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
        public static SetVARMAPArrayElemValueDelegate<int> SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArrayDelegate<int> GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static SetVARMAPArrayDelegate<int> SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<bool> GET_ITEM_MENU_ACTIVE;
        public static SetVARMAPValueDelegate<bool> SET_ITEM_MENU_ACTIVE;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_PLAYER_TRANSACTION;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PLAYER_TRANSACTION;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_PLAYER_TRANSACTION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// Saves game at any moment
        /// </summary>
        public static SAVE_GAME_DELEGATE SAVE_GAME;
        /// <summary> 
        /// Loads a room (for example when crossing a door)
        /// </summary>
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
        /// <summary> 
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// </summary>
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// </summary>
        public static LODING_COMPLETED_DELEGATE LOADING_COMPLETED;
        /// <summary> 
        /// This service is called to pause game or enter cinematic
        /// </summary>
        public static FREEZE_PLAY_DELEGATE FREEZE_PLAY;
        /// <summary> 
        /// Registers an NPC in system
        /// </summary>
        public static NPC_REGISTER_DELEGATE NPC_REGISTER;
        /// <summary> 
        /// Registers an item in system
        /// </summary>
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        /// <summary> 
        /// Removes an item from level
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_DELEGATE ITEM_OBTAIN_PICKABLE;
        /// <summary> 
        /// Registers a player in scene
        /// </summary>
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        /// <summary> 
        /// Registers a Waypoint in level
        /// </summary>
        public static WP_REGISTER_DELEGATE WP_REGISTER;
        /// <summary> 
        /// Registers a door in level
        /// </summary>
        public static DOOR_REGISTER_DELEGATE DOOR_REGISTER;
        /// <summary> 
        /// Updates actual player waypoint when crossing or stopping on it
        /// </summary>
        public static PLAYER_WAYPOINT_UPDATE_DELEGATE PLAYER_WAYPOINT_UPDATE;
        /// <summary> 
        /// Selects player
        /// </summary>
        public static SELECT_PLAYER_DELEGATE SELECT_PLAYER;
        /// <summary> 
        /// Gets a list of actual players
        /// </summary>
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        /// <summary> 
        /// Gets a list of actual NPCs 
        /// </summary>
        public static GET_NPC_LIST_DELEGATE GET_NPC_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// </summary>
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        /// <summary> 
        /// Tells if an event is occurred
        /// </summary>
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Tells if item is taken from scene
        /// </summary>
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        /// <summary> 
        /// Interacts player with an item
        /// </summary>
        public static INTERACT_PLAYER_DELEGATE INTERACT_PLAYER;
        /// <summary> 
        /// Gets scenario item list
        /// </summary>
        public static GET_SCENARIO_ITEM_LIST_DELEGATE GET_SCENARIO_ITEM_LIST;
        /// <summary> 
        /// Cancels selected item
        /// </summary>
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// </summary>
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /// <summary> 
        /// Trigger actions when crossing a door
        /// </summary>
        public static CROSS_DOOR_DELEGATE CROSS_DOOR;
        /// <summary> 
        /// Locks player so it cannot act until an action over it has been done (or removes lock)
        /// </summary>
        public static LOCK_PLAYER_DELEGATE LOCK_PLAYER;
        /* > ATG 3 END */
    }
}
