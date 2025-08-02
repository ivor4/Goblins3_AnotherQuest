using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.NPCMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.InputMaster;

namespace Gob3AQ.VARMAP.PlayerMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_PlayerMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_ELEM_PLAYER_ACTUAL_WAYPOINT = _GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
            GET_SIZE_PLAYER_ACTUAL_WAYPOINT = _GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
            GET_ARRAY_PLAYER_ACTUAL_WAYPOINT = _GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_SHADOW_PLAYER_SELECTED = _GET_SHADOW_PLAYER_SELECTED;
            SET_PLAYER_SELECTED = _SET_PLAYER_SELECTED;
            REG_PLAYER_SELECTED = _REG_PLAYER_SELECTED;
            UNREG_PLAYER_SELECTED = _UNREG_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            GET_ELEM_PLAYER_TRANSACTION = _GET_ELEM_PLAYER_TRANSACTION;
            GET_SHADOW_ELEM_PLAYER_TRANSACTION = _GET_SHADOW_ELEM_PLAYER_TRANSACTION;
            SET_ELEM_PLAYER_TRANSACTION = _SET_ELEM_PLAYER_TRANSACTION;
            GET_SIZE_PLAYER_TRANSACTION = _GET_SIZE_PLAYER_TRANSACTION;
            GET_ARRAY_PLAYER_TRANSACTION = _GET_ARRAY_PLAYER_TRANSACTION;
            GET_SHADOW_ARRAY_PLAYER_TRANSACTION = _GET_SHADOW_ARRAY_PLAYER_TRANSACTION;
            SET_ARRAY_PLAYER_TRANSACTION = _SET_ARRAY_PLAYER_TRANSACTION;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            MONO_REGISTER = _MONO_REGISTER;
            PLAYER_WAYPOINT_UPDATE = _PLAYER_WAYPOINT_UPDATE;
            SELECT_PLAYER = _SELECT_PLAYER;
            GET_PLAYER_LIST = _GET_PLAYER_LIST;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            USE_ITEM = _USE_ITEM;
            INTERACT_PLAYER = _INTERACT_PLAYER;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            CROSS_DOOR = _CROSS_DOOR;
            INTERACT_PLAYER_NPC = _INTERACT_PLAYER_NPC;
            LOCK_PLAYER = _LOCK_PLAYER;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayElemValueDelegate<int> GET_ELEM_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPArrayDelegate<int> GET_ARRAY_PLAYER_ACTUAL_WAYPOINT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<CharacterType> GET_SHADOW_PLAYER_SELECTED;
        public static SetVARMAPValueDelegate<CharacterType> SET_PLAYER_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> REG_PLAYER_SELECTED;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<CharacterType> UNREG_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_ELEM_PLAYER_TRANSACTION;
        public static GetVARMAPArrayElemValueDelegate<ulong> GET_SHADOW_ELEM_PLAYER_TRANSACTION;
        public static SetVARMAPArrayElemValueDelegate<ulong> SET_ELEM_PLAYER_TRANSACTION;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PLAYER_TRANSACTION;
        public static GetVARMAPArrayDelegate<ulong> GET_ARRAY_PLAYER_TRANSACTION;
        public static GetVARMAPArrayDelegate<ulong> GET_SHADOW_ARRAY_PLAYER_TRANSACTION;
        public static SetVARMAPArrayDelegate<ulong> SET_ARRAY_PLAYER_TRANSACTION;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// This service subscribes for late start. This happens at some moment after Start event. when everything has been setup
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, NPCMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LateStartSubrsciptionService"/> </para> 
        /// </summary>
        public static LATE_START_SUBSCRIPTION_DELEGATE LATE_START_SUBSCRIPTION;
        /// <summary> 
        /// Registers a player in scene
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.MonoRegisterService"/> </para> 
        /// </summary>
        public static MONO_REGISTER_DELEGATE MONO_REGISTER;
        /// <summary> 
        /// Updates actual player waypoint when crossing or stopping on it
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.PlayerWaypointUpdateService"/> </para> 
        /// </summary>
        public static PLAYER_WAYPOINT_UPDATE_DELEGATE PLAYER_WAYPOINT_UPDATE;
        /// <summary> 
        /// Selects player
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.SelectPlayerService"/> </para> 
        /// </summary>
        public static SELECT_PLAYER_DELEGATE SELECT_PLAYER;
        /// <summary> 
        /// Gets a list of actual players
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: GraphicsMaster, PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetPlayerListService"/> </para> 
        /// </summary>
        public static GET_PLAYER_LIST_DELEGATE GET_PLAYER_LIST;
        /// <summary> 
        /// Gets nearest WP from a given coordinates of level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster, NPCMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetNearestWPService"/> </para> 
        /// </summary>
        public static GET_NEAREST_WP_DELEGATE GET_NEAREST_WP;
        /// <summary> 
        /// Tells if an event is occurred
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, PlayerMaster, NPCMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_OCCURRED_DELEGATE IS_EVENT_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, PlayerMaster, NPCMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Uses an item with something
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.UseItemService"/> </para> 
        /// </summary>
        public static USE_ITEM_DELEGATE USE_ITEM;
        /// <summary> 
        /// Interacts player with an item
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.InteractPlayerService"/> </para> 
        /// </summary>
        public static INTERACT_PLAYER_DELEGATE INTERACT_PLAYER;
        /// <summary> 
        /// Cancels selected item
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.CancelPickableItemService"/> </para> 
        /// </summary>
        public static CANCEL_PICKABLE_ITEM_DELEGATE CANCEL_PICKABLE_ITEM;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, PlayerMaster, NPCMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.EventSubscriptionService"/> </para> 
        /// </summary>
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /// <summary> 
        /// Trigger actions when crossing a door
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.CrossDoorService"/> </para> 
        /// </summary>
        public static CROSS_DOOR_DELEGATE CROSS_DOOR;
        /// <summary> 
        /// Interacts player with NPC
        /// <para> Owner: NPCMaster </para> 
        /// <para> Accessors: PlayerMaster,  </para> 
        /// <para> Method: <see cref="NPCMasterClass.InteractPlayerNPCService"/> </para> 
        /// </summary>
        public static INTERACT_PLAYER_NPC_DELEGATE INTERACT_PLAYER_NPC;
        /// <summary> 
        /// Locks player so it cannot act until an action over it has been done (or removes lock)
        /// <para> Owner: PlayerMaster </para> 
        /// <para> Accessors: LevelMaster,  </para> 
        /// <para> Method: <see cref="PlayerMasterClass.LockPlayerService"/> </para> 
        /// </summary>
        public static LOCK_PLAYER_DELEGATE LOCK_PLAYER;
        /* > ATG 3 END */
    }
}
