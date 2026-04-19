using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.LevelMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.LevelMaster
{
    public class LevelMasterClass : MonoBehaviour
    {
        private Rect _playMouseArea;
        private int _LayerOnlyPlayers;
        private int _LayerPlayersAndItemsNPC;

        private static LevelMasterClass _singleton;

        private IReadOnlyList<WaypointInfo> _WP_Info_List;
        private PlayableCharScript[] _Player_List;
        private Dictionary<GameItem, DoorInfo> _Door_Dict;
        private Dictionary<GameItem, GameElementClass> _ItemDictionary;
        private LevelElemInfo _HoveredElem;
        private HashSet<LevelElemInfo> _HoveredPending;
        private PendingCharacterInteraction[] _PendingCharInteractions;
        private HashSet<IGameObjectHoverable> _RaycastedItems;
        private HashSet<IGameObjectHoverable> _PrevRaycastedItems;
        private RaycastHit2D[] _RaycastedItemColliders;
        private Dictionary<Collider2D, IGameObjectHoverable> _ItemColliderDictionary;
        private ItemMenuHoverable itemMenuHoverable;
        private Camera mainCamera;
        private bool somePlayerRegistered;


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
            Span<GameEventCombi> gameEventCombis = stackalloc GameEventCombi[1];
            if (_singleton != null)
            {
                float minDistance = float.PositiveInfinity;
                candidate_pos = Vector2.zero;
                candidate_index = -1;

                for (int i = 0; i < _singleton._WP_Info_List.Count; ++i)
                {
                    WaypointInfo wpinfo = _singleton._WP_Info_List[i];

                    bool pass;

                    switch(wpinfo.Reachability)
                    {
                        case WaypointReachability.REACHABLE_ALWAYS:
                            pass = true;
                            break;
                        case WaypointReachability.REACHABLE_WHEN_COMBI:
                            GameEventCombi combi = new(wpinfo.NeededEvent.ev, wpinfo.NeededEvent.not);
                            gameEventCombis[0] = combi;
                            VARMAP_LevelMaster.IS_EVENT_COMBI_OCCURRED(gameEventCombis, out pass);
                            break;
                        default:
                            pass = false;
                            break;
                    }

                    if (pass)
                    {
                        Vector2 wp_pos = wpinfo.Position;
                        float dist = Vector2.Distance(wp_pos, position);

                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            candidate_pos = wp_pos;
                            candidate_index = i;
                        }
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

        public static void GetWaypointListService(out IReadOnlyList<WaypointInfo> infos)
        {
            if (_singleton != null)
            {
                infos = _singleton._WP_Info_List;
            }
            else
            {
                infos = null;
            }
        }


        public static void ItemRegisterService(bool register, GameElementClass instance)
        {
            if (_singleton != null)
            {
                if (register)
                {
                    _singleton._ItemDictionary.Add(instance.ItemID, instance);
                    _singleton._ItemColliderDictionary[instance.My2DCollider] = instance;
                }
                else
                {
                    _singleton._ItemDictionary.Remove(instance.ItemID);
                    _singleton._ItemColliderDictionary.Remove(instance.My2DCollider);
                }
            }
        }

        public static void MonoRegisterService(PlayableCharScript mono, bool add)
        {
            if (_singleton != null)
            {
                if (add)
                {
                    _singleton._Player_List[(int)mono.CharType] = mono;
                    _singleton.somePlayerRegistered = true;
                }
                else
                {
                    _singleton._Player_List[(int)mono.CharType] = null;
                }
            }
        }

        public static void DoorRegisterService(GameItem doorItem, bool add, in DoorInfo doorInfo)
        {
            if (_singleton != null)
            {
                if (add)
                {
                    _singleton._Door_Dict.Add(doorItem, doorInfo);
                }
                else
                {
                    _singleton._Door_Dict.Remove(doorItem);
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

        

        private void GameElementOverService(in LevelElemInfo info, bool enableMask)
        {
            bool changed = false;

            /* Add/Remove and update */
            if (info.active & enableMask)
            {
                _ = _HoveredPending.Add(info);
            }
            else
            {
                if ((_HoveredElem.family == info.family) && (_HoveredElem.item == info.item))
                {
                    _HoveredElem = LevelElemInfo.EMPTY;
                    changed = true;
                }
            }

            foreach (LevelElemInfo iteratedInfo in _HoveredPending)
            {
                if ((iteratedInfo.family > GameItemFamily.ITEM_FAMILY_TYPE_NONE) &&
                    (
                    (_HoveredElem.family < iteratedInfo.family) ||
                    (_HoveredElem.family == iteratedInfo.family) && (_HoveredElem.hoverPriority < iteratedInfo.hoverPriority)
                    )
                    )
                {
                    _HoveredElem = iteratedInfo;
                    changed = true;
                }
            }

            if (changed)
            {
                VARMAP_LevelMaster.SET_ITEM_HOVER(_HoveredElem.item);
            }
        }

        


        #endregion


        #region "Internal Services"

        public static void WPListRegister(IReadOnlyList<WaypointInfo> waypointInfoList)
        {
            if (_singleton != null)
            {
                _singleton._WP_Info_List = waypointInfoList;
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
            mainCamera = Camera.main;
            VARMAP_LevelMaster.SET_PLAYER_SELECTED(CharacterType.CHARACTER_NONE);
            VARMAP_LevelMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_LevelMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_LevelMaster);
        }

        private IEnumerator LoadCoroutine()
        {
            while (!somePlayerRegistered)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
            }
            yield return ResourceAtlasClass.WaitForNextFrame;

            for (int i=0;i< _Player_List.Length; ++i)
            {
                if (_Player_List[i] != null)
                {
                    VARMAP_LevelMaster.SET_PLAYER_SELECTED(_Player_List[i].CharType);
                    break;
                }
            }

            VARMAP_LevelMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_LevelMaster);
        }

        private void Update()
        {
            BusyState busyState = VARMAP_LevelMaster.GET_BUSY_STATE();
            ref readonly MousePropertiesStruct mouse = ref VARMAP_LevelMaster.GET_MOUSE_PROPERTIES();

            _HoveredPending.Clear();
            UpdateHoverElements(in mouse);


            if (busyState == BusyState.GAME_NOT_BUSY)
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
                VARMAP_LevelMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }


        private void Initializations()
        {
            _Player_List = new PlayableCharScript[(int)CharacterType.CHARACTER_TOTAL];
            _ItemDictionary = new Dictionary<GameItem, GameElementClass>(GameFixedConfig.MAX_POOLED_ITEMS);
            _Door_Dict = new(GameFixedConfig.MAX_SCENE_DOORS);
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

            itemMenuHoverable = new();

            somePlayerRegistered = false;
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

            if (_playMouseArea.Contains(mouse.posPixels))
            {
                for (int i = 0; i < matches; ++i)
                {
                    if (_ItemColliderDictionary.TryGetValue(_RaycastedItemColliders[i].collider, out IGameObjectHoverable hoverable))
                    {
                        _RaycastedItems.Add(hoverable);
                    }
                }


                /* Add Item menu selected element as one more */
                GameItem menuItemHovered = VARMAP_LevelMaster.GET_ITEM_MENU_HOVER();
                if (menuItemHovered != GameItem.ITEM_NONE)
                {
                    itemMenuHoverable.SetHoverInfo(new LevelElemInfo(menuItemHovered, GameItemFamily.ITEM_FAMILY_TYPE_OBJECT, -1, 0, true));
                    _RaycastedItems.Add(itemMenuHoverable);
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
                    LevelElemInfo hoverableInfo = hoverable.GetHoverableLevelElemInfo();

                    /* Enter (or ReEnter) */
                    if (_RaycastedItems.Contains(hoverable))
                    {
                        GameElementOverService(in hoverableInfo, true);
                    }
                    /* Exit */
                    else
                    {
                        GameElementOverService(in hoverableInfo, false);
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
                        /* Item to Character "Dict" */
                        for(int i=0; i < (int)CharacterType.CHARACTER_TOTAL; ++i)
                        {
                            PlayableCharScript instance = _Player_List[i];
                            if ((instance != null) && (instance.ItemID == hovered.item))
                            {
                                VARMAP_LevelMaster.SET_PLAYER_SELECTED(instance.CharType);
                                break;
                            }
                        }
                        
                        VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                    }
                    break;

                case GameItemFamily.ITEM_FAMILY_TYPE_OBJECT:
                case GameItemFamily.ITEM_FAMILY_TYPE_NPC:
                    accepted = InteractWithItem(playerSelected, chosenItem, ref usage, in hovered);
                    break;


                case GameItemFamily.ITEM_FAMILY_TYPE_DOOR:
                    accepted = InteractWithDoor(playerSelected, ref usage, in hovered);
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

        private bool InteractWithDoor(CharacterType playerSelected, ref InteractionUsage usage, in LevelElemInfo hovered)
        {
            bool accepted = false;

            if (playerSelected != CharacterType.CHARACTER_NONE)
            {
                usage = InteractionUsage.CreateCrossDoor(playerSelected, hovered.item, hovered.waypoint);

                VARMAP_LevelMaster.INTERACT_PLAYER(playerSelected, hovered.waypoint, out accepted);
                VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
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
            ref PendingCharacterInteraction charPendingAction = ref _PendingCharInteractions[(int)character];
            ref readonly InteractionUsage usage = ref charPendingAction.usage;

            /* Now interact if buffered */
            if (charPendingAction.pending)
            {
                GameAction actionWhenCross = _WP_Info_List[_Player_List[(int)character].Waypoint].ActionWhenCross;
                if (actionWhenCross != GameAction.ACTION_NONE)
                {
                    Span<GameAction> stackAction = stackalloc GameAction[1];
                    stackAction[0] = actionWhenCross;
                    VARMAP_LevelMaster.PERFORM_ACTION(stackAction, null);
                }
                else if (charPendingAction.pending && (usage.type != ItemInteractionType.INTERACTION_MOVE))
                {
                    ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemDest);

                    /* Check if it is available and is still in original position */
                    bool validTransaction = IsItemAvailable(usage.itemDest);
                    validTransaction &= _ItemDictionary[usage.itemDest].Waypoint == usage.destWaypoint_index;


                    if (validTransaction)
                    {
                        /* Use Item is also Take Item */
                        VARMAP_LevelMaster.USE_ITEM(in usage, out InteractionUsageOutcome outcome);

                        if ((usage.type == ItemInteractionType.INTERACTION_CROSS_DOOR) && (!outcome.ok))
                        {
                            /* May sound strange, if no special conditions, door can be crossed */
                            CrossDoor(usage.itemDest);
                        }

                        VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                    }
                }
                else
                {
                    /**/
                }
            }

            /* Pending and ended no more */
            charPendingAction.pending = false;
            charPendingAction.ended = false;
        }

        private void PlayerEntryWalk()
        {
            Span<GameEventCombi> stackCombi = stackalloc GameEventCombi[1];
            stackCombi[0] = new GameEventCombi(GameEvent.EVENT_MASTER_CHANGE_ROOM, false);

            VARMAP_LevelMaster.IS_EVENT_COMBI_OCCURRED(stackCombi, out bool occurred);

            if(occurred)
            {
                Room room = VARMAP_LevelMaster.GET_ACTUAL_ROOM();

                for (int i=0; i < _Player_List.Length; ++i)
                {
                    if (_Player_List[i] != null)
                    {
                        ReadOnlySpan<InitialWalkInfo> walkInfos = ResourceAtlasClass.GetInitialWalkInfo(room);

                        foreach (InitialWalkInfo walkInfo in walkInfos)
                        {
                            if ((walkInfo.waypointFrom == _Player_List[i].Waypoint)&&(walkInfo.waypointFrom != walkInfo.waypointTo))
                            {
                                InteractionUsage usage = InteractionUsage.CreatePlayerMove((CharacterType)i, walkInfo.waypointTo);

                                VARMAP_LevelMaster.INTERACT_PLAYER(_Player_List[i].CharType, walkInfo.waypointTo, out bool accepted);
                                if (accepted)
                                {
                                    _PendingCharInteractions[i] = new PendingCharacterInteraction(in usage);
                                }
                                break;
                            }
                        }
                    }
                }
                
            }
        }

        private void CrossDoor(GameItem doorItem)
        {
            DoorInfo doorInfo = _Door_Dict[doorItem];

            int waypointIndex = doorInfo.waypointLeadTo;
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_MAIN, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_PARROT, waypointIndex);
            VARMAP_LevelMaster.SET_ELEM_PLAYER_ACTUAL_WAYPOINT((int)CharacterType.CHARACTER_SNAKE, waypointIndex);
            VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
            VARMAP_LevelMaster.SET_PLAYER_SELECTED(CharacterType.CHARACTER_NONE);

            VARMAP_LevelMaster.LOAD_ROOM(doorInfo.roomLeadTo, out _);
        }

        private void ClearHovered()
        {
            _HoveredPending.Clear();
            _HoveredElem = LevelElemInfo.EMPTY;
            GameElementOverService(in LevelElemInfo.EMPTY, false);
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

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        Array.Clear(_Player_List, 0, _Player_List.Length);
                        _ItemDictionary.Clear();
                        _Door_Dict.Clear();
                        _RaycastedItems.Clear();
                        _PrevRaycastedItems.Clear();
                        Array.Clear(_RaycastedItemColliders, 0, _RaycastedItemColliders.Length);
                        _ItemColliderDictionary.Clear();
                        somePlayerRegistered = false;

                        Array.Clear(_PendingCharInteractions, 0, _PendingCharInteractions.Length);
                        _HoveredElem = LevelElemInfo.EMPTY;
                        _HoveredPending.Clear();
                        VARMAP_LevelMaster.CANCEL_PICKABLE_ITEM();
                        VARMAP_LevelMaster.SET_ITEM_HOVER(_HoveredElem.item);

                        _WP_Info_List = null;
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        StartCoroutine(LoadCoroutine());
                        break;

                    case Game_Status.GAME_STATUS_PLAY:
                        /* Force new status */
                        _PrevRaycastedItems.Clear();

                        /* Player entry */
                        if(oldval == Game_Status.GAME_STATUS_LOADING)
                        {
                            PlayerEntryWalk();
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

