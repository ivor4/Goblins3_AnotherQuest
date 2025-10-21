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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogsClass
    {
        private const string PHRASES_PATH = "PHRASES_CSV";
        private const string NAMES_PATH = "NAMES_CSV";

        private static NameType[] _fixedNamesArray;
        private static NameType[] _namesToLoadArray;
        private static DialogPhrase[] _fixedPhrasesArray;
        private static DialogPhrase[] _phrasesToLoadArray;
        private static Dictionary<DialogPhrase, int> _cachedPhrasesFinder;
        private static Dictionary<NameType, int> _cachedNamesFinder;
        private static PhraseContent[] _cachedPhrasesArray;
        private static string[] _cachedNamesArray;
        private static DialogLanguages _language;

        private static int _fixedPhrasesToLoad;
        private static int _fixedNamesToLoad;
        private static int _namesToLoad;
        private static int _phrasesToLoad;
        private static int _cachedNames;
        private static int _cachedPhrases;



        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _cachedNamesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _fixedPhrasesArray = new DialogPhrase[GameFixedConfig.MAX_FIXED_PHRASES_TO_LOAD];
            _fixedNamesArray = new NameType[GameFixedConfig.MAX_FIXED_NAMES_TO_LOAD];
            _phrasesToLoadArray = new DialogPhrase[GameFixedConfig.MAX_CACHED_PHRASES];
            _namesToLoadArray = new NameType[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedPhrasesArray = new PhraseContent[GameFixedConfig.MAX_CACHED_PHRASES];
            _cachedNamesArray = new string[GameFixedConfig.MAX_CACHED_PHRASES];

            _fixedNamesToLoad = 0;

            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_CHAR_MAIN;
            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_CHAR_PARROT;
            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_CHAR_SNAKE;
            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_INTERACTION_TAKE;
            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_INTERACTION_TALK;
            _fixedNamesArray[_fixedNamesToLoad++] = NameType.NAME_INTERACTION_OBSERVE;

            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(ItemsInteractionsClass.GetItemFromPickable(i));
                _fixedNamesArray[_fixedNamesToLoad++] = itemInfo.name;
            }

            _fixedPhrasesToLoad = 0;
            _fixedPhrasesArray[_fixedPhrasesToLoad++] = DialogPhrase.PHRASE_NONSENSE;

            _namesToLoad = 0;
            _phrasesToLoad = 0;
            _cachedNames = 0;
            _cachedPhrases = 0;
        }

        public static void UnloadUsedDialogsAndNames()
        {
            _cachedNamesFinder.Clear();
            _cachedPhrasesFinder.Clear();
            Array.Clear(_namesToLoadArray, 0, _namesToLoadArray.Length);
            Array.Clear(_phrasesToLoadArray, 0, _phrasesToLoadArray.Length);
            Array.Clear(_cachedNamesArray, 0, _cachedNamesArray.Length);
            Array.Clear(_cachedPhrasesArray, 0, _cachedPhrasesArray.Length);

            _namesToLoad = 0;
            _phrasesToLoad = 0;
            _cachedNames = 0;
            _cachedPhrases = 0;
        }

        public static IEnumerator PreloadRoomPhrasesCoroutine(Room room)
        {
            TextAsset textAsset;

            AsyncOperationHandle<TextAsset> handler1 = Addressables.LoadAssetAsync<TextAsset>(PHRASES_PATH);
            AsyncOperationHandle<TextAsset> handler2 = Addressables.LoadAssetAsync<TextAsset>(NAMES_PATH);

            yield return handler1;
            yield return handler2;

            textAsset = handler1.Result;
            string[] phrases = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            textAsset = handler2.Result;
            string[] names = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            Addressables.Release(handler1);
            Addressables.Release(handler2);

            /* Empty cached dialogs */
            UnloadUsedDialogsAndNames();


            yield return PreloadRoomPhrasesCoroutine(room, phrases);
            yield return PreloadRoomNamesCoroutine(room, names);
        }

        private static void PreloadRoomPhrasesPrepareList(Room room)
        {
            /* Copy fixed ones */
            _fixedPhrasesArray.CopyTo(_phrasesToLoadArray, 0);
            _phrasesToLoad = _fixedPhrasesToLoad;

            /* Get room info and its linked phrases */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ReadOnlySpan<DialogPhrase> roomPhrases = roomInfo.Phrases;

            /* Copy imposed ones from room */
            Span<DialogPhrase> phrasesDest = new(_phrasesToLoadArray);
            phrasesDest = phrasesDest[_phrasesToLoad..];
            roomPhrases.CopyTo(phrasesDest);
            _phrasesToLoad += roomPhrases.Length;
        }

        private static IEnumerator PreloadRoomPhrasesCoroutine(Room room, string[] lines)
        {
            int _loadedPhrases = 0;

            PreloadRoomPhrasesPrepareList(room);

            while (_loadedPhrases < _phrasesToLoad)
            {
                bool already = _cachedPhrasesFinder.TryGetValue(_phrasesToLoadArray[_loadedPhrases], out _);

                if (!already)
                {
                    PreloadRoomPhrases_TaskCycle(lines, _loadedPhrases);
                    yield return ResourceAtlasClass.WaitForNextFrame;
                }

                ++_loadedPhrases;
            }
        }

        private static void PreloadRoomPhrases_TaskCycle(string[] lines, int index)
        {
            /* Get phrases which use actual Room or Room.NONE */
            ReadOnlySpan<PhraseConfig> phraseConfigs = ResourceDialogsAtlasClass.PhraseConfigs;
            DialogPhrase phrase = _phrasesToLoadArray[index];
            ref readonly PhraseConfig phraseConfig = ref phraseConfigs[(int)phrase];

            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)phrase];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedPhrasesArray[_cachedPhrases] = new(phraseConfig, columns[(int)_language]);
            _cachedPhrasesFinder[phrase] = _cachedPhrases;
            ++_cachedPhrases;
        }

        private static void PreloadRoomNamesPrepareList(Room room)
        {
            /* Copy fixed ones */
            _fixedNamesArray.CopyTo(_namesToLoadArray, 0);
            _namesToLoad = _fixedNamesToLoad;

            /* Get room info and its linked phrases */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            ReadOnlySpan<NameType> roomNames = roomInfo.Names;

            /* Copy imposed ones from room */
            Span<NameType> namesDest = _namesToLoadArray;
            namesDest = namesDest[_namesToLoad..];
            roomNames.CopyTo(namesDest);
            _namesToLoad += roomNames.Length;
        }

        private static IEnumerator PreloadRoomNamesCoroutine(Room room, string[] lines)
        {
            int _loadedNames = 0;

            PreloadRoomNamesPrepareList(room);

            while (_loadedNames < _namesToLoad)
            {
                bool already = _cachedNamesFinder.TryGetValue(_namesToLoadArray[_loadedNames], out _);

                if (!already)
                {
                    PreloadRoomNames_AddName(lines, _loadedNames);
                    yield return ResourceAtlasClass.WaitForNextFrame;
                }

                ++_loadedNames;
            }
        }

        private static void PreloadRoomNames_AddName(string[] lines, int index)
        {
            NameType name = _namesToLoadArray[index];

            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)name];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedNamesArray[_cachedNames] = columns[(int)_language];
            _cachedNamesFinder[name] = _cachedNames;
            ++_cachedNames;
        }



        public static ref readonly PhraseContent GetPhraseContent(DialogPhrase phraseType)
        {
            if(_cachedPhrasesFinder.TryGetValue(phraseType, out int storedIndex))
            {
                return ref _cachedPhrasesArray[storedIndex];
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
                return ref _cachedNamesArray[storedIndex];
            }
            else
            {
                Debug.LogError($"Name type {name} not found in cached names.");
                return ref string.Empty;
            }
        }


    }
}