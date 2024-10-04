using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.LevelMaster;
using System;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.Waypoint;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private const byte NO_PLAYER = 0xFF;

        private static LevelMasterClass _singleton;

        private static List<WaypointClass> _WP_List;
        private static List<PlayableCharScript> _Player_List;
        private static List<NPCMasterClass> _NPC_List;


        private int loadpercentage;

        private LayerMask WPLayerMask;

        #region "Services"


           

        public static void GetPlayerListService(ref ReadOnlyList<PlayableCharScript> rolist)
        {
            rolist.SetRefList(_Player_List);
        }

        public static void NPCRegisterService(bool register, NPCMasterClass instance)
        {
            if (register)
            {
                _NPC_List.Add(instance);
            }
            else
            {
                _NPC_List.Remove(instance);
            }
        }

        public static void MonoRegisterService(PlayableCharScript mono, bool add, out byte id)
        {
            if (add)
            {
                id = (byte)_Player_List.Count;
                _Player_List.Add(mono);
            }
            else
            {
                id = NO_PLAYER;
                _Player_List.Remove(mono);
            }
        }

        public static void WPRegisterService(WaypointClass waypoint, bool add)
        {
            if (add)
            {
                _WP_List.Add(waypoint);
            }
            else
            {
                _WP_List.Remove(waypoint);
            }
        }


        #endregion

        

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

            VARMAP_LevelMaster.SET_PLAYER_ID_SELECTED(NO_PLAYER);
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

        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
                _NPC_List = null;
                VARMAP_LevelMaster.UNREG_GAMESTATUS(_OnGameStatusChanged);
            }
        }


        private void Initializations()
        {
            _NPC_List = new List<NPCMasterClass>(GameFixedConfig.MAX_POOLED_ENEMIES);
            _Player_List = new List<PlayableCharScript>(GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS);
            _WP_List = new List<WaypointClass>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);

            WPLayerMask.value = LayerMask.NameToLayer("WPLayer");
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
            UpdateMouseEvents();
        }

        
        private void Update_Play_Freeze()
        {

        }


        private void UpdateMouseEvents()
        {
            MousePropertiesStruct mouse = VARMAP_LevelMaster.GET_MOUSE_PROPERTIES();
            byte playerSelected = VARMAP_LevelMaster.GET_PLAYER_ID_SELECTED();
            bool consumed = false;

            /* If no items menu or just a menu opened */
            if (mouse.primaryReleased)
            {
                for (byte i=0; i < _Player_List.Count; ++i)
                {
                    PlayableCharScript player = _Player_List[i];
                    Collider2D collider = player.Collider;

                    if(collider.OverlapPoint(mouse.pos1))
                    {
                        VARMAP_LevelMaster.SET_PLAYER_ID_SELECTED(i);
                        consumed = true;
                        break;
                    }
                }

                if((playerSelected != NO_PLAYER)&&(!consumed))
                {
                    /* Proceed with items/NPCs if event has not been consumed */
                }

                /* Make player move */
                if((!consumed) && (playerSelected != NO_PLAYER))
                {
                    MovePlayerRequest(ref mouse);
                }
            }
        }

        private void MovePlayerRequest(ref MousePropertiesStruct mouse)
        {
            float minDistance = float.PositiveInfinity;
            WaypointClass candidate = null;

            foreach (WaypointClass wp in _WP_List)
            {
                float dist = Vector2.Distance(wp.transform.position, mouse.pos1);

                if ((dist < minDistance) && (dist <= GameFixedConfig.DISTANCE_MOUSE_FURTHEST_WP))
                {
                    minDistance = dist;
                    candidate = wp;
                }
            }

            if (candidate)
            {
                VARMAP_LevelMaster.MOVE_PLAYER(candidate);
            }
        }



        #region "Events"
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
                for (int i = 0; i < _Player_List.Count; i++)
                {
                    _Player_List[i].enabled = activate;
                }
            }
        }
        #endregion


       
    }
}

