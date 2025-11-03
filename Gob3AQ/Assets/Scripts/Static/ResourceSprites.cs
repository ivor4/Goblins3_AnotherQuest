using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
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
        private static HashSet<AsyncOperationHandle<Sprite>> _cachedHandles;
        private static Dictionary<GameSprite, Sprite> _cachedSpritesFinder;
        private static HashSet<GameSprite> _spritesToLoadArray;
        private static ReadOnlyHashSet<GameSprite> _fixedSpritesArray;

        public static void Initialize()
        {
            _cachedHandles = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _cachedSpritesFinder = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _spritesToLoadArray = new(GameFixedConfig.MAX_CACHED_SPRITES);

            HashSet<GameSprite> editableHash = new(GameFixedConfig.MAX_CACHED_SPRITES)
            {
                GameSprite.SPRITE_CURSOR_NORMAL,
                GameSprite.SPRITE_CURSOR_USING,
                GameSprite.SPRITE_CURSOR_DRAG,
                GameSprite.SPRITE_INVENTORY,
                GameSprite.SPRITE_UI_TAKE,
                GameSprite.SPRITE_UI_TALK,
                GameSprite.SPRITE_UI_OBSERVE,
                GameSprite.SPRITE_UI_MOUSE_MOVE
            };

            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                editableHash.Add(ItemsInteractionsClass.GetSpriteFromPickable(i));
            }

            _fixedSpritesArray = new(editableHash);
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            PreloadSpritesPrepareList(room);

            foreach(GameSprite spriteToLoad in _spritesToLoadArray)
            {
                AsyncOperationHandle<Sprite> handle = PreloadRoomSpritesCycle(room, spriteToLoad);

                yield return handle;
                Sprite spriteRes = handle.Result;
                _cachedHandles.Add(handle);
                _cachedSpritesFinder[spriteToLoad] = spriteRes;
            }
        }

        public static void UnloadUsedSprites()
        {
            _cachedSpritesFinder.Clear();
            _spritesToLoadArray.Clear();

            foreach(AsyncOperationHandle<Sprite> handle in _cachedHandles)
            {
                Addressables.Release(handle);
            }
            _cachedHandles.Clear();
        }

        private static void PreloadSpritesPrepareList(Room room)
        {
            /* Clear */
            UnloadUsedSprites();

            /* First fixed sprites to load */
            _spritesToLoadArray.UnionWith(_fixedSpritesArray);

            /* Then room sprites */
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
            _spritesToLoadArray.UnionWith(roomInfo.sprites);

            /* Then present room items sprites */
            foreach(GameItem item in roomInfo.items)
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);
                _spritesToLoadArray.UnionWith(itemInfo.sprites);
            }
        }

        private static AsyncOperationHandle<Sprite> PreloadRoomSpritesCycle(Room room, GameSprite sprite)
        {
            AsyncOperationHandle<Sprite> handle;
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
