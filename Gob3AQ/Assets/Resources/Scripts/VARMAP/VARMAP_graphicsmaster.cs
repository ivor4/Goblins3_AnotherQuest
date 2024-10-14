using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;

namespace Gob3AQ.VARMAP.GraphicsMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GraphicsMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            GET_ITEM_MENU_ACTIVE = _GET_ITEM_MENU_ACTIVE;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            REG_PICKABLE_ITEM_CHOSEN = _REG_PICKABLE_ITEM_CHOSEN;
            UNREG_PICKABLE_ITEM_CHOSEN = _UNREG_PICKABLE_ITEM_CHOSEN;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        public static GetVARMAPValueDelegate<bool> GET_ITEM_MENU_ACTIVE;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> REG_PICKABLE_ITEM_CHOSEN;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameItem> UNREG_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
