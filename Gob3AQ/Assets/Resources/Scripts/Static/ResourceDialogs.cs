using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogsClass
    {
        private const string DIALOGS_PATH = "Dialogs/DIALOGS_CSV";


        private static Dictionary<DialogType, string> _cachedDialogs;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedDialogs = new Dictionary<DialogType, string>(GameFixedConfig.MAX_CACHED_DIALOGS);

            // Preload dialogs for the default room
            TextAsset textAsset = Resources.Load<TextAsset>(DIALOGS_PATH);
            PreloadRoomDialogs(Room.ROOM_NONE, textAsset);
        }

        public static async Task PreloadRoomDialogsAsync(Room room)
        {
            TextAsset textAsset;

            ResourceRequest resrq = Resources.LoadAsync<TextAsset>(DIALOGS_PATH);

            await resrq;

            textAsset = resrq.asset as TextAsset;

            PreloadRoomDialogs(room, textAsset);
        }

        private static void PreloadRoomDialogs(Room room, TextAsset textAsset)
        {
            /* Empty cached dialogs */
            _cachedDialogs.Clear();

            ReadOnlySpan<string> rows = textAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            rows = rows[1..]; /* Skip header row */

            DialogType iteratedDialog = DialogType.DIALOG_NONE;


            foreach (string row in rows)
            {
                ReadOnlySpan<string> columns = row.Split(',');
                columns = columns[3..];

                int parsedRoom = int.Parse(columns[0]);

                if (((int)room == parsedRoom) || (parsedRoom == (int)Room.ROOM_NONE))
                {
                    _cachedDialogs[iteratedDialog] = columns[(int)_language + 1];
                }

                ++iteratedDialog;
            }

            Resources.UnloadAsset(textAsset);
        }

        public static string GetDialogText(DialogType dialogType)
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