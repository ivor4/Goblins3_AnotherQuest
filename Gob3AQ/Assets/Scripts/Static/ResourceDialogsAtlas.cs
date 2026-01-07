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
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE, }
            ),
            new( /* DIALOG_OPTION_REME_INTRO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_REME,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_INTRO_1, DialogPhrase.PHRASE_DIALOG_REME_INTRO_2, DialogPhrase.PHRASE_DIALOG_REME_INTRO_3, }
            ),
            new( /* DIALOG_OPTION_REME_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_REME,
            new DialogPhrase[3]{DialogPhrase.PHRASE_DIALOG_REME_1_1, DialogPhrase.PHRASE_DIALOG_REME_1_2, DialogPhrase.PHRASE_DIALOG_REME_1_3, }
            ),
            new( /* DIALOG_OPTION_REME_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_DIALOG_REME_2_1, }
            ),
            new( /* DIALOG_OPTION_REME_CARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[2]{DialogPhrase.PHRASE_USE_CARDS_REME_1, DialogPhrase.PHRASE_USE_CARDS_REME_2, }
            ),
            new( /* DIALOG_OPTION_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
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
            /* > ATG 3 END < */
        };
    }
}