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
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            SET_PICKABLE_ITEM_CHOSEN = _SET_PICKABLE_ITEM_CHOSEN;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            ITEM_REGISTER = _ITEM_REGISTER;
            ITEM_REMOVE_FROM_SCENE = _ITEM_REMOVE_FROM_SCENE;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            TAKE_ITEM_FROM_SCENE_EVENT = _TAKE_ITEM_FROM_SCENE_EVENT;
            USE_ITEM = _USE_ITEM;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            GET_SCENARIO_ITEM_LIST = _GET_SCENARIO_ITEM_LIST;
            SELECT_PICKABLE_ITEM = _SELECT_PICKABLE_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            SET_PLAYER_ANIMATION = _SET_PLAYER_ANIMATION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayElemValueDelegate<CharacterType> SET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayDelegate<CharacterType> SET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static SetVARMAPValueDelegate<GameItem> SET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        public static ITEM_REMOVE_FROM_SCENE_DELEGATE ITEM_REMOVE_FROM_SCENE;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static TAKE_ITEM_FROM_SCENE_EVENT_DELEGATE TAKE_ITEM_FROM_SCENE_EVENT;
        public static USE_ITEM_DELEGATE USE_ITEM;
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        public static GET_SCENARIO_ITEM_LIST_DELEGATE GET_SCENARIO_ITEM_LIST;
        public static SELECT_PICKABLE_ITEM_DELEGATE SELECT_PICKABLE_ITEM;
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        public static SET_PLAYER_ANIMATION_DELEGATE SET_PLAYER_ANIMATION;
        /* > ATG 3 END */
    }
}
