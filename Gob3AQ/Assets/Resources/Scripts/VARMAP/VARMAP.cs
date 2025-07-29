using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.VARMAP.Types.Items;
using Gob3AQ.VARMAP.Variable;
using UnityEngine;

namespace Gob3AQ.VARMAP
{
    /// <summary>
    /// Unreachable from the outside mother class
    /// </summary>
    public abstract class VARMAP
    {
        /* All GET/SET/REG/UNREG Links */
        /* > ATG 1 START < */
        protected static GetVARMAPValueDelegate<GameOptionsStruct> _GET_GAME_OPTIONS;
        protected static SetVARMAPValueDelegate<GameOptionsStruct> _SET_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _REG_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _UNREG_GAME_OPTIONS;
        protected static GetVARMAPValueDelegate<ulong> _GET_ELAPSED_TIME_MS;
        protected static SetVARMAPValueDelegate<ulong> _SET_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_ELAPSED_TIME_MS;
        protected static GetVARMAPValueDelegate<Room> _GET_ACTUAL_ROOM;
        protected static SetVARMAPValueDelegate<Room> _SET_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _REG_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _UNREG_ACTUAL_ROOM;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_ELEM_EVENTS_OCCURRED;
        protected static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _SET_ELEM_EVENTS_OCCURRED;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_EVENTS_OCCURRED;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_ARRAY_EVENTS_OCCURRED;
        protected static SetVARMAPArrayDelegate<MultiBitFieldStruct> _SET_ARRAY_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _REG_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _UNREG_EVENTS_OCCURRED;
        protected static GetVARMAPArrayElemValueDelegate<CharacterType> _GET_ELEM_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayElemValueDelegate<CharacterType> _SET_ELEM_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayDelegate<CharacterType> _GET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayDelegate<CharacterType> _SET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayElemValueDelegate<int> _GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        protected static SetVARMAPArrayElemValueDelegate<int> _SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArrayDelegate<int> _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        protected static SetVARMAPArrayDelegate<int> _SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<int> _REG_PLAYER_ACTUAL_WAYPOINT;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<int> _UNREG_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPValueDelegate<Game_Status> _GET_GAMESTATUS;
        protected static SetVARMAPValueDelegate<Game_Status> _SET_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _REG_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _UNREG_GAMESTATUS;
        protected static GetVARMAPValueDelegate<KeyStruct> _GET_PRESSED_KEYS;
        protected static SetVARMAPValueDelegate<KeyStruct> _SET_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _REG_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _UNREG_PRESSED_KEYS;
        protected static GetVARMAPValueDelegate<MousePropertiesStruct> _GET_MOUSE_PROPERTIES;
        protected static SetVARMAPValueDelegate<MousePropertiesStruct> _SET_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _REG_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _UNREG_MOUSE_PROPERTIES;
        protected static GetVARMAPValueDelegate<CharacterType> _GET_PLAYER_SELECTED;
        protected static SetVARMAPValueDelegate<CharacterType> _SET_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PLAYER_SELECTED;
        protected static GetVARMAPValueDelegate<bool> _GET_ITEM_MENU_ACTIVE;
        protected static SetVARMAPValueDelegate<bool> _SET_ITEM_MENU_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_ITEM_MENU_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_ITEM_MENU_ACTIVE;
        protected static GetVARMAPValueDelegate<GameItem> _GET_PICKABLE_ITEM_CHOSEN;
        protected static SetVARMAPValueDelegate<GameItem> _SET_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _REG_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _UNREG_PICKABLE_ITEM_CHOSEN;
        protected static GetVARMAPArrayElemValueDelegate<ulong> _GET_ELEM_PLAYER_TRANSACTION;
        protected static SetVARMAPArrayElemValueDelegate<ulong> _SET_ELEM_PLAYER_TRANSACTION;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PLAYER_TRANSACTION;
        protected static GetVARMAPArrayDelegate<ulong> _GET_ARRAY_PLAYER_TRANSACTION;
        protected static SetVARMAPArrayDelegate<ulong> _SET_ARRAY_PLAYER_TRANSACTION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_PLAYER_TRANSACTION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_PLAYER_TRANSACTION;
        protected static GetVARMAPValueDelegate<bool> _GET_LAST_VARMAP_VAL;
        protected static SetVARMAPValueDelegate<bool> _SET_LAST_VARMAP_VAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_LAST_VARMAP_VAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_LAST_VARMAP_VAL;
        /* > ATG 1 END < */

