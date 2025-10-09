using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Door;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.LevelMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using Gob3AQ.Waypoint;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private static List<DoorClass> _Door_List;
        private static Dictionary<GameItem, GameElementClass> _ItemDictionary;
        private static LevelElemInfo _HoveredElem;
        private static PendingCharacterInteraction[] _PendingCharInteractions;


        public struct PendingCharacterInteraction
        {
            public bool pending;
            public readonly InteractionUsage usage;

            public PendingCharacterInteraction(in InteractionUsage interaction)
            {
                pending = true;
                usage = interaction;
            }
        }


        #region "Services"




        public static void GetPlayerListService(out ReadOnlySpan<PlayableCharScript> rolist)
        {
            rolist = _Player_List;
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


        public static void ItemRegisterService(bool register, GameElementClass instance)
        {
            if (register)
            {
                _ItemDictionary.Add(instance.ItemID, instance);
            }
            else
            {
                _ItemDictionary.Remove(instance.ItemID);
            }
        }

        public static void ItemObtainPickableService(GameItem item)
        {
            bool found = _ItemDictionary.TryGetValue(item, out GameElementClass itemInstance);

            if(found)
            {
                itemInstance.VirtualDestroy();
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

        public static void PlayerReachedWaypointService(CharacterType character)
        {
            /* Generate stack array of 2 talkers, player and dst */
            Span<GameItem> talkers = stackalloc GameItem[2];

            ref PendingCharacterInteraction charPendingAction = ref _PendingCharInteractions[(int)character];
            ref readonly InteractionUsage usage = ref charPendingAction.usage;

            /* Now interact if buffered */
            if (charPendingAction.pending && (usage.type != InteractionType.PLAYER_MOVE))
            {
                ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemDest);

                switch (itemInfo.family)
                {
                    case GameItemFamily.ITEM_FAMILY_TYPE_DOOR:
                        CrossDoor(character, usage.destListIndex);
                        break;
                    default:
                        /* Check if it is available and is still in original position */
                        bool validTransaction = IsItemAvailable(usage.itemDest);
                        validTransaction &= _ItemDictionary[usage.itemDest].Waypoint == usage.destWaypoint;


                        if (validTransaction)
                        {
                            /* Use Item is also Take Item */
                            VARMAP_LevelMaster.USE_ITEM(in usage, out InteractionUsageOutcome outcome);

                            if (outcome.dialogType != DialogType.DIALOG_NONE)
                            {
                                /* Default talkers are own player and itemDest */
                                talkers[0] = _Player_List[(int)usage.playerSource].ItemID;
                                talkers[1] = usage.itemDest;

                                VARMAP_LevelMaster.ENABLE_DIALOGUE(true, talkers, outcome.dialogType, outcome.dialogPhrase);
                            }
                            else if (outcome.animation != CharacterAnimation.ITEM_USE_ANIMATION_NONE)
                            {
                                //ActAnimationRequest(outcome.animation);
                            }
                            else
                            {
                                /**/
                            }
                        }
                        break;
                }
            }

            /* Pending no more */
            charPendingAction.pending = false;
        }

        

        public static void GameElementOverService(in LevelElemInfo info)
        {
            /* Overwrite */
            if (info.active)
            {
                _HoveredElem = info;
            }
            /* Undo if actual family slot was used by this one */
            else
            {
                if ((_HoveredElem.family == info.family) && (_HoveredElem.index == info.index))
                {
                    _HoveredElem = LevelElemInfo.EMPTY;
                }
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
            _ItemDictionary = new Dictionary<GameItem, GameElementClass>(GameFixedConfig.MAX_POOLED_ITEMS);
            _Door_List = new List<DoorClass>(GameFixedConfig.MAX_SCENE_DOORS);


            _LayerOnlyPlayers = 0x1 << LayerMask.NameToLayer("PlayerLayer");
            _LayerPlayersAndItemsNPC = _LayerOnlyPlayers | (0x1 << LayerMask.NameToLayer("ItemLayer"));

            _playMouseArea = new Rect(0, 0, Screen.width, Screen.height * GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT);

            _PendingCharInteractions = new PendingCharacterInteraction[GameFixedConfig.MAX_LEVEL_PLAYABLE_CHARACTERS];
            _HoveredElem = LevelElemInfo.EMPTY;
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

            
            /* If no items menu or just a menu opened */
            if (mouse.secondaryReleased)
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
                bool accepted;
                /* If there is buffered action */
                if (_HoveredElem.family != GameItemFamily.ITEM_FAMILY_TYPE_NONE)
                {
                    switch (_HoveredElem.family)
                    {
                        case GameItemFamily.ITEM_FAMILY_TYPE_PLAYER:
                            if ((chosenItem != GameItem.ITEM_NONE) && (playerSelected != CharacterType.CHARACTER_NONE))
                            {
                                usage = InteractionUsage.CreatePlayerUseItemWithItem(playerSelected, chosenItem,
                                        (GameItem)_HoveredElem.index, _HoveredElem.waypoint);
                                VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, _HoveredElem.waypoint, out accepted);

                                if(accepted)
                                {
                                    _PendingCharInteractions[(int)playerSelected] = new PendingCharacterInteraction(in usage);
                                }
                            }
                            else
                            {
                                VARMAP_LevelMaster.SELECT_PLAYER((CharacterType)_HoveredElem.index);
                                VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                            }
                            break;

                        case GameItemFamily.ITEM_FAMILY_TYPE_OBJECT:
                            if (playerSelected != CharacterType.CHARACTER_NONE)
                            {
                                if (chosenItem == GameItem.ITEM_NONE)
                                {
                                    usage = InteractionUsage.CreatePlayerWithItem(playerSelected, (GameItem)_HoveredElem.index,
                                        _HoveredElem.waypoint);
                                }
                                else
                                {
                                    usage = InteractionUsage.CreatePlayerUseItemWithItem(playerSelected, chosenItem,
                                        (GameItem)_HoveredElem.index, _HoveredElem.waypoint);
                                }

                                VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, _HoveredElem.waypoint, out accepted);

                                if (accepted)
                                {
                                    _PendingCharInteractions[(int)playerSelected] = new PendingCharacterInteraction(in usage);
                                }
                            }
                            break;


                        case GameItemFamily.ITEM_FAMILY_TYPE_DOOR:
                            usage = InteractionUsage.CreatePlayerWithItem(playerSelected, (GameItem)_HoveredElem.index,
                                _HoveredElem.waypoint);

                            VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, _HoveredElem.waypoint, out accepted);
                            if (accepted)
                            {
                                _PendingCharInteractions[(int)playerSelected] = new PendingCharacterInteraction(in usage);
                            }
                            VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                            break;

                        default:
                            break;
                    }
                }
                /* Selected player or item */
                else if (playerSelected != CharacterType.CHARACTER_NONE)
                {
                    CheckPlayerMovementOrder(in mouse, playerSelected);
                }
                else
                {
                    /**/
                }
            }
            else
            {
                /**/
            }
        }

        private static void CheckPlayerMovementOrder(in MousePropertiesStruct mouse, CharacterType selectedCharacter)
        {
            GetNearestWPService(mouse.pos1, GameFixedConfig.DISTANCE_MOUSE_FURTHEST_WP, out WaypointClass candidate);

            if (candidate)
            {
                InteractionUsage usage = InteractionUsage.CreatePlayerMove(selectedCharacter, candidate);

                VARMAP_LevelMaster.INTERACT_PLAYER(selectedCharacter, candidate, out bool accepted);
                if (accepted)
                {
                    _PendingCharInteractions[(int)selectedCharacter] = new PendingCharacterInteraction(in usage);
                }
            }
        }

        private static void CrossDoor(CharacterType character, int doorIndex)
        {
            DoorClass door = _Door_List[doorIndex];

            _ = character;  /* TODO: out animation? Or should have been done before calling Cross Door? */

            int waypointIndex = LevelOptionsClass.GetLevelDoorToWaypoint(door.RoomLead, door.RoomAppearPosition);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_MAIN, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_PARROT, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_SNAKE, waypointIndex);

            VARMAP_LevelMaster.LOAD_ROOM(door.RoomLead, out _);
        }

        private static bool IsItemAvailable(GameItem item)
        {
            bool available;
            bool found = _ItemDictionary.TryGetValue(item, out GameElementClass itemInstance);

            if (found)
            {
                available = itemInstance.IsAvailable;
            }
            else
            {
                available = false;
            }

            return available;
        }
    }
}

