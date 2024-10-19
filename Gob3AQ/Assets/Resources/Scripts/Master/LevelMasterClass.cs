using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.LevelMaster;
using System;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.GameElement.Item;
using Gob3AQ.Waypoint;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private enum MouseActiveElemsType
        {
            MELEMS_PLAYER,
            MELEMS_ITEM,
            MELEMS_NPC
        }

        private class MouseActiveElems
        {
            public MouseActiveElemsType type;
            public PlayableCharScript player;
            public NPCMasterClass npc;
            public ItemClass item;

            public MouseActiveElems(PlayableCharScript player)
            {
                type = MouseActiveElemsType.MELEMS_PLAYER;
                this.player = player;
                this.npc = null;
                this.item = null;
            }

            public MouseActiveElems(NPCMasterClass npc)
            {
                type = MouseActiveElemsType.MELEMS_NPC;
                this.player = null;
                this.npc = npc;
                this.item = null;
            }

            public MouseActiveElems(ItemClass item)
            {
                type = MouseActiveElemsType.MELEMS_ITEM;
                this.player = null;
                this.npc = null;
                this.item = item;
            }
        }

        private static Dictionary<Collider2D, MouseActiveElems> _ColliderReference;
        private static RaycastHit2D[] _HitArray;
        private static int _LayerOnlyPlayers;
        private static int _LayerPlayersAndItemsNPC;

        private static LevelMasterClass _singleton;

        private static List<WaypointClass> _WP_List;
        private static List<PlayableCharScript> _Player_List;
        private static List<NPCMasterClass> _NPC_List;
        private static List<ItemClass> _Item_List;


        private int loadpercentage;

        #region "Services"


           

        public static void GetPlayerListService(out ReadOnlyList<PlayableCharScript> rolist)
        {
            rolist = new ReadOnlyList<PlayableCharScript>(_Player_List);
        }

        public static void GetScenarioItemListService(out ReadOnlyList<ItemClass> rolist)
        {
            rolist = new ReadOnlyList<ItemClass>(_Item_List);
        }

        public static void GetNearestWPService(Vector2 position, float maxRadius, out WaypointClass candidate)
        {
            float minDistance = float.PositiveInfinity;
            candidate = null;

            foreach (WaypointClass wp in _WP_List)
            {
                float dist = Vector2.Distance(wp.transform.position, position);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    candidate = wp;
                }
            }

            /* Its better to cancel with ONE comparison at the end than do this comparison on each element */
            if(minDistance > maxRadius)
            {
                candidate = null;
            }
        }

        public static void NPCRegisterService(bool register, NPCMasterClass instance)
        {
            if (register)
            {
                _NPC_List.Add(instance);
                MouseActiveElems melems = new(instance);
                _ColliderReference.Add(instance.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(instance.Collider);
                _NPC_List.Remove(instance);
            }
        }

        public static void ItemRegisterService(bool register, ItemClass instance)
        {
            if (register)
            {
                _Item_List.Add(instance);
                MouseActiveElems melems = new(instance);
                _ColliderReference.Add(instance.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(instance.Collider);
                _Item_List.Remove(instance);
            }
        }

        public static void ItemRemoveFromSceneService(GameItem item)
        {
            foreach(ItemClass itemclass in _Item_List)
            {
                if(itemclass.ItemID == item)
                {
                    itemclass.gameObject.SetActive(false);
                    break;
                }
            }
        }

        public static void MonoRegisterService(PlayableCharScript mono, bool add)
        {
            if (add)
            {
                _Player_List.Add(mono);
                MouseActiveElems melems = new(mono);
                _ColliderReference.Add(mono.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(mono.Collider);
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
            _Item_List = new List<ItemClass>(GameFixedConfig.MAX_POOLED_ENEMIES);

            _ColliderReference = new Dictionary<Collider2D, MouseActiveElems>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
            _HitArray = new RaycastHit2D[GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS];


            _LayerOnlyPlayers = 0x1 << LayerMask.NameToLayer("PlayerLayer");
            _LayerPlayersAndItemsNPC = _LayerOnlyPlayers | (0x1 << LayerMask.NameToLayer("ItemLayer")) | (0x1 << LayerMask.NameToLayer("NPCLayer"));
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

            bool itemsActive = VARMAP_LevelMaster.GET_ITEM_MENU_ACTIVE();

            if(itemsActive)
            {
                if(mouse.secondaryReleased)
                {
                    VARMAP_LevelMaster.SET_ITEM_MENU_ACTIVE(false);
                }
            }
            else
            {
                UpdateCharItemMouseEvents(ref mouse);
            }
          
        }

        private void UpdateCharItemMouseEvents(ref MousePropertiesStruct mouse)
        {
            GameItem chosenItem = VARMAP_LevelMaster.GET_PICKABLE_ITEM_CHOSEN();

            /* If no items menu or just a menu opened */
            if (mouse.secondaryReleased)
            {
                if (chosenItem == GameItem.ITEM_NONE)
                {
                    VARMAP_LevelMaster.SET_ITEM_MENU_ACTIVE(true);
                }
                else
                {
                    VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                }
            }
            else if (mouse.primaryReleased)
            {
                CharacterType playerSelected = VARMAP_LevelMaster.GET_PLAYER_SELECTED();
                int layerMask;

                if(playerSelected == CharacterType.CHARACTER_NONE)
                {
                    layerMask = _LayerOnlyPlayers;
                }
                else
                {
                    layerMask = _LayerPlayersAndItemsNPC;
                }

                int hits = Physics2D.RaycastNonAlloc(mouse.pos1, Vector2.zero, _HitArray, float.PositiveInfinity, layerMask);
                MouseActiveElems candidateMelems = null;


                /* Give first priority to player selection, then items, then npcs */
                for (int i = 0; i < hits; i++)
                {
                    MouseActiveElems melems = _ColliderReference[_HitArray[i].collider];

                    if (candidateMelems != null)
                    {
                        /* Lower type index, higher priority */
                        if (melems.type < candidateMelems.type)
                        {
                            candidateMelems = melems;
                        }
                    }
                    else
                    {
                        candidateMelems = melems;
                    }
                }

                /* Selected player or item */
                if(candidateMelems != null)
                {
                    switch(candidateMelems.type)
                    {
                        case MouseActiveElemsType.MELEMS_PLAYER:
                            VARMAP_LevelMaster.SELECT_PLAYER(candidateMelems.player.charType);
                            break;
                        case MouseActiveElemsType.MELEMS_ITEM:
                            ItemUsageType usageType;
                            GameItem itemSource;

                            if (chosenItem == GameItem.ITEM_NONE)
                            {
                                usageType = ItemUsageType.PLAYER_WITH_ITEM;
                                itemSource = GameItem.ITEM_NONE;
                            }
                            else
                            {
                                usageType = ItemUsageType.ITEM_WITH_ITEM;
                                itemSource = chosenItem;
                            }

                            ItemUsage usage = new(usageType, playerSelected, itemSource, CharacterType.CHARACTER_NONE,
                                CharacterType.CHARACTER_NONE, candidateMelems.item.ItemID);

                            VARMAP_LevelMaster.INTERACT_PLAYER_ITEM(in usage, candidateMelems.item.Waypoint);
                            break;
                    }
                }
                else if(playerSelected != CharacterType.CHARACTER_NONE)
                {
                    CheckPlayerMovementOrder(ref mouse);
                }
                else
                {
                    /* Nothing interesting */
                }
            }
            else
            {
                /**/
            }
        }

        private void CheckPlayerMovementOrder(ref MousePropertiesStruct mouse)
        {
            GetNearestWPService(mouse.pos1, GameFixedConfig.DISTANCE_MOUSE_FURTHEST_WP, out WaypointClass candidate);

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

