using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.VARMAP.ItemMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_ItemMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_PICKABLE_ITEM_OWNER = _GET_ELEM_PICKABLE_ITEM_OWNER;
            SET_ELEM_PICKABLE_ITEM_OWNER = _SET_ELEM_PICKABLE_ITEM_OWNER;
            GET_SIZE_PICKABLE_ITEM_OWNER = _GET_SIZE_PICKABLE_ITEM_OWNER;
            GET_ARRAY_PICKABLE_ITEM_OWNER = _GET_ARRAY_PICKABLE_ITEM_OWNER;
            SET_ARRAY_PICKABLE_ITEM_OWNER = _SET_ARRAY_PICKABLE_ITEM_OWNER;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            GET_PICKABLE_ITEM_CHOSEN = _GET_PICKABLE_ITEM_CHOSEN;
            SET_PICKABLE_ITEM_CHOSEN = _SET_PICKABLE_ITEM_CHOSEN;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            ITEM_REGISTER = _ITEM_REGISTER;
            ITEM_OBTAIN_PICKABLE = _ITEM_OBTAIN_PICKABLE;
            ITEM_OBTAIN_PICKABLE_EVENT = _ITEM_OBTAIN_PICKABLE_EVENT;
            GET_NEAREST_WP = _GET_NEAREST_WP;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            USE_ITEM = _USE_ITEM;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            GET_SCENARIO_ITEM_LIST = _GET_SCENARIO_ITEM_LIST;
            SELECT_PICKABLE_ITEM = _SELECT_PICKABLE_ITEM;
            CANCEL_PICKABLE_ITEM = _CANCEL_PICKABLE_ITEM;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<CharacterType> GET_ELEM_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayElemValueDelegate<CharacterType> SET_ELEM_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArraySizeDelegate GET_SIZE_PICKABLE_ITEM_OWNER;
        public static GetVARMAPArrayDelegate<CharacterType> GET_ARRAY_PICKABLE_ITEM_OWNER;
        public static SetVARMAPArrayDelegate<CharacterType> SET_ARRAY_PICKABLE_ITEM_OWNER;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
        public static GetVARMAPValueDelegate<GameItem> GET_PICKABLE_ITEM_CHOSEN;
        public static SetVARMAPValueDelegate<GameItem> SET_PICKABLE_ITEM_CHOSEN;
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
        /// Registers an item in system
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ItemRegisterService"/> </para> 
        /// </summary>
        public static ITEM_REGISTER_DELEGATE ITEM_REGISTER;
        /// <summary> 
        /// Removes an item from level
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.ItemObtainPickableService"/> </para> 
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_DELEGATE ITEM_OBTAIN_PICKABLE;
        /// <summary> 
        /// Takes an item from scene (triggering event)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.ItemObtainPickableEventService"/> </para> 
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE ITEM_OBTAIN_PICKABLE_EVENT;
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
        /// Tells if item is taken from scene
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsItemTakenFromSceneService"/> </para> 
        /// </summary>
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        /// <summary> 
        /// Gets scenario item list
        /// <para> Owner: LevelMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="LevelMasterClass.GetScenarioItemListService"/> </para> 
        /// </summary>
        public static GET_SCENARIO_ITEM_LIST_DELEGATE GET_SCENARIO_ITEM_LIST;
        /// <summary> 
        /// Selects some pickable from inventory
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.SelectPickableItemService"/> </para> 
        /// </summary>
        public static SELECT_PICKABLE_ITEM_DELEGATE SELECT_PICKABLE_ITEM;
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
        /* > ATG 3 END */
    }
}
