using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameElement.NPC;
using UnityEngine;
using Gob3AQ.Waypoint;
using Gob3AQ.Libs.Arith;
using System;

namespace Gob3AQ.VARMAP.Types.Delegates
{
    public delegate void START_GAME_DELEGATE(out bool error);
    public delegate void LOAD_ROOM_DELEGATE(Room room, out bool error);
    public delegate void EXIT_GAME_DELEGATE(out bool error);
    public delegate void LODING_COMPLETED_DELEGATE(out bool error);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void NPC_REGISTER_DELEGATE(bool register, NPCMasterClass instance);
    public delegate void MONO_REGISTER_DELEGATE(PlayableCharScript mono, bool add, out byte id);
    public delegate void WP_REGISTER_DELEGATE(WaypointClass wp, bool add);
    public delegate void MOVE_PLAYER_DELEGATE(WaypointClass wp);
    public delegate void GET_PLAYER_LIST_DELEGATE(ref ReadOnlyList<PlayableCharScript> list);
    public delegate void GET_NEAREST_WP_DELEGATE(Vector2 pos, float maxradius, out WaypointClass wp);
}