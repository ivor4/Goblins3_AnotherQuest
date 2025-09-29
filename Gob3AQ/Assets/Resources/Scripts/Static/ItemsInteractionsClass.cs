using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.Brain.ItemsInteraction
{ 
    public static class ItemsInteractionsClass
    {
        public static ReadOnlySpan<NPCInfo> NPC_INFO => _NPCInfo;
        public static ReadOnlySpan<GamePickableItem> ITEM_TO_PICKABLE => _ItemToPickable;
        public static ReadOnlySpan<ItemConditions> ITEM_CONDITIONS => _ItemConditions;

        private static readonly ItemInteractionInfo[] _FailedInteractionInfo = new ItemInteractionInfo[0];

        private static readonly NPCInteractionInfo[] _FailedNPCInteractionInfo = new NPCInteractionInfo[0];

        public static ReadOnlySpan<NPCInteractionInfo> GetNPCInteractions(NPCType npc)
        {
            if ((uint)npc < (uint)NPCType.NPC_TOTAL)
            {
                return _NPCInteractions[(int)npc];
            }
            else
            {
                return _FailedNPCInteractionInfo;
            }
        }

        public static ReadOnlySpan<ItemInteractionInfo> GetItemInteractions(GameItem item)
        {
            if ((uint)item < (uint)GameItem.ITEM_TOTAL)
            {
                return _ItemInteractions[(int)item];
            }
            else
            {
                return _FailedInteractionInfo;
            }
        }


        private static readonly ItemConditions[] _ItemConditions = new ItemConditions[(int)ItemConditionsType.COND_TOTAL]
        {
            /* > ATG 1 START < */
            new(GameEvent.EVENT_NONE,false,CharacterAnimation.ITEM_USE_ANIMATION_TAKE,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE), /* COND_OK */
            new(GameEvent.EVENT_NONE,false,CharacterAnimation.ITEM_USE_ANIMATION_POUR,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE), /* COND_FOUNTAIN */
            new(GameEvent.EVENT_NONE,false,CharacterAnimation.ITEM_USE_ANIMATION_STARE_SCREEN,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_FOUNTAIN,DialogPhrase.PHRASE_NONE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE), /* COND_FOUNTAIN2 */
            new(GameEvent.EVENT_NONE,false,CharacterAnimation.ITEM_USE_ANIMATION_TAKE,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE), /* COND_LAST */
            /* > ATG 1 END < */
        };


        private static readonly GamePickableItem[] _ItemToPickable = new GamePickableItem[(int)GameItem.ITEM_TOTAL]
        {
            /* > ATG 2 START < */
            GamePickableItem.ITEM_PICK_POTION, /* ITEM_POTION */
            GamePickableItem.ITEM_PICK_POTION_BLUE, /* ITEM_POTION_BLUE */
            GamePickableItem.ITEM_PICK_NONE, /* ITEM_FOUNTAIN */
            GamePickableItem.ITEM_PICK_NONE, /* ITEM_LAST */
            /* > ATG 2 END < */
        };



        private static readonly ItemInteractionInfo[][] _ItemInteractions = new ItemInteractionInfo[(int)GameItem.ITEM_TOTAL][]
        {
            /* > ATG 3 START < */
            new ItemInteractionInfo[1] 
            { /* ITEM_POTION */
            new(CharacterType.CHARACTER_MAIN,ItemInteractionType.INTERACTION_TAKE,GameItem.ITEM_NONE,ItemConditionsType.COND_OK,GameEvent.EVENT_NONE,true),
            }, 
            new ItemInteractionInfo[1] 
            { /* ITEM_POTION_BLUE */
            new(CharacterType.CHARACTER_PARROT,ItemInteractionType.INTERACTION_TAKE,GameItem.ITEM_NONE,ItemConditionsType.COND_OK,GameEvent.EVENT_NONE,true),
            }, 
            new ItemInteractionInfo[2] 
            { /* ITEM_FOUNTAIN */
            new(CharacterType.CHARACTER_MAIN,ItemInteractionType.INTERACTION_USE,GameItem.ITEM_POTION,ItemConditionsType.COND_FOUNTAIN,GameEvent.EVENT_FOUNTAIN_FULL,true),
            new(CharacterType.CHARACTER_PARROT,ItemInteractionType.INTERACTION_USE,GameItem.ITEM_POTION_BLUE,ItemConditionsType.COND_FOUNTAIN2,GameEvent.EVENT_NONE,false),
            }, 
            new ItemInteractionInfo[0] 
            { /* ITEM_LAST */
            }, 
            /* > ATG 3 END < */
        };

        private static readonly NPCInfo[] _NPCInfo = new NPCInfo[(int)NPCType.NPC_TOTAL]
        {
            /* > ATG 4 START < */
            new(NameType.NAME_NPC_MILITO,Room.ROOM_FIRST), 	/* NPC_FIRST */
            new(NameType.NAME_NONE,Room.ROOM_NONE), 	/* NPC_LAST */
            /* > ATG 4 END < */
        };

        private static readonly NPCInteractionInfo[][] _NPCInteractions = new NPCInteractionInfo[(int)NPCType.NPC_TOTAL][]
        {
            /* > ATG 5 START < */
            new NPCInteractionInfo[1] 
            { /* NPC_FIRST */
            new(CharacterType.CHARACTER_MAIN,ItemInteractionType.INTERACTION_TALK,GameItem.ITEM_NONE,
            ItemConditionsType.COND_OK,DialogType.DIALOG_FOUNTAIN,GameEvent.EVENT_NONE,false),
            }, 
            new NPCInteractionInfo[0] 
            { /* NPC_LAST */
            }, 
            /* > ATG 5 END < */
        };

    }
}
