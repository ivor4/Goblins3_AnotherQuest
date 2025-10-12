using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogsClass
    {
        private const string PHRASES_PATH = "Dialogs/PHRASES_CSV";
        private const string NAMES_PATH = "Dialogs/NAMES_CSV";


        private static PhraseContent[] _cachedPhrases;
        private static Dictionary<DialogPhrase, int> _cachedPhrasesFinder;
        private static string[] _cachedNames;
        private static Dictionary<NameType, int> _cachedNamesFinder;
        private static DialogLanguages _language;


        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedPhrases = new PhraseContent[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _cachedNames = new string[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedNamesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
        }

        public static IEnumerator PreloadRoomPhrasesCoroutine(Room room)
        {
            TextAsset textAsset;

            ResourceRequest resrq = Resources.LoadAsync<TextAsset>(PHRASES_PATH);

            yield return resrq;

            textAsset = resrq.asset as TextAsset;
            string[] phrases = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            resrq = Resources.LoadAsync<TextAsset>(NAMES_PATH);

            yield return resrq;

            textAsset = resrq.asset as TextAsset;
            string[] names = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            yield return Task.Run(() => PreloadRoomPhrases(room, phrases));
            yield return Task.Run(() => PreloadRoomNames(room, names));
        }

        private static void PreloadRoomPhrases(Room room, string[] lines)
        {
            /* Empty cached dialogs */
            _cachedPhrasesFinder.Clear();
            Array.Clear(_cachedPhrases, 0, _cachedPhrases.Length);

            int storedIndex = 0;

            /* Get phrases which use actual Room or Room.NONE */
            ReadOnlySpan<PhraseConfig> phraseConfigs = ResourceDialogsAtlasClass.PhraseConfigs;

            /* Get room info and its linked phrases */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ReadOnlySpan<DialogPhrase> roomPhrases = roomInfo.Phrases;


            for (int i = 0; i < roomPhrases.Length; i++)
            {
                DialogPhrase phrase = roomPhrases[i];
                ref readonly PhraseConfig phraseConfig = ref phraseConfigs[(int)phrase];

                /* Retrieve configuration for given phrase */
                ref readonly string row = ref lines[(int)phrase];
                ReadOnlySpan<string> columns = row.Split(',');

                _cachedPhrases[storedIndex] = new(phraseConfig, columns[(int)_language]);
                _cachedPhrasesFinder[phrase] = storedIndex++;
            }
        }

        private static void PreloadRoomNames(Room room, string[] lines)
        {
            /* Empty cached dialogs */
            _cachedNamesFinder.Clear();
            Array.Clear(_cachedNames, 0, _cachedNames.Length);

            int storedIndex = 0;

            /* Get room info and its linked phrases */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ReadOnlySpan<NameType> roomNames = roomInfo.Names;


            for (int i = 0; i < roomNames.Length; i++)
            {
                NameType nameType = roomNames[i];
                PreloadRoomNames_AddName(nameType, lines, ref storedIndex);
            }
        }

        private static void PreloadRoomNames_AddName(NameType name, string[] lines, ref int storedIndex)
        {
            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)name];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedNames[storedIndex] = columns[(int)_language];
            _cachedNamesFinder[name] = storedIndex++;
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

        public static ref readonly string GetName(NameType name)
        {
            if (_cachedNamesFinder.TryGetValue(name, out int storedIndex))
            {
                return ref _cachedNames[storedIndex];
            }
            else
            {
                Debug.LogError($"Name type {name} not found in cached names.");
                return ref string.Empty;
            }
        }


    }
}