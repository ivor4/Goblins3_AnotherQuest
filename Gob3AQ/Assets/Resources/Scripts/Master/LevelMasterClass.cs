using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.LevelMaster;
using System;
using Gob3AQ.FixedConfig;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.Door;
using Gob3AQ.Waypoint;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private static Rect _playMouseArea;
        private static int _LayerOnlyPlayers;
        private static int _LayerPlayersAndItemsNPC;

        private static LevelMasterClass _singleton;

        private static List<WaypointClass> _WP_List;
        private static PlayableCharScript[] _Player_List;
        private static List<ItemClass> _Item_List;
        private static List<DoorClass> _Door_List;
        private static Dictionary<GameItem, ItemClass> _ItemDictionary;
        private static LevelElemInfo _ClickedElem;
        private static int _freeClickDebounce;


        #region "Services"


           

        public static void GetPlayerListService(out ReadOnlySpan<PlayableCharScript> rolist)
        {
            rolist = _Player_List;
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


        public static void ItemRegisterService(bool register, ItemClass instance)
        {
            if (register)
            {
                _Item_List.Add(instance);
                _ItemDictionary.Add(instance.ItemID, instance);
            }
            else
            {
                _Item_List.Remove(instance);
                _ItemDictionary.Remove(instance.ItemID);
            }
        }

        public static void ItemObtainPickableService(GameItem item)
        {
            bool found = _ItemDictionary.TryGetValue(item, out ItemClass itemInstance);

            if(found)
            {
                itemInstance.gameObject.SetActive(false);
            }
        }

        public static void MonoRegisterService(PlayableCharScript mono, bool add)
        {
            if (add)
            {
                _Player_List[(int)mono.CharType] = mono;
            }
            else
            {
                _Player_List[(int)mono.CharType] = null;
            }
        }

        public static void DoorRegisterService(DoorClass door, bool add)
        {
            if (add)
            {
                _Door_List.Add(door);
            }
            else
            {
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

        public static void GameElementClickService(in LevelElemInfo info)
        {
            /* Overwrite new type is higher than actual */
            if ((int)_ClickedElem.type < (int)info.type)
            {
                _ClickedElem = info;
            }
        }


        #endregion


        #region "Internal Services"
        public static void DeclareAllWaypointsLoaded()
        {
            VARMAP_LevelMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_LevelMaster);
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


        private void Update()
        {
            Game_Status gstatus = VARMAP_LevelMaster.GET_GAMESTATUS();

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                    Update_Play(gstatus);
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
            }
        }


        private void Initializations()
        {
            _Player_List = new PlayableCharScript[GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS];
            _WP_List = new List<WaypointClass>(GameFixedConfig.MAX_LEVEL_WAYPOINTS);
            _Item_List = new List<ItemClass>(GameFixedConfig.MAX_POOLED_ITEMS);
            _ItemDictionary = new Dictionary<GameItem, ItemClass>(GameFixedConfig.MAX_POOLED_ITEMS);
            _Door_List = new List<DoorClass>(GameFixedConfig.MAX_SCENE_DOORS);


            _LayerOnlyPlayers = 0x1 << LayerMask.NameToLayer("PlayerLayer");
            _LayerPlayersAndItemsNPC = _LayerOnlyPlayers | (0x1 << LayerMask.NameToLayer("ItemLayer"));

            _playMouseArea = new Rect(0, 0, Screen.width, Screen.height * GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT);

            _ClickedElem = LevelElemInfo.EMPTY;

            _freeClickDebounce = 0;
        }





        private void Update_Play(Game_Status gstatus)
        {
            UpdateMouseEvents(gstatus);
        }

       


        private void UpdateMouseEvents(Game_Status gstatus)
        {
            ref readonly MousePropertiesStruct mouse = ref VARMAP_LevelMaster.GET_MOUSE_PROPERTIES();

            if(gstatus == Game_Status.GAME_STATUS_PLAY_ITEM_MENU)
            {
                if(mouse.secondaryReleased)
                {
                    VARMAP_LevelMaster.ENABLE_ITEM_MENU(false);
                }
            }
            else if (_playMouseArea.Contains(mouse.posPixels))
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
            GameItem chosenItem = VARMAP_LevelMaster.GET_PICKABLE_ITEM_CHOSEN();
            CharacterType playerSelected = VARMAP_LevelMaster.GET_PLAYER_SELECTED();
            InteractionUsage usage;

            /* If there is buffered action */
            if(_ClickedElem.type != GameElementType.GAME_ELEMENT_NONE)
            {
                switch (_ClickedElem.type)
                {
                    case GameElementType.GAME_ELEMENT_PLAYER:
                        if ((chosenItem != GameItem.ITEM_NONE) && (playerSelected != CharacterType.CHARACTER_NONE))
                        {
                            if (_ClickedElem.available)
                            {
                                /* Transaction is used to ensure player is in the same state as when intended to use item */
                                ulong playerTransactionState = VARMAP_LevelMaster.GET_ELEM_PLAYER_TRANSACTION(_ClickedElem.index);
                                usage = InteractionUsage.CreatePlayerUseItemWithPlayer(playerSelected, chosenItem,
                                    (CharacterType)_ClickedElem.index, playerTransactionState, _ClickedElem.waypoint);


                                VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                            }
                        }
                        else
                        {
                            VARMAP_LevelMaster.SELECT_PLAYER((CharacterType)_ClickedElem.index);
                            VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                        }
                        break;

                    case GameElementType.GAME_ELEMENT_ITEM:
                        if (playerSelected != CharacterType.CHARACTER_NONE)
                        {
                            if (chosenItem == GameItem.ITEM_NONE)
                            {
                                usage = InteractionUsage.CreatePlayerTakeItem(playerSelected, (GameItem)_ClickedElem.index,
                                    _ClickedElem.waypoint);
                            }
                            else
                            {
                                usage = InteractionUsage.CreatePlayerUseItemWithItem(playerSelected, chosenItem,
                                    (GameItem)_ClickedElem.index, _ClickedElem.waypoint);
                            }

                            VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                        }
                        break;


                    case GameElementType.GAME_ELEMENT_DOOR:
                        usage = InteractionUsage.CreatePlayerUseDoor(playerSelected, _ClickedElem.index,
                            _ClickedElem.waypoint);

                        VARMAP_LevelMaster.INTERACT_PLAYER(in usage);
                        VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                        break;

                    default:
                        break;
                }

                /* Reset */
                _ClickedElem = LevelElemInfo.EMPTY;

                /* Avoid immediate free click after an interaction */
                _freeClickDebounce = 2; 
            }
            else if (_freeClickDebounce > 0)
            {
                _freeClickDebounce--;
            }
            /* If no items menu or just a menu opened */
            else if (mouse.secondaryReleased)
            {
                if ((chosenItem == GameItem.ITEM_NONE) && (playerSelected != CharacterType.CHARACTER_NONE))
                {
                    VARMAP_LevelMaster.ENABLE_ITEM_MENU(true);
                }
                else
                {
                    VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                }
            }
            else if (mouse.primaryReleased)
            {
                /* Selected player or item */
                if (playerSelected != CharacterType.CHARACTER_NONE)
                {
                    CheckPlayerMovementOrder(in mouse, playerSelected);
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
    }
}

