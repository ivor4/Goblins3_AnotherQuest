using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceDialogsAtlas
{
    public static class ResourceDecisionsAtlasClass
    {
        public static ref readonly DialogConfig GetDialogConfig(DialogType dialog)
        {
            if((uint)dialog < (uint)DialogType.DIALOG_TOTAL)
            {
                return ref _DialogConfig[(int)dialog];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDialogConfig: Invalid dialog {dialog}");
                return ref DialogConfig.EMPTY;
            }
        }

        public static ref readonly DialogOptionConfig GetDialogOptionConfig(DialogOption option)
        {
            if((uint)option < (uint)DialogOption.DIALOG_OPTION_TOTAL)
            {
                return ref _DialogOptionConfig[(int)option];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDialogOptionConfig: Invalid dialog option {option}");
                return ref DialogOptionConfig.EMPTY;
            }
        }



        private static readonly DialogConfig[] _DialogConfig = new DialogConfig[(int)DialogType.DIALOG_TOTAL]
        {
            /* > ATG 1 START < */
            /* > ATG 1 END < */
        };

        private static readonly DialogOptionConfig[] _DialogOptionConfig = new DialogOptionConfig[(int)DialogOption.DIALOG_OPTION_TOTAL]
        {
            /* > ATG 2 START < */
            /* > ATG 2 END < */
        };
    }
}