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

        private static bool ItemInteractionCommon(ItemInteractionType interactionType,
            in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            bool isOK;
            ReadOnlySpan<ItemInteractionInfo> infoArray = ItemsInteractionsClass.GetItemInteractions(usage.itemDest);

            /* If item is not defined, it is not possible to process it */
            permitted = ItemInteractionType.INTERACTION_NONE;
            animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
            isOK = false;

            for (int i = 0; i < infoArray.Length; i++)
            {
                ref readonly ItemInteractionInfo interactionInfo = ref infoArray[i];

                if ((usage.playerSource == interactionInfo.srcChar) && (interactionInfo.interaction == interactionType))
                {
                    ref readonly ItemConditions conditions = ref ItemsInteractionsClass.ITEM_CONDITIONS[(int)interactionInfo.conditions];

                    bool occurred;

                    if (conditions.eventType != GameEvent.EVENT_NONE)
                    {
                        VARMAP_ItemMaster.IS_EVENT_OCCURRED(conditions.eventType, out occurred);
                    }
                    else
                    {
                        occurred = true;
                    }

                    if (occurred)
                    {
                        permitted = interactionInfo.interaction;
                        animation = conditions.animationOK;

                        /* Trigger additional event (in case) */
                        if (interactionInfo.outEvent != GameEvent.EVENT_NONE)
                        {
                            VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.outEvent, true);
                        }

                        /* Dialog OK */

                        isOK = true;
                    }
                    else
                    {
                        permitted = ItemInteractionType.INTERACTION_NONE;
                        animation = conditions.animationNOK_Event;

                        /* Dialog NOK */
                    }
                    break;
                }
            }

            return isOK;
        }


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        private static void TakePickableItem(in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            bool isOk = ItemInteractionCommon(ItemInteractionType.INTERACTION_TAKE, in usage, out permitted, out animation);

            if (isOk)
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

        private static void UseItemWithItem(in InteractionUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            bool isOk = ItemInteractionCommon(ItemInteractionType.INTERACTION_USE, in usage, out permitted, out animation);

            if(isOk)
            {
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemSource];

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
            }

        }
    }
}