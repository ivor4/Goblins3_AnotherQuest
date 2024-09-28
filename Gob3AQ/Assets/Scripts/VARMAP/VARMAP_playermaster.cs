using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;
using UnityEngine;

namespace MVerse.VARMAP.PlayerMaster
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
            GET_ELEM_POWERS = _GET_ELEM_POWERS;
            GET_SIZE_POWERS = _GET_SIZE_POWERS;
            GET_ARRAY_POWERS = _GET_ARRAY_POWERS;
            GET_LIFE_ACTUAL = _GET_LIFE_ACTUAL;
            SET_LIFE_ACTUAL = _SET_LIFE_ACTUAL;
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_ELEM_SELECTED_CHARMS = _GET_ELEM_SELECTED_CHARMS;
            GET_SIZE_SELECTED_CHARMS = _GET_SIZE_SELECTED_CHARMS;
            GET_ARRAY_SELECTED_CHARMS = _GET_ARRAY_SELECTED_CHARMS;
            GET_OTHER_WORLD = _GET_OTHER_WORLD;
            GET_OTHER_WORLD_TRANSITION_ACTIVE = _GET_OTHER_WORLD_TRANSITION_ACTIVE;
            GET_OTHER_WORLD_TRANSITION_PROGRESS = _GET_OTHER_WORLD_TRANSITION_PROGRESS;
            GET_OTHER_WORLD_MODE = _GET_OTHER_WORLD_MODE;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            SET_PLAYER_POSITION = _SET_PLAYER_POSITION;
            CHANGE_OTHER_WORLD = _CHANGE_OTHER_WORLD;
            MONO_REGISTER = _MONO_REGISTER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<bool> GET_ELEM_POWERS;
        public static GetVARMAPArraySizeDelegate GET_SIZE_POWERS;
        public static GetVARMAPArrayDelegate<bool> GET_ARRAY_POWERS;
        public static GetVARMAPValueDelegate<byte> GET_LIFE_ACTUAL;
        public static SetVARMAPValueDelegate<byte> SET_LIFE_ACTUAL;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPArrayElemValueDelegate<byte> GET_ELEM_SELECTED_CHARMS;
        public static GetVARMAPArraySizeDelegate GET_SIZE_SELECTED_CHARMS;
        public static GetVARMAPArrayDelegate<byte> GET_ARRAY_SELECTED_CHARMS;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD_TRANSITION_ACTIVE;
        public static GetVARMAPValueDelegate<float> GET_OTHER_WORLD_TRANSITION_PROGRESS;
        public static GetVARMAPValueDelegate<OtherWorldMode> GET_OTHER_WORLD_MODE;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        public static SetVARMAPValueDelegate<Vector3Struct> SET_PLAYER_POSITION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static CHANGE_OTHER_WORLD_DELEGATE CHANGE_OTHER_WORLD;
        public static MONO_REGISTER_SERVICE MONO_REGISTER;
        /* > ATG 3 END */
    }
}
