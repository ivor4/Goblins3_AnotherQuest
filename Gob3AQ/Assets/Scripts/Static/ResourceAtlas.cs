using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Gob3AQ.FixedConfig;
using System.Collections;
using UnityEngine.AddressableAssets;




#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

namespace Gob3AQ.ResourceAtlas
{
#if UNITY_EDITOR
    public enum PrefabForEditorEnum
    {
        PREFAB_EDITOR_WAYPOINT,

        PREFAB_EDITOR_TOTAL
    }
#endif

    public enum PrefabEnum
    {
        PREFAB_MEMENTO_ITEM,

        PREFAB_TOTAL
    }


    public static class ResourceAtlasClass
    {
        public static readonly WaitForNextFrameUnit WaitForNextFrame = new WaitForNextFrameUnit();

        private static ReadOnlyHashSet<PrefabEnum> _fixedPrefabsToLoad;
        private static HashSet<PrefabEnum> _prefabsToLoad;
        private static HashSet<PrefabEnum> _prefabsLoaded;
        private static Dictionary<PrefabEnum, AsyncOperationHandle<GameObject>> _cachedPrefabsFinder;

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

        public static GameObject GetPrefab(PrefabEnum prefab)
        {
            if (_cachedPrefabsFinder.TryGetValue(prefab, out AsyncOperationHandle<GameObject> handle))
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Prefab type {prefab} not found in cached prefabs.");
                return null;
            }
        }
        
        public static void Initialize()
        {
            HashSet<PrefabEnum> editableHash = new(GameFixedConfig.MAX_CACHED_PREFABS)
            {
                PrefabEnum.PREFAB_MEMENTO_ITEM
            };

            _fixedPrefabsToLoad = new(editableHash);

            _cachedPrefabsFinder = new(GameFixedConfig.MAX_CACHED_PREFABS);
            _prefabsToLoad = new(GameFixedConfig.MAX_CACHED_PREFABS);
            _prefabsLoaded = new(GameFixedConfig.MAX_CACHED_PREFABS);
        }

        public static IEnumerator PreloadPrefabsCoroutine(Room room)
        {
            PreloadPrefabsPrepareList(room);

            foreach (PrefabEnum prefab in _fixedPrefabsToLoad)
            {
                AsyncOperationHandle<GameObject> handle = LoadPrefabCycle(prefab);
                yield return handle;

                _cachedPrefabsFinder[prefab] = handle;
            }

            _prefabsToLoad.Clear();
        }

        public static void UnloadUnusedPrefabs(bool fullClear)
        {
            if(fullClear)
            {
                _prefabsLoaded.UnionWith(_cachedPrefabsFinder.Keys);
            }
            else
            {
                _prefabsLoaded.ExceptWith(_prefabsToLoad);
            }

            foreach (PrefabEnum handle in _prefabsLoaded)
            {
                _cachedPrefabsFinder[handle].Release();
                _cachedPrefabsFinder.Remove(handle);
            }

            _prefabsLoaded.Clear();
        }

        private static void PreloadPrefabsPrepareList(Room room)
        {
            _ = room;

            _prefabsLoaded.UnionWith(_cachedPrefabsFinder.Keys);
            _prefabsToLoad.Clear();
            _prefabsToLoad.UnionWith(_fixedPrefabsToLoad);

            UnloadUnusedPrefabs(false);

            /* Load the ones which are not already loaded */
            _prefabsToLoad.ExceptWith(_cachedPrefabsFinder.Keys);
        }

        private static AsyncOperationHandle<GameObject> LoadPrefabCycle(PrefabEnum prefab)
        {
            AsyncOperationHandle<GameObject> handle;
            handle = Addressables.LoadAssetAsync<GameObject>(_PrefabAddressableName[(int)prefab]);

            return handle;
        }

#if UNITY_EDITOR
        public static GameObject GetPrefabForEditor(PrefabForEditorEnum prefabId)
        {
            string address = _PrefabForEditorList[(int)prefabId];

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
            var guid = AssetDatabase.AssetPathToGUID(address);
            UnityEditor.AddressableAssets.Settings.AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(entry.AssetPath);

            return prefabAsset;
        }


        private static readonly string[] _PrefabForEditorList = new string[(int)PrefabForEditorEnum.PREFAB_EDITOR_TOTAL]
        {
            "Assets/Prefabs/WaypointClass.prefab"
        };
#endif


        private static readonly string[] _PrefabAddressableName = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "PREFAB_MEMENTO"
        };

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
    }
}
