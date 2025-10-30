using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;

namespace Gob3AQ.VARMAP.GameEventMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_GameEventMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SHADOW_ELEM_EVENTS_OCCURRED = _GET_SHADOW_ELEM_EVENTS_OCCURRED;
            SET_ELEM_EVENTS_OCCURRED = _SET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_SHADOW_ARRAY_EVENTS_OCCURRED = _GET_SHADOW_ARRAY_EVENTS_OCCURRED;
            SET_ARRAY_EVENTS_OCCURRED = _SET_ARRAY_EVENTS_OCCURRED;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            ITEM_OBTAIN_PICKABLE_EVENT = _ITEM_OBTAIN_PICKABLE_EVENT;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_SHADOW_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_SHADOW_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_EVENTS_OCCURRED;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        public static LODING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, GameMenu, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
        /// Takes an item from scene (triggering event)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.ItemObtainPickableEventService"/> </para> 
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE ITEM_OBTAIN_PICKABLE_EVENT;
        /// <summary> 
        /// Checks if a combination of events is totally complied (event absence can also be requested)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventCombiOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_COMBI_OCCURRED_DELEGATE IS_EVENT_COMBI_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.CommitEventService"/> </para> 
        /// </summary>
        public static COMMIT_EVENT_DELEGATE COMMIT_EVENT;
        /// <summary> 
        /// Tells if a pickable item has already been picked in game
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsItemTakenFromSceneService"/> </para> 
        /// </summary>
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
        /// <summary> 
        /// Subscribe to an event. Invoke when event changes
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, PlayerMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.EventSubscriptionService"/> </para> 
        /// </summary>
        public static EVENT_SUBSCRIPTION_DELEGATE EVENT_SUBSCRIPTION;
        /* > ATG 3 END */
    }
}
