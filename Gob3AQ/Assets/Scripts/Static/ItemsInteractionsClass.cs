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
            new( /* UNCHAIN_RED_POTION_TOOK */
            UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_RED_POTION_TOOK, false),}, 
            GameItem.ITEM_POTION, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_RED_POTION_TOOK_2 */
            UnchainType.UNCHAIN_TYPE_EARN_ITEM,new(GameEvent.EVENT_RED_POTION_TOOK, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_RED_POTION_TOOK, false),}, 
            GameItem.ITEM_POTION, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_RED_POTION_TOOK_3 */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_RED_POTION_TOOK, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_RED_POTION_TOOK, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_RED_POTION_1, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_BLUE_POTION_TOOK */
            UnchainType.UNCHAIN_TYPE_DESPAWN,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_BLUE_POTION_TOOK, false),}, 
            GameItem.ITEM_POTION_BLUE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_BLUE_POTION_TOOK_2 */
            UnchainType.UNCHAIN_TYPE_EARN_ITEM,new(GameEvent.EVENT_RED_POTION_TOOK, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_BLUE_POTION_TOOK, false),}, 
            GameItem.ITEM_POTION_BLUE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_PARROT,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_OBSERVED */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FOUNTAIN_OBSERVED, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_OBSERVED, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_FOUNTAIN_1, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_THOUGHT_MEMENTO */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FOUNTAIN_THOUGHT_MEMENTO, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_THOUGHT_MEMENTO, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_FOUNTAIN_2, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_FULL */
            UnchainType.UNCHAIN_TYPE_SET_SPRITE,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false),}, 
            GameItem.ITEM_FOUNTAIN, GameSprite.SPRITE_FOUNTAIN_FULL,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_FULL_2 */
            UnchainType.UNCHAIN_TYPE_LOSE_ITEM,new(GameEvent.EVENT_FOUNTAIN_FULL, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false),}, 
            GameItem.ITEM_POTION, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_FULL_3 */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FOUNTAIN_FULL, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_RED_POTION_2, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_FOUNTAIN_FULL_4 */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FOUNTAIN_FULL, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_FOUNTAIN_3, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            /* > ATG 1 END < */
        };



        private static readonly ActionConditionsInfo[] _ActionConditions = new ActionConditionsInfo[(int)ActionConditions.COND_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* COND_OK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_TAKE_POTION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_RED_POTION_TOOK, false),}), 
            
            new( /* COND_OBSERVE_POTION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_RED_POTION,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_OBSERVE_FOUNTAIN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_FOUNTAIN,
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_OBSERVED, false),}), 
            
            new( /* COND_TAKE_POTION_BLUE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_PARROT,GameItem.ITEM_POTION_BLUE,ItemInteractionType.INTERACTION_TAKE,
            CharacterAnimation.ITEM_USE_ANIMATION_TAKE,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BLUE_POTION_TOOK, false),}), 
            
            new( /* COND_FOUNTAIN_THOUGHT */
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_THOUGHT_MEMENTO, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_POTION,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_POUR,
            DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false),}), 
            
            new( /* COND_FOUNTAIN_NOT_THOUGHT */
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_THOUGHT_MEMENTO, true),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_POTION,ItemInteractionType.INTERACTION_USE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE_NOT_THOUGHT,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* COND_TALK_MILITO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_MILITO,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
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
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_POTION_RED,}),
            GameSprite.SPRITE_POTION_RED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_PLAYER_PARROT */
            NameType.NAME_CHAR_PARROT,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_POTION_RED,}),
            GameSprite.SPRITE_POTION_RED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_PLAYER_SNAKE */
            NameType.NAME_CHAR_SNAKE,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_POTION_RED,}),
            GameSprite.SPRITE_POTION_RED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_POTION */
            NameType.NAME_ITEM_POTION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_POTION_RED,}),
            GameSprite.SPRITE_POTION_RED,true,GameSprite.SPRITE_POTION_RED,GamePickableItem.ITEM_PICK_POTION,
            new ActionConditions[2]{ActionConditions.COND_OBSERVE_POTION,ActionConditions.COND_TAKE_POTION,}),
            
            new ( /* ITEM_POTION_BLUE */
            NameType.NAME_ITEM_BLUE_POTION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_POTION_BLUE,}),
            GameSprite.SPRITE_POTION_BLUE,true,GameSprite.SPRITE_POTION_BLUE,GamePickableItem.ITEM_PICK_POTION_BLUE,
            new ActionConditions[1]{ActionConditions.COND_TAKE_POTION_BLUE,}),
            
            new ( /* ITEM_FOUNTAIN */
            NameType.NAME_ITEM_FOUNTAIN,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_FOUNTAIN,GameSprite.SPRITE_FOUNTAIN_FULL,}),
            GameSprite.SPRITE_FOUNTAIN,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[3]{ActionConditions.COND_OBSERVE_FOUNTAIN,ActionConditions.COND_FOUNTAIN_THOUGHT,ActionConditions.COND_FOUNTAIN_NOT_THOUGHT,}),
            
            new ( /* ITEM_NPC_MILITO */
            NameType.NAME_NPC_MILITO,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_MILITO,}),
            GameSprite.SPRITE_NPC_MILITO,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_TALK_MILITO,}),
            
            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST,}),
            GameSprite.SPRITE_LAST,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            /* > ATG 3 END < */
        };

        private static readonly GameItem[] _PickableToItem = new GameItem[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 4 START < */
            GameItem.ITEM_POTION,	/* ITEM_PICK_POTION */
            GameItem.ITEM_POTION_BLUE,	/* ITEM_PICK_POTION_BLUE */
            /* > ATG 4 END < */
        };

        private static readonly GameSprite[] _PickableSprite = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 5 START < */
            GameSprite.SPRITE_POTION_RED,	/* ITEM_PICK_POTION */
            GameSprite.SPRITE_POTION_BLUE,	/* ITEM_PICK_POTION_BLUE */
            /* > ATG 5 END < */
        };

        private static readonly MementoParentInfo[] _MementoParentInfo = new MementoParentInfo[(int)MementoParent.MEMENTO_PARENT_TOTAL]
        {
            /* > ATG 6 START < */
            /* MEMENTO_PARENT_RED_POTION */
            new(
            NameType.NAME_ITEM_POTION,GameSprite.SPRITE_POTION_RED,
            new Memento[2]{Memento.MEMENTO_RED_POTION_1,Memento.MEMENTO_RED_POTION_2,}
            ),
            
            /* MEMENTO_PARENT_FOUNTAIN */
            new(
            NameType.NAME_ITEM_FOUNTAIN,GameSprite.SPRITE_FOUNTAIN_FULL,
            new Memento[3]{Memento.MEMENTO_FOUNTAIN_1,Memento.MEMENTO_FOUNTAIN_2,Memento.MEMENTO_FOUNTAIN_3,}
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
            /* MEMENTO_RED_POTION_1 */
            new(MementoParent.MEMENTO_PARENT_RED_POTION,DialogPhrase.PHRASE_MEMENTO_POTION_1,true,false),
            /* MEMENTO_RED_POTION_2 */
            new(MementoParent.MEMENTO_PARENT_RED_POTION,DialogPhrase.PHRASE_MEMENTO_POTION_2,false,true),
            /* MEMENTO_FOUNTAIN_1 */
            new(MementoParent.MEMENTO_PARENT_FOUNTAIN,DialogPhrase.PHRASE_MEMENTO_FOUNTAIN_1,true,false),
            /* MEMENTO_FOUNTAIN_2 */
            new(MementoParent.MEMENTO_PARENT_FOUNTAIN,DialogPhrase.PHRASE_MEMENTO_FOUNTAIN_2,false,false),
            /* MEMENTO_FOUNTAIN_3 */
            new(MementoParent.MEMENTO_PARENT_FOUNTAIN,DialogPhrase.PHRASE_MEMENTO_FOUNTAIN_3,false,true),
            /* MEMENTO_LAST */
            new(MementoParent.MEMENTO_PARENT_LAST,DialogPhrase.PHRASE_NONE,false,false),
            /* > ATG 7 END < */
        };

        private static readonly MementoCombiInfo[] _MementoCombiInfo = new MementoCombiInfo[(int)MementoCombi.MEMENTO_COMBI_TOTAL]
        {
            /* > ATG 8 START */
            /* MEMENTO_COMBI_POTION_FOUNTAIN */
            new(
            new(new HashSet<Memento>(2){Memento.MEMENTO_RED_POTION_1,Memento.MEMENTO_FOUNTAIN_1}),
            GameEvent.EVENT_FOUNTAIN_THOUGHT_MEMENTO
            ),
            /* > ATG 8 END */
        };
    }
}
