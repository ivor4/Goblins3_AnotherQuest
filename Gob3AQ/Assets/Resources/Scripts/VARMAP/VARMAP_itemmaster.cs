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
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            SET_ELEM_PICKABLE_ITEM_OWNER = _SET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            SET_ARRAY_PICKABLE_ITEM_OWNER = _SET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            SET_PICKABLE_ITEM_CHOSEN = _SET_PICKABLE_ITEM_CHOSEN;
            ITEM_REGISTER = _ITEM_REGISTER;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            TAKE_ITEM_FROM_SCENE_EVENT = _TAKE_ITEM_FROM_SCENE_EVENT;
            TAKE_ITEM = _TAKE_ITEM;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            IS_ITEM_OWNED = _IS_ITEM_OWNED;
            GET_SCENARIO_ITEM_LIST = _GET_SCENARIO_ITEM_LIST;
            GET_PICKED_ITEM_LIST = _GET_PICKED_ITEM_LIST;
            SELECT_PICKABLE_ITEM = _SELECT_PICKABLE_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<GamePickableItem> GET_PICKABLE_ITEM_CHOSEN;
        public static SetVARMAPValueDelegate<GamePickableItem> SET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static TAKE_ITEM_FROM_SCENE_EVENT_DELEGATE TAKE_ITEM_FROM_SCENE_EVENT;
        public static TAKE_ITEM_DELEGATE TAKE_ITEM;
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        public static IS_ITEM_OWNED_DELEGATE IS_ITEM_OWNED;
        public static GET_SCENARIO_ITEM_LIST_DELEGATE GET_SCENARIO_ITEM_LIST;
        public static GET_PICKED_ITEM_LIST_DELEGATE GET_PICKED_ITEM_LIST;
        public static SELECT_PICKABLE_ITEM_DELEGATE SELECT_PICKABLE_ITEM;
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /* > ATG 3 END */
    }
}
