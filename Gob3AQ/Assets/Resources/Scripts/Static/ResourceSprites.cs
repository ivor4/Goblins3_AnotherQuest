using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gob3AQ.ResourceSprites
{
    public static class ResourceSpritesClass
    {
        private static Sprite[] _cachedSprites;
        private static Dictionary<GameSprite, int> _cachedSpritesFinder;


        public static void Initialize()
        {
            _cachedSprites = new Sprite[GameFixedConfig.MAX_CACHED_SPRITES];
            _cachedSpritesFinder = new(GameFixedConfig.MAX_CACHED_SPRITES);
        }

        public static IEnumerator PreloadRoomSpritesCoroutine(Room room)
        {
            int loadedCount = 0;

            _cachedSpritesFinder.Clear();


            for (GameSprite sprite = 0; sprite < GameSprite.SPRITE_TOTAL; ++sprite)
            {
                ResourceRequest request = PreloadSprite(room, sprite);
                
                /* If we don't have a valid request, just skip it */
                if (request == null)
                {
                    yield return null;
                }
                /* If we have a valid request, wait for it to finish */
                else
                {
                    /* Wait for the request to finish */
                    yield return request;

                    /* If we have a valid sprite, store it */
                    if (request.asset != null)
                    {
                        _cachedSprites[loadedCount] = (Sprite)request.asset;
                        _cachedSpritesFinder[sprite] = loadedCount;
                        ++loadedCount;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load sprite {sprite} for room {room}");
                    }
                }   
            }
        }



        private static ResourceRequest PreloadSprite(Room room, GameSprite sprite)
        {
            ResourceRequest request = null;
            ref readonly SpriteConfig spriteConfig = ref ResourceSpritesAtlasClass.SpriteConfigs[(int)sprite];

            if ((spriteConfig.item != GameItem.ITEM_NONE) || (spriteConfig.room == room))
            {
                request = Resources.LoadAsync<Sprite>(spriteConfig.path);

                if (request.asset == null)
                {
                    Debug.LogError($"Failed to load sprite at path: {spriteConfig.path}");
                }
            }   


            return request;
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
