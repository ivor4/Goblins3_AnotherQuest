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
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_ELEM_EVENTS_OCCURRED = _GET_ELEM_EVENTS_OCCURRED;
            GET_SHADOW_ELEM_EVENTS_OCCURRED = _GET_SHADOW_ELEM_EVENTS_OCCURRED;
            SET_ELEM_EVENTS_OCCURRED = _SET_ELEM_EVENTS_OCCURRED;
            GET_SIZE_EVENTS_OCCURRED = _GET_SIZE_EVENTS_OCCURRED;
            GET_ARRAY_EVENTS_OCCURRED = _GET_ARRAY_EVENTS_OCCURRED;
            GET_SHADOW_ARRAY_EVENTS_OCCURRED = _GET_SHADOW_ARRAY_EVENTS_OCCURRED;
            SET_ARRAY_EVENTS_OCCURRED = _SET_ARRAY_EVENTS_OCCURRED;
            GET_ELEM_UNLOCKED_MEMENTO = _GET_ELEM_UNLOCKED_MEMENTO;
            GET_SHADOW_ELEM_UNLOCKED_MEMENTO = _GET_SHADOW_ELEM_UNLOCKED_MEMENTO;
            SET_ELEM_UNLOCKED_MEMENTO = _SET_ELEM_UNLOCKED_MEMENTO;
            GET_SIZE_UNLOCKED_MEMENTO = _GET_SIZE_UNLOCKED_MEMENTO;
            GET_ARRAY_UNLOCKED_MEMENTO = _GET_ARRAY_UNLOCKED_MEMENTO;
            GET_SHADOW_ARRAY_UNLOCKED_MEMENTO = _GET_SHADOW_ARRAY_UNLOCKED_MEMENTO;
            SET_ARRAY_UNLOCKED_MEMENTO = _SET_ARRAY_UNLOCKED_MEMENTO;
            GET_ELEM_UNWATCHED_PARENT_MEMENTO = _GET_ELEM_UNWATCHED_PARENT_MEMENTO;
            GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO = _GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO;
            SET_ELEM_UNWATCHED_PARENT_MEMENTO = _SET_ELEM_UNWATCHED_PARENT_MEMENTO;
            GET_SIZE_UNWATCHED_PARENT_MEMENTO = _GET_SIZE_UNWATCHED_PARENT_MEMENTO;
            GET_ARRAY_UNWATCHED_PARENT_MEMENTO = _GET_ARRAY_UNWATCHED_PARENT_MEMENTO;
            GET_SHADOW_ARRAY_UNWATCHED_PARENT_MEMENTO = _GET_SHADOW_ARRAY_UNWATCHED_PARENT_MEMENTO;
            SET_ARRAY_UNWATCHED_PARENT_MEMENTO = _SET_ARRAY_UNWATCHED_PARENT_MEMENTO;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_EVENTS_BEING_PROCESSED = _GET_EVENTS_BEING_PROCESSED;
            GET_SHADOW_EVENTS_BEING_PROCESSED = _GET_SHADOW_EVENTS_BEING_PROCESSED;
            SET_EVENTS_BEING_PROCESSED = _SET_EVENTS_BEING_PROCESSED;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            COMMIT_MEMENTO_NOTIF = _COMMIT_MEMENTO_NOTIF;
            IS_MEMENTO_UNLOCKED = _IS_MEMENTO_UNLOCKED;
            MEMENTO_PARENT_WATCHED = _MEMENTO_PARENT_WATCHED;
            UNCHAIN_TO_ITEM = _UNCHAIN_TO_ITEM;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_SHADOW_ELEM_EVENTS_OCCURRED;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_EVENTS_OCCURRED;
        public static GetVARMAPArraySizeDelegate GET_SIZE_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_SHADOW_ARRAY_EVENTS_OCCURRED;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_EVENTS_OCCURRED;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_UNLOCKED_MEMENTO;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_SHADOW_ELEM_UNLOCKED_MEMENTO;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_UNLOCKED_MEMENTO;
        public static GetVARMAPArraySizeDelegate GET_SIZE_UNLOCKED_MEMENTO;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_UNLOCKED_MEMENTO;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_SHADOW_ARRAY_UNLOCKED_MEMENTO;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_UNLOCKED_MEMENTO;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_ELEM_UNWATCHED_PARENT_MEMENTO;
        public static GetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> GET_SHADOW_ELEM_UNWATCHED_PARENT_MEMENTO;
        public static SetVARMAPArrayElemValueDelegate<MultiBitFieldStruct> SET_ELEM_UNWATCHED_PARENT_MEMENTO;
        public static GetVARMAPArraySizeDelegate GET_SIZE_UNWATCHED_PARENT_MEMENTO;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_ARRAY_UNWATCHED_PARENT_MEMENTO;
        public static GetVARMAPArrayDelegate<MultiBitFieldStruct> GET_SHADOW_ARRAY_UNWATCHED_PARENT_MEMENTO;
        public static SetVARMAPArrayDelegate<MultiBitFieldStruct> SET_ARRAY_UNWATCHED_PARENT_MEMENTO;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<bool> GET_EVENTS_BEING_PROCESSED;
        public static GetVARMAPValueDelegate<bool> GET_SHADOW_EVENTS_BEING_PROCESSED;
        public static SetVARMAPValueDelegate<bool> SET_EVENTS_BEING_PROCESSED;
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
        /// Tells Memento Manager (Menu) a new memento has been unlocked
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.CommitMementoNotifService"/> </para> 
        /// </summary>
        public static COMMIT_MEMENTO_NOTIF_DELEGATE COMMIT_MEMENTO_NOTIF;
        /// <summary> 
        /// Tells if a memento is unlocked
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsMementoUnlockedService"/> </para> 
        /// </summary>
        public static IS_MEMENTO_UNLOCKED_DELEGATE IS_MEMENTO_UNLOCKED;
        /// <summary> 
        /// If a Memento has been analyzed
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.MementoParentWatchedService"/> </para> 
        /// </summary>
        public static MEMENTO_PARENT_WATCHED_DELEGATE MEMENTO_PARENT_WATCHED;
        /// <summary> 
        /// Applies an unchain event to an item such as spawn or setsprite
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.UnchainToItemService"/> </para> 
        /// </summary>
        public static UNCHAIN_TO_ITEM_DELEGATE UNCHAIN_TO_ITEM;
        /* > ATG 3 END */
    }
}
