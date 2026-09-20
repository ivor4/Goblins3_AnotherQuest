using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Animation;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.DialogMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Gob3AQ.DialogMaster
{
    [Serializable]
    public class DialogMasterClass : MonoBehaviour
    {
        private enum DialogTaskType
        {
            DIALOG_STATE_NONE,
            DIALOG_STATE_STARTING,
            DIALOG_STATE_WAIT_ANIMATION_START,
            DIALOG_STATE_SAYING,
            DIALOG_STATE_DEAD_TIME,
            DIALOG_STATE_LAUNCH_NEXT_DIALOG
        }

        private enum AnimationTaskType
        {
            ANIMATION_STATE_NONE,
            ANIMATION_STATE_STARTING,
            ANIMATION_STATE_PERFORMING,
            ANIMATION_STATE_WAITING_FOR_END
        }


       

        private static DialogMasterClass _singleton;

        [SerializeField]
        private GameObject UICanvas;

        private UICanvasClass _uicanvas_cls;
        private Dictionary<GameAnimation, AnimationDirectorClass> animation_directors;
        private GameAnimation animation_pendingStart;
        private GameAnimation animation_actual_performing;
        private AnimationTaskType animation_actualTaskType;

        private GameItem[] dialog_input_talkers;
        private int dialog_input_numTalkers;
        private DialogType dialog_input_type;
        private DialogOption dialog_input_dialogOption;
        private DialogPhrase dialog_input_phrase;
        private GameItem dialog_input_forcedSingleTalker;
        private bool dialog_input_backgroundDialog;

        private int dialog_currentPhraseIndex;
        private int dialog_totalPhrases;
        private DialogPhrase dialog_phrasePendingAfterAnimation;
        private bool dialog_waitAnimationCompleted;
        private DialogOption dialog_optionPhrases;
        private bool dialog_optionPending;
        private bool dialog_background;
        private GameSound dialog_actualPhraseSoundStop;
        private DialogTaskType dialog_actualTaskType;
        private ulong dialog_timestamp;
        private Dictionary<DialogOption, List<byte>> dialog_randomized_left_indexes;




        /// <summary>
        /// Requests the display of a dialogue interface.
        /// </summary>
        /// <remarks>
        /// This method schedules a dialogue to start. If a background dialogue is already running, it will be interrupted.
        /// The actual processing occurs in the next Update cycle via the DIALOG_STATE_STARTING state.
        /// </remarks>
        /// <param name="dialog">The type of dialogue to display, which determines the structure and options available.</param>
        /// <param name="option">The option to display for the dialogue. (Optional)</param>
        /// <param name="phrase">The initial phrase to display if the dialogue type is simple.</param>
        /// <param name="forcedSingleTalker">An optional specific item to act as the speaker, overriding defaults.</param>
        /// <param name="backgroundDialog">If true, the dialogue plays without blocking user interaction.</param>
        public static void ShowDialogueService(DialogType dialog, DialogOption option, DialogPhrase phrase, GameItem forcedSingleTalker, bool backgroundDialog)
        {
            if (!_singleton) return;

            if ((_singleton.dialog_actualTaskType != DialogTaskType.DIALOG_STATE_NONE) &&
                !_singleton.dialog_input_backgroundDialog) return;
            
            /* Stop previous background dialog */
            if(_singleton.dialog_actualTaskType != DialogTaskType.DIALOG_STATE_NONE)
            {
                VARMAP_DialogMaster.STOP_SOUND(_singleton.dialog_actualPhraseSoundStop);
                _singleton.dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            }

            /* Copy default talkers to array */
            _singleton.dialog_input_type = dialog;
            _singleton.dialog_input_dialogOption = option;
            _singleton.dialog_input_phrase = phrase;
            _singleton.dialog_input_forcedSingleTalker = forcedSingleTalker;
            _singleton.dialog_input_backgroundDialog = backgroundDialog;
            _singleton.dialog_actualTaskType = DialogTaskType.DIALOG_STATE_STARTING;
        }

        public static void IsDialogActiveService(out bool active)
        {
            if (_singleton)
            {
                active = _singleton.dialog_actualTaskType != DialogTaskType.DIALOG_STATE_NONE;
            }
            else
            {
                active = false;
            }
        }

        public static void DirectorRegisterService(GameAnimation animation, AnimationDirectorClass director, bool add)
        {
            if (!_singleton) return;

            if (add)
            {
                _singleton.animation_directors.Add(animation, director);
            }
            else
            {
                _singleton.animation_directors.Remove(animation);
            }
        }

        public static void StartAnimationService(GameAnimation animation)
        {
            if (!_singleton) return;

            if (_singleton.animation_actualTaskType == AnimationTaskType.ANIMATION_STATE_NONE)
            {
                _singleton.animation_pendingStart = animation;
                _singleton.animation_actualTaskType = AnimationTaskType.ANIMATION_STATE_STARTING;
            }
            else
            {
                Debug.LogError($"Already performing animation (req: {animation}), actual: {_singleton.animation_actual_performing}");
            }
        }

        public static void DialogueSelectOptionService(DialogOption option, DialogPhrase phrase)
        {
            if (!_singleton) return;
            
            if ((VARMAP_DialogMaster.GET_GAMESTATUS() != Game_Status.GAME_STATUS_PLAY_DIALOG) ||
                !_singleton.dialog_optionPending) return;
            
            ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(option);

            _singleton.dialog_optionPending = false;

            /* If option is permitted, show it */
            VARMAP_DialogMaster.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);
            MomentType currentMoment = VARMAP_DialogMaster.GET_DAY_MOMENT();


            if (!valid || ((currentMoment != dialogOptionConfig.momentType) &&
                           (dialogOptionConfig.momentType != MomentType.MOMENT_ANY))) return;
            
            ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
            int length = dialogOptionConfig.randomized ? 1 : dialogPhrases.Length;

            _singleton.PreloadDialogueData(option, length, false);
            _singleton.PreparePhrase(phrase);
        }


        private void DialogSoundEnded()
        {
            dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
        }

        private void AnimationStarted()
        {
            dialog_waitAnimationCompleted = true;
        }

        private void ShowDialogueExec(DialogType dialog, DialogOption forcedOption, DialogPhrase phrase, GameItem forcedSingleTalker,bool background, bool dialogStart)
        {
            int selectableOptions;

            DialogPhrase uniquePhrase = DialogPhrase.PHRASE_NONE;
            DialogOption uniqueOption = DialogOption.DIALOG_OPTION_NONE;
            int uniqueNumPhrases = 0;
            MomentType currentMoment = VARMAP_DialogMaster.GET_DAY_MOMENT();

            ref readonly DialogConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogConfig(dialog);


            if (dialog == DialogType.DIALOG_SIMPLE)
            {
                uniquePhrase = phrase;
                uniqueOption = DialogOption.DIALOG_OPTION_SIMPLE;
                selectableOptions = 1;
                uniqueNumPhrases = 1;
            }
            /* A dialog with only one option should be given. But, in case, take always first option for background mode */
            /* This is unconditional, even if option would not be available due to Needed Events */
            else if (background)
            {
                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;
                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[0]);

                ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                DialogPhrase headPhrase;

                if (dialogOptionConfig.randomized)
                {
                    int randomIndex = GetRandomizedOption(dialogOptions[0], in dialogOptionConfig);
                    headPhrase = dialogPhrases[randomIndex];
                    uniqueNumPhrases = 1;
                }
                else
                {
                    headPhrase = dialogPhrases[0];
                    uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                }

                uniquePhrase = headPhrase;
                uniqueOption = dialogOptions[0];

                selectableOptions = 1;
            }
            else if(forcedOption != DialogOption.DIALOG_OPTION_NONE)
            {
                ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(forcedOption);

                ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                DialogPhrase headPhrase;

                if (dialogOptionConfig.randomized)
                {
                    int randomIndex = GetRandomizedOption(forcedOption, in dialogOptionConfig);
                    headPhrase = dialogPhrases[randomIndex];
                    uniqueNumPhrases = 1;
                }
                else
                {
                    headPhrase = dialogPhrases[0];
                    uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                }

                uniquePhrase = headPhrase;
                uniqueOption = forcedOption;

                selectableOptions = 1;
            }
            else
            {
                selectableOptions = 0;

                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                foreach (var dialogOption in dialogOptions)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOption);

                    VARMAP_DialogMaster.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);

                    if (valid && ((currentMoment == dialogOptionConfig.momentType) || (dialogOptionConfig.momentType == MomentType.MOMENT_ANY)))
                    {
                        ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                        DialogPhrase headPhrase;

                        if (dialogOptionConfig.randomized)
                        {
                            int randomIndex = GetRandomizedOption(dialogOption, in dialogOptionConfig);
                            headPhrase = dialogPhrases[randomIndex];
                            uniqueNumPhrases = 1;
                        }
                        else
                        {
                            headPhrase = dialogPhrases[0];
                            uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                        }

                        uniquePhrase = headPhrase;
                        uniqueOption = dialogOption;

                        ResourceDialogsClass.GetPhraseContent(headPhrase, out PhraseContent optionPhraseContent);
                        _uicanvas_cls.ActivateDialogOption(selectableOptions, true, dialogOption, headPhrase, optionPhraseContent.message);

                        ++selectableOptions;
                    }
                }

                /* Clear previous usage data and deactivate */
                for (int i = selectableOptions; i < GameFixedConfig.MAX_DIALOG_OPTIONS; ++i)
                {
                    _uicanvas_cls.ActivateDialogOption(i, false, DialogOption.DIALOG_OPTION_NONE, DialogPhrase.PHRASE_NONE, string.Empty);
                }
            }



            /* Chose between default talkers or imposed */

            if (dialogConfig.Talkers[0] != GameItem.ITEM_NONE)
            {
                dialogConfig.Talkers.CopyTo(dialog_input_talkers);
                dialog_input_numTalkers = dialogConfig.Talkers.Length;
            }
            else
            {
                GameItem selectedCharacterItem;
                
                /* Default talker, Player */
                if (forcedSingleTalker == GameItem.ITEM_NONE)
                {
                    CharacterType selectedCharacter = VARMAP_DialogMaster.GET_PLAYER_SELECTED();
                    selectedCharacterItem = ResourceDialogsAtlasClass.GetItemForCharacter(selectedCharacter);
                }
                else
                {
                    selectedCharacterItem = forcedSingleTalker;
                }
                
                dialog_input_talkers[0] = selectedCharacterItem;
                dialog_input_numTalkers = 1;
            }

            /* Zoom service (if not background) */
            if((!background) && dialogStart && (dialog_input_numTalkers > 0))
            {
                VARMAP_DialogMaster.GET_ITEM_SPRITE_BOUNDARIES(dialog_input_talkers[0], out var zoomBounds);


                for (int i = 1; i < dialog_input_numTalkers; ++i)
                {
                    VARMAP_DialogMaster.GET_ITEM_SPRITE_BOUNDARIES(dialog_input_talkers[i], out Bounds itemBounds);
                    zoomBounds.Encapsulate(itemBounds);
                }
                
                VARMAP_DialogMaster.ACTIVATE_FORCED_ZOOM_MODE(true, true, zoomBounds);
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectableOptions > 1)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_OPTIONS, string.Empty, string.Empty);

                dialog_optionPending = true;
            }
            else if (selectableOptions == 1)
            {
                /* Initialize phrase index */
                dialog_optionPending = false;

                PreloadDialogueData(uniqueOption, uniqueNumPhrases, background);
                PreparePhrase(uniquePhrase);
            }
            else
            {
                dialog_optionPending = false;
                Debug.LogError("GameMenuClass.ShowDialogueService: No valid dialog options found for dialog " + dialog.ToString());
            }
        }

        private void PreloadDialogueData(DialogOption option, int totalPhrases, bool background)
        {
            dialog_optionPhrases = option;
            dialog_totalPhrases = totalPhrases;
            dialog_currentPhraseIndex = 0;
            dialog_background = background;
        }

        private void PreparePhrase(DialogPhrase phrase)
        {
            ResourceDialogsClass.GetPhraseContent(phrase, out PhraseContent content);
            dialog_phrasePendingAfterAnimation = phrase;
            
            Action startAnimationCallback = null;
            bool startCallbackGiven = false;
            dialog_waitAnimationCompleted = true;

            if (!dialog_background)
            {
                for (int i = 0; i < content.config.AnimationTrigger.Length; ++i)
                {
                    AnimationTrigger trigger = content.config.AnimationTrigger[i];
                    if (trigger == AnimationTrigger.ANIMATION_TRIGGER_ZERO) continue;

                    if ((startAnimationCallback == null) && (!startCallbackGiven) &&
                       (content.config.talkerIndex == i))
                    {
                        startAnimationCallback = AnimationStarted;
                        startCallbackGiven = true;
                        dialog_waitAnimationCompleted = false;
                    }
                    else if (startCallbackGiven)
                    {
                        startAnimationCallback = null;
                    }

                    VARMAP_DialogMaster.ITEM_PERFORM_ANIMATION(dialog_input_talkers[i],
                        trigger, startAnimationCallback, null, false, null, false);
                }
            }

            _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_NONE, string.Empty, string.Empty);

            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_WAIT_ANIMATION_START;
        }

        private void StartPhrase(DialogPhrase phrase)
        {
            ResourceDialogsClass.GetPhraseContent(phrase, out PhraseContent content);
            GameItem talkerItem = dialog_input_talkers[content.config.talkerIndex];
            ItemInfo talkerItemInfo = ItemsInteractionsClass.GetItemInfo(talkerItem);
            NameType talkerName = talkerItemInfo.name;

            string sender = ResourceDialogsClass.GetName(talkerName);
            string msg = content.message;

            if (content.config.sound != GameSound.SOUND_NONE)
            {
                VARMAP_DialogMaster.PLAY_SOUND(content.config.sound, DialogSoundEnded, false);
                dialog_actualPhraseSoundStop = content.config.sound;
            }
            else
            {
                dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            }
            
            _uicanvas_cls.SetDialogMode(
                dialog_background ? DialogMode.DIALOG_MODE_BACKGROUND : DialogMode.DIALOG_MODE_PHRASE, sender, msg);

            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_SAYING;
        }

        private void Stop_DialogAndPhrase()
        {
            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
            VARMAP_DialogMaster.STOP_SOUND(dialog_actualPhraseSoundStop);
            dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            dialog_optionPending = false;

            VARMAP_DialogMaster.ACTIVATE_FORCED_ZOOM_MODE(false, false, default);
            Stop_DialogTalkersAnimation();
            

            VARMAP_DialogMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_DIALOG);
        }

        private void Stop_DialogTalkersAnimation()
        {
            if (!dialog_background)
            {
                for (int i = 0; i < dialog_input_numTalkers; ++i)
                {
                    VARMAP_DialogMaster.ITEM_PERFORM_ANIMATION(dialog_input_talkers[i], AnimationTrigger.ANIMATION_TRIGGER_AUTO_STEADY, null, null, false, null, false);
                }
            }

            dialog_input_numTalkers = 0;
        }



        private void EndPhrase_Action()
        {
            VARMAP_DialogMaster.STOP_SOUND(dialog_actualPhraseSoundStop);
            dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            
            ++dialog_currentPhraseIndex;
            ref readonly DialogOptionConfig dialogConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);

            if (dialog_totalPhrases > dialog_currentPhraseIndex)
            {
                /* More phrases to say, wait for user interaction */
                PreparePhrase(dialogConfig.Phrases[dialog_currentPhraseIndex]);
            }
            else
            {
                /* If end of conversation triggers an event */
                VARMAP_DialogMaster.PERFORM_ACTION(dialogConfig.TriggeredActions, null);

                if (dialogConfig.dialogTriggered != DialogType.DIALOG_NONE)
                {
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_LAUNCH_NEXT_DIALOG;
                    Stop_DialogTalkersAnimation();
                }
                else
                {
                    /* End of dialog */
                    _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_NONE, string.Empty, string.Empty);

                    if (!dialog_background)
                    {
                        VARMAP_DialogMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                    }

                    Stop_DialogAndPhrase();
                }
            }
        }

        private int GetRandomizedOption(DialogOption option, in DialogOptionConfig dialogOptionConfig)
        {
            if (!dialog_randomized_left_indexes.TryGetValue(option, out List<byte> leftIndexes))
            {
                List<byte> newList = new(dialogOptionConfig.Phrases.Length);
                dialog_randomized_left_indexes[option] = newList;
                leftIndexes = newList;
            }

            if (leftIndexes.Count == 0)
            {
                /* Refill and reshuffle */
                for (byte i = 0; i < dialogOptionConfig.Phrases.Length; i++)
                {
                    int j = UnityEngine.Random.Range(0, 2);

                    if (j == 0)
                    {
                        leftIndexes.Insert(0, i);
                    }
                    else
                    {
                        leftIndexes.Add(i);
                    }
                }
            }
            int lastIndex = leftIndexes.Count - 1;
            int optionIndex = leftIndexes[lastIndex];
            leftIndexes.RemoveAt(lastIndex);

            return optionIndex;
        }

        


        private void Awake()
        {
            if (_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                VARMAP_DialogMaster.REG_GAMESTATUS(_GameStatusChanged);

                dialog_input_talkers = new GameItem[GameFixedConfig.MAX_DIALOG_TALKERS];
                dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                dialog_randomized_left_indexes = new(GameFixedConfig.MAX_RANDOMIZED_DIALOGS_PER_SCENE);
                animation_directors = new(GameFixedConfig.MAX_ANIMATIONS_PERFORMING);
            }
        }

        private void Start()
        {
            _uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();
            VARMAP_DialogMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_DialogMaster);
        }

        private void Update()
        {
            ulong actualTimestamp = VARMAP_DialogMaster.GET_ELAPSED_TIME_MS();

            /* Animations */
            switch(animation_actualTaskType)
            {
                case AnimationTaskType.ANIMATION_STATE_STARTING:
                    {
                        AnimationDirectorClass director = animation_directors[animation_pendingStart];
                        animation_actual_performing = animation_pendingStart;
                        animation_pendingStart = GameAnimation.ANIMATION_NONE;
                        animation_actualTaskType = AnimationTaskType.ANIMATION_STATE_PERFORMING;

                        director.Play(AnimationEndedCallback);

                        VARMAP_DialogMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY_ANIMATION, out _);
                    }
                    break;

                case AnimationTaskType.ANIMATION_STATE_WAITING_FOR_END:
                    animation_actualTaskType = AnimationTaskType.ANIMATION_STATE_NONE;
                    animation_actual_performing = GameAnimation.ANIMATION_NONE;
                    VARMAP_DialogMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_ANIMATION);

                    VARMAP_DialogMaster.CHANGE_GAME_MODE(Game_Status.GAME_STATUS_PLAY, out _);
                    break;

                default:
                    break;
            }

            /* Dialogs */
            switch (dialog_actualTaskType)
            {
                case DialogTaskType.DIALOG_STATE_STARTING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                    ShowDialogueExec(dialog_input_type, dialog_input_dialogOption, dialog_input_phrase, dialog_input_forcedSingleTalker, dialog_input_backgroundDialog, true);
                    break;
                case DialogTaskType.DIALOG_STATE_WAIT_ANIMATION_START:
                    {
                        if (!dialog_waitAnimationCompleted)
                        {
                            break;
                        }

                        dialog_waitAnimationCompleted = false;
                        StartPhrase(dialog_phrasePendingAfterAnimation);
                        break;
                    }

                case DialogTaskType.DIALOG_STATE_SAYING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_DEAD_TIME;
                    dialog_timestamp = actualTimestamp;
                    break;

                case DialogTaskType.DIALOG_STATE_DEAD_TIME:
                    {
                        ref readonly KeyStruct ks = ref VARMAP_DialogMaster.GET_PRESSED_KEYS();

                        if (ks.isKeyCyclePressed(KeyFunctions.KEYFUNC_SKIPDIALOG))
                        {
                            dialog_currentPhraseIndex = dialog_totalPhrases - 1;
                            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                            EndPhrase_Action();
                        }
                        else if ((((actualTimestamp - dialog_timestamp) >= 2000) && (dialog_actualPhraseSoundStop == GameSound.SOUND_NONE)) ||
                            (((actualTimestamp - dialog_timestamp) >= 14000) && (dialog_actualPhraseSoundStop != GameSound.SOUND_NONE))
                            )
                        {
                            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                            EndPhrase_Action();
                        }
                        break;
                    }

                case DialogTaskType.DIALOG_STATE_LAUNCH_NEXT_DIALOG:
                    /* If previous dialog unchained some action which make new dialog options visible. Wait for Event manager to process everything */
                    if ((VARMAP_DialogMaster.GET_BUSY_STATE() & BusyState.GAME_PROCESSING_EVENTS) == 0)
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        DialogOptionConfig dialogConfig = ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);
                        ShowDialogueExec(dialogConfig.dialogTriggered, DialogOption.DIALOG_OPTION_NONE, DialogPhrase.PHRASE_NONE, GameItem.ITEM_NONE, dialog_background, false);
                    }
                    break;

                default:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                    break;
            }
        }


        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
                VARMAP_DialogMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void AnimationEndedCallback()
        {
            if(animation_actualTaskType == AnimationTaskType.ANIMATION_STATE_PERFORMING)
            {
                animation_actualTaskType = AnimationTaskType.ANIMATION_STATE_WAITING_FOR_END;
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_STOPPED:
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        animation_directors.Clear();
                        animation_actual_performing = GameAnimation.ANIMATION_NONE;
                        animation_pendingStart = GameAnimation.ANIMATION_NONE;
                        animation_actualTaskType = AnimationTaskType.ANIMATION_STATE_NONE;

                        Stop_DialogAndPhrase();
                        dialog_randomized_left_indexes.Clear();
                        break;

                    case Game_Status.GAME_STATUS_PLAY_CARDS:
                        Stop_DialogAndPhrase();
                        dialog_randomized_left_indexes.Clear();
                        break;

                    case Game_Status.GAME_STATUS_PLAY:
                        Stop_DialogAndPhrase();
                        break;

                    case Game_Status.GAME_STATUS_LOADING:
                        _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_NONE, string.Empty, string.Empty);
                        VARMAP_DialogMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_DialogMaster);
                        break;
                    
                }
            }
        }
    }
}