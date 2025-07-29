using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Items;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.Item;
using Gob3AQ.Brain.ItemsInteraction;
using System;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;



        public static void SelectPickableItemService(GameItem item)
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(item);
        }

        public static void CancelPickableItemService()
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(GameItem.ITEM_NONE);
        }

        
        public static void UseItemService(in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            switch(usage.type)
            {
                case InteractionType.PLAYER_WITH_ITEM:
                    TakePickableItem(in usage, out permitted, out animation);
                    break;
                case InteractionType.ITEM_WITH_ITEM:
                    UseItemWithItem(in usage, out permitted, out animation);
                    break;
                case InteractionType.ITEM_WITH_PLAYER:
                    permitted = ItemInteractionType.INTERACTION_NONE;
                    animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
                    break;
                default:
                    permitted = ItemInteractionType.INTERACTION_NONE;
                    animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
                    break;
            }
        }



        void Awake()
        {
            if(_singleton)
            {
                Destroy(this);
            }
            else
            {
                _singleton = this;
            }
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }



        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        private static void TakePickableItem(in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            ref readonly ItemInteractionInfo interactionInfo = ref ItemsInteractionsClass.GetItemInteraction(in usage);

            permitted = interactionInfo.type;
            bool valid;

            /* If interaction is take, remove item from scenario and trigger events, also add it to inventory */
            switch (interactionInfo.type)
            {
                case ItemInteractionType.INTERACTION_TAKE:
                    GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemDest];

                    /* Get item on this scenario (physically) */
                    VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE(usage.itemDest);

                    /* Officially take it and set corresponding events.
                     * If it was not in scene, at least an event will be set (which could be useful) */
                    VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE_EVENT(pickable);

                    /* Set item owner in VARMAP */
                    VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, usage.playerSource);

                    valid = true;
                    break;

                case ItemInteractionType.INTERACTION_USE:
                case ItemInteractionType.INTERACTION_RECEIVE:
                case ItemInteractionType.INTERACTION_TAKE_AND_RECEIVE:
                    valid = true;
                    break;

                default:
                    valid = false;
                    break;
            }

            /* Unchain event (in case) */
            if (valid && (interactionInfo.linkedEvent != GameEvent.EVENT_NONE))
            {
                VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.linkedEvent, true);
            }
            

            /* Pass animation */
            animation = interactionInfo.useAnimation;
        }

        private static void UseItemWithItem(in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemSource];
            ref readonly ItemInteractionInfo interactionInfo = ref ItemsInteractionsClass.GetItemInteraction(in usage);
            permitted = interactionInfo.type;

            /* Only if action is permitted */
            if (permitted == ItemInteractionType.INTERACTION_USE)
            {
                /* Pickable items which are already stored in inventory */
                if (pickable != GamePickableItem.ITEM_PICK_NONE)
                {
                    /* Lose item in case it is pickable and disposable */
                    if (ItemsInteractionsClass.IS_PICKABLE_DISPOSABLE[(int)pickable])
                    {
                        VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);

                        /* Diselect in case it was selected */
                        GameItem choosenItem = VARMAP_ItemMaster.GET_PICKABLE_ITEM_CHOSEN();
                        if (choosenItem == usage.itemSource)
                        {
                            VARMAP_ItemMaster.CANCEL_PICKABLE_ITEM();
                        }
                    }
                }

                /* Unchain event (in case) */
                if (interactionInfo.linkedEvent != GameEvent.EVENT_NONE)
                {
                    VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.linkedEvent, true);
                }
            }

            /* Pass animation */
            animation = interactionInfo.useAnimation;
            
        }

    }
}