using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Clickable;
using Gob3AQ.GameElement.Item.Door;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.VARMAP.Types.Delegates
{
    public delegate void VARMAPValueChangedEvent<T>(ChangedEventType eventType, in T oldval, in T newval);
    public delegate ref readonly T GetVARMAPValueDelegate<T>();
    public delegate void SetVARMAPValueDelegate<T>(in T newValue);
    public delegate void ReUnRegisterVARMAPValueChangeEventDelegate<T>(VARMAPValueChangedEvent<T> callback);
    public delegate ref readonly T GetVARMAPArrayElemValueDelegate<T>(int pos);
    public delegate void SetVARMAPArrayElemValueDelegate<T>(int pos, in T newval);
    public delegate int GetVARMAPArraySizeDelegate();
    public delegate ReadOnlySpan<T> GetVARMAPArrayDelegate<T>();
    public delegate void SetVARMAPArrayDelegate<T>(ReadOnlySpan<T> newvals);
    public delegate void EVENT_SUBSCRIPTION_CALL_DELEGATE(bool newStatus);
    public delegate void KEY_SUBSCRIPTION_CALL_DELEGATE(KeyFunctionsIndex key, bool pressed);
    public delegate void GAME_ELEMENT_HOVER_DELEGATE(in LevelElemInfo info);


    public delegate void START_GAME_DELEGATE(out bool error);
    public delegate void SAVE_GAME_DELEGATE();
    public delegate void LOAD_GAME_DELEGATE();
    public delegate void LOAD_ROOM_DELEGATE(Room room, out bool error);
    public delegate void EXIT_GAME_DELEGATE(out bool error);
    public delegate void LODING_COMPLETED_DELEGATE(GameModules module);
    public delegate void IS_MODULE_LOADED_DELEGATE(GameModules module, out bool loaded);
    public delegate void FREEZE_PLAY_DELEGATE(bool freeze);
    public delegate void CHANGE_GAME_MODE_DELEGATE(Game_Status mode, out bool error);
    public delegate void SHOW_DIALOGUE_DELEGATE(ReadOnlySpan<GameItem> talkers, DialogType dialog, DialogPhrase phrase);
    public delegate void ITEM_REGISTER_DELEGATE(bool register, GameElementClass instance, GameElementClickable clickable);
    public delegate void ITEM_OBTAIN_PICKABLE_DELEGATE(GameItem item);
    public delegate void MONO_REGISTER_DELEGATE(PlayableCharScript mono, bool add);
    public delegate void DOOR_REGISTER_DELEGATE(DoorClass door, bool add);
    public delegate void OBTAIN_SCENARIO_ITEMS_DELEGATE(out IReadOnlyDictionary<GameItem, GameElementClass> dict);
    public delegate void MOVE_PLAYER_DELEGATE(CharacterType character, WaypointClass wp);
    public delegate void PLAYER_WAYPOINT_UPDATE_DELEGATE(CharacterType character, int wpIndex);
    public delegate void GET_WP_LIST_DELEGATE(out IReadOnlyList<Vector2> waypoints, out IReadOnlyList<WaypointSolution> solutions);
    public delegate void SELECT_PLAYER_DELEGATE(CharacterType character);
    public delegate void GET_PLAYER_LIST_DELEGATE(out ReadOnlySpan<PlayableCharScript> list);
    public delegate void GET_NEAREST_WP_DELEGATE(Vector2 position, float maxRadius, out int candidate_index, out Vector2 candidate_pos);
    public delegate void IS_EVENT_OCCURRED_DELEGATE(GameEvent ev, out bool occurred);
    public delegate void COMMIT_EVENT_DELEGATE(ReadOnlySpan<GameEventCombi> combi);
    public delegate void COMMIT_MEMENTO_NOTIF_DELEGATE(Memento memento);
    public delegate void IS_ITEM_AVAILABLE_DELEGATE(GameItem item, out bool available);
    public delegate void INTERACT_PLAYER_DELEGATE(CharacterType character,int destWp_index, out bool accepted);
    public delegate void UNCHAIN_TO_ITEM_DELEGATE(in UnchainInfo unchainInfo, bool spawnPreDisappear);
    public delegate void PLAYER_REACHED_WAYPOINT_DELEGATE(CharacterType character);
    public delegate void IS_EVENT_COMBI_OCCURRED_DELEGATE(ReadOnlySpan<GameEventCombi> combi, out bool occurred);
    public delegate void IS_MEMENTO_UNLOCKED_DELEGATE(Memento memento, out bool occurred, out bool unwatched);
    public delegate void MEMENTO_PARENT_WATCHED_DELEGATE(MementoParent mementoParent);
    public delegate void ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE(GamePickableItem item);
    public delegate void IS_ITEM_OWNED_DELEGATE(GamePickableItem item, out CharacterType character);
    public delegate void INTERACT_PLAYER_DOOR_DELEGATE(CharacterType character, WaypointClass doorWaypoint, int doorIndex);
    public delegate void TAKE_ITEM_DELEGATE(CharacterType character, GameItem item, out ItemInteractionType permitted);
    public delegate void USE_ITEM_DELEGATE(in InteractionUsage usage, out InteractionUsageOutcome outcome);
    public delegate void SELECT_PICKABLE_ITEM_DELEGATE(GameItem item);
    public delegate void CANCEL_PICKABLE_ITEM_DELEGATE();
    public delegate void KEY_SUBSCRIPTION_DELEGATE(KeyFunctionsIndex key, KEY_SUBSCRIPTION_CALL_DELEGATE callable, bool add);
    public delegate void SET_PLAYER_ANIMATION_DELEGATE(CharacterType character, CharacterAnimation animation);
    public delegate void EVENT_SUBSCRIPTION_DELEGATE(GameEvent gevent, EVENT_SUBSCRIPTION_CALL_DELEGATE callable, bool add);
    public delegate void CROSS_DOOR_DELEGATE(CharacterType character, int doorIndex);
    public delegate void INTERACT_PLAYER_NPC_DELEGATE(CharacterType character, int npcindex);
    public delegate void LOCK_PLAYER_DELEGATE(CharacterType character, bool lockPlayer);

}