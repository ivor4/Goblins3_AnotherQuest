using UnityEngine;

namespace Gob3AQ.VARMAP.Types
{
    public enum DialogType
    {
        /* > ATG 1 START < */
        DIALOG_NONE = -1, 
        DIALOG_SIMPLE, 
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
        PHRASE_NONSENSE_NOT_THOUGHT, 
        PHRASE_NONSENSE_COMBI, 
        PHRASE_ALREADY_COMBI, 
        PHRASE_GREAT_IDEA_COMBI, 
        PHRASE_OBSERVE_RED_POTION, 
        PHRASE_OBSERVE_FOUNTAIN, 
        PHRASE_ASK_MILITO_1, 
        PHRASE_ASK_MILITO_2, 
        PHRASE_MEMENTO_POTION_1, 
        PHRASE_MEMENTO_POTION_2, 
        PHRASE_MEMENTO_FOUNTAIN_1, 
        PHRASE_MEMENTO_FOUNTAIN_2, 
        PHRASE_MEMENTO_FOUNTAIN_3, 
        
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
