using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;

namespace Gob3AQ.Brain.ItemsInteraction
{ 
    public static class ItemsInteractionsClass
    {
        public static ref readonly MementoCombiInfo GetMementoCombiInfo(MementoCombi mementoCombi)
        {
            if ((uint)mementoCombi >= (uint)MementoCombi.MEMENTO_COMBI_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoCombiInfo: Invalid memento combi item {mementoCombi}");
                return ref MementoCombiInfo.EMPTY;
            }
            else
            {
                return ref _MementoCombiInfo[(int)mementoCombi];
            }
        }

        public static ref readonly MementoParentInfo GetMementoParentInfo(MementoParent mementoParent)
        {
            if ((uint)mementoParent >= (uint)MementoParent.MEMENTO_PARENT_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoParentInfo: Invalid memento parent item {mementoParent}");
                return ref MementoParentInfo.EMPTY;
            }
            else
            {
                return ref _MementoParentInfo[(int)mementoParent];
            }
        }

        public static ref readonly MementoInfo GetMementoInfo(Memento memento)
        {
            if ((uint)memento >= (uint)Memento.MEMENTO_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoInfo: Invalid memento item {memento}");
                return ref MementoInfo.EMPTY;
            }
            else
            {
                return ref _MementoInfo[(int)memento];
            }
        }

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

        public static ref readonly UnchainInfo GetUnchainInfo(UnchainConditions condition)
        {
            if ((uint)condition >= (uint)UnchainConditions.UNCHAIN_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {condition}");
                return ref UnchainInfo.EMPTY;
            }
            else
            {
                return ref _UnchainConditions[(int)condition];
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


        private static readonly UnchainInfo[] _UnchainConditions = new UnchainInfo[(int)UnchainConditions.UNCHAIN_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* UNCHAIN_ROOM1_INITIAL_MEMENTO_1 */
            false,UnchainType.UNCHAIN_TYPE_EVENT,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_FIRST, false),}), 
            
