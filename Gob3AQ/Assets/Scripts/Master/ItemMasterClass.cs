using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.GameElement;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;
        private IReadOnlyDictionary<GameItem, GameElementClass> _levelItems;


        public static void UnchainToItemService(in UnchainInfo unchainInfo)
        {
            if (_singleton != null)
            {
                GameElementClass instance;

                switch (unchainInfo.type)
                {
                    case UnchainType.UNCHAIN_TYPE_EARN_ITEM:
                        EarnLosePickableItem(unchainInfo.targetCharacter, unchainInfo.targetItem, true);
                        break;
                    case UnchainType.UNCHAIN_TYPE_LOSE_ITEM:
                        EarnLosePickableItem(CharacterType.CHARACTER_NONE, unchainInfo.targetItem, false);
                        break;
                    case UnchainType.UNCHAIN_TYPE_SET_SPRITE:
                        if(_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetSprite(unchainInfo.targetSprite);
                        }
                        break;
                    case UnchainType.UNCHAIN_TYPE_SPAWN:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetActive(true);
                            instance.SetClickable(true);
                            instance.SetVisible(true);
                        }
                        break;

                    case UnchainType.UNCHAIN_TYPE_DESPAWN:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.VirtualDestroy();
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static void UseItemService(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            _ = ref ItemInteractionCommon(in usage, out outcome);
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
            VARMAP_ItemMaster.OBTAIN_SCENARIO_ITEMS(out _levelItems);
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

            /* Ensure items are loaded */
            yield return ResourceAtlasClass.WaitForNextFrame;

            VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }

        private static void EarnLosePickableItem(CharacterType character, GameItem item, bool earn)
        {
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(item);

            if (itemInfo.isPickable)
            {
                GamePickableItem pickable = itemInfo.pickableItem;

                if (earn)
                {
                    /* Set item owner in VARMAP */
                    VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, character);
                }
                else
                {
                    VARMAP_ItemMaster.SET_ELEM_PICKABLE_ITEM_OWNER((int)pickable, CharacterType.CHARACTER_NONE);

                    /* Diselect in case it was selected */
                    GameItem choosenItem = VARMAP_ItemMaster.GET_PICKABLE_ITEM_CHOSEN();
                    if (choosenItem == item)
                    {
                        VARMAP_ItemMaster.CANCEL_PICKABLE_ITEM();
                    }
                }
            }
        }

        private static ref readonly ItemInfo ItemInteractionCommon(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            bool conditionOK;
            CharacterAnimation animation;
            DialogType dialog;
            DialogPhrase phrase;

            /* If item is not defined, it is not possible to process it */
            conditionOK = false;
            animation = CharacterAnimation.ITEM_USE_ANIMATION_NONE;
            dialog = DialogType.DIALOG_SIMPLE;
            phrase = GetDefaultNegativePhrase(usage.type);


            /* Retrieve item info and conditions */
            ref readonly ItemInfo srcItemInfo = ref ItemInfo.EMPTY;
            if(usage.itemSource != GameItem.ITEM_NONE)
            {
                srcItemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemSource);
            }
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemDest);
            ReadOnlySpan<ActionConditions> conditionsEnumArray = itemInfo.Conditions;
            ReadOnlySpan<CharacterType> owners = VARMAP_ItemMaster.GET_SHADOW_ARRAY_PICKABLE_ITEM_OWNER();

            /* Search for the first condition which matches (char and/or srcItem) */
            for (int i = 0; i < conditionsEnumArray.Length; i++)
            {
                ref readonly ActionConditionsInfo condition = ref ItemsInteractionsClass.GetActionConditionsInfo(conditionsEnumArray[i]);
                
                /* Validation if, check if interaction slot is the one which fits actual conditions */
                if ((usage.playerSource == condition.srcChar) && (condition.actionOK == usage.type) &&
                    ((usage.type != ItemInteractionType.INTERACTION_USE) ||
                    ((usage.itemSource == condition.srcItem) && srcItemInfo.isPickable && (owners[(int)srcItemInfo.pickableItem] == usage.playerSource))
                    ))
                {
                    VARMAP_ItemMaster.IS_EVENT_COMBI_OCCURRED(condition.NeededEvents, out bool occurred);

                    if (occurred)
                    {
                        /* Trigger additional event (in case) */
                        VARMAP_ItemMaster.COMMIT_EVENT(condition.UnchainEvents);

                        animation = condition.animationOK;
                        dialog = condition.dialogOK;
                        phrase = condition.phraseOK;
                        conditionOK = true;
                        break;
                    }
                }
            }

            outcome = new(animation, dialog, phrase, conditionOK);

            return ref itemInfo;
        }





        private static DialogPhrase GetDefaultNegativePhrase(ItemInteractionType interaction)
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