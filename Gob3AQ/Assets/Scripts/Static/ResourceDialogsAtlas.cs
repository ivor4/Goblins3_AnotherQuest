using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceDialogsAtlas
{
    public static class ResourceDialogsAtlasClass
    {
        public static ref readonly DialogConfig GetDialogConfig(DialogType dialog)
        {
            if((uint)dialog < (uint)DialogType.DIALOG_TOTAL)
            {
                return ref _DialogConfig[(int)dialog];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDialogConfig: Invalid dialog {dialog}");
                return ref DialogConfig.EMPTY;
            }
        }

        public static ref readonly DialogOptionConfig GetDialogOptionConfig(DialogOption option)
        {
            if((uint)option < (uint)DialogOption.DIALOG_OPTION_TOTAL)
            {
                return ref _DialogOptionConfig[(int)option];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetDialogOptionConfig: Invalid dialog option {option}");
                return ref DialogOptionConfig.EMPTY;
            }
        }

        public static ref readonly PhraseConfig GetPhraseConfig(DialogPhrase phrase)
        {
            if((uint)phrase < (uint)DialogPhrase.PHRASE_TOTAL)
            {
                return ref _PhraseConfig[(int)phrase];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetPhraseConfig: Invalid dialog phrase {phrase}");
                return ref PhraseConfig.EMPTY;
            }
        }




        private static readonly DialogConfig[] _DialogConfig = new DialogConfig[(int)DialogType.DIALOG_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* DIALOG_SIMPLE */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_SIMPLE, }
            ),
            
            new( /* DIALOG_REME_INTRO */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_REME_INTRO, }
            ),
            
            new( /* DIALOG_REME */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[2]{DialogOption.DIALOG_OPTION_REME_1, DialogOption.DIALOG_OPTION_REME_2, }
            ),
            
            new( /* DIALOG_REME_CARDS */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_REME_CARDS, }
            ),
            
            new( /* DIALOG_TRY_TALK_PHARMACIST_1 */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_TRY_TALK_PHARMACIST, }
            ),
            
            new( /* DIALOG_TRY_TALK_PHARMACIST_2 */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_PHARMACIST_BUSY, }
            ),
            
            new( /* DIALOG_MANYO_OWNER_INTRO */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_ONWER_INTRO, }
            ),
            
            new( /* DIALOG_MANYO_OWNER */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[5]{DialogOption.DIALOG_OPTION_MANYO_WORK_NOTE, DialogOption.DIALOG_OPTION_MANYO_WORKS_CITY, DialogOption.DIALOG_OPTION_MANYO_MENU_DAY, DialogOption.DIALOG_OPTION_MANYO_CROWD, DialogOption.DIALOG_OPTION_MANYO_BYE, }
            ),
            
            new( /* DIALOG_MANYO_UMBRELLA */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN,GameItem.ITEM_ELMANYO_OWNER,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_UMBRELLA, }
            ),
            
            new( /* DIALOG_MANYO_BCKG_CROWD */
            new GameItem[1]{GameItem.ITEM_ELMANYO_CROWD,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_BCKG_CROWD, }
            ),
            
            new( /* DIALOG_HIVE1_BCKG_POOR_MAN_WC */
            new GameItem[1]{GameItem.ITEM_HIVE1_POOR_MAN_WC,},
            new DialogOption[1]{DialogOption.DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTIONS, }
            ),
            
            new( /* DIALOG_HIVE1_POOR_MAN_WC */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[4]{DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_0, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_1, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_2, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_3, }
            ),
            
            new( /* DIALOG_HIVE1_POOR_MAN_WC_INTRO */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_INTRO, }
            ),
            
            new( /* DIALOG_ARTURO_HALL_INN */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[3]{DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_0, DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_1, DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_2, }
            ),
            
            new( /* DIALOG_ARTURO_HALL_INN_INTRO */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_INTRO, }
            ),
            
            new( /* DIALOG_PHARMACIST_NOT_TAKE_INKWELL */
            new GameItem[1]{GameItem.ITEM_PHARMACY_NPC_OWNER,},
            new DialogOption[1]{DialogOption.DIALOG_PHARMACIST_NOT_TAKE_INKWELL_OPTION_INTRO, }
            ),
            
            new( /* DIALOG_USE_UMBRELLA_WITH_INKWELL */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL, }
            ),
            
            new( /* DIALOG_USE_UMBRELLA_WITH_INKWELL_REACT */
            new GameItem[1]{GameItem.ITEM_PHARMACY_NPC_OWNER,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL_REACT, }
            ),
            
            new( /* DIALOG_LAST */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_NONE, }
            ),
            
            /* > ATG 1 END < */
        };

        private static readonly DialogOptionConfig[] _DialogOptionConfig = new DialogOptionConfig[(int)DialogOption.DIALOG_OPTION_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* DIALOG_OPTION_SIMPLE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE, }
            ),
            new( /* DIALOG_OPTION_REME_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_REME,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_INTRO_1, DialogPhrase.PHRASE_DIALOG_REME_INTRO_2, DialogPhrase.PHRASE_DIALOG_REME_INTRO_3, }
            ),
            new( /* DIALOG_OPTION_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_REME,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_1_1, DialogPhrase.PHRASE_DIALOG_REME_1_2, DialogPhrase.PHRASE_DIALOG_REME_1_3, }
            ),
            new( /* DIALOG_OPTION_REME_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_REME_2_1, }
            ),
            new( /* DIALOG_OPTION_REME_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_USE_CARDS_REME_1, DialogPhrase.PHRASE_USE_CARDS_REME_2, }
            ),
            new( /* DIALOG_OPTION_TRY_TALK_PHARMACIST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_TRY_TALK_PHARMACIST_2,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_TRY_TALK_PHARMACIST_BUSY, }
            ),
            new( /* DIALOG_OPTION_PHARMACIST_BUSY */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[2]{DialogPhrase.PHRASE_PHARMACIST_BUSY_1, DialogPhrase.PHRASE_PHARMACIST_BUSY_2, }
            ),
            new( /* DIALOG_OPTION_MANYO_ONWER_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_MANYO_OWNER,true,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_MANYO_INTRO_1, DialogPhrase.PHRASE_DIALOG_MANYO_INTRO_2, }
            ),
            new( /* DIALOG_OPTION_MANYO_WORK_NOTE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_MORNING,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false), },
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_1, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_2, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_3, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_4, }
            ),
            new( /* DIALOG_OPTION_MANYO_WORKS_CITY */
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false), },
            MomentType.MOMENT_MORNING,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_MANYO_WORKS_CITY_1, DialogPhrase.PHRASE_DIALOG_MANYO_WORKS_CITY_2, }
            ),
            new( /* DIALOG_OPTION_MANYO_MENU_DAY */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[6]{DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_1, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_2, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_3, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_4, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_5, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_6, }
            ),
            new( /* DIALOG_OPTION_MANYO_CROWD */
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false), },
            MomentType.MOMENT_NIGHT,
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, false), },
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_1, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_2, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_3, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_4, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_5, }
            ),
            new( /* DIALOG_OPTION_MANYO_BYE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_MANYO_BYE_1, }
            ),
            new( /* DIALOG_OPTION_MANYO_UMBRELLA */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_MANYO_UMBRELLA_NOT_1, }
            ),
            new( /* DIALOG_OPTION_MANYO_BCKG_CROWD */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_1, DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_2, DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_3, }
            ),
            new( /* DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTIONS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_3_0, }
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_3, }
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_1, }
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_POOR_MAN_WC_NEED_WATER, false), },
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_2, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_3, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_4, }
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_3_0, }
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,true,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_2, }
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_PUB, false), },
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_3, }
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_PUB, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[2]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_INN_COMPLETED, false), new(GameEvent.EVENT_ITEM_EXTRAPERLO_INVITATION_PICKABLE_TAKEN, false), },
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[7]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_3, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_4, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_5, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_6, }
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_2_0, }
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_1, }
            ),
            new( /* DIALOG_PHARMACIST_NOT_TAKE_INKWELL_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_PHARMACIST_NOT_TAKE_INKWELL_INTRO, }
            ),
            new( /* DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_INKWELL_WASTED, false), },
            DialogType.DIALOG_USE_UMBRELLA_WITH_INKWELL_REACT,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL, }
            ),
            new( /* DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL_REACT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER, DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER_2, }
            ),
            new( /* DIALOG_OPTION_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            MomentType.MOMENT_ANY,
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE, }
            ),
            /* > ATG 2 END < */
        };

        private static readonly PhraseConfig[] _PhraseConfig = new PhraseConfig[(int)DialogPhrase.PHRASE_TOTAL]
        {
            /* > ATG 3 START < */
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE_OBSERVE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE_TALK */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE_NOT_THOUGHT */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE_COMBI */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ALREADY_COMBI */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_GREAT_IDEA_COMBI */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_INTRO_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_INTRO_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_INTRO_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_1_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_1_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_1_3 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_REME_2_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_FIND_JOB_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_HIVE1_AD_BOARD_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NOT_EXIT_HIVE1_HALL_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NOT_EXIT_HIVE1_HALL_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_HIVE1_BASIN_NO_SOAP */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_HIVE1_BASIN_SOAP */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_HIVE1_PERFUME */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_HIVE1_PERFUME_NOT_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_HIVE1_PERFUME_NOT_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_CARDS_REME_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_CARDS_REME_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DECISION_NOT_SLEEP */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DECISION_SLEEP_NAP */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DECISION_SLEEP_LONG */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_WONT_GO_SOUTH_NEIGH */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TRY_TALK_PHARMACIST_BUSY */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_PHARMACIST_BUSY_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_PHARMACIST_BUSY_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_HELLO_DEER */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_INTRO_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_INTRO_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORK_NOTE_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORK_NOTE_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORK_NOTE_3 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORK_NOTE_4 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORKS_CITY_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_WORKS_CITY_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_4 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_5 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_MENU_DAY_6 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_CROWD_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_CROWD_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_CROWD_3 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_CROWD_4 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_CROWD_5 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_BYE_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_RECIPE_MISSION_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_MANYO_UMBRELLA_NOT_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_UMBRELLA_TAKEN */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_POOR_MAN_WC */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_0_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_1_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_2_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_3_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_4 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_3_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_POOR_MAN_WC_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_POOR_MAN_WC_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_POOR_MAN_WC_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_ROACH */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_PIPE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_ITEM_HIVE1_ROACH_HEAD */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_VALVE_BOX_CLOSED */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_VALVE_BOX_CLOSED_MORNING */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_SHOELACE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NO_REASON_TO_DO */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_TAKE_ITEM_HIVE1_SHOELACE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_USE_SHOELACE_VALVE_BOX */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_HIVE1_VALVE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_HIVE1_VALVE_NOT */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_HIVE1_MAN_WC_CURED */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_3 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_4 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_5 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_6 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_2_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_BLURR */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_PHARMACY_INKWELL */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_OBSERVE_ITEM_PHARMACY_INK */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_PHARMACIST_NOT_TAKE_INKWELL_INTRO */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_INVITATION_WITH_INK */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_USE_INVITATION_WITH_INK_ALREADY */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_DIALOG_LAST */ 
            /* > ATG 3 END < */
        };
    }
}