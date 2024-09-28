using MVerse.EnemyMaster;
using UnityEngine;

namespace MVerse.VARMAP.Types.Delegates
{
    public delegate void START_GAME_DELEGATE(out bool error);
    public delegate void LOAD_OBF_DELEGATE(OBFight fight, out bool error);
    public delegate void LOAD_ROOM_DELEGATE(Room room, out bool error);
    public delegate void EXIT_GAME_DELEGATE(out bool error);
    public delegate void LODING_COMPLETED_DELEGATE(out bool error);
    public delegate void CHANGE_OTHER_WORLD_DELEGATE(bool toOtherWorld, OtherWorldMode otherWorldMode);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void ENEMY_REGISTER_SERVICE(bool register, EnemyMasterClass instance);
    public delegate void MONO_REGISTER_SERVICE(MonoBehaviour mono, bool add);
}