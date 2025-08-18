using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogsClass
    {
        private const string PHRASES_PATH = "Dialogs/PHRASES_CSV";


        private static PhraseContent[] _cachedPhrases;
        private static Dictionary<DialogPhrase, int> _cachedPhrasesFinder;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedPhrases = new PhraseContent[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);

            // Preload dialogs for the default room
            TextAsset textAsset = Resources.Load<TextAsset>(PHRASES_PATH);
            string[] lines = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Resources.UnloadAsset(textAsset);

            PreloadRoomPhrases(Room.ROOM_NONE, lines);
        }

        public static async Task PreloadRoomPhrasesAsync(Room room)
        {
            TextAsset textAsset;

            ResourceRequest resrq = Resources.LoadAsync<TextAsset>(PHRASES_PATH);

            await resrq;

            textAsset = resrq.asset as TextAsset;
            string[] lines = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Resources.UnloadAsset(textAsset);

            await Task.Run(() => PreloadRoomPhrases(room, lines));
        }

        private static void PreloadRoomPhrases(Room room, string[] lines)
        {
            /* Empty cached dialogs */
            _cachedPhrasesFinder.Clear();

            int storedIndex = 0;

            /* Get phrases which use actual Room or Room.NONE */
            ReadOnlySpan<PhraseConfig> phraseConfigs = ResourceDialogsAtlasClass.PhraseConfigs;

            for (int i = 0; i < phraseConfigs.Length; i++)
            {
                ref readonly PhraseConfig phraseConfig = ref phraseConfigs[i];
                if ((phraseConfig.room == room) || (phraseConfig.room == Room.ROOM_NONE))
                {
                    DialogPhrase actualPhrase = (DialogPhrase)i;

                    /* Retrieve configuration for given phrase */
                    ref readonly string row = ref lines[i];
                    ReadOnlySpan<string> columns = row.Split(',');

                    _cachedPhrases[storedIndex] = new(phraseConfig, columns[0], columns[(int)_language + 1]);
                    _cachedPhrasesFinder[actualPhrase] = storedIndex++;
                }
            }
        }

        

        public static ref readonly PhraseContent GetPhraseContent(DialogPhrase phraseType)
        {
            if(_cachedPhrasesFinder.TryGetValue(phraseType, out int storedIndex))
            {
                return ref _cachedPhrases[storedIndex];
            }
            else
            {
                Debug.LogError($"Phrase type {phraseType} not found in cached phrases.");
                return ref PhraseContent.EMPTY;
            }
        }

  
    }
}