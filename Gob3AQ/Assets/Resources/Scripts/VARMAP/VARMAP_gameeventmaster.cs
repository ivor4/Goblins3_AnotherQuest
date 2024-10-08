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
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            TAKE_ITEM_EVENT = _TAKE_ITEM_EVENT;
            RETAKE_ITEM_EVENT = _RETAKE_ITEM_EVENT;
            IS_ITEM_TAKEN_FIRST = _IS_ITEM_TAKEN_FIRST;
            IS_ITEM_TAKEN = _IS_ITEM_TAKEN;
            GET_ITEM_INTERACTION = _GET_ITEM_INTERACTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static TAKE_ITEM_EVENT_DELEGATE TAKE_ITEM_EVENT;
        public static RETAKE_ITEM_EVENT_DELEGATE RETAKE_ITEM_EVENT;
        public static IS_ITEM_TAKEN_FIRST_DELEGATE IS_ITEM_TAKEN_FIRST;
        public static IS_ITEM_TAKEN_DELEGATE IS_ITEM_TAKEN;
        public static GET_ITEM_INTERACTION_DELEGATE GET_ITEM_INTERACTION;
        /* > ATG 3 END */
    }
}
