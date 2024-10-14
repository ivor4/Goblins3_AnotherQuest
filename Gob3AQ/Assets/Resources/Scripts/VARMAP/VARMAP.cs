using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.VARMAP.Variable;
using UnityEngine;

namespace Gob3AQ.VARMAP
{
    /// <summary>
    /// Unreachable from the outside mother class
    /// </summary>
    public abstract class VARMAP
    {
        /* All GET/SET/REG/UNREG Links */
        /* > ATG 1 START < */
        protected static GetVARMAPValueDelegate<GameOptionsStruct> _GET_GAME_OPTIONS;
        protected static SetVARMAPValueDelegate<GameOptionsStruct> _SET_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _REG_GAME_OPTIONS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> _UNREG_GAME_OPTIONS;
        protected static GetVARMAPValueDelegate<ulong> _GET_ELAPSED_TIME_MS;
        protected static SetVARMAPValueDelegate<ulong> _SET_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_ELAPSED_TIME_MS;
        protected static GetVARMAPValueDelegate<Room> _GET_ACTUAL_ROOM;
        protected static SetVARMAPValueDelegate<Room> _SET_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _REG_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _UNREG_ACTUAL_ROOM;
        protected static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _GET_ELEM_EVENTS_OCCURRED;
        protected static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> _SET_ELEM_EVENTS_OCCURRED;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_EVENTS_OCCURRED;
        protected static GetVARMAPArrayDelegate<MultiBitFieldStruct> _GET_ARRAY_EVENTS_OCCURRED;
        protected static SetVARMAPArrayDelegate<MultiBitFieldStruct> _SET_ARRAY_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _REG_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MultiBitFieldStruct> _UNREG_EVENTS_OCCURRED;
        protected static GetVARMAPArrayElemValueDelegate<CharacterType> _GET_ELEM_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayElemValueDelegate<CharacterType> _SET_ELEM_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPArrayDelegate<CharacterType> _GET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static SetVARMAPArrayDelegate<CharacterType> _SET_ARRAY_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PICKABLE_ITEM_OWNER;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PICKABLE_ITEM_OWNER;
        protected static GetVARMAPValueDelegate<Game_Status> _GET_GAMESTATUS;
        protected static SetVARMAPValueDelegate<Game_Status> _SET_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _REG_GAMESTATUS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> _UNREG_GAMESTATUS;
        protected static GetVARMAPValueDelegate<KeyStruct> _GET_PRESSED_KEYS;
        protected static SetVARMAPValueDelegate<KeyStruct> _SET_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _REG_PRESSED_KEYS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<KeyStruct> _UNREG_PRESSED_KEYS;
        protected static GetVARMAPValueDelegate<MousePropertiesStruct> _GET_MOUSE_PROPERTIES;
        protected static SetVARMAPValueDelegate<MousePropertiesStruct> _SET_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _REG_MOUSE_PROPERTIES;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<MousePropertiesStruct> _UNREG_MOUSE_PROPERTIES;
        protected static GetVARMAPValueDelegate<Vector3Struct> _GET_PLAYER_POSITION;
        protected static SetVARMAPValueDelegate<Vector3Struct> _SET_PLAYER_POSITION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Vector3Struct> _REG_PLAYER_POSITION;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Vector3Struct> _UNREG_PLAYER_POSITION;
        protected static GetVARMAPValueDelegate<CharacterType> _GET_PLAYER_SELECTED;
        protected static SetVARMAPValueDelegate<CharacterType> _SET_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _REG_PLAYER_SELECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> _UNREG_PLAYER_SELECTED;
        protected static GetVARMAPValueDelegate<bool> _GET_ITEM_MENU_ACTIVE;
        protected static SetVARMAPValueDelegate<bool> _SET_ITEM_MENU_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_ITEM_MENU_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_ITEM_MENU_ACTIVE;
        protected static GetVARMAPValueDelegate<GamePickableItem> _GET_PICKABLE_ITEM_CHOSEN;
        protected static SetVARMAPValueDelegate<GamePickableItem> _SET_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GamePickableItem> _REG_PICKABLE_ITEM_CHOSEN;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<GamePickableItem> _UNREG_PICKABLE_ITEM_CHOSEN;
        /* > ATG 1 END < */

        /* All SERVICE Links */
        /* > ATG 2 START < */
        protected static START_GAME_DELEGATE _START_GAME;
        protected static LOAD_ROOM_DELEGATE _LOAD_ROOM;
        protected static EXIT_GAME_DELEGATE _EXIT_GAME;
        protected static LODING_COMPLETED_DELEGATE _LOADING_COMPLETED;
        protected static FREEZE_PLAY_DELEGATE _FREEZE_PLAY;
        protected static NPC_REGISTER_DELEGATE _NPC_REGISTER;
        protected static ITEM_REGISTER_DELEGATE _ITEM_REGISTER;
        protected static MONO_REGISTER_DELEGATE _MONO_REGISTER;
        protected static WP_REGISTER_DELEGATE _WP_REGISTER;
        protected static MOVE_PLAYER_DELEGATE _MOVE_PLAYER;
        protected static SELECT_PLAYER_DELEGATE _SELECT_PLAYER;
        protected static GET_PLAYER_LIST_DELEGATE _GET_PLAYER_LIST;
        protected static GET_NEAREST_WP_DELEGATE _GET_NEAREST_WP;
        protected static IS_EVENT_OCCURRED_DELEGATE _IS_EVENT_OCCURRED;
        protected static COMMIT_EVENT_DELEGATE _COMMIT_EVENT;
        protected static TAKE_ITEM_FROM_SCENE_EVENT_DELEGATE _TAKE_ITEM_FROM_SCENE_EVENT;
        protected static TAKE_ITEM_DELEGATE _TAKE_ITEM;
        protected static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE _IS_ITEM_TAKEN_FROM_SCENE;
        protected static INTERACT_PLAYER_ITEM_DELEGATE _INTERACT_PLAYER_ITEM;
        protected static GET_ITEM_INTERACTION_DELEGATE _GET_ITEM_INTERACTION;
        protected static GET_SCENARIO_ITEM_LIST_DELEGATE _GET_SCENARIO_ITEM_LIST;
        protected static SELECT_PICKABLE_ITEM_DELEGATE _SELECT_PICKABLE_ITEM;
        protected static CANCEL_PICKABLE_ITEM_DELEGATE _CANCEL_PICKABLE_ITEM;
        protected static USE_ITEM_DELEGATE _USE_ITEM;
        protected static EXIT_GAME_DELEGATE _LAST_SERVICE;
        /* > ATG 2 END < */
        
        



        /// <summary>
        /// All VARMAP Data
        /// </summary>
        protected static VARMAP_Variable_Indexable[] DATA;

        /// <summary>
        /// Memory security concept.
        /// </summary>
        protected static uint[] RUBISH_BIN;

    }



}
