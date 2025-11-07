using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

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
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_ROOM_FIRST, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(4){GameItem.ITEM_POTION, GameItem.ITEM_POTION_BLUE, GameItem.ITEM_FOUNTAIN, GameItem.ITEM_NPC_MILITO, }) 
            ),
            
            new( /* ROOM_LAST */
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_LAST, }) 
            ),
            
            /* > ATG 1 END < */
        };



#if UNITY_EDITOR
        public static GameObject GetPrefab(PrefabEnum prefabId)
        {
            string address = _PrefabList[(int)prefabId];

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
            var guid = AssetDatabase.AssetPathToGUID(address);
            UnityEditor.AddressableAssets.Settings.AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(entry.AssetPath);

            return prefabAsset;
        }


        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "Assets/Prefabs/WaypointClass.prefab"
        };
#endif
    }
}
