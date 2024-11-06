using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Items;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.NPCMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_NPCMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            NPC_REGISTER = _NPC_REGISTER;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        public static NPC_REGISTER_DELEGATE NPC_REGISTER;
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /* > ATG 3 END */
    }
}
