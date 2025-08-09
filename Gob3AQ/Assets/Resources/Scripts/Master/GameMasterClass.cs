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
using UnityEditor;
using Gob3AQ.VARMAP;
using System.Threading.Tasks;
using System.Threading;

namespace Gob3AQ.GameMaster
{

    public class GameMasterClass : MonoBehaviour
    {
        private static GameMasterClass _singleton;
        private static Game_Status prevPauseStatus;
        private static SUBSCRIPTION_CALL_DELEGATE _lateStartSubscibers;
        private static int firstFrameOfScenePending;
        private static bool loadResourcesDone;
        private static bool levelMasterLoadedCompleted;

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
                VARMAP_Initialization.InitializeVARMAP();
            }
        }



        void Update()
        {
            VARMAP_Safe.IncrementTick();
            VARMAP_Initialization.CommitVARMAP();

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
                    /* Call late update subscribers */
                    if (loadResourcesDone && (firstFrameOfScenePending >= 0))
                    {
                        if (firstFrameOfScenePending == 0)
                        {
                            _lateStartSubscibers?.Invoke();
                        }
                        --firstFrameOfScenePending;
                    }

                    if (loadResourcesDone && levelMasterLoadedCompleted && (firstFrameOfScenePending < 0))
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

            elapsed_millis += elapsedDelta * GameFixedConfig.MILLISECONDS_TO_SECONDS;

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
            if ((room > Room.ROOM_NONE) && (room < Room.ROOMS_TOTAL))
            {
                error = false;
                

                VARMAP_GameMaster.SET_ACTUAL_ROOM(room);
                _SetGameStatus(Game_Status.GAME_STATUS_CHANGING_ROOM);

                /* Operations prepared for next level */
                firstFrameOfScenePending = 1;
                _lateStartSubscibers = null;
                levelMasterLoadedCompleted = false;
                loadResourcesDone = false;

                UnloadAndLoadRoomAsync(room);
            }
            else
            {
                error = true;
            }
        }

        public static void StartGameService(out bool error)
        {
            VARMAP_Initialization.ResetVARMAP();

            LaunchResourcesInitializations();

            LoadRoomService(Room.ROOM_FIRST, out error);
        }

        public static void SaveGameService()
        {
            VARMAP_Initialization.SaveVARMAPData();
        }

        public static void LoadGameService()
        {
            VARMAP_Initialization.ResetVARMAP();
            VARMAP_Initialization.LoadVARMAPData();

            LaunchResourcesInitializations();

            LoadRoomService(VARMAP_GameMaster.GET_ACTUAL_ROOM(), out _);
        }


        public static void LoadingCompletedService(out bool error)
        {
            levelMasterLoadedCompleted = true;
            error = false;
        }

        public static void FreezePlayService(bool freeze)
        {
            Game_Status status = VARMAP_GameMaster.GET_SHADOW_GAMESTATUS();

            if (freeze && (status == Game_Status.GAME_STATUS_PLAY))
            {
                _SetGameStatus(Game_Status.GAME_STATUS_PLAY_FREEZE);
            }
            else if((!freeze) && (status == Game_Status.GAME_STATUS_PLAY_FREEZE))
            {
                _SetGameStatus(Game_Status.GAME_STATUS_PLAY);
            }
            else
            {

            }

        }

        public static void StartDialogueService(DialogType dialog)
        {
            if (VARMAP_GameMaster.GET_SHADOW_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY)
            {
                VARMAP_GameMaster.SET_GAMESTATUS(Game_Status.GAME_STATUS_PLAY_DIALOG);
                VARMAP_GameMaster.CANCEL_PICKABLE_ITEM();
            }
        }

        public static void ExitGameService(out bool error)
        {
            if (VARMAP_GameMaster.GET_SHADOW_GAMESTATUS() != Game_Status.GAME_STATUS_STOPPED)
            {
                _SetGameStatus(Game_Status.GAME_STATUS_STOPPED);
                SceneManager.LoadScene(GameFixedConfig.ROOM_MAINMENU, LoadSceneMode.Single);
                error = false;
            }
            else
            {
                error = true;
            }
        }

        public static void LateStartSubrsciptionService(SUBSCRIPTION_CALL_DELEGATE callable, bool add)
        {
            if (add)
            {
                _lateStartSubscibers += callable;
            }
            else
            {
                _lateStartSubscibers -= callable;
            }
        }

        private static void _SetGameStatus(Game_Status status)
        {
            VARMAP_GameMaster.SET_GAMESTATUS(status);
        }

        private static void LaunchResourcesInitializations()
        {
            ResourceDialogsClass.Initialize(DialogLanguages.DIALOG_LANG_ENGLISH);
        }

        private static async void UnloadAndLoadRoomAsync(Room room)
        {
            string sceneName = GameFixedConfig.ROOM_TO_SCENE_NAME[(int)room];

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            _SetGameStatus(Game_Status.GAME_STATUS_LOADING);

            await Resources.UnloadUnusedAssets();
            await ResourceDialogsClass.PreloadRoomDialogsAsync(room);

            loadResourcesDone = true;
        }
        
    }
}

