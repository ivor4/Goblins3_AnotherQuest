using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.GameElement.Item;
using UnityEngine;
using Gob3AQ.Waypoint;
using Gob3AQ.Libs.Arith;
using System;

namespace Gob3AQ.VARMAP.Types.Delegates
{
    public delegate void START_GAME_DELEGATE(out bool error);
    public delegate void SAVE_GAME_DELEGATE();
    public delegate void LOAD_GAME_DELEGATE();
    public delegate void LOAD_ROOM_DELEGATE(Room room, out bool error);
    public delegate void EXIT_GAME_DELEGATE(out bool error);
    public delegate void LATE_START_SUBSCRIPTION_DELEGATE(LATE_START_CALL_DELEGATE callable, bool add);
    public delegate void LODING_COMPLETED_DELEGATE(out bool error);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void NPC_REGISTER_DELEGATE(bool register, NPCMasterClass instance);
    public delegate void ITEM_REGISTER_DELEGATE(bool register, ItemClass instance);
    public delegate void ITEM_REMOVE_FROM_SCENE_DELEGATE(GameItem item);
    public delegate void MONO_REGISTER_DELEGATE(PlayableCharScript mono, bool add);
    public delegate void WP_REGISTER_DELEGATE(WaypointClass wp, bool add);
    public delegate void MOVE_PLAYER_DELEGATE(CharacterType character, WaypointClass wp);
    public delegate void SELECT_PLAYER_DELEGATE(CharacterType character);
    public delegate void GET_PLAYER_LIST_DELEGATE(out ReadOnlySpan<PlayableCharScript> list);
    public delegate void GET_NEAREST_WP_DELEGATE(Vector2 pos, float maxradius, out WaypointClass wp);
    public delegate void IS_EVENT_OCCURRED_DELEGATE(GameEvent ev, out bool occurred);
    public delegate void COMMIT_EVENT_DELEGATE(GameEvent ev, bool occurred);
    public delegate void TAKE_ITEM_FROM_SCENE_EVENT_DELEGATE(GamePickableItem item);
    public delegate void IS_ITEM_TAKEN_FROM_SCENE_DELEGATE(GamePickableItem item, out bool taken);
    public delegate void IS_ITEM_OWNED_DELEGATE(GamePickableItem item, out CharacterType character);
    public delegate void INTERACT_PLAYER_ITEM_DELEGATE(in ItemUsage usage, WaypointClass wp);
    public delegate void TAKE_ITEM_DELEGATE(CharacterType character, GameItem item, out ItemInteractionType permitted);
    public delegate void USE_ITEM_DELEGATE(in ItemUsage usage, out ItemInteractionType permitted);
    public delegate void GET_SCENARIO_ITEM_LIST_DELEGATE(out ReadOnlyList<ItemClass> list);
    public delegate void SELECT_PICKABLE_ITEM_DELEGATE(GameItem item);
    public delegate void CANCEL_PICKABLE_ITEM_DELEGATE();
    public delegate void SET_PLAYER_ANIMATION_DELEGATE(CharacterType character, CharacterAnimation animation);

}