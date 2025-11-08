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
            ((VARMAP_Variable_Interface<Room>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM]).SetValue(Room.ROOM_NONE);
            ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED]).InitializeListElems(default);
            ((VARMAP_Variable_Interface<MultiBitFieldStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_UNLOCKED_MEMENTO]).InitializeListElems(default);
            ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER]).InitializeListElems(CharacterType.CHARACTER_NONE);
            ((VARMAP_Variable_Interface<int>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT]).InitializeListElems(-1);
            ((VARMAP_Variable_Interface<CameraDispositionStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_CAMERA_DISPOSITION]).SetValue(CameraDispositionStruct_Default);
            ((VARMAP_Variable_Interface<Game_Status>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS]).SetValue(Game_Status.GAME_STATUS_STOPPED);
            ((VARMAP_Variable_Interface<KeyStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS]).SetValue(KeyStruct_Default);
            ((VARMAP_Variable_Interface<MousePropertiesStruct>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES]).SetValue(MouseProperties_Default);
            ((VARMAP_Variable_Interface<CharacterType>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED]).SetValue(CharacterType.CHARACTER_NONE);
            ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN]).SetValue(GameItem.ITEM_NONE);
            ((VARMAP_Variable_Interface<GameItem>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_HOVER]).SetValue(GameItem.ITEM_NONE);
            ((VARMAP_Variable_Interface<UserInputInteraction>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_USER_INPUT_INTERACTION]).SetValue(UserInputInteraction.INPUT_INTERACTION_TAKE);
            ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_BEING_PROCESSED]).SetValue(false);
            ((VARMAP_Variable_Interface<bool>)DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL]).SetValue(false);
            /* > ATG 1 END < */
        }


    }
}