using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSounds;
using Gob3AQ.ResourceSoundsAtlas;
using Gob3AQ.VARMAP.SoundMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Gob3AQ.SoundMaster
{
    [Serializable]
    public class SoundMasterClass : MonoBehaviour
    {
        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource[] pooledSources;

        [SerializeField]
        private AudioMixerGroup normalMixer;

        [SerializeField]
        private AudioMixerGroup echoMixer;

        [SerializeField]
        private AudioMixerGroup chorusMixer;

        [SerializeField]
        private AudioMixerGroup musicMixer;

        private class PooledAudioSource
        {
            public bool IsPlaying => source.isPlaying;
            public Action Callback => callback;
            private readonly AudioSource source;
            private readonly List<PooledAudioSource> usedList;
            private readonly Queue<PooledAudioSource> availableQueue;
            private Action callback;
            
            public PooledAudioSource(AudioSource source, List<PooledAudioSource> usedList, Queue<PooledAudioSource> availableQueue)
            {
                this.source = source;
                this.usedList = usedList;
                this.availableQueue = availableQueue;
            }

            public void Play(AudioClip clip, AudioMixerGroup group, Action callback)
            {
                this.callback = callback;
                source.clip = clip;
                source.outputAudioMixerGroup = group;
                source.Play();
            }

            public void Stop()
            {
                source.Stop();
                source.clip = null;
                callback = null;
                usedList.Remove(this);
                availableQueue.Enqueue(this);
            }
        }

        private static SoundMasterClass _singleton;
        private bool newRoomPending;
        private GameSound actualMusic;
        private Queue<PooledAudioSource> availableSources;
        private List<PooledAudioSource> usedSources;


        public static void PlaySoundService(GameSound sound, Action callback, out Action stopAction)
        {
            if(_singleton != null)
            {
                if(_singleton.availableSources.TryDequeue(out PooledAudioSource source))
                {
                    AudioClip clip = ResourceSoundsClass.GetSound(sound);
                    ref readonly SoundConfig soundConfig = ref ResourceSoundsAtlasClass.GetSoundConfig(sound);

                    AudioMixerGroup group;

                    switch(soundConfig.effect)
                    {
                        case SoundEffect.EFFECT_ECHO:
                            group = _singleton.echoMixer;
                            break;
                        case SoundEffect.EFFECT_CHORUS:
                            group = _singleton.chorusMixer;
                            break;
                        default:
                            group = _singleton.normalMixer;
                            break;
                    }

                    source.Play(clip, group, callback);
                    _singleton.usedSources.Add(source);
                    stopAction = source.Stop;
                }
                else
                {
                    stopAction = null;
                }
            }
            else
            {
                stopAction = null;
            }
        }

        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
            }

            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = musicMixer;
            actualMusic = GameSound.SOUND_NONE;

            availableSources = new(pooledSources.Length);
            usedSources = new(pooledSources.Length);
        }

        // Start is called before the first frame update
        private void Start()
        {
            VARMAP_SoundMaster.REG_GAMESTATUS(_GameStatusChanged);
            
            newRoomPending = true;

            for (int i = 0; i < pooledSources.Length; i++)
            {
                AudioSource source = pooledSources[i];
                if (source != null)
                {
                    availableSources.Enqueue(new PooledAudioSource(source, usedSources, availableSources));
                }
                else
                {
                    Debug.LogError($"Pooled source at index {i} does not have an AudioSource component.");
                }
            }

            StopAllSounds();
            StopMusic();

            VARMAP_SoundMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_SoundMaster);
        }

        private void StopAllSounds()
        {
            while(usedSources.Count > 0)
            {
                usedSources[0].Stop();
            }
        }

        private void ChangingRoom()
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(VARMAP_SoundMaster.GET_ACTUAL_ROOM());

            if((roomInfo.backgroundMusic != actualMusic)||(roomInfo.backgroundMusic == GameSound.SOUND_NONE))
            {
                StopMusic();
            }

            StopAllSounds();
        }

        private void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
            actualMusic = GameSound.SOUND_NONE;
        }


        private void ChangedToPlayMode()
        {
            /* Evaluate if room has background music and if it is different from actual music */
            if(newRoomPending)
            {
                ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(VARMAP_SoundMaster.GET_ACTUAL_ROOM());
                GameSound bgMusic = roomInfo.backgroundMusic;

                if((bgMusic != GameSound.SOUND_NONE) && (bgMusic != actualMusic))
                {
                    musicSource.clip = ResourceSoundsClass.GetSound(bgMusic);
                    musicSource.Play();
                }

                actualMusic = bgMusic;
            }

            newRoomPending = false;
        }

        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;

                VARMAP_SoundMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void Update()
        {
            for(int i=0; i < usedSources.Count;++i)
            {
                PooledAudioSource usedSource = usedSources[i];
                if (!usedSource.IsPlaying)
                {
                    usedSource.Callback?.Invoke();
                    usedSource.Stop();
                    --i;
                }
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                switch (newval)
                {
                    /* In this moment, actual room is next one. If bgMusic is not actual, stop sound */
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        ChangingRoom();
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_SoundMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_SoundMaster);
                        newRoomPending = true;
                        break;
                    /* Not always coming from loading, could come from Inventory or Dialog */
                    case Game_Status.GAME_STATUS_PLAY:
                        ChangedToPlayMode();
                        break;
                    case Game_Status.GAME_STATUS_STOPPED:
                        StopAllSounds();
                        StopMusic();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
