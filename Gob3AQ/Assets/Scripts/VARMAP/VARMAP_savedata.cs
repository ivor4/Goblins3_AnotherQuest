using UnityEditor;
using UnityEngine;
using MVerse.VARMAP.Enum;

namespace MVerse.VARMAP.SaveData
{
    public abstract class VARMAP_savedata : VARMAP
    {
        /// <summary>
        /// This array contains IDs of VARMAP which should be stored into savegame data and loaded
        /// </summary>
        public static readonly VARMAP_Variable_ID[] SAVE_IDS = new VARMAP_Variable_ID[]
        {
            /* > ATG 1 START < */
            VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS,
            VARMAP_Variable_ID.VARMAP_ID_POWERS,
            VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS,
            VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM,
            VARMAP_Variable_ID.VARMAP_ID_LIFE_TOTAL,
            VARMAP_Variable_ID.VARMAP_ID_LIFE_ACTUAL,
            VARMAP_Variable_ID.VARMAP_ID_ITEMS_COLLECTED,
            VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED,
            VARMAP_Variable_ID.VARMAP_ID_SELECTED_CHARMS,
            /* > ATG 1 END < */
        };

    }
}