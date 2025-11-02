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

            _fixedSpritesToLoad = 0;

            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_CURSOR_NORMAL;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_CURSOR_USING;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_CURSOR_DRAG;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_INVENTORY;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_UI_TAKE;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_UI_TALK;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_UI_OBSERVE;
            _fixedSpritesArray[_fixedSpritesToLoad++] = GameSprite.SPRITE_UI_MOUSE_MOVE;


            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                _fixedSpritesArray[_fixedSpritesToLoad++] = ItemsInteractionsClass.GetSpriteFromPickable(i);
            }
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            int _loadedSprites = 0;

            PreloadSpritesPrepareList(room);

            while (_loadedSprites < _spritesToLoad)
            {
                bool already = _cachedSpritesFinder.TryGetValue(_spritesToLoadArray[_loadedSprites], out _);

                if (!already)
                {
                    AsyncOperationHandle<Sprite> handle = PreloadRoomSpritesCycle(room, _loadedSprites, out GameSprite sprite);

                    yield return handle;
                    Sprite spriteRes = handle.Result;
                    _cachedHandles.Add(handle);
                    _cachedSpritesFinder[sprite] = spriteRes;
                }

                ++_loadedSprites;
            }
        }

        public static void UnloadUsedSprites()
        {
            _cachedSpritesFinder.Clear();
            Array.Clear(_spritesToLoadArray, 0, _spritesToLoadArray.Length);

            for (int i=0; i< _cachedHandles.Count; i++)
            {
                Addressables.Release(_cachedHandles[i]);
            }
            _cachedHandles.Clear();
        }

        private static void PreloadSpritesPrepareList(Room room)
        {
            /* Clear */
            UnloadUsedSprites();

            /* First fixed sprites to load */
            _fixedSpritesArray.CopyTo(_spritesToLoadArray, 0);
            _spritesToLoad = _fixedSpritesToLoad;

            /* Then room sprites */
            Span<GameSprite> spriteDest = _spritesToLoadArray;
            spriteDest = spriteDest[_spritesToLoad..];
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            roomInfo.sprites.CopyTo(spriteDest);
            _spritesToLoad += roomInfo.sprites.Count;

            /* Then present room items sprites */
            foreach(GameItem item in roomInfo.items)
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);
                ReadOnlySpan<GameSprite> itemSprites = itemInfo.Sprites;
                for (int j = 0; j < itemSprites.Length; j++)
                {
                    GameSprite itemSprite = itemSprites[j];
                    _spritesToLoadArray[_spritesToLoad++] = itemSprite;
                }
            }
        }

        private static AsyncOperationHandle<Sprite> PreloadRoomSpritesCycle(Room room, int index, out GameSprite sprite)
        {
            AsyncOperationHandle<Sprite> handle;

            sprite = _spritesToLoadArray[index];
            ref readonly SpriteConfig config = ref ResourceSpritesAtlasClass.GetSpriteConfig(sprite);
            handle = Addressables.LoadAssetAsync<Sprite>(config.path);

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
