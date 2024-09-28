using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVerse.VARMAP.Enum;
using MVerse.VARMAP.Types;
using MVerse.LevelOptions;
using MVerse.GameMaster;
using MVerse.LevelMaster;
using MVerse.InputMaster;
using MVerse.PhysicsMaster;
using MVerse.GraphicsMaster;
using MVerse.VARMAP.Variable;

namespace MVerse.VARMAP.Initialization
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
            _GET_ELEM_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).GetListElem;
            _SET_ELEM_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).SetListElem;
            _GET_SIZE_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).GetListSize;
            _GET_ARRAY_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).GetListCopy;
            _SET_ARRAY_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).SetListValues;
            _REG_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).RegisterChangeEvent;
            _UNREG_POWERS = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).UnregisterChangeEvent;
            _GET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).GetValue;
            _SET_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).SetValue;
            _REG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).RegisterChangeEvent;
            _UNREG_ELAPSED_TIME_MS = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).UnregisterChangeEvent;
            _GET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).GetValue;
            _SET_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue;
            _REG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).RegisterChangeEvent;
            _UNREG_ACTUAL_ROOM = ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).UnregisterChangeEvent;
            _GET_LIFE_TOTAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL]).GetValue;
            _SET_LIFE_TOTAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL]).SetValue;
            _REG_LIFE_TOTAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL]).RegisterChangeEvent;
            _UNREG_LIFE_TOTAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL]).UnregisterChangeEvent;
            _GET_LIFE_ACTUAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL]).GetValue;
            _SET_LIFE_ACTUAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL]).SetValue;
            _REG_LIFE_ACTUAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL]).RegisterChangeEvent;
            _UNREG_LIFE_ACTUAL = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL]).UnregisterChangeEvent;
            _GET_ELEM_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListElem;
            _SET_ELEM_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).SetListElem;
            _GET_SIZE_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListSize;
            _GET_ARRAY_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).GetListCopy;
            _SET_ARRAY_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).SetListValues;
            _REG_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).RegisterChangeEvent;
            _UNREG_ITEMS_COLLECTED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).UnregisterChangeEvent;
            _GET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListElem;
            _SET_ELEM_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListElem;
            _GET_SIZE_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListSize;
            _GET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).GetListCopy;
            _SET_ARRAY_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).SetListValues;
            _REG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).RegisterChangeEvent;
            _UNREG_EVENTS_OCCURRED = ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).UnregisterChangeEvent;
            _GET_ELEM_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).GetListElem;
            _SET_ELEM_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).SetListElem;
            _GET_SIZE_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).GetListSize;
            _GET_ARRAY_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).GetListCopy;
            _SET_ARRAY_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).SetListValues;
            _REG_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).RegisterChangeEvent;
            _UNREG_SELECTED_CHARMS = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).UnregisterChangeEvent;
            _GET_OTHER_WORLD = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD]).GetValue;
            _SET_OTHER_WORLD = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD]).SetValue;
            _REG_OTHER_WORLD = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD]).RegisterChangeEvent;
            _UNREG_OTHER_WORLD = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD]).UnregisterChangeEvent;
            _GET_OTHER_WORLD_TRANSITION_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_ACTIVE]).GetValue;
            _SET_OTHER_WORLD_TRANSITION_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_ACTIVE]).SetValue;
            _REG_OTHER_WORLD_TRANSITION_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_ACTIVE]).RegisterChangeEvent;
            _UNREG_OTHER_WORLD_TRANSITION_ACTIVE = ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_ACTIVE]).UnregisterChangeEvent;
            _GET_OTHER_WORLD_TRANSITION_PROGRESS = ((VARMAP_Variable_Interface<float>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_PROGRESS]).GetValue;
            _SET_OTHER_WORLD_TRANSITION_PROGRESS = ((VARMAP_Variable_Interface<float>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_PROGRESS]).SetValue;
            _REG_OTHER_WORLD_TRANSITION_PROGRESS = ((VARMAP_Variable_Interface<float>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_PROGRESS]).RegisterChangeEvent;
            _UNREG_OTHER_WORLD_TRANSITION_PROGRESS = ((VARMAP_Variable_Interface<float>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_PROGRESS]).UnregisterChangeEvent;
            _GET_OTHER_WORLD_MODE = ((VARMAP_Variable_Interface<OtherWorldMode>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_MODE]).GetValue;
            _SET_OTHER_WORLD_MODE = ((VARMAP_Variable_Interface<OtherWorldMode>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_MODE]).SetValue;
            _REG_OTHER_WORLD_MODE = ((VARMAP_Variable_Interface<OtherWorldMode>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_MODE]).RegisterChangeEvent;
            _UNREG_OTHER_WORLD_MODE = ((VARMAP_Variable_Interface<OtherWorldMode>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_MODE]).UnregisterChangeEvent;
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
            _GET_BOSS_STEP = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_BOSS_STEP]).GetValue;
            _SET_BOSS_STEP = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_BOSS_STEP]).SetValue;
            _REG_BOSS_STEP = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_BOSS_STEP]).RegisterChangeEvent;
            _UNREG_BOSS_STEP = ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_BOSS_STEP]).UnregisterChangeEvent;
            /* > ATG 1 END */


            /* All Service Links */
            /* > ATG 2 START */
            _START_GAME = GameMasterClass.StartGameService;
            _LOAD_OBF = GameMasterClass.LoadOneBossFightService;
            _LOAD_ROOM = GameMasterClass.LoadRoomService;
            _EXIT_GAME = GameMasterClass.ExitGameService;
            _LOADING_COMPLETED = GameMasterClass.LoadingCompletedService;
            _CHANGE_OTHER_WORLD = LevelMasterClass.ChangeOtherWorldService;
            _FREEZE_PLAY = GameMasterClass.FreezePlayService;
            _ENEMY_REGISTER = LevelMasterClass.EnemyRegisterService;
            _MONO_REGISTER = LevelMasterClass.MonoRegisterService;
            /* > ATG 2 END */
        }
    }
}
