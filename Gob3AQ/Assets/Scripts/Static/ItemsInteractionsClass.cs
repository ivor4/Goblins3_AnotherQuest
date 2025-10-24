using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.Brain.ItemsInteraction
{ 
    public static class ItemsInteractionsClass
    {
        public static GameItem GetItemFromPickable(GamePickableItem pickable)
        {
            if((uint)pickable >= (uint)GamePickableItem.ITEM_PICK_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemFromPickable: Invalid pickable item {pickable}");
                return GameItem.ITEM_NONE;
            }
            else
            {
                return _PickableToItem[(int)pickable];
            }
        }

        public static GameSprite GetSpriteFromPickable(GamePickableItem pickable)
        {
            if ((uint)pickable >= (uint)GamePickableItem.ITEM_PICK_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetSpriteFromPickable: Invalid pickable item {pickable}");
                return GameSprite.SPRITE_NONE;
            }
            else
            {
                return _PickableSprite[(int)pickable];
            }
        }

        public static ref readonly ItemInfo GetItemInfo(GameItem item)
        {
            if((uint)item >= (uint)GameItem.ITEM_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {item}");
                return ref ItemInfo.EMPTY;
            }
            else
            {
                return ref _ItemInfo[(int)item];
            }
        }

        public static ref readonly ActionConditionsInfo GetActionConditionsInfo(ActionConditions conditions)
        {
            if ((uint)conditions >= (uint)ActionConditions.COND_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {conditions}");
                return ref ActionConditionsInfo.EMPTY;
            }
            else
            {
                return ref _ActionConditions[(int)conditions];
            }
        }


        private static readonly ActionConditionsInfo[] _ActionConditions = new ActionConditionsInfo[(int)ActionConditions.COND_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* COND_OK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_NONE,false),
            
            new( /* COND_TAKE_POTION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_NONE,false),
            
            new( /* COND_OBSERVE_POTION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_RED_POTION,
            GameEvent.EVENT_NONE,false),
            
            new( /* COND_TAKE_POTION_BLUE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_PARROT,GameItem.ITEM_POTION_BLUE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_NONE,false),
            
            new( /* COND_FOUNTAIN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_POTION,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_POUR,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_FOUNTAIN_FULL,true),
            
            new( /* COND_FOUNTAIN2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_PARROT,GameItem.ITEM_POTION_BLUE,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_STARE_SCREEN,
            DialogType.DIALOG_FOUNTAIN,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_NONE,false),
            
            new( /* COND_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            GameEvent.EVENT_NONE,false),
            
            /* > ATG 1 END < */
        };




        private static readonly ItemInfo[] _ItemInfo = new ItemInfo[(int)GameItem.ITEM_TOTAL]
        {
            /* > ATG 2 START < */
            new ( /* ITEM_PLAYER_MAIN */
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new GameSprite[1]{GameSprite.SPRITE_POTION_RED,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_PLAYER_PARROT */
            NameType.NAME_CHAR_PARROT,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new GameSprite[1]{GameSprite.SPRITE_POTION_RED,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_PLAYER_SNAKE */
            NameType.NAME_CHAR_SNAKE,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new GameSprite[1]{GameSprite.SPRITE_POTION_RED,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_POTION */
            NameType.NAME_ITEM_POTION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new GameSprite[1]{GameSprite.SPRITE_POTION_RED,},
            true,GameSprite.SPRITE_POTION_RED,GamePickableItem.ITEM_PICK_POTION,
            new ActionConditions[2]{ActionConditions.COND_OBSERVE_POTION,ActionConditions.COND_TAKE_POTION,}),
            
            new ( /* ITEM_POTION_BLUE */
            NameType.NAME_ITEM_BLUE_POTION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new GameSprite[1]{GameSprite.SPRITE_POTION_BLUE,},
            true,GameSprite.SPRITE_POTION_BLUE,GamePickableItem.ITEM_PICK_POTION_BLUE,
            new ActionConditions[1]{ActionConditions.COND_TAKE_POTION_BLUE,}),
            
            new ( /* ITEM_FOUNTAIN */
            NameType.NAME_ITEM_FOUNTAIN,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new GameSprite[2]{GameSprite.SPRITE_FOUNTAIN,GameSprite.SPRITE_FOUNTAIN_FULL,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[2]{ActionConditions.COND_FOUNTAIN,ActionConditions.COND_FOUNTAIN2,}),
            
            new ( /* ITEM_NPC_MILITO */
            NameType.NAME_NPC_MILITO,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new GameSprite[1]{GameSprite.SPRITE_NPC_MILITO,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new GameSprite[1]{GameSprite.SPRITE_LAST,},
            false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            /* > ATG 2 END < */
        };

        private static readonly GameItem[] _PickableToItem = new GameItem[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 3 START < */
            GameItem.ITEM_POTION,	/* ITEM_PICK_POTION */
            GameItem.ITEM_POTION_BLUE,	/* ITEM_PICK_POTION_BLUE */
            /* > ATG 3 END < */
        };

        private static readonly GameSprite[] _PickableSprite = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 4 START < */
            GameSprite.SPRITE_POTION_RED,	/* ITEM_PICK_POTION */
            GameSprite.SPRITE_POTION_BLUE,	/* ITEM_PICK_POTION_BLUE */
            /* > ATG 4 END < */
        };
    }
}
