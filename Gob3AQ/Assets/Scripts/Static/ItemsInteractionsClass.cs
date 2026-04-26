using UnityEngine;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;

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
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_NONE,}), 
            
            new( /* UNCHAIN_ROOM1_INITIAL_MEMENTO_1 */
            false,false,true,new(GameEvent.EVENT_FIRST, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_EVENT_INITIAL_MEMENTO,GameAction.ACTION_MEMENTO_INITIAL_MEMENTO,}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_CHEST_1 */
            false,true,false,new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_CARDS,}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_CHEST_2 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SET_SPRITE_CHEST_OPENED,GameAction.ACTION_SPAWN_HIVE1_PERFUME,}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_CHEST_1 */
            false,true,false,new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESPAWN_HIVE1_CARDS,}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_CHEST_2 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SET_SPRITE_CHEST_CLOSED,GameAction.ACTION_DESPAWN_HIVE1_PERFUME,}), 
            
            new( /* UNCHAIN_CARDS_PICKABLE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_HIVE1_CARDS,}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_1 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SPAWN_HIVE1_WARDROBE_OPENED,GameAction.ACTION_DESPAWN_HIVE1_WARDROBE,}), 
            
            new( /* UNCHAIN_HIVE1_OPEN_WARDROBE_2 */
            false,true,false,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_SPAWN_SOAP,}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_1 */
            false,true,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_SPAWN_HIVE1_WARDROBE,GameAction.ACTION_DESPAWN_HIVE1_WARDROBE_OPENED,}), 
            
            new( /* UNCHAIN_HIVE1_CLOSE_WARDROBE_2 */
            false,true,false,new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESPAWN_SOAP,}), 
            
            new( /* UNCHAIN_SOAP_PICKABLE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_SOAP,}), 
            
            new( /* UNCHAIN_REME_DAY */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_REME_DAY,}), 
            
            new( /* UNCHAIN_PHARMACY_DOOR_AVAIL */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_PHARMACY_DOOR,}), 
            
            new( /* UNCHAIN_MANYO_OWNER */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_MANYO_OWNER_DAY,}), 
            
            new( /* UNCHAIN_MANYO_OWNER_NIGHT */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_MANYO_OWNER_NIGHT,}), 
            
            new( /* UNCHAIN_UMBRELLA_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_UMBRELLA,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_POOR_MAN_WC_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false),new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_POOR_MAN_WC,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_ROACH_HEAD_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_CHEATED, true),}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_ROACH_HEAD,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_ROACH_HEAD_DESPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, false),}, 
            MomentType.MOMENT_MORNING, 
            new GameAction[1]{GameAction.ACTION_DESPAWN_HIVE1_ROACH_HEAD,}), 
            
            new( /* UNCHAIN_REBUILD_COCHROACH_SCARED */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_COCKROACH_SCARED, false),new(GameEvent.EVENT_COCKROACH_CHEATED, true),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_EVENT_REBUILD_ROACH_SCARED,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_SHOELACE_TAKE_1 */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_HIVE1_SHOELACE_PICKABLE_TAKEN, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_DESTROY_HIVE1_SHOELACE,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_VALVE_BOX_DESPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_DESTROY_HIVE1_VALVE_BOX,GameAction.ACTION_SPAWN_HIVE1_VALVE,}), 
            
            new( /* UNCHAIN_ITEM_HIVE1_MAN_WC_CURED_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false),new(GameEvent.EVENT_TALKED_ARTURO_HALL_INN_COMPLETED, true),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_HIVE1_MAN_WC_CURED,}), 
            
            new( /* UNCHAIN_INK_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_WASTED, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[2]{GameAction.ACTION_DESPAWN_INKWELL,GameAction.ACTION_SPAWN_INK,}), 
            
            new( /* UNCHAIN_FIK_SPAWN */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[2]{GameAction.ACTION_SPAWN_FIK_1,GameAction.ACTION_SPWAN_GERMAN_1,}), 
            
            new( /* UNCHAIN_MANYO_BCKG_DIALOGUE */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_DIALOGUE_MANYO_BCKG,}), 
            
            new( /* UNCHAIN_POOR_MAN_WC_BCKG_DIALOGUE */
            true,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[2]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false),new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_POOR_MAN_WC_BCKG,}), 
            
            new( /* UNCHAIN_EXTRAPERLO_DOOR */
            false,false,false,new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_SPAWN_EXTRAPERLO_DOOR,}), 
            
            new( /* UNCHAIN_SHOW_EXTRAPERLO_DOOR_OPENED */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[3]{GameAction.ACTION_SPAWN_EXTRAPERLO_WALL,GameAction.ACTION_DESPAWN_EXTRAPERLO_DOOR,GameAction.ACTION_SPAWN_EXTRAPERLO_DOOR_REAL,}), 
            
            new( /* UNCHAIN_SET_SPRITE_WATER_FLOWING_NIGHT */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_NIGHT, 
            new GameAction[1]{GameAction.ACTION_ANIMATE_WATER_FLOWING_NIGHT,}), 
            
            new( /* UNCHAIN_LAST */
            false,false,false,new(GameEvent.EVENT_NONE, false), 
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY, 
            new GameAction[1]{GameAction.ACTION_NONE,}), 
            
            /* > ATG 1 END < */
        };



        private static readonly ActionConditionsInfo[] _ActionConditions = new ActionConditionsInfo[(int)ActionConditions.COND_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* COND_OK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_NONE,}), 
            
            new( /* COND_OPEN_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_OPEN_CHEST,}), 
            
            new( /* COND_CLOSE_CHEST */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_CANCEL_OPEN_CHEST,}), 
            
            new( /* COND_TAKE_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_OBTAIN_HIVE1_CARDS,GameAction.ACTION_EVENT_CARDS_PICKABLE_TAKEN,}), 
            
            new( /* COND_OPEN_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_OPEN_WARDROBE,}), 
            
            new( /* COND_CLOSE_HIVE1_WARDROBE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_EVENT_CANCEL_OPEN_WARDROBE,}), 
            
            new( /* COND_TALK_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_REME_1,}), 
            
            new( /* COND_OBSERVE_HIVE1_AD_BOARD_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_HIVE1_AD_BOARD_1,GameAction.ACTION_EVENT_HIVE1_AD_OBSERVED,GameAction.ACTION_MEMENTO_JOB_FIND_1_2,}), 
            
            new( /* COND_EXIT_HIVE1_HALL_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_1,}), 
            
            new( /* COND_EXIT_HIVE1_HALL_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_2,}), 
            
            new( /* COND_EXIT_HIVE1_HALL_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_EXIT_HIVE1_HALL_3,}), 
            
            new( /* COND_USE_HIVE1_BASIN_NO_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_BASIN_NO_SOAP,}), 
            
            new( /* COND_USE_HIVE1_BASIN_W_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_BASIN_W_SOAP,GameAction.ACTION_EVENT_USED_BASIN,}), 
            
            new( /* COND_USE_HIVE1_BASIN_W_SOAP_REPEAT */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_SOAP_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_BASIN_W_SOAP_REPEAT,}), 
            
            new( /* COND_TAKE_SOAP */
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_OBTAIN_SOAP,GameAction.ACTION_EVENT_SOAP_PICKABLE_TAKEN,}), 
            
            new( /* COND_USE_HIVE1_PERFUME */
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true),new(GameEvent.EVENT_HIVE1_USED_BASIN, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_PERFUME,GameAction.ACTION_EVENT_USED_PERFUME,}), 
            
            new( /* COND_USE_HIVE1_PERFUME_NOT_1 */
            new GameEventCombi[2]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, true),new(GameEvent.EVENT_HIVE1_USED_BASIN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_PERFUME_NOT_1,}), 
            
            new( /* COND_USE_HIVE1_PERFUME_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_PERFUME_NOT_2,}), 
            
            new( /* COND_USE_CARDS_REME */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_CARDS_PICKABLE,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_CARDS_REME,}), 
            
            new( /* COND_USE_HIVE1_BED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DECISION_SLEEP,}), 
            
            new( /* COND_GO_STREET1_SOUTH_NEIGH */
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_EXTRAPERLO_INVITATION_PICKABLE_TAKEN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_NOT_GO_SOUTH_NEIGH,}), 
            
            new( /* COND_TRY_TALK_PHARMACIST */
            new GameEventCombi[1]{new(GameEvent.EVENT_PHARMACY_EMPTY, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TRY_TALK_PHARMACIST,}), 
            
            new( /* COND_TALK_DEER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_DEER,}), 
            
            new( /* COND_TALK_MANYO_OWNER */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_MANYO_OWNER,}), 
            
            new( /* COND_TAKE_UMBRELLA_MORNING */
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, true),}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_UMBRELLA_MORNING,}), 
            
            new( /* COND_TAKE_UMBRELLA_NIGHT */
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, true),}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_TAKE_UMBRELLA_NIGHT,GameAction.ACTION_OBTAIN_UMBRELLA,GameAction.ACTION_EVENT_UMBRELLA_PICKABLE_TAKEN,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_POOR_MAN_WC */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_POOR_MAN_WC,}), 
            
            new( /* COND_TALK_POOR_MAN_WC */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_POOR_MAN_WC,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_ROACH */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_ROACH,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_PIPE */
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_PIPE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE_2,GameAction.ACTION_EVENT_PIPE_OBSERVATION_1,}), 
            
            new( /* COND_TAKE_HIVE1_ROACH_HEAD */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_ROACH_HEAD,GameAction.ACTION_EVENT_COCKROACH_SCARED,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE_BOX */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE_BOX,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE_2,GameAction.ACTION_EVENT_PIPE_OBSERVATION_2,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE_BOX_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE_BOX_2,}), 
            
            new( /* COND_TAKE_VALVE_BOX_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false),}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_VALVE_BOX_1,GameAction.ACTION_EVENT_VALVE_BOX_NEED_OPEN,}), 
            
            new( /* COND_TAKE_VALVE_BOX_MORNING */
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false),}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_VALVE_BOX_MORNING,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_SHOELACE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_SHOEALCE,}), 
            
            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE_NOT */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE_NOT,}), 
            
            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false),}, 
            MomentType.MOMENT_MORNING,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE_NOT_2,}), 
            
            new( /* COND_TAKE_ITEM_HIVE1_SHOELACE */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false),}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[3]{GameAction.ACTION_DIALOGUE_TAKE_SHOEALCE,GameAction.ACTION_OBTAIN_HIVE1_SHOELACE,GameAction.ACTION_EVENT_SHOELACE_PICKABLE_TAKEN,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_VALVE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_VALVE,}), 
            
            new( /* COND_USE_SHOELACE_VALVE_BOX */
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, true),}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_HIVE1_SHOELACE,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_SHOELACE_VALVE_BOX,GameAction.ACTION_EVENT_VALVE_BOX_OPENED,}), 
            
            new( /* COND_TAKE_HIVE1_VALVE */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, true),}, 
            MomentType.MOMENT_NIGHT,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_VALVE,GameAction.ACTION_EVENT_VALVE_ACTIVATED,}), 
            
            new( /* COND_TAKE_HIVE1_VALVE_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_HIVE1_VALVE_2,}), 
            
            new( /* COND_OBSERVE_ITEM_HIVE1_MAN_WC_CURED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_MAN_WC_CURED,}), 
            
            new( /* COND_TALK_ITEM_HIVE1_MAN_WC_CURED */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TALK_MAN_WC_CURED,}), 
            
            new( /* COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_1,GameAction.ACTION_EVENT_OBSERVED_INVITATION_RELIEF,}), 
            
            new( /* COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_2,GameAction.ACTION_EVENT_INVITATION_UNDERSTOOD_PHRASE,}), 
            
            new( /* COND_OBSERVE_ITEM_PHARMACY_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_PHARMACY_INKWELL,}), 
            
            new( /* COND_OBSERVE_ITEM_PHARMACY_INK */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_OBSERVE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_OBSERVE_PHARMACY_INK,}), 
            
            new( /* COND_TAKE_INKWELL_NOT_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_1,}), 
            
            new( /* COND_TAKE_INKWELL_NOT_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TAKE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_2,GameAction.ACTION_EVENT_INKWELL_NOT_TOUCH_WARN,}), 
            
            new( /* COND_TAKE_INKWELL_NOT_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_TAKE_INKWELL_NOT_3,GameAction.ACTION_EVENT_INKWELL_NOT_TOUCH_WARN,}), 
            
            new( /* COND_USE_UMBRELLA_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_NOT_TOUCH_WARN, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_CITY1_UMBRELLA,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_UMBRELLA_INKWELL,}), 
            
            new( /* COND_USE_INVITATION_INK */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[2]{GameAction.ACTION_DIALOGUE_USE_INVITATION_INK,GameAction.ACTION_EVENT_INVITATION_REVEALED,}), 
            
            new( /* COND_USE_INVITATION_INK_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_EXTRAPERLO_INVITATION,ItemInteractionType.INTERACTION_USE,
            new GameAction[1]{GameAction.ACTION_DIALOGUE_USE_INVITATION_INK_2,}), 
            
            new( /* COND_TALK_FIK */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_FIK,}), 
            
            new( /* COND_TRY_CROSS_EXTRAPERLO_DOOR */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, true),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_CROSS_DOOR,
            new GameAction[1]{GameAction.ACTION_TALK_FIK_NOT_CROSS,}), 
            
            new( /* COND_TALK_GERMAN */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_MAIN,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_TALK,
            new GameAction[1]{GameAction.ACTION_TALK_GERMAN,}), 
            
            new( /* COND_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            MomentType.MOMENT_ANY,CharacterType.CHARACTER_NONE,GameItem.ITEM_NONE,ItemInteractionType.INTERACTION_NONE,
            new GameAction[1]{GameAction.ACTION_NONE,}), 
            
            /* > ATG 2 END < */
        };




        private static readonly ItemInfo[] _ItemInfo = new ItemInfo[(int)GameItem.ITEM_TOTAL]
        {
            /* > ATG 3 START < */
            new ( /* ITEM_PLAYER_MAIN */
            NameType.NAME_CHAR_MAIN,GameItemFamily.ITEM_FAMILY_TYPE_PLAYER,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NONE,}),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_HIVE1_CHEST */
            NameType.NAME_ITEM_SECR_DESK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_ITEM_CHEST_OPENED,GameSprite.SPRITE_ITEM_CHEST_CLOSED,}),
            GameSprite.SPRITE_ITEM_CHEST_CLOSED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OPEN_CHEST,ActionConditions.COND_CLOSE_CHEST,})),
            
            new ( /* ITEM_CARDS_PICKABLE */
            NameType.NAME_ITEM_CARDS,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_CARDS,}),
            GameSprite.SPRITE_ITEM_CARDS,true,GameSprite.SPRITE_ITEM_CARDS_PICKABLE,GamePickableItem.ITEM_PICK_CARDS_PICKABLE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TAKE_CARDS,})),
            
            new ( /* ITEM_HIVE1_WARDROBE */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OPEN_HIVE1_WARDROBE,})),
            
            new ( /* ITEM_HIVE1_WARDROBE_OPENED */
            NameType.NAME_ITEM_WARDROBE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ITEM_WARDROBE_OPENED,}),
            GameSprite.SPRITE_ITEM_WARDROBE_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_CLOSE_HIVE1_WARDROBE,})),
            
            new ( /* ITEM_GENERIC_DOOR1 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_HIVE1_NPC_REME */
            NameType.NAME_NPC_REME,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_REME_IDLE,}),
            GameSprite.SPRITE_NPC_REME_IDLE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TALK_REME_1,ActionConditions.COND_USE_CARDS_REME,})),
            
            new ( /* ITEM_GENERIC_DOOR2 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_GENERIC_DOOR3 */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_HIVE1_PERFUME */
            NameType.NAME_ITEM_PERFUME,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_USE_HIVE1_PERFUME,ActionConditions.COND_USE_HIVE1_PERFUME_NOT_1,ActionConditions.COND_USE_HIVE1_PERFUME_NOT_2,})),
            
            new ( /* ITEM_HIVE1_AD_BOARD */
            NameType.NAME_ITEM_AD_BOARD,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OBSERVE_HIVE1_AD_BOARD_1,})),
            
            new ( /* ITEM_HIVE1_EXIT_DOOR */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_EXIT_HIVE1_HALL_1,ActionConditions.COND_EXIT_HIVE1_HALL_2,ActionConditions.COND_EXIT_HIVE1_HALL_3,})),
            
            new ( /* ITEM_HIVE1_BASIN */
            NameType.NAME_ITEM_BASIN,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_USE_HIVE1_BASIN_NO_SOAP,ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP,ActionConditions.COND_USE_HIVE1_BASIN_W_SOAP_REPEAT,})),
            
            new ( /* ITEM_SOAP_PICKABLE */
            NameType.NAME_ITEM_SOAP,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,true,GameSprite.SPRITE_PICKABLE_SOAP,GamePickableItem.ITEM_PICK_SOAP_PICKABLE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TAKE_SOAP,})),
            
            new ( /* ITEM_HIVE1_BED */
            NameType.NAME_ITEM_BED,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_USE_HIVE1_BED,})),
            
            new ( /* ITEM_STREET1_STH_DOOR */
            NameType.NAME_SOUTH_NEIGHBORHOOD,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_GO_STREET1_SOUTH_NEIGH,})),
            
            new ( /* ITEM_STREET1_CENTER_DOOR */
            NameType.NAME_CITY_CENTER,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_STREET2_PERIPH_DOOR */
            NameType.NAME_CITY_PERIPH,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_PHARMACY_DOOR */
            NameType.NAME_PHARMACY,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_PHARMACY_NPC_QUEUE */
            NameType.NAME_QUEUE_PEOPLE,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_PHARMACY_QUEUE,}),
            GameSprite.SPRITE_NPC_PHARMACY_QUEUE,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_PHARMACY_NPC_OWNER */
            NameType.NAME_PHARMACIST,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_NPC_PHARMACY_OWNER,}),
            GameSprite.SPRITE_NPC_PHARMACY_OWNER,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TRY_TALK_PHARMACIST,})),
            
            new ( /* ITEM_CITY1_UMBRELLA */
            NameType.NAME_UMBRELLA,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_CITY1_MANYO_UMBRELLA,}),
            GameSprite.SPRITE_CITY1_MANYO_UMBRELLA,true,GameSprite.SPRITE_PICKABLE_UMBRELLA,GamePickableItem.ITEM_PICK_CITY1_UMBRELLA,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_TAKE_UMBRELLA_MORNING,ActionConditions.COND_TAKE_UMBRELLA_NIGHT,})),
            
            new ( /* ITEM_ELMANYO_DOOR */
            NameType.NAME_ELMANYO,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_ELMANYO_OWNER */
            NameType.NAME_OWNER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_MANYO_OWNER,}),
            GameSprite.SPRITE_MANYO_OWNER,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_MANYO_OWNER,})),
            
            new ( /* ITEM_STUFFED_DEER */
            NameType.NAME_STUFFED_DEER,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_DEER,})),
            
            new ( /* ITEM_ELMANYO_OWNER_NIGHT */
            NameType.NAME_OWNER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_MANYO_OWNER,})),
            
            new ( /* ITEM_ELMANYO_CROWD */
            NameType.NAME_CROWD,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_HIVE1_POOR_MAN_WC */
            NameType.NAME_POOR_MAN_WC,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_POOR_MAN_STEADY,}),
            GameSprite.SPRITE_HIVE1_POOR_MAN_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_POOR_MAN_WC,ActionConditions.COND_TALK_POOR_MAN_WC,})),
            
            new ( /* ITEM_HIVE1_ROACH_HEAD */
            NameType.NAME_ROACH,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_ROACH_HEAD,}),
            GameSprite.SPRITE_HIVE1_ROACH_HEAD,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_ROACH,ActionConditions.COND_TAKE_HIVE1_ROACH_HEAD,})),
            
            new ( /* ITEM_HIVE1_PIPE */
            NameType.NAME_PIPE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_PIPE,ActionConditions.COND_OBSERVE_ITEM_HIVE1_PIPE_2,})),
            
            new ( /* ITEM_HIVE1_VALVE_BOX */
            NameType.NAME_VALVE_BOX,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(5){ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE_BOX,ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE_BOX_2,ActionConditions.COND_TAKE_VALVE_BOX_1,ActionConditions.COND_TAKE_VALVE_BOX_MORNING,ActionConditions.COND_USE_SHOELACE_VALVE_BOX,})),
            
            new ( /* ITEM_HIVE1_BACKALLEY_PIPE */
            NameType.NAME_PIPE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE,ActionConditions.COND_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2,})),
            
            new ( /* ITEM_HIVE1_SHOELACE */
            NameType.NAME_SHOELACE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_SHOELACE,}),
            GameSprite.SPRITE_HIVE1_SHOELACE,true,GameSprite.SPRITE_SHOELACE_PICKABLE,GamePickableItem.ITEM_PICK_HIVE1_SHOELACE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(4){ActionConditions.COND_OBSERVE_ITEM_HIVE1_SHOELACE,ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE,ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE_NOT,ActionConditions.COND_TAKE_ITEM_HIVE1_SHOELACE_NOT_2,})),
            
            new ( /* ITEM_HIVE1_VALVE */
            NameType.NAME_VALVE,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_HIVE1_VALVE_BOX_OPENED,}),
            GameSprite.SPRITE_HIVE1_VALVE_BOX_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_HIVE1_VALVE,ActionConditions.COND_TAKE_HIVE1_VALVE,ActionConditions.COND_TAKE_HIVE1_VALVE_2,})),
            
            new ( /* ITEM_HIVE1_MAN_WC_CURED */
            NameType.NAME_ARTURO,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_ARTURO_STANDING,}),
            GameSprite.SPRITE_ARTURO_STANDING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_HIVE1_MAN_WC_CURED,ActionConditions.COND_TALK_ITEM_HIVE1_MAN_WC_CURED,})),
            
            new ( /* ITEM_EXTRAPERLO_INVITATION */
            NameType.NAME_INVITATION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,true,GameSprite.SPRITE_PICKABLE_EXTRAPERLO,GamePickableItem.ITEM_PICK_EXTRAPERLO_INVITATION,DetailType.DETAIL_EXTRAPERLO,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_EXTRAPERLO_INVITATION_DETAIL */
            NameType.NAME_ANNOTATION,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(2){ActionConditions.COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_1,ActionConditions.COND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_2,})),
            
            new ( /* ITEM_PHARMACY_INKWELL */
            NameType.NAME_INKWELL,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PHARMACY_INKWELL_NORMAL,}),
            GameSprite.SPRITE_PHARMACY_INKWELL_NORMAL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(5){ActionConditions.COND_OBSERVE_ITEM_PHARMACY_INKWELL,ActionConditions.COND_TAKE_INKWELL_NOT_1,ActionConditions.COND_TAKE_INKWELL_NOT_2,ActionConditions.COND_TAKE_INKWELL_NOT_3,ActionConditions.COND_USE_UMBRELLA_INKWELL,})),
            
            new ( /* ITEM_PHARMACY_INK */
            NameType.NAME_INK,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_PHARMACY_INKWELL_WASTED,}),
            GameSprite.SPRITE_PHARMACY_INKWELL_WASTED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(3){ActionConditions.COND_OBSERVE_ITEM_PHARMACY_INK,ActionConditions.COND_USE_INVITATION_INK,ActionConditions.COND_USE_INVITATION_INK_2,})),
            
            new ( /* ITEM_NPC_FIK */
            NameType.NAME_FIK,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_FIK_STANDING,}),
            GameSprite.SPRITE_FIK_STANDING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_FIK,})),
            
            new ( /* ITEM_DOOR_EXTRAPERLO */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_BLANK,}),
            GameSprite.SPRITE_BLANK,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TRY_CROSS_EXTRAPERLO_DOOR,})),
            
            new ( /* ITEM_FOREGROUND_EXTRP_WALL */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_FOREGROUND_EXTRAPERLO_WALL,}),
            GameSprite.SPRITE_FOREGROUND_EXTRAPERLO_WALL,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_HIVE1_WATER_FLOWING */
            NameType.NAME_WATER_FLOWING,GameItemFamily.ITEM_FAMILY_TYPE_OBJECT,new(new HashSet<GameSprite>(2){GameSprite.SPRITE_WATER_FLOW_BACKALLEY,GameSprite.SPRITE_WATER_FLOW_BACKALLEY_NIGHT,}),
            GameSprite.SPRITE_WATER_FLOW_BACKALLEY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_DOOR_EXTRAPERLO_REAL */
            NameType.NAME_ITEM_CROSS,GameItemFamily.ITEM_FAMILY_TYPE_DOOR,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_EXTRAPERLO_DOOR_OPENED,}),
            GameSprite.SPRITE_EXTRAPERLO_DOOR_OPENED,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_NPC_GERMAN */
            NameType.NAME_GERMAN,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_GERMAN_WAITING,}),
            GameSprite.SPRITE_GERMAN_WAITING,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_TALK_GERMAN,})),
            
            new ( /* ITEM_NPC_WAITER */
            NameType.NAME_WAITER,GameItemFamily.ITEM_FAMILY_TYPE_NPC,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_WAITER_STEADY,}),
            GameSprite.SPRITE_WAITER_STEADY,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
            new ( /* ITEM_LAST */
            NameType.NAME_NPC_LAST,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(1){GameSprite.SPRITE_LAST,}),
            GameSprite.SPRITE_LAST,false,GameSprite.SPRITE_NONE,GamePickableItem.ITEM_PICK_NONE,DetailType.DETAIL_NONE,
            new(new HashSet<ActionConditions>(1){ActionConditions.COND_OK,})),
            
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
            /* > ATG 5 END < */
        };

        private static readonly MementoParentInfo[] _MementoParentInfo = new MementoParentInfo[(int)MementoParent.MEMENTO_PARENT_TOTAL]
        {
            /* > ATG 6 START < */
            /* MEMENTO_PARENT_JOB_FIND_1 */
            new(
            NameType.NAME_MEMENTO_FIND_JOB_1,GameSprite.SPRITE_MEMENTO_JOB,
            new Memento[2]{Memento.MEMENTO_JOB_FIND_1_1,Memento.MEMENTO_JOB_FIND_1_2,}
            ),
            
            /* MEMENTO_PARENT_RECIPE_MISSION */
            new(
            NameType.NAME_MEMENTO_RECIPE_MISSION,GameSprite.SPRITE_MEMENTO_RECIPE,
            new Memento[1]{Memento.MEMENTO_RECIPE_MISSION_1,}
            ),
            
            /* MEMENTO_PARENT_POOR_MAN_WC */
            new(
            NameType.NAME_MEMENTO_PARENT_POOR_MAN_WC,GameSprite.SPRITE_MEMENTO_POOR_MAN_WC,
            new Memento[3]{Memento.MEMENTO_POOR_MAN_WC_1,Memento.MEMENTO_POOR_MAN_WC_2,Memento.MEMENTO_POOR_MAN_WC_3,}
            ),
            
            /* MEMENTO_PARENT_LAST */
            new(
            NameType.NAME_NPC_LAST,GameSprite.SPRITE_NONE,
            new Memento[1]{Memento.MEMENTO_LAST,}
            ),
            
            /* > ATG 6 END < */
        };

        private static readonly MementoInfo[] _MementoInfo = new MementoInfo[(int)Memento.MEMENTO_TOTAL]
        {
            /* > ATG 7 START < */
            /* MEMENTO_JOB_FIND_1_1 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_MEMENTO_FIND_JOB_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_JOB_FIND_1_2 */
            new(MementoParent.MEMENTO_PARENT_JOB_FIND_1,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_RECIPE_MISSION_1 */
            new(MementoParent.MEMENTO_PARENT_RECIPE_MISSION,DialogPhrase.PHRASE_MEMENTO_RECIPE_MISSION_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_POOR_MAN_WC_1 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_1,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_POOR_MAN_WC_2 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_2,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
            /* MEMENTO_POOR_MAN_WC_3 */
            new(MementoParent.MEMENTO_PARENT_POOR_MAN_WC,DialogPhrase.PHRASE_MEMENTO_POOR_MAN_WC_3,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            true),
            /* MEMENTO_LAST */
            new(MementoParent.MEMENTO_PARENT_LAST,DialogPhrase.PHRASE_NONE,
            new(new HashSet<MementoCombi>(1){MementoCombi.MEMENTO_COMBI_NONE,}),
            false),
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
            new(new HashSet<NameType>(1){NameType.NAME_NONE,}),
            new(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONE,})),

            new(PrefabEnum.PREFAB_NONE, /* DETAIL_LAST */ 
            new(new HashSet<NameType>(1){NameType.NAME_NONE,}),
            new(new HashSet<DialogPhrase>(1){DialogPhrase.PHRASE_NONE,})),

            /* > ATG 9 END < */
        };

        private static readonly ActionInfo[] _ActionInfo = new ActionInfo[(int)GameAction.ACTION_TOTAL]
        {
            /* > ATG 10 START < */
            new( /* ACTION_NONE */
            false,ActionType.ACTION_TYPE_NONE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DAY_MOMENT_DAY */
            false,ActionType.ACTION_TYPE_CHANGE_MOMENT_DAY,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_MORNING,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DAY_MOMENT_NIGHT */
            false,ActionType.ACTION_TYPE_CHANGE_MOMENT_DAY,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_NIGHT,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INITIAL_MEMENTO */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_FIRST, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_MEMENTO_INITIAL_MEMENTO */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SET_SPRITE_CHEST_OPENED */
            false,ActionType.ACTION_TYPE_SET_SPRITE,GameItem.ITEM_HIVE1_CHEST,GameSprite.SPRITE_ITEM_CHEST_OPENED,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_PERFUME */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_PERFUME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SET_SPRITE_CHEST_CLOSED */
            false,ActionType.ACTION_TYPE_SET_SPRITE,GameItem.ITEM_HIVE1_CHEST,GameSprite.SPRITE_ITEM_CHEST_CLOSED,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_HIVE1_PERFUME */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_PERFUME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESTROY_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_OBTAIN_HIVE1_CARDS */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_CARDS_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_WARDROBE_OPENED */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_WARDROBE_OPENED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_HIVE1_WARDROBE */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_WARDROBE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_SOAP */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_WARDROBE */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_WARDROBE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_HIVE1_WARDROBE_OPENED */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_WARDROBE_OPENED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_SOAP */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_MEMENTO_JOB_FIND_1_2 */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_JOB_FIND_1_2,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESTROY_SOAP */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_OBTAIN_SOAP */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_SOAP_PICKABLE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DECISION_SLEEP */
            false,ActionType.ACTION_TYPE_DECISION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_SLEEP_1,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_REME_DAY */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_NPC_REME,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_PHARMACY_DOOR */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_PHARMACY_DOOR,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_MANYO_OWNER_DAY */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_ELMANYO_OWNER,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_MANYO_OWNER_NIGHT */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_ELMANYO_OWNER_NIGHT,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_MEMENTO_RECEIPT_MISSION */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_RECIPE_MISSION_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESTROY_UMBRELLA */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_CITY1_UMBRELLA,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_OBTAIN_UMBRELLA */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_CITY1_UMBRELLA,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_POOR_MAN_WC,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_MEMENTO_POOR_MAN_WC_1 */
            false,ActionType.ACTION_TYPE_MEMENTO,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_POOR_MAN_WC_1,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_ROACH_HEAD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_HIVE1_ROACH_HEAD,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_REBUILD_ROACH_SCARED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, true),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESTROY_HIVE1_SHOELACE */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_HIVE1_SHOELACE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_OBTAIN_HIVE1_SHOELACE */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_HIVE1_SHOELACE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESTROY_HIVE1_VALVE_BOX */
            false,ActionType.ACTION_TYPE_DESTROY,GameItem.ITEM_HIVE1_VALVE_BOX,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_VALVE */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_VALVE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_HIVE1_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_HIVE1_MAN_WC_CURED,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_OBTAIN_EXTRAPERLO_INVITATION */
            false,ActionType.ACTION_TYPE_EARN_ITEM,GameItem.ITEM_EXTRAPERLO_INVITATION,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_MAIN,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_INKWELL */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_PHARMACY_INKWELL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_INK */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_PHARMACY_INK,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_REME_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_REME_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_HIVE1_AD_BOARD_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_HIVE1_AD_BOARD_1,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_1,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_EXIT_HIVE1_HALL_3 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NOT_EXIT_HIVE1_HALL_2,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_BASIN_NO_SOAP */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_NO_SOAP,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_BASIN_W_SOAP */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_HIVE1_BASIN_SOAP,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_BASIN_W_SOAP_REPEAT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_ALREADY_COMBI,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_PERFUME */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_PERFUME_NOT_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME_NOT_1,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_PERFUME_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_PERFUME_NOT_2,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_CARDS_REME */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_REME_CARDS,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_NOT_GO_SOUTH_NEIGH */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_WONT_GO_SOUTH_NEIGH,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TRY_TALK_PHARMACIST */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_TRY_TALK_PHARMACIST_1,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TALK_DEER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_HELLO_DEER,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TALK_MANYO_OWNER */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_OWNER_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_UMBRELLA_MORNING */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_UMBRELLA,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_UMBRELLA_NIGHT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_DIALOG_UMBRELLA_TAKEN,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_MANYO_BCKG */
            false,ActionType.ACTION_TYPE_START_DIALOGUE_BCKG,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_MANYO_BCKG_CROWD,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_POOR_MAN_WC,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TALK_POOR_MAN_WC_BCKG */
            false,ActionType.ACTION_TYPE_START_DIALOGUE_BCKG,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_HIVE1_BCKG_POOR_MAN_WC,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TALK_POOR_MAN_WC */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_HIVE1_POOR_MAN_WC_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_ROACH */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_ROACH,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_HIVE1_PIPE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_PIPE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_HIVE1_ROACH_HEAD */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_ITEM_HIVE1_ROACH_HEAD,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_VALVE_BOX */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_BACKALLEY_PIPE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_VALVE_BOX_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX_2,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_VALVE_BOX_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_VALVE_BOX_CLOSED,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_VALVE_BOX_MORNING */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_VALVE_BOX_CLOSED_MORNING,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_SHOEALCE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_SHOELACE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE_NOT */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_SHOEALCE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_TAKE_ITEM_HIVE1_SHOELACE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_VALVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_VALVE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_SHOELACE_VALVE_BOX */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_SHOELACE_VALVE_BOX,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_HIVE1_VALVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_VALVE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_HIVE1_VALVE_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_HIVE1_VALVE_NOT,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_HIVE1_MAN_WC_CURED,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TALK_MAN_WC_CURED */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_ARTURO_HALL_INN_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_BLURR,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_INVITATION_DETAIL_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_PHARMACY_INKWELL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_PHARMACY_INKWELL,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_OBSERVE_PHARMACY_INK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_OBSERVE_ITEM_PHARMACY_INK,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_1 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NO_REASON_TO_DO,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PHARMACIST_NOT_TAKE_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_TAKE_INKWELL_NOT_3 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_PHARMACIST_NOT_TAKE_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_UMBRELLA_INKWELL */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_USE_UMBRELLA_WITH_INKWELL,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_INVITATION_INK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_INVITATION_WITH_INK,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOGUE_USE_INVITATION_INK_2 */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_USE_INVITATION_WITH_INK_ALREADY,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_OPEN_CHEST */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_CANCEL_OPEN_CHEST */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_CHEST_OPENED, true),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_CARDS_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_OPEN_WARDROBE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_CANCEL_OPEN_WARDROBE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_WARDROBE_OPENED, true),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_HIVE1_AD_OBSERVED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_AD_BOARD_OBSERVED_1, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_USED_BASIN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_BASIN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_SOAP_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_SOAP_PICKABLE_TAKEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_USED_PERFUME */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_USED_PERFUME, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_UMBRELLA_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_UMBRELLA_PICKABLE_TAKEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_PIPE_OBSERVATION_1 */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_1, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_PIPE_OBSERVATION_2 */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_PIPE_OBSERVATION_2, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_COCKROACH_SCARED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_COCKROACH_SCARED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_VALVE_BOX_NEED_OPEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_NEED_OPEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_SHOELACE_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_HIVE1_SHOELACE_PICKABLE_TAKEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_VALVE_BOX_OPENED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_VALVE_BOX_OPENED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_VALVE_ACTIVATED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_HIVE1_VALVE_ACTIVATED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_OBSERVED_INVITATION_RELIEF */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_OBSERVED_INVITATION_RELIEF, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INVITATION_UNDERSTOOD_PHRASE */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_UNDERSTOOD_PHRASE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INKWELL_NOT_TOUCH_WARN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_NOT_TOUCH_WARN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INVITATION_REVEALED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_REVEALED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_MANYO_REFUSED_WORK */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_MANYO_LOOK_FOR_RECIPE_MISSION */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_POOR_MAN_WC_NEEDS_WATER */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_TALKED_ARTURO_HALL_PUB */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_PUB, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_TALKED_ARTURO_HALL_COMPLETED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_INN_COMPLETED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INVITATION_PICKABLE_TAKEN */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_ITEM_EXTRAPERLO_INVITATION_PICKABLE_TAKEN, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_INKWELL_WASTED */
            false,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_WASTED, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOG_USELESS_OBSERVE */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE_OBSERVE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOG_USELESS_TALK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE_TALK,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DIALOG_USELESS_ACTION */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_SIMPLE,DialogPhrase.PHRASE_NONSENSE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_ANIMATE_REME_TEST */
            true,ActionType.ACTION_TYPE_START_ANIMATION,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_REME_TEST),
            
            new( /* ACTION_SPAWN_FIK_1 */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_NPC_FIK,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_TALK_FIK */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_FIK_1_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_EVENT_EXTRAPERLO_SAID_PHRASE */
            true,ActionType.ACTION_TYPE_EVENT,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SAID_PHRASE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_TALK_FIK_NOT_CROSS */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_FIK_NOT_CROSS_DOOR,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_EXTRAPERLO_DOOR */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_DOOR_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_EXTRAPERLO_WALL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_FOREGROUND_EXTRP_WALL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_DESPAWN_EXTRAPERLO_DOOR */
            false,ActionType.ACTION_TYPE_DESPAWN,GameItem.ITEM_DOOR_EXTRAPERLO,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPAWN_EXTRAPERLO_DOOR_REAL */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_DOOR_EXTRAPERLO_REAL,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_ANIMATE_WATER_FLOWING_NIGHT */
            false,ActionType.ACTION_TYPE_TRIGGER_ITEM_ANIMATION,GameItem.ITEM_HIVE1_WATER_FLOWING,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_TALK_GERMAN */
            false,ActionType.ACTION_TYPE_START_DIALOGUE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_GERMAN_1_INTRO,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_SPWAN_GERMAN_1 */
            false,ActionType.ACTION_TYPE_SPAWN,GameItem.ITEM_NPC_GERMAN,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            new( /* ACTION_LAST */
            false,ActionType.ACTION_TYPE_NONE,GameItem.ITEM_NONE,GameSprite.SPRITE_NONE,
            CharacterType.CHARACTER_NONE,Memento.MEMENTO_NONE,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false),}, 
            DecisionType.DECISION_NONE,MomentType.MOMENT_ANY,DialogType.DIALOG_NONE,DialogPhrase.PHRASE_NONE,AnimationTrigger.ANIMATION_TRIGGER_NONE,GameAnimation.ANIMATION_NONE),
            
            /* > ATG 10 END < */
        };
    }
}
