using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.GameMaster;
using UnityEngine.SceneManagement;
using Gob3AQ.VARMAP.Initialization;
using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Safe;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.VARMAP.Variable;
using System;
using Gob3AQ.ResourceSprites;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Gob3AQ.ResourceAtlas;

namespace Gob3AQ.GameMaster
{

    public class GameMasterClass : MonoBehaviour
    {
        private const uint ALL_MODULES_LOADED_MASK = (1 << (int)GameModules.MODULE_TOTAL) - 1;

        private static GameMasterClass _singleton;
        private static Game_Status prevPauseStatus;
        private static uint moduleLoadingDone;
        private static AsyncOperationHandle<SceneInstance> prevRoom;
        private static bool prevRoomLoaded;
        private static bool saveGamePending;
        private static Room loadScenePending;

        void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                DontDestroyOnLoad(gameObject);

                /* Initialize VARMAP once for all the game */
                VARMAP_DataSystem.InitializeVARMAP();

                /* Make default (undefined behavior) */
                prevRoom = default;
                prevRoomLoaded = false;
                saveGamePending = false;
                loadScenePending = Room.ROOM_NONE;
            }
        }



        void Update()
        {
            VARMAP_Safe.IncrementTick();
            VARMAP_Variable_Indexable.CommitPending();

            /* -------- Old game cycle ends here -------- */

            bool eventsBeingProcessed = VARMAP_GameMaster.GET_EVENTS_BEING_PROCESSED();

            if (!eventsBeingProcessed)
            {
                if (saveGamePending)
                {
                    saveGamePending = false;
                    VARMAP_DataSystem.SaveVARMAPData();
                }

                if (loadScenePending != Room.ROOM_NONE)
                {
                    Room loadRoom = loadScenePending;
                    loadScenePending = Room.ROOM_NONE;
                    _singleton.StartCoroutine(UnloadAndLoadRoomCoroutine(loadRoom));
                }
            }

            /* -------- New game cycle starts here -------- */

            VARMAP_Variable_Indexable.InvokePending();

            bool pausePressed;

            Game_Status gstatus = VARMAP_GameMaster.GET_GAMESTATUS();
            KeyStruct kstruct = VARMAP_GameMaster.GET_PRESSED_KEYS();

                       

            pausePressed = (kstruct.cyclepressedKeys & KeyFunctions.KEYFUNC_PAUSE) != KeyFunctions.KEYFUNC_NONE;

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                case Game_Status.GAME_STATUS_PLAY_DIALOG:
                    if (pausePressed)
                    {
                        PauseGame(true);
                    }
                    else
                    {
                        Play_Process_Time();
                    }
                    break;

                case Game_Status.GAME_STATUS_PAUSE:
                    if(pausePressed)
                    {
                        PauseGame(false);
                    }
                    break;

                case Game_Status.GAME_STATUS_LOADING:
                    if (moduleLoadingDone == ALL_MODULES_LOADED_MASK)
                    {
                        _SetGameStatus(Game_Status.GAME_STATUS_PLAY);
                    }
                    break;

            }

            

        }

        void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
            }
        }

        private static void Play_Process_Time()
        {
            float elapsed_millis;
            float elapsedDelta = Time.deltaTime;

            elapsed_millis = VARMAP_GameMaster.GET_ELAPSED_TIME_MS();

            elapsed_millis += elapsedDelta * GameFixedConfig.SECONDS_TO_MILLISECONDS;

            VARMAP_GameMaster.SET_ELAPSED_TIME_MS((ulong)elapsed_millis);
        }

        private static void PauseGame(bool pause)
        {
            Game_Status status = VARMAP_GameMaster.GET_GAMESTATUS();

            if (pause)
            {
                if ((status == Game_Status.GAME_STATUS_PLAY)||(status == Game_Status.GAME_STATUS_PLAY_DIALOG))
                {
                    VARMAP_GameMaster.SET_GAMESTATUS(Game_Status.GAME_STATUS_PAUSE);
                    Physics2D.simulationMode = SimulationMode2D.Script;
                    prevPauseStatus = status;
                }
            }
            else
            {
                if (status == Game_Status.GAME_STATUS_PAUSE)
                {
                    _SetGameStatus(prevPauseStatus);
                    Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
                }
            }
        }


        public static void LoadRoomService(Room room, out bool error)
        {
            saveGamePending = false;
            Game_Status gstatus = VARMAP_GameMaster.GET_SHADOW_GAMESTATUS();

            if (((uint)room < (uint)Room.ROOMS_TOTAL) &&
                (gstatus != Game_Status.GAME_STATUS_CHANGING_ROOM) && (gstatus != Game_Status.GAME_STATUS_CHANGING_ROOM))
            {
                error = false;
                loadScenePending = room;
            }
            else
            {
                error = true;
            }
        }

        public static void StartGameService(out bool error)
        {
            VARMAP_DataSystem.ResetVARMAP();

            LaunchResourcesInitializations();

            LoadRoomService(Room.ROOM_FIRST, out error);
        }

        public static void SaveGameService()
        {
            saveGamePending = true;
        }

        public static void LoadGameService()
        {
            VARMAP_DataSystem.ResetVARMAP();
            VARMAP_DataSystem.LoadVARMAPData();

            LaunchResourcesInitializations();

            LoadRoomService(VARMAP_GameMaster.GET_ACTUAL_ROOM(), out _);
        }


        public static void LoadingCompletedService(GameModules module)
        {
            moduleLoadingDone |= (uint)(1 << (int)module);
        }

        public static void IsModuleLoadedService(GameModules module, out bool loaded)
        {
            loaded = (moduleLoadingDone & (1 << (int)module)) != 0;
        }


        public static void ChangeGameModeService(Game_Status newmode, out bool error)
        {
            Game_Status oldmode = VARMAP_GameMaster.GET_SHADOW_GAMESTATUS();

            bool valid;

            switch (newmode)
            {
                case Game_Status.GAME_STATUS_PLAY_DIALOG:
                    valid = (oldmode == Game_Status.GAME_STATUS_PLAY) || (oldmode == Game_Status.GAME_STATUS_PLAY_MEMENTO);
                    break;
                case Game_Status.GAME_STATUS_STOPPED:
                case Game_Status.GAME_STATUS_PLAY_MEMENTO:
                case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                case Game_Status.GAME_STATUS_PLAY_FREEZE:
                case Game_Status.GAME_STATUS_PAUSE:
                    valid = oldmode == Game_Status.GAME_STATUS_PLAY;
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    valid = (oldmode == Game_Status.GAME_STATUS_PLAY_DIALOG) ||
                        (oldmode == Game_Status.GAME_STATUS_PLAY_MEMENTO) ||
                        (oldmode == Game_Status.GAME_STATUS_STOPPED) ||
                        (oldmode == Game_Status.GAME_STATUS_PLAY_ITEM_MENU) ||
                        (oldmode == Game_Status.GAME_STATUS_PLAY_FREEZE)||
                        (oldmode == Game_Status.GAME_STATUS_PAUSE);
                    break;
                default:
                    valid = false;
                    break;
            }

            if(valid)
            {
                _SetGameStatus(newmode);
            }

            error = !valid;
        }


        public static void ExitGameService(out bool error)
        {
            saveGamePending = false;
            loadScenePending = Room.ROOM_NONE;

            if (VARMAP_GameMaster.GET_SHADOW_GAMESTATUS() != Game_Status.GAME_STATUS_STOPPED)
            {
                VARMAP_DataSystem.ResetVARMAP();
                _singleton.StartCoroutine(ExitGameCoroutine());
                error = false;
            }
            else
            {
                error = true;
            }
        }


        private static void _SetGameStatus(Game_Status status)
        {
            VARMAP_GameMaster.SET_GAMESTATUS(status);
        }

        private static void LaunchResourcesInitializations()
        {
            ResourceDialogsClass.Initialize(DialogLanguages.DIALOG_LANG_ENGLISH);
            ResourceSpritesClass.Initialize();
            ResourceAtlasClass.Initialize();
        }

        private static IEnumerator UnloadAndLoadRoomCoroutine(Room room)
        {
            AsyncOperationHandle<SceneInstance> nextRoom;
            AsyncOperationHandle<SceneInstance> loadingRoom;

            /* Operations prepared for next level */
            /* Commit pending changes */
            
            VARMAP_GameMaster.SET_ACTUAL_ROOM(room);
            _SetGameStatus(Game_Status.GAME_STATUS_CHANGING_ROOM);
            VARMAP_DataSystem.ClearVARMAPChangeEvents();
            VARMAP_Variable_Indexable.CommitPending();

            /* Load loading room */
            loadingRoom = Addressables.LoadSceneAsync(GameFixedConfig.ROOM_LOADING, LoadSceneMode.Single, true);
            yield return loadingRoom;

            /* Prepare for next scene loading process bitfield */
            moduleLoadingDone = 0;

            /* Unlaod previous room resources */
            yield return UnloadPreviousRoomResources(false);

            /* Load texts */
            yield return ResourceDialogsClass.PreloadRoomTextsCoroutine(room);

            /* Load sprites */
            yield return ResourceSpritesClass.PreloadRoomSpritesCoroutine(room);

            /* Load prefabs */
            yield return ResourceAtlasClass.PreloadPrefabsCoroutine(room);

            /* Now load desired room and its resources */
            string resourceName = GameFixedConfig.ROOM_TO_SCENE_NAME[(int)room];

            /* Prepare, but not go into next room yet */
            nextRoom = Addressables.LoadSceneAsync(resourceName, LoadSceneMode.Single, false);
            yield return nextRoom;


            /* Activate loaded room */
            yield return nextRoom.Result.ActivateAsync();

            /* Unload loading room */
            yield return Addressables.UnloadSceneAsync(loadingRoom);


            /* Move to previous for next load */
            prevRoom = nextRoom;
            prevRoomLoaded = true;

            yield return ResourceAtlasClass.WaitForNextFrame;

            
            _SetGameStatus(Game_Status.GAME_STATUS_LOADING);

            yield return ResourceAtlasClass.WaitForNextFrame;

            VARMAP_GameMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMaster);
        }


        private static IEnumerator ExitGameCoroutine()
        {
            /* Load bootstrap */
            yield return SceneManager.LoadSceneAsync(GameFixedConfig.ROOM_MAINMENU, LoadSceneMode.Single);

            /* Unlaod previous room resources */
            yield return UnloadPreviousRoomResources(true);
        }


        private static IEnumerable UnloadPreviousRoomResources(bool fullclear)
        {
            /* Unload previous room (in case) */
            if (prevRoomLoaded)
            {
                yield return Addressables.UnloadSceneAsync(prevRoom);
                prevRoom = default;
                prevRoomLoaded = false;
            }

            if (fullclear)
            {
                ResourceSpritesClass.UnloadUsedSprites(true);
                ResourceDialogsClass.UnloadUsedTexts(true);
                ResourceAtlasClass.UnloadUnusedPrefabs(true);
            }

            /* Just in case */
            yield return Resources.UnloadUnusedAssets();

            /* Collect as much as possible */
            GC.Collect();
        }

    }
}

