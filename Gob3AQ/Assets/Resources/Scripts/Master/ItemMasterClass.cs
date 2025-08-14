using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
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

        
        public static void UseItemService(in InteractionUsage usage, out CharacterAnimation animation, out DialogType dialog)
        {
            switch(usage.type)
            {
                case InteractionType.PLAYER_WITH_ITEM:
                    TakePickableItem(in usage, out animation, out dialog);
                    break;
                case InteractionType.ITEM_WITH_ITEM:
                    UseItemWithItem(in usage, out animation, out dialog);
                    break;
                case InteractionType.ITEM_WITH_PLAYER:
                    animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
                    dialog = DialogType.DIALOG_NONE;
                    break;
                default:
                    animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
                    dialog = DialogType.DIALOG_NONE;
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

        void Start()
        {
            VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }

        /// <summary>
        /// Determines whether a specific item interaction is permitted based on the provided interaction type and usage
        /// context.
        /// </summary>
        /// <remarks>This method evaluates the interaction based on predefined conditions, including the
        /// source and destination items,  the initiating player, and any associated game events. If the interaction is
        /// permitted, it may trigger additional  events and return the appropriate animation for the
        /// interaction.</remarks>
        /// <param name="interactionType">The type of interaction being attempted, such as use, equip, or inspect.</param>
        /// <param name="usage">The context of the interaction, including the source and destination items and the player initiating the
        /// interaction.</param>
        /// <param name="permitted">When the method returns, contains the permitted interaction type if the interaction is allowed; otherwise, 
        /// <see cref="ItemInteractionType.INTERACTION_NONE"/>.</param>
        /// <param name="animation">When the method returns, contains the animation to be played for the interaction if it is allowed;
        /// otherwise,  contains the animation to be played for a failed interaction.</param>
        /// <param name="dialog">When the method returns, contains the dialog type to be displayed for the interaction if it is allowed;
        /// <returns><see langword="true"/> if the interaction is permitted and consumes the item; otherwise, <see
        /// langword="false"/>.</returns>
        private static bool ItemInteractionCommon(ItemInteractionType interactionType,
            in InteractionUsage usage, out CharacterAnimation animation, out DialogType dialog)
        {
            bool consumeItem;
            ReadOnlySpan<ItemInteractionInfo> infoArray = ItemsInteractionsClass.GetItemInteractions(usage.itemDest);

            /* If item is not defined, it is not possible to process it */
            animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
            consumeItem = false;
            dialog = DialogType.DIALOG_NONE;

            for (int i = 0; i < infoArray.Length; i++)
            {
                ref readonly ItemInteractionInfo interactionInfo = ref infoArray[i];
                
                /* Validation if, check if interaction slot is the one which fits actual conditions */
                if ((usage.playerSource == interactionInfo.srcChar) && (interactionInfo.interaction == interactionType) &&
                    ((interactionType != ItemInteractionType.INTERACTION_USE) || (usage.itemSource == interactionInfo.srcItem)))
                {
                    ref readonly ItemConditions conditions = ref ItemsInteractionsClass.ITEM_CONDITIONS[(int)interactionInfo.conditions];

                    bool occurred;

                    if (conditions.eventType != GameEvent.EVENT_NONE)
                    {
                        VARMAP_ItemMaster.IS_EVENT_OCCURRED(conditions.eventType, out occurred);
                    }
                    else
                    {
                        occurred = true; // If no event is defined, it is considered as occurred
                    }

                    if (occurred)
                    {
                        animation = conditions.animationOK;

                        /* Trigger additional event (in case) */
                        if (interactionInfo.outEvent != GameEvent.EVENT_NONE)
                        {
                            VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.outEvent, true);
                        }

                        dialog = conditions.dialogOK;
                        consumeItem = interactionInfo.consumes;
                    }
                    else
                    {
                        animation = conditions.animationNOK_Event;
                        dialog = conditions.dialogNOK_Event;
                    }
                    break;
                }
            }

            return consumeItem;
        }


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        private static void TakePickableItem(in InteractionUsage usage, out CharacterAnimation animation, out DialogType dialog)
        {
            bool consumeItem = ItemInteractionCommon(ItemInteractionType.INTERACTION_TAKE, in usage, out animation, out dialog);

            if (consumeItem)
            {
                /* If interaction is take, remove item from scenario and trigger events, also add it to inventory */
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemDest];
                /* Get item on this scenario (physically) */
                VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE(usage.itemDest);
                /* Officially take it and set corresponding events.
                    * If it was not in scene, at least an event will be set (which could be useful) */
                VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE_EVENT(pickable);
                /* Set item owner in VARMAP */
                VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, usage.playerSource);
            }
        }

        private static void UseItemWithItem(in InteractionUsage usage, out CharacterAnimation animation, out DialogType dialog)
        {
            bool consumeItem = ItemInteractionCommon(ItemInteractionType.INTERACTION_USE, in usage, out animation, out dialog);

            if (consumeItem)
            {
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemSource];

                /* Pickable items which are already stored in inventory */
                if (pickable != GamePickableItem.ITEM_PICK_NONE)
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

        }
    }
}