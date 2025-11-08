using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.GameElement.Door;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.VARMAP.LevelMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private Rect _playMouseArea;
        private int _LayerOnlyPlayers;
        private int _LayerPlayersAndItemsNPC;

        private static LevelMasterClass _singleton;

        private IReadOnlyList<Vector2> _WP_List;
        private IReadOnlyList<WaypointSolution> _WP_Solution_List;
        private PlayableCharScript[] _Player_List;
        private List<DoorClass> _Door_List;
        private Dictionary<GameItem, GameElementClass> _ItemDictionary;
        private LevelElemInfo _HoveredElem;
        private HashSet<LevelElemInfo> _HoveredPending;
        private PendingCharacterInteraction[] _PendingCharInteractions;
        private HashSet<IGameObjectHoverable> _RaycastedItems;
        private HashSet<IGameObjectHoverable> _PrevRaycastedItems;
        private RaycastHit2D[] _RaycastedItemColliders;
        private Dictionary<Collider2D, IGameObjectHoverable> _ItemColliderDictionary;
        private Camera mainCamera;


        public struct PendingCharacterInteraction
        {
            public bool pending;
            public bool ended;
            public readonly InteractionUsage usage;

            public PendingCharacterInteraction(in InteractionUsage interaction)
            {
                pending = true;
                ended = false;
                usage = interaction;
            }
        }


        #region "Services"


        public static void GetPlayerListService(out ReadOnlySpan<PlayableCharScript> rolist)
        {
            rolist = _singleton._Player_List;
        }

        public static void ObtainScenarioItemsService(out IReadOnlyDictionary<GameItem, GameElementClass> dict)
        {
            if (_singleton != null)
            {
                dict = _singleton._ItemDictionary;
            }
            else
            {
                dict = null;
            }
        }

        public static void GetNearestWPService(Vector2 position, float maxRadius, out int candidate_index,
            out Vector2 candidate_pos)
        {
            if (_singleton != null)
            {
                float minDistance = float.PositiveInfinity;
                candidate_pos = Vector2.zero;
                candidate_index = -1;

                for (int i = 0; i < _singleton._WP_List.Count; ++i)
                {
                    Vector2 wp_pos = _singleton._WP_List[i];
                    float dist = Vector2.Distance(wp_pos, position);

                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        candidate_pos = wp_pos;
                        candidate_index = i;
                    }
                }

                /* Its better to cancel with ONE comparison at the end than do this comparison on each element */
                if (minDistance > maxRadius)
                {
                    candidate_index = -1;
                }
            }
            else
            {
                candidate_index = -1;
                candidate_pos = Vector2.zero;
            }
        }

        public static void GetWaypointListService(out IReadOnlyList<Vector2> waypoints, out IReadOnlyList<WaypointSolution> solutions)
        {
            if (_singleton != null)
            {
                waypoints = _singleton._WP_List;
                solutions = _singleton._WP_Solution_List;
            }
            else
            {
                waypoints = null;
                solutions = null;
            }
        }


        public static void ItemRegisterService(bool register, GameElementClass instance, GameElementClickable clickable)
        {
            if (_singleton != null)
            {
                if (register)
                {
                    _singleton._ItemDictionary.Add(instance.ItemID, instance);
                    _singleton._ItemColliderDictionary[instance.My2DCollider] = clickable;
                }
                else
                {
                    _singleton._ItemDictionary.Remove(instance.ItemID);
                    _singleton._ItemColliderDictionary.Remove(instance.My2DCollider);
                }
            }
        }

        public static void ItemObtainPickableService(GameItem item)
        {
            bool found = _singleton._ItemDictionary.TryGetValue(item, out GameElementClass itemInstance);

            if(found)
            {
                itemInstance.VirtualDestroy();
            }
        }

        public static void MonoRegisterService(PlayableCharScript mono, bool add)
        {
            if (_singleton != null)
            {
                if (add)
                {
                    _singleton._Player_List[(int)mono.CharType] = mono;
                }
                else
                {
                    _singleton._Player_List[(int)mono.CharType] = null;
                }
            }
        }

        public static void DoorRegisterService(DoorClass door, bool add)
        {
            if (_singleton != null)
            {
                if (add)
                {
                    _singleton._Door_List.Add(door);
                }
                else
                {
                    _singleton._Door_List.Remove(door);
                }
            }
        }

        

        public static void PlayerWaypointUpdateService(CharacterType character, int waypointIndex)
        {
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)character, waypointIndex);
        }

        public static void PlayerReachedWaypointService(CharacterType character)
        {
            _singleton._PendingCharInteractions[(int)character].ended = true;
        }

        

        public static void GameElementOverService(in LevelElemInfo info)
        {
            if (_singleton != null)
            {
                bool changed = false;

                /* Add/Remove and update */
                if (info.active)
                {
                    _singleton._HoveredPending.Add(info);
                }
                else
                {
                    if ((_singleton._HoveredElem.family == info.family) && (_singleton._HoveredElem.item == info.item))
                    {
                        _singleton._HoveredElem = LevelElemInfo.EMPTY;
                        changed = true;
                    }
                }

                foreach (LevelElemInfo iteratedInfo in _singleton._HoveredPending)
                {
                    if (_singleton._HoveredElem.family < iteratedInfo.family)
                    {
                        _singleton._HoveredElem = iteratedInfo;
                        changed = true;
                    }
                }

                if (changed)
                {
                    VARMAP_LevelMaster.SET_ITEM_HOVER(_singleton._HoveredElem.item);
                }
            }
        }

        


        #endregion


        #region "Internal Services"

        public static void WPListRegister(IReadOnlyList<Vector2> waypointList, IReadOnlyList<WaypointSolution> solutions)
        {
            if (_singleton != null)
            {
                _singleton._WP_List = waypointList;
                _singleton._WP_Solution_List = solutions;
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
            VARMAP_LevelMaster.SET_PLAYER_SELECTED(CharacterType.CHARACTER_NONE);
            VARMAP_LevelMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_LevelMaster);
            mainCamera = Camera.main;
        }


        private void Update()
        {
            bool events_processed = VARMAP_LevelMaster.GET_EVENTS_BEING_PROCESSED();
            ref readonly MousePropertiesStruct mouse = ref VARMAP_LevelMaster.GET_MOUSE_PROPERTIES();

            _HoveredPending.Clear();
            UpdateHoverElements(in mouse);


            if (!events_processed)
            {
                Game_Status gstatus = VARMAP_LevelMaster.GET_GAMESTATUS();
               
                ref readonly KeyStruct keys = ref VARMAP_LevelMaster.GET_PRESSED_KEYS();

                switch (gstatus)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        Update_Play(gstatus, in mouse, in keys);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                    case Game_Status.GAME_STATUS_PLAY_MEMENTO:
                        if (keys.isKeyCycleReleased(KeyFunctions.KEYFUNC_INVENTORY))
                        {
                            VARMAP_LevelMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                            ClearHovered();
                        }
                        break;

                    default:
                        break;
                }
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
            _Player_List = new PlayableCharScript[(int)CharacterType.CHARACTER_TOTAL];
            _ItemDictionary = new Dictionary<GameItem, GameElementClass>(GameFixedConfig.MAX_POOLED_ITEMS);
            _Door_List = new List<DoorClass>(GameFixedConfig.MAX_SCENE_DOORS);
            _RaycastedItems = new(GameFixedConfig.MAX_RAYCASTED_ITEMS);
            _PrevRaycastedItems = new(GameFixedConfig.MAX_RAYCASTED_ITEMS);
            _RaycastedItemColliders = new RaycastHit2D[GameFixedConfig.MAX_RAYCASTED_ITEMS];
            _ItemColliderDictionary = new(GameFixedConfig.MAX_POOLED_ITEMS);


            _LayerOnlyPlayers = 0x1 << LayerMask.NameToLayer("PlayerLayer");
            _LayerPlayersAndItemsNPC = _LayerOnlyPlayers | (0x1 << LayerMask.NameToLayer("ItemLayer"));

            _playMouseArea = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height * GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT);

            _PendingCharInteractions = new PendingCharacterInteraction[(int)CharacterType.CHARACTER_TOTAL];
            _HoveredElem = LevelElemInfo.EMPTY;
            _HoveredPending = new(GameFixedConfig.MAX_RAYCASTED_ITEMS);
        }





        private void Update_Play(Game_Status gstatus, in MousePropertiesStruct mouse, in KeyStruct keys)
        {
            for(int i = 0; i < (int)CharacterType.CHARACTER_TOTAL; i++)
            {
                ref readonly PendingCharacterInteraction pending = ref _PendingCharInteractions[i];

                if(pending.ended)
                {
                    ExecutePlayerEndOfInteraction((CharacterType)i);
                }
            }
            UpdateMouseEvents(gstatus, in mouse, in keys);
        }

        private void UpdateHoverElements(in MousePropertiesStruct mouse)
        {
            Ray ray = mainCamera.ScreenPointToRay(new(mouse.posPixels.x,mouse.posPixels.y));
            int matches = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _RaycastedItemColliders, float.PositiveInfinity, _LayerPlayersAndItemsNPC);

            _RaycastedItems.Clear();

            for(int i=0;i<matches;++i)
            {
                if (_ItemColliderDictionary.TryGetValue(_RaycastedItemColliders[i].collider, out IGameObjectHoverable hoverable))
                {
                    _RaycastedItems.Add(hoverable);
                }
            }

            /* Remain only items which are not common between previous and actual cycle  */
            /* This will make system forget about elements which are in continuous hover or absence of */
            _PrevRaycastedItems.SymmetricExceptWith(_RaycastedItems);

            /* If there is at least one difference, recalculate */
            if (_PrevRaycastedItems.Count > 0)
            {
                /* Actuals (+) Differences = Actuals (+) Previous */
                _PrevRaycastedItems.UnionWith(_RaycastedItems);

                foreach (IGameObjectHoverable hoverable in _PrevRaycastedItems)
                {
                    /* Enter (or ReEnter) */
                    if (_RaycastedItems.Contains(hoverable))
                    {
                        hoverable.OnHover(true);
                    }
                    /* Exit */
                    else
                    {
                        hoverable.OnHover(false);
                    }
                }
            }

            /* Take actual ones, which will be used as previous */
            _PrevRaycastedItems.Clear();
            _PrevRaycastedItems.UnionWith(_RaycastedItems);
        }


        private void UpdateMouseEvents(Game_Status gstatus, in MousePropertiesStruct mouse, in KeyStruct keys)
        {
            if (_playMouseArea.Contains(mouse.posPixels))
            {
                UpdateCharItemMouseEvents(in mouse, in keys);
            }
        }

        

        private void UpdateCharItemMouseEvents(in MousePropertiesStruct mouse, in KeyStruct keys)
        {
            GameItem chosenItem = VARMAP_LevelMaster.GET_PICKABLE_ITEM_CHOSEN();
            CharacterType playerSelected = VARMAP_LevelMaster.GET_PLAYER_SELECTED();
            

            
            /* If no items menu or just a menu opened */
            if (keys.isKeyCycleReleased(KeyFunctions.KEYFUNC_INVENTORY))
            {
                if ((chosenItem == GameItem.ITEM_NONE) && (playerSelected != CharacterType.CHARACTER_NONE))
                {
                    VARMAP_LevelMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_ITEM_MENU, out _);
                    ClearHovered();
                }
                else
                {
                    VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                }
            }
            else if (keys.isKeyCycleReleased(KeyFunctions.KEYFUNC_SELECT))
            {
                /* If there is buffered action */
                if (_HoveredElem.family != GameItemFamily.ITEM_FAMILY_TYPE_NONE)
                {
                    CheckInteractionWithHoveredItem(playerSelected, chosenItem, in _HoveredElem);
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

        private void CheckInteractionWithHoveredItem(CharacterType playerSelected, GameItem chosenItem, in LevelElemInfo hovered)
        {
            InteractionUsage usage = default;
            bool accepted = false;

            switch (hovered.family)
            {
                case GameItemFamily.ITEM_FAMILY_TYPE_PLAYER:
                    if ((chosenItem != GameItem.ITEM_NONE) && (playerSelected != CharacterType.CHARACTER_NONE))
                    {
                        accepted = InteractWithItem(playerSelected, chosenItem, ref usage, in hovered);
                    }
                    else
                    {
                        /* This cast works just because first declared items match in same order as characters (chiripa) */
                        VARMAP_LevelMaster.SET_PLAYER_SELECTED((CharacterType)hovered.item);
                        VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                    }
                    break;

                case GameItemFamily.ITEM_FAMILY_TYPE_OBJECT:
                    accepted = InteractWithItem(playerSelected, chosenItem, ref usage, in hovered);
                    break;


                case GameItemFamily.ITEM_FAMILY_TYPE_DOOR:
                    usage = InteractionUsage.CreateTakeItem(playerSelected, hovered.item, hovered.waypoint);

                    VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, hovered.waypoint, out accepted);
                    VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                    break;

                default:
                    break;
            }

            if (accepted)
            {
                _PendingCharInteractions[(int)playerSelected] = new PendingCharacterInteraction(in usage);
            }
        }

        private bool InteractWithItem(CharacterType playerSelected,GameItem chosenItem, ref InteractionUsage usage, in LevelElemInfo hovered)
        {
            bool accepted = false;

            if (playerSelected != CharacterType.CHARACTER_NONE)
            {
                if (chosenItem == GameItem.ITEM_NONE)
                {
                    UserInputInteraction userInteraction = VARMAP_LevelMaster.GET_USER_INPUT_INTERACTION();

                    switch (userInteraction)
                    {
                        case UserInputInteraction.INPUT_INTERACTION_TAKE:
                            usage = InteractionUsage.CreateTakeItem(playerSelected, hovered.item,hovered.waypoint);
                            break;
                        case UserInputInteraction.INPUT_INTERACTION_TALK:
                            usage = InteractionUsage.CreateTalkItem(playerSelected, hovered.item, hovered.waypoint);
                            break;
                        default:
                            usage = InteractionUsage.CreateObserveItem(playerSelected, hovered.item, hovered.waypoint);
                            break;
                    }
                }
                else
                {
                    usage = InteractionUsage.CreateUseItemWithItem(playerSelected, chosenItem,
                        hovered.item, hovered.waypoint);
                }

                VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, hovered.waypoint, out accepted);
            }

            return accepted;
        }

        private void CheckPlayerMovementOrder(in MousePropertiesStruct mouse, CharacterType selectedCharacter)
        {
            GetNearestWPService(mouse.pos1, GameFixedConfig.DISTANCE_MOUSE_FURTHEST_WP, out int candidate_index, out Vector2 candidate_pos);

            if (candidate_index >= 0)
            {
                InteractionUsage usage = InteractionUsage.CreatePlayerMove(selectedCharacter, candidate_index);

                VARMAP_LevelMaster.INTERACT_PLAYER(selectedCharacter, candidate_index, out bool accepted);
                if (accepted)
                {
                    _PendingCharInteractions[(int)selectedCharacter] = new PendingCharacterInteraction(in usage);
                }
            }
        }

        private void ExecutePlayerEndOfInteraction(CharacterType character)
        {
            /* Generate stack array of 2 talkers, player and dst */
            Span<GameItem> talkers = stackalloc GameItem[2];

            ref PendingCharacterInteraction charPendingAction = ref _PendingCharInteractions[(int)character];
            ref readonly InteractionUsage usage = ref charPendingAction.usage;

            /* Now interact if buffered */
            if (charPendingAction.pending && (usage.type != ItemInteractionType.INTERACTION_MOVE))
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
                        validTransaction &= _ItemDictionary[usage.itemDest].Waypoint == usage.destWaypoint_index;


                        if (validTransaction)
                        {
                            /* Use Item is also Take Item */
                            VARMAP_LevelMaster.USE_ITEM(in usage, out InteractionUsageOutcome outcome);

                            if (outcome.dialogType != DialogType.DIALOG_NONE)
                            {
                                /* Default talkers are own player and itemDest */
                                talkers[0] = _Player_List[(int)usage.playerSource].ItemID;
                                talkers[1] = usage.itemDest;

                                /* No need to know about error, as this function is executed from play mode */
                                VARMAP_LevelMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_DIALOG, out _);
                                VARMAP_LevelMaster.SHOW_DIALOGUE(talkers, outcome.dialogType, outcome.dialogPhrase);
                            }
                            else if (outcome.animation != CharacterAnimation.ITEM_USE_ANIMATION_NONE)
                            {
                                //ActAnimationRequest(outcome.animation);
                            }
                            else
                            {
                                /**/
                            }

                            VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                        }
                        break;
                }
            }

            /* Pending and ended no more */
            charPendingAction.pending = false;
            charPendingAction.ended = false;
        }

        private void CrossDoor(CharacterType character, int doorIndex)
        {
            DoorClass door = _Door_List[doorIndex];

            _ = character;  /* TODO: out animation? Or should have been done before calling Cross Door? */

            int waypointIndex = LevelOptionsClass.GetLevelDoorToWaypoint(door.RoomLead, door.RoomAppearPosition);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_MAIN, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_PARROT, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_SNAKE, waypointIndex);

            VARMAP_LevelMaster.LOAD_ROOM(door.RoomLead, out _);
        }

        private void ClearHovered()
        {
            _HoveredPending.Clear();
            _HoveredElem = LevelElemInfo.EMPTY;
            GameElementOverService(in LevelElemInfo.EMPTY);
        }
        private bool IsItemAvailable(GameItem item)
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

