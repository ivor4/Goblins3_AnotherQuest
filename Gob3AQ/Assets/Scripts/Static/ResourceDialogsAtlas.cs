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
        public static ReadOnlySpan<PhraseConfig> PhraseConfigs => _PhraseConfig;





        private static readonly DialogConfig[] _DialogConfig = new DialogConfig[(int)DialogType.DIALOG_TOTAL]
        {
            /* > ATG 1 START < */
            new( /* DIALOG_SIMPLE */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[1]{DialogOption.DIALOG_OPTION_SIMPLE, }
            ),
            
            new( /* DIALOG_FOUNTAIN */
            new GameItem[1]{GameItem.ITEM_NONE,},
            new DialogOption[4]{DialogOption.DIALOG_OPTION_ASK_FOUNTAIN_1, DialogOption.DIALOG_OPTION_ASK_FOUNTAIN_2, DialogOption.DIALOG_OPTION_ASK_FOUNTAIN_3, DialogOption.DIALOG_OPTION_ASK_FOUNTAIN_4, }
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
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE, }
            ),
            new( /* DIALOG_OPTION_ASK_FOUNTAIN_1 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[2]{DialogPhrase.PHRASE_ASK_FOUNTAIN1_1, DialogPhrase.PHRASE_ASK_FOUNTAIN1_2, }
            ),
            new( /* DIALOG_OPTION_ASK_FOUNTAIN_2 */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_ASK_FOUNTAIN2_1, }
            ),
            new( /* DIALOG_OPTION_ASK_FOUNTAIN_3 */
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, false), },
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_ASK_FOUNTAIN3_1, }
            ),
            new( /* DIALOG_OPTION_ASK_FOUNTAIN_4 */
            new GameEventCombi[1]{new(GameEvent.EVENT_FOUNTAIN_FULL, true), },
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_ASK_FOUNTAIN4_1, }
            ),
            new( /* DIALOG_OPTION_LAST */
            new GameEventCombi[1]{new(GameEvent.EVENT_NONE, false), },
            GameEvent.EVENT_NONE, DialogType.DIALOG_NONE,
            new DialogPhrase[1]{DialogPhrase.PHRASE_NONE, }
            ),
            /* > ATG 2 END < */
        };

        private static readonly PhraseConfig[] _PhraseConfig = new PhraseConfig[(int)DialogPhrase.PHRASE_TOTAL]
        {
            /* > ATG 3 START < */
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_NONSENSE */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ASK_FOUNTAIN1_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ASK_FOUNTAIN1_2 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ASK_FOUNTAIN2_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ASK_FOUNTAIN3_1 */ 
            new(0,0, DialogAnimation.DIALOG_ANIMATION_TALK), /* PHRASE_ASK_FOUNTAIN4_1 */ 
            /* > ATG 3 END < */
        };
    }
}