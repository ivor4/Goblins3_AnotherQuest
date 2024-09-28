using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVerse.VARMAP.Types;
using MVerse.VARMAP.LevelMaster;
using System;
using MVerse.FixedConfig;
using MVerse.EnemyMaster;

namespace MVerse.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private static LevelMasterClass _singleton;

        private static List<MonoBehaviour> _Mono_List;
        private static List<EnemyMasterClass> _Enemy_List;

        private int loadpercentage;
        private float otherworld_transition_time;
        private float otherworld_observe_time;

        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;

                Initializations();
            }

        }


        private void Start()
        {
            loadpercentage = 0;
            otherworld_transition_time = 0f;

            VARMAP_LevelMaster.REG_GAMESTATUS(_OnGameStatusChanged);
        }


        private void Update()
        {
            Game_Status gstatus = VARMAP_LevelMaster.GET_GAMESTATUS();

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_STOPPED:
                    break;

                case Game_Status.GAME_STATUS_LOADING:
                    Update_Loading();
                    break;

                case Game_Status.GAME_STATUS_PAUSE:
                    break;

                case Game_Status.GAME_STATUS_PLAY:
                    Update_Play();
                    break;

                case Game_Status.GAME_STATUS_PLAY_FREEZE:
                    Update_Play_Freeze();
                    break;

                default:
                    break;
            }
        }


        private void Update_Loading()
        {
            if (loadpercentage == 0)
            {
                loadpercentage = 99;
            }
            else
            {
                loadpercentage = 100;
                VARMAP_LevelMaster.LOADING_COMPLETED(out bool _);
            }
        }


        private void Update_Play()
        {
            OtherWorldMode otherworldmode = VARMAP_LevelMaster.GET_OTHER_WORLD_MODE();
            bool otherworldTransition = VARMAP_LevelMaster.GET_OTHER_WORLD_TRANSITION_ACTIVE();
            bool otherworld = VARMAP_LevelMaster.GET_OTHER_WORLD();
            float elapsedTime = Time.deltaTime;

            /* Use timeout when observe Mode of otherworld is active */
            if(otherworld && (!otherworldTransition) && (otherworldmode == OtherWorldMode.OTHER_WORLD_OBSERVE))
            {
                otherworld_observe_time += elapsedTime;
                float progress;

                progress = 1f - otherworld_observe_time / GameFixedConfig.OBSERVE_OTHER_WORLD_TIMEOUT;

                if (otherworld_observe_time >= GameFixedConfig.OBSERVE_OTHER_WORLD_TIMEOUT)
                {
                    progress = 0f;
                    ChangeOtherWorldService(false, OtherWorldMode.OTHER_WORLD_STAY);
                }

                VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_PROGRESS(progress);
            }
        }

        
        private void Update_Play_Freeze()
        {
            if(VARMAP_LevelMaster.GET_OTHER_WORLD_TRANSITION_ACTIVE())
            {
                if (otherworld_transition_time < GameFixedConfig.OTHER_WORLD_TRANSITION_TIME_SECONDS)
                {
                    otherworld_transition_time += Time.deltaTime;

                    if (otherworld_transition_time >= GameFixedConfig.OTHER_WORLD_TRANSITION_TIME_SECONDS)
                    {
                        /* At least one cycle of 100% */
                        VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_PROGRESS(1f);
                    }
                    else
                    {
                        VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_PROGRESS(otherworld_transition_time / GameFixedConfig.OTHER_WORLD_TRANSITION_TIME_SECONDS);
                    }
                }
                else
                {
                    VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_ACTIVE(false);
                    VARMAP_LevelMaster.FREEZE_PLAY(false);

                    bool otherworld = VARMAP_LevelMaster.GET_OTHER_WORLD();

    

                    otherworld_observe_time = 0f;
                    otherworld_transition_time = 0f;
                    DeactivateAllWorldEnemies(otherworld);
                }
            }
        }


        private void Initializations()
        {
            _Enemy_List = new List<EnemyMasterClass>(GameFixedConfig.MAX_POOLED_ENEMIES);
            _Mono_List = new List<MonoBehaviour>();
        }

        
        public static void ChangeOtherWorldService(bool toOtherWorld, OtherWorldMode otherWorldMode)
        {
            /* And many more conditions to check here */
            if((VARMAP_LevelMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY) && (!VARMAP_LevelMaster.GET_OTHER_WORLD_TRANSITION_ACTIVE()))
            {
                ActivateAllWorldEnemies(toOtherWorld);

                VARMAP_LevelMaster.SET_OTHER_WORLD(toOtherWorld);
                VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_ACTIVE(true);
                VARMAP_LevelMaster.SET_OTHER_WORLD_TRANSITION_PROGRESS(0f);
                VARMAP_LevelMaster.SET_OTHER_WORLD_MODE(otherWorldMode);
                VARMAP_LevelMaster.FREEZE_PLAY(true);

                _singleton.otherworld_transition_time = 0f;
            }
        }


        public static void EnemyRegisterService(bool register, EnemyMasterClass instance)
        {
            if(register)
            {
                _Enemy_List.Add(instance);

                if(instance.BelongsToOtherWorld != VARMAP_LevelMaster.GET_OTHER_WORLD())
                {
                    instance.gameObject.SetActive(false);
                }
            }
            else
            {
                _Enemy_List.Remove(instance);
            }
        }

        public static void MonoRegisterService(MonoBehaviour mono, bool add)
        {
            if (add)
            {
                _Mono_List.Add(mono);
            }
            else
            {
                _Mono_List.Remove(mono);
            }
        }

        private static void ActivateAllWorldEnemies(bool otherworld)
        {
            for(int i=0;i<_Enemy_List.Count;i++)
            {
                if (_Enemy_List[i].BelongsToOtherWorld == otherworld)
                {
                    _Enemy_List[i].gameObject.SetActive(true);
                }
            }
        }

        private static void DeactivateAllWorldEnemies(bool otherworld)
        {
            for (int i = 0; i < _Enemy_List.Count; i++)
            {
                if (_Enemy_List[i].BelongsToOtherWorld != otherworld)
                {
                    _Enemy_List[i].gameObject.SetActive(false);
                }
            }
        }


        private void _OnGameStatusChanged(ChangedEventType evType, ref Game_Status oldval, ref Game_Status newval)
        {
            bool use;
            bool activate;

            if (newval == Game_Status.GAME_STATUS_PAUSE)
            {
                use = true;
                activate = false;
            }
            else if (newval == Game_Status.GAME_STATUS_PLAY)
            {
                use = true;
                activate = true;
            }
            else
            {
                use = false;
                activate = false;
            }

            if (use)
            {
                for (int i = 0; i < _Mono_List.Count; i++)
                {
                    _Mono_List[i].enabled = activate;
                }
            }
        }


        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
                _Enemy_List = null;
                VARMAP_LevelMaster.UNREG_GAMESTATUS(_OnGameStatusChanged);
            }
        }
    }
}

