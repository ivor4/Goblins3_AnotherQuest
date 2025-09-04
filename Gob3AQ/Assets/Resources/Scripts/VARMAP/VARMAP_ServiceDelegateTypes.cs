using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.Door;
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
    public delegate void LODING_COMPLETED_DELEGATE(GameModules module);
    public delegate void IS_MODULE_LOADED_DELEGATE(GameModules module, out bool loaded);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void START_DIALOGUE_DELEGATE(CharacterType charType, DialogType dialog, DialogPhrase phrase);
    public delegate void END_DIALOGUE_DELEGATE();
    public delegate void SHOW_DIALOGUE_DELEGATE(CharacterType charType, DialogType dialog, DialogPhrase phrase);
    public delegate void NPC_REGISTER_DELEGATE(NPCClass instance, bool register);
    public delegate void ITEM_REGISTER_DELEGATE(bool register, ItemClass instance);
    public delegate void ITEM_OBTAIN_PICKABLE_DELEGATE(GameItem item);
    public delegate void MONO_REGISTER_DELEGATE(PlayableCharScript mono, bool add);
    public delegate void WP_REGISTER_DELEGATE(WaypointClass wp, bool add);
    public delegate void DOOR_REGISTER_DELEGATE(DoorClass door, bool add);
    public delegate void MOVE_PLAYER_DELEGATE(CharacterType character, WaypointClass wp);
    public delegate void PLAYER_WAYPOINT_UPDATE_DELEGATE(CharacterType character, int wpIndex);
    public delegate void SELECT_PLAYER_DELEGATE(CharacterType character);
    public delegate void GET_PLAYER_LIST_DELEGATE(out ReadOnlySpan<PlayableCharScript> list);
    public delegate void GET_NPC_LIST_DELEGATE(out ReadOnlyList<NPCClass> list);
    public delegate void GET_NEAREST_WP_DELEGATE(Vector2 pos, float maxradius, out WaypointClass wp);
    public delegate void IS_EVENT_OCCURRED_DELEGATE(GameEvent ev, out bool occurred);
    public delegate void COMMIT_EVENT_DELEGATE(GameEvent ev, bool occurred);
    public delegate void ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE(GamePickableItem item);
    public delegate void IS_ITEM_TAKEN_FROM_SCENE_DELEGATE(GamePickableItem item, out bool taken);
    public delegate void IS_ITEM_OWNED_DELEGATE(GamePickableItem item, out CharacterType character);
    public delegate void INTERACT_PLAYER_DELEGATE(in InteractionUsage usage);
    public delegate void INTERACT_PLAYER_DOOR_DELEGATE(CharacterType character, WaypointClass doorWaypoint, int doorIndex);
    public delegate void TAKE_ITEM_DELEGATE(CharacterType character, GameItem item, out ItemInteractionType permitted);
    public delegate void USE_ITEM_DELEGATE(in InteractionUsage usage, out InteractionUsageOutcome outcome);
    public delegate void GET_SCENARIO_ITEM_LIST_DELEGATE(out ReadOnlyList<ItemClass> list);
    public delegate void SELECT_PICKABLE_ITEM_DELEGATE(GameItem item);
    public delegate void CANCEL_PICKABLE_ITEM_DELEGATE();
    public delegate void SET_PLAYER_ANIMATION_DELEGATE(CharacterType character, CharacterAnimation animation);
    public delegate void EVENT_SUBSCRIPTION_DELEGATE(GameEvent gevent, EVENT_SUBSCRIPTION_CALL_DELEGATE callable, bool add);
    public delegate void CROSS_DOOR_DELEGATE(CharacterType character, int doorIndex);
    public delegate void INTERACT_PLAYER_NPC_DELEGATE(CharacterType character, int npcindex);
    public delegate void LOCK_PLAYER_DELEGATE(CharacterType character, bool lockPlayer);

}