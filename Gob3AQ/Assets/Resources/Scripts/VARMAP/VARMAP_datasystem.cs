using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Variable;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Parsers;
using UnityEngine;

namespace Gob3AQ.VARMAP.Initialization
{
    public abstract partial class VARMAP_Initialization : VARMAP
    {
        /// <summary>
        /// Should only be called once in Program execution, at Start.
        /// Creates every VARMAP Variable instance according to architecture type
        /// </summary>
        public static void InitializeDataSystem()
        {
            /* > ATG 1 START < */
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_NONE] = null;
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS] = new VARMAP_SafeVariable<GameOptionsStruct>(VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS, true, GameOptionsStruct.StaticParseFromBytes, GameOptionsStruct.StaticParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS] = new VARMAP_SafeVariable<ulong>(VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS, true, VARMAP_parsers.ulong_ParseFromBytes, VARMAP_parsers.ulong_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM] = new VARMAP_SafeVariable<Room>(VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM, true, VARMAP_parsers.Room_ParseFromBytes, VARMAP_parsers.Room_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED] = new VARMAP_SafeArray<MultiBitFieldStruct>(VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED, 2, true, MultiBitFieldStruct.StaticParseFromBytes, MultiBitFieldStruct.StaticParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER] = new VARMAP_SafeArray<CharacterType>(VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER, 4, true, VARMAP_parsers.CharacterType_ParseFromBytes, VARMAP_parsers.CharacterType_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT] = new VARMAP_SafeArray<int>(VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT, 3, true, VARMAP_parsers.int_ParseFromBytes, VARMAP_parsers.int_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS] = new VARMAP_Variable<Game_Status>(VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS, VARMAP_parsers.Game_Status_ParseFromBytes, VARMAP_parsers.Game_Status_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS] = new VARMAP_Variable<KeyStruct>(VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS, KeyStruct.StaticParseFromBytes, KeyStruct.StaticParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES] = new VARMAP_Variable<MousePropertiesStruct>(VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES, MousePropertiesStruct.StaticParseFromBytes, MousePropertiesStruct.StaticParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED] = new VARMAP_Variable<CharacterType>(VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED, VARMAP_parsers.CharacterType_ParseFromBytes, VARMAP_parsers.CharacterType_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE] = new VARMAP_Variable<bool>(VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_ACTIVE, VARMAP_parsers.bool_ParseFromBytes, VARMAP_parsers.bool_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN] = new VARMAP_SafeVariable<GameItem>(VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN, false, VARMAP_parsers.GameItem_ParseFromBytes, VARMAP_parsers.GameItem_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION] = new VARMAP_Array<ulong>(VARMAP_Variable_ID.VARMAP_ID_PLAYER_TRANSACTION, 3, VARMAP_parsers.ulong_ParseFromBytes, VARMAP_parsers.ulong_ParseToBytes, null);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL] = new VARMAP_Variable<bool>(VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL, VARMAP_parsers.bool_ParseFromBytes, VARMAP_parsers.bool_ParseToBytes, null);
            /* > ATG 1 END < */

        }
    }
}