        /* All SERVICE Links */
        /* > ATG 2 START < */
        /// <summary> 
        /// Starts game from main menu
        /// </summary>
        protected static START_GAME_DELEGATE _START_GAME;
        /// <summary> 
        /// Saves game at any moment
        /// </summary>
        protected static SAVE_GAME_DELEGATE _SAVE_GAME;
        /// <summary> 
        /// Loads game from any moment
        /// </summary>
        protected static LOAD_GAME_DELEGATE _LOAD_GAME;
        /// <summary> 
        /// Loads a room (for example when crossing a door)
        /// </summary>
        protected static LOAD_ROOM_DELEGATE _LOAD_ROOM;
        /// <summary> 
        /// Exits games to OS
        /// </summary>
        protected static EXIT_GAME_DELEGATE _EXIT_GAME;
        /// <summary> 
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// </summary>
        protected static LATE_START_SUBSCRIPTION_DELEGATE _LATE_START_SUBSCRIPTION;
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// </summary>
        protected static LODING_COMPLETED_DELEGATE _LOADING_COMPLETED;
        /// <summary> 
        /// This service is called to pause game or enter cinematic
        /// </summary>
        protected static FREEZE_PLAY_DELEGATE _FREEZE_PLAY;
        /// <summary> 
        /// Registers an NPC in system
        /// </summary>
        protected static NPC_REGISTER_DELEGATE _NPC_REGISTER;
        /// <summary> 
        /// Registers an item in system
        /// </summary>
        protected static ITEM_REGISTER_DELEGATE _ITEM_REGISTER;
        /// <summary> 
        /// Removes an item from level
        /// </summary>
        protected static ITEM_OBTAIN_PICKABLE_DELEGATE _ITEM_OBTAIN_PICKABLE;
        /// <summary> 
        /// Takes an item from scene (triggering event)
        /// </summary>
        protected static ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE _ITEM_OBTAIN_PICKABLE_EVENT;
        /// <summary> 
        /// Registers a player in scene
        /// </summary>
        protected static MONO_REGISTER_DELEGATE _MONO_REGISTER;
        /// <summary> 
        /// Registers a Waypoint in level
        /// </summary>
        protected static WP_REGISTER_DELEGATE _WP_REGISTER;
        /// <summary> 
        /// Registers a door in level
        /// </summary>
        protected static DOOR_REGISTER_DELEGATE _DOOR_REGISTER;
        /// <summary> 
        /// Updates actual player waypoint when crossing or stopping on it
        /// </summary>
        protected static PLAYER_WAYPOINT_UPDATE_DELEGATE _PLAYER_WAYPOINT_UPDATE;
        /// <summary> 
        /// Selects player
        /// </summary>
        protected static SELECT_PLAYER_DELEGATE _SELECT_PLAYER;
        /// <summary> 
        /// Gets a list of actual players
        /// </summary>
        protected static GET_PLAYER_LIST_DELEGATE _GET_PLAYER_LIST;
        /// <summary> 
        /// Gets a list of actual NPCs 
        /// </summary>
        protected static GET_NPC_LIST_DELEGATE _GET_NPC_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// </summary>
        protected static GET_NEAREST_WP_DELEGATE _GET_NEAREST_WP;
        /// <summary> 
        /// Tells if an event is occurred
        /// </summary>
        protected static IS_EVENT_OCCURRED_DELEGATE _IS_EVENT_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// </summary>
        protected static COMMIT_EVENT_DELEGATE _COMMIT_EVENT;
        /// <summary> 
        /// Uses an item with something
        /// </summary>
        protected static USE_ITEM_DELEGATE _USE_ITEM;
        /// <summary> 
        /// Tells if item is taken from scene
        /// </summary>
        protected static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE _IS_ITEM_TAKEN_FROM_SCENE;
        /// <summary> 
        /// Interacts player with an item
        /// </summary>
        protected static INTERACT_PLAYER_DELEGATE _INTERACT_PLAYER;
        /// <summary> 
        /// Gets scenario item list
        /// </summary>
        protected static GET_SCENARIO_ITEM_LIST_DELEGATE _GET_SCENARIO_ITEM_LIST;
        /// <summary> 
        /// Selects some pickable from inventory
        /// </summary>
        protected static SELECT_PICKABLE_ITEM_DELEGATE _SELECT_PICKABLE_ITEM;
        /// <summary> 
        /// Cancels selected item
        /// </summary>
        protected static CANCEL_PICKABLE_ITEM_DELEGATE _CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// </summary>
        protected static EVENT_SUBSCRIPTION_DELEGATE _EVENT_SUBSCRIPTION;
        /// <summary> 
        /// Trigger actions when crossing a door
        /// </summary>
        protected static CROSS_DOOR_DELEGATE _CROSS_DOOR;
        /// <summary> 
        /// Interacts player with NPC
        /// </summary>
        protected static INTERACT_PLAYER_NPC_DELEGATE _INTERACT_PLAYER_NPC;
        /// <summary> 
        /// Locks player so it cannot act until an action over it has been done (or removes lock)
        /// </summary>
        protected static LOCK_PLAYER_DELEGATE _LOCK_PLAYER;
        /// <summary> 
        /// Last service
        /// </summary>
        protected static EXIT_GAME_DELEGATE _LAST_SERVICE;
        /* > ATG 2 END < */
        
        



        /// <summary>
        /// All VARMAP Data
        /// </summary>
        protected static VARMAP_Variable_Indexable[] DATA;

        /// <summary>
        /// Memory security concept.
        /// </summary>
        protected static uint[] RUBISH_BIN;

    }



}
