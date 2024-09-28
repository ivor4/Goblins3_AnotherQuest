using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;

namespace MVerse.VARMAP.BossMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_BossMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_BOSS_STEP = _GET_BOSS_STEP;
            SET_BOSS_STEP = _SET_BOSS_STEP;
            REG_BOSS_STEP = _REG_BOSS_STEP;
            UNREG_BOSS_STEP = _UNREG_BOSS_STEP;
            MONO_REGISTER = _MONO_REGISTER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<byte> GET_BOSS_STEP;
        public static SetVARMAPValueDelegate<byte> SET_BOSS_STEP;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> REG_BOSS_STEP;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<byte> UNREG_BOSS_STEP;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static MONO_REGISTER_SERVICE MONO_REGISTER;
        /* > ATG 3 END */
    }
}
