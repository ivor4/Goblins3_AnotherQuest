using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.GameMaster;
using UnityEngine.SceneManagement;
using Gob3AQ.VARMAP.Initialization;
using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Safe;
using UnityEditor;

namespace Gob3AQ.GameMaster
{

    public class GameMasterClass : MonoBehaviour
    {
        private static GameMasterClass _singleton;
        private static Game_Status prevPauseStatus;
        private static SUBSCRIPTION_CALL_DELEGATE _lateStartSubscibers;
        private static int firstFrameOfScenePending;

        void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                firstFrameOfScenePending = -1;
                DontDestroyOnLoad(gameObject);

                /* Initialize VARMAP once for all the game */
                VARMAP_Initialization.InitializeVARMAP();
            }
        }



        void Update()
        {
            bool pausePressed;
            Game_Status gstatus = VARMAP_GameMaster.GET_GAMESTATUS();
            KeyStruct kstruct = VARMAP_GameMaster.GET_PRESSED_KEYS();

            /* Call late update subscribers */
            if(firstFrameOfScenePending >= 0)
            {
                if (firstFrameOfScenePending == 0)
                {
                    _lateStartSubscibers?.Invoke();
                }
                --firstFrameOfScenePending;
            }
            

            pausePressed = (kstruct.cyclepressedKeys & KeyFunctions.KEYFUNC_PAUSE) != KeyFunctions.KEYFUNC_NONE;

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    if (pausePressed)
                    {
                        PauseGameService(true);
                    }
                    else
                    {
                        Play_Process_Time();
                    }
                    break;

                case Game_Status.GAME_STATUS_PAUSE:
                    if(pausePressed)
                    {
                        PauseGameService(false);
                    }
                    break;

                case Game_Status.GAME_STATUS_STOPPED:
                    break;
            }

            VARMAP_Safe.IncrementTick();

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

        public static void PauseGameService(bool pause)
        {
            if (pause)
            {
                VARMAP_GameMaster.SET_GAMESTATUS(Game_Status.GAME_STATUS_PAUSE);
                Physics2D.simulationMode = SimulationMode2D.Script;
            }
            else
            {
                _SetGameStatus(prevPauseStatus);
                Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
            }
        }


        public static void LoadRoomService(Room room, out bool error)
        {
            if ((room > Room.ROOM_NONE) && (room < Room.TOTAL_ROOMS))
            {
                error = false;
                string sceneName = GameFixedConfig.ROOM_TO_SCENE_NAME[(int)room];

                VARMAP_GameMaster.SET_ACTUAL_ROOM(room);
                _SetGameStatus(Game_Status.GAME_STATUS_LOADING);
                firstFrameOfScenePending = 1;

                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                error = true;
            }
        }

        public static void StartGameService(out bool error)
        {
            VARMAP_Initialization.ResetVARMAP();
            LoadRoomService(Room.ROOM_1_ORIGIN, out error);
        }

        public static void SaveGameService()
        {
            VARMAP_Initialization.SaveVARMAPData();
        }

        public static void LoadGameService()
        {
            VARMAP_Initialization.ResetVARMAP();
            VARMAP_Initialization.LoadVARMAPData();

            LoadRoomService(VARMAP_GameMaster.GET_ACTUAL_ROOM(), out _);
        }


        public static void LoadingCompletedService(out bool error)
        {
            if (VARMAP_GameMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_LOADING)
            {
                _SetGameStatus(Game_Status.GAME_STATUS_PLAY);
                error = false;
            }
            else
            {
                error = true;
            }
        }

        public static void FreezePlayService(bool freeze)
        {
            Game_Status status = VARMAP_GameMaster.GET_GAMESTATUS();

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

        public static void ExitGameService(out bool error)
        {
            if (VARMAP_GameMaster.GET_GAMESTATUS() != Game_Status.GAME_STATUS_STOPPED)
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
            prevPauseStatus = status;
        }


        
    }
}

