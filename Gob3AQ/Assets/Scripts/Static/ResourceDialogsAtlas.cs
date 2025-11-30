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
            
            new( /* DIALOG_WITNESS1 */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_WITNESS1_INNOCENT, }
            ),
            
            new( /* DIALOG_WITNESS2 */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_WITNESS2_GUARDS, }
            ),
            
            new( /* DIALOG_WITNESS3_PROLOGUE */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_WITNESS3_PROLO, }
            ),
            
            new( /* DIALOG_WITNESS3_MAIN */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[2]{DialogOption.DIALOG_OPTION_WITNESS3_MAIN_1, DialogOption.DIALOG_OPTION_WITNESS3_MAIN_2, }
            ),
            
            new( /* DIALOG_WITNESS3_SPOON */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[2]{DialogOption.DIALOG_OPTION_WITNESS3_SPOON, DialogOption.DIALOG_OPTION_WITNESS3_SPOON_2, }
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
            new( /* DIALOG_OPTION_WITNESS1_INNOCENT */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_ROOM1_AWARE_CHASE_1, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[4]{DialogPhrase.PHRASE_ROOM1_WITNESS1_INNOCENT0, DialogPhrase.PHRASE_ROOM1_WITNESS1_INNOCENT1, DialogPhrase.PHRASE_ROOM1_WITNESS1_INNOCENT2, DialogPhrase.PHRASE_ROOM1_WITNESS1_INNOCENT3, }
            ),
            new( /* DIALOG_OPTION_WITNESS2_GUARDS */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_ROOM1_AWARE_CHASE_2, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[2]{DialogPhrase.PHRASE_ROOM1_WITNESS2_GUARDS0, DialogPhrase.PHRASE_ROOM1_WITNESS2_GUARDS1, }
            ),
            new( /* DIALOG_OPTION_WITNESS3_PROLO */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_WITNESS3_MAIN,
            new DialogPhrase[1]{DialogPhrase.PHRASE_ROOM1_WITNESS3_PROLO, }
            ),
            new( /* DIALOG_OPTION_WITNESS3_MAIN_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[2]{DialogPhrase.PHRASE_ROOM1_WITNESS3_MAIN_1_1, DialogPhrase.PHRASE_ROOM1_WITNESS3_MAIN_1_2, }
            ),
            new( /* DIALOG_OPTION_WITNESS3_MAIN_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[2]{DialogPhrase.PHRASE_ROOM1_WITNESS3_MAIN_2_1, DialogPhrase.PHRASE_ROOM1_WITNESS3_MAIN_2_2, }
            ),
            new( /* DIALOG_OPTION_WITNESS3_SPOON */
            new GameEventCombi[2]{new(GameEvent.EVENT_ROOM1_SPOON_WITH_POISON_TAKEN, false), new(GameEvent.EVENT_ROOM1_OLD_KEY_TAKEN, true), },
            new GameEventCombi[1]{new(GameEvent.EVENT_ROOM1_OLD_KEY_TAKEN, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[5]{DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_1, DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_2, DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_3, DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_4, DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_5, }
            ),
            new( /* DIALOG_OPTION_WITNESS3_SPOON_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_ROOM1_OLD_KEY_TAKEN, false), },
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_ROOM1_WITNESS3_SPOON_2_1, }
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
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_MEMENTO_VICTIM_CASE_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE), /* PHRASE_MEMENTO_VICTIM_CASE_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_OBSERVE_VICTIM */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS1_INNOCENT0 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS1_INNOCENT1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS1_INNOCENT2 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS1_INNOCENT3 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS2_GUARDS0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS2_GUARDS1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_CHASE_0 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_MEMENTO_CHASE_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_NOT_CROSS_DOOR */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_NOT_CROSS_MAIN_DOOR */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_SPOON */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_PROLO */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_MAIN_1_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_MAIN_1_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_MAIN_2_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_MAIN_2_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_1 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_3 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_4 */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_5 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_SPOON_POISON */ 
            new(1,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_WITNESS3_SPOON_2_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_USE_KEY_WINDOW */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_TROWEL */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_DOOR_LOCKED */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_CAR_DOOR_OPENED */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_MATCHES */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_DARKNESS */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_FIRE_CANDLE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ROOM1_TAKE_LOCKPICK */ 
            /* > ATG 3 END < */
        };
    }
}