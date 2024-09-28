using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;
using UnityEngine;

namespace MVerse.VARMAP.GameEventMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GameEventMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_POWERS = _GET_ELEM_POWERS;
            GET_SIZE_POWERS = _GET_SIZE_POWERS;
            GET_ARRAY_POWERS = _GET_ARRAY_POWERS;
            GET_LIFE_TOTAL = _GET_LIFE_TOTAL;
            SET_LIFE_TOTAL = _SET_LIFE_TOTAL;
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            SET_ELEM_EVENTS_OCCURRED = _SET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            SET_ARRAY_EVENTS_OCCURRED = _SET_ARRAY_EVENTS_OCCURRED;
            GET_ELEM_SELECTED_CHARMS = _GET_ELEM_SELECTED_CHARMS;
            GET_SIZE_SELECTED_CHARMS = _GET_SIZE_SELECTED_CHARMS;
            GET_ARRAY_SELECTED_CHARMS = _GET_ARRAY_SELECTED_CHARMS;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<bool> GET_ELEM_POWERS;
        public static GetVARMAPArraySizeDelegate GET_SIZE_POWERS;
        public static GetVARMAPArrayDelegate<bool> GET_ARRAY_POWERS;
        public static GetVARMAPValueDelegate<byte> GET_LIFE_TOTAL;
        public static SetVARMAPValueDelegate<byte> SET_LIFE_TOTAL;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<ulong> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<ulong> SET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPArrayElemValueDelegate<byte> GET_ELEM_SELECTED_CHARMS;
        public static GetVARMAPArraySizeDelegate GET_SIZE_SELECTED_CHARMS;
        public static GetVARMAPArrayDelegate<byte> GET_ARRAY_SELECTED_CHARMS;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
