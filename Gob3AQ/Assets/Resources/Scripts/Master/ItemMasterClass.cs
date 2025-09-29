using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Item;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;
        private static uint _itemsAvailableToLoad;
        private static uint _itemsLoaded;



        public static void SelectPickableItemService(GameItem item)
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(item);
        }

        public static void CancelPickableItemService()
        {
            VARMAP_ItemMaster.SET_PICKABLE_ITEM_CHOSEN(GameItem.ITEM_NONE);
        }


        public static void UseItemService(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            switch(usage.type)
            {
                case InteractionType.PLAYER_WITH_ITEM:
                    TakePickableItem(in usage, out outcome);
                    break;
                case InteractionType.ITEM_WITH_ITEM:
                    UseItemWithItem(in usage, out outcome);
                    break;
                case InteractionType.ITEM_WITH_PLAYER:
                    outcome = new(CharacterAnimation.ITEM_USE_ANIMATION_NONE, DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE, GameEvent.EVENT_NONE, false);
                    break;
                default:
                    outcome = new(CharacterAnimation.ITEM_USE_ANIMATION_NONE, DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE, GameEvent.EVENT_NONE, false);
                    break;
            }
        }


        public static void SetItemAvailableForLoad(GameItem item)
        {
            _itemsAvailableToLoad |= (uint)(1 << (int)item);
        }

        public static void SetItemLoaded(GameItem item)
        {
            _itemsLoaded |= (uint)(1 << (int)item);

            if (_itemsLoaded == _itemsAvailableToLoad)
            {
                VARMAP_PlayerMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
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
            _itemsAvailableToLoad = 0;
            _itemsLoaded = 0;
            

            _ = StartCoroutine(LoadCoroutine());
        }

        private IEnumerator LoadCoroutine()
        {
            yield return new WaitForNextFrameUnit();

            if(_itemsAvailableToLoad == 0)
            {
                // No items to load, just notify
                VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
            }
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }


        private static void ItemInteractionCommon(ItemInteractionType interactionType,
            in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            bool consumeItem;
            CharacterAnimation animation;
            DialogType dialog;
            DialogPhrase phrase;
            GameEvent outEvent;
            ReadOnlySpan<ItemInteractionInfo> infoArray = ItemsInteractionsClass.GetItemInteractions(usage.itemDest);

            /* If item is not defined, it is not possible to process it */
            animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
            consumeItem = false;
            dialog = DialogType.DIALOG_NONE;
            phrase = DialogPhrase.PHRASE_NONE;
            outEvent = GameEvent.EVENT_NONE;

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
                        occurred ^= conditions.eventNOT; // If condition is NOT, invert result
                    }
                    else
                    {
                        occurred = true; // If no event is defined, it is considered as occurred
                    }

                    if (occurred)
                    {
                        /* Trigger additional event (in case) */
                        if (interactionInfo.outEvent != GameEvent.EVENT_NONE)
                        {
                            VARMAP_ItemMaster.COMMIT_EVENT(interactionInfo.outEvent, true);
                        }

                        outEvent = interactionInfo.outEvent;
                        animation = conditions.animationOK;
                        dialog = conditions.dialogOK;
                        phrase = conditions.phraseOK;
                        consumeItem = interactionInfo.consumes;
                    }
                    else
                    {
                        phrase = conditions.phraseNOK_Event;
                        dialog = conditions.dialogNOK_Event;
                        animation = conditions.animationNOK_Event;
                    }
                    break;
                }
            }

            outcome = new(animation, dialog, phrase, outEvent, consumeItem);
        }


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        private static void TakePickableItem(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            ItemInteractionCommon(ItemInteractionType.INTERACTION_TAKE, in usage, out outcome);

            if (outcome.consumes)
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

        private static void UseItemWithItem(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            ItemInteractionCommon(ItemInteractionType.INTERACTION_USE, in usage, out outcome);

            if (outcome.consumes)
            {
                GamePickableItem pickable = ItemsInteractionsClass.ITEM_TO_PICKABLE[(int)usage.itemSource];

                /* Pickable items which are already stored in inventory */
                if (pickable != GamePickableItem.ITEM_PICK_NONE)
                {
                    VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);

                    /* Diselect in case it was selected */
                    GameItem choosenItem = VARMAP_ItemMaster.GET_SHADOW_PICKABLE_ITEM_CHOSEN();
                    if (choosenItem == usage.itemSource)
                    {
                        VARMAP_ItemMaster.CANCEL_PICKABLE_ITEM();
                    }
                }
            }

        }
    }
}