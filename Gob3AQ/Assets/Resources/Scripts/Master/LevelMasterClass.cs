using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.LevelMaster;
using System;
using Gob3AQ.FixedConfig;
using Gob3AQ.NPCMaster;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private static LevelMasterClass _singleton;

        private static List<MonoBehaviour> _Mono_List;
        private static List<NPCMasterClass> _Enemy_List;

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

        }

        
        private void Update_Play_Freeze()
        {

        }


        private void Initializations()
        {
            _Enemy_List = new List<NPCMasterClass>(GameFixedConfig.MAX_POOLED_ENEMIES);
            _Mono_List = new List<MonoBehaviour>();
        }

        


        public static void NPCRegisterService(bool register, NPCMasterClass instance)
        {
            if(register)
            {
                _Enemy_List.Add(instance);
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

