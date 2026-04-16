using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.GameMenu;
using Gob3AQ.DialogMaster;

namespace Gob3AQ.VARMAP.GameEventMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for GameEvent module
    /// </summary>
    public sealed class VARMAP_GameEventMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
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
            GET_DAY_MOMENT = _GET_DAY_MOMENT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_BUSY_STATE = _GET_BUSY_STATE;
            GET_SHADOW_BUSY_STATE = _GET_SHADOW_BUSY_STATE;
            SET_BUSY_STATE = _SET_BUSY_STATE;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            COMMIT_EVENT = _COMMIT_EVENT;
            COMMIT_MEMENTO_NOTIF = _COMMIT_MEMENTO_NOTIF;
            IS_MEMENTO_UNLOCKED = _IS_MEMENTO_UNLOCKED;
            MEMENTO_PARENT_WATCHED = _MEMENTO_PARENT_WATCHED;
            ACTION_TO_ITEM = _ACTION_TO_ITEM;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            SHOW_DIALOGUE = _SHOW_DIALOGUE;
            SHOW_DECISION = _SHOW_DECISION;
            CHANGE_DAY_MOMENT = _CHANGE_DAY_MOMENT;
            PLAY_SOUND = _PLAY_SOUND;
            STOP_SOUND = _STOP_SOUND;
            START_ANIMATION = _START_ANIMATION;
            PERFORM_ACTION = _PERFORM_ACTION;
            IS_DIALOG_ACTIVE = _IS_DIALOG_ACTIVE;
            NOTIFY_ENDED_ACTION = _NOTIFY_ENDED_ACTION;
            EXECUTE_EXIT_ROOM_CONDS = _EXECUTE_EXIT_ROOM_CONDS;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
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
        public static GetVARMAPValueDelegate<MomentType> GET_DAY_MOMENT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<BusyState> GET_BUSY_STATE;
        public static GetVARMAPValueDelegate<BusyState> GET_SHADOW_BUSY_STATE;
        public static SetVARMAPValueDelegate<BusyState> SET_BUSY_STATE;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        public static LODING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
        /// Checks if a combination of events is totally complied (event absence can also be requested)
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, DialogMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.IsEventCombiOccurredService"/> </para> 
        /// </summary>
        public static IS_EVENT_COMBI_OCCURRED_DELEGATE IS_EVENT_COMBI_OCCURRED;
        /// <summary> 
        /// Activates/Deactivates an event
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMaster, GameMenu,  </para> 
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
        /// <para> Method: <see cref="ItemMasterClass.ActionToItemService"/> </para> 
        /// </summary>
        public static ACTION_TO_ITEM_DELEGATE ACTION_TO_ITEM;
        /// <summary> 
        /// Asks Game Master to set game mode
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, DialogMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeGameModeService"/> </para> 
        /// </summary>
        public static CHANGE_GAME_MODE_DELEGATE CHANGE_GAME_MODE;
        /// <summary> 
        /// Second part of start dialogue. Tells Game Menu to prepare menu elements
        /// <para> Owner: DialogMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="DialogMasterClass.ShowDialogueService"/> </para> 
        /// </summary>
        public static SHOW_DIALOGUE_DELEGATE SHOW_DIALOGUE;
        /// <summary> 
        /// Shows a set of decisions
        /// <para> Owner: GameMenu </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMenuClass.ShowDecisionService"/> </para> 
        /// </summary>
        public static SHOW_DECISION_DELEGATE SHOW_DECISION;
        /// <summary> 
        /// Requests to change moment of day to a given one
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.ChangeDayMomentService"/> </para> 
        /// </summary>
        public static CHANGE_DAY_MOMENT_DELEGATE CHANGE_DAY_MOMENT;
        /// <summary> 
        /// Plays a sound and (optionally) callback is called
        /// <para> Owner: SoundMaster </para> 
        /// <para> Accessors: DialogMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="SoundMasterClass.PlaySoundService"/> </para> 
        /// </summary>
        public static PLAY_SOUND_DELEGATE PLAY_SOUND;
        /// <summary> 
        /// Stops first match of sound with given ID which is being played
        /// <para> Owner: SoundMaster </para> 
        /// <para> Accessors: LevelMaster, DialogMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="SoundMasterClass.StopSoundService"/> </para> 
        /// </summary>
        public static STOP_SOUND_DELEGATE STOP_SOUND;
        /// <summary> 
        /// Starts an animation in background or main mode
        /// <para> Owner: DialogMaster </para> 
        /// <para> Accessors: ItemMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="DialogMasterClass.StartAnimationService"/> </para> 
        /// </summary>
        public static START_ANIMATION_DELEGATE START_ANIMATION;
        /// <summary> 
        /// Performs a named action
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: LevelMaster, GameMenu, DialogMaster, ItemMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.PerformActionService"/> </para> 
        /// </summary>
        public static PERFORM_ACTION_DELEGATE PERFORM_ACTION;
        /// <summary> 
        /// Returns if a dialog (Background or not) is active
        /// <para> Owner: DialogMaster </para> 
        /// <para> Accessors: GameEventMaster,  </para> 
        /// <para> Method: <see cref="DialogMasterClass.IsDialogActiveService"/> </para> 
        /// </summary>
        public static IS_DIALOG_ACTIVE_DELEGATE IS_DIALOG_ACTIVE;
        /// <summary> 
        /// Notifies Event manager a Dialog / Animation action has been performed
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: DialogMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.NotifyEndedActionService"/> </para> 
        /// </summary>
        public static NOTIFY_ENDED_ACTION_DELEGATE NOTIFY_ENDED_ACTION;
        /// <summary> 
        /// Executes exit room unchainers before leaving room
        /// <para> Owner: GameEventMaster </para> 
        /// <para> Accessors: GameMaster,  </para> 
        /// <para> Method: <see cref="GameEventMasterClass.ExecuteExitRoomCondsService"/> </para> 
        /// </summary>
        public static EXECUTE_EXIT_ROOM_CONDS_DELEGATE EXECUTE_EXIT_ROOM_CONDS;
        /* > ATG 3 END */
    }
}
