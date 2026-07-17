using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types.Cards;

namespace Gob3AQ.Brain.ItemsInteraction
{ 
    public static class ItemsInteractionsClass
    {
        public static ref readonly MementoCombiInfo GetMementoCombiInfo(MementoCombi mementoCombi)
        {
            if ((uint)mementoCombi >= (uint)MementoCombi.MEMENTO_COMBI_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoCombiInfo: Invalid memento combi item {mementoCombi}");
                return ref MementoCombiInfo.EMPTY;
            }
            else
            {
                return ref _MementoCombiInfo[(int)mementoCombi];
            }
        }

        public static ref readonly MementoParentInfo GetMementoParentInfo(MementoParent mementoParent)
        {
            if ((uint)mementoParent >= (uint)MementoParent.MEMENTO_PARENT_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoParentInfo: Invalid memento parent item {mementoParent}");
                return ref MementoParentInfo.EMPTY;
            }
            else
            {
                return ref _MementoParentInfo[(int)mementoParent];
            }
        }

        public static ref readonly MementoInfo GetMementoInfo(Memento memento)
        {
            if ((uint)memento >= (uint)Memento.MEMENTO_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetMementoInfo: Invalid memento item {memento}");
                return ref MementoInfo.EMPTY;
            }
            else
            {
                return ref _MementoInfo[(int)memento];
            }
        }

        public static GameItem GetItemFromPickable(GamePickableItem pickable)
        {
            if((uint)pickable >= (uint)GamePickableItem.ITEM_PICK_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemFromPickable: Invalid pickable item {pickable}");
                return GameItem.ITEM_NONE;
            }
            else
            {
                return _PickableToItem[(int)pickable];
            }
        }

        public static GameSprite GetSpriteFromPickable(GamePickableItem pickable)
        {
            if ((uint)pickable >= (uint)GamePickableItem.ITEM_PICK_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetSpriteFromPickable: Invalid pickable item {pickable}");
                return GameSprite.SPRITE_NONE;
            }
            else
            {
                return _PickableSprite[(int)pickable];
            }
        }

        public static ref readonly ItemInfo GetItemInfo(GameItem item)
        {
            if((uint)item >= (uint)GameItem.ITEM_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {item}");
                return ref ItemInfo.EMPTY;
            }
            else
            {
                return ref _ItemInfo[(int)item];
            }
        }

        public static ref readonly UnchainInfo GetUnchainInfo(UnchainConditions condition)
        {
            if ((uint)condition >= (uint)UnchainConditions.UNCHAIN_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {condition}");
                return ref UnchainInfo.EMPTY;
            }
            else
            {
                return ref _UnchainConditions[(int)condition];
            }
        }

        public static ref readonly ActionConditionsInfo GetActionConditionsInfo(ActionConditions conditions)
        {
            if ((uint)conditions >= (uint)ActionConditions.COND_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetItemInfo: Invalid item {conditions}");
                return ref ActionConditionsInfo.EMPTY;
            }
            else
            {
                return ref _ActionConditions[(int)conditions];
            }
        }

        public static ref readonly ActionInfo GetActionInfo(GameAction action)
        {
            if ((uint)action >= (uint)GameAction.ACTION_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetActionInfo: Invalid action {action}");
                return ref ActionInfo.EMPTY;
            }
            else
            {
                return ref _ActionInfo[(int)action];
            }
        }

        public static ref readonly DetailInfo GetDetailInfo(DetailType detailType)
        {
            if ((uint)detailType >= (uint)DetailType.DETAIL_TOTAL)
            {
                Debug.LogError($"[ItemsInteractionsClass] GetDetailInfo: Invalid detail type {detailType}");
                return ref DetailInfo.EMPTY;
            }
            else
            {
                return ref _DetailInfo[(int)detailType];
            }
        }


        private static readonly UnchainInfo[] _UnchainConditions = new UnchainInfo[(int)UnchainConditions.UNCHAIN_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* UNCHAIN_NONE */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_NONE}), 

