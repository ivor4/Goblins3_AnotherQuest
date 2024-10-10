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
            GamePickableItem.ITEM_PICK_POTION,  /* ITEM_POTION */
            GamePickableItem.ITEM_PICK_NONE     /* ITEM_FORK */
        };

        public static InteractionItemType GetItemInteraction(CharacterType character, GameItem item)
        {
            InteractionItemType interaction = InteractionItemType.INTERACTION_NONE;

            switch (item)
            {
                case GameItem.ITEM_POTION:
                    if (character == CharacterType.CHARACTER_MAIN)
                    {
                        interaction = InteractionItemType.INTERACTION_TAKE;
                    }
                    break;

                default:
                    break;
            }

            return interaction;
        }
    }
}
