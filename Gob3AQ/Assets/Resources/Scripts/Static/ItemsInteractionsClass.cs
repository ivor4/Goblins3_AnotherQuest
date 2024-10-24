using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.Brain.ItemsInteraction
{
    public static class ItemsInteractionsClass
    {
        public static ReadOnlySpan<GamePickableItem> ITEM_TO_PICKABLE => _ItemToPickable;

        public static ReadOnlySpan<bool> IS_PICKABLE_DISPOSABLE => _PickableDisposable;


        private static readonly GamePickableItem[] _ItemToPickable = new GamePickableItem[(int)GameItem.ITEM_TOTAL]
        {
            GamePickableItem.ITEM_PICK_NONE,    /* ITEM_NONE */
            GamePickableItem.ITEM_PICK_POTION,  /* ITEM_POTION */
            GamePickableItem.ITEM_PICK_NONE     /* ITEM_FOUNTAIN */
        };

        private static bool[] _PickableDisposable = new bool[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            false,  /* ITEM_PICK_NONE */
            true   /* ITEM_PICK_POTION */
        };

        private static readonly ItemInteractionInfo _InvalidInteraction = 
            new ItemInteractionInfo(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                CharacterAnimation.ITEM_USE_ANIMATION_NONE);


        private static readonly ItemInteractionInfo[,] _PlayerWithItemIteraction =
            new ItemInteractionInfo[(int)CharacterType.CHARACTER_TOTAL - 1, (int)GameItem.ITEM_TOTAL - 1]
            {
                /* CHARACTER_MAIN */
                {
                    new(ItemInteractionType.INTERACTION_TAKE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_POTION */
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_FOUNTAIN */
                },
                /* CHARACTER_PARROT */
                {
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_POTION */
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_FOUNTAIN */
                },
                /* CHARACTER_SNAKE */
                {
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_POTION */
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL)    /* ITEM_FOUNTAIN */
                }
            };

        private static readonly ItemInteractionInfo[,] _ItemWithItemIteraction =
            new ItemInteractionInfo[(int)GameItem.ITEM_TOTAL - 1, (int)GameItem.ITEM_TOTAL - 1]
            {
                /* ITEM_POTION */
                {
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_POTION */
                    new(ItemInteractionType.INTERACTION_USE, GameEvent.GEVENT_FOUNTAIN_FULL,
                        CharacterAnimation.ITEM_USE_ANIMATION_POUR),    /* ITEM_FOUNTAIN */
                },
                /* ITEM_FOUNTAIN */
                {
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL),   /* ITEM_POTION */
                    new(ItemInteractionType.INTERACTION_NONE, GameEvent.GEVENT_NONE,
                        CharacterAnimation.ITEM_USE_ANIMATION_NORMAL)    /* ITEM_FOUNTAIN */
                }
            };


        public static ref readonly ItemInteractionInfo GetItemInteraction(in ItemUsage usage)
        {
            ref readonly ItemInteractionInfo interaction = ref _InvalidInteraction;
            switch (usage.type)
            {
                case ItemUsageType.PLAYER_WITH_ITEM:
                    if ((usage.playerSource != CharacterType.CHARACTER_NONE)&&(usage.itemDest != GameItem.ITEM_NONE))
                    {
                        interaction = ref _PlayerWithItemIteraction[(int)usage.playerSource - 1, (int)usage.itemDest - 1];
                    }
                    break;

                case ItemUsageType.ITEM_WITH_ITEM:
                    if((usage.itemSource != GameItem.ITEM_NONE)&&(usage.itemDest != GameItem.ITEM_NONE))
                    {
                        interaction = ref _ItemWithItemIteraction[(int)usage.itemSource - 1, (int)usage.itemDest - 1];
                    }
                    break;
                case ItemUsageType.ITEM_WITH_PLAYER:
                    break;
                case ItemUsageType.ITEM_WITH_NPC:
                    break;
                default:
                    break; 
            }

            return ref interaction;
        }
    }
}
