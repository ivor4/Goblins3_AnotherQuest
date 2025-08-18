using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.InputMaster;
using Gob3AQ.GraphicsMaster;
using Gob3AQ.NPCMaster;
using Gob3AQ.VARMAP.Variable;
using Gob3AQ.PlayerMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameMenu;

namespace Gob3AQ.VARMAP.Initialization
{
    public sealed class VARMAP_Initialization : VARMAP
    {
        /// <summary>
        /// Updates delegates according to recently created instances of VARMAP Data. Must be called with Initialization process
        /// </summary>
        public static void UpdateDelegates()
        {
            /* All GET/SET/REG/UNREG Links */
            /* > ATG 1 START */
            _GET_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).GetValue;
            _GET_SHADOW_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).GetShadowValue;
            _SET_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).SetValue;
            _REG_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).RegisterChangeEvent;
            _UNREG_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).UnregisterChangeEvent;
            _GET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).GetValue;
            _GET_SHADOW_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).GetShadowValue;
            _SET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).SetValue;
            _REG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).RegisterChangeEvent;
            _UNREG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).UnregisterChangeEvent;
            _GET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).GetValue;
            _GET_SHADOW_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).GetShadowValue;
            _SET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue;
            _REG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).RegisterChangeEvent;
            _UNREG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).UnregisterChangeEvent;
            _GET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListElem;
            _GET_SHADOW_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetShadowListElem;
            _SET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListElem;
            _GET_SIZE_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListSize;
            _GET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListCopy;
            _GET_SHADOW_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetShadowListCopy;
            _SET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListValues;
            _REG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).RegisterChangeEvent;
            _UNREG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).UnregisterChangeEvent;
            _GET_ELEM_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).GetListElem;
            _GET_SHADOW_ELEM_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).GetShadowListElem;
            _SET_ELEM_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).SetListElem;
            _GET_SIZE_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).GetListSize;
            _GET_ARRAY_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).GetListCopy;
            _GET_SHADOW_ARRAY_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).GetShadowListCopy;
            _SET_ARRAY_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).SetListValues;
            _REG_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).RegisterChangeEvent;
            _UNREG_PICKABLE_ITEM_OWNER = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).UnregisterChangeEvent;
            _GET_ELEM_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).GetListElem;
            _GET_SHADOW_ELEM_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).GetShadowListElem;
            _SET_ELEM_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).SetListElem;
            _GET_SIZE_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).GetListSize;
            _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).GetListCopy;
            _GET_SHADOW_ARRAY_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).GetShadowListCopy;
            _SET_ARRAY_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).SetListValues;
            _REG_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).RegisterChangeEvent;
            _UNREG_PLAYER_ACTUAL_WAYPOINT = ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).UnregisterChangeEvent;
            _GET_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).GetValue;
            _GET_SHADOW_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).GetShadowValue;
            _SET_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).SetValue;
            _REG_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).RegisterChangeEvent;
            _UNREG_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).UnregisterChangeEvent;
            _GET_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).GetValue;
            _GET_SHADOW_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).GetShadowValue;
            _SET_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).SetValue;
            _REG_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).RegisterChangeEvent;
            _UNREG_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).UnregisterChangeEvent;
            _GET_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).GetValue;
            _GET_SHADOW_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).GetShadowValue;
            _SET_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).SetValue;
            _REG_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).RegisterChangeEvent;
            _UNREG_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).UnregisterChangeEvent;
            _GET_PLAYER_SELECTED = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).GetValue;
            _GET_SHADOW_PLAYER_SELECTED = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).GetShadowValue;
            _SET_PLAYER_SELECTED = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).SetValue;
            _REG_PLAYER_SELECTED = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).RegisterChangeEvent;
            _UNREG_PLAYER_SELECTED = ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).UnregisterChangeEvent;
            _GET_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).GetValue;
            _GET_SHADOW_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).GetShadowValue;
            _SET_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).SetValue;
            _REG_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).RegisterChangeEvent;
            _UNREG_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).UnregisterChangeEvent;
            _GET_PICKABLE_ITEM_CHOSEN = ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).GetValue;
            _GET_SHADOW_PICKABLE_ITEM_CHOSEN = ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).GetShadowValue;
            _SET_PICKABLE_ITEM_CHOSEN = ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).SetValue;
            _REG_PICKABLE_ITEM_CHOSEN = ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).RegisterChangeEvent;
            _UNREG_PICKABLE_ITEM_CHOSEN = ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).UnregisterChangeEvent;
            _GET_ELEM_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).GetListElem;
            _GET_SHADOW_ELEM_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).GetShadowListElem;
            _SET_ELEM_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).SetListElem;
            _GET_SIZE_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).GetListSize;
            _GET_ARRAY_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).GetListCopy;
            _GET_SHADOW_ARRAY_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).GetShadowListCopy;
            _SET_ARRAY_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).SetListValues;
            _REG_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).RegisterChangeEvent;
            _UNREG_PLAYER_TRANSACTION = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION]).UnregisterChangeEvent;
            _GET_LAST_VARMAP_VAL = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).GetValue;
            _GET_SHADOW_LAST_VARMAP_VAL = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).GetShadowValue;
            _SET_LAST_VARMAP_VAL = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).SetValue;
            _REG_LAST_VARMAP_VAL = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).RegisterChangeEvent;
            _UNREG_LAST_VARMAP_VAL = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).UnregisterChangeEvent;
            /* > ATG 1 END */


            /* All Service Links */
            /* > ATG 2 START */
            _START_GAME = GameMasterClass.StartGameService;
            _SAVE_GAME = GameMasterClass.SaveGameService;
            _LOAD_GAME = GameMasterClass.LoadGameService;
            _LOAD_ROOM = GameMasterClass.LoadRoomService;
            _EXIT_GAME = GameMasterClass.ExitGameService;
            _LATE_START_SUBSCRIPTION = GameMasterClass.LateStartSubrsciptionService;
            _MODULE_LOADING_COMPLETED = GameMasterClass.LoadingCompletedService;
            _IS_MODULE_LOADED = GameMasterClass.IsModuleLoadedService;
            _FREEZE_PLAY = GameMasterClass.FreezePlayService;
            _NPC_REGISTER = LevelMasterClass.NPCRegisterService;
            _ITEM_REGISTER = LevelMasterClass.ItemRegisterService;
            _ITEM_OBTAIN_PICKABLE = LevelMasterClass.ItemObtainPickableService;
            _ITEM_OBTAIN_PICKABLE_EVENT = GameEventMasterClass.ItemObtainPickableEventService;
            _MONO_REGISTER = LevelMasterClass.MonoRegisterService;
            _WP_REGISTER = LevelMasterClass.WPRegisterService;
            _DOOR_REGISTER = LevelMasterClass.DoorRegisterService;
            _PLAYER_WAYPOINT_UPDATE = LevelMasterClass.PlayerWaypointUpdateService;
            _SELECT_PLAYER = PlayerMasterClass.SelectPlayerService;
            _GET_PLAYER_LIST = LevelMasterClass.GetPlayerListService;
            _GET_NPC_LIST = LevelMasterClass.GetNPCListService;
            _GET_NEAREST_WP = LevelMasterClass.GetNearestWPService;
            _IS_EVENT_OCCURRED = GameEventMasterClass.IsEventOccurredService;
            _COMMIT_EVENT = GameEventMasterClass.CommitEventService;
            _USE_ITEM = ItemMasterClass.UseItemService;
            _IS_ITEM_TAKEN_FROM_SCENE = GameEventMasterClass.IsItemTakenFromSceneService;
            _INTERACT_PLAYER = PlayerMasterClass.InteractPlayerService;
            _GET_SCENARIO_ITEM_LIST = LevelMasterClass.GetScenarioItemListService;
            _SELECT_PICKABLE_ITEM = ItemMasterClass.SelectPickableItemService;
            _CANCEL_PICKABLE_ITEM = ItemMasterClass.CancelPickableItemService;
            _EVENT_SUBSCRIPTION = GameEventMasterClass.EventSubscriptionService;
            _CROSS_DOOR = LevelMasterClass.CrossDoorService;
            _INTERACT_PLAYER_NPC = NPCMasterClass.InteractPlayerNPCService;
            _LOCK_PLAYER = PlayerMasterClass.LockPlayerService;
            _START_DIALOGUE = GameMasterClass.StartDialogueService;
            _SHOW_DIALOGUE = GameMenuClass.ShowDialogueService;
            _LAST_SERVICE = GameMasterClass.ExitGameService;
            /* > ATG 2 END */
        }
    }
}
