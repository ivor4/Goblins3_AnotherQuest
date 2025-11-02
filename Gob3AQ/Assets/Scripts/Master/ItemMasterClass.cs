using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using UnityEngine;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;



        public static void UseItemService(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            ref readonly ItemInfo dstItemInfo = ref _singleton.ItemInteractionCommon(in usage, out outcome);

            /* Additional actions (like consuming items) */
            switch (usage.type)
            {
                case ItemInteractionType.INTERACTION_TAKE:
                    _singleton.TakePickableItem(in usage, in dstItemInfo, in outcome);
                    break;
                case ItemInteractionType.INTERACTION_USE:
                    _singleton.UseItemWithItem(in usage, in outcome);
                    break;
                default:
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
            _ = StartCoroutine(LoadCoroutine());
        }

        private IEnumerator LoadCoroutine()
        {
            bool masterLoaded = false;

            while (!masterLoaded)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out masterLoaded);
            }

            VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }


        private ref readonly ItemInfo ItemInteractionCommon(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            bool conditionOK;
            bool consumeItem;
            CharacterAnimation animation;
            DialogType dialog;
            DialogPhrase phrase;
            GameEvent outEvent;

            /* If item is not defined, it is not possible to process it */
            conditionOK = false;
            consumeItem = false;
            animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
            outEvent = GameEvent.EVENT_NONE;
            dialog = DialogType.DIALOG_SIMPLE;
            phrase = GetDefaultNegativePhrase(usage.type);
            

            /* Retrieve item info and conditions */
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemDest);
            ReadOnlySpan<ActionConditions> conditionsEnumArray = itemInfo.Conditions;

            /* Search for the first condition which matches (char and/or srcItem) */
            for (int i = 0; i < conditionsEnumArray.Length; i++)
            {
                ref readonly ActionConditionsInfo condition = ref ItemsInteractionsClass.GetActionConditionsInfo(conditionsEnumArray[i]);
                
                /* Validation if, check if interaction slot is the one which fits actual conditions */
                if ((usage.playerSource == condition.srcChar) && (condition.actionOK == usage.type) &&
                    ((usage.type != ItemInteractionType.INTERACTION_USE) || (usage.itemSource == condition.srcItem)))
                {

                    VARMAP_ItemMaster.IS_EVENT_COMBI_OCCURRED(condition.NeededEvents, out bool occurred);

                    if (occurred)
                    {
                        /* Trigger additional event (in case) */
                        if (condition.unchainEvent != GameEvent.EVENT_NONE)
                        {
                            VARMAP_ItemMaster.COMMIT_EVENT(condition.unchainEvent, true);
                        }

                        outEvent = condition.unchainEvent;
                        animation = condition.animationOK;
                        dialog = condition.dialogOK;
                        phrase = condition.phraseOK;
                        consumeItem = condition.consumes;
                        conditionOK = true;
                        break;
                    }
                }
            }

            outcome = new(animation, dialog, phrase, outEvent, conditionOK, consumeItem);

            return ref itemInfo;
        }


        /// <summary>
        /// With purpose of take an object from scenario. There would be a different service to retrieve an item from a conversation or other event
        /// </summary>
        /// <param name="item">involved labelled item</param>
        /// <param name="character">Character who took the item</param>
        private void TakePickableItem(in InteractionUsage usage, in ItemInfo dstItemInfo, in InteractionUsageOutcome outcome)
        {
            if (dstItemInfo.isPickable && outcome.ok)
            {
                /* If interaction is take, remove item from scenario and trigger events, also add it to inventory */
                GamePickableItem pickable = dstItemInfo.pickableItem;
                /* Get item on this scenario (physically) */
                VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE(usage.itemDest);
                /* Officially take it and set corresponding events.
                    * If it was not in scene, at least an event will be set (which could be useful) */
                VARMAP_ItemMaster.ITEM_OBTAIN_PICKABLE_EVENT(pickable);
                /* Set item owner in VARMAP */
                VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, usage.playerSource);
            }
        }

        private void UseItemWithItem(in InteractionUsage usage, in InteractionUsageOutcome outcome)
        {
            ref readonly ItemInfo srcItemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemSource);

            if (outcome.consumes && srcItemInfo.isPickable)
            {
                GamePickableItem pickable = srcItemInfo.pickableItem;

                VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);

                /* Diselect in case it was selected */
                GameItem choosenItem = VARMAP_ItemMaster.GET_PICKABLE_ITEM_CHOSEN();
                if (choosenItem == usage.itemSource)
                {
                    VARMAP_ItemMaster.CANCEL_PICKABLE_ITEM();
                }
            }
        }

        private DialogPhrase GetDefaultNegativePhrase(ItemInteractionType interaction)
        {
            switch (interaction)
            {
                case ItemInteractionType.INTERACTION_TALK:
                    return DialogPhrase.PHRASE_NONSENSE_TALK;
                case ItemInteractionType.INTERACTION_OBSERVE:
                    return DialogPhrase.PHRASE_NONSENSE_OBSERVE;
                default:
                    return DialogPhrase.PHRASE_NONSENSE;
            }
        }
    }
}