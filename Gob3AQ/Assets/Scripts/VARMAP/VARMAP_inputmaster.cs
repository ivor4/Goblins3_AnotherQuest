using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;

namespace MVerse.VARMAP.InputMaster
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
            SET_PRESSED_KEYS = _SET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            SET_MOUSE_PROPERTIES = _SET_MOUSE_PROPERTIES;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> REG_GAME_OPTIONS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<GameOptionsStruct> UNREG_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static SetVARMAPValueDelegate<KeyStruct> SET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static SetVARMAPValueDelegate<MousePropertiesStruct> SET_MOUSE_PROPERTIES;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
