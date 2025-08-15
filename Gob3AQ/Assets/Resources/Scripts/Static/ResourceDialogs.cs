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
        private const string PHRASES_PATH = "Dialogs/PHRASES_CSV";


        private static PhraseInfo[] _cachedPhrases;
        private static Dictionary<DialogPhrase, int> _cachedPhrasesFinder;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedPhrases = new PhraseInfo[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);

            // Preload dialogs for the default room
            TextAsset textAsset = Resources.Load<TextAsset>(PHRASES_PATH);
            PreloadRoomPhrases(Room.ROOM_NONE, textAsset);
        }

        public static async Task PreloadRoomPhrasesAsync(Room room)
        {
            TextAsset textAsset;

            ResourceRequest resrq = Resources.LoadAsync<TextAsset>(PHRASES_PATH);

            await resrq;

            textAsset = resrq.asset as TextAsset;

            PreloadRoomPhrases(room, textAsset);
        }

        private static void PreloadRoomPhrases(Room room, TextAsset textAsset)
        {
            /* Empty cached dialogs */
            _cachedPhrasesFinder.Clear();

            ReadOnlySpan<string> rows = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            rows = rows[1..]; /* Skip header row */

            DialogPhrase iteratedPhrase = DialogPhrase.PHRASE_NONE;
            int storedIndex = 0;


            foreach (string row in rows)
            {
                ReadOnlySpan<string> columns = row.Split(',');
                columns = columns[2..];

                bool useAlways = columns[0][0] == 'T';
                int sound = int.Parse(columns[2]);
                DialogAnimation animation = (DialogAnimation)int.Parse(columns[4]);

                if (useAlways)
                {
                    _cachedPhrases[storedIndex] = new(columns[1], columns[(int)_language + 5], sound, animation);
                    _cachedPhrasesFinder[iteratedPhrase] = storedIndex++;
                }

                ++iteratedPhrase;
            }

            Resources.UnloadAsset(textAsset);
        }

        public static ref readonly PhraseInfo GetPhraseInfo(DialogPhrase phraseType)
        {

            if(_cachedPhrasesFinder.TryGetValue(phraseType, out int storedIndex))
            {
                return ref _cachedPhrases[storedIndex];
            }
            else
            {
                Debug.LogWarning($"Phrase type {phraseType} not found in cached phrases.");
                return ref PhraseInfo.EMPTY;
            }
        }
    }
}