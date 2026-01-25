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
        private static HashSet<PrefabEnum> _prefabsToRelease;
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
            _prefabsToRelease = new(GameFixedConfig.MAX_CACHED_PREFABS);
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
                _prefabsToRelease.UnionWith(_cachedPrefabsFinder.Keys);
            }
            else
            {
                _prefabsToRelease.ExceptWith(_prefabsToLoad);
            }

            foreach (PrefabEnum handle in _prefabsToRelease)
            {
                _cachedPrefabsFinder[handle].Release();
                _cachedPrefabsFinder.Remove(handle);
            }

            _prefabsToRelease.Clear();
        }

        private static void PreloadPrefabsPrepareList(Room room)
        {
            _ = room;

            _prefabsToRelease.UnionWith(_cachedPrefabsFinder.Keys);
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
            new( /* HIVE1_ROOM_1 */
            new GameSprite[1]{GameSprite.BACKGROUND_HIVE1_ROOM1, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_ROOM1, GameSprite.SPRITE_ITEM_DECO_BED_LAYER, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(4){DialogPhrase.PHRASE_NONSENSE, DialogPhrase.PHRASE_DECISION_NOT_SLEEP, DialogPhrase.PHRASE_DECISION_SLEEP_NAP, DialogPhrase.PHRASE_DECISION_SLEEP_LONG, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(8){GameItem.ITEM_CARDS_PICKABLE, GameItem.ITEM_HIVE1_CHEST, GameItem.ITEM_HIVE1_WARDROBE, GameItem.ITEM_HIVE1_WARDROBE_OPENED, GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_PERFUME, GameItem.ITEM_SOAP_PICKABLE, GameItem.ITEM_HIVE1_BED, }) 
            ),
            
            new( /* HIVE1_CORRIDOR_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_CORRIDOR1, GameSprite.BACKGROUND_HIVE1_CORRIDOR1_N, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_CORRIDOR1, GameSprite.BACKGROUND_HIVE1_CORRIDOR1_N, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_NPC_REME, }) 
            ),
            
            new( /* HIVE1_HALL_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_HALL1, GameSprite.BACKGROUND_HIVE1_HALL1_N, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_HALL1, GameSprite.BACKGROUND_HIVE1_HALL1_N, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(4){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_GENERIC_DOOR2, GameItem.ITEM_HIVE1_AD_BOARD, GameItem.ITEM_HIVE1_EXIT_DOOR, }) 
            ),
            
            new( /* HIVE1_WC_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_WC, GameSprite.BACKGROUND_HIVE1_WC_N, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_WC, GameSprite.BACKGROUND_HIVE1_WC_N, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_BASIN, }) 
            ),
            
            new( /* CITY1_STREET_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_STREET1, GameSprite.BACKGROUND_CITY1_STREET1_N, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_STREET1, GameSprite.BACKGROUND_CITY1_STREET1_N, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_STREET1_STH_DOOR, GameItem.ITEM_STREET1_CENTER_DOOR, }) 
            ),
            
            new( /* CITY1_STREET_2 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_STREET2, GameSprite.BACKGROUND_CITY1_STREET2_N, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_STREET2, GameSprite.BACKGROUND_CITY1_STREET2_N, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_STREET2_PERIPH_DOOR, GameItem.ITEM_PHARMACY_DOOR, GameItem.ITEM_ELMANYO_DOOR, }) 
            ),
            
            new( /* PHARMACY_1 */
            new GameSprite[1]{GameSprite.BACKGROUND_PHARMACY1, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_PHARMACY1, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_PHARMACY_NPC_QUEUE, GameItem.ITEM_PHARMACY_NPC_OWNER, }) 
            ),
            
            new( /* MANYO_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_MANYO, GameSprite.BACKGROUND_CITY1_MANYO_NIGHT, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_MANYO, GameSprite.BACKGROUND_CITY1_MANYO_NIGHT, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_CITY1_UMBRELLA, GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_ELMANYO_OWNER, GameItem.ITEM_STUFFED_DEER, GameItem.ITEM_ELMANYO_OWNER_NIGHT, }) 
            ),
            
            new( /* ROOM_LAST */
            new GameSprite[1]{GameSprite.SPRITE_NONE, },
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST, }), 
            new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONSENSE, }), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_LAST, }) 
            ),
            
            /* > ATG 1 END < */
        };
    }
}
