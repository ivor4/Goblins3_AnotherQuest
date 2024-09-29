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
        /* > ATG 1 END < */

        /* All SERVICE Links */
        /* > ATG 2 START < */
        protected static START_GAME_DELEGATE _START_GAME;
        protected static LOAD_ROOM_DELEGATE _LOAD_ROOM;
        protected static EXIT_GAME_DELEGATE _EXIT_GAME;
        protected static LODING_COMPLETED_DELEGATE _LOADING_COMPLETED;
        protected static FREEZE_PLAY_DELEGATE _FREEZE_PLAY;
        protected static NPC_REGISTER_SERVICE _NPC_REGISTER;
        protected static MONO_REGISTER_SERVICE _MONO_REGISTER;
        protected static MOVE_PLAYER_SERVICE _MOVE_PLAYER;
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
