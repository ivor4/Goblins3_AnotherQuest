using Gob3AQ.CardMaster;
using Gob3AQ.DialogMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;
using Gob3AQ.SoundMaster;

namespace Gob3AQ.VARMAP.CardMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_CardMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ELAPSED_TIME_MS = _GET_ELAPSED_TIME_MS;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            GET_PRESSED_KEYS = _GET_PRESSED_KEYS;
            GET_MOUSE_PROPERTIES = _GET_MOUSE_PROPERTIES;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            CHANGE_GAME_MODE = _CHANGE_GAME_MODE;
            SHOW_DIALOGUE = _SHOW_DIALOGUE;
            PLAY_SOUND = _PLAY_SOUND;
            PERFORM_ACTION = _PERFORM_ACTION;
            IS_DIALOG_ACTIVE = _IS_DIALOG_ACTIVE;
            START_CARD_GAME = _START_CARD_GAME;
            GIVE_UP_CARD_GAME = _GIVE_UP_CARD_GAME;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<ulong> GET_ELAPSED_TIME_MS;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        public static GetVARMAPValueDelegate<KeyStruct> GET_PRESSED_KEYS;
        public static GetVARMAPValueDelegate<MousePropertiesStruct> GET_MOUSE_PROPERTIES;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
/// This service is called when whole room has been loaded 
/// <para> Owner: GameMaster </para> 
/// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
/// </summary>
public static LOADING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
/// This service returns a bool which tells if given module has been loaded in Room Loading Process 
/// <para> Owner: GameMaster </para> 
/// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
/// </summary>
public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
/// Asks Game Master to set game mode 
/// <para> Owner: GameMaster </para> 
/// <para> Accessors: LevelMaster, GameMenu, DialogMaster, CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="GameMasterClass.ChangeGameModeService"/> </para> 
/// </summary>
public static CHANGE_GAME_MODE_DELEGATE CHANGE_GAME_MODE;
        /// <summary> 
/// Second part of start dialogue. Tells Game Menu to prepare menu elements 
/// <para> Owner: DialogMaster </para> 
/// <para> Accessors: CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="DialogMasterClass.ShowDialogueService"/> </para> 
/// </summary>
public static SHOW_DIALOGUE_DELEGATE SHOW_DIALOGUE;
        /// <summary> 
/// Plays a sound and (optionally) callback is called 
/// <para> Owner: SoundMaster </para> 
/// <para> Accessors: DialogMaster, CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="SoundMasterClass.PlaySoundService"/> </para> 
/// </summary>
public static PLAY_SOUND_DELEGATE PLAY_SOUND;
        /// <summary> 
/// Performs a named action 
/// <para> Owner: GameEventMaster </para> 
/// <para> Accessors: LevelMaster, GameMenu, DialogMaster, ItemMaster, CardMaster </para> 
/// <para> Method: <see cref="GameEventMasterClass.PerformActionService"/> </para> 
/// </summary>
public static PERFORM_ACTION_DELEGATE PERFORM_ACTION;
        /// <summary> 
/// Returns if a dialog (Background or not) is active 
/// <para> Owner: DialogMaster </para> 
/// <para> Accessors: CardMaster, GameEventMaster </para> 
/// <para> Method: <see cref="DialogMasterClass.IsDialogActiveService"/> </para> 
/// </summary>
public static IS_DIALOG_ACTIVE_DELEGATE IS_DIALOG_ACTIVE;
        /// <summary> 
/// Tells Card Master to prepare a game with given parameters 
/// <para> Owner: CardMaster </para> 
/// <para> Accessors: GameEventMaster </para> 
/// <para> Method: <see cref="CardMasterClass.StartCardGameService"/> </para> 
/// </summary>
public static START_CARD_GAME_DELEGATE START_CARD_GAME;
        /// <summary> 
/// Tells Card Master to give up match 
/// <para> Owner: CardMaster </para> 
/// <para> Accessors: GameMenu </para> 
/// <para> Method: <see cref="CardMasterClass.GiveUpCardGameService"/> </para> 
/// </summary>
public static GIVE_UP_CARD_GAME_DELEGATE GIVE_UP_CARD_GAME;
        /* > ATG 3 END */
    }
}
