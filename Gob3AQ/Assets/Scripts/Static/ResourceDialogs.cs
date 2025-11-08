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

        private static ReadOnlyHashSet<NameType> _fixedNamesArray;
        private static HashSet<NameType> _namesToLoadArray;
        private static ReadOnlyHashSet<DialogPhrase> _fixedPhrasesArray;
        private static HashSet<DialogPhrase> _phrasesToLoadArray;
        private static Dictionary<DialogPhrase, int> _cachedPhrasesFinder;
        private static Dictionary<NameType, string> _cachedNamesFinder;
        private static PhraseContent[] _cachedPhrasesArray;
        private static DialogLanguages _language;

        private static int _cachedPhrases;



        public static void Initialize(DialogLanguages language)
        {
            _language = language;
            _cachedPhrasesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _cachedNamesFinder = new(GameFixedConfig.MAX_CACHED_PHRASES);
            
            _phrasesToLoadArray = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _namesToLoadArray = new(GameFixedConfig.MAX_CACHED_PHRASES);
            _cachedPhrasesArray = new PhraseContent[GameFixedConfig.MAX_CACHED_PHRASES];

            HashSet<DialogPhrase> editablePhraseHash = new(GameFixedConfig.MAX_FIXED_PHRASES_TO_LOAD)
            {
                DialogPhrase.PHRASE_NONSENSE,
                DialogPhrase.PHRASE_NONSENSE_OBSERVE,
                DialogPhrase.PHRASE_NONSENSE_TALK
            };

            HashSet<NameType> editableNameHash = new(GameFixedConfig.MAX_FIXED_NAMES_TO_LOAD)
            {
                NameType.NAME_CHAR_MAIN,
                NameType.NAME_CHAR_PARROT,
                NameType.NAME_CHAR_SNAKE,
                NameType.NAME_INTERACTION_TAKE,
                NameType.NAME_INTERACTION_TALK,
                NameType.NAME_INTERACTION_OBSERVE
            };

            for(Memento i = 0; i < Memento.MEMENTO_TOTAL; ++i)
            {
                ref readonly MementoInfo memInfo = ref ItemsInteractionsClass.GetMementoInfo(i);
                editablePhraseHash.Add(memInfo.phrase);
            }

            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(ItemsInteractionsClass.GetItemFromPickable(i));
                editableNameHash.Add(itemInfo.name);
            }

            _ = editablePhraseHash.Remove(DialogPhrase.PHRASE_NONE);
            _ = editableNameHash.Remove(NameType.NAME_NONE);

            _fixedPhrasesArray = new(editablePhraseHash);
            _fixedNamesArray = new(editableNameHash);

            _cachedPhrases = 0;
        }

        public static void UnloadUsedTexts()
        {
            _cachedNamesFinder.Clear();
            _cachedPhrasesFinder.Clear();
            _namesToLoadArray.Clear();
            _phrasesToLoadArray.Clear();
            Array.Clear(_cachedPhrasesArray, 0, _cachedPhrasesArray.Length);

            _cachedPhrases = 0;
        }

        public static IEnumerator PreloadRoomTextsCoroutine(Room room)
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
            UnloadUsedTexts();


            yield return PreloadRoomTextsCoroutine(room, names, phrases);
        }


        private static void PreloadRoomPhrases_TaskCycle(string[] lines, DialogPhrase phrase)
        {
            ref readonly PhraseConfig phraseConfig = ref ResourceDialogsAtlasClass.GetPhraseConfig(phrase);
            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)phrase];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedPhrasesArray[_cachedPhrases] = new(phraseConfig, columns[(int)_language]);
            _cachedPhrasesFinder[phrase] = _cachedPhrases;
            ++_cachedPhrases;
        }

        private static void PreloadRoomTextsPrepareList(Room room)
        {
            /* Copy fixed ones */
            _namesToLoadArray.UnionWith(_fixedNamesArray);
            _phrasesToLoadArray.UnionWith(_fixedPhrasesArray);

            /* Get room info and its linked phrases */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);

            /* Necessary to use heap for this (but it is loading) */
            HashSet<DialogType> processedDialogues = new(GameFixedConfig.MAX_CACHED_PHRASES);
            Queue<DialogType> dialoguesToProcess = new(GameFixedConfig.MAX_CACHED_PHRASES);

            foreach(GameItem item in roomInfo.items)
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);
                _namesToLoadArray.Add(itemInfo.name);

                ReadOnlySpan<ActionConditions> conditionsEnumArray = itemInfo.Conditions;

                for (int j = 0; j < conditionsEnumArray.Length; j++)
                {
                    ref readonly ActionConditionsInfo condition = ref ItemsInteractionsClass.GetActionConditionsInfo(conditionsEnumArray[j]);

                    /* Standalone phrases */
                    _phrasesToLoadArray.Add(condition.phraseOK);

                    /* Include phrases from dialogs */
                    if (!processedDialogues.Contains(condition.dialogOK))
                    {
                        dialoguesToProcess.Enqueue(condition.dialogOK);
                        processedDialogues.Add(condition.dialogOK);
                    }

                    while (dialoguesToProcess.TryDequeue(out DialogType remainingDialog))
                    {
                        if ((remainingDialog != DialogType.DIALOG_NONE) && (remainingDialog != DialogType.DIALOG_SIMPLE))
                        {
                            ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogConfig(remainingDialog);
                            ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                            for (int k = 0; k < dialogOptions.Length; ++k)
                            {
                                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[k]);
                                ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;

                                for (int l = 0; l < dialogPhrases.Length; ++l)
                                {
                                    _phrasesToLoadArray.Add(dialogPhrases[l]);
                                }

                                /* If option triggers another dialog, include its phrases too */
                                if (dialogOptionConfig.dialogTriggered != DialogType.DIALOG_NONE)
                                {
                                    if (!processedDialogues.Contains(dialogOptionConfig.dialogTriggered))
                                    {
                                        dialoguesToProcess.Enqueue(dialogOptionConfig.dialogTriggered);
                                        processedDialogues.Add(dialogOptionConfig.dialogTriggered);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _ = _namesToLoadArray.Remove(NameType.NAME_NONE);
            _ = _phrasesToLoadArray.Remove(DialogPhrase.PHRASE_NONE);

        }

        private static IEnumerator PreloadRoomTextsCoroutine(Room room, string[] names, string[] phrases)
        {

            PreloadRoomTextsPrepareList(room);

            foreach(NameType name in _namesToLoadArray)
            {
                PreloadRoomNames_AddName(names, name);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            foreach(DialogPhrase phrase in _phrasesToLoadArray)
            {
                PreloadRoomPhrases_TaskCycle(phrases, phrase);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }
        }

        private static void PreloadRoomNames_AddName(string[] lines, NameType name)
        {
            /* Retrieve configuration for given phrase */
            ref readonly string row = ref lines[(int)name];
            ReadOnlySpan<string> columns = row.Split(',');

            _cachedNamesFinder[name] = columns[(int)_language];
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