            new( /* UNCHAIN_ROOM1_INITIAL_MEMENTO_2 */
            false,UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FIRST, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_1, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_CHEST_1 */
            true,UnchainType.UNCHAIN_TYPE_SET_SPRITE,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            GameItem.ITEM_HIVE1_CHEST, GameSprite.SPRITE_ITEM_CHEST_OPENED,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_CHEST_2 */
            true,UnchainType.UNCHAIN_TYPE_SPAWN,new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            GameItem.ITEM_CARDS_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_CHEST_1 */
            true,UnchainType.UNCHAIN_TYPE_SET_SPRITE,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            GameItem.ITEM_HIVE1_CHEST, GameSprite.SPRITE_ITEM_CHEST_CLOSED,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_CHEST_2 */
            true,UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            GameItem.ITEM_CARDS_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_CARDS_PICKABLE_TAKE_1 */
            false,UnchainType.UNCHAIN_TYPE_DESTROY,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false),}, 
            GameItem.ITEM_CARDS_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_CARDS_PICKABLE_TAKE_2 */
            false,UnchainType.UNCHAIN_TYPE_EARN_ITEM,new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false),}, 
            GameItem.ITEM_CARDS_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_1 */
            true,UnchainType.UNCHAIN_TYPE_SPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            GameItem.ITEM_HIVE1_WARDROBE_OPENED, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_2 */
            true,UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            GameItem.ITEM_HIVE1_WARDROBE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_3 */
            true,UnchainType.UNCHAIN_TYPE_SPAWN,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            GameItem.ITEM_SOAP_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_1 */
            true,UnchainType.UNCHAIN_TYPE_SPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            GameItem.ITEM_HIVE1_WARDROBE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_2 */
            true,UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            GameItem.ITEM_HIVE1_WARDROBE_OPENED, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_3 */
            true,UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            GameItem.ITEM_SOAP_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_OBSERVE_HIVE1_AD_BOARD_1 */
            false,UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_2, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_SOAP_PICKABLE_TAKE_1 */
            false,UnchainType.UNCHAIN_TYPE_DESTROY,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false),}, 
            GameItem.ITEM_SOAP_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_SOAP_PICKABLE_TAKE_2 */
            false,UnchainType.UNCHAIN_TYPE_EARN_ITEM,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false),}, 
            GameItem.ITEM_SOAP_PICKABLE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            /* > ATG 1 END < */
        };



        private static readonly ActionConditionsInfo[] _ActionConditions = new ActionConditionsInfo[(int)ActionConditions.COND_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* COND_OK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_OPEN_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}), 
            
            new( /* COND_CLOSE_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}), 
            
            new( /* COND_TAKE_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false),}), 
            
            new( /* COND_OPEN_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}), 
            
            new( /* COND_CLOSE_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}), 
            
            new( /* COND_TALK_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_REME_INTRO,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_OBSERVE_HIVE1_AD_BOARD_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            CharacterAnimation.ITEM_USE_ANIMATION_STARE_SCREEN,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, false),}), 
            
            new( /* COND_EXIT_HIVE1_HALL_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_EXIT_HIVE1_HALL_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_EXIT_HIVE1_HALL_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_USE_HIVE1_BASIN_NO_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_NO_SOAP,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_USE_HIVE1_BASIN_W_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_NORMAL,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_SOAP,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false),}), 
            
            new( /* COND_USE_HIVE1_BASIN_W_SOAP_REPEAT */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_ALREADY_COMBI,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_TAKE_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false),}), 
            
            new( /* COND_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            /* > ATG 2 END < */
        };




        private static readonly ItemInfo[] _ItemInfo = new ItemInfo[(int)GameItem.ITEM_TOTAL]
        {
            /* > ATG 3 START < */
            new ( /* ITEM_PLAYER_MAIN */
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE,}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_HIVE1_CHEST */
            NameType.NAME_ITEM_SECR_DESK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_ITEM_CHEST_OPENED,GameSprite.SPRITE_ITEM_CHEST_CLOSED,}),
            GameSprite.SPRITE_ITEM_CHEST_CLOSED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[2]{ActionConditions.COND_OPEN_CHEST,ActionConditions.COND_CLOSE_CHEST,}),
            
            new ( /* ITEM_CARDS_PICKABLE */
            NameType.NAME_ITEM_CARDS,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_CARDS,}),
            GameSprite.SPRITE_ITEM_CARDS,true,GameSprite.SPRITE_ITEM_CARDS_PICKABLE,GamePickableItem.ITEM_PICK_CARDS_PICKABLE,
            new ActionConditions[1]{ActionConditions.COND_TAKE_CARDS,}),
            
            new ( /* ITEM_HIVE1_WARDROBE */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OPEN_HIVE1_WARDROBE,}),
            
            new ( /* ITEM_HIVE1_WARDROBE_OPENED */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_WARDROBE_OPENED,}),
            GameSprite.SPRITE_ITEM_WARDROBE_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_CLOSE_HIVE1_WARDROBE,}),
            
            new ( /* ITEM_GENERIC_DOOR1 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_HIVE1_NPC_REME */
            NameType.NAME_NPC_REME,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_REME_IDLE,}),
            GameSprite.SPRITE_NPC_REME_IDLE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_TALK_REME_1,}),
            
            new ( /* ITEM_GENERIC_DOOR2 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_GENERIC_DOOR3 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_HIVE1_PERFUME */
            NameType.NAME_ITEM_PERFUME,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_HIVE1_AD_BOARD */
            NameType.NAME_ITEM_AD_BOARD,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_BLANK,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OBSERVE_HIVE1_AD_BOARD_1,}),
            
            new ( /* ITEM_HIVE1_EXIT_DOOR */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_BLANK,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[3]{ActionConditions.COND_EXIT_HIVE1_HALL_1,ActionConditions.COND_EXIT_HIVE1_HALL_2,ActionConditions.COND_EXIT_HIVE1_HALL_3,}),
            
            new ( /* ITEM_HIVE1_BASIN */
            NameType.NAME_ITEM_BASIN,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_BLANK,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[3]{ActionConditions.COND_USE_HIVE1_BASIN_NO_SOAP,ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP,ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP_REPEAT,}),
            
            new ( /* ITEM_SOAP_PICKABLE */
            NameType.NAME_ITEM_SOAP,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,true,GameSprite.SPRITE_PICKABLE_SOAP,GamePickableItem.ITEM_PICK_SOAP_PICKABLE,
            new ActionConditions[1]{ActionConditions.COND_TAKE_SOAP,}),
            
            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST,}),
            GameSprite.SPRITE_LAST,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            /* > ATG 3 END < */
        };

        private static readonly GameItem[] _PickableToItem = new GameItem[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 4 START < */
            GameItem.ITEM_CARDS_PICKABLE,	/* ITEM_PICK_CARDS_PICKABLE */
            GameItem.ITEM_SOAP_PICKABLE,	/* ITEM_PICK_SOAP_PICKABLE */
            /* > ATG 4 END < */
        };

        private static readonly GameSprite[] _PickableSprite = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 5 START < */
            GameSprite.SPRITE_ITEM_CARDS_PICKABLE,	/* ITEM_PICK_CARDS_PICKABLE */
            GameSprite.SPRITE_PICKABLE_SOAP,	/* ITEM_PICK_SOAP_PICKABLE */
            /* > ATG 5 END < */
        };

        private static readonly MementoParentInfo[] _MementoParentInfo = new MementoParentInfo[(int)MementoParent.MEMENTO_PARENT_TOTAL]
        {
            /* > ATG 6 START < */
            /* MEMENTO_PARENT_JOB_FIND_1 */
            new(
            NameType.NAME_MEMENTO_FIND_JOB_1,GameSprite.SPRITE_MEMENTO_JOB,
            new Memento[2]{Memento.MEMENTO_JOB_FIND_1_1,Memento.MEMENTO_JOB_FIND_1_2,}
            ),
            
            /* MEMENTO_PARENT_LAST */
            new(
            NameType.NAME_NPC_LAST,GameSprite.SPRITE_NONE,
            new Memento[1]{Memento.MEMENTO_LAST,}
            ),
            
            /* > ATG 6 END < */
        };

        private static readonly MementoInfo[] _MementoInfo = new MementoInfo[(int)Memento.MEMENTO_TOTAL]
        {
            /* > ATG 7 START < */
            /* MEMENTO_JOB_FIND_1_1 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_MEMENTO_FIND_JOB_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_JOB_FIND_1_2 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_LAST */
            new(MementoParent.MEMENTO_PARENT_LAST,DialogPhrase.PHRASE_NONE,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* > ATG 7 END < */
        };

        private static readonly MementoCombiInfo[] _MementoCombiInfo = new MementoCombiInfo[(int)MementoCombi.MEMENTO_COMBI_TOTAL]
        {
            /* > ATG 8 START */
            /* > ATG 8 END */
        };
    }
}
