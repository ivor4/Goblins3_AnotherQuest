using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.GameMenu
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GameMenu : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAME_OPTIONS = _GET_GAME_OPTIONS;
            SET_GAME_OPTIONS = _SET_GAME_OPTIONS;
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            GET_ITEM_MENU_ACTIVE = _GET_ITEM_MENU_ACTIVE;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            START_GAME = _START_GAME;
            EXIT_GAME = _EXIT_GAME;
            GET_PICKED_ITEM_LIST = _GET_PICKED_ITEM_LIST;
            PICK_PICKABLE_ITEM = _PICK_PICKABLE_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<GameOptionsStruct> GET_GAME_OPTIONS;
        public static SetVARMAPValueDelegate<GameOptionsStruct> SET_GAME_OPTIONS;
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        public static GetVARMAPValueDelegate<bool> GET_ITEM_MENU_ACTIVE;
        public static GetVARMAPValueDelegate<GamePickableItem> GET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static START_GAME_DELEGATE START_GAME;
        public static EXIT_GAME_DELEGATE EXIT_GAME;
        public static GET_PICKED_ITEM_LIST_DELEGATE GET_PICKED_ITEM_LIST;
        public static PICK_PICKABLE_ITEM_DELEGATE PICK_PICKABLE_ITEM;
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /* > ATG 3 END */
    }
}
