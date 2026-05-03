using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using UnityEngine;
using Gob3AQ.GameMaster;
using Gob3AQ.LevelMaster;
using Gob3AQ.PlayerMaster;
using Gob3AQ.ItemMaster;
using Gob3AQ.GameEventMaster;

namespace Gob3AQ.VARMAP.SoundMaster
{
    /// <summary>
    /// VARMAP inheritance with permissions for MainMenu module
    /// </summary>
    public sealed class VARMAP_SoundMaster : VARMAP
    {
        /* All delegate update */
        public static void UpdateDelegates()
        {
            /* > ATG 1 START */
            GET_ACTUAL_ROOM = _GET_ACTUAL_ROOM;
            GET_DAY_MOMENT = _GET_DAY_MOMENT;
            GET_GAMESTATUS = _GET_GAMESTATUS;
            REG_GAMESTATUS = _REG_GAMESTATUS;
            UNREG_GAMESTATUS = _UNREG_GAMESTATUS;
            MODULE_LOADING_COMPLETED = _MODULE_LOADING_COMPLETED;
            IS_MODULE_LOADED = _IS_MODULE_LOADED;
            LOAD_ADDITIONAL_SOUND = _LOAD_ADDITIONAL_SOUND;
            PLAY_SOUND = _PLAY_SOUND;
            STOP_SOUND = _STOP_SOUND;
            /* > ATG 1 END */
        }



        /* GET/SET */
        /* > ATG 2 START */
        public static GetVARMAPValueDelegate<Room> GET_ACTUAL_ROOM;
        public static GetVARMAPValueDelegate<MomentType> GET_DAY_MOMENT;
        public static GetVARMAPValueDelegate<Game_Status> GET_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> REG_GAMESTATUS;
        public static ReUnRegisterVARMAPValueChangeEventDelegate<Game_Status> UNREG_GAMESTATUS;
        /* > ATG 2 END */

        /* SERVICES */
        /* > ATG 3 START */
        /// <summary> 
        /// This service is called when whole room has been loaded
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, CardMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadingCompletedService"/> </para> 
        /// </summary>
        public static LODING_COMPLETED_DELEGATE MODULE_LOADING_COMPLETED;
        /// <summary> 
        /// This service returns a bool which tells if given module has been loaded in Room Loading Process
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: InputMaster, LevelMaster, GraphicsMaster, SoundMaster, GameMenu, DialogMaster, PlayerMaster, ItemMaster, CardMaster, GameEventMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.IsModuleLoadedService"/> </para> 
        /// </summary>
        public static IS_MODULE_LOADED_DELEGATE IS_MODULE_LOADED;
        /// <summary> 
        /// Loads/Unloads a set of names and Phrases
        /// <para> Owner: GameMaster </para> 
        /// <para> Accessors: SoundMaster,  </para> 
        /// <para> Method: <see cref="GameMasterClass.LoadAdditionalSoundService"/> </para> 
        /// </summary>
        public static LOAD_ADDITIONAL_SOUND_DELEGATE LOAD_ADDITIONAL_SOUND;
        /// <summary> 
        /// Plays a sound and (optionally) callback is called
        /// <para> Owner: SoundMaster </para> 
        /// <para> Accessors: DialogMaster, CardMaster, GameEventMaster,  </para> 
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
        /* > ATG 3 END */
    }
}
