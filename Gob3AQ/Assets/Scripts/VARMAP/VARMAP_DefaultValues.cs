using MVerse.VARMAP.Types;
using MVerse.VARMAP.Enum;
using MVerse.VARMAP.Variable;
using System.IO;
using UnityEngine;

namespace MVerse.VARMAP.DefaultValues
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
            ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_POWERS]).InitializeListElems(false);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS]).SetValue(0UL);
            ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue(Room.NONE);
            ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL]).SetValue(3);
            ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL]).SetValue(3);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED]).InitializeListElems(0UL);
            ((VARMAP_Variable_Interface<ulong>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).InitializeListElems(0UL);
            ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS]).InitializeListElems(0);
            ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD]).SetValue(false);
            ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_ACTIVE]).SetValue(false);
            ((VARMAP_Variable_Interface<float>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_TRANSITION_PROGRESS]).SetValue(0f);
            ((VARMAP_Variable_Interface<OtherWorldMode>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_OTHER_WORLD_MODE]).SetValue(OtherWorldMode.OTHER_WORLD_OBSERVE);
            ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).SetValue(Game_Status.GAME_STATUS_STOPPED);
            ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).SetValue(KeyStruct_Default);
            ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).SetValue(MouseProperties_Default);
            ((VARMAP_Variable_Interface<Vector3Struct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_POSITION]).SetValue(Vector3Struct_Default);
            ((VARMAP_Variable_Interface<byte>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_BOSS_STEP]).SetValue(0);
            /* > ATG 1 END < */
        }


    }
}