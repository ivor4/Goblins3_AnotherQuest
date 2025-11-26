using Gob3AQ.GameEventMaster;
using Gob3AQ.GameMaster;
using Gob3AQ.GameMenu;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.InputMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.VARMAP.Variable;
using System;
using System.Collections.Generic;
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
        protected static GetVARMAPValueDelegate<GameOptionsStruct> _GET_SHADOW_GAME_OPTIONS;
        protected static SetVARMAPValueDelegate<GameOptionsStruct> _SET_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _REG_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _UNREG_GAME_OPTIONS;
        protected static GetVARMAPValueDelegate<ulong> _GET_ELAPSED_TIME_MS;
        protected static GetVARMAPValueDelegate<ulong> _GET_SHADOW_ELAPSED_TIME_MS;
        protected static SetVARMAPValueDelegate<ulong> _SET_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_ELAPSED_TIME_MS;
        protected static GetVARMAPValueDelegate<Room> _GET_ACTUAL_ROOM;
        protected static GetVARMAPValueDelegate<Room> _GET_SHADOW_ACTUAL_ROOM;
        protected static SetVARMAPValueDelegate<Room> _SET_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _REG_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _UNREG_ACTUAL_ROOM;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_ELEM_EVENTS_OCCURRED;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_SHADOW_ELEM_EVENTS_OCCURRED;
        protected static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _SET_ELEM_EVENTS_OCCURRED;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_EVENTS_OCCURRED;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_ARRAY_EVENTS_OCCURRED;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_SHADOW_ARRAY_EVENTS_OCCURRED;
        protected static SetVARMAPArrayDelegate<MultiBitFieldStruct> _SET_ARRAY_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _REG_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _UNREG_EVENTS_OCCURRED;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_ELEM_UNLOCKED_MEMENTO;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_SHADOW_ELEM_UNLOCKED_MEMENTO;
        protected static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _SET_ELEM_UNLOCKED_MEMENTO;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_UNLOCKED_MEMENTO;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_ARRAY_UNLOCKED_MEMENTO;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_SHADOW_ARRAY_UNLOCKED_MEMENTO;
        protected static SetVARMAPArrayDelegate<MultiBitFieldStruct> _SET_ARRAY_UNLOCKED_MEMENTO;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _REG_UNLOCKED_MEMENTO;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _UNREG_UNLOCKED_MEMENTO;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_ELEM_UNWATCHED_PARENT_MEMENTO;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO;
        protected static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _SET_ELEM_UNWATCHED_PARENT_MEMENTO;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_UNWATCHED_PARENT_MEMENTO;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_ARRAY_UNWATCHED_PARENT_MEMENTO;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_SHADOW_ARRAY_UNWATCHED_PARENT_MEMENTO;
        protected static SetVARMAPArrayDelegate<MultiBitFieldStruct> _SET_ARRAY_UNWATCHED_PARENT_MEMENTO;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _REG_UNWATCHED_PARENT_MEMENTO;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _UNREG_UNWATCHED_PARENT_MEMENTO;
        protected static GetVARMAPArrayElemValueDelegate<CharacterType> _GET_ELEM_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayElemValueDelegate<CharacterType> _GET_SHADOW_ELEM_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayElemValueDelegate<CharacterType> _SET_ELEM_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayDelegate<CharacterType> _GET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayDelegate<CharacterType> _GET_SHADOW_ARRAY_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayDelegate<CharacterType> _SET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayElemValueDelegate<int> _GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArrayElemValueDelegate<int> _GET_SHADOW_ELEM_PLAYER_ACTUAL_WAYPOINT;
        protected static SetVARMAPArrayElemValueDelegate<int> _SET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArrayDelegate<int> _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPArrayDelegate<int> _GET_SHADOW_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        protected static SetVARMAPArrayDelegate<int> _SET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<int> _REG_PLAYER_ACTUAL_WAYPOINT;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<int> _UNREG_PLAYER_ACTUAL_WAYPOINT;
        protected static GetVARMAPValueDelegate<CameraDispositionStruct> _GET_CAMERA_DISPOSITION;
        protected static GetVARMAPValueDelegate<CameraDispositionStruct> _GET_SHADOW_CAMERA_DISPOSITION;
        protected static SetVARMAPValueDelegate<CameraDispositionStruct> _SET_CAMERA_DISPOSITION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CameraDispositionStruct> _REG_CAMERA_DISPOSITION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CameraDispositionStruct> _UNREG_CAMERA_DISPOSITION;
        protected static GetVARMAPValueDelegate<Game_Status> _GET_GAMESTATUS;
        protected static GetVARMAPValueDelegate<Game_Status> _GET_SHADOW_GAMESTATUS;
        protected static SetVARMAPValueDelegate<Game_Status> _SET_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _REG_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _UNREG_GAMESTATUS;
        protected static GetVARMAPValueDelegate<KeyStruct> _GET_PRESSED_KEYS;
        protected static GetVARMAPValueDelegate<KeyStruct> _GET_SHADOW_PRESSED_KEYS;
        protected static SetVARMAPValueDelegate<KeyStruct> _SET_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _REG_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _UNREG_PRESSED_KEYS;
        protected static GetVARMAPValueDelegate<MousePropertiesStruct> _GET_MOUSE_PROPERTIES;
        protected static GetVARMAPValueDelegate<MousePropertiesStruct> _GET_SHADOW_MOUSE_PROPERTIES;
        protected static SetVARMAPValueDelegate<MousePropertiesStruct> _SET_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _REG_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _UNREG_MOUSE_PROPERTIES;
        protected static GetVARMAPValueDelegate<CharacterType> _GET_PLAYER_SELECTED;
        protected static GetVARMAPValueDelegate<CharacterType> _GET_SHADOW_PLAYER_SELECTED;
        protected static SetVARMAPValueDelegate<CharacterType> _SET_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PLAYER_SELECTED;
        protected static GetVARMAPValueDelegate<GameItem> _GET_PICKABLE_ITEM_CHOSEN;
        protected static GetVARMAPValueDelegate<GameItem> _GET_SHADOW_PICKABLE_ITEM_CHOSEN;
        protected static SetVARMAPValueDelegate<GameItem> _SET_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _REG_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _UNREG_PICKABLE_ITEM_CHOSEN;
        protected static GetVARMAPValueDelegate<GameItem> _GET_ITEM_HOVER;
        protected static GetVARMAPValueDelegate<GameItem> _GET_SHADOW_ITEM_HOVER;
        protected static SetVARMAPValueDelegate<GameItem> _SET_ITEM_HOVER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _REG_ITEM_HOVER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> _UNREG_ITEM_HOVER;
        protected static GetVARMAPValueDelegate<UserInputInteraction> _GET_USER_INPUT_INTERACTION;
        protected static GetVARMAPValueDelegate<UserInputInteraction> _GET_SHADOW_USER_INPUT_INTERACTION;
        protected static SetVARMAPValueDelegate<UserInputInteraction> _SET_USER_INPUT_INTERACTION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<UserInputInteraction> _REG_USER_INPUT_INTERACTION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<UserInputInteraction> _UNREG_USER_INPUT_INTERACTION;
        protected static GetVARMAPValueDelegate<bool> _GET_EVENTS_BEING_PROCESSED;
        protected static GetVARMAPValueDelegate<bool> _GET_SHADOW_EVENTS_BEING_PROCESSED;
        protected static SetVARMAPValueDelegate<bool> _SET_EVENTS_BEING_PROCESSED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_EVENTS_BEING_PROCESSED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_EVENTS_BEING_PROCESSED;
        protected static GetVARMAPValueDelegate<bool> _GET_LAST_VARMAP_VAL;
        protected static GetVARMAPValueDelegate<bool> _GET_SHADOW_LAST_VARMAP_VAL;
        protected static SetVARMAPValueDelegate<bool> _SET_LAST_VARMAP_VAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_LAST_VARMAP_VAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_LAST_VARMAP_VAL;
        /* > ATG 1 END < */

        /* All SERVICE Links */
        /* > ATG 2 START < */
        /// <summary> 
        /// Starts game from main menu
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.StartGameService"/> </para> 
        /// </summary>
        protected static START_GAME_DELEGATE _START_GAME;
        /// <summary> 
        /// Saves game at any moment
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.SaveGameService"/> </para> 
        /// </summary>
        protected static SAVE_GAME_DELEGATE _SAVE_GAME;
        /// <summary> 
        /// Loads game from any moment
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadGameService"/> </para> 
        /// </summary>
        protected static LOAD_GAME_DELEGATE _LOAD_GAME;
        /// <summary> 
        /// Loads a room (for example when crossing a door)
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadRoomService"/> </para> 
        /// </summary>
        protected static LOAD_ROOM_DELEGATE _LOAD_ROOM;
        /// <summary> 
        /// Exits games to OS
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ExitGameService"/> </para> 
        /// </summary>
        protected static EXIT_GAME_DELEGATE _EXIT_GAME;
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        protected static LODING_COMPLETED_DELEGATE _MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        protected static IS_MODULE_LOADED_DELEGATE _IS_MODULE_LOADED;
        /// <summary> 
        /// Registers an item in system
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ItemRegisterService"/> </para> 
        /// </summary>
        protected static ITEM_REGISTER_DELEGATE _ITEM_REGISTER;
        /// <summary> 
        /// Registers a player in scene
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.MonoRegisterService"/> </para> 
        /// </summary>
        protected static MONO_REGISTER_DELEGATE _MONO_REGISTER;
        /// <summary> 
        /// Registers a door in level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.DoorRegisterService"/> </para> 
        /// </summary>
        protected static DOOR_REGISTER_DELEGATE _DOOR_REGISTER;
        /// <summary> 
        /// Obtains dictionary of placed elements in actual Scene
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ObtainScenarioItemsService"/> </para> 
        /// </summary>
        protected static OBTAIN_SCENARIO_ITEMS_DELEGATE _OBTAIN_SCENARIO_ITEMS;
        /// <summary> 
        /// Updates actual player waypoint when crossing or stopping on it
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.PlayerWaypointUpdateService"/> </para> 
        /// </summary>
        protected static PLAYER_WAYPOINT_UPDATE_DELEGATE _PLAYER_WAYPOINT_UPDATE;
        /// <summary> 
        /// Gets list of waypoints and solutions
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetWaypointListService"/> </para> 
        /// </summary>
        protected static GET_WP_LIST_DELEGATE _GET_WP_LIST;
        /// <summary> 
        /// Any of Game Elements (Player or Item or Door) will call with essential info
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: GameMenu, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GameElementOverService"/> </para> 
        /// </summary>
        protected static GAME_ELEMENT_HOVER_DELEGATE _GAME_ELEMENT_HOVER;
        /// <summary> 
        /// Gets a list of actual players
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: GraphicsMaster, PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetPlayerListService"/> </para> 
        /// </summary>
        protected static GET_PLAYER_LIST_DELEGATE _GET_PLAYER_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetNearestWPService"/> </para> 
        /// </summary>
        protected static GET_NEAREST_WP_DELEGATE _GET_NEAREST_WP;
        /// <summary> 
        /// Checks if a combination of events is totally complied (event absence can also be requested)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventCombiOccurredService"/> </para> 
        /// </summary>
        protected static IS_EVENT_COMBI_OCCURRED_DELEGATE _IS_EVENT_COMBI_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        protected static COMMIT_EVENT_DELEGATE _COMMIT_EVENT;
        /// <summary> 
        /// Tells Memento Manager (Menu) a new memento has been unlocked
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.CommitMementoNotifService"/> </para> 
        /// </summary>
        protected static COMMIT_MEMENTO_NOTIF_DELEGATE _COMMIT_MEMENTO_NOTIF;
        /// <summary> 
        /// Tells if a memento is unlocked
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsMementoUnlockedService"/> </para> 
        /// </summary>
        protected static IS_MEMENTO_UNLOCKED_DELEGATE _IS_MEMENTO_UNLOCKED;
        /// <summary> 
        /// If a Memento has been analyzed
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.MementoParentWatchedService"/> </para> 
        /// </summary>
        protected static MEMENTO_PARENT_WATCHED_DELEGATE _MEMENTO_PARENT_WATCHED;
        /// <summary> 
        /// Makes player interact with usage data
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.InteractPlayerService"/> </para> 
        /// </summary>
        protected static INTERACT_PLAYER_DELEGATE _INTERACT_PLAYER;
        /// <summary> 
        /// Applies an unchain event to an item such as spawn or setsprite
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.UnchainToItemService"/> </para> 
        /// </summary>
        protected static UNCHAIN_TO_ITEM_DELEGATE _UNCHAIN_TO_ITEM;
        /// <summary> 
        /// Tells LevelMaster that player reached Waypoint to start action in case
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.PlayerReachedWaypointService"/> </para> 
        /// </summary>
        protected static PLAYER_REACHED_WAYPOINT_DELEGATE _PLAYER_REACHED_WAYPOINT;
        /// <summary> 
        /// Uses an item with something
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.UseItemService"/> </para> 
        /// </summary>
        protected static USE_ITEM_DELEGATE _USE_ITEM;
        /// <summary> 
        /// Cancels selected item
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.CancelPickableItemService"/> </para> 
        /// </summary>
        protected static CANCEL_PICKABLE_ITEM_DELEGATE _CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to determined key events to avoid polling
        /// <para> Owner: InputMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="InputMasterClass.KeySubscriptionService"/> </para> 
        /// </summary>
        protected static KEY_SUBSCRIPTION_DELEGATE _KEY_SUBSCRIPTION;
        /// <summary> 
        /// Locks player so it cannot act until an action over it has been done (or removes lock)
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.LockPlayerService"/> </para> 
        /// </summary>
        protected static LOCK_PLAYER_DELEGATE _LOCK_PLAYER;
        /// <summary> 
        /// Asks Game Master to set game mode
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeGameModeService"/> </para> 
        /// </summary>
        protected static CHANGE_GAME_MODE_DELEGATE _CHANGE_GAME_MODE;
        /// <summary> 
        /// Second part of start dialogue. Tells Game Menu to prepare menu elements
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.ShowDialogueService"/> </para> 
        /// </summary>
        protected static SHOW_DIALOGUE_DELEGATE _SHOW_DIALOGUE;
        /// <summary> 
        /// Subscribe to zoom changes
        /// <para> Owner: GraphicsMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="GraphicsMasterClass.ZoomSubscriptionService"/> </para> 
        /// </summary>
        protected static ZOOM_SUBSCRIPTION_DELEGATE _ZOOM_SUBSCRIPTION;
        /// <summary> 
        /// Last service
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors:  </para> 
        /// <para> Method: <see cref="GameMasterClass.ExitGameService"/> </para> 
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
