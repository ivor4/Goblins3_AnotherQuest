using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;

namespace Gob3AQ.VARMAP.PlayerMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_PlayerMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            SET_PLAYER_POSITION = _SET_PLAYER_POSITION;
            GET_PLAYER_ID_SELECTED = _GET_PLAYER_ID_SELECTED;
            REG_PLAYER_ID_SELECTED = _REG_PLAYER_ID_SELECTED;
            UNREG_PLAYER_ID_SELECTED = _UNREG_PLAYER_ID_SELECTED;
            MONO_REGISTER = _MONO_REGISTER;
            MOVE_PLAYER = _MOVE_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        public static SetVARMAPValueDelegate<Vector3Struct> SET_PLAYER_POSITION;
        public static GetVARMAPValueDelegate<byte> GET_PLAYER_ID_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> REG_PLAYER_ID_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> UNREG_PLAYER_ID_SELECTED;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        public static MOVE_PLAYER_DELEGATE MOVE_PLAYER;
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        /* > ATG 3 END */
    }
}
