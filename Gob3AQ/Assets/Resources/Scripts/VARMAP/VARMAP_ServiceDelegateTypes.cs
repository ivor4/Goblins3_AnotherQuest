using Gob3AQ.NPCMaster;
using UnityEngine;

namespace Gob3AQ.VARMAP.Types.Delegates
{
    public delegate void START_GAME_DELEGATE(out bool error);
    public delegate void LOAD_ROOM_DELEGATE(Room room, out bool error);
    public delegate void EXIT_GAME_DELEGATE(out bool error);
    public delegate void LODING_COMPLETED_DELEGATE(out bool error);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void NPC_REGISTER_SERVICE(bool register, NPCMasterClass instance);
    public delegate void MONO_REGISTER_SERVICE(MonoBehaviour mono, bool add);
    public delegate void MOVE_PLAYER_SERVICE(Vector2 position);
}