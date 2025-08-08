using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogs
    {
        private const string DIALOGS_PATH = "Dialogs/DIALOGS_CSV";

        private static Dictionary<DialogType, string> _cachedDialogs;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            PreloadRoomDialogs(Room.ROOM_NONE);
        }

        public static void PreloadRoomDialogs(Room room)
        {
            /* Empty cached dialogs */
            _cachedDialogs.Clear();

            // Preload logic for room dialogs
            // This could involve loading dialog data from resources or initializing dialog states
            TextAsset textAsset = Resources.Load<TextAsset>(DIALOGS_PATH);
            

            string[] rows = textAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            DialogType iteratedDialog = DialogType.DIALOG_NONE;

            foreach (string row in rows)
            {
                Span<string> columns = row.Split(',');
                columns = columns[3..];
                
                if((int)room == int.Parse(columns[0]))
                {
                    _cachedDialogs[iteratedDialog] = columns[(int)_language + 1];
                }

                ++iteratedDialog;
            }

            Resources.UnloadAsset(textAsset);
        }

        public static string GetDialogName(DialogType dialogType)
        {
            if (_cachedDialogs.ContainsKey(dialogType))
            {
                return _cachedDialogs[dialogType];
            }
            else
            {
                Debug.LogWarning($"Dialog {dialogType} not found in cached dialogs.");
                return string.Empty;
            }
        }
    }
}