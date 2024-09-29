using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;

namespace Gob3AQ.VARMAP.GameEventMaster
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
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            SET_ELEM_EVENTS_OCCURRED = _SET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            SET_ARRAY_EVENTS_OCCURRED = _SET_ARRAY_EVENTS_OCCURRED;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<ulong> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<ulong> SET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
