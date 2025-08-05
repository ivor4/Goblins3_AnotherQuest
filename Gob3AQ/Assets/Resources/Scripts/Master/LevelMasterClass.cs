using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.LevelMaster;
using System;
using Gob3AQ.FixedConfig;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.Door;
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
            MELEMS_NPC,
            MELEMS_DOOR
        }

        private class MouseActiveElems
        {
            public readonly MouseActiveElemsType type;
            public readonly PlayableCharScript player;
            public readonly NPCClass npc;
            public readonly ItemClass item;
            public readonly DoorClass door;
            public readonly int arrayIndex;

            public MouseActiveElems(PlayableCharScript player, int index)
            {
                type = MouseActiveElemsType.MELEMS_PLAYER;
                this.player = player;
                this.npc = null;
                this.item = null;
                this.door = null;
                this.arrayIndex = index;
            }

            public MouseActiveElems(NPCClass npc, int index)
            {
                type = MouseActiveElemsType.MELEMS_NPC;
                this.player = null;
                this.npc = npc;
                this.item = null;
                this.door = null;
                this.arrayIndex = index;
            }

            public MouseActiveElems(ItemClass item, int index)
            {
                type = MouseActiveElemsType.MELEMS_ITEM;
                this.player = null;
                this.npc = null;
                this.item = item;
                this.door = null;
                this.arrayIndex = index;
            }

            public MouseActiveElems(DoorClass door, int index)
            {
                type = MouseActiveElemsType.MELEMS_DOOR;
                this.player = null;
                this.npc = null;
                this.item = null;
                this.door = door;
                this.arrayIndex = index;
            }
        }

        private static Rect _playMouseArea;
        private static Dictionary<Collider2D, MouseActiveElems> _ColliderReference;
        private static RaycastHit2D[] _HitArray;
        private static int _LayerOnlyPlayers;
        private static int _LayerPlayersAndItemsNPC;

        private static LevelMasterClass _singleton;

        private static List<WaypointClass> _WP_List;
        private static PlayableCharScript[] _Player_List;
        private static List<NPCClass> _NPC_List;
        private static List<ItemClass> _Item_List;
        private static List<DoorClass> _Door_List;


        private int loadpercentage;

        #region "Services"


           

        public static void GetPlayerListService(out ReadOnlySpan<PlayableCharScript> rolist)
        {
            rolist = _Player_List;
        }

        /// <summary>
        /// Retrieves a read-only list of NPCs (non-player characters).
        /// </summary>
        /// <param name="rolist">When this method returns, contains a read-only list of <see cref="NPCClass"/> objects representing the
        /// current NPCs. This parameter is passed uninitialized.</param>
        public static void GetNPCListService(out ReadOnlyList<NPCClass> rolist)
        {
            rolist = new(_NPC_List);
        }

        public static void GetScenarioItemListService(out ReadOnlyList<ItemClass> rolist)
        {
            rolist = new(_Item_List);
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

        public static void NPCRegisterService(NPCClass instance, bool register)
        {
            if (register)
            {
                _NPC_List.Add(instance);
                MouseActiveElems melems = new(instance, _NPC_List.Count - 1);
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
                MouseActiveElems melems = new(instance, _Item_List.Count - 1);
                _ColliderReference.Add(instance.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(instance.Collider);
                _Item_List.Remove(instance);
            }
        }

        public static void ItemObtainPickableService(GameItem item)
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
                _Player_List[(int)mono.CharType] = mono;
                MouseActiveElems melems = new(mono, (int)mono.CharType);
                _ColliderReference.Add(mono.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(mono.Collider);
                _Player_List[(int)mono.CharType] = null;
            }
        }

        public static void DoorRegisterService(DoorClass door, bool add)
        {
            if (add)
            {
                _Door_List.Add(door);
                MouseActiveElems melems = new(door, _Door_List.Count - 1);
                _ColliderReference.Add(door.Collider, melems);
            }
            else
            {
                _ColliderReference.Remove(door.Collider);
                _Door_List.Remove(door);
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

        public static void PlayerWaypointUpdateService(CharacterType character, int waypointIndex)
        {
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)character, waypointIndex);
        }

        public static void CrossDoorService(CharacterType character, int doorIndex)
        {
            DoorClass door = _Door_List[doorIndex];

            _ = character;  /* TODO: out animation? Or should have been done before calling Cross Door? */

            int waypointIndex = LevelOptionsClass.GetLevelDoorToWaypoint(door.RoomLead, door.RoomAppearPosition);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_MAIN, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_PARROT, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_SNAKE, waypointIndex);

            VARMAP_LevelMaster.LOAD_ROOM(door.RoomLead, out _);
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
                VARMAP_LevelMaster.UNREG_GAMESTATUS(_OnGameStatusChanged);
            }
        }


        private void Initializations()
        {
            _NPC_List = new List<NPCClass>(GameFixedConfig.MAX_POOLED_ENEMIES);
            _Player_List = new PlayableCharScript[GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS];
            _WP_List = new List<WaypointClass>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
            _Item_List = new List<ItemClass>(GameFixedConfig.MAX_POOLED_ENEMIES);
            _Door_List = new List<DoorClass>(GameFixedConfig.MAX_POOLED_ENEMIES);

            _ColliderReference = new Dictionary<Collider2D, MouseActiveElems>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
            _HitArray = new RaycastHit2D[GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS];


            _LayerOnlyPlayers = 0x1 << LayerMask.NameToLayer("PlayerLayer");
            _LayerPlayersAndItemsNPC = _LayerOnlyPlayers | (0x1 << LayerMask.NameToLayer("ItemLayer")) | (0x1 << LayerMask.NameToLayer("NPCLayer"));

            _playMouseArea = new Rect(0, 0, Screen.width, Screen.height * GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT);
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
            ref readonly MousePropertiesStruct mouse = ref VARMAP_LevelMaster.GET_MOUSE_PROPERTIES();

            bool itemMenuActive = VARMAP_LevelMaster.GET_ITEM_MENU_ACTIVE();

            if(itemMenuActive)
            {
                if(mouse.secondaryReleased)
                {
                    VARMAP_LevelMaster.SET_ITEM_MENU_ACTIVE(false);
                }
            }
            else if(_playMouseArea.Contains(mouse.posPixels))
            {
                UpdateCharItemMouseEvents(in mouse);
            }
            else
            {
                /**/
            }
          
        }

        private void UpdateCharItemMouseEvents(in MousePropertiesStruct mouse)
        {
            InteractionType usageType;
            GameItem itemSource;

            GameItem chosenItem = VARMAP_LevelMaster.GET_PICKABLE_ITEM_CHOSEN();
            CharacterType playerSelected = VARMAP_LevelMaster.GET_PLAYER_SELECTED();
            InteractionUsage usage;

            /* If no items menu or just a menu opened */
            if (mouse.secondaryReleased)
            {
                if ((chosenItem == GameItem.ITEM_NONE)&&(playerSelected != CharacterType.CHARACTER_NONE))
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
                            if((chosenItem != GameItem.ITEM_NONE)&&(playerSelected != CharacterType.CHARACTER_NONE))
                            {
                                if (candidateMelems.player.IsSteady)
                                {
                                    /* Transaction is used to ensure player is in the same state as when intended to use item */
                                    ulong playerTransactionState = VARMAP_LevelMaster.GET_ELEM_PLAYER_TRANSACTION((int)candidateMelems.player.CharType);
                                    usage = InteractionUsage.CreatePlayerUseItemWithPlayer(playerSelected, chosenItem,
                                        candidateMelems.player.CharType, playerTransactionState, candidateMelems.player.Waypoint);


                                    VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                                }
                            }
                            else
                            {
                                VARMAP_LevelMaster.SELECT_PLAYER(candidateMelems.player.CharType);
                                VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                            }

                            

                            break;

                        case MouseActiveElemsType.MELEMS_ITEM:
                            if (playerSelected != CharacterType.CHARACTER_NONE)
                            {
                                if (chosenItem == GameItem.ITEM_NONE)
                                {
                                    usage = InteractionUsage.CreatePlayerTakeItem(playerSelected, candidateMelems.item.ItemID,
                                        candidateMelems.item.Waypoint);
                                }
                                else
                                {
                                    usage = InteractionUsage.CreatePlayerUseItemWithItem(playerSelected, chosenItem,
                                        candidateMelems.item.ItemID, candidateMelems.item.Waypoint);
                                }

                                VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                            }
                            break;

                        case MouseActiveElemsType.MELEMS_NPC:

                            if (chosenItem == GameItem.ITEM_NONE)
                            {
                                usageType = InteractionType.PLAYER_WITH_NPC;
                                itemSource = GameItem.ITEM_NONE;
                            }
                            else
                            {
                                usageType = InteractionType.ITEM_WITH_NPC;
                                itemSource = chosenItem;
                            }

                            usage = new(usageType, playerSelected, itemSource, CharacterType.CHARACTER_NONE,
                                candidateMelems.npc.NPType, GameItem.ITEM_NONE, candidateMelems.arrayIndex,
                                candidateMelems.npc.Waypoint, 0);

                            VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                            break;

                        case MouseActiveElemsType.MELEMS_DOOR:
                            usage = InteractionUsage.CreatePlayerUseDoor(playerSelected, candidateMelems.arrayIndex,
                                candidateMelems.door.Waypoint);

                            VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                            VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                            break;

                        default:
                            break;
                    }
                }
                else if(playerSelected != CharacterType.CHARACTER_NONE)
                {
                    CheckPlayerMovementOrder(in mouse, playerSelected);
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

        private void CheckPlayerMovementOrder(in MousePropertiesStruct mouse, CharacterType selectedCharacter)
        {
            GetNearestWPService(mouse.pos1, GameFixedConfig.DISTANCE_MOUSE_FURTHEST_WP, out WaypointClass candidate);

            if (candidate)
            {
                InteractionUsage usage = InteractionUsage.CreatePlayerMove(selectedCharacter, candidate);
                VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
            }
        }



        #region "Events"
        private void _OnGameStatusChanged(ChangedEventType evType, in Game_Status oldval, in Game_Status newval)
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
                for (int i = 0; i < _Player_List.Length; i++)
                {
                    if (_Player_List[i] != null)
                    {
                        _Player_List[i].enabled = activate;
                    }
                }
            }
        }
        #endregion


       
    }
}

