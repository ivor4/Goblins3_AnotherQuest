﻿using Gob3AQ.GameElement;
using Gob3AQ.GameElement.Door;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;
using Gob3AQ.Waypoint;
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
    public delegate void SetVARMAPArrayDelegate<T>(List<T> newvals);
    public delegate void EVENT_SUBSCRIPTION_CALL_DELEGATE(bool newStatus);
    public delegate void GAME_ELEMENT_OVER_DELEGATE(in LevelElemInfo info);


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
    public delegate void ITEM_REGISTER_DELEGATE(bool register, GameElementClass instance);
    public delegate void ITEM_OBTAIN_PICKABLE_DELEGATE(GameItem item);
    public delegate void MONO_REGISTER_DELEGATE(PlayableCharScript mono, bool add);
    public delegate void WP_REGISTER_DELEGATE(WaypointClass wp, bool add);
    public delegate void DOOR_REGISTER_DELEGATE(DoorClass door, bool add);
    public delegate void MOVE_PLAYER_DELEGATE(CharacterType character, WaypointClass wp);
    public delegate void PLAYER_WAYPOINT_UPDATE_DELEGATE(CharacterType character, int wpIndex);
    public delegate void SELECT_PLAYER_DELEGATE(CharacterType character);
    public delegate void GET_PLAYER_LIST_DELEGATE(out ReadOnlySpan<PlayableCharScript> list);
    public delegate void GET_NEAREST_WP_DELEGATE(Vector2 pos, float maxradius, out WaypointClass wp);
    public delegate void IS_EVENT_OCCURRED_DELEGATE(GameEvent ev, out bool occurred);
    public delegate void COMMIT_EVENT_DELEGATE(GameEvent ev, bool occurred);
    public delegate void IS_ITEM_AVAILABLE_DELEGATE(GameItem item, out bool available);
    public delegate void INTERACT_PLAYER_DELEGATE(CharacterType character, WaypointClass dest, out bool accepted);
    public delegate void PLAYER_REACHED_WAYPOINT(CharacterType character);
    public delegate void IS_EVENT_COMBI_OCCURRED_DELEGATE(ReadOnlySpan<GameEventCombi> combi, out bool occurred);
    public delegate void ITEM_OBTAIN_PICKABLE_EVENT_DELEGATE(GamePickableItem item);
    public delegate void IS_ITEM_TAKEN_FROM_SCENE_DELEGATE(GamePickableItem item, out bool taken);
    public delegate void IS_ITEM_OWNED_DELEGATE(GamePickableItem item, out CharacterType character);
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