using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
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


        private static DialogSenderAndMsg[] _cachedDialogs;
        private static Dictionary<DialogType, int> _cachedDialogsFinder;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedDialogs = new DialogSenderAndMsg[GameFixedConfig.MAX_CACHED_DIALOGS];
            _cachedDialogsFinder = new(GameFixedConfig.MAX_CACHED_DIALOGS);

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
            _cachedDialogsFinder.Clear();

            ReadOnlySpan<string> rows = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            rows = rows[1..]; /* Skip header row */

            DialogType iteratedDialog = DialogType.DIALOG_NONE;
            int storedIndex = 0;


            foreach (string row in rows)
            {
                ReadOnlySpan<string> columns = row.Split(',');
                columns = columns[3..];

                int parsedRoom = int.Parse(columns[0]);

                if (((int)room == parsedRoom) || (parsedRoom == (int)Room.ROOM_NONE))
                {
                    _cachedDialogs[storedIndex] = new(columns[1], columns[(int)_language + 2]);
                    _cachedDialogsFinder[iteratedDialog] = storedIndex++;
                }

                ++iteratedDialog;
            }

            Resources.UnloadAsset(textAsset);
        }

        public static ref readonly DialogSenderAndMsg GetDialogText(DialogType dialogType)
        {

            if(_cachedDialogsFinder.TryGetValue(dialogType, out int storedIndex))
            {
                return ref _cachedDialogs[storedIndex];
            }
            else
            {
                Debug.LogWarning($"Dialog type {dialogType} not found in cached dialogs.");
                return ref DialogSenderAndMsg.EMPTY;
            }
        }
    }
}