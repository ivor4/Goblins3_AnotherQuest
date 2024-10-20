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
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_PLAYER_POSITION = _GET_PLAYER_POSITION;
            SET_PLAYER_POSITION = _SET_PLAYER_POSITION;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            SET_PLAYER_SELECTED = _SET_PLAYER_SELECTED;
            REG_PLAYER_SELECTED = _REG_PLAYER_SELECTED;
            UNREG_PLAYER_SELECTED = _UNREG_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            MONO_REGISTER = _MONO_REGISTER;
            MOVE_PLAYER = _MOVE_PLAYER;
            SELECT_PLAYER = _SELECT_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            USE_ITEM = _USE_ITEM;
            INTERACT_PLAYER_ITEM = _INTERACT_PLAYER_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            SET_PLAYER_ANIMATION = _SET_PLAYER_ANIMATION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<Vector3Struct> GET_PLAYER_POSITION;
        public static SetVARMAPValueDelegate<Vector3Struct> SET_PLAYER_POSITION;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static SetVARMAPValueDelegate<CharacterType> SET_PLAYER_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> REG_PLAYER_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> UNREG_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        public static MOVE_PLAYER_DELEGATE MOVE_PLAYER;
        public static SELECT_PLAYER_DELEGATE SELECT_PLAYER;
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static USE_ITEM_DELEGATE USE_ITEM;
        public static INTERACT_PLAYER_ITEM_DELEGATE INTERACT_PLAYER_ITEM;
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        public static SET_PLAYER_ANIMATION_DELEGATE SET_PLAYER_ANIMATION;
        /* > ATG 3 END */
    }
}
