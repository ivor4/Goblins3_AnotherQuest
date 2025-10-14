using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;


namespace Gob3AQ.ResourceSprites
{
    public static class ResourceSpritesClass
    {
        private static List<AsyncOperationHandle<Sprite>> _cachedHandles;
        private static Dictionary<GameSprite, Sprite> _cachedSpritesFinder;
        private static GameSprite[] _spritesToLoadArray;
        private static GameSprite[] _fixedSpritesArray;
        private static int _fixedSpritesToLoad;
        private static int _spritesToLoad;


        public static void Initialize()
        {
            _cachedHandles = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _cachedSpritesFinder = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _spritesToLoadArray = new GameSprite[GameFixedConfig.MAX_CACHED_SPRITES];
            _fixedSpritesArray = new GameSprite[GameFixedConfig.MAX_FIXED_SPRITES_TO_LOAD];

            _fixedSpritesArray[0] = GameSprite.SPRITE_CURSOR_NORMAL;
            _fixedSpritesArray[1] = GameSprite.SPRITE_CURSOR_USING;
            _fixedSpritesArray[2] = GameSprite.SPRITE_INVENTORY;
            _fixedSpritesToLoad = 3;

            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                _fixedSpritesArray[_fixedSpritesToLoad + (int)i] = ItemsInteractionsClass.GetSpriteFromPickable(i);
            }

            _fixedSpritesToLoad += (int)GamePickableItem.ITEM_PICK_TOTAL;
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            bool keepProcessing = true;
            int _loadedSprites = 0;

            PreloadSpritesPrepareList(room);

            while (keepProcessing)
            {
                AsyncOperationHandle<Sprite> handle = PreloadRoomSpritesCycle(room, _loadedSprites, out GameSprite sprite);

                if (sprite != GameSprite.SPRITE_NONE)
                {
                    bool already = _cachedSpritesFinder.TryGetValue(_spritesToLoadArray[_loadedSprites], out _);

                    if (!already)
                    {
                        yield return handle;
                        Sprite spriteRes = handle.Result;
                        _cachedHandles.Add(handle);
                        _cachedSpritesFinder[sprite] = spriteRes;
                    }
                    ++_loadedSprites;
                }
                else
                {
                    keepProcessing = false;
                }
            }
        }

        public static void UnloadUsedSprites()
        {
            for(int i=0; i< _cachedHandles.Count; i++)
            {
                Addressables.Release(_cachedHandles[i]);
            }

            _cachedHandles.Clear();
        }

        private static void PreloadSpritesPrepareList(Room room)
        {
            _cachedSpritesFinder.Clear();
            Array.Clear(_spritesToLoadArray, 0, _spritesToLoadArray.Length);
            _cachedHandles.Clear();

            /* Prepare sprites to load */

            /* First fixed sprites to load */
            _fixedSpritesArray.CopyTo(_spritesToLoadArray, 0);
            _spritesToLoad = _fixedSpritesToLoad;

            /* Then room sprites */
            Span<GameSprite> spriteDest = _spritesToLoadArray;
            spriteDest = spriteDest[_spritesToLoad..];
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            roomInfo.Sprites.CopyTo(spriteDest);
            _spritesToLoad += roomInfo.Sprites.Length;
        }

        private static AsyncOperationHandle<Sprite> PreloadRoomSpritesCycle(Room room, int index, out GameSprite sprite)
        {
            AsyncOperationHandle<Sprite> handle;
            

            if(index < _spritesToLoad)
            { 
                sprite = _spritesToLoadArray[index];
                ref readonly SpriteConfig config = ref ResourceSpritesAtlasClass.GetSpriteConfig(sprite);
                handle = Addressables.LoadAssetAsync<Sprite>(config.path);
            }
            else
            {
                sprite = GameSprite.SPRITE_NONE;
                handle = default;
            }

            return handle;
        }


        public static Sprite GetSprite(GameSprite sprite)
        {
            if (_cachedSpritesFinder.TryGetValue(sprite, out Sprite res_sprite))
            {
                return res_sprite;
            }
            else
            {
                Debug.LogError($"Sprite type {sprite} not found in cached sprites.");
                return null;
            }
        }
    }
}
