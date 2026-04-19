using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;


namespace Gob3AQ.ResourceSprites
{
    public static class ResourceSpritesClass
    {
        private static Dictionary<GameSprite, AsyncOperationHandle<Sprite>> _cachedHandles;
        private static HashSet<GameSprite> _spritesToLoadArray;
        private static HashSet<GameSprite> _spritesToRelease;
        private static ReadOnlyHashSet<GameSprite> _fixedSpritesArray;
        private static Dictionary<string, AsyncOperationHandle<SpriteAtlas>> _cachedAtlasHandles;
        private static Dictionary<string, int> _atlasReferenceCount;

        public static void Initialize()
        {
            _cachedHandles = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _spritesToLoadArray = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _spritesToRelease = new(GameFixedConfig.MAX_CACHED_SPRITES);

            HashSet<GameSprite> editableHash = new(GameFixedConfig.MAX_CACHED_SPRITES)
            {
                GameSprite.SPRITE_BLANK,
                GameSprite.SPRITE_CURSOR_NORMAL,
                GameSprite.SPRITE_CURSOR_USING,
                GameSprite.SPRITE_CURSOR_DRAG,
                GameSprite.SPRITE_INVENTORY,
                GameSprite.SPRITE_UI_TAKE,
                GameSprite.SPRITE_UI_TALK,
                GameSprite.SPRITE_UI_OBSERVE,
                GameSprite.SPRITE_UI_MOUSE_MOVE,
                GameSprite.SPRITE_UI_CURSOR_DOOR,
                GameSprite.SPRITE_MAINCHAR_STEADY
            };

            for (GamePickableItem i = 0; i < GamePickableItem.ITEM_PICK_TOTAL; i++)
            {
                editableHash.Add(ItemsInteractionsClass.GetSpriteFromPickable(i));
            }

            for (MementoParent i = 0; i < MementoParent.MEMENTO_PARENT_TOTAL; i++)
            {
                ref readonly MementoParentInfo memparInfo = ref ItemsInteractionsClass.GetMementoParentInfo(i);
                editableHash.Add(memparInfo.sprite);
            }

            _ = editableHash.Remove(GameSprite.SPRITE_NONE);

            _fixedSpritesArray = new(editableHash);

            _cachedAtlasHandles = new(GameFixedConfig.MAX_CACHED_SPRITES);
            _atlasReferenceCount = new(GameFixedConfig.MAX_CACHED_SPRITES);

            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            PreloadSpritesPrepareList(room);

            /* Atlas */
            foreach (GameSprite spriteToLoad in _spritesToLoadArray)
            {
                string atlas = ResourceSpritesAtlasClass.GetSpriteConfig(spriteToLoad).atlaspath;
                if (!_cachedAtlasHandles.ContainsKey(atlas))
                {
                    AsyncOperationHandle<SpriteAtlas> atlasHandle = Addressables.LoadAssetAsync<SpriteAtlas>(atlas);
                    _cachedAtlasHandles[atlas] = atlasHandle;
                    _atlasReferenceCount[atlas] = 1;
                }
                else
                {
                    _atlasReferenceCount[atlas] = _atlasReferenceCount[atlas] + 1;
                }
            }

            foreach (AsyncOperationHandle<SpriteAtlas> atlasHandle in _cachedAtlasHandles.Values)
            {
                if (!atlasHandle.IsDone)
                {
                    yield return atlasHandle;
                }
            }

            /* Sprites */
            foreach (GameSprite spriteToLoad in _spritesToLoadArray)
            {
                AsyncOperationHandle<Sprite> handle = PreloadRoomSpritesCycle(spriteToLoad);
                _cachedHandles[spriteToLoad] = handle;
            }

            foreach (AsyncOperationHandle<Sprite> handle in _cachedHandles.Values)
            {
                if(!handle.IsDone)
                {
                    yield return handle;
                }
            }
            

            _spritesToLoadArray.Clear();
        }

        public static void UnloadUsedSprites(bool fullClear)
        {
            _spritesToRelease.UnionWith(_cachedHandles.Keys);

            if (!fullClear)
            {
                /* This gives the ones which are not present in ToLoad, in order to release them */
                _spritesToRelease.ExceptWith(_spritesToLoadArray);
            }

            foreach(GameSprite sprite in _spritesToRelease)
            {
                _cachedHandles[sprite].Release();
                _cachedHandles.Remove(sprite);

                string atlas = ResourceSpritesAtlasClass.GetSpriteConfig(sprite).atlaspath;
                if(_atlasReferenceCount.TryGetValue(atlas, out int refCount))
                {
                    --refCount;
                    _atlasReferenceCount[atlas] = refCount;

                    if(refCount <= 0)
                    {
                        _cachedAtlasHandles[atlas].Release();
                        _atlasReferenceCount.Remove(atlas);
                        _cachedAtlasHandles.Remove(atlas);
                    }
                }
            }

            _spritesToRelease.Clear();
        }

        private static void PreloadSpritesPrepareList(Room room)
        {            
            /* First fixed sprites to load */
            _spritesToLoadArray.UnionWith(_fixedSpritesArray);

            /* Then room sprites */
            if (room != Room.ROOM_NONE)
            {
                ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);
                _spritesToLoadArray.UnionWith(roomInfo.sprites);

                /* Then present room items sprites */
                foreach (GameItem item in roomInfo.items)
                {
                    ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);
                    _spritesToLoadArray.UnionWith(itemInfo.sprites);
                }
            }

            _ = _spritesToLoadArray.Remove(GameSprite.SPRITE_NONE);

            /* Clear the ones which are not used */
            UnloadUsedSprites(false);

            /* Load only the ones which are not yet loaded */
            _spritesToLoadArray.ExceptWith(_cachedHandles.Keys);
        }

        private static AsyncOperationHandle<Sprite> PreloadRoomSpritesCycle(GameSprite sprite)
        {
            AsyncOperationHandle<Sprite> handle;
            ref readonly SpriteConfig config = ref ResourceSpritesAtlasClass.GetSpriteConfig(sprite);
            handle = Addressables.LoadAssetAsync<Sprite>(config.path);

            return handle;
        }


        public static Sprite GetSprite(GameSprite sprite)
        {
            if (_cachedHandles.TryGetValue(sprite, out AsyncOperationHandle<Sprite> handler))
            {
                return handler.Result;
            }
            else
            {
                Debug.LogError($"Sprite type {sprite} not found in cached sprites.");
                return null;
            }
        }

        private static void RequestAtlas(string tag, Action<SpriteAtlas> callback)
        {
            if(_cachedAtlasHandles.TryGetValue(tag, out AsyncOperationHandle<SpriteAtlas> atlasHandle))
            {
                if (atlasHandle.IsDone)
                {
                    callback(atlasHandle.Result);
                }
                else
                {
                    /* This sprites are requested because SpriteAtlas is being loaded before sprite.
                     * Some of them bring sprites which indeed are used, but not that instance which will be dynamically done */
                }
            }
            else
            {
                Debug.LogError($"Atlas with tag {tag} requested but not found in cached atlases.");
                callback(null);
            }
        }
    }
}
