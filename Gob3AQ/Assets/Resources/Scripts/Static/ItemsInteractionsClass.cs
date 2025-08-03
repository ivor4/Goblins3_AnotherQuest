using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.Brain.ItemsInteraction
{ 
    public static class ItemsInteractionsClass
    {
        public static ReadOnlySpan<GamePickableItem> ITEM_TO_PICKABLE => _ItemToPickable;
        public static ReadOnlySpan<bool> IS_PICKABLE_DISPOSABLE => _PickableDisposable;
        public static ReadOnlySpan<ItemConditions> ITEM_CONDITIONS => _ItemConditions;

        private static readonly ItemInteractionInfo[] _FailedInteractionInfo = new ItemInteractionInfo[0];


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
            new(GameEvent.EVENT_NONE,CharacterAnimation.ITEM_USE_ANIMATION_TAKE,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogType.DIALOG_NONE), /* COND_OK */
            new(GameEvent.EVENT_NONE,CharacterAnimation.ITEM_USE_ANIMATION_POUR,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogType.DIALOG_NONE), /* COND_FOUNTAIN */
            new(GameEvent.EVENT_NONE,CharacterAnimation.ITEM_USE_ANIMATION_STARE_SCREEN,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONSENSE,DialogType.DIALOG_NONE), /* COND_FOUNTAIN2 */
            new(GameEvent.EVENT_NONE,CharacterAnimation.ITEM_USE_ANIMATION_TAKE,CharacterAnimation.ITEM_USE_ANIMATION_CONFUSE,
            DialogType.DIALOG_NONE,DialogType.DIALOG_NONE), /* COND_LAST */
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

        /// <summary>
        /// If pickable is disposed once it has been used
        /// </summary>
        private static readonly bool[] _PickableDisposable = new bool[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 3 START < */
            true, /* ITEM_PICK_POTION */
            true, /* ITEM_PICK_POTION_BLUE */
            /* > ATG 3 END < */
        };


        private static readonly ItemInteractionInfo[][] _ItemInteractions = new ItemInteractionInfo[(int)GameItem.ITEM_TOTAL][]
        {
            /* > ATG 4 START < */
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
            /* > ATG 4 END < */
        };

    }
}
