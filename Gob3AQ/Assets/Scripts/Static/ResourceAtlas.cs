using UnityEngine;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Brain.ItemsInteraction;
using System;

namespace Gob3AQ.ResourceAtlas
{
#if UNITY_EDITOR
    public enum PrefabEnum
    {
        PREFAB_WAYPOINT,
        PREFAB_MENU_PICKABLE_ITEM,

        PREFAB_TOTAL
    }
#endif

    


    public static class ResourceAtlasClass
    {
        public static ref readonly RoomInfo GetRoomInfo(Room room)
        {
            if ((uint)room < (uint)Room.ROOMS_TOTAL)
            {
                return ref _RoomInfo[(int)room];
            }
            else
            {
                Debug.LogError($"Trying to get RoomInfo for invalid room {room}");
                return ref RoomInfo.EMPTY;
            }
        }

        private static readonly RoomInfo[] _RoomInfo = new RoomInfo[(int)Room.ROOMS_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* ROOM_FIRST */
            new GameSprite[4]{GameSprite.SPRITE_POTION_RED, GameSprite.SPRITE_POTION_BLUE, GameSprite.SPRITE_FOUNTAIN, GameSprite.SPRITE_FOUNTAIN_FULL, }, 
            new DialogPhrase[6]{DialogPhrase.PHRASE_NONSENSE, DialogPhrase.PHRASE_ASK_FOUNTAIN1_1, DialogPhrase.PHRASE_ASK_FOUNTAIN1_2, DialogPhrase.PHRASE_ASK_FOUNTAIN2_1, DialogPhrase.PHRASE_ASK_FOUNTAIN3_1, DialogPhrase.PHRASE_ASK_FOUNTAIN4_1, }, 
            new NameType[5]{NameType.NAME_CHAR_MAIN, NameType.NAME_CHAR_PARROT, NameType.NAME_ITEM_POTION, NameType.NAME_ITEM_BLUE_POTION, NameType.NAME_ITEM_FOUNTAIN, } 
            ),
            
            new( /* ROOM_LAST */
            new GameSprite[3]{GameSprite.SPRITE_LAST, GameSprite.SPRITE_POTION_RED, GameSprite.SPRITE_POTION_BLUE, }, 
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONSENSE, }, 
            new NameType[3]{NameType.NAME_NPC_LAST, NameType.NAME_ITEM_POTION, NameType.NAME_ITEM_BLUE_POTION, } 
            ),
            
            /* > ATG 1 END < */
        };



#if UNITY_EDITOR
        public static GameObject GetPrefab(PrefabEnum prefabId)
        {
            return Resources.Load<GameObject>(_PrefabList[(int)prefabId]);
        }


        private static readonly string[] _PrefabList = new string[(int)PrefabEnum.PREFAB_TOTAL]
        {
            "Prefabs/Waypoint",
            "Prefabs/PickableItemDisplay"
        };
#endif
    }
}