            new( /* UNCHAIN_ROOM1_INITIAL_MEMENTO_1 */
            false,false,true,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_EVENT_INITIAL_MEMENTO, GameAction.ACTION_MEMENTO_INITIAL_MEMENTO}), 

            new( /* UNCHAIN_HIVE1_OPEN_CHEST_2 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SET_SPRITE_CHEST_OPENED, GameAction.ACTION_SPAWN_HIVE1_PERFUME}), 

            new( /* UNCHAIN_HIVE1_CLOSE_CHEST_2 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SET_SPRITE_CHEST_CLOSED, GameAction.ACTION_DESPAWN_HIVE1_PERFUME}), 

            new( /* UNCHAIN_CARDS_PICKABLE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_HIVE1_CARDS}), 

            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_1 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SPAWN_HIVE1_WARDROBE_OPENED, GameAction.ACTION_DESPAWN_HIVE1_WARDROBE}), 

            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_2 */
            false,true,false,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_SOAP}), 

            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_1 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SPAWN_HIVE1_WARDROBE, GameAction.ACTION_DESPAWN_HIVE1_WARDROBE_OPENED}), 

            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_2 */
            false,true,false,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESPAWN_SOAP}), 

            new( /* UNCHAIN_SOAP_PICKABLE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_SOAP}), 

            new( /* UNCHAIN_REME_DAY */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_REME_DAY}), 

            new( /* UNCHAIN_PHARMACY_DOOR_AVAIL */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_PHARMACY_DOOR}), 

            new( /* UNCHAIN_MANYO_OWNER */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_MANYO_OWNER_DAY}), 

            new( /* UNCHAIN_MANYO_OWNER_NIGHT */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_MANYO_OWNER_NIGHT}), 

            new( /* UNCHAIN_UMBRELLA_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_UMBRELLA}), 

            new( /* UNCHAIN_ITEM_HIVE1_POOR_MAN_WC_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false), new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_POOR_MAN_WC}), 

            new( /* UNCHAIN_ITEM_HIVE1_ROACH_HEAD_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_CHEATED, true)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_ROACH_HEAD}), 

            new( /* UNCHAIN_ITEM_HIVE1_ROACH_HEAD_DESPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, false)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_DESPAWN_HIVE1_ROACH_HEAD}), 

            new( /* UNCHAIN_REBUILD_COCHROACH_SCARED */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_COCKROACH_SCARED, false), new(GameEvent.EVENT_COCKROACH_CHEATED, true)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_EVENT_REBUILD_ROACH_SCARED}), 

            new( /* UNCHAIN_ITEM_HIVE1_SHOELACE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_HIVE1_SHOELACE_PICKABLE_TAKEN, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_HIVE1_SHOELACE}), 

            new( /* UNCHAIN_ITEM_HIVE1_VALVE_BOX_DESPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_DESTROY_HIVE1_VALVE_BOX, GameAction.ACTION_SPAWN_HIVE1_VALVE}), 

            new( /* UNCHAIN_ITEM_HIVE1_MAN_WC_CURED_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false), new(GameEvent.EVENT_TALKED_ARTURO_HALL_INN_COMPLETED, true)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_MAN_WC_CURED}), 

            new( /* UNCHAIN_INK_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_WASTED, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_DESPAWN_INKWELL, GameAction.ACTION_SPAWN_INK}), 

            new( /* UNCHAIN_FIK_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[2]{GameAction.ACTION_SPAWN_FIK_1, GameAction.ACTION_SPWAN_GERMAN_1}), 

            new( /* UNCHAIN_MANYO_BCKG_DIALOGUE */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_DIALOGUE_MANYO_BCKG}), 

            new( /* UNCHAIN_POOR_MAN_WC_BCKG_DIALOGUE */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false), new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_POOR_MAN_WC_BCKG}), 

            new( /* UNCHAIN_EXTRAPERLO_DOOR */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, true)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_EXTRAPERLO_DOOR}), 

            new( /* UNCHAIN_SHOW_EXTRAPERLO_DOOR_OPENED */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, false)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[3]{GameAction.ACTION_SPAWN_EXTRAPERLO_WALL, GameAction.ACTION_DESPAWN_EXTRAPERLO_DOOR, GameAction.ACTION_SPAWN_EXTRAPERLO_DOOR_REAL}), 

            new( /* UNCHAIN_SET_SPRITE_WATER_FLOWING_DAY */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_ANIMATE_WATER_FLOWING_DAY}), 

            new( /* UNCHAIN_SET_SPRITE_WATER_FLOWING_NIGHT */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_ANIMATE_WATER_FLOWING_NIGHT}), 

            new( /* UNCHAINER_ITEM_OBJECT_OLIVE_BOWL_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_WAITER_EXTRAPERLO_GAVE_OLIVES, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_ITEM_OBJECT_OLIVE_BOWL}), 

            new( /* UNCHAINER_ITEM_OBJECT_BEER_FULL_SPAWN */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_JAR_READY, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_ITEM_OBJECT_BEER_FULL}), 

            new( /* UNCHAINER_DIALOG_MAINCHAR_NEED_ORINE */
            true,false,false,new(GameEvent.EVENT_NEED_ORINE_SAID, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NEED_ORINE_SAID, true)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[3]{GameAction.ACTION_EVENT_NEED_ORINE_SAID, GameAction.ACTION_OBTAIN_FULL_BLADDER, GameAction.ACTION_DIALOGUE_SIMPLE_MAINCHAR_NEED_ORINE}), 

            new( /* UNCHAIN_SILVANA_GARDEN_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_USED_ORINE_FLOWER, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_SILVANA_GARDEN}), 

            new( /* UNCHAINER_DIALOG_SILVANA_MAINCHAR_GARDEN_LONG */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_SILVANA_GARDEN_2}), 

            new( /* UNCHAIN_RECAP_EXTRAPERLO_GARDEN_IN_ROOM */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_RECAP_EXTRAPERLO_GARDEN, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_DIALOGUE_MAINCHAR_RECAP_EXTRAPERLO, GameAction.ACTION_EVENT_REMOVE_PENDING_RECAP_EXTRAPERLO_GARDEN}), 

            new( /* UNCHAIN_ENTRY_DIALOG_DREAM_1 */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DREAM_1, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[3]{GameAction.ACTION_EVENT_REMOVE_PENDING_DREAM_1, GameAction.ACTION_DIALOGUE_MAINCHAR_ENTRY_DREAM_1, GameAction.ACTION_EVENT_PENDING_DESCRIBE_DREAM_FRAMEWORK}), 

            new( /* UNCHAIN_PLAY_SOUND_CHAPTER_IN */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_PLAY_SOUND_CHAPTER_IN}), 

            new( /* UNCHAIN_CLEAR_EPHIMERAL_ON */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_EVENT_EPHIMERAL_OFF}), 

            new( /* UNCHAIN_ENTRY_DIALOG_DREAM_1_FRAMEWORK */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DESCRIBE_DREAM_FRAMEWORK, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_EVENT_REMOVE_DESCRIBE_DREAM_FRAMEWORK, GameAction.ACTION_DIALOGUE_MAINCHAR_ENTRY_DREAM_1_FRAMEWORK}), 

            new( /* UNCHAIN_LAST */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_NONE}), 

            /* > ATG 1 END < */
        };



        private static readonly ActionConditionsInfo[] _ActionConditions = new ActionConditionsInfo[(int)ActionConditions.COND_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* COND_OK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_NONE}), 

            new( /* COND_OPEN_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_OPEN_CHEST}), 

            new( /* COND_CLOSE_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_CANCEL_OPEN_CHEST}), 

            new( /* COND_TAKE_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_OBTAIN_HIVE1_CARDS, GameAction.ACTION_EVENT_CARDS_PICKABLE_TAKEN}), 

            new( /* COND_OPEN_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_OPEN_WARDROBE}), 

            new( /* COND_CLOSE_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_CANCEL_OPEN_WARDROBE}), 

            new( /* COND_TALK_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_REME_1}), 

            new( /* COND_OBSERVE_HIVE1_AD_BOARD_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_HIVE1_AD_BOARD_1, GameAction.ACTION_EVENT_HIVE1_AD_OBSERVED, GameAction.ACTION_MEMENTO_JOB_FIND_1_2}), 

            new( /* COND_EXIT_HIVE1_HALL_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_1}), 

            new( /* COND_EXIT_HIVE1_HALL_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_2}), 

            new( /* COND_EXIT_HIVE1_HALL_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_3}), 

            new( /* COND_USE_HIVE1_BASIN_NO_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_BASIN_NO_SOAP}), 

            new( /* COND_USE_HIVE1_BASIN_W_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_BASIN_W_SOAP, GameAction.ACTION_EVENT_USED_BASIN}), 

            new( /* COND_USE_HIVE1_BASIN_W_SOAP_REPEAT */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_BASIN_W_SOAP_REPEAT}), 

            new( /* COND_TAKE_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_OBTAIN_SOAP, GameAction.ACTION_EVENT_SOAP_PICKABLE_TAKEN}), 

            new( /* COND_USE_HIVE1_PERFUME */
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true), new(GameEvent.EVENT_HIVE1_USED_BASIN, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_PERFUME, GameAction.ACTION_EVENT_USED_PERFUME}), 

            new( /* COND_USE_HIVE1_PERFUME_NOT_1 */
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true), new(GameEvent.EVENT_HIVE1_USED_BASIN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_PERFUME_NOT_1}), 

            new( /* COND_USE_HIVE1_PERFUME_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_PERFUME_NOT_2}), 

            new( /* COND_USE_CARDS_REME */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_CARDS_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_CARDS_REME}), 

            new( /* COND_USE_HIVE1_BED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DECISION_SLEEP}), 

            new( /* COND_GO_STREET1_SOUTH_NEIGH */
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_EXTRAPERLO_INVITATION_PICKABLE_TAKEN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_NOT_GO_SOUTH_NEIGH}), 

            new( /* COND_TRY_TALK_PHARMACIST */
            new GameEventCombi[1]{new(GameEvent.EVENT_PHARMACY_EMPTY, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TRY_TALK_PHARMACIST}), 

            new( /* COND_TALK_DEER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_DEER}), 

            new( /* COND_TALK_MANYO_OWNER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_MANYO_OWNER}), 

            new( /* COND_TAKE_UMBRELLA_MORNING */
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, true)}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_UMBRELLA_MORNING}), 

            new( /* COND_TAKE_UMBRELLA_NIGHT */
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, true)}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_TAKE_UMBRELLA_NIGHT, GameAction.ACTION_OBTAIN_UMBRELLA, GameAction.ACTION_EVENT_UMBRELLA_PICKABLE_TAKEN}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_POOR_MAN_WC */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_POOR_MAN_WC}), 

            new( /* COND_TALK_POOR_MAN_WC */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_POOR_MAN_WC}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_ROACH */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_ROACH}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_PIPE */
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_PIPE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE_2, GameAction.ACTION_EVENT_PIPE_OBSERVATION_1}), 

            new( /* COND_TAKE_HIVE1_ROACH_HEAD */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_ROACH_HEAD, GameAction.ACTION_EVENT_COCKROACH_SCARED}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE_BOX */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE_BOX}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE_2, GameAction.ACTION_EVENT_PIPE_OBSERVATION_2}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE_BOX_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE_BOX_2}), 

            new( /* COND_TAKE_VALVE_BOX_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false)}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_VALVE_BOX_1, GameAction.ACTION_EVENT_VALVE_BOX_NEED_OPEN}), 

            new( /* COND_TAKE_VALVE_BOX_MORNING */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false)}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_VALVE_BOX_MORNING}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_SHOELACE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_SHOEALCE}), 

            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE_NOT */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE_NOT}), 

            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false)}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE_NOT_2}), 

            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false)}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE, GameAction.ACTION_OBTAIN_HIVE1_SHOELACE, GameAction.ACTION_EVENT_SHOELACE_PICKABLE_TAKEN}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE}), 

            new( /* COND_USE_SHOELACE_VALVE_BOX */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, true)}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_HIVE1_SHOELACE,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_SHOELACE_VALVE_BOX, GameAction.ACTION_EVENT_VALVE_BOX_OPENED}), 

            new( /* COND_TAKE_HIVE1_VALVE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true)}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_VALVE, GameAction.ACTION_EVENT_VALVE_ACTIVATED}), 

            new( /* COND_TAKE_HIVE1_VALVE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_VALVE_2}), 

            new( /* COND_OBSERVE_ITEM_HIVE1_MAN_WC_CURED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_MAN_WC_CURED}), 

            new( /* COND_TALK_ITEM_HIVE1_MAN_WC_CURED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_MAN_WC_CURED}), 

            new( /* COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_1, GameAction.ACTION_EVENT_OBSERVED_INVITATION_RELIEF}), 

            new( /* COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_2, GameAction.ACTION_EVENT_INVITATION_UNDERSTOOD_PHRASE}), 

            new( /* COND_OBSERVE_ITEM_PHARMACY_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_PHARMACY_INKWELL}), 

            new( /* COND_OBSERVE_ITEM_PHARMACY_INK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_PHARMACY_INK}), 

            new( /* COND_TAKE_INKWELL_NOT_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_1}), 

            new( /* COND_TAKE_INKWELL_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_2, GameAction.ACTION_EVENT_INKWELL_NOT_TOUCH_WARN}), 

            new( /* COND_TAKE_INKWELL_NOT_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_3, GameAction.ACTION_EVENT_INKWELL_NOT_TOUCH_WARN}), 

            new( /* COND_USE_UMBRELLA_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_NOT_TOUCH_WARN, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_CITY1_UMBRELLA,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_UMBRELLA_INKWELL}), 

            new( /* COND_USE_INVITATION_INK */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_INVITATION_INK, GameAction.ACTION_EVENT_INVITATION_REVEALED}), 

            new( /* COND_USE_INVITATION_INK_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_INVITATION_INK_2}), 

            new( /* COND_TALK_FIK */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_FIK}), 

            new( /* COND_TRY_CROSS_EXTRAPERLO_DOOR */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_TALK_FIK_NOT_CROSS}), 

            new( /* COND_TALK_WAITER_NO_INVITATION */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_NEW_INVITATION, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_WAITER_NO_INVITATION}), 

            new( /* COND_USE_OLD_INVITATION_W_WAITER */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_NEW_INVITATION, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_TALK_WAITER_OLD_INVITATION}), 

            new( /* COND_TALK_UNKNOWN_WOMEN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_UNKNOWN_WOMEN}), 

            new( /* COND_TAKE_UNKNOWN_WOMEN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_TAKE_UNKNOWN_WOMEN}), 

            new( /* COND_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO}), 

            new( /* COND_TALK_ITEM_NPC_ARTURO_EXTRAPERLO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_ARTURO_EXTRAPERLO}), 

            new( /* COND_TALK_ITEM_NPC_CLOWN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_CLOWN_EXTRAPERLO}), 

            new( /* COND_TALK_ITEM_NPC_SILVANA_EXTRAPERLO */
            new GameEventCombi[2]{new(GameEvent.EVENT_DRUNK_STATE, true), new(GameEvent.EVENT_OLIVE_OFFERED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[3]{GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_2, GameAction.ACTION_TALK_DIALOG_SILVANA_EXTRAPERLO}), 

            new( /* COND_OBSERVE_INVITATION_CORNER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOG_OBSERVE_INVITATION_CORNER}), 

            new( /* COND_TAKE_INVITATION_CORNER */
            new GameEventCombi[2]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_OLD_INVITATION, false), new(GameEvent.EVENT_EXTRAPERLO_INV_FOLDED_CORNERS, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[4]{GameAction.ACTION_LOSE_INVALID_EXTRAPERLO_INVITATION, GameAction.ACTION_EARN_EXTRAPERLO_INVITATION_FOLDED, GameAction.ACTION_EVENT_FOLDED_INVITATION, GameAction.ACTION_DIALOG_MANIPULATE_INVITATION_CORNER}), 

            new( /* COND_USE_NEW_INVITATION_W_WAITER */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_NEW_INVITATION, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION_FOLDED,ItemInteractionType.INTERACTION_USE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_WAITER_USE_NEW_INVITATION, GameAction.ACTION_EVENT_SHOWN_NEW_INVITATION, GameAction.ACTION_LOSE_NEW_EXTRAPERLO_INVITATION}), 

            new( /* COND_TALK_WAITER_W_INVITATION */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_NEW_INVITATION, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_WAITER_W_INVITATION}), 

            new( /* COND_OBSERVE_ITEM_OBJECT_OLIVE_BOWL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_OBJECT_OLIVE_BOWL}), 

            new( /* COND_TAKE_OLIVE_FROM_BOWL */
            new GameEventCombi[1]{new(GameEvent.EVENT_TOOK_OLIVE_FROM_BOWL, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[3]{GameAction.ACTION_ANIMATE_MAINCHAR_TAKE_OLIVE, GameAction.ACTION_EVENT_TOOK_OLIVE_FROM_BOWL, GameAction.ACTION_OBTAIN_ITEM_PICKABLE_OLIVE}), 

            new( /* COND_TAKE_OLIVE_FROM_BOWL_ALREADY */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_DONT_WANT_MORE}), 

            new( /* COND_OBSERVE_ITEM_OBJECT_BEER_FULL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_OBJECT_BEER_FULL}), 

            new( /* COND_TAKE_BEER_FIRST_ROUND */
            new GameEventCombi[2]{new(GameEvent.EVENT_BEER_JAR_READY, false), new(GameEvent.EVENT_BEER_FIRST_ROUND, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[5]{GameAction.ACTION_DESPAWN_ITEM_OBJECT_BEER_FULL, GameAction.ACTION_EVENT_REMOVE_BEER_PLACED, GameAction.ACTION_ANIMATE_MAINCHAR_DRINK_BEER, GameAction.ACTION_EVENT_DRINK_FIRST_BEER_ROUND, GameAction.ACTION_EVENT_DRUNK_STATE}), 

            new( /* COND_TAKE_BEER_SECOND_ROUND */
            new GameEventCombi[2]{new(GameEvent.EVENT_BEER_JAR_READY, false), new(GameEvent.EVENT_BEER_SECOND_ROUND, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[5]{GameAction.ACTION_DIALOGUE_SIMPLE_SECOND_BEER_COMMENT, GameAction.ACTION_DESPAWN_ITEM_OBJECT_BEER_FULL, GameAction.ACTION_EVENT_REMOVE_BEER_PLACED, GameAction.ACTION_ANIMATE_MAINCHAR_DRINK_BEER, GameAction.ACTION_EVENT_DRINK_SECOND_BEER_ROUND}), 

            new( /* COND_TAKE_BEER_THIRD_ROUND */
            new GameEventCombi[2]{new(GameEvent.EVENT_BEER_JAR_READY, false), new(GameEvent.EVENT_BEER_THIRD_ROUND, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[5]{GameAction.ACTION_DESPAWN_ITEM_OBJECT_BEER_FULL, GameAction.ACTION_EVENT_REMOVE_BEER_PLACED, GameAction.ACTION_ANIMATE_MAINCHAR_DRINK_BEER, GameAction.ACTION_EVENT_DRINK_THIRD_BEER_ROUND, GameAction.ACTION_DIALOGUE_SIMPLE_AFTER_THIRD_BEER_COMMENT}), 

            new( /* COND_TALK_CLOWN_DRUNK */
            new GameEventCombi[2]{new(GameEvent.EVENT_DRUNK_STATE, false), new(GameEvent.EVENT_BEER_THIRD_ROUND, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_CLOWN_DRUNK}), 

            new( /* COND_TALK_ARTURO_DRUNK */
            new GameEventCombi[2]{new(GameEvent.EVENT_DRUNK_STATE, false), new(GameEvent.EVENT_BEER_THIRD_ROUND, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_ARTURO_DRUNK}), 

            new( /* COND_OBSERVE_ITEM_PICKABLE_OLIVE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_PICKABLE_OLIVE}), 

            new( /* COND_TALK_SILVANA_DRUNK */
            new GameEventCombi[2]{new(GameEvent.EVENT_DRUNK_STATE, false), new(GameEvent.EVENT_OLIVE_OFFERED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[3]{GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_2, GameAction.ACTION_TALK_DIALOG_SILVANA_DRUNK}), 

            new( /* COND_TALK_SILVANA_OLIVE */
            new GameEventCombi[2]{new(GameEvent.EVENT_DRUNK_STATE, false), new(GameEvent.EVENT_OLIVE_OFFERED, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_PICKABLE_OLIVE,ItemInteractionType.INTERACTION_USE,
            new GameAction[3]{GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_CLOSING_BOOK_2, GameAction.ACTION_TALK_DIALOG_SILVANA_OLIVE}), 

            new( /* COND_OBSERVE_ITEM_INNOCENT_PLANT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_INNOCENT_PLANT}), 

            new( /* COND_OBSERVE_BLADDER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_BLADDER}), 

            new( /* COND_USE_BLADDER_WITH_PLANT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_PICKABLE_BLADDER,ItemInteractionType.INTERACTION_USE,
            new GameAction[4]{GameAction.ACTION_LOSE_FULL_BLADDER, GameAction.ACTION_ZOOM_PEE_ZONE, GameAction.ACTION_EVENT_USED_ORINE_FLOWER, GameAction.ACTION_DIALOGUE_ABOUT_PEE_PLANT}), 

            new( /* COND_TALK_ITEM_NPC_SILVANA_EXTRAPERLO_GARD_SEAT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_SILVANA_GARDEN_2}), 

            new( /* COND_USE_BED_PENDING_DREAM_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DREAM_1, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[4]{GameAction.ACTION_SET_GAME_ANIMATION_MODE, GameAction.ACTION_ANIMATE_MAINCHAR_SIT_BED, GameAction.ACTION_ZOOM_LAMP_BED, GameAction.ACTION_SCENE_CHAPTER_SHOW_1}), 

            new( /* COND_OBSERVE_ITEM_DREAM_RADIO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_DREAM_RADIO}), 

            new( /* COND_TAKE_DREAM_RADIO_ON */
            new GameEventCombi[1]{new(GameEvent.EVENT_EPHIMERAL_ON, true)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_EVENT_EPHIMERAL_ON, GameAction.ACTION_SOUND_RADIO_DREAM_1}), 

            new( /* COND_TAKE_DREAM_RADIO_OFF */
            new GameEventCombi[1]{new(GameEvent.EVENT_EPHIMERAL_ON, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_EVENT_EPHIMERAL_OFF, GameAction.ACTION_SOUND_STOP_RADIO_DREAM_1}), 

            new( /* COND_OBSERVE_ITEM_CLASSROOM_PORTRAIT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_CLASSROOM_PORTRAIT}), 

            new( /* COND_OBSERVE_ITEM_DREAM_CLOCK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_DREAM_CLOCK}), 

            new( /* COND_OBSERVE_ITEM_NPC_SULTAN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_NPC_SULTAN}), 

            new( /* COND_OBSERVE_ITEM_NPC_PILAR_DREAM_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_ITEM_NPC_PILAR_DREAM_1}), 

            new( /* COND_TALK_SULTAN_DREAM_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_SULTAN_1}), 

            new( /* COND_TALK_ITEM_NPC_PILAR_DREAM_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN_DREAM,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_DIALOG_PILAR_DREAM_1}), 

            new( /* COND_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_NONE}), 

            /* > ATG 2 END < */
        };




        private static readonly ItemInfo[] _ItemInfo = new ItemInfo[(int)GameItem.ITEM_TOTAL]
        {
            /* > ATG 3 START < */
            new ( /* ITEM_PLAYER_MAIN */
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_HIVE1_CHEST */
            NameType.NAME_ITEM_SECR_DESK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_ITEM_CHEST_OPENED, GameSprite.SPRITE_ITEM_CHEST_CLOSED}),
            GameSprite.SPRITE_ITEM_CHEST_CLOSED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OPEN_CHEST, ActionConditions.COND_CLOSE_CHEST})),

            new ( /* ITEM_CARDS_PICKABLE */
            NameType.NAME_ITEM_CARDS,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_CARDS}),
            GameSprite.SPRITE_ITEM_CARDS,true,GameSprite.SPRITE_ITEM_CARDS_PICKABLE,GamePickableItem.ITEM_PICK_CARDS_PICKABLE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TAKE_CARDS})),

            new ( /* ITEM_HIVE1_WARDROBE */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OPEN_HIVE1_WARDROBE})),

            new ( /* ITEM_HIVE1_WARDROBE_OPENED */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_WARDROBE_OPENED}),
            GameSprite.SPRITE_ITEM_WARDROBE_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_CLOSE_HIVE1_WARDROBE})),

            new ( /* ITEM_GENERIC_DOOR1 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_HIVE1_NPC_REME */
            NameType.NAME_NPC_REME,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_REME_IDLE}),
            GameSprite.SPRITE_NPC_REME_IDLE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TALK_REME_1, ActionConditions.COND_USE_CARDS_REME})),

            new ( /* ITEM_GENERIC_DOOR2 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_GENERIC_DOOR3 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_HIVE1_PERFUME */
            NameType.NAME_ITEM_PERFUME,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_USE_HIVE1_PERFUME, ActionConditions.COND_USE_HIVE1_PERFUME_NOT_1, ActionConditions.COND_USE_HIVE1_PERFUME_NOT_2})),

            new ( /* ITEM_HIVE1_AD_BOARD */
            NameType.NAME_ITEM_AD_BOARD,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_HIVE1_AD_BOARD_1})),

            new ( /* ITEM_HIVE1_EXIT_DOOR */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_EXIT_HIVE1_HALL_1, ActionConditions.COND_EXIT_HIVE1_HALL_2, ActionConditions.COND_EXIT_HIVE1_HALL_3})),

            new ( /* ITEM_HIVE1_BASIN */
            NameType.NAME_ITEM_BASIN,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_USE_HIVE1_BASIN_NO_SOAP, ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP, ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP_REPEAT})),

            new ( /* ITEM_SOAP_PICKABLE */
            NameType.NAME_ITEM_SOAP,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,true,GameSprite.SPRITE_PICKABLE_SOAP,GamePickableItem.ITEM_PICK_SOAP_PICKABLE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TAKE_SOAP})),

            new ( /* ITEM_HIVE1_BED */
            NameType.NAME_ITEM_BED,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_USE_BED_PENDING_DREAM_1, ActionConditions.COND_USE_HIVE1_BED})),

            new ( /* ITEM_STREET1_STH_DOOR */
            NameType.NAME_SOUTH_NEIGHBORHOOD,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_GO_STREET1_SOUTH_NEIGH})),

            new ( /* ITEM_STREET1_CENTER_DOOR */
            NameType.NAME_CITY_CENTER,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_STREET2_PERIPH_DOOR */
            NameType.NAME_CITY_PERIPH,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_PHARMACY_DOOR */
            NameType.NAME_PHARMACY,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_PHARMACY_NPC_QUEUE */
            NameType.NAME_QUEUE_PEOPLE,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_PHARMACY_QUEUE}),
            GameSprite.SPRITE_NPC_PHARMACY_QUEUE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_PHARMACY_NPC_OWNER */
            NameType.NAME_PHARMACIST,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_PHARMACY_OWNER}),
            GameSprite.SPRITE_NPC_PHARMACY_OWNER,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TRY_TALK_PHARMACIST})),

            new ( /* ITEM_CITY1_UMBRELLA */
            NameType.NAME_UMBRELLA,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_CITY1_MANYO_UMBRELLA}),
            GameSprite.SPRITE_CITY1_MANYO_UMBRELLA,true,GameSprite.SPRITE_PICKABLE_UMBRELLA,GamePickableItem.ITEM_PICK_CITY1_UMBRELLA,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TAKE_UMBRELLA_MORNING, ActionConditions.COND_TAKE_UMBRELLA_NIGHT})),

            new ( /* ITEM_ELMANYO_DOOR */
            NameType.NAME_ELMANYO,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_ELMANYO_OWNER */
            NameType.NAME_OWNER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_MANYO_OWNER}),
            GameSprite.SPRITE_MANYO_OWNER,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_MANYO_OWNER})),

            new ( /* ITEM_STUFFED_DEER */
            NameType.NAME_STUFFED_DEER,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_DEER})),

            new ( /* ITEM_ELMANYO_OWNER_NIGHT */
            NameType.NAME_OWNER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_MANYO_OWNER})),

            new ( /* ITEM_ELMANYO_CROWD */
            NameType.NAME_CROWD,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_HIVE1_POOR_MAN_WC */
            NameType.NAME_POOR_MAN_WC,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_POOR_MAN_STEADY}),
            GameSprite.SPRITE_HIVE1_POOR_MAN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_POOR_MAN_WC, ActionConditions.COND_TALK_POOR_MAN_WC})),

            new ( /* ITEM_HIVE1_ROACH_HEAD */
            NameType.NAME_ROACH,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_ROACH_HEAD}),
            GameSprite.SPRITE_HIVE1_ROACH_HEAD,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_ROACH, ActionConditions.COND_TAKE_HIVE1_ROACH_HEAD})),

            new ( /* ITEM_HIVE1_PIPE */
            NameType.NAME_PIPE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_PIPE, ActionConditions.COND_OBSERVE_ITEM_HIVE1_PIPE_2})),

            new ( /* ITEM_HIVE1_VALVE_BOX */
            NameType.NAME_VALVE_BOX,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(5){ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE_BOX, ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE_BOX_2, ActionConditions.COND_TAKE_VALVE_BOX_1, ActionConditions.COND_TAKE_VALVE_BOX_MORNING, ActionConditions.COND_USE_SHOELACE_VALVE_BOX})),

            new ( /* ITEM_HIVE1_BACKALLEY_PIPE */
            NameType.NAME_PIPE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE, ActionConditions.COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2})),

            new ( /* ITEM_HIVE1_SHOELACE */
            NameType.NAME_SHOELACE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_SHOELACE}),
            GameSprite.SPRITE_HIVE1_SHOELACE,true,GameSprite.SPRITE_SHOELACE_PICKABLE,GamePickableItem.ITEM_PICK_HIVE1_SHOELACE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(4){ActionConditions.COND_OBSERVE_ITEM_HIVE1_SHOELACE, ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE, ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE_NOT, ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE_NOT_2})),

            new ( /* ITEM_HIVE1_VALVE */
            NameType.NAME_VALVE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_VALVE_BOX_OPENED}),
            GameSprite.SPRITE_HIVE1_VALVE_BOX_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE, ActionConditions.COND_TAKE_HIVE1_VALVE, ActionConditions.COND_TAKE_HIVE1_VALVE_2})),

            new ( /* ITEM_HIVE1_MAN_WC_CURED */
            NameType.NAME_ARTURO,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ARTURO_STEADY}),
            GameSprite.SPRITE_ARTURO_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_MAN_WC_CURED, ActionConditions.COND_TALK_ITEM_HIVE1_MAN_WC_CURED})),

            new ( /* ITEM_EXTRAPERLO_INVITATION */
            NameType.NAME_INVITATION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,true,GameSprite.SPRITE_PICKABLE_EXTRAPERLO,GamePickableItem.ITEM_PICK_EXTRAPERLO_INVITATION,DetailType.DETAIL_EXTRAPERLO,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_EXTRAPERLO_INVITATION_DETAIL */
            NameType.NAME_ANNOTATION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_1, ActionConditions.COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_2})),

            new ( /* ITEM_PHARMACY_INKWELL */
            NameType.NAME_INKWELL,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PHARMACY_INKWELL_NORMAL}),
            GameSprite.SPRITE_PHARMACY_INKWELL_NORMAL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(5){ActionConditions.COND_OBSERVE_ITEM_PHARMACY_INKWELL, ActionConditions.COND_TAKE_INKWELL_NOT_1, ActionConditions.COND_TAKE_INKWELL_NOT_2, ActionConditions.COND_TAKE_INKWELL_NOT_3, ActionConditions.COND_USE_UMBRELLA_INKWELL})),

            new ( /* ITEM_PHARMACY_INK */
            NameType.NAME_INK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PHARMACY_INKWELL_WASTED}),
            GameSprite.SPRITE_PHARMACY_INKWELL_WASTED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_PHARMACY_INK, ActionConditions.COND_USE_INVITATION_INK, ActionConditions.COND_USE_INVITATION_INK_2})),

            new ( /* ITEM_NPC_FIK */
            NameType.NAME_FIK,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_FIK_STANDING}),
            GameSprite.SPRITE_FIK_STANDING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_FIK})),

            new ( /* ITEM_DOOR_EXTRAPERLO */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TRY_CROSS_EXTRAPERLO_DOOR})),

            new ( /* ITEM_FOREGROUND_EXTRP_WALL */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_FOREGROUND_EXTRAPERLO_WALL}),
            GameSprite.SPRITE_FOREGROUND_EXTRAPERLO_WALL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_HIVE1_WATER_FLOWING */
            NameType.NAME_WATER_FLOWING,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_WATER_FLOW_BACKALLEY, GameSprite.SPRITE_WATER_FLOW_BACKALLEY_NIGHT}),
            GameSprite.SPRITE_WATER_FLOW_BACKALLEY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_DOOR_EXTRAPERLO_REAL */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_EXTRAPERLO_DOOR_OPENED}),
            GameSprite.SPRITE_EXTRAPERLO_DOOR_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_NPC_GERMAN */
            NameType.NAME_GERMAN,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_GERMAN_WAITING}),
            GameSprite.SPRITE_GERMAN_WAITING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_NPC_WAITER */
            NameType.NAME_WAITER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_WAITER_STEADY}),
            GameSprite.SPRITE_WAITER_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(4){ActionConditions.COND_TALK_WAITER_NO_INVITATION, ActionConditions.COND_USE_OLD_INVITATION_W_WAITER, ActionConditions.COND_USE_NEW_INVITATION_W_WAITER, ActionConditions.COND_TALK_WAITER_W_INVITATION})),

            new ( /* ITEM_NPC_UNKNOWN_WOMEN */
            NameType.NAME_UNKNOWN_GIRLS,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_UNKNOWN_WOMEN_STEADY}),
            GameSprite.SPRITE_UNKNOWN_WOMEN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TALK_UNKNOWN_WOMEN, ActionConditions.COND_TAKE_UNKNOWN_WOMEN})),

            new ( /* ITEM_NPC_ARTURO_EXTRAPERLO */
            NameType.NAME_ARTURO,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ARTURO_STEADY}),
            GameSprite.SPRITE_ARTURO_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO, ActionConditions.COND_TALK_ARTURO_DRUNK, ActionConditions.COND_TALK_ITEM_NPC_ARTURO_EXTRAPERLO})),

            new ( /* ITEM_NPC_CLOWN */
            NameType.NAME_CLOWN,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_CLOWN_STEADY}),
            GameSprite.SPRITE_CLOWN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TALK_CLOWN_DRUNK, ActionConditions.COND_TALK_ITEM_NPC_CLOWN})),

            new ( /* ITEM_NPC_SILVANA_EXTRAPERLO */
            NameType.NAME_SILVANA,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_SILVANA_STEADY_READING}),
            GameSprite.SPRITE_SILVANA_STEADY_READING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_TALK_SILVANA_DRUNK, ActionConditions.COND_TALK_ITEM_NPC_SILVANA_EXTRAPERLO, ActionConditions.COND_TALK_SILVANA_OLIVE})),

            new ( /* ITEM_EXTRAPERLO_INVITATION_FOLDED */
            NameType.NAME_INVITATION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PICKABLE_EXTRAPERLO_FOLDED}),
            GameSprite.SPRITE_PICKABLE_EXTRAPERLO_FOLDED,true,GameSprite.SPRITE_PICKABLE_EXTRAPERLO_FOLDED,GamePickableItem.ITEM_PICK_EXTRAPERLO_INVITATION_FOLDED,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_EXTRAPERLO_INVITATION_CORNER */
            NameType.NAME_CORNER,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_INVITATION_CORNER, ActionConditions.COND_TAKE_INVITATION_CORNER})),

            new ( /* ITEM_OBJECT_OLIVE_BOWL */
            NameType.NAME_OLIVE_BOWL,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_EXTRAPERLO_OLIVE_BOWL}),
            GameSprite.SPRITE_EXTRAPERLO_OLIVE_BOWL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_OBJECT_OLIVE_BOWL, ActionConditions.COND_TAKE_OLIVE_FROM_BOWL, ActionConditions.COND_TAKE_OLIVE_FROM_BOWL_ALREADY})),

            new ( /* ITEM_OBJECT_BEER_FULL */
            NameType.NAME_BEER_FULL,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BEER_FULL}),
            GameSprite.SPRITE_BEER_FULL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(4){ActionConditions.COND_OBSERVE_ITEM_OBJECT_BEER_FULL, ActionConditions.COND_TAKE_BEER_FIRST_ROUND, ActionConditions.COND_TAKE_BEER_SECOND_ROUND, ActionConditions.COND_TAKE_BEER_THIRD_ROUND})),

            new ( /* ITEM_PICKABLE_OLIVE */
            NameType.NAME_OLIVE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PICKABLE_OLIVE}),
            GameSprite.SPRITE_PICKABLE_OLIVE,true,GameSprite.SPRITE_PICKABLE_OLIVE,GamePickableItem.ITEM_PICK_PICKABLE_OLIVE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_ITEM_PICKABLE_OLIVE})),

            new ( /* ITEM_INNOCENT_PLANT */
            NameType.NAME_INNOCENT_PLANT,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_INNOCENT_PLANT, ActionConditions.COND_USE_BLADDER_WITH_PLANT})),

            new ( /* ITEM_PICKABLE_BLADDER */
            NameType.NAME_FULL_BLADDER,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PICKABLE_BLADDER}),
            GameSprite.SPRITE_PICKABLE_BLADDER,true,GameSprite.SPRITE_PICKABLE_BLADDER,GamePickableItem.ITEM_PICK_PICKABLE_BLADDER,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_BLADDER})),

            new ( /* ITEM_NPC_SILVANA_EXTRAPERLO_GARD */
            NameType.NAME_SILVANA,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_SILVANA_STEADY_STANDING}),
            GameSprite.SPRITE_SILVANA_STEADY_STANDING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_NPC_SILVANA_EXTRAPERLO_GARD_SEAT */
            NameType.NAME_SILVANA,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_SILVANA_SEATED_GARDEN_STEADY}),
            GameSprite.SPRITE_SILVANA_SEATED_GARDEN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OK, ActionConditions.COND_TALK_ITEM_NPC_SILVANA_EXTRAPERLO_GARD_SEAT})),

            new ( /* ITEM_PAMFRY */
            NameType.NAME_CLIENT,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PAMFRY_SHOUTING}),
            GameSprite.SPRITE_PAMFRY_SHOUTING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_FIK_EXTRAPERLO_GARDEN */
            NameType.NAME_FIK,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_FIK_STANDING}),
            GameSprite.SPRITE_FIK_STANDING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_MAINCHAR_DREAM */
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            new ( /* ITEM_DREAM_RADIO */
            NameType.NAME_RADIO,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_DREAM_RADIO, ActionConditions.COND_TAKE_DREAM_RADIO_ON, ActionConditions.COND_TAKE_DREAM_RADIO_OFF})),

            new ( /* ITEM_CLASSROOM_PORTRAIT */
            NameType.NAME_PORTRAIT,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_ITEM_CLASSROOM_PORTRAIT})),

            new ( /* ITEM_DREAM_CLOCK */
            NameType.NAME_CLOCK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_ITEM_DREAM_CLOCK})),

            new ( /* ITEM_NPC_SULTAN */
            NameType.NAME_SULTAN,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_SULTAN_STEADY}),
            GameSprite.SPRITE_SULTAN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_NPC_SULTAN, ActionConditions.COND_TALK_SULTAN_DREAM_1})),

            new ( /* ITEM_NPC_PILAR_DREAM_1 */
            NameType.NAME_PILAR,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PILAR_STEADY}),
            GameSprite.SPRITE_PILAR_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_NPC_PILAR_DREAM_1, ActionConditions.COND_TALK_ITEM_NPC_PILAR_DREAM_1})),

            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST}),
            GameSprite.SPRITE_LAST,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK})),

            /* > ATG 3 END < */
        };

        private static readonly GameItem[] _PickableToItem = new GameItem[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 4 START < */
            GameItem.ITEM_CARDS_PICKABLE,	/* ITEM_PICK_CARDS_PICKABLE */
            GameItem.ITEM_SOAP_PICKABLE,	/* ITEM_PICK_SOAP_PICKABLE */
            GameItem.ITEM_CITY1_UMBRELLA,	/* ITEM_PICK_CITY1_UMBRELLA */
            GameItem.ITEM_HIVE1_SHOELACE,	/* ITEM_PICK_HIVE1_SHOELACE */
            GameItem.ITEM_EXTRAPERLO_INVITATION,	/* ITEM_PICK_EXTRAPERLO_INVITATION */
            GameItem.ITEM_EXTRAPERLO_INVITATION_FOLDED,	/* ITEM_PICK_EXTRAPERLO_INVITATION_FOLDED */
            GameItem.ITEM_PICKABLE_OLIVE,	/* ITEM_PICK_PICKABLE_OLIVE */
            GameItem.ITEM_PICKABLE_BLADDER,	/* ITEM_PICK_PICKABLE_BLADDER */
            /* > ATG 4 END < */
        };

        private static readonly GameSprite[] _PickableSprite = new GameSprite[(int)GamePickableItem.ITEM_PICK_TOTAL]
        {
            /* > ATG 5 START < */
            GameSprite.SPRITE_ITEM_CARDS_PICKABLE,	/* ITEM_PICK_CARDS_PICKABLE */
            GameSprite.SPRITE_PICKABLE_SOAP,	/* ITEM_PICK_SOAP_PICKABLE */
            GameSprite.SPRITE_PICKABLE_UMBRELLA,	/* ITEM_PICK_CITY1_UMBRELLA */
            GameSprite.SPRITE_SHOELACE_PICKABLE,	/* ITEM_PICK_HIVE1_SHOELACE */
            GameSprite.SPRITE_PICKABLE_EXTRAPERLO,	/* ITEM_PICK_EXTRAPERLO_INVITATION */
            GameSprite.SPRITE_PICKABLE_EXTRAPERLO_FOLDED,	/* ITEM_PICK_EXTRAPERLO_INVITATION_FOLDED */
            GameSprite.SPRITE_PICKABLE_OLIVE,	/* ITEM_PICK_PICKABLE_OLIVE */
            GameSprite.SPRITE_PICKABLE_BLADDER,	/* ITEM_PICK_PICKABLE_BLADDER */
            /* > ATG 5 END < */
        };

        private static readonly MementoParentInfo[] _MementoParentInfo = new MementoParentInfo[(int)MementoParent.MEMENTO_PARENT_TOTAL]
        {
            /* > ATG 6 START < */
            /* MEMENTO_PARENT_JOB_FIND_1 */
            new(
            NameType.NAME_MEMENTO_FIND_JOB_1,GameSprite.SPRITE_MEMENTO_JOB,
            new Memento[2]{Memento.MEMENTO_JOB_FIND_1_1, Memento.MEMENTO_JOB_FIND_1_2}
            ),

            /* MEMENTO_PARENT_RECIPE_MISSION */
            new(
            NameType.NAME_MEMENTO_RECIPE_MISSION,GameSprite.SPRITE_MEMENTO_RECIPE,
            new Memento[1]{Memento.MEMENTO_RECIPE_MISSION_1}
            ),

            /* MEMENTO_PARENT_POOR_MAN_WC */
            new(
            NameType.NAME_MEMENTO_PARENT_POOR_MAN_WC,GameSprite.SPRITE_MEMENTO_POOR_MAN_WC,
            new Memento[3]{Memento.MEMENTO_POOR_MAN_WC_1, Memento.MEMENTO_POOR_MAN_WC_2, Memento.MEMENTO_POOR_MAN_WC_3}
            ),

            /* MEMENTO_PARENT_LAST */
            new(
            NameType.NAME_NPC_LAST,GameSprite.SPRITE_NONE,
            new Memento[1]{Memento.MEMENTO_LAST}
            ),

            /* > ATG 6 END < */
        };

        private static readonly MementoInfo[] _MementoInfo = new MementoInfo[(int)Memento.MEMENTO_TOTAL]
        {
            /* > ATG 7 START < */
            /* MEMENTO_JOB_FIND_1_1 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_MEMENTO_FIND_JOB_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* MEMENTO_JOB_FIND_1_2 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* MEMENTO_RECIPE_MISSION_1 */
            new(MementoParent.MEMENTO_PARENT_RECIPE_MISSION,DialogPhrase.PHRASE_MEMENTO_RECIPE_MISSION_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* MEMENTO_POOR_MAN_WC_1 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* MEMENTO_POOR_MAN_WC_2 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_2,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* MEMENTO_POOR_MAN_WC_3 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_3,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),true),
            /* MEMENTO_LAST */
            new(MementoParent.MEMENTO_PARENT_LAST,DialogPhrase.PHRASE_NONE,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE}),false),
            /* > ATG 7 END < */
        };

        private static readonly MementoCombiInfo[] _MementoCombiInfo = new MementoCombiInfo[(int)MementoCombi.MEMENTO_COMBI_TOTAL]
        {
            /* > ATG 8 START */
            /* > ATG 8 END */
        };

        private static readonly DetailInfo[] _DetailInfo = new DetailInfo[(int)DetailType.DETAIL_TOTAL]
        {
            /* > ATG 9 START < */
            new(PrefabEnum.PREFAB_DETAIL_EXTRAPERLO, /* DETAIL_EXTRAPERLO */ 
            new(new HashSet<NameType>(1){NameType.NAME_NONE}),
            new(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONE})),

            new(PrefabEnum.PREFAB_NONE, /* DETAIL_LAST */ 
            new(new HashSet<NameType>(1){NameType.NAME_NONE}),
            new(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONE})),

            /* > ATG 9 END < */
        };

        private static readonly ActionInfo[] _ActionInfo = new ActionInfo[(int)GameAction.ACTION_TOTAL]
        {
            /* > ATG 10 START < */
            new( /* ACTION_NONE */
            false,ActionType.ACTION_TYPE_NONE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DAY_MOMENT_DAY */
            false,ActionType.ACTION_TYPE_CHANGE_MOMENT_DAY,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_MORNING,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DAY_MOMENT_NIGHT */
            false,ActionType.ACTION_TYPE_CHANGE_MOMENT_DAY,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_NIGHT,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INITIAL_MEMENTO */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_FIRST, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MEMENTO_INITIAL_MEMENTO */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SET_SPRITE_CHEST_OPENED */
            false,ActionType.ACTION_TYPE_SET_SPRITE,GameItem.ITEM_HIVE1_CHEST,GameSprite.SPRITE_ITEM_CHEST_OPENED,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_PERFUME */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_PERFUME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SET_SPRITE_CHEST_CLOSED */
            false,ActionType.ACTION_TYPE_SET_SPRITE,GameItem.ITEM_HIVE1_CHEST,GameSprite.SPRITE_ITEM_CHEST_CLOSED,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_HIVE1_PERFUME */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_PERFUME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESTROY_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_WARDROBE_OPENED */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_WARDROBE_OPENED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_HIVE1_WARDROBE */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_WARDROBE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_SOAP */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_WARDROBE */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_WARDROBE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_HIVE1_WARDROBE_OPENED */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_WARDROBE_OPENED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_SOAP */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MEMENTO_JOB_FIND_1_2 */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_2,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESTROY_SOAP */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_SOAP */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DECISION_SLEEP */
            false,ActionType.ACTION_TYPE_DECISION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_SLEEP_1,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_REME_DAY */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_NPC_REME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_PHARMACY_DOOR */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_PHARMACY_DOOR,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_MANYO_OWNER_DAY */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_ELMANYO_OWNER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_MANYO_OWNER_NIGHT */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_ELMANYO_OWNER_NIGHT,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MEMENTO_RECEIPT_MISSION */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_RECIPE_MISSION_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESTROY_UMBRELLA */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_CITY1_UMBRELLA,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_UMBRELLA */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_CITY1_UMBRELLA,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_POOR_MAN_WC,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MEMENTO_POOR_MAN_WC_1 */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_POOR_MAN_WC_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_ROACH_HEAD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_ROACH_HEAD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_REBUILD_ROACH_SCARED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESTROY_HIVE1_SHOELACE */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_HIVE1_SHOELACE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_HIVE1_SHOELACE */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_HIVE1_SHOELACE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESTROY_HIVE1_VALVE_BOX */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_HIVE1_VALVE_BOX,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_VALVE */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_VALVE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_HIVE1_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_MAN_WC_CURED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_EXTRAPERLO_INVITATION */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_EXTRAPERLO_INVITATION,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_INKWELL */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_PHARMACY_INKWELL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_INK */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_PHARMACY_INK,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_REME_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_REME_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_HIVE1_AD_BOARD_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_1,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_3 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_BASIN_NO_SOAP */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_NO_SOAP,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_BASIN_W_SOAP */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_SOAP,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_BASIN_W_SOAP_REPEAT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_ALREADY_COMBI,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_PERFUME */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_PERFUME_NOT_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME_NOT_1,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_PERFUME_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME_NOT_2,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_CARDS_REME */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_REME_CARDS,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_NOT_GO_SOUTH_NEIGH */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_WONT_GO_SOUTH_NEIGH,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TRY_TALK_PHARMACIST */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_TRY_TALK_PHARMACIST_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TALK_DEER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_HELLO_DEER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TALK_MANYO_OWNER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_OWNER_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_UMBRELLA_MORNING */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_UMBRELLA,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_UMBRELLA_NIGHT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_DIALOG_UMBRELLA_TAKEN,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_MANYO_BCKG */
            false,ActionType.ACTION_TYPE_START_DIALOGUE_BCKG,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_BCKG_CROWD,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_POOR_MAN_WC,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TALK_POOR_MAN_WC_BCKG */
            false,ActionType.ACTION_TYPE_START_DIALOGUE_BCKG,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_HIVE1_BCKG_POOR_MAN_WC,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TALK_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_HIVE1_POOR_MAN_WC_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_ROACH */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_ROACH,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_PIPE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_ITEM_HIVE1_ROACH_HEAD,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_VALVE_BOX */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_VALVE_BOX_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX_2,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_VALVE_BOX_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_VALVE_BOX_CLOSED,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_VALVE_BOX_MORNING */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_VALVE_BOX_CLOSED_MORNING,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_SHOEALCE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_SHOELACE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE_NOT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_ITEM_HIVE1_SHOELACE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_VALVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_SHOELACE_VALVE_BOX */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_SHOELACE_VALVE_BOX,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_HIVE1_VALVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_VALVE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_HIVE1_VALVE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_VALVE_NOT,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_MAN_WC_CURED,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TALK_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_ARTURO_HALL_INN_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_BLURR,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_PHARMACY_INKWELL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_PHARMACY_INKWELL,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_PHARMACY_INK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_PHARMACY_INK,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PHARMACIST_NOT_TAKE_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_3 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PHARMACIST_NOT_TAKE_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_UMBRELLA_INKWELL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_USE_UMBRELLA_WITH_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_INVITATION_INK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_INVITATION_WITH_INK,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_USE_INVITATION_INK_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_INVITATION_WITH_INK_ALREADY,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_OPEN_CHEST */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_CANCEL_OPEN_CHEST */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_CARDS_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_OPEN_WARDROBE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_CANCEL_OPEN_WARDROBE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_HIVE1_AD_OBSERVED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_USED_BASIN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_SOAP_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_USED_PERFUME */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_UMBRELLA_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_PIPE_OBSERVATION_1 */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_PIPE_OBSERVATION_2 */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_COCKROACH_SCARED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_VALVE_BOX_NEED_OPEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_SHOELACE_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_HIVE1_SHOELACE_PICKABLE_TAKEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_VALVE_BOX_OPENED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_VALVE_ACTIVATED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_OBSERVED_INVITATION_RELIEF */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INVITATION_UNDERSTOOD_PHRASE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_UNDERSTOOD_PHRASE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INKWELL_NOT_TOUCH_WARN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_NOT_TOUCH_WARN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INVITATION_REVEALED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_MANYO_REFUSED_WORK */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_MANYO_LOOK_FOR_RECIPE_MISSION */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_POOR_MAN_WC_NEEDS_WATER */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_TALKED_ARTURO_HALL_PUB */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_PUB, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_TALKED_ARTURO_HALL_COMPLETED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_INN_COMPLETED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INVITATION_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_EXTRAPERLO_INVITATION_PICKABLE_TAKEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_INKWELL_WASTED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_WASTED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOG_USELESS_OBSERVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE_OBSERVE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOG_USELESS_TALK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE_TALK,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOG_USELESS_ACTION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_REME_TEST */
            true,ActionType.ACTION_TYPE_START_ANIMATION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_REME_TEST,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_FIK_1 */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_NPC_FIK,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_FIK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_FIK_1_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_EXTRAPERLO_SAID_PHRASE */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_FIK_NOT_CROSS */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_FIK_NOT_CROSS_DOOR,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_EXTRAPERLO_DOOR */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_DOOR_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_EXTRAPERLO_WALL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_FOREGROUND_EXTRP_WALL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_EXTRAPERLO_DOOR */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_DOOR_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_EXTRAPERLO_DOOR_REAL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_DOOR_EXTRAPERLO_REAL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_WATER_FLOWING_DAY */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_HIVE1_WATER_FLOWING,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_WATER_FLOWING_NIGHT */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_HIVE1_WATER_FLOWING,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_GERMAN */
            false,ActionType.ACTION_TYPE_NONE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPWAN_GERMAN_1 */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_NPC_GERMAN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_SHOWN_OLD_INVITATION */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_OLD_INVITATION, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_FOLD_CORNERS */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_INV_FOLDED_CORNERS, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_SHOWN_NEW_INVITATION */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SHOWN_NEW_INVITATION, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_WAITER_NO_INVITATION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_WAITER_1_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_WAITER_OLD_INVITATION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_WAITER_USE_OLD_INVITATION,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_UNKNOWN_WOMEN */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_UNKNOWN_GIRLS_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TAKE_UNKNOWN_WOMEN */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_UNKNOWN_WOMEN,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_ARTURO_EXTRAPERLO */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_ARTURO_EXTRAPERLO_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_START_TEST_CARD_GAME */
            false,ActionType.ACTION_TYPE_START_CARD_GAME,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_TEST), 

            new( /* ACTION_EVENT_WIN_CARDS_ARTURO */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_WON_CARDS_ARTURO, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_CLOWN_EXTRAPERLO */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_CLOWN_EXTRAPERLO_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_SILVANA_CLOSING_BOOK_1 */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_SILVANA_CLOSING_BOOK_2 */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_SILVANA_EXTRAPERLO */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_EXTRAPERLO_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_SILVANA_OPENING_BOOK_1 */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_SILVANA_OPENING_BOOK_2 */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_THREE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_SILVANA_REFUSED_MAINCHAR */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SILVANA_REFUSED_MAINCHAR, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_LOSE_INVALID_EXTRAPERLO_INVITATION */
            false,ActionType.ACTION_TYPE_LOSE_ITEM,GameItem.ITEM_EXTRAPERLO_INVITATION,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EARN_EXTRAPERLO_INVITATION_FOLDED */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_EXTRAPERLO_INVITATION_FOLDED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_FOLDED_INVITATION */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_INV_FOLDED_CORNERS, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOG_OBSERVE_INVITATION_CORNER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_INVITATION_CORNER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOG_MANIPULATE_INVITATION_CORNER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_MANIPULATE_INVITATION_CORNER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_LOSE_NEW_EXTRAPERLO_INVITATION */
            false,ActionType.ACTION_TYPE_LOSE_ITEM,GameItem.ITEM_EXTRAPERLO_INVITATION_FOLDED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_WAITER_USE_NEW_INVITATION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_WAITER_USE_NEW_INVITATION,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_WAITER_W_INVITATION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_WAITER_W_INVITATION_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_WAITER_SERVE_OLIVES */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_WAITER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_PLAY_SOUND_DISH */
            false,ActionType.ACTION_TYPE_PLAY_SOUND,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_DISH_PLACED,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_OLIVES_PLACED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_WAITER_EXTRAPERLO_GAVE_OLIVES, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_OBJECT_OLIVE_BOWL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_OBJECT_OLIVE_BOWL,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_ITEM_OBJECT_OLIVE_BOWL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_OBJECT_OLIVE_BOWL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_MAINCHAR_TAKE_OLIVE */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PLAYER_MAIN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_MAINCHAR_EAT_CHEW */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PLAYER_MAIN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_WAITER_SERVE_JAR */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_WAITER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_THREE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_BEER_PLACED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_JAR_READY, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_REMOVE_BEER_PLACED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_JAR_READY, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_OBJECT_BEER_FULL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_OBJECT_BEER_FULL,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DESPAWN_ITEM_OBJECT_BEER_FULL */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_OBJECT_BEER_FULL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_ITEM_OBJECT_BEER_FULL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_OBJECT_BEER_FULL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_MAINCHAR_DRINK_BEER */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PLAYER_MAIN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_THREE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_DRUNK_STATE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_DRUNK_STATE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_DRINK_FIRST_BEER_ROUND */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_FIRST_ROUND, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_DRINK_SECOND_BEER_ROUND */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_SECOND_ROUND, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SIMPLE_SECOND_BEER_COMMENT */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_COMMENT_SECOND_BEER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_DRINK_THIRD_BEER_ROUND */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_BEER_THIRD_ROUND, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SIMPLE_AFTER_THIRD_BEER_COMMENT */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_COMMENT_AFTER_THIRD_BEER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_CLOWN_DRUNK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_CLOWN_DRUNK,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_ARTURO_DRUNK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_ARTURO_DRUNK_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_START_DRUNK_TEST_CARD_GAME */
            false,ActionType.ACTION_TYPE_START_CARD_GAME,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_TEST_DRUNK), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_PICKABLE_OLIVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_PICKABLE_OLIVE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_ITEM_PICKABLE_OLIVE */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_PICKABLE_OLIVE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_TOOK_OLIVE_FROM_BOWL */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_TOOK_OLIVE_FROM_BOWL, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_DONT_WANT_MORE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_MAINCHAR_DONT_WANT_MORE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_SILVANA_DRUNK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_DRUNK,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_SILVANA_OLIVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_OLIVE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_OLIVE_OFFERED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_OLIVE_OFFERED, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SCENE_EXTRAPERLO_TERRACE */
            false,ActionType.ACTION_TYPE_SCENE_CHANGE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.CITY1_EXTRAPERLO3,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_DRUNK_STATE_REMOVE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_DRUNK_STATE, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_WAYPOINT_WAITER_TERRACE */
            true,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_NPC_WAITER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"WaiterOut",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SIMPLE_MAINCHAR_NEED_ORINE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_MAINCHAR_NEED_ORINE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_NEED_ORINE_SAID */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NEED_ORINE_SAID, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_INNOCENT_PLANT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_INNOCENT_PLANT,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_BLADDER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_BLADDER,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_OBTAIN_FULL_BLADDER */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_PICKABLE_BLADDER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_ABOUT_PEE_PLANT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MAINCHAR_PEE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SPAWN_SILVANA_GARDEN */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_USED_ORINE_FLOWER */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_USED_ORINE_FLOWER, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_AFTER_PEE */
            false,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_PLAYER_MAIN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"AfterPee",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_SILVANA_SHOCKED_ANGRY */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SILVANA_OBSERVE_PEE */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_OBSERVE_PEE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_PEE_ZONE */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_RIGHT",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_SILVANA_SHOCKED_ZONE */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_LEFT",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_LOSE_FULL_BLADDER */
            false,ActionType.ACTION_TYPE_LOSE_ITEM,GameItem.ITEM_PICKABLE_BLADDER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_GARDEN_OUT */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_ALL",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_SILVANA_GARDEN_TALK_APROACH */
            true,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"SilvanaTalks",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_SILVANA_GARDEN_1 */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_GARDEN_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SCENE_EXTRAPERLO_TERRACE_2 */
            false,ActionType.ACTION_TYPE_SCENE_CHANGE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.CITY1_EXTRAPERLO3_2,"",null,null,4,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_SILVANA_GARDEN_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_GARDEN_2_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_PAMFRY_1 */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PAMFRY_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_PAMFRY_APPEAR */
            true,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"PamfryRun1",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_PAMFRY_FLIP */
            true,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"PamfryRunFlip",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PAMFRY_RUNNING */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_PAMFRY_RUN */
            true,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"PamfryRun2",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PAMFRY_CLIMBING */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PAMFRY_DIE_1 */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"storeOnly",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PAMFRY_DIE_2 */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PAMFRY,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_PLAY_SOUND_GUNSHOT */
            false,ActionType.ACTION_TYPE_PLAY_SOUND,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_GUNSHOT,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_PAMFRY_DEATH */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_PAMFRY_END",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SET_GAME_ANIMATION_MODE */
            true,ActionType.ACTION_TYPE_SET_TO_ANIMATION_MODE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_REMOVE_GAME_ANIMATION_MODE */
            true,ActionType.ACTION_TYPE_REMOVE_ANIMATION_MODE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MOVE_FIK_TO_SHOT */
            false,ActionType.ACTION_TYPE_MOVE_TO_WAYPOINT,GameItem.ITEM_FIK_EXTRAPERLO_GARDEN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"PamfryRun1",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_FIK_SHOOTING */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_FIK_SHOOTING",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_FIK_SHOOTING */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_FIK_SHOOTING,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_GERMAN_SHOUTING_GARDEN */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_GERMAN_SHOUTING_GARDEN,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SILVANA_ASKING_BILL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SILVANA_ASKING_BILL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_ALL_END_GARDEN_SCENE */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_ALL",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SCENE_BACK_TO_HIVE1_ROOM */
            false,ActionType.ACTION_TYPE_SCENE_CHANGE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.HIVE1_ROOM_1,"",null,null,1,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_PENDING_RECAP_EXTRAPERLO_GARDEN */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_RECAP_EXTRAPERLO_GARDEN, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_MAINCHAR_RECAP_EXTRAPERLO */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MAINCHAR_RECAP_EXTRAPERLO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_REMOVE_PENDING_RECAP_EXTRAPERLO_GARDEN */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_RECAP_EXTRAPERLO_GARDEN, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_PENDING_DREAM_1 */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DREAM_1, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_MAINCHAR_SIT_BED */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_PLAYER_MAIN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_CYCLE_THREE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ZOOM_LAMP_BED */
            true,ActionType.ACTION_TYPE_SET_ZOOM_REGION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"ZOOM_LAMP",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SCENE_CHAPTER_SHOW_1 */
            false,ActionType.ACTION_TYPE_SCENE_CHANGE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_MORNING,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.CHAPTER_SHOW,"",null,null,1,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_DREAM_RADIO */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_DREAM_RADIO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_MAINCHAR_ENTRY_DREAM_1 */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_MAINCHAR_ENTRY_DIALOG_DREAM_1,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_REMOVE_PENDING_DREAM_1 */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DREAM_1, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SOUND_RADIO_DREAM_1 */
            false,ActionType.ACTION_TYPE_PLAY_SOUND,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_RADIO_PRE_WAR_1,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_SOUND_STOP_RADIO_DREAM_1 */
            false,ActionType.ACTION_TYPE_STOP_SOUND,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_RADIO_PRE_WAR_1,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_EPHIMERAL_ON */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EPHIMERAL_ON, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_EPHIMERAL_OFF */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EPHIMERAL_ON, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_PLAY_SOUND_CHAPTER_IN */
            false,ActionType.ACTION_TYPE_PLAY_SOUND,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_CHAPTER_IN,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_CLASSROOM_PORTRAIT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_CLASSROOM_PORTRAIT,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_DREAM_CLOCK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_DREAM_CLOCK,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_PENDING_DESCRIBE_DREAM_FRAMEWORK */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DESCRIBE_DREAM_FRAMEWORK, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_EVENT_REMOVE_DESCRIBE_DREAM_FRAMEWORK */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PENDING_DESCRIBE_DREAM_FRAMEWORK, true)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_MAINCHAR_ENTRY_DREAM_1_FRAMEWORK */
            true,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_MAINCHAR_ENTRY_DREAM_1_FRAMEWORK,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_NPC_SULTAN */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_NPC_SULTAN,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_OBSERVE_ITEM_NPC_PILAR_DREAM_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_NPC_PILAR_DREAM_1,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_DIALOGUE_SULTAN_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SULTAN_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_TALK_DIALOG_PILAR_DREAM_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PILAR_DREAM_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MAINCHAR_DREAM_INVISIBLE */
            false,ActionType.ACTION_TYPE_INVISIBLE,GameItem.ITEM_MAINCHAR_DREAM,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,1,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_MAINCHAR_DREAM_VISIBLE */
            false,ActionType.ACTION_TYPE_VISIBLE,GameItem.ITEM_MAINCHAR_DREAM,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PILAR_STEADY */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_PILAR_DREAM_1,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_ANIMATE_PILAR_KISSING */
            true,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_NPC_PILAR_DREAM_1,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",false,null,0,0,CardGameID.CARD_GAME_NONE), 

            new( /* ACTION_LAST */
            false,ActionType.ACTION_TYPE_NONE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,GameAnimation.ANIMATION_NONE,GameSound.SOUND_NONE,Room.ROOM_NONE,"",null,null,0,0,CardGameID.CARD_GAME_NONE), 

            /* > ATG 10 END < */
        };
    }
}
