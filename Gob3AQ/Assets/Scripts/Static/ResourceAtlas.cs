using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gob3AQ.ResourceAtlas
{
#if UNITY_EDITOR
    public enum PrefabEnum
    {
        PREFAB_WAYPOINT,

        PREFAB_TOTAL
    }
#endif

    


    public static class ResourceAtlasClass
    {
        public static readonly WaitForNextFrameUnit WaitForNextFrame = new WaitForNextFrameUnit();
        public static ref readonly RoomInfo GetRoomInfo(Room room)
        {
            if ((uint)room < (uint)Room.ROOMS_TOTAL)
            {
                return ref _RoomInfo[(int)room];
            }
            else
            {
                Debug.LogError($"Trying to get RoomInfo for invalid room {room}");
                return ref RoomInfo.EMPTY;
            }
        }

        private static readonly RoomInfo[] _RoomInfo = new RoomInfo[(int)Room.ROOMS_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* ROOM_FIRST */
            new GameSprite[1]{GameSprite.BACKGROUND_ROOM_FIRST, }, 
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONSENSE, }, 
            new GameItem[4]{GameItem.ITEM_POTION, GameItem.ITEM_POTION_BLUE, GameItem.ITEM_FOUNTAIN, GameItem.ITEM_NPC_MILITO, } 
            ),
            
            new( /* ROOM_LAST */
            new GameSprite[1]{GameSprite.SPRITE_LAST, }, 
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONSENSE, }, 
            new GameItem[1]{GameItem.ITEM_LAST, } 
            ),
            
            /* > ATG 1 END < */
        };



#if UNITY_EDITOR
        public static AsyncOperationHandle<GameObject> GetPrefab(PrefabEnum prefabId, out bool success)
        {
            string address = _PrefabList[(int)prefabId];
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);


            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                success = true;
                return handle;
            }
            else
            {
                Debug.LogError($"[Milito] Error al cargar el prefab Addressable en el Editor: {address}");
                success = false;
                return default;
            }
        }


        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "PREFAB_WAYPOINT"
        };
#endif
    }
}
