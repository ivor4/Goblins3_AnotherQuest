using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Variable;
using System.IO;
using UnityEngine;

namespace Gob3AQ.VARMAP.DefaultValues
{
    public abstract partial class VARMAP_DefaultValues : VARMAP
    {

        /// <summary>
        /// Could be called more than once in Program execution. Reassigns values of VARMAP to defaults, respecting already created instances
        /// </summary>
        public static void SetDefaultValues()
        {
            /* > ATG 1 START < */
            ((VARMAP_Variable_Interface<GameOptionsStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS]).SetValue(GameOptionsStruct_Default);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).SetValue(0UL);
            ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue(Room.NONE);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).InitializeListElems(0UL);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).InitializeListElems(0UL);
            ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).SetValue(Game_Status.GAME_STATUS_STOPPED);
            ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).SetValue(KeyStruct_Default);
            ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).SetValue(MouseProperties_Default);
            ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).SetValue(Vector3Struct_Default);
            /* > ATG 1 END < */
        }


    }
}