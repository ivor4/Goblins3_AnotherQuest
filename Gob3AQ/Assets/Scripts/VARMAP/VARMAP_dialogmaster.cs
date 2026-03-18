using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.InputMaster;
using Gob3AQ.GameMenu;
using Gob3AQ.GraphicsMaster;
using Gob3AQ.SoundMaster;
using Gob3AQ.DialogMaster;

namespace Gob3AQ.VARMAP.DialogMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_DialogMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_DAY_MOMENT = _GET_DAY_MOMENT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PLAYER_SELECTED = _GET_PLAYER_SELECTED;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            IS_EVENT_COMBI_OCCURRED = _IS_EVENT_COMBI_OCCURRED;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            SHOW_DIALOGUE = _SHOW_DIALOGUE;
            DIALOGUE_SELECT_OPTION = _DIALOGUE_SELECT_OPTION;
            PLAY_SOUND = _PLAY_SOUND;
            STOP_SOUND = _STOP_SOUND;
            START_ANIMATION = _START_ANIMATION;
            ITEM_PERFORM_ANIMATION = _ITEM_PERFORM_ANIMATION;
            PERFORM_ACTION = _PERFORM_ACTION;
            IS_DIALOG_ACTIVE = _IS_DIALOG_ACTIVE;
            NOTIFY_ENDED_ACTION = _NOTIFY_ENDED_ACTION;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<MomentType> GET_DAY_MOMENT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<CharacterType> GET_PLAYER_SELECTED;
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
        /// Option selection in multi option dialogue
        /// <para> Owner: DialogMaster </para> 
        /// <para> Accessors: GameMenu,  </para> 
        /// <para> Method: <see cref="DialogMasterClass.DialogueSelectOptionService"/> </para> 
        /// </summary>
        public static DIALOGUE_SELECT_OPTION_DELEGATE DIALOGUE_SELECT_OPTION;
        /// <summary> 
        /// Plays a sound and (optionally) callback is called
        /// <para> Owner: SoundMaster </para> 
        /// <para> Accessors: LevelMaster, DialogMaster,  </para> 
        /// <para> Method: <see cref="SoundMasterClass.PlaySoundService"/> </para> 
        /// </summary>
        public static PLAY_SOUND_DELEGATE PLAY_SOUND;
        /// <summary> 
        /// Stops first match of sound with given ID which is being played
        /// <para> Owner: SoundMaster </para> 
        /// <para> Accessors: LevelMaster, DialogMaster,  </para> 
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
        /// Makes an item start an animation
        /// <para> Owner: ItemMaster </para> 
        /// <para> Accessors: DialogMaster,  </para> 
        /// <para> Method: <see cref="ItemMasterClass.PerformAnimationService"/> </para> 
        /// </summary>
        public static ITEM_PERFORM_ANIMATION_DELEGATE ITEM_PERFORM_ANIMATION;
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
        public static NOTIFY_ENDED_ACTION NOTIFY_ENDED_ACTION;
        /* > ATG 3 END */
    }
}
