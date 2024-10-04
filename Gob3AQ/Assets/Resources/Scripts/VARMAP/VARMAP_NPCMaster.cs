using Gob3AQ.VARMAP.Types;
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
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            NPC_REGISTER = _NPC_REGISTER;
            MONO_REGISTER = _MONO_REGISTER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static NPC_REGISTER_DELEGATE NPC_REGISTER;
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        /* > ATG 3 END */
    }
}
