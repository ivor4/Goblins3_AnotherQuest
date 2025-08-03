using Gob3AQ.VARMAP.Types;

namespace Gob3AQ.Dialogs
{
    public class DialogsClass
    {
        public static string GetDialogName(DialogType dialogType)
        {
            switch (dialogType)
            {
                case DialogType.DIALOG_NONE:
                    return "None";
                case DialogType.DIALOG_NONSENSE:
                    return "Nonsense";
                default:
                    return "Unknown";
            }
        }
    }
}