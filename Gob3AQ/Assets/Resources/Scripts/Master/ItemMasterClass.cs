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

        
        public static void UseItemService(in ItemUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            switch(usage.type)
            {
                case ItemUsageType.PLAYER_WITH_ITEM:
                    TakePickableItem(in usage, out permitted, out animation);
                    break;
                case ItemUsageType.ITEM_WITH_ITEM:
                    UseItemWithItem(in usage, out permitted, out animation);
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
        private static void TakePickableItem(in ItemUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
        {
            ref readonly ItemInteractionInfo interactionInfo = ref ItemsInteractionsClass.GetItemInteraction(in usage);

            permitted = interactionInfo.type;

            /* If interaction is take, remove item from scenario and trigger events, also add it to inventory */
            if (interactionInfo.type == ItemInteractionType.INTERACTION_TAKE)
            {
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemDest];

                /* Get item on this scenario (physically) */
                VARMAP_ItemMaster.ITEM_REMOVE_FROM_SCENE(usage.itemDest);

                /* Officially take it and set corresponding events.
                 * If it was not in scene, at least an event will be set (which could be useful) */
                VARMAP_ItemMaster.TAKE_ITEM_FROM_SCENE_EVENT(pickable);

                /* Set item owner in VARMAP */
                VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, usage.playerSource);
            }

            /* Pass animation */
            animation = interactionInfo.useAnimation;
        }

        private static void UseItemWithItem(in ItemUsage usage, out ItemInteractionType permitted, out CharacterAnimation animation)
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
                    CharacterType owner = VARMAP_ItemMaster.GET_ELEM_PICKABLE_ITEM_OWNER((int)pickable);

                    /* Lose item in case it is pickable and disposable */
                    if ((owner == usage.playerSource) && ItemsInteractionsClass.IS_PICKABLE_DISPOSABLE[(int)pickable])
                    {
                        VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);
                    }
                }

                /* Unchain event (in case) */
                if (interactionInfo.linkedEvent != GameEvent.GEVENT_NONE)
                {
                    VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.linkedEvent, true);
                }
            }

            /* Pass animation */
            animation = interactionInfo.useAnimation;

            /* No more available */
            VARMAP_ItemMaster.CANCEL_PICKABLE_ITEM();
        }

    }
}