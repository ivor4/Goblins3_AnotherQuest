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
            GET_NPC_LIST = _GET_NPC_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            INTERACT_PLAYER_NPC = _INTERACT_PLAYER_NPC;
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
        /// <summary> 
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// </summary>
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        /// <summary> 
        /// Registers an NPC in system
        /// </summary>
        public static NPC_REGISTER_DELEGATE NPC_REGISTER;
        /// <summary> 
        /// Gets a list of actual NPCs 
        /// </summary>
        public static GET_NPC_LIST_DELEGATE GET_NPC_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// </summary>
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        /// <summary> 
        /// Tells if an event is occurred
        /// </summary>
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// </summary>
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /// <summary> 
        /// Interacts player with NPC
        /// </summary>
        public static INTERACT_PLAYER_NPC_DELEGATE INTERACT_PLAYER_NPC;
        /* > ATG 3 END */
    }
}
