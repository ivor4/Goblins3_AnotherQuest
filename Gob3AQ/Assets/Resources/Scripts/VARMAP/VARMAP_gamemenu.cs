using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.NPCMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;

namespace Gob3AQ.VARMAP.GameMenu
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GameMenu : VARMAP
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
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_ITEM_MENU_ACTIVE = _GET_ITEM_MENU_ACTIVE;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            START_GAME = _START_GAME;
            SAVE_GAME = _SAVE_GAME;
            LOAD_GAME = _LOAD_GAME;
            EXIT_GAME = _EXIT_GAME;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            SELECT_PICKABLE_ITEM = _SELECT_PICKABLE_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
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
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<bool> GET_ITEM_MENU_ACTIVE;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
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
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, NPCMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LateStartSubrsciptionService"/> </para> 
        /// </summary>
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        /// <summary> 
        /// Selects some pickable from inventory
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.SelectPickableItemService"/> </para> 
        /// </summary>
        public static SELECT_PICKABLE_ITEM_DELEGATE SELECT_PICKABLE_ITEM;
        /// <summary> 
        /// Cancels selected item
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameMaster, LevelMaster, GameMenu,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.CancelPickableItemService"/> </para> 
        /// </summary>
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /* > ATG 3 END */
    }
}
