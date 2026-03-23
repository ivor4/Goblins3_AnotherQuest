using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceAnimationsAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.VARMAP.DialogMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Gob3AQ.DialogMaster
{
    [System.Serializable]
    public class DialogMasterClass : MonoBehaviour
    {
        private enum DialogTaskType
        {
            DIALOG_STATE_NONE,
            DIALOG_STATE_STARTING,
            DIALOG_STATE_SAYING,
            DIALOG_STATE_DEAD_TIME,
            DIALOG_STATE_LAUNCH_NEXT_DIALOG
        }

        private struct AnimationRuntime
        {
            public bool isMainMode;
            public GameAnimation animation;
            public Action callback;
            public int milestoneIndex;
            public ulong prevMilestoneTimestamp;
            public bool started;

            public AnimationRuntime(bool isMainMode, GameAnimation animation, Action callback, ulong startTimestamp)
            {
                this.isMainMode = isMainMode;
                this.animation = animation;
                this.callback = callback;
                prevMilestoneTimestamp = startTimestamp;
                milestoneIndex = 0;
                started = false;
            }
        }

        private static DialogMasterClass _singleton;

        [SerializeField]
        private GameObject UICanvas;

        private UICanvasClass _uicanvas_cls;

        private GameItem[] dialog_input_talkers;
        private DialogType dialog_input_type;
        private DialogPhrase dialog_input_phrase;
        private bool dialog_input_backgroundDialog;

        private int dialog_currentPhraseIndex;
        private int dialog_totalPhrases;
        private DialogOption dialog_optionPhrases;
        private bool dialog_optionPending;
        private bool dialog_tellingInProgress;
        private bool dialog_background;
        private GameSound dialog_actualPhraseSoundStop;
        private DialogTaskType dialog_actualTaskType;
        private ulong dialog_timestamp;
        private Dictionary<DialogOption, List<byte>> dialog_randomized_left_indexes;


        /// <summary>
        /// All active animations, used to know if an animation is being played or not
        /// </summary>
        private List<AnimationRuntime> animation_activeAnimations;


        /// <summary>
        /// Displays a dialogue interface based on the specified character type, dialogue type, and initial phrase.
        /// </summary>
        /// <remarks>This method configures and displays a dialogue interface based on the provided
        /// parameters. For simple dialogues,  the specified phrase is displayed directly. For multi-choice dialogues,
        /// the available options are dynamically  populated based on the active dialogue configurations. If no valid
        /// options are found, an error is logged.</remarks>
        /// <param name="defaultTalkers">Default list of talkers (Player and ItemDest)</param>
        /// <param name="dialog">The type of dialogue to display, which determines the structure and options available.</param>
        /// <param name="phrase">The initial phrase to display if the dialogue type is simple.</param>
        /// <param name="backgroundDialog">If background does not need user interaction (as a background conversation).</param>
        public static void ShowDialogueService(DialogType dialog, DialogPhrase phrase, bool backgroundDialog)
        {
            if (_singleton != null)
            {
                if ((_singleton.dialog_actualTaskType == DialogTaskType.DIALOG_STATE_NONE) || _singleton.dialog_input_backgroundDialog)
                {
                    /* Stop previous background dialog */
                    if(_singleton.dialog_actualTaskType != DialogTaskType.DIALOG_STATE_NONE)
                    {
                        VARMAP_DialogMaster.STOP_SOUND(_singleton.dialog_actualPhraseSoundStop);
                    }

                    /* Copy default talkers to array */
                    _singleton.dialog_input_type = dialog;
                    _singleton.dialog_input_phrase = phrase;
                    _singleton.dialog_input_backgroundDialog = backgroundDialog;
                    _singleton.dialog_actualTaskType = DialogTaskType.DIALOG_STATE_STARTING;
                }
            }
        }

        public static void IsDialogActiveService(out bool active)
        {
            if (_singleton != null)
            {
                active = _singleton.dialog_actualTaskType != DialogTaskType.DIALOG_STATE_NONE;
            }
            else
            {
                active = false;
            }
        }

        public static void StartAnimationService(GameAnimation animation, bool mainMode, Action callback)
        {
            AnimationRuntime animationRuntime = new(mainMode, animation, callback, VARMAP_DialogMaster.GET_ELAPSED_TIME_MS());
            _singleton.animation_activeAnimations.Add(animationRuntime);
        }

        public static void DialogueSelectOptionService(DialogOption option, DialogPhrase phrase)
        {
            if (_singleton != null)
            {
                if ((VARMAP_DialogMaster.GET_GAMESTATUS() == Game_Status.GAME_STATUS_PLAY_DIALOG) && _singleton.dialog_optionPending)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(option);

                    _singleton.dialog_optionPending = false;

                    /* If option is permitted, show it */
                    VARMAP_DialogMaster.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);
                    MomentType currentMoment = VARMAP_DialogMaster.GET_DAY_MOMENT();


                    if (valid && ((currentMoment == dialogOptionConfig.momentType) || (dialogOptionConfig.momentType == MomentType.MOMENT_ANY)))
                    {
                        ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                        int length = dialogOptionConfig.randomized ? 1 : dialogPhrases.Length;

                        _singleton.PreloadDialogueData(option, length, false);
                        _singleton.StartPhrase(phrase);
                    }
                }
            }
        }


        private void DialogSoundEnded()
        {
            if (dialog_actualTaskType == DialogTaskType.DIALOG_STATE_DEAD_TIME)
            {
                dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                EndPhrase_Action();
            }
        }

        private void ShowDialogueExec(DialogType dialog, DialogPhrase phrase, bool background)
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
            /* This is inconditional, even if option would not be available due to Needed Events */
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
            else
            {
                selectableOptions = 0;

                ReadOnlySpan<DialogOption> dialogOptions = dialogConfig.Options;

                /* Iterate through dialog available options */
                for (int i = 0; i < dialogOptions.Length; i++)
                {
                    ref readonly DialogOptionConfig dialogOptionConfig = ref ResourceDialogsAtlasClass.GetDialogOptionConfig(dialogOptions[i]);

                    VARMAP_DialogMaster.IS_EVENT_COMBI_OCCURRED(dialogOptionConfig.ConditionEvents, out bool valid);

                    if (valid && ((currentMoment == dialogOptionConfig.momentType) || (dialogOptionConfig.momentType == MomentType.MOMENT_ANY)))
                    {
                        ReadOnlySpan<DialogPhrase> dialogPhrases = dialogOptionConfig.Phrases;
                        DialogPhrase headPhrase;

                        if (dialogOptionConfig.randomized)
                        {
                            int randomIndex = GetRandomizedOption(dialogOptions[i], in dialogOptionConfig);
                            headPhrase = dialogPhrases[randomIndex];
                            uniqueNumPhrases = 1;
                        }
                        else
                        {
                            headPhrase = dialogPhrases[0];
                            uniqueNumPhrases = dialogOptionConfig.Phrases.Length;
                        }

                        uniquePhrase = headPhrase;
                        uniqueOption = dialogOptions[i];

                        ResourceDialogsClass.GetPhraseContent(headPhrase, out PhraseContent optionPhraseContent);
                        _uicanvas_cls.ActivateDialogOption(selectableOptions, true, dialogOptions[i], headPhrase, optionPhraseContent.message);

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
            }
            else
            {
                /* Default talkers, Player and ItemDest */
                CharacterType selectedCharacter = VARMAP_DialogMaster.GET_PLAYER_SELECTED();
                GameItem selectedCharacterItem = ResourceDialogsAtlasClass.GetItemForCharacter(selectedCharacter);
                dialog_input_talkers[0] = selectedCharacterItem;
            }


            /* If it is multichoice, enable selectors. If only 1 say it directly */
            if (selectableOptions > 1)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_OPTIONS, string.Empty, string.Empty);

                dialog_optionPending = true;
                dialog_tellingInProgress = false;
            }
            else if (selectableOptions == 1)
            {
                /* Initialize phrase index */
                dialog_optionPending = false;

                PreloadDialogueData(uniqueOption, uniqueNumPhrases, background);
                StartPhrase(uniquePhrase);
            }
            else
            {
                dialog_optionPending = false;
                dialog_tellingInProgress = false;
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

        private void StartPhrase(DialogPhrase phrase)
        {
            /* Preset */
            dialog_tellingInProgress = true;

            ResourceDialogsClass.GetPhraseContent(phrase, out PhraseContent content);
            GameItem talkerItem = dialog_input_talkers[content.config.talkerIndex];
            ItemInfo talkerItemInfo = ItemsInteractionsClass.GetItemInfo(talkerItem);
            NameType talkerName = talkerItemInfo.name;

            string sender = ResourceDialogsClass.GetName(talkerName);
            string msg = content.message;

            if (content.config.sound != GameSound.SOUND_NONE)
            {
                VARMAP_DialogMaster.PLAY_SOUND(content.config.sound, DialogSoundEnded);
                dialog_actualPhraseSoundStop = content.config.sound;
            }
            else
            {
                dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            }

            if (dialog_background)
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_BACKGROUND, sender, msg);
            }
            else
            {
                _uicanvas_cls.SetDialogMode(DialogMode.DIALOG_MODE_PHRASE, sender, msg);
            }

            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_SAYING;
        }

        private void Stop_DialogAndPhrase()
        {
            dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
            VARMAP_DialogMaster.STOP_SOUND(dialog_actualPhraseSoundStop);
            dialog_actualPhraseSoundStop = GameSound.SOUND_NONE;
            dialog_tellingInProgress = false;
            dialog_optionPending = false;

            VARMAP_DialogMaster.NOTIFY_ENDED_ACTION();
        }

        private void EndPhrase_Action()
        {
            if (dialog_tellingInProgress)
            {
                dialog_tellingInProgress = false;
                ++dialog_currentPhraseIndex;
                DialogOptionConfig dialogConfig = ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);

                if (dialog_totalPhrases > dialog_currentPhraseIndex)
                {
                    /* More phrases to say, wait for user interaction */
                    StartPhrase(dialogConfig.Phrases[dialog_currentPhraseIndex]);
                }
                else
                {
                    /* If end of conversation triggers an event */
                    VARMAP_DialogMaster.PERFORM_ACTION(dialogConfig.TriggeredActions, null);

                    if (dialogConfig.dialogTriggered != DialogType.DIALOG_NONE)
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_LAUNCH_NEXT_DIALOG;
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
        }

        private int GetRandomizedOption(DialogOption option, in DialogOptionConfig dialogOptionConfig)
        {
            int optionIndex;

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
            optionIndex = leftIndexes[lastIndex];
            leftIndexes.RemoveAt(lastIndex);

            return optionIndex;
        }

        private bool ProcessAnimationRuntime(ref AnimationRuntime runtime, ulong timestamp)
        {
            bool ended;
            ref readonly AnimationConfig animationConfig = ref ResourceAnimationsAtlasClass.GetAnimationConfig(runtime.animation);
            ulong deltaTime = timestamp - runtime.prevMilestoneTimestamp;
            bool executeActions;

            ref readonly AnimationMilestoneConfig milestoneConfig = ref animationConfig.Milestones[runtime.milestoneIndex];

            if (!runtime.started)
            {
                executeActions = true;
                runtime.started = true;
                ended = false;
            }
            else
            {
                if (milestoneConfig.srcTrigger == AnimationSrcTrigger.SRC_TRIGGER_TIME_FROM_PREV)
                {
                    executeActions = deltaTime >= milestoneConfig.srcTriggerTime;
                }
                else
                {
                    /* Some callback for animation end for this case */
                    executeActions = true;
                }

                if(executeActions)
                {
                    runtime.prevMilestoneTimestamp = timestamp;
                    if (runtime.milestoneIndex == animationConfig.Milestones.Length - 1)
                    {
                        ended = true;
                    }
                    else
                    {
                        ++runtime.milestoneIndex;
                        milestoneConfig = ref animationConfig.Milestones[runtime.milestoneIndex];
                        ended = false;
                    }
                }
                else
                {
                    ended = false;
                }
            }

            if (executeActions && !ended)
            {
                foreach (AnimationActionConfig actionconfig in milestoneConfig.Actions)
                {
                    VARMAP_DialogMaster.PERFORM_ACTION(actionconfig.TriggeredActions, null);
                    /* Apply animator to dstItem */
                }
            }

            return ended;
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

                animation_activeAnimations = new(GameFixedConfig.MAX_ANIMATIONS_PERFORMING);
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
            for(int i = animation_activeAnimations.Count - 1; i >= 0; --i)
            {
                AnimationRuntime animation = animation_activeAnimations[i];
                bool ended = ProcessAnimationRuntime(ref animation, actualTimestamp);

                if (ended)
                {
                    animation.callback?.Invoke();
                    animation_activeAnimations.RemoveAt(i);
                }
                else
                {
                    animation_activeAnimations[i] = animation;
                }
            }

            /* Dialogs */
            switch (dialog_actualTaskType)
            {
                case DialogTaskType.DIALOG_STATE_STARTING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                    ShowDialogueExec(dialog_input_type, dialog_input_phrase, dialog_input_backgroundDialog);
                    break;

                case DialogTaskType.DIALOG_STATE_SAYING:
                    dialog_actualTaskType = DialogTaskType.DIALOG_STATE_DEAD_TIME;
                    dialog_timestamp = actualTimestamp;
                    break;

                case DialogTaskType.DIALOG_STATE_DEAD_TIME:
                    if ((((actualTimestamp - dialog_timestamp) >= 2000) && (dialog_actualPhraseSoundStop == GameSound.SOUND_NONE)) ||
                        (((actualTimestamp - dialog_timestamp) >= 6000) && (dialog_actualPhraseSoundStop != GameSound.SOUND_NONE))
                        )
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        EndPhrase_Action();
                    }
                    break;

                case DialogTaskType.DIALOG_STATE_LAUNCH_NEXT_DIALOG:
                    /* If previous dialog unchained some action which make new dialog options visible. Wait for Event manager to process everything */
                    if ((VARMAP_DialogMaster.GET_BUSY_STATE() & BusyState.GAME_PROCESSING_EVENTS) == 0)
                    {
                        dialog_actualTaskType = DialogTaskType.DIALOG_STATE_NONE;
                        DialogOptionConfig dialogConfig = ResourceDialogsAtlasClass.GetDialogOptionConfig(dialog_optionPhrases);
                        ShowDialogueExec(dialogConfig.dialogTriggered, DialogPhrase.PHRASE_NONE, dialog_background);
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

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        Stop_DialogAndPhrase();
                        dialog_randomized_left_indexes.Clear();
                        break;

                    case Game_Status.GAME_STATUS_PLAY:
                        Stop_DialogAndPhrase();
                        break;

                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_DialogMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_DialogMaster);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}