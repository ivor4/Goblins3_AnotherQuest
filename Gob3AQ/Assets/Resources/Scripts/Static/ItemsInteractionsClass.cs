using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;

namespace Gob3AQ.Brain.ItemsInteraction
{
    public static class ItemsInteractionsClass
    {
        public static ReadOnlySpan<GamePickableItem> ITEM_TO_PICKABLE => _ItemToPickable;

        private static readonly GamePickableItem[] _ItemToPickable = new GamePickableItem[(int)GameItem.ITEM_TOTAL]
        {
            GamePickableItem.ITEM_PICK_NONE,    /* ITEM_NONE */
            GamePickableItem.ITEM_PICK_POTION,  /* ITEM_POTION */
            GamePickableItem.ITEM_PICK_NONE     /* ITEM_FORK */
        };

        private static readonly InteractionItemType[,] _PlayerWithItemIteraction =
            new InteractionItemType[(int)CharacterType.CHARACTER_TOTAL - 1, (int)GameItem.ITEM_TOTAL - 1]
            {
                /* CHARACTER_MAIN */
                {
                    InteractionItemType.INTERACTION_TAKE,   /* ITEM_POTION */
                    InteractionItemType.INTERACTION_NONE    /* ITEM_FORK */
                },
                /* CHARACTER_PARROT */
                {
                    InteractionItemType.INTERACTION_NONE,   /* ITEM_POTION */
                    InteractionItemType.INTERACTION_NONE    /* ITEM_FORK */
                },
                /* CHARACTER_SNAKE */
                {
                    InteractionItemType.INTERACTION_NONE,   /* ITEM_POTION */
                    InteractionItemType.INTERACTION_NONE    /* ITEM_FORK */
                }
            };
        

        public static InteractionItemType GetItemInteraction(in ItemUsage usage)
        {
            InteractionItemType interaction = InteractionItemType.INTERACTION_NONE;

            switch(usage.type)
            {
                case ItemUsageType.PLAYER_WITH_ITEM:
                    if ((usage.playerSource != CharacterType.CHARACTER_NONE)&&(usage.itemDest != GameItem.ITEM_NONE))
                    {
                        interaction = _PlayerWithItemIteraction[(int)usage.playerSource - 1, (int)usage.itemDest - 1];
                    }
                    break;

                case ItemUsageType.ITEM_WITH_ITEM:
                    break;
                case ItemUsageType.ITEM_WITH_PLAYER:
                    break;
                case ItemUsageType.ITEM_WITH_NPC:
                    break;
                default:
                    break; 
            }

            return interaction;
        }
    }
}
