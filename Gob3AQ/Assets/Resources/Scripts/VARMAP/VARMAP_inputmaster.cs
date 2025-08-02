using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.NPCMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.InputMaster;

namespace Gob3AQ.VARMAP.InputMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_InputMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            REG_GAME_OPTIONS = _REG_GAME_OPTIONS;
            UNREG_GAME_OPTIONS = _UNREG_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_SHADOW_PRESSED_KEYS = _GET_SHADOW_PRESSED_KEYS;
            SET_PRESSED_KEYS = _SET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_SHADOW_MOUSE_PROPERTIES = _GET_SHADOW_MOUSE_PROPERTIES;
            SET_MOUSE_PROPERTIES = _SET_MOUSE_PROPERTIES;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> REG_GAME_OPTIONS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> UNREG_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_SHADOW_PRESSED_KEYS;
        public static SetVARMAPValueDelegate<KeyStruct> SET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_SHADOW_MOUSE_PROPERTIES;
        public static SetVARMAPValueDelegate<MousePropertiesStruct> SET_MOUSE_PROPERTIES;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, NPCMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LateStartSubrsciptionService"/> </para> 
        /// </summary>
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        /* > ATG 3 END */
    }
}
