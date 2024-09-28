using MVerse.VARMAP.Types;
using MVerse.VARMAP.Types.Delegates;

namespace MVerse.VARMAP.PhysicsMaster
{
    public abstract class VARMAP_PhysicsMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_OTHER_WORLD = _GET_OTHER_WORLD;
            REG_OTHER_WORLD = _REG_OTHER_WORLD;
            UNREG_OTHER_WORLD = _UNREG_OTHER_WORLD;
            GET_OTHER_WORLD_TRANSITION_ACTIVE = _GET_OTHER_WORLD_TRANSITION_ACTIVE;
            GET_OTHER_WORLD_TRANSITION_PROGRESS = _GET_OTHER_WORLD_TRANSITION_PROGRESS;
            GET_OTHER_WORLD_MODE = _GET_OTHER_WORLD_MODE;
            REG_OTHER_WORLD_MODE = _REG_OTHER_WORLD_MODE;
            UNREG_OTHER_WORLD_MODE = _UNREG_OTHER_WORLD_MODE;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<bool> REG_OTHER_WORLD;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<bool> UNREG_OTHER_WORLD;
        public static GetVARMAPValueDelegate<bool> GET_OTHER_WORLD_TRANSITION_ACTIVE;
        public static GetVARMAPValueDelegate<float> GET_OTHER_WORLD_TRANSITION_PROGRESS;
        public static GetVARMAPValueDelegate<OtherWorldMode> GET_OTHER_WORLD_MODE;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> REG_OTHER_WORLD_MODE;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<OtherWorldMode> UNREG_OTHER_WORLD_MODE;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /* > ATG 3 END */
    }
}
