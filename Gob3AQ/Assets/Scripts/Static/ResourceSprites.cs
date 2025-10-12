using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
            int processedSprites = 0;
            bool keepProcessing = true;

            _cachedSpritesFinder.Clear();
            Array.Clear(_cachedSprites, 0, _cachedSprites.Length);

            while (keepProcessing)
            {
                ResourceRequest request = PreloadRoomSpritesCycle(room, processedSprites, out GameSprite sprite);

                if(request != null)
                {
                    yield return request;
                    Sprite spriteRes = (Sprite)request.asset;
                    _cachedSprites[processedSprites] = spriteRes;
                    _cachedSpritesFinder[sprite] = processedSprites++;
                }
                else
                {
                    keepProcessing = false;
                }
            }
        }

        private static ResourceRequest PreloadRoomSpritesCycle(Room room, int index, out GameSprite sprite)
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(room);

            ReadOnlySpan<GameSprite> roomSprites = roomInfo.Sprites;

            if (index < roomSprites.Length)
            {
                ref readonly SpriteConfig config = ref ResourceSpritesAtlasClass.GetSpriteConfig(roomSprites[index]);
                ResourceRequest request = Resources.LoadAsync<Sprite>(config.path);
                sprite = roomSprites[index];
                return request;
            }
            else
            {
                sprite = GameSprite.SPRITE_NONE;
                return null;
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
