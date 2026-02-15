using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GameMaster;
using Gob3AQ.VARMAP.Initialization;
using Gob3AQ.VARMAP.Safe;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Variable;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace Gob3AQ.GameMaster
{

    public class GameMasterClass : MonoBehaviour
    {
        private const uint ALL_MODULES_LOADED_MASK = (1 << (int)GameModules.MODULE_TOTAL) - 1;

        private static GameMasterClass _singleton;
        private static Game_Status prevPauseStatus;
        private static uint moduleLoadingDone;
        private static AsyncOperationHandle<SceneInstance> prevRoom;
        private static AsyncOperationHandle<SceneInstance> baseScene;
        private static bool prevRoomLoaded;
        private static bool saveGamePending;
        private static Room loadScenePending;
        private static bool loadScenePendingFromGameLoad;
        private static MomentType changeMomentPending;

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
                loadScenePendingFromGameLoad = false;
                changeMomentPending = MomentType.MOMENT_ANY;

                SpriteAtlasManager.atlasRequested += RequestAtlas;
            }
        }

        private void Start()
        {
            LaunchResourcesInitializations();
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

                Room loadRoom;
                MomentType moment;
                bool changeRoomEvent;

                if (changeMomentPending != MomentType.MOMENT_ANY)
                {
                    moment = changeMomentPending;

                    if (loadScenePending == Room.ROOM_NONE)
                    {
                        loadRoom = VARMAP_GameMaster.GET_ACTUAL_ROOM();
                        changeRoomEvent = false;
                    }
                    else
                    {
                        loadRoom = loadScenePending;
                        changeRoomEvent = true;
                    }
                    loadScenePending = Room.ROOM_NONE;
                    changeMomentPending = MomentType.MOMENT_ANY;
                    loadScenePendingFromGameLoad = false;

                    _singleton.StartCoroutine(UnloadAndLoadRoomCoroutine(loadRoom, true, changeRoomEvent, moment));
                }
                else if (loadScenePending != Room.ROOM_NONE)
                {
                    loadRoom = loadScenePending;
                    moment = VARMAP_GameMaster.GET_DAY_MOMENT();
                    changeRoomEvent = !loadScenePendingFromGameLoad;
                    loadScenePending = Room.ROOM_NONE;
                    changeMomentPending = MomentType.MOMENT_ANY;
                    loadScenePendingFromGameLoad = false;
                    _singleton.StartCoroutine(UnloadAndLoadRoomCoroutine(loadRoom, false, changeRoomEvent, moment));
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
                case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
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
            if(_singleton != null)
            {
                VARMAP_DataSystem.ResetVARMAP();
                _singleton.StartCoroutine(_singleton.StartGameOnRoomCoroutine(Room.HIVE1_ROOM_1));
                error = false;
            }
            else
            {
                error = true;
            } 
        }

        public static void SaveGameService()
        {
            saveGamePending = true;
        }

        public static void LoadGameService()
        {
            if(_singleton != null)
            {
                VARMAP_DataSystem.ResetVARMAP();
                VARMAP_DataSystem.LoadVARMAPData();
                _singleton.StartCoroutine(_singleton.StartGameOnRoomCoroutine(VARMAP_GameMaster.GET_ACTUAL_ROOM()));
            }
        }

        private IEnumerator StartGameOnRoomCoroutine(Room room)
        {
            /* Load sprites (base Room) */
            Coroutine spritesCoroutine = StartCoroutine(ResourceSpritesClass.PreloadRoomSpritesCoroutine(Room.ROOM_NONE));

            /* Load prefabs (base Room) */
            Coroutine prefabsCoroutine = StartCoroutine(ResourceAtlasClass.PreloadPrefabsCoroutine(Room.ROOM_NONE));

            /* Load texts (base Room) */
            Coroutine dialogsCoroutine = StartCoroutine(ResourceDialogsClass.PreloadRoomTextsCoroutine(Room.ROOM_NONE));

            yield return spritesCoroutine;
            yield return prefabsCoroutine;
            yield return dialogsCoroutine;

            /* Prepare for next scene loading process bitfield */
            moduleLoadingDone = 0;

            baseScene = Addressables.LoadSceneAsync(GameFixedConfig.ROOM_BASE, LoadSceneMode.Single, true);
            yield return baseScene;

            VARMAP_GameMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMaster);

            /* Ensure Masters have executed their Loading tasks */
            while (moduleLoadingDone != ALL_MODULES_LOADED_MASK)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            loadScenePendingFromGameLoad = true;
            LoadRoomService(room, out _);
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
                case Game_Status.GAME_STATUS_PLAY_DECISION:
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
                        (oldmode == Game_Status.GAME_STATUS_PLAY_DECISION) ||
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

        public static void ChangeDayMomentService(MomentType moment)
        {
            if(VARMAP_GameMaster.GET_SHADOW_GAMESTATUS() != Game_Status.GAME_STATUS_STOPPED)
            {
                changeMomentPending = moment;
            }
        }

        public static void LoadAdditionalResourcesService(bool load, ReadOnlyHashSet<NameType> names, ReadOnlyHashSet<DialogPhrase> phrases)
        {

        }


        public static void ExitGameService(out bool error)
        {
            saveGamePending = false;
            loadScenePending = Room.ROOM_NONE;
            loadScenePendingFromGameLoad = false;
            changeMomentPending = MomentType.MOMENT_ANY;

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
            ResourceDialogsClass.Initialize(DialogLanguages.DIALOG_LANG_SPANISH);
            ResourceSpritesClass.Initialize();
            ResourceAtlasClass.Initialize();
        }

        private IEnumerator UnloadAndLoadRoomCoroutine(Room room, bool changeDayEvent, bool changeRoomEvent, MomentType moment)
        {
            AsyncOperationHandle<SceneInstance> nextRoom;

            /* Operations prepared for next level */
            /* Commit pending changes */

            ManageChangeRoomEvents(changeDayEvent, changeRoomEvent);
            VARMAP_GameMaster.SET_DAY_MOMENT(moment);
            VARMAP_GameMaster.SET_ACTUAL_ROOM(room);
            _SetGameStatus(Game_Status.GAME_STATUS_CHANGING_ROOM);

            /* Ensure Changing room operations have been performed by masters */
            yield return ResourceAtlasClass.WaitForNextFrame;
            yield return ResourceAtlasClass.WaitForNextFrame;

            /* Prepare for next scene loading process bitfield */
            moduleLoadingDone = 0;

            /* Unlaod previous room resources */
            yield return UnloadPreviousRoomResources(false);

            /* Load texts */
            Coroutine dialogsCoroutine = StartCoroutine(ResourceDialogsClass.PreloadRoomTextsCoroutine(room));

            /* Load sprites */
            Coroutine spritesCoroutine = StartCoroutine(ResourceSpritesClass.PreloadRoomSpritesCoroutine(room));

            /* Load prefabs */
            Coroutine prefabsCoroutine = StartCoroutine(ResourceAtlasClass.PreloadPrefabsCoroutine(room));

            yield return dialogsCoroutine;
            yield return spritesCoroutine;
            yield return prefabsCoroutine;

            _SetGameStatus(Game_Status.GAME_STATUS_LOADING);

            /* Now load desired room and its resources */
            string resourceName = GameFixedConfig.ROOM_TO_SCENE_NAME[(int)room];

            /* Prepare, but not go into next room yet */
            nextRoom = Addressables.LoadSceneAsync(resourceName, LoadSceneMode.Additive, true);
            yield return nextRoom;

            /* Move to previous for next load */
            prevRoom = nextRoom;
            prevRoomLoaded = true;

            yield return ResourceAtlasClass.WaitForNextFrame;

            VARMAP_GameMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMaster);
        }

        private static void ManageChangeRoomEvents(bool changeDayEvent, bool changeRoomEvent)
        {
            Span<GameEventCombi> combiStack = stackalloc GameEventCombi[1];

            if(changeDayEvent)
            {
                combiStack[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_MOMENT_DAY, false);
                VARMAP_GameMaster.COMMIT_EVENT(combiStack);
            }

            if (changeRoomEvent)
            {
                combiStack[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_ROOM, false);
                VARMAP_GameMaster.COMMIT_EVENT(combiStack);
            }
        }


        private static IEnumerator ExitGameCoroutine()
        {
            /* Load bootstrap */
            yield return SceneManager.LoadSceneAsync(GameFixedConfig.ROOM_MAINMENU, LoadSceneMode.Single);

            yield return Addressables.UnloadSceneAsync(baseScene);

            /* Unlaod all resources */
            yield return UnloadPreviousRoomResources(true);
        }


        private static IEnumerator UnloadPreviousRoomResources(bool fullclear)
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

                /* Just in case */
                yield return Resources.UnloadUnusedAssets();

                /* Collect as much as possible */
                GC.Collect();
            }
        }

        void RequestAtlas(string tag, Action<SpriteAtlas> callback)
        {
            _ = tag;
            _ = callback;
            /* Just for testing, not used */
        }

    }
}

