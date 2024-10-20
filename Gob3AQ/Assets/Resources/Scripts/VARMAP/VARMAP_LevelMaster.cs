using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.LevelMaster
{
    public abstract class VARMAP_LevelMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_ITEM_MENU_ACTIVE = _GET_ITEM_MENU_ACTIVE;
            SET_ITEM_MENU_ACTIVE = _SET_ITEM_MENU_ACTIVE;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            SAVE_GAME = _SAVE_GAME;
            LOAD_ROOM = _LOAD_ROOM;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            LOADING_COMPLETED = _LOADING_COMPLETED;
            FREEZE_PLAY = _FREEZE_PLAY;
            NPC_REGISTER = _NPC_REGISTER;
            ITEM_REGISTER = _ITEM_REGISTER;
            ITEM_REMOVE_FROM_SCENE = _ITEM_REMOVE_FROM_SCENE;
            MONO_REGISTER = _MONO_REGISTER;
            WP_REGISTER = _WP_REGISTER;
            MOVE_PLAYER = _MOVE_PLAYER;
            SELECT_PLAYER = _SELECT_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            INTERACT_PLAYER_ITEM = _INTERACT_PLAYER_ITEM;
            GET_SCENARIO_ITEM_LIST = _GET_SCENARIO_ITEM_LIST;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            SET_PLAYER_ANIMATION = _SET_PLAYER_ANIMATION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<bool> GET_ITEM_MENU_ACTIVE;
        public static SetVARMAPValueDelegate<bool> SET_ITEM_MENU_ACTIVE;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static SAVE_GAME_DELEGATE SAVE_GAME;
        public static LOAD_ROOM_DELEGATE LOAD_ROOM;
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        public static LODING_COMPLETED_DELEGATE LOADING_COMPLETED;
        public static FREEZE_PLAY_DELEGATE FREEZE_PLAY;
        public static NPC_REGISTER_DELEGATE NPC_REGISTER;
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        public static ITEM_REMOVE_FROM_SCENE_DELEGATE ITEM_REMOVE_FROM_SCENE;
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        public static WP_REGISTER_DELEGATE WP_REGISTER;
        public static MOVE_PLAYER_DELEGATE MOVE_PLAYER;
        public static SELECT_PLAYER_DELEGATE SELECT_PLAYER;
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        public static INTERACT_PLAYER_ITEM_DELEGATE INTERACT_PLAYER_ITEM;
        public static GET_SCENARIO_ITEM_LIST_DELEGATE GET_SCENARIO_ITEM_LIST;
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        public static SET_PLAYER_ANIMATION_DELEGATE SET_PLAYER_ANIMATION;
        /* > ATG 3 END */
    }
}
