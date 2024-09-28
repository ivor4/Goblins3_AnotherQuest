using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;
using MVerse.VARMAP.Variable;
using UnityEngine;

namespace MVerse.VARMAP
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
        protected static GetVARMAPArrayElemValueDelegate<bool> _GET_ELEM_POWERS;
        protected static SetVARMAPArrayElemValueDelegate<bool> _SET_ELEM_POWERS;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_POWERS;
        protected static GetVARMAPArrayDelegate<bool> _GET_ARRAY_POWERS;
        protected static SetVARMAPArrayDelegate<bool> _SET_ARRAY_POWERS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_POWERS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_POWERS;
        protected static GetVARMAPValueDelegate<ulong> _GET_ELAPSED_TIME_MS;
        protected static SetVARMAPValueDelegate<ulong> _SET_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_ELAPSED_TIME_MS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_ELAPSED_TIME_MS;
        protected static GetVARMAPValueDelegate<Room> _GET_ACTUAL_ROOM;
        protected static SetVARMAPValueDelegate<Room> _SET_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _REG_ACTUAL_ROOM;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<Room> _UNREG_ACTUAL_ROOM;
        protected static GetVARMAPValueDelegate<byte> _GET_LIFE_TOTAL;
        protected static SetVARMAPValueDelegate<byte> _SET_LIFE_TOTAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _REG_LIFE_TOTAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _UNREG_LIFE_TOTAL;
        protected static GetVARMAPValueDelegate<byte> _GET_LIFE_ACTUAL;
        protected static SetVARMAPValueDelegate<byte> _SET_LIFE_ACTUAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _REG_LIFE_ACTUAL;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _UNREG_LIFE_ACTUAL;
        protected static GetVARMAPArrayElemValueDelegate<ulong> _GET_ELEM_ITEMS_COLLECTED;
        protected static SetVARMAPArrayElemValueDelegate<ulong> _SET_ELEM_ITEMS_COLLECTED;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_ITEMS_COLLECTED;
        protected static GetVARMAPArrayDelegate<ulong> _GET_ARRAY_ITEMS_COLLECTED;
        protected static SetVARMAPArrayDelegate<ulong> _SET_ARRAY_ITEMS_COLLECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_ITEMS_COLLECTED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_ITEMS_COLLECTED;
        protected static GetVARMAPArrayElemValueDelegate<ulong> _GET_ELEM_EVENTS_OCCURRED;
        protected static SetVARMAPArrayElemValueDelegate<ulong> _SET_ELEM_EVENTS_OCCURRED;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_EVENTS_OCCURRED;
        protected static GetVARMAPArrayDelegate<ulong> _GET_ARRAY_EVENTS_OCCURRED;
        protected static SetVARMAPArrayDelegate<ulong> _SET_ARRAY_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _REG_EVENTS_OCCURRED;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<ulong> _UNREG_EVENTS_OCCURRED;
        protected static GetVARMAPArrayElemValueDelegate<byte> _GET_ELEM_SELECTED_CHARMS;
        protected static SetVARMAPArrayElemValueDelegate<byte> _SET_ELEM_SELECTED_CHARMS;
        protected static GetVARMAPArraySizeDelegate _GET_SIZE_SELECTED_CHARMS;
        protected static GetVARMAPArrayDelegate<byte> _GET_ARRAY_SELECTED_CHARMS;
        protected static SetVARMAPArrayDelegate<byte> _SET_ARRAY_SELECTED_CHARMS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _REG_SELECTED_CHARMS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _UNREG_SELECTED_CHARMS;
        protected static GetVARMAPValueDelegate<bool> _GET_OTHER_WORLD;
        protected static SetVARMAPValueDelegate<bool> _SET_OTHER_WORLD;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_OTHER_WORLD;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_OTHER_WORLD;
        protected static GetVARMAPValueDelegate<bool> _GET_OTHER_WORLD_TRANSITION_ACTIVE;
        protected static SetVARMAPValueDelegate<bool> _SET_OTHER_WORLD_TRANSITION_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _REG_OTHER_WORLD_TRANSITION_ACTIVE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<bool> _UNREG_OTHER_WORLD_TRANSITION_ACTIVE;
        protected static GetVARMAPValueDelegate<float> _GET_OTHER_WORLD_TRANSITION_PROGRESS;
        protected static SetVARMAPValueDelegate<float> _SET_OTHER_WORLD_TRANSITION_PROGRESS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<float> _REG_OTHER_WORLD_TRANSITION_PROGRESS;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<float> _UNREG_OTHER_WORLD_TRANSITION_PROGRESS;
        protected static GetVARMAPValueDelegate<OtherWorldMode> _GET_OTHER_WORLD_MODE;
        protected static SetVARMAPValueDelegate<OtherWorldMode> _SET_OTHER_WORLD_MODE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> _REG_OTHER_WORLD_MODE;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> _UNREG_OTHER_WORLD_MODE;
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
        protected static GetVARMAPValueDelegate<byte> _GET_BOSS_STEP;
        protected static SetVARMAPValueDelegate<byte> _SET_BOSS_STEP;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _REG_BOSS_STEP;
        protected static ReUnRegisterVARMAPValueChangeEventDelegate<byte> _UNREG_BOSS_STEP;
        /* > ATG 1 END < */

        /* All SERVICE Links */
        /* > ATG 2 START < */
        protected static START_GAME_DELEGATE _START_GAME;
        protected static LOAD_OBF_DELEGATE _LOAD_OBF;
        protected static LOAD_ROOM_DELEGATE _LOAD_ROOM;
        protected static EXIT_GAME_DELEGATE _EXIT_GAME;
        protected static LODING_COMPLETED_DELEGATE _LOADING_COMPLETED;
        protected static CHANGE_OTHER_WORLD_DELEGATE _CHANGE_OTHER_WORLD;
        protected static FREEZE_PLAY_DELEGATE _FREEZE_PLAY;
        protected static ENEMY_REGISTER_SERVICE _ENEMY_REGISTER;
        protected static MONO_REGISTER_SERVICE _MONO_REGISTER;
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
