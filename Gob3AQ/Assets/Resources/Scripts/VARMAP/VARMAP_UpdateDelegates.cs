using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.LevelOptions;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.InputMaster;
using Gob3AQ.GraphicsMaster;
using Gob3AQ.VARMAP.Variable;
using Gob3AQ.PlayerMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.ItemMaster;

namespace Gob3AQ.VARMAP.Initialization
{
    public abstract partial class VARMAP_Initialization : VARMAP
    {
        /// <summary>
        /// Updates delegates according to recently created instances of VARMAP Data. Must be called with Initialization process
        /// </summary>
        public static void UpdateDelegates()
        {
            /* All GET/SET/REG/UNREG Links */
            /* > ATG 1 START */
            _GET_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).GetValue;
            _SET_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).SetValue;
            _REG_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).RegisterChangeEvent;
            _UNREG_GAME_OPTIONS = ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).UnregisterChangeEvent;
            _GET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).GetValue;
            _SET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).SetValue;
            _REG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).RegisterChangeEvent;
            _UNREG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).UnregisterChangeEvent;
            _GET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).GetValue;
            _SET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue;
            _REG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).RegisterChangeEvent;
            _UNREG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).UnregisterChangeEvent;
            _GET_ELEM_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListElem;
            _SET_ELEM_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).SetListElem;
            _GET_SIZE_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListSize;
            _GET_ARRAY_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListCopy;
            _SET_ARRAY_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).SetListValues;
            _REG_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).RegisterChangeEvent;
            _UNREG_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).UnregisterChangeEvent;
            _GET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListElem;
            _SET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListElem;
            _GET_SIZE_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListSize;
            _GET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListCopy;
            _SET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListValues;
            _REG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).RegisterChangeEvent;
            _UNREG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).UnregisterChangeEvent;
            _GET_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).GetValue;
            _SET_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).SetValue;
            _REG_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).RegisterChangeEvent;
            _UNREG_GAMESTATUS = ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).UnregisterChangeEvent;
            _GET_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).GetValue;
            _SET_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).SetValue;
            _REG_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).RegisterChangeEvent;
            _UNREG_PRESSED_KEYS = ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).UnregisterChangeEvent;
            _GET_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).GetValue;
            _SET_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).SetValue;
            _REG_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).RegisterChangeEvent;
            _UNREG_MOUSE_PROPERTIES = ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).UnregisterChangeEvent;
            _GET_PLAYER_POSITION = ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).GetValue;
            _SET_PLAYER_POSITION = ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).SetValue;
            _REG_PLAYER_POSITION = ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).RegisterChangeEvent;
            _UNREG_PLAYER_POSITION = ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).UnregisterChangeEvent;
            _GET_PLAYER_ID_SELECTED = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ID_SELECTED]).GetValue;
            _SET_PLAYER_ID_SELECTED = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ID_SELECTED]).SetValue;
            _REG_PLAYER_ID_SELECTED = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ID_SELECTED]).RegisterChangeEvent;
            _UNREG_PLAYER_ID_SELECTED = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ID_SELECTED]).UnregisterChangeEvent;
            _GET_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).GetValue;
            _SET_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).SetValue;
            _REG_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).RegisterChangeEvent;
            _UNREG_ITEM_MENU_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE]).UnregisterChangeEvent;
            /* > ATG 1 END */


            /* All Service Links */
            /* > ATG 2 START */
            _START_GAME = GameMasterClass.StartGameService;
            _LOAD_ROOM = GameMasterClass.LoadRoomService;
            _EXIT_GAME = GameMasterClass.ExitGameService;
            _LOADING_COMPLETED = GameMasterClass.LoadingCompletedService;
            _FREEZE_PLAY = GameMasterClass.FreezePlayService;
            _NPC_REGISTER = LevelMasterClass.NPCRegisterService;
            _ITEM_REGISTER = LevelMasterClass.ItemRegisterService;
            _MONO_REGISTER = LevelMasterClass.MonoRegisterService;
            _WP_REGISTER = LevelMasterClass.WPRegisterService;
            _MOVE_PLAYER = PlayerMasterClass.MovePlayerService;
            _GET_PLAYER_LIST = LevelMasterClass.GetPlayerListService;
            _GET_NEAREST_WP = LevelMasterClass.GetNearestWPService;
            _IS_EVENT_OCCURRED = GameEventMasterClass.IsEventOccurredService;
            _COMMIT_EVENT = GameEventMasterClass.CommitEventService;
            _TAKE_ITEM_EVENT = GameEventMasterClass.TakeItemEventService;
            _RETAKE_ITEM_EVENT = GameEventMasterClass.ReTakeItemEventService;
            _IS_ITEM_TAKEN_FIRST = GameEventMasterClass.IsItemTakenFirstService;
            _IS_ITEM_TAKEN = GameEventMasterClass.IsItemTakenService;
            _INTERACT_ITEM_PLAYER = PlayerMasterClass.InteractItemPlayerService;
            _GET_ITEM_INTERACTION = GameEventMasterClass.GetItemInteractionService;
            _TAKE_ITEM_OBJECT = ItemMasterClass.TakeItemObjectService;
            _GET_SCENARIO_ITEM_LIST = LevelMasterClass.GetScenarioItemListService;
            _GET_PICKED_ITEM_LIST = ItemMasterClass.GetPickedItemListService;
            /* > ATG 2 END */
        }
    }
}
