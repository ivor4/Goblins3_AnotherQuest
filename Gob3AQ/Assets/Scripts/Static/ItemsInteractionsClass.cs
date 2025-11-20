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
            new( /* UNCHAIN_ROOM1_INITIAL_MEMENTO */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_VICTIM_CASE_0, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_ROOM1_INITIAL_EVENT */
            UnchainType.UNCHAIN_TYPE_EVENT,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE, 
            new GameEventCombi[1]{new(GameEvent.EVENT_FIRST, false),}), 
            
            new( /* UNCHAIN_ROOM1_OBSERVE_VICTIM */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_OBSERVE_VICTIM, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVE_VICTIM, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_VICTIM_CASE_1, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_ROOM1_MEMENTO_CHASE_0 */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_AWARE_CHASE_1, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_AWARE_CHASE_1, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_CHASE_0, 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}), 
            
            new( /* UNCHAIN_ROOM1_MEMENTO_CHASE_1 */
            UnchainType.UNCHAIN_TYPE_MEMENTO,new(GameEvent.EVENT_AWARE_CHASE_2, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_AWARE_CHASE_2, false),}, 
            GameItem.ITEM_NONE, GameSprite.SPRITE_NONE,CharacterType.CHARACTER_NONE,Memento.MEMENTO_CHASE_1, 
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
            
            new( /* COND_OBSERVE_VICTIM */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_ROOM1_OBSERVE_VICTIM,
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVE_VICTIM, false),}), 
            
            new( /* COND_TALK_WITNESS1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_WITNESS1,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_AWARE_CHASE_1, false),}), 
            
            new( /* COND_TALK_WITNESS2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_WITNESS2,DialogPhrase.PHRASE_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_AWARE_CHASE_2, false),}), 
            
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
            
            new ( /* ITEM_PLAYER_PARROT */
            NameType.NAME_CHAR_PARROT,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE,}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_PLAYER_SNAKE */
            NameType.NAME_CHAR_SNAKE,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE,}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_VICTIM */
            NameType.NAME_VICTIM,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ROOM1_VICTIM,}),
            GameSprite.SPRITE_ROOM1_VICTIM,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OBSERVE_VICTIM,}),
            
            new ( /* ITEM_WITNESS1 */
            NameType.NAME_WITNESS,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ROOM1_WITNESS1,}),
            GameSprite.SPRITE_ROOM1_WITNESS1,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_TALK_WITNESS1,}),
            
            new ( /* ITEM_WITNESS2 */
            NameType.NAME_WITNESS,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ROOM1_WITNESS2,}),
            GameSprite.SPRITE_ROOM1_WITNESS2,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_TALK_WITNESS2,}),
            
            new ( /* ITEM_WITNESS3 */
            NameType.NAME_WITNESS,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ROOM1_WITNESS3,}),
            GameSprite.SPRITE_ROOM1_WITNESS3,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_DOOR_NORMAL */
            NameType.NAME_DOOR,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE,}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST,}),
            GameSprite.SPRITE_LAST,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,
            new ActionConditions[1]{ActionConditions.COND_OK,}),
            
            /* > ATG 3 END < */
        };

        private static readonly GameItem[] _PickableToItem = new GameItem[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 4 START < */
            /* > ATG 4 END < */
        };

        private static readonly GameSprite[] _PickableSprite = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 5 START < */
            /* > ATG 5 END < */
        };

        private static readonly MementoParentInfo[] _MementoParentInfo = new MementoParentInfo[(int)MementoParent.MEMENTO_PARENT_TOTAL]
        {
            /* > ATG 6 START < */
            /* MEMENTO_PARENT_VICTIM_CASE */
            new(
            NameType.NAME_MEMENTO_CASE,GameSprite.SPRITE_ROOM1_VICTIM,
            new Memento[2]{Memento.MEMENTO_VICTIM_CASE_0,Memento.MEMENTO_VICTIM_CASE_1,}
            ),
            
            /* MEMENTO_PARENT_CHASE */
            new(
            NameType.NAME_MEMENTO_CHASE,GameSprite.SPRITE_MEMENTO_CHASE,
            new Memento[2]{Memento.MEMENTO_CHASE_0,Memento.MEMENTO_CHASE_1,}
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
            /* MEMENTO_VICTIM_CASE_0 */
            new(MementoParent.MEMENTO_PARENT_VICTIM_CASE,DialogPhrase.PHRASE_MEMENTO_VICTIM_CASE_0,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            true,false),
            /* MEMENTO_VICTIM_CASE_1 */
            new(MementoParent.MEMENTO_PARENT_VICTIM_CASE,DialogPhrase.PHRASE_MEMENTO_VICTIM_CASE_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false,false),
            /* MEMENTO_CHASE_0 */
            new(MementoParent.MEMENTO_PARENT_CHASE,DialogPhrase.PHRASE_MEMENTO_CHASE_0,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            true,false),
            /* MEMENTO_CHASE_1 */
            new(MementoParent.MEMENTO_PARENT_CHASE,DialogPhrase.PHRASE_MEMENTO_CHASE_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false,false),
            /* MEMENTO_LAST */
            new(MementoParent.MEMENTO_PARENT_LAST,DialogPhrase.PHRASE_NONE,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false,false),
            /* > ATG 7 END < */
        };

        private static readonly MementoCombiInfo[] _MementoCombiInfo = new MementoCombiInfo[(int)MementoCombi.MEMENTO_COMBI_TOTAL]
        {
            /* > ATG 8 START */
            /* > ATG 8 END */
        };
    }
}
