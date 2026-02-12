using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.InputMaster;
using Gob3AQ.GameEventMaster;

namespace Gob3AQ.VARMAP.GameMenu
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_GameMenu : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_SHADOW_GAME_OPTIONS = _GET_SHADOW_GAME_OPTIONS;
            SET_GAME_OPTIONS = _SET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            REG_ACTUAL_ROOM = _REG_ACTUAL_ROOM;
            UNREG_ACTUAL_ROOM = _UNREG_ACTUAL_ROOM;
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            REG_PICKABLE_ITEM_OWNER = _REG_PICKABLE_ITEM_OWNER;
            UNREG_PICKABLE_ITEM_OWNER = _UNREG_PICKABLE_ITEM_OWNER;
            GET_ITEM_MENU_HOVER = _GET_ITEM_MENU_HOVER;
            GET_SHADOW_ITEM_MENU_HOVER = _GET_SHADOW_ITEM_MENU_HOVER;
            SET_ITEM_MENU_HOVER = _SET_ITEM_MENU_HOVER;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            GET_SHADOW_PICKABLE_ITEM_CHOSEN = _GET_SHADOW_PICKABLE_ITEM_CHOSEN;
            SET_PICKABLE_ITEM_CHOSEN = _SET_PICKABLE_ITEM_CHOSEN;
            GET_USER_INPUT_INTERACTION = _GET_USER_INPUT_INTERACTION;
            GET_SHADOW_USER_INPUT_INTERACTION = _GET_SHADOW_USER_INPUT_INTERACTION;
            SET_USER_INPUT_INTERACTION = _SET_USER_INPUT_INTERACTION;
            GET_EVENTS_BEING_PROCESSED = _GET_EVENTS_BEING_PROCESSED;
            GET_DAY_MOMENT = _GET_DAY_MOMENT;
            START_GAME = _START_GAME;
            SAVE_GAME = _SAVE_GAME;
            LOAD_GAME = _LOAD_GAME;
            EXIT_GAME = _EXIT_GAME;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            COMMIT_MEMENTO_NOTIF = _COMMIT_MEMENTO_NOTIF;
            IS_MEMENTO_UNLOCKED = _IS_MEMENTO_UNLOCKED;
            MEMENTO_PARENT_WATCHED = _MEMENTO_PARENT_WATCHED;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            KEY_SUBSCRIPTION = _KEY_SUBSCRIPTION;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            SHOW_DIALOGUE = _SHOW_DIALOGUE;
            SHOW_DECISION = _SHOW_DECISION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_SHADOW_GAME_OPTIONS;
        public static SetVARMAPValueDelegate<GameOptionsStruct> SET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Room> REG_ACTUAL_ROOM;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Room> UNREG_ACTUAL_ROOM;
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> REG_PICKABLE_ITEM_OWNER;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> UNREG_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<GameItem> GET_ITEM_MENU_HOVER;
        public static GetVARMAPValueDelegate<GameItem> GET_SHADOW_ITEM_MENU_HOVER;
        public static SetVARMAPValueDelegate<GameItem> SET_ITEM_MENU_HOVER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static GetVARMAPValueDelegate<GameItem> GET_SHADOW_PICKABLE_ITEM_CHOSEN;
        public static SetVARMAPValueDelegate<GameItem> SET_PICKABLE_ITEM_CHOSEN;
        public static GetVARMAPValueDelegate<UserInputInteraction> GET_USER_INPUT_INTERACTION;
        public static GetVARMAPValueDelegate<UserInputInteraction> GET_SHADOW_USER_INPUT_INTERACTION;
        public static SetVARMAPValueDelegate<UserInputInteraction> SET_USER_INPUT_INTERACTION;
        public static GetVARMAPValueDelegate<bool> GET_EVENTS_BEING_PROCESSED;
        public static GetVARMAPValueDelegate<MomentType> GET_DAY_MOMENT;
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
        /// Checks if a combination of events is totally complied (event absence can also be requested)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventCombiOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_COMBI_OCCURRED_DELEGATE IS_EVENT_COMBI_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMaster, LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Tells Memento Manager (Menu) a new memento has been unlocked
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.CommitMementoNotifService"/> </para> 
        /// </summary>
        public static COMMIT_MEMENTO_NOTIF_DELEGATE COMMIT_MEMENTO_NOTIF;
        /// <summary> 
        /// Tells if a memento is unlocked
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsMementoUnlockedService"/> </para> 
        /// </summary>
        public static IS_MEMENTO_UNLOCKED_DELEGATE IS_MEMENTO_UNLOCKED;
        /// <summary> 
        /// If a Memento has been analyzed
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.MementoParentWatchedService"/> </para> 
        /// </summary>
        public static MEMENTO_PARENT_WATCHED_DELEGATE MEMENTO_PARENT_WATCHED;
        /// <summary> 
        /// Cancels selected item
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.CancelPickableItemService"/> </para> 
        /// </summary>
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to determined key events to avoid polling
        /// <para> Owner: InputMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="InputMasterClass.KeySubscriptionService"/> </para> 
        /// </summary>
        public static KEY_SUBSCRIPTION_DELEGATE KEY_SUBSCRIPTION;
        /// <summary> 
        /// Asks Game Master to set game mode
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, GameEventMaster,  </para> 
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
        /// <summary> 
        /// Shows a set of decisions
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.ShowDecisionService"/> </para> 
        /// </summary>
        public static SHOW_DECISION_DELEGATE SHOW_DECISION;
        /* > ATG 3 END */
    }
}
