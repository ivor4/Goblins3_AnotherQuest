using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.Enum;

namespace Gob3AQ.VARMAP.SaveData
{
    public sealed class VARMAP_savedata : VARMAP
    {
        /// <summary>
        /// This array contains IDs of VARMAP which should be stored into savegame data and loaded
        /// </summary>
        public static readonly VARMAP_Variable_ID[] SAVE_IDS = new VARMAP_Variable_ID[]
        {
            /* > ATG 1 START < */
            VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS,
            VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS,
            VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM,
            VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED,
            VARMAP_Variable_ID.VARMAP_ID_UNLOCKED_MEMENTO,
            VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER,
            VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT,
            VARMAP_Variable_ID.VARMAP_ID_CAMERA_DISPOSITION,
            /* > ATG 1 END < */
        };

    }
}