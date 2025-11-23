using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.InputMaster;

namespace Gob3AQ.VARMAP.InputMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_InputMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_SHADOW_PRESSED_KEYS = _GET_SHADOW_PRESSED_KEYS;
            SET_PRESSED_KEYS = _SET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_SHADOW_MOUSE_PROPERTIES = _GET_SHADOW_MOUSE_PROPERTIES;
            SET_MOUSE_PROPERTIES = _SET_MOUSE_PROPERTIES;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            KEY_SUBSCRIPTION = _KEY_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
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
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        public static LODING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
        /// Subscribe to determined key events to avoid polling
        /// <para> Owner: InputMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="InputMasterClass.KeySubscriptionService"/> </para> 
        /// </summary>
        public static KEY_SUBSCRIPTION_DELEGATE KEY_SUBSCRIPTION;
        /* > ATG 3 END */
    }
}
