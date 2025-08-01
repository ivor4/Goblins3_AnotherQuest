using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;

namespace Gob3AQ.VARMAP.GameEventMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public abstract class VARMAP_GameEventMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            SET_ELEM_EVENTS_OCCURRED = _SET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            SET_ARRAY_EVENTS_OCCURRED = _SET_ARRAY_EVENTS_OCCURRED;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            LATE_START_SUBSCRIPTION = _LATE_START_SUBSCRIPTION;
            ITEM_OBTAIN_PICKABLE_EVENT = _ITEM_OBTAIN_PICKABLE_EVENT;
            IS_EVENT_OCCURRED = _IS_EVENT_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            IS_ITEM_TAKEN_FROM_SCENE = _IS_ITEM_TAKEN_FROM_SCENE;
            EVENT_SUBSCRIPTION = _EVENT_SUBSCRIPTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
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
        /// Takes an item from scene (triggering event)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.ItemObtainPickableEventService"/> </para> 
        /// </summary>
        public static ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE ITEM_OBTAIN_PICKABLE_EVENT;
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
        /// Tells if item is taken from scene
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsItemTakenFromSceneService"/> </para> 
        /// </summary>
        public static IS_ITEM_TAKEN_FROM_SCENE_DELEGATE IS_ITEM_TAKEN_FROM_SCENE;
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
