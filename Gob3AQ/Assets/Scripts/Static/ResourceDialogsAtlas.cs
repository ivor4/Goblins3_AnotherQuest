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

        public static GameItem GetItemForCharacter(CharacterType character)
        {
            if((uint)character < (uint)CharacterType.CHARACTER_TOTAL)
            {
                return _CharacterToItemDict[(int)character];
            }
            else
            {
                Debug.LogError($"[ResourceDialogsAtlas] GetItemForCharacter: Invalid character {character}");
                return GameItem.ITEM_NONE;
            }
        }

        private static readonly GameItem[] _CharacterToItemDict = new GameItem[(int)CharacterType.CHARACTER_TOTAL]
        {
            GameItem.ITEM_PLAYER_MAIN,
            GameItem.ITEM_NONE,
            GameItem.ITEM_NONE
        };


        private static readonly DialogConfig[] _DialogConfig = new DialogConfig[(int)DialogType.DIALOG_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* DIALOG_SIMPLE */
            new GameItem[1]{GameItem.ITEM_NONE},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_SIMPLE}
            ),

            new( /* DIALOG_REME_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_NPC_REME},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_REME_INTRO}
            ),

            new( /* DIALOG_REME */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_NPC_REME},
            new DialogOption[3]{DialogOption.DIALOG_OPTION_REME_1, DialogOption.DIALOG_OPTION_REME_2, DialogOption.DIALOG_OPTION_REME_3}
            ),

            new( /* DIALOG_REME_CARDS */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_NPC_REME},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_REME_CARDS}
            ),

            new( /* DIALOG_TRY_TALK_PHARMACIST_1 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_PHARMACY_NPC_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_TRY_TALK_PHARMACIST}
            ),

            new( /* DIALOG_TRY_TALK_PHARMACIST_2 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_PHARMACY_NPC_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_PHARMACIST_BUSY}
            ),

            new( /* DIALOG_MANYO_OWNER_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_ELMANYO_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_ONWER_INTRO}
            ),

            new( /* DIALOG_MANYO_OWNER */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_ELMANYO_OWNER},
            new DialogOption[5]{DialogOption.DIALOG_OPTION_MANYO_WORK_NOTE, DialogOption.DIALOG_OPTION_MANYO_WORKS_CITY, DialogOption.DIALOG_OPTION_MANYO_MENU_DAY, DialogOption.DIALOG_OPTION_MANYO_CROWD, DialogOption.DIALOG_OPTION_MANYO_BYE}
            ),

            new( /* DIALOG_MANYO_UMBRELLA */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_ELMANYO_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_UMBRELLA}
            ),

            new( /* DIALOG_MANYO_BCKG_CROWD */
            new GameItem[1]{GameItem.ITEM_ELMANYO_CROWD},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_MANYO_BCKG_CROWD}
            ),

            new( /* DIALOG_HIVE1_BCKG_POOR_MAN_WC */
            new GameItem[1]{GameItem.ITEM_HIVE1_POOR_MAN_WC},
            new DialogOption[1]{DialogOption.DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTIONS}
            ),

            new( /* DIALOG_HIVE1_POOR_MAN_WC */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_POOR_MAN_WC},
            new DialogOption[4]{DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_0, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_1, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_2, DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_3}
            ),

            new( /* DIALOG_HIVE1_POOR_MAN_WC_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_POOR_MAN_WC},
            new DialogOption[1]{DialogOption.DIALOG_HIVE1_POOR_MAN_WC_OPTION_INTRO}
            ),

            new( /* DIALOG_ARTURO_HALL_INN */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_MAN_WC_CURED},
            new DialogOption[3]{DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_0, DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_1, DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_2}
            ),

            new( /* DIALOG_ARTURO_HALL_INN_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_HIVE1_MAN_WC_CURED},
            new DialogOption[1]{DialogOption.DIALOG_ARTURO_HALL_INN_OPTION_INTRO}
            ),

            new( /* DIALOG_PHARMACIST_NOT_TAKE_INKWELL */
            new GameItem[1]{GameItem.ITEM_PHARMACY_NPC_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_PHARMACIST_NOT_TAKE_INKWELL_OPTION_INTRO}
            ),

            new( /* DIALOG_USE_UMBRELLA_WITH_INKWELL */
            new GameItem[1]{GameItem.ITEM_PLAYER_MAIN},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL}
            ),

            new( /* DIALOG_USE_UMBRELLA_WITH_INKWELL_REACT */
            new GameItem[1]{GameItem.ITEM_PHARMACY_NPC_OWNER},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL_REACT}
            ),

            new( /* DIALOG_FIK_1 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_FIK},
            new DialogOption[4]{DialogOption.DIALOG_FIK_1_OPTION_0, DialogOption.DIALOG_FIK_1_OPTION_1, DialogOption.DIALOG_FIK_1_OPTION_2, DialogOption.DIALOG_FIK_1_OPTION_3}
            ),

            new( /* DIALOG_FIK_1_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_FIK},
            new DialogOption[1]{DialogOption.DIALOG_FIK_1_OPTION_INTRO}
            ),

            new( /* DIALOG_FIK_2 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_FIK},
            new DialogOption[4]{DialogOption.DIALOG_FIK_2_OPTION_0, DialogOption.DIALOG_FIK_2_OPTION_1, DialogOption.DIALOG_FIK_2_OPTION_2, DialogOption.DIALOG_FIK_2_OPTION_3}
            ),

            new( /* DIALOG_FIK_NOT_CROSS_DOOR */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_FIK},
            new DialogOption[1]{DialogOption.DIALOG_FIK_NOT_CROSS_OPTION_0}
            ),

            new( /* DIALOG_GERMAN_1 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_GERMAN},
            new DialogOption[3]{DialogOption.DIALOG_GERMAN_1_OPTION_0, DialogOption.DIALOG_GERMAN_1_OPTION_1, DialogOption.DIALOG_GERMAN_1_OPTION_2}
            ),

            new( /* DIALOG_GERMAN_1_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_GERMAN},
            new DialogOption[1]{DialogOption.DIALOG_GERMAN_1_OPTION_INTRO}
            ),

            new( /* DIALOG_WAITER_1_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[1]{DialogOption.DIALOG_WAITER_OPTION_INTRO_1}
            ),

            new( /* DIALOG_WAITER_2_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[1]{DialogOption.DIALOG_WAITER_OPTION_INTRO_2}
            ),

            new( /* DIALOG_WAITER_USE_OLD_INVITATION */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[1]{DialogOption.DIALOG_WAITER_OPTION_USE_OLD_INVITATION}
            ),

            new( /* DIALOG_WAITER_USE_NEW_INVITATION */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[1]{DialogOption.DIALOG_WAITER_OPTION_USE_NEW_INVITATION}
            ),

            new( /* DIALOG_UNKNOWN_GIRLS_1 */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_UNKNOWN_WOMEN},
            new DialogOption[1]{DialogOption.DIALOG_UNKNOWN_GIRLS_OPTION_INTRO_1}
            ),

            new( /* DIALOG_ARTURO_EXTRAPERLO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO},
            new DialogOption[4]{DialogOption.DIALOG_ARTURO_EXTRAPERLO_OPTION_0, DialogOption.DIALOG_ARTURO_EXTRAPERLO_OPTION_1, DialogOption.DIALOG_ARTURO_EXTRAPERLO_OPTION_2, DialogOption.DIALOG_FIK_1_OPTION_3}
            ),

            new( /* DIALOG_ARTURO_EXTRAPERLO_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO},
            new DialogOption[1]{DialogOption.DIALOG_ARTURO_EXTRAPERLO_OPTION_INTRO}
            ),

            new( /* DIALOG_CLOWN_EXTRAPERLO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[5]{DialogOption.DIALOG_CLOWN_EXTRAPERLO_OPTION_0, DialogOption.DIALOG_CLOWN_EXTRAPERLO_OPTION_1, DialogOption.DIALOG_CLOWN_EXTRAPERLO_OPTION_2, DialogOption.DIALOG_CLOWN_EXTRAPERLO_OPTION_3, DialogOption.DIALOG_FIK_1_OPTION_3}
            ),

            new( /* DIALOG_CLOWN_EXTRAPERLO_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[1]{DialogOption.DIALOG_CLOWN_EXTRAPERLO_OPTION_INTRO}
            ),

            new( /* DIALOG_SILVANA_EXTRAPERLO */
            new GameItem[3]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[3]{DialogOption.DIALOG_SILVANA_EXTRAPERLO_OPTION_0, DialogOption.DIALOG_SILVANA_EXTRAPERLO_OPTION_1, DialogOption.DIALOG_SILVANA_EXTRAPERLO_OPTION_2}
            ),

            new( /* DIALOG_SILVANA_EXTRAPERLO_INTRO */
            new GameItem[3]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[1]{DialogOption.DIALOG_SILVANA_EXTRAPERLO_OPTION_INTRO}
            ),

            new( /* DIALOG_WAITER_W_INVITATION */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[4]{DialogOption.DIALOG_WAITER_W_INVITATION_OPTION_0, DialogOption.DIALOG_WAITER_W_INVITATION_OPTION_1, DialogOption.DIALOG_WAITER_OPTION_TERRACE, DialogOption.DIALOG_WAITER_W_INVITATION_OPTION_2}
            ),

            new( /* DIALOG_WAITER_W_INVITATION_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_WAITER},
            new DialogOption[1]{DialogOption.DIALOG_WAITER_W_INVITATION_OPTION_INTRO}
            ),

            new( /* DIALOG_CLOWN_DRUNK */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[1]{DialogOption.DIALOG_CLOWN_DRUNK_OPTION_0}
            ),

            new( /* DIALOG_ARTURO_DRUNK_INTRO */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO},
            new DialogOption[1]{DialogOption.DIALOG_ARTURO_DRUNK_INTRO}
            ),

            new( /* DIALOG_ARTURO_DRUNK */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO},
            new DialogOption[2]{DialogOption.DIALOG_ARTURO_DRUNK_OPTION_0, DialogOption.DIALOG_ARTURO_DRUNK_OPTION_1}
            ),

            new( /* DIALOG_SILVANA_DRUNK */
            new GameItem[2]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO},
            new DialogOption[2]{DialogOption.DIALOG_SILVANA_DRUNK_OPTION_0, DialogOption.DIALOG_SILVANA_DRUNK_OPTION_1}
            ),

            new( /* DIALOG_SILVANA_OLIVE */
            new GameItem[3]{GameItem.ITEM_PLAYER_MAIN, GameItem.ITEM_NPC_SILVANA_EXTRAPERLO, GameItem.ITEM_NPC_CLOWN},
            new DialogOption[1]{DialogOption.DIALOG_SILVANA_OLIVE_OPTION_0}
            ),

            new( /* DIALOG_MAINCHAR_PEE */
            new GameItem[1]{GameItem.ITEM_PLAYER_MAIN},
            new DialogOption[1]{DialogOption.DIALOG_MAINCHAR_PEE_OPTION_0}
            ),

            new( /* DIALOG_SILVANA_OBSERVE_PEE */
            new GameItem[1]{GameItem.ITEM_NPC_SILVANA_EXTRAPERLO_GARD},
            new DialogOption[1]{DialogOption.DIALOG_SILVANA_OBSERVED_PEE_OPTION_0}
            ),

            new( /* DIALOG_LAST */
            new GameItem[1]{GameItem.ITEM_NONE},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_NONE}
            ),

            /* > ATG 1 END < */
        };

        private static readonly DialogOptionConfig[] _DialogOptionConfig = new DialogOptionConfig[(int)DialogOption.DIALOG_OPTION_TOTAL]
        {
            /* > ATG 2 START < */
            new( /* DIALOG_OPTION_SIMPLE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE}
            ),
            new( /* DIALOG_OPTION_REME_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_REME,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_INTRO_1, DialogPhrase.PHRASE_DIALOG_REME_INTRO_2, DialogPhrase.PHRASE_DIALOG_REME_INTRO_3}
            ),
            new( /* DIALOG_OPTION_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_REME,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_1_1, DialogPhrase.PHRASE_DIALOG_REME_1_2, DialogPhrase.PHRASE_DIALOG_REME_1_3}
            ),
            new( /* DIALOG_OPTION_REME_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_REME_2_1}
            ),
            new( /* DIALOG_OPTION_REME_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[2]{GameAction.ACTION_ANIMATE_REME_TEST, GameAction.ACTION_DIALOGUE_OBSERVE_SHOEALCE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_REME_3_TEST}
            ),
            new( /* DIALOG_OPTION_REME_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_USE_CARDS_REME_1, DialogPhrase.PHRASE_USE_CARDS_REME_2}
            ),
            new( /* DIALOG_OPTION_TRY_TALK_PHARMACIST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_TRY_TALK_PHARMACIST_2,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_TRY_TALK_PHARMACIST_BUSY}
            ),
            new( /* DIALOG_OPTION_PHARMACIST_BUSY */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[2]{DialogPhrase.PHRASE_PHARMACIST_BUSY_1, DialogPhrase.PHRASE_PHARMACIST_BUSY_2}
            ),
            new( /* DIALOG_OPTION_MANYO_ONWER_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_MANYO_OWNER,true,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_MANYO_INTRO_1, DialogPhrase.PHRASE_DIALOG_MANYO_INTRO_2}
            ),
            new( /* DIALOG_OPTION_MANYO_WORK_NOTE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_MORNING,
            new GameAction[1]{GameAction.ACTION_EVENT_MANYO_REFUSED_WORK},
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_1, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_2, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_3, DialogPhrase.PHRASE_DIALOG_MANYO_WORK_NOTE_4}
            ),
            new( /* DIALOG_OPTION_MANYO_WORKS_CITY */
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false)},
            MomentType.MOMENT_MORNING,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_MANYO_WORKS_CITY_1, DialogPhrase.PHRASE_DIALOG_MANYO_WORKS_CITY_2}
            ),
            new( /* DIALOG_OPTION_MANYO_MENU_DAY */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[6]{DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_1, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_2, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_3, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_4, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_5, DialogPhrase.PHRASE_DIALOG_MANYO_MENU_DAY_6}
            ),
            new( /* DIALOG_OPTION_MANYO_CROWD */
            new GameEventCombi[1]{new(GameEvent.EVENT_MANYO_REFUSED_WORK, false)},
            MomentType.MOMENT_NIGHT,
            new GameAction[2]{GameAction.ACTION_EVENT_MANYO_LOOK_FOR_RECIPE_MISSION, GameAction.ACTION_MEMENTO_RECEIPT_MISSION},
            DialogType.DIALOG_MANYO_OWNER,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_1, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_2, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_3, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_4, DialogPhrase.PHRASE_DIALOG_MANYO_CROWD_5}
            ),
            new( /* DIALOG_OPTION_MANYO_BYE */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_MANYO_BYE_1}
            ),
            new( /* DIALOG_OPTION_MANYO_UMBRELLA */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_MANYO_UMBRELLA_NOT_1}
            ),
            new( /* DIALOG_OPTION_MANYO_BCKG_CROWD */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_1, DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_2, DialogPhrase.PHRASE_DIALOG_MANYO_BCKG_CROWD_3}
            ),
            new( /* DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTIONS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,true,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_3_0}
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_3}
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_1}
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[2]{GameAction.ACTION_EVENT_POOR_MAN_WC_NEEDS_WATER, GameAction.ACTION_MEMENTO_POOR_MAN_WC_1},
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_2, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_3, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_4}
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_3_0}
            ),
            new( /* DIALOG_HIVE1_POOR_MAN_WC_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_HIVE1_POOR_MAN_WC,true,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_0, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_1, DialogPhrase.PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_2}
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_EVENT_TALKED_ARTURO_HALL_PUB},
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_3}
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_TALKED_ARTURO_HALL_PUB, false)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_EVENT_TALKED_ARTURO_HALL_COMPLETED, GameAction.ACTION_EVENT_INVITATION_PICKABLE_TAKEN, GameAction.ACTION_OBTAIN_EXTRAPERLO_INVITATION},
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[7]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_3, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_4, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_5, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_6}
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_2_0}
            ),
            new( /* DIALOG_ARTURO_HALL_INN_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_ARTURO_HALL_INN,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_0, DialogPhrase.PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_1}
            ),
            new( /* DIALOG_PHARMACIST_NOT_TAKE_INKWELL_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_PHARMACIST_NOT_TAKE_INKWELL_INTRO}
            ),
            new( /* DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_EVENT_INKWELL_WASTED},
            DialogType.DIALOG_USE_UMBRELLA_WITH_INKWELL_REACT,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL}
            ),
            new( /* DIALOG_OPTION_USE_UMBRELLA_WITH_INKWELL_REACT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER, DialogPhrase.PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER_2}
            ),
            new( /* DIALOG_FIK_1_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_1,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_0_1}
            ),
            new( /* DIALOG_FIK_1_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_2,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_1_3}
            ),
            new( /* DIALOG_FIK_1_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_2_1, DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_2_2}
            ),
            new( /* DIALOG_FIK_1_OPTION_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_3_0}
            ),
            new( /* DIALOG_FIK_1_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_1,true,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_1_INTRO_0, DialogPhrase.PHRASE_DIALOG_FIK_1_INTRO_1}
            ),
            new( /* DIALOG_FIK_2_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_1,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_0_1}
            ),
            new( /* DIALOG_FIK_2_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_1,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_1_1}
            ),
            new( /* DIALOG_FIK_2_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_FIK_1,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_2_1}
            ),
            new( /* DIALOG_FIK_2_OPTION_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_INVITATION_UNDERSTOOD_PHRASE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_EVENT_EXTRAPERLO_SAID_PHRASE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_3_0, DialogPhrase.PHRASE_DIALOG_FIK_2_OPTION_3_1}
            ),
            new( /* DIALOG_FIK_NOT_CROSS_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_FIK_NOT_CROSS}
            ),
            new( /* DIALOG_GERMAN_1_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_GERMAN_1,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_0_1}
            ),
            new( /* DIALOG_GERMAN_1_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_GERMAN_1,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_1_3, DialogPhrase.PHRASE_DIALOG_GERMAN_1_OPTION_1_4}
            ),
            new( /* DIALOG_GERMAN_1_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_FIK_1_OPTION_3_0}
            ),
            new( /* DIALOG_GERMAN_1_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_GERMAN_1,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_GERMAN_1_INTRO}
            ),
            new( /* DIALOG_WAITER_OPTION_INTRO_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_WAITER_INTRO_1_0, DialogPhrase.PHRASE_DIALOG_WAITER_INTRO_1_1, DialogPhrase.PHRASE_DIALOG_WAITER_INTRO_1_2, DialogPhrase.PHRASE_DIALOG_WAITER_INTRO_1_3}
            ),
            new( /* DIALOG_WAITER_OPTION_INTRO_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE}
            ),
            new( /* DIALOG_WAITER_OPTION_USE_OLD_INVITATION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_EVENT_SHOWN_OLD_INVITATION},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_WAITER_USE_OLD_INV_0, DialogPhrase.PHRASE_DIALOG_WAITER_USE_OLD_INV_1, DialogPhrase.PHRASE_DIALOG_WAITER_USE_OLD_INV_2}
            ),
            new( /* DIALOG_WAITER_OPTION_USE_NEW_INVITATION */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_WAITER_USE_NEW_INV_0, DialogPhrase.PHRASE_DIALOG_WAITER_USE_NEW_INV_1}
            ),
            new( /* DIALOG_UNKNOWN_GIRLS_OPTION_INTRO_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_0, DialogPhrase.PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_1, DialogPhrase.PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_2}
            ),
            new( /* DIALOG_ARTURO_EXTRAPERLO_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_ARTURO_EXTRAPERLO,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_3}
            ),
            new( /* DIALOG_ARTURO_EXTRAPERLO_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_START_TEST_CARD_GAME},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_3, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_4}
            ),
            new( /* DIALOG_ARTURO_EXTRAPERLO_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_WON_CARDS_ARTURO, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_ARTURO_EXTRAPERLO,false,
            new DialogPhrase[8]{DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_1, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_2, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_3, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_4, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_5, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_6, DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_7}
            ),
            new( /* DIALOG_ARTURO_EXTRAPERLO_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_ARTURO_EXTRAPERLO,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_ARTURO_EXTRAPERLO_INTRO}
            ),
            new( /* DIALOG_CLOWN_EXTRAPERLO_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_CLOWN_EXTRAPERLO,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_1, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_2, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_3, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_4}
            ),
            new( /* DIALOG_CLOWN_EXTRAPERLO_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_CLOWN_EXTRAPERLO,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_0, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_1}
            ),
            new( /* DIALOG_CLOWN_EXTRAPERLO_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_CLOWN_EXTRAPERLO,false,
            new DialogPhrase[8]{DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_0, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_2, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_4, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_5, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_6, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_7, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_8, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_9}
            ),
            new( /* DIALOG_CLOWN_EXTRAPERLO_OPTION_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_EXTRAPERLO_SILVANA_REFUSED_MAINCHAR, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_CLOWN_EXTRAPERLO,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_0, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_1, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_2, DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_3}
            ),
            new( /* DIALOG_CLOWN_EXTRAPERLO_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_CLOWN_EXTRAPERLO,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_CLOWN_EXTRAPERLO_INTRO}
            ),
            new( /* DIALOG_SILVANA_EXTRAPERLO_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_EVENT_SILVANA_REFUSED_MAINCHAR, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_2},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_0_LAUGH, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_3}
            ),
            new( /* DIALOG_SILVANA_EXTRAPERLO_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_EVENT_SILVANA_REFUSED_MAINCHAR},
            DialogType.DIALOG_SILVANA_EXTRAPERLO,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_1}
            ),
            new( /* DIALOG_SILVANA_EXTRAPERLO_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[2]{GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_2},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_2_0}
            ),
            new( /* DIALOG_SILVANA_EXTRAPERLO_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_SILVANA_EXTRAPERLO,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_0, DialogPhrase.PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_1}
            ),
            new( /* DIALOG_WAITER_W_INVITATION_OPTION_0 */
            new GameEventCombi[2]{new(GameEvent.EVENT_BEER_JAR_READY, true), new(GameEvent.EVENT_BEER_THIRD_ROUND, true)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_ANIMATE_WAITER_SERVE_JAR, GameAction.ACTION_PLAY_SOUND_DISH, GameAction.ACTION_EVENT_BEER_PLACED},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_1}
            ),
            new( /* DIALOG_WAITER_W_INVITATION_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_WAITER_EXTRAPERLO_GAVE_OLIVES, true)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_ANIMATE_WAITER_SERVE_OLIVES, GameAction.ACTION_PLAY_SOUND_DISH, GameAction.ACTION_EVENT_OLIVES_PLACED},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_0, DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_1}
            ),
            new( /* DIALOG_WAITER_W_INVITATION_OPTION_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_2_0}
            ),
            new( /* DIALOG_WAITER_W_INVITATION_OPTION_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_WAITER_W_INVITATION,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_0, DialogPhrase.PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_1}
            ),
            new( /* DIALOG_CLOWN_DRUNK_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_CLOWN_DRUNK_INTRO_1, DialogPhrase.PHRASE_DIALOG_CLOWN_DRUNK_INTRO_2}
            ),
            new( /* DIALOG_ARTURO_DRUNK_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_ARTURO_DRUNK,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_DIALOG_ARTURO_DRUNK_INTRO_1, DialogPhrase.PHRASE_DIALOG_ARTURO_DRUNK_INTRO_2}
            ),
            new( /* DIALOG_ARTURO_DRUNK_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_START_DRUNK_TEST_CARD_GAME},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_ARTURO_DRUNK_OPTION_0_0}
            ),
            new( /* DIALOG_ARTURO_DRUNK_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_MAINCHAR_GOOD_NIGHT_DRUNK}
            ),
            new( /* DIALOG_SILVANA_DRUNK_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_SILVANA_DRUNK,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_3, DialogPhrase.PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_4}
            ),
            new( /* DIALOG_SILVANA_DRUNK_OPTION_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[2]{GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_2},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_MAINCHAR_GOOD_NIGHT_DRUNK}
            ),
            new( /* DIALOG_SILVANA_OLIVE_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_EVENT_OLIVE_OFFERED, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_1, GameAction.ACTION_ANIMATE_SILVANA_OPENING_BOOK_2},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[4]{DialogPhrase.PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_3}
            ),
            new( /* DIALOG_WAITER_OPTION_TERRACE */
            new GameEventCombi[1]{new(GameEvent.EVENT_OLIVE_OFFERED, false)},
            MomentType.MOMENT_ANY,
            new GameAction[3]{GameAction.ACTION_MOVE_WAYPOINT_WAITER_TERRACE, GameAction.ACTION_EVENT_DRUNK_STATE_REMOVE, GameAction.ACTION_SCENE_EXTRAPERLO_TERRACE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[5]{DialogPhrase.PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_0, DialogPhrase.PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_1, DialogPhrase.PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_2, DialogPhrase.PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_3, DialogPhrase.PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_4}
            ),
            new( /* DIALOG_MAINCHAR_PEE_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[4]{GameAction.ACTION_MOVE_AFTER_PEE, GameAction.ACTION_ZOOM_SILVANA_SHOCKED_ZONE, GameAction.ACTION_ANIMATE_SILVANA_SHOCKED_ANGRY, GameAction.ACTION_DIALOGUE_SILVANA_OBSERVE_PEE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[2]{DialogPhrase.PHRASE_ABOUT_PEE_PLANT, DialogPhrase.PHRASE_BLANK_PEE_SOUND}
            ),
            new( /* DIALOG_SILVANA_OBSERVED_PEE_OPTION_0 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[2]{GameAction.ACTION_ZOOM_GARDEN_OUT, GameAction.ACTION_MOVE_SILVANA_GARDEN_TALK_APROACH},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_SILVANA_OBSERVED_PEE}
            ),
            new( /* DIALOG_OPTION_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false)},
            MomentType.MOMENT_ANY,
            new GameAction[1]{GameAction.ACTION_NONE},
            DialogType.DIALOG_NONE,false,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE}
            ),
            /* > ATG 2 END < */
        };

        private static readonly PhraseConfig[] _PhraseConfig = new PhraseConfig[(int)DialogPhrase.PHRASE_TOTAL]
        {
            /* > ATG 3 START < */
            new(0,GameSound.SOUND_MAINCHAR_NONSENSE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NONSENSE */ 
            new(0,GameSound.SOUND_MAINCHAR_NONSENSE_OBSERVE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NONSENSE_OBSERVE */ 
            new(0,GameSound.SOUND_MAINCHAR_NONSENSE_TALK, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NONSENSE_TALK */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NONSENSE_NOT_THOUGHT */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NONSENSE_COMBI */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_ALREADY_COMBI */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_GREAT_IDEA_COMBI */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_INTRO_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_INTRO_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_INTRO_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_1_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_1_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_1_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_2_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_REME_3_TEST */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MEMENTO_FIND_JOB_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_HIVE1_AD_BOARD_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NOT_EXIT_HIVE1_HALL_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NOT_EXIT_HIVE1_HALL_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_HIVE1_BASIN_NO_SOAP */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_HIVE1_BASIN_SOAP */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_HIVE1_PERFUME */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_HIVE1_PERFUME_NOT_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_HIVE1_PERFUME_NOT_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_CARDS_REME_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_CARDS_REME_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DECISION_NOT_SLEEP */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DECISION_SLEEP_DAY */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DECISION_SLEEP_NIGHT */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_WONT_GO_SOUTH_NEIGH */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TRY_TALK_PHARMACIST_BUSY */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_PHARMACIST_BUSY_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_PHARMACIST_BUSY_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_HELLO_DEER */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_INTRO_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_INTRO_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORK_NOTE_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORK_NOTE_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORK_NOTE_3 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORK_NOTE_4 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORKS_CITY_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_WORKS_CITY_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_4 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_5 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_MENU_DAY_6 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_CROWD_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_CROWD_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_CROWD_3 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_CROWD_4 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_CROWD_5 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_BYE_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MEMENTO_RECIPE_MISSION_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_UMBRELLA_NOT_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_UMBRELLA_TAKEN */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_MANYO_BCKG_CROWD_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_MANYO_BCKG_CROWD_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_2 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_MANYO_BCKG_CROWD_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_MANYO_BCKG_CROWD_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_POOR_MAN_WC */ 
            new(0,GameSound.SOUND_FART_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_0_0 */ 
            new(0,GameSound.SOUND_POOR_MAN_BCKG_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_1_0 */ 
            new(0,GameSound.SOUND_POOR_MAN_BCKG_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_2_0 */ 
            new(0,GameSound.SOUND_POOR_MAN_BCKG_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_BCKG_POOR_MAN_WC_OPTION_3_0 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_0_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_1_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_2_4 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_OPTION_3_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_1 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_HIVE1_POOR_MAN_WC_INTRO_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MEMENTO_POOR_MAN_WC_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MEMENTO_POOR_MAN_WC_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MEMENTO_POOR_MAN_WC_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_ROACH */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_PIPE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_ITEM_HIVE1_ROACH_HEAD */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_BACKALLEY_PIPE_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE_BOX_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_VALVE_BOX_CLOSED */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_VALVE_BOX_CLOSED_MORNING */ 
            new(0,GameSound.SOUND_OBSERVE_ITEM_SHOELACE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_SHOELACE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_NO_REASON_TO_DO */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_ITEM_HIVE1_SHOELACE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_VALVE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_SHOELACE_VALVE_BOX */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_HIVE1_VALVE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_HIVE1_VALVE_NOT */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_HIVE1_MAN_WC_CURED */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_0_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_2 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_4 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_5 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_1_6 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_OPTION_2_0 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_0 */ 
            new(1,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_HALL_INN_INTRO_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_BLURR */ 
            new(0,GameSound.SOUND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_PHARMACY_INKWELL */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_PHARMACY_INK */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_PHARMACIST_NOT_TAKE_INKWELL_INTRO */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_USE_UMBRELLA_WITH_INKWELL_OWNER_2 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_INVITATION_WITH_INK */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_USE_INVITATION_WITH_INK_ALREADY */ 
            new(0,GameSound.SOUND_FIK_1_OP_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_0_0 */ 
            new(1,GameSound.SOUND_FIK_1_OP_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_0_1 */ 
            new(0,GameSound.SOUND_FIK_1_OP_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_1_0 */ 
            new(1,GameSound.SOUND_FIK_1_OP_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_1_1 */ 
            new(0,GameSound.SOUND_FIK_1_OP_1_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_1_2 */ 
            new(1,GameSound.SOUND_FIK_1_OP_1_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_THREE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_1_3 */ 
            new(0,GameSound.SOUND_FIK_1_OP_2_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_2_0 */ 
            new(1,GameSound.SOUND_FIK_1_OP_2_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_2_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_2_2 */ 
            new(0,GameSound.SOUND_FIK_1_OP_3_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_OPTION_3_0 */ 
            new(1,GameSound.SOUND_FIK_INTRO_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_INTRO_0 */ 
            new(1,GameSound.SOUND_FIK_INTRO_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_1_INTRO_1 */ 
            new(0,GameSound.SOUND_FIK_2_OP_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_FOUR,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_0_0 */ 
            new(1,GameSound.SOUND_FIK_2_OP_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_THREE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_0_1 */ 
            new(0,GameSound.SOUND_FIK_2_OP_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_FOUR,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_1_0 */ 
            new(1,GameSound.SOUND_FIK_2_OP_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_1_1 */ 
            new(0,GameSound.SOUND_FIK_2_OP_2_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_FOUR,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_2_0 */ 
            new(1,GameSound.SOUND_FIK_2_OP_2_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_2_1 */ 
            new(0,GameSound.SOUND_FIK_2_OP_3_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_3_0 */ 
            new(1,GameSound.SOUND_FIK_2_OP_3_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_2_OPTION_3_1 */ 
            new(1,GameSound.SOUND_FIK_NOT_CROSS, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_FIK_NOT_CROSS */ 
            new(0,GameSound.SOUND_GERMAN_1_OP_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_0_0 */ 
            new(1,GameSound.SOUND_GERMAN_1_OP_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_0_1 */ 
            new(0,GameSound.SOUND_GERMAN_1_OP_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_1_0 */ 
            new(1,GameSound.SOUND_GERMAN_1_OP_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_1_1 */ 
            new(0,GameSound.SOUND_GERMAN_1_OP_1_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_1_2 */ 
            new(1,GameSound.SOUND_GERMAN_SHOUTS_COCAI, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_1_3 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_OPTION_1_4 */ 
            new(1,GameSound.SOUND_GERMAN_INTRO, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_GERMAN_1_INTRO */ 
            new(1,GameSound.SOUND_WAITER_INTRO_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_INTRO_1_0 */ 
            new(0,GameSound.SOUND_WAITER_INTRO_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_INTRO_1_1 */ 
            new(1,GameSound.SOUND_WAITER_INTRO_1_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_INTRO_1_2 */ 
            new(0,GameSound.SOUND_WAITER_INTRO_1_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_INTRO_1_3 */ 
            new(0,GameSound.SOUND_WAITER_USE_INVITATION_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_USE_OLD_INV_0 */ 
            new(1,GameSound.SOUND_WAITER_USE_INVITATION_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_USE_OLD_INV_1 */ 
            new(0,GameSound.SOUND_WAITER_USE_INVITATION_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_USE_OLD_INV_2 */ 
            new(0,GameSound.SOUND_UNKNOWN_WOMEN_INTRO_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_0 */ 
            new(1,GameSound.SOUND_UNKNOWN_WOMEN_INTRO_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_1 */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_UNKNOWN_GIRLS_INTRO_1_2 */ 
            new(0,GameSound.SOUND_TAKE_UNKNOWN_WOMEN, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_TAKE_UNKNOWN_WOMEN */ 
            new(0,GameSound.SOUND_OBSERVE_EXTRAPERLO_ARTURO, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_NPC_ARTURO_EXTRAPERLO */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_0 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_1 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_0_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_2 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_0_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_0_3 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_0 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_1 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_1_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_2 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_1_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_3 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_1_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_1_4 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_0 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_1 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_2 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_3 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_4 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_5, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_5 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_6, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_6 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_OP_2_7, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_OPTION_2_7 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_INTRO_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_EXTRAPERLO_INTRO */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_1 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_2 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_3 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_4 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_5, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_5 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_6, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_6 */ 
            new(0,GameSound.SOUND_CARDS1_TAUNT_ARTURO_WIN, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_ARTURO_TAUNT_WIN */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_TAUNT1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_1 */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_TAUNT2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_2 */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_TAUNT3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_3 */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_TAUNT4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_4 */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_TAUNT5, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_5 */ 
            new(0,GameSound.SOUND_CARDS1_MAINCHAR_WIN, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS1_MAINCHAR_TAUNT_WIN */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_2 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_3 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_1_4 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_2_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_2 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_4 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_5, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_5 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_6, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_6 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_7, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_7 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_8, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_8 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_9, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_3_9 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_2 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_OPTION_4_3 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_CLOWN_EXTRAPERLO_INTRO, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_EXTRAPERLO_INTRO */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_0 */ 
            new(2,GameSound.SOUND_CLOWN_LAUGHING, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_TWO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_0_LAUGH */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_2 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_0_3 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_1_1 */ 
            new(0,GameSound.SOUND_FIK_1_OP_3_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_OPTION_2_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_0 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_EXTRAPERLO_INTRO_1 */ 
            new(0,GameSound.SOUND_OBSERVE_INVITATION_CORNER, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_INVITATION_CORNER */ 
            new(0,GameSound.SOUND_MANIPULATE_INVITATION_CORNER, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MANIPULATE_INVITATION_CORNER */ 
            new(0,GameSound.SOUND_WAITER_USE_NEW_INVITATION_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_USE_NEW_INV_0 */ 
            new(1,GameSound.SOUND_WAITER_USE_NEW_INVITATION_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_USE_NEW_INV_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_0_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_1_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_2_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_OPTION_2_0 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_W_INVITATION_INTRO_1 */ 
            new(0,GameSound.SOUND_OBSERVE_OLIVE_BOWL, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_OBJECT_OLIVE_BOWL */ 
            new(0,GameSound.SOUND_OBSERVE_BEER, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_OBJECT_BEER_FULL */ 
            new(0,GameSound.SOUND_MAINCHAR_SECOND_BEER_COMMENT, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_COMMENT_SECOND_BEER */ 
            new(0,GameSound.SOUND_MAINCHAR_AFTER_THIRD_BEER_COMMENT, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_COMMENT_AFTER_THIRD_BEER */ 
            new(0,GameSound.SOUND_CLOWN_INTRO_DRUNK, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_DRUNK_INTRO_1 */ 
            new(1,GameSound.SOUND_CLOWN_INTRO_DRUNK_PART_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_CLOWN_DRUNK_INTRO_2 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_DRUNK_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_DRUNK_INTRO_1 */ 
            new(1,GameSound.SOUND_ARTURO_EXTRAPERLO_DRUNK_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_DRUNK_INTRO_2 */ 
            new(0,GameSound.SOUND_ARTURO_EXTRAPERLO_DRUNK_OPTION_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_ARTURO_DRUNK_OPTION_0_0 */ 
            new(0,GameSound.SOUND_MAINCHAR_GOOD_NIGHT_DRUNK, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_AUTO_STEADY,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MAINCHAR_GOOD_NIGHT_DRUNK */ 
            new(0,GameSound.SOUND_CARDS2_MAINCHAR_TAUNT1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS2_MAINCHAR_TAUNT_1 */ 
            new(0,GameSound.SOUND_CARDS2_MAINCHAR_TAUNT2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS2_MAINCHAR_TAUNT_2 */ 
            new(0,GameSound.SOUND_CARDS2_MAINCHAR_TAUNT3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS2_MAINCHAR_TAUNT_3 */ 
            new(0,GameSound.SOUND_CARDS2_MAINCHAR_TAUNT4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS2_MAINCHAR_TAUNT_4 */ 
            new(0,GameSound.SOUND_CARDS2_MAINCHAR_TAUNT5, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_CARDS2_MAINCHAR_TAUNT_5 */ 
            new(0,GameSound.SOUND_OBSERVE_ITEM_OLIVE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_PICKABLE_OLIVE */ 
            new(0,GameSound.SOUND_MAINCHAR_DONT_WANT_MORE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MAINCHAR_DONT_WANT_MORE */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_0 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_1 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_2 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_3 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_SILVANA_DRUNK_OPTION_0_4 */ 
            new(0,GameSound.SOUND_PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE}), /* PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_0 */ 
            new(2,GameSound.SOUND_PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE}), /* PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_1 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE}), /* PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_2 */ 
            new(1,GameSound.SOUND_PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE}), /* PHRASE_DIALOG_SILVANA_OLIVE_OPTION_0_3 */ 
            new(0,GameSound.SOUND_WAITER_1_OP_3_0, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_0 */ 
            new(1,GameSound.SOUND_WAITER_1_OP_3_1, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_1 */ 
            new(0,GameSound.SOUND_WAITER_1_OP_3_2, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_2 */ 
            new(1,GameSound.SOUND_WAITER_1_OP_3_3, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_3 */ 
            new(1,GameSound.SOUND_WAITER_1_OP_3_4, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE,AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_WAITER_TERRACE_OPTION_0_4 */ 
            new(0,GameSound.SOUND_MAINCHAR_NEED_ORINE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_MAINCHAR_NEED_ORINE */ 
            new(0,GameSound.SOUND_OBSERVE_INNOCENT_PLANT, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_ITEM_INNOCENT_PLANT */ 
            new(0,GameSound.SOUND_OBSERVE_BLADDER, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_OBSERVE_BLADDER */ 
            new(0,GameSound.SOUND_MAINCHAR_ABOUT_PEE_PLANT, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_ABOUT_PEE_PLANT */ 
            new(0,GameSound.SOUND_PEE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_CYCLE_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_BLANK_PEE_SOUND */ 
            new(0,GameSound.SOUND_SILVANA_OBSERVED_PEE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_TWO,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_SILVANA_OBSERVED_PEE */ 
            new(0,GameSound.SOUND_NONE, new AnimationTrigger[3]{AnimationTrigger.ANIMATION_TRIGGER_TALK_ONE,AnimationTrigger.ANIMATION_TRIGGER_ZERO,AnimationTrigger.ANIMATION_TRIGGER_ZERO}), /* PHRASE_DIALOG_LAST */ 
            /* > ATG 3 END < */
        };
    }
}