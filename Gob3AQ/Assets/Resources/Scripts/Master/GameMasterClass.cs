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

namespace Gob3AQ.GameMaster
{

    public class GameMasterClass : MonoBehaviour
    {
        private const uint ALL_MODULES_LOADED_MASK = (1 << (int)GameModules.MODULE_TOTAL) - 1;

        private static GameMasterClass _singleton;
        private static Game_Status prevPauseStatus;
        private static SUBSCRIPTION_CALL_DELEGATE _lateStartSubscibers;
        private static int firstFrameOfScenePending;
        private static uint moduleLoadingDone;

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
            }
        }



        void Update()
        {
            VARMAP_Safe.IncrementTick();
            VARMAP_Variable_Indexable.CommitPending();

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
                    if (firstFrameOfScenePending >= 0)
                    {
                        if (firstFrameOfScenePending == 0)
                        {
                            _lateStartSubscibers?.Invoke();
                        }
                        --firstFrameOfScenePending;
                    }

                    if ((moduleLoadingDone == ALL_MODULES_LOADED_MASK) && (firstFrameOfScenePending < 0))
                    {
                        GC.Collect();
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
                moduleLoadingDone = 0;

                UnloadAndLoadRoomAsync(room);
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
            VARMAP_DataSystem.SaveVARMAPData();
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

        public static void StartDialogueService(CharacterType charType, DialogType dialog, DialogPhrase phrase)
        {
            if (VARMAP_GameMaster.GET_SHADOW_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY)
            {
                VARMAP_GameMaster.SET_GAMESTATUS(Game_Status.GAME_STATUS_PLAY_DIALOG);
                VARMAP_GameMaster.CANCEL_PICKABLE_ITEM();

                VARMAP_GameMaster.SHOW_DIALOGUE(charType, dialog, phrase);
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
            await ResourceDialogsClass.PreloadRoomPhrasesAsync(room);

            VARMAP_GameMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GameMaster);
        }
        
    }
}

