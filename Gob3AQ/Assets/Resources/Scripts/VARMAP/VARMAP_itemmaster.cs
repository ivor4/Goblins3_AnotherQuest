using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.ItemMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_ItemMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_ITEMS_COLLECTED = _GET_ELEM_ITEMS_COLLECTED;
            SET_ELEM_ITEMS_COLLECTED = _SET_ELEM_ITEMS_COLLECTED;
            GET_SIZE_ITEMS_COLLECTED = _GET_SIZE_ITEMS_COLLECTED;
            GET_ARRAY_ITEMS_COLLECTED = _GET_ARRAY_ITEMS_COLLECTED;
            SET_ARRAY_ITEMS_COLLECTED = _SET_ARRAY_ITEMS_COLLECTED;
            MONO_REGISTER = _MONO_REGISTER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_ITEMS_COLLECTED;
        public static SetVARMAPArrayElemValueDelegate<ulong> SET_ELEM_ITEMS_COLLECTED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_ITEMS_COLLECTED;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_ITEMS_COLLECTED;
        public static SetVARMAPArrayDelegate<ulong> SET_ARRAY_ITEMS_COLLECTED;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        public static MONO_REGISTER_SERVICE MONO_REGISTER;
        /* > ATG 3 END */
    }
}
