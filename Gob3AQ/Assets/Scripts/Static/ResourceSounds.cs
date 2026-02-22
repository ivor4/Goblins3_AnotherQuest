using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSoundsAtlas;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Gob3AQ.ResourceSounds
{
    public static class ResourceSoundsClass
    {
        private static Dictionary<GameSound, AsyncOperationHandle<AudioClip>> _cachedHandles;
        private static HashSet<GameSound> _soundsToLoadArray;
        private static HashSet<GameSound> _soundsToRelease;
        private static ReadOnlyHashSet<GameSound> _fixedSoundsArray;

        public static void Initialize()
        {
            _cachedHandles = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _soundsToLoadArray = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _soundsToRelease = new(GameFixedConfig.MAX_CACHED_SPRITES);

            HashSet<GameSound> editableHash = new(GameFixedConfig.MAX_CACHED_SPRITES)
            {

            };


            _ = editableHash.Remove(GameSound.SOUND_NONE);

            _fixedSoundsArray = new(editableHash);
        }

        public static IEnumerator PreloadRoomSoundsCoroutine(Room room)
        {
            PreloadSoundsPrepareList(room);

            foreach(GameSound soundToLoad in _soundsToLoadArray)
            {
                AsyncOperationHandle<AudioClip> handle = PreloadRoomSoundsCycle(soundToLoad);
                _cachedHandles[soundToLoad] = handle;
            }

            foreach(AsyncOperationHandle<AudioClip> handle in _cachedHandles.Values)
            {
                if(!handle.IsDone)
                {
                    yield return handle;
                }
            }

            _soundsToLoadArray.Clear();
        }

        public static void UnloadUsedSounds(bool fullClear)
        {
            if (fullClear)
            {
                _soundsToRelease.UnionWith(_cachedHandles.Keys);
            }
            else
            {
                /* This gives the ones which are not present in ToLoad, in order to release them */
                _soundsToRelease.ExceptWith(_soundsToLoadArray);
            }

            foreach(GameSound sound in _soundsToRelease)
            {
                _cachedHandles[sound].Release();
                _cachedHandles.Remove(sound);
            }

            _soundsToRelease.Clear();
        }

        private static void PreloadSoundsPrepareList(Room room)
        {
            /* Move previous room "to load" into "loaded" */
            _soundsToRelease.UnionWith(_cachedHandles.Keys);
            

            /* First fixed sprites to load */
            _soundsToLoadArray.UnionWith(_fixedSoundsArray);

            /* Then room sprites */
            if (room != Room.ROOM_NONE)
            {
                ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
                _soundsToLoadArray.UnionWith(roomInfo.sounds);
            }

            _ = _soundsToLoadArray.Remove(GameSound.SOUND_NONE);

            /* Clear the ones which are not used */
            UnloadUsedSounds(false);

            /* Load only the ones which are not yet loaded */
            _soundsToLoadArray.ExceptWith(_cachedHandles.Keys);
        }

        private static AsyncOperationHandle<AudioClip> PreloadRoomSoundsCycle(GameSound sound)
        {
            AsyncOperationHandle<AudioClip> handle;
            ref readonly SoundConfig config = ref ResourceSoundsAtlasClass.GetSoundConfig(sound);
            handle = Addressables.LoadAssetAsync<AudioClip>(config.path);

            return handle;
        }


        public static AudioClip GetSound(GameSound sound)
        {
            if (_cachedHandles.TryGetValue(sound, out AsyncOperationHandle<AudioClip> handler))
            {
                return handler.Result;
            }
            else
            {
                Debug.LogError($"AudioClip type {sound} not found in cached sounds.");
                return null;
            }
        }
    }
}
