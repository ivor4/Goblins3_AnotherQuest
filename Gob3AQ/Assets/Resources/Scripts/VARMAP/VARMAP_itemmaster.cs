using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.ItemMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_ItemMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_ITEMS_COLLECTED = _GET_ELEM_ITEMS_COLLECTED;
            SET_ELEM_ITEMS_COLLECTED = _SET_ELEM_ITEMS_COLLECTED;
            GET_SIZE_ITEMS_COLLECTED = _GET_SIZE_ITEMS_COLLECTED;
            GET_ARRAY_ITEMS_COLLECTED = _GET_ARRAY_ITEMS_COLLECTED;
            SET_ARRAY_ITEMS_COLLECTED = _SET_ARRAY_ITEMS_COLLECTED;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            ITEM_REGISTER = _ITEM_REGISTER;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            TAKE_ITEM_EVENT = _TAKE_ITEM_EVENT;
            RETAKE_ITEM_EVENT = _RETAKE_ITEM_EVENT;
            IS_ITEM_TAKEN_FIRST = _IS_ITEM_TAKEN_FIRST;
            IS_ITEM_TAKEN = _IS_ITEM_TAKEN;
            TAKE_ITEM_OBJECT = _TAKE_ITEM_OBJECT;
            GET_ITEM_LIST = _GET_ITEM_LIST;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_ITEMS_COLLECTED;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_ITEMS_COLLECTED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_ITEMS_COLLECTED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_ITEMS_COLLECTED;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_ITEMS_COLLECTED;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static TAKE_ITEM_EVENT_DELEGATE TAKE_ITEM_EVENT;
        public static RETAKE_ITEM_EVENT_DELEGATE RETAKE_ITEM_EVENT;
        public static IS_ITEM_TAKEN_FIRST_DELEGATE IS_ITEM_TAKEN_FIRST;
        public static IS_ITEM_TAKEN_DELEGATE IS_ITEM_TAKEN;
        public static TAKE_ITEM_OBJECT_DELEGATE TAKE_ITEM_OBJECT;
        public static GET_ITEM_LIST_DELEGATE GET_ITEM_LIST;
        /* > ATG 3 END */
    }
}
