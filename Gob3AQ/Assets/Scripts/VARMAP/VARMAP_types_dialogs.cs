using UnityEngine;

namespace Gob3AQ.VARMAP.Types
{
    public enum DialogType
    {
        /* > ATG 1 START < */
        DIALOG_NONE = -1, 
        DIALOG_SIMPLE, 
        DIALOG_FOUNTAIN, 
        DIALOG_MILITO, 
        DIALOG_LAST, 
        
        DIALOG_TOTAL
        /* > ATG 1 END < */
    }

    public enum DialogOption
    {
        /* > ATG 2 START < */
        DIALOG_OPTION_NONE = -1, 
        DIALOG_OPTION_SIMPLE, 
        DIALOG_OPTION_ASK_FOUNTAIN_1, 
        DIALOG_OPTION_ASK_FOUNTAIN_2, 
        DIALOG_OPTION_ASK_FOUNTAIN_3, 
        DIALOG_OPTION_ASK_FOUNTAIN_4, 
        DIALOG_OPTION_ASK_MILITO_1, 
        DIALOG_OPTION_ASK_MILITO_2, 
        DIALOG_OPTION_LAST, 
        
        DIALOG_OPTION_TOTAL
        /* > ATG 2 END < */
    }

    public enum DialogPhrase
    {
        /* > ATG 3 START < */
        PHRASE_NONE = -1, 
        PHRASE_NONSENSE, 
        PHRASE_NONSENSE_OBSERVE, 
        PHRASE_NONSENSE_TALK, 
        PHRASE_ASK_FOUNTAIN1_1, 
        PHRASE_ASK_FOUNTAIN1_2, 
        PHRASE_ASK_FOUNTAIN2_1, 
        PHRASE_ASK_FOUNTAIN3_1, 
        PHRASE_ASK_FOUNTAIN4_1, 
        PHRASE_OBSERVE_RED_POTION, 
        PHRASE_ASK_MILITO_1, 
        PHRASE_ASK_MILITO_2, 
        
        PHRASE_TOTAL
        /* > ATG 3 END < */
    }

    public enum DialogLanguages
    {
        DIALOG_LANG_ENGLISH = 0,
        DIALOG_LANG_SPANISH,
        DIALOG_LANG_FRENCH,
        DIALOG_LANG_GERMAN,
        DIALOG_LANG_ITALIAN,
        DIALOG_LANG_PORTUGUESE,
        
        DIALOG_LANG_TOTAL
    }
}
