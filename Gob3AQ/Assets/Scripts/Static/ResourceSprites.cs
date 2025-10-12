using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


namespace Gob3AQ.ResourceSprites
{
    public static class ResourceSpritesClass
    {
        private static AsyncOperationHandle<Sprite>[] _cachedHandles;
        private static Sprite[] _cachedSprites;
        private static Dictionary<GameSprite, int> _cachedSpritesFinder;
        private static int _loadedSprites;


        public static void Initialize()
        {
            _cachedHandles = new AsyncOperationHandle<Sprite>[GameFixedConfig.MAX_CACHED_SPRITES];
            _cachedSprites = new Sprite[GameFixedConfig.MAX_CACHED_SPRITES];
            _cachedSpritesFinder = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _loadedSprites = 0;
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            bool keepProcessing = true;

            _cachedSpritesFinder.Clear();
            Array.Clear(_cachedSprites, 0, _cachedSprites.Length);
            Array.Clear(_cachedHandles, 0, _cachedHandles.Length);
            _loadedSprites = 0;

            while (keepProcessing)
            {
                AsyncOperationHandle<Sprite> handle = PreloadRoomSpritesCycle(room, _loadedSprites, out GameSprite sprite);

                if(sprite != GameSprite.SPRITE_NONE)
                {
                    yield return handle;
                    Sprite spriteRes = handle.Result;
                    _cachedHandles[_loadedSprites] = handle;
                    _cachedSprites[_loadedSprites] = spriteRes;
                    _cachedSpritesFinder[sprite] = _loadedSprites++;
                }
                else
                {
                    keepProcessing = false;
                }
            }
        }

        public static void UnloadUsedSprites()
        {
            for(int i=0; i< _loadedSprites; i++)
            {
                Addressables.Release(_cachedHandles[i]);
            }

            _loadedSprites = 0;
        }

        private static AsyncOperationHandle<Sprite> PreloadRoomSpritesCycle(Room room, int index, out GameSprite sprite)
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);

            ReadOnlySpan<GameSprite> roomSprites = roomInfo.Sprites;

            if (index < roomSprites.Length)
            {
                ref readonly SpriteConfig config = ref ResourceSpritesAtlasClass.GetSpriteConfig(roomSprites[index]);
                AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(config.path);
                sprite = roomSprites[index];
                return handle;
            }
            else
            {
                sprite = GameSprite.SPRITE_NONE;
                return default;
            }
        }


        public static Sprite GetSprite(GameSprite sprite)
        {
            if (_cachedSpritesFinder.TryGetValue(sprite, out int storedIndex))
            {
                return _cachedSprites[storedIndex];
            }
            else
            {
                Debug.LogError($"Sprite type {sprite} not found in cached sprites.");
                return null;
            }
        }
    }
}
