using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gob3AQ.ResourceDialogs
{
    public static class ResourceDialogsClass
    {
        private const string PHRASES_PATH = "PHRASES_CSV";
        private const string NAMES_PATH = "NAMES_CSV";

        private static Dictionary<DialogPhrase, PhraseContent> _cachedPhrasesFinder;
        private static Dictionary<NameType, string> _cachedNamesFinder;
        private static DialogLanguages _language;



        public static void Initialize()
        {
            _language = DialogLanguages.DIALOG_LANG_NONE;
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _cachedNamesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
        }

        public static void UnloadUsedTexts(bool fullclear)
        {
            if(fullclear)
            {
                _cachedPhrasesFinder.Clear();
                _cachedNamesFinder.Clear();
                _language = DialogLanguages.DIALOG_LANG_NONE;
            }
        }


        public static IEnumerator PreloadTextsCoroutine(DialogLanguages language)
        {
            if (language != _language)
            {
                _language = language;
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

                yield return PreloadRoomTextsCoroutine(names, phrases);
            }
        }

        private static void PreloadRoomPhrases_TaskCycle(string[] lines, DialogPhrase phrase)
        {
            ref readonly PhraseConfig phraseConfig = ref ResourceDialogsAtlasClass.GetPhraseConfig(phrase);
            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)phrase];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedPhrasesFinder[phrase] = new(phraseConfig, columns[(int)_language]);
        }

        private static IEnumerator PreloadRoomTextsCoroutine(string[] names, string[] phrases)
        {
            for(NameType name = NameType.NAME_NONE + 1; name < NameType.NAME_TOTAL; ++name)
            {
                PreloadRoomNames_AddName(names, name);
            }
            yield return ResourceAtlasClass.WaitForNextFrame;


            for(DialogPhrase phrase = DialogPhrase.PHRASE_NONE + 1; phrase < DialogPhrase.PHRASE_TOTAL; ++phrase)
            {
                PreloadRoomPhrases_TaskCycle(phrases, phrase);
            }
            yield return ResourceAtlasClass.WaitForNextFrame;
        }

        private static void PreloadRoomNames_AddName(string[] lines, NameType name)
        {
            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)name];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedNamesFinder[name] = columns[(int)_language];
        }



        public static void GetPhraseContent(DialogPhrase phraseType, out PhraseContent phraseContent)
        {
            if(!_cachedPhrasesFinder.TryGetValue(phraseType, out phraseContent))
            {
                Debug.LogError($"Phrase type {phraseType} not found in cached phrases.");
                phraseContent = PhraseContent.EMPTY;
            }
        }

        public static string GetName(NameType name)
        {
            if (_cachedNamesFinder.TryGetValue(name, out string outName))
            {
                return outName;
            }
            else
            {
                Debug.LogError($"Name type {name} not found in cached names.");
                return string.Empty;
            }
        }


    }
}