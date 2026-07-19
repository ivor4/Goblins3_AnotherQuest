using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Gob3AQ.FixedConfig;
using System.Collections;
using UnityEngine.AddressableAssets;
using System;





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
                PrefabEnum.PREFAB_MEMENTO_ITEM,
                PrefabEnum.PREFAB_MAINCHARACTER,
                PrefabEnum.PREFAB_MAINCHARACTER_SEATED,
                PrefabEnum.PREFAB_MAINCHARACTER_DREAM,
                PrefabEnum.PREFAB_MAINCHARACTER_BED
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

        public static IEnumerator LoadSpecificPrefab(PrefabEnum prefab)
        {
            if (!_cachedPrefabsFinder.ContainsKey(prefab))
            {
                AsyncOperationHandle<GameObject> handle = LoadPrefabCycle(prefab);
                yield return handle;
                _cachedPrefabsFinder[prefab] = handle;
            }
        }

        public static void UnloadSpecificPrefab(PrefabEnum prefab)
        {
            if (_cachedPrefabsFinder.TryGetValue(prefab, out AsyncOperationHandle<GameObject> handle))
            {
                handle.Release();
                _cachedPrefabsFinder.Remove(prefab);
            }
        }

        public static void UnloadUnusedPrefabs(bool fullClear)
        {
            _prefabsToRelease.UnionWith(_cachedPrefabsFinder.Keys);


            if (!fullClear)
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
            "PREFAB_MEMENTO",
            "PREFAB_MAINCHARACTER",
            "PREFAB_DETAIL_EXTRAPERLO",
            "PREFAB_MAINCHARACTER_SEATED",
            "PREFAB_MAINCHARACTER_DREAM",
            "PREFAB_MAINCHARACTER_BED"
        };

        

        private static readonly RoomInfo[] _RoomInfo = new RoomInfo[(int)Room.ROOMS_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* HIVE1_ROOM_1 */
            new GameSprite[1]{GameSprite.BACKGROUND_HIVE1_ROOM1},
            new GameSound[1]{GameSound.MUSIC_INN},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_ROOM1, GameSprite.SPRITE_ITEM_DECO_BED_LAYER}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(7){GameItem.ITEM_HIVE1_CHEST, GameItem.ITEM_HIVE1_WARDROBE, GameItem.ITEM_HIVE1_WARDROBE_OPENED, GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_PERFUME, GameItem.ITEM_SOAP_PICKABLE, GameItem.ITEM_HIVE1_BED}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_INN}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_RECAP_EXTRAPERLO_GARDEN_IN_ROOM}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* HIVE1_CORRIDOR_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_CORRIDOR1, GameSprite.BACKGROUND_HIVE1_CORRIDOR1_N},
            new GameSound[1]{GameSound.MUSIC_INN},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_CORRIDOR1, GameSprite.BACKGROUND_HIVE1_CORRIDOR1_N}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_NPC_REME}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_INN}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* HIVE1_HALL_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_HALL1, GameSprite.BACKGROUND_HIVE1_HALL1_N},
            new GameSound[1]{GameSound.MUSIC_INN},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_HALL1, GameSprite.BACKGROUND_HIVE1_HALL1_N}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(7){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_GENERIC_DOOR2, GameItem.ITEM_HIVE1_AD_BOARD, GameItem.ITEM_HIVE1_EXIT_DOOR, GameItem.ITEM_HIVE1_POOR_MAN_WC, GameItem.ITEM_HIVE1_SHOELACE, GameItem.ITEM_HIVE1_MAN_WC_CURED}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_INN}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_POOR_MAN_WC_BCKG_DIALOGUE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* HIVE1_WC_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_HIVE1_WC, GameSprite.BACKGROUND_HIVE1_WC_N},
            new GameSound[1]{GameSound.MUSIC_INN},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_HIVE1_WC, GameSprite.BACKGROUND_HIVE1_WC_N}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_BASIN, GameItem.ITEM_HIVE1_POOR_MAN_WC, GameItem.ITEM_HIVE1_ROACH_HEAD, GameItem.ITEM_HIVE1_PIPE}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_INN}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_POOR_MAN_WC_BCKG_DIALOGUE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_REBUILD_COCHROACH_SCARED}) 
            ),

            new( /* CITY1_STREET_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_STREET1, GameSprite.BACKGROUND_CITY1_STREET1_N},
            new GameSound[2]{GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_STREET1, GameSprite.BACKGROUND_CITY1_STREET1_N}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_STREET1_STH_DOOR, GameItem.ITEM_STREET1_CENTER_DOOR}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(2){GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_STREET_2 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_STREET2, GameSprite.BACKGROUND_CITY1_STREET2_N},
            new GameSound[2]{GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_STREET2, GameSprite.BACKGROUND_CITY1_STREET2_N}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_STREET2_PERIPH_DOOR, GameItem.ITEM_PHARMACY_DOOR, GameItem.ITEM_ELMANYO_DOOR}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(2){GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* PHARMACY_1 */
            new GameSprite[1]{GameSprite.BACKGROUND_PHARMACY1},
            new GameSound[1]{GameSound.MUSIC_PHARMACY},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_PHARMACY1}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_PHARMACY_NPC_QUEUE, GameItem.ITEM_PHARMACY_NPC_OWNER, GameItem.ITEM_PHARMACY_INKWELL, GameItem.ITEM_PHARMACY_INK}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_PHARMACY}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* MANYO_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_MANYO, GameSprite.BACKGROUND_CITY1_MANYO_NIGHT},
            new GameSound[1]{GameSound.MUSIC_MANYO},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_MANYO, GameSprite.BACKGROUND_CITY1_MANYO_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(6){GameItem.ITEM_CITY1_UMBRELLA, GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_ELMANYO_OWNER, GameItem.ITEM_STUFFED_DEER, GameItem.ITEM_ELMANYO_OWNER_NIGHT, GameItem.ITEM_ELMANYO_CROWD}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(2){GameSound.MUSIC_MANYO, GameSound.SOUND_AMBIENCE_MANYO_NIGHT}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_MANYO_BCKG_DIALOGUE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* HIVE1_BACKALLEY */
            new GameSprite[2]{GameSprite.BACKGROUND_BACKALLEY, GameSprite.BACKGROUND_BACKALLEY_NIGHT},
            new GameSound[2]{GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_BACKALLEY, GameSprite.BACKGROUND_BACKALLEY_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_HIVE1_VALVE_BOX, GameItem.ITEM_HIVE1_BACKALLEY_PIPE, GameItem.ITEM_HIVE1_VALVE, GameItem.ITEM_HIVE1_WATER_FLOWING}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(3){GameSound.SOUND_AMBIENCE_CITY_DAY, GameSound.SOUND_AMBIENCE_CITY_NIGHT, GameSound.SOUND_AMBIENCE_WATER_FLOW}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_SOUTH_STREET_1 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_SOUTH_STREET_1, GameSprite.BACKGROUND_CITY1_SOUTH_STREET_1_NIGHT},
            new GameSound[1]{GameSound.MUSIC_SOUTH_NEIGH},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_SOUTH_STREET_1, GameSprite.BACKGROUND_CITY1_SOUTH_STREET_1_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_GENERIC_DOOR2}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_SOUTH_NEIGH}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_SOUTH_STREET_2 */
            new GameSprite[2]{GameSprite.BACKGROUND_CITY1_SOUTH_STREET_2, GameSprite.BACKGROUND_CITY1_SOUTH_STREET_2_NIGHT},
            new GameSound[2]{GameSound.MUSIC_SOUTH_NEIGH, GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_SOUTH_STREET_2, GameSprite.BACKGROUND_CITY1_SOUTH_STREET_2_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_FIK, GameItem.ITEM_DOOR_EXTRAPERLO, GameItem.ITEM_FOREGROUND_EXTRP_WALL, GameItem.ITEM_DOOR_EXTRAPERLO_REAL}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(3){GameSound.MUSIC_SOUTH_NEIGH, GameSound.SOUND_AMBIENCE_CITY_NIGHT, GameSound.SOUND_AMBIENCE_OUTSIDE_PUB}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_EXTRAPERLO1 */
            new GameSprite[1]{GameSprite.BACKGROUND_CITY1_EXTRAPERLO},
            new GameSound[1]{GameSound.MUSIC_EXTRAPERLO},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_CITY1_EXTRAPERLO, GameSprite.SPRITE_EXTRAPERLO_COUPLE_TALKING}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(7){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_WAITER, GameItem.ITEM_GENERIC_DOOR2, GameItem.ITEM_NPC_UNKNOWN_WOMEN, GameItem.ITEM_OBJECT_OLIVE_BOWL, GameItem.ITEM_OBJECT_BEER_FULL, GameItem.ITEM_PICKABLE_OLIVE}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(2){GameSound.MUSIC_EXTRAPERLO, GameSound.SOUND_AMBIENCE_INSIDE_PUB}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_EXTRAPERLO2 */
            new GameSprite[1]{GameSprite.BACKGROUND_CITY1_EXTRAPERLO2},
            new GameSound[1]{GameSound.MUSIC_EXTRAPERLO},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(4){GameSprite.BACKGROUND_CITY1_EXTRAPERLO2, GameSprite.SPRITE_FIRE_DANCING, GameSprite.SPRITE_EXTRAPERLO_TABLE1_STEADY, GameSprite.SPRITE_EXTRAPERLO_TABLE2_STEADY}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(4){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO, GameItem.ITEM_NPC_CLOWN, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(2){GameSound.SOUND_AMBIENCE_INSIDE_PUB, GameSound.MUSIC_EXTRAPERLO}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_EXTRAPERLO3 */
            new GameSprite[1]{GameSprite.BACKGROUND_EXTRAPERLO_GARDEN},
            new GameSound[1]{GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_EXTRAPERLO_GARDEN}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_INNOCENT_PLANT, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_AMBIENCE_CITY_NIGHT}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAINER_DIALOG_MAINCHAR_NEED_ORINE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CITY1_EXTRAPERLO3_2 */
            new GameSprite[1]{GameSprite.BACKGROUND_EXTRAPERLO_GARDEN},
            new GameSound[1]{GameSound.SOUND_AMBIENCE_CITY_NIGHT},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_EXTRAPERLO_GARDEN}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(5){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD_SEAT, GameItem.ITEM_PAMFRY, GameItem.ITEM_FIK_EXTRAPERLO_GARDEN, GameItem.ITEM_NPC_GERMAN}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_AMBIENCE_CITY_NIGHT}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAINER_DIALOG_SILVANA_MAINCHAR_GARDEN_LONG}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* CHAPTER_SHOW */
            new GameSprite[1]{GameSprite.SPRITE_BLANK},
            new GameSound[1]{GameSound.SOUND_NONE},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_GENERIC_DOOR1}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_PLAY_SOUND_CHAPTER_IN}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_CORRIDOR */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_CORRIDOR},
            new GameSound[1]{GameSound.MUSIC_DREAM_1},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_CORRIDOR}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(4){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_DREAM_RADIO, GameItem.ITEM_DREAM_CLOCK, GameItem.ITEM_GENERIC_DOOR2}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_DREAM_1}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(2){UnchainConditions.UNCHAIN_ENTRY_DIALOG_DREAM_1, UnchainConditions.UNCHAIN_CLEAR_EPHIMERAL_ON}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_CLEAR_EPHIMERAL_ON}) 
            ),

            new( /* DREAM_1_FRAMEWORK */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_FRAMEWORK},
            new GameSound[1]{GameSound.MUSIC_DREAM_1},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_FRAMEWORK}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_CLASSROOM_PORTRAIT}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_DREAM_1}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_ENTRY_DIALOG_DREAM_1_FRAMEWORK}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_KITCHEN */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_KITCHEN},
            new GameSound[1]{GameSound.MUSIC_DREAM_1},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_KITCHEN}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_GENERIC_DOOR1}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_DREAM_1}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_BEDROOM */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_BEDROOM},
            new GameSound[1]{GameSound.MUSIC_DREAM_1},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_BEDROOM}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_SULTAN, GameItem.ITEM_NPC_PILAR_DREAM_1}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_DREAM_1}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_BEDROOM_NIGHT */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_BEDROOM_NIGHT},
            new GameSound[1]{GameSound.MUSIC_DREAM_1},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(2){GameSprite.BACKGROUND_DREAM_1_BEDROOM_NIGHT, GameSprite.SPRITE_MAINCHAR_STEADY_BED}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_GENERIC_DOOR1}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.MUSIC_DREAM_1}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_ENTRY_DIALOG_DREAM_1_BED_PILAR}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_CORRIDOR_NIGHT */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_CORRIDOR_NIGHT},
            new GameSound[1]{GameSound.SOUND_NONE},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_CORRIDOR_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(3){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_SULTAN, GameItem.ITEM_DOOR_DREAM_1_EXIT}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_DREAM_FEMALE_CRY}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_SOUND_FEMALE_CRY}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* DREAM_1_KITCHEN_NIGHT */
            new GameSprite[1]{GameSprite.BACKGROUND_DREAM_1_KITCHEN_NIGHT},
            new GameSound[1]{GameSound.SOUND_NONE},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.BACKGROUND_DREAM_1_KITCHEN_NIGHT}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(2){GameItem.ITEM_GENERIC_DOOR1, GameItem.ITEM_NPC_ALTER_EGO_1}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_DREAM_FEMALE_CRY}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_SOUND_FEMALE_CRY}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            new( /* ROOM_LAST */
            new GameSprite[1]{GameSprite.SPRITE_NONE},
            new GameSound[1]{GameSound.SOUND_NONE},
            new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST}), 
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(1){GameItem.ITEM_LAST}), 
            new ReadOnlyHashSet<NameType>(new HashSet<NameType>(1){NameType.NAME_NONE}), 
            new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>(1){GameSound.SOUND_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}), 
            new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>(1){UnchainConditions.UNCHAIN_NONE}) 
            ),

            /* > ATG 1 END < */
        };
    }
}
