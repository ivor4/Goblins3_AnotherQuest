using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.Libs.Arith;

namespace Gob3AQ.ItemMaster
{
    public class ItemMasterClass : MonoBehaviour
    {
        private static ItemMasterClass _singleton;
        private IReadOnlyDictionary<GameItem, GameElementClass> _levelItems;
        private int itemsToLoad;
        private int itemsLoaded;


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
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetSprite(unchainInfo.targetSprite);
                        }
                        break;
                    case UnchainType.UNCHAIN_TYPE_SPAWN:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetUnspawned(false);
                            instance.SetMotion(true);
                            instance.SetActive(true);
                            instance.SetClickable(true);
                            instance.SetVisible(true);
                        }
                        break;

                    case UnchainType.UNCHAIN_TYPE_DESPAWN:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetUnspawned(true);
                            instance.SetMotion(false);
                            instance.SetActive(false);
                            instance.SetClickable(false);
                            instance.SetVisible(false);
                        }
                        break;

                    case UnchainType.UNCHAIN_TYPE_DESTROY:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.VirtualDestroy();
                        }
                        break;
                    case UnchainType.UNCHAIN_TYPE_UNCLICKABLE:
                        if (_singleton._levelItems.TryGetValue(unchainInfo.targetItem, out instance))
                        {
                            instance.SetUnclickable(true);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static void UseItemService(in InteractionUsage usage, out InteractionUsageOutcome outcome)
        {
            ItemInteractionCommon(in usage, out outcome);
        }


        public static void AddOneItemToLoad()
        {
            if (_singleton != null)
            {
                _singleton.itemsToLoad++;
            }
        }

        public static void AddOneItemLoaded()
        {
            if (_singleton != null)
            {
                _singleton.itemsLoaded++;
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
            VARMAP_ItemMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
            itemsLoaded = 0;
            itemsToLoad = 0;
        }

        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
                VARMAP_ItemMaster.UNREG_GAMESTATUS(_GameStatusChanged);
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

        private static void ItemInteractionCommon(in InteractionUsage usage, out InteractionUsageOutcome outcome)
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
            MomentType actualMoment = VARMAP_ItemMaster.GET_DAY_MOMENT();


            /* Retrieve item info and conditions */
            ref readonly ItemInfo srcItemInfo = ref ItemInfo.EMPTY;
            if(usage.itemSource != GameItem.ITEM_NONE)
            {
                srcItemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemSource);
            }
            ref readonly ItemInfo itemInfo = ref ItemsInteractionsClass.GetItemInfo(usage.itemDest);
            ReadOnlyHashSet<ActionConditions> conditionsEnumArray = itemInfo.conditions;
            ReadOnlySpan<CharacterType> owners = VARMAP_ItemMaster.GET_SHADOW_ARRAY_PICKABLE_ITEM_OWNER();

            /* Search for the first condition which matches (char and/or srcItem) */
            foreach(ActionConditions condition in conditionsEnumArray)
            {
                ref readonly ActionConditionsInfo conditionInfo = ref ItemsInteractionsClass.GetActionConditionsInfo(condition);
                
                /* Validation if, check if interaction slot is the one which fits actual conditions */
                if (((usage.playerSource == conditionInfo.srcChar)||(conditionInfo.srcChar == CharacterType.CHARACTER_NONE)) && (conditionInfo.actionOK == usage.type) &&
                    ((conditionInfo.momentType == MomentType.MOMENT_ANY) || (conditionInfo.momentType == actualMoment)) &&
                    ((usage.type != ItemInteractionType.INTERACTION_USE) ||
                    ((usage.itemSource == conditionInfo.srcItem) && srcItemInfo.isPickable && (owners[(int)srcItemInfo.pickableItem] == usage.playerSource))
                    ))
                {
                    VARMAP_ItemMaster.IS_EVENT_COMBI_OCCURRED(conditionInfo.NeededEvents, out bool occurred);

                    if (occurred)
                    {
                        /* Trigger additional event (in case) */
                        VARMAP_ItemMaster.COMMIT_EVENT(conditionInfo.UnchainEvents);
        
                        animation = conditionInfo.animationOK;
                        dialog = conditionInfo.dialogOK;
                        phrase = conditionInfo.phraseOK;
                        conditionOK = true;
                        break;
                    }
                }
            }

            outcome = new(animation, dialog, phrase, conditionOK);
        }

        private IEnumerator LoadingCoroutine()
        {
            bool moduleLoaded = false;

            while (!moduleLoaded)
            {
                VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out moduleLoaded);
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            yield return ResourceAtlasClass.WaitForNextFrame;

            while (itemsLoaded < itemsToLoad)
            {
                yield return ResourceAtlasClass.WaitForNextFrame;
            }

            VARMAP_ItemMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_ItemMaster);
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

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch(newval)
                {
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        itemsLoaded = 0;
                        itemsToLoad = 0;
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_ItemMaster.OBTAIN_SCENARIO_ITEMS(out _levelItems);
                        StartCoroutine(LoadingCoroutine());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}