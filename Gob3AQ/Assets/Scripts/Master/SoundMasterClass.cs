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

        [SerializeField]
        private AudioMixerGroup reverbmixer;

        private class PooledAudioSource
        {
            public bool IsPlaying => source.isPlaying;
            public bool IsLoading => loading;
            public bool IsLoop => loop;
            public Action Callback => callback;
            public GameSound Sound => currentSound;
            private readonly AudioSource source;
            private Action callback;
            private GameSound currentSound;
            private bool loading;
            private bool loop;

            public PooledAudioSource(AudioSource source)
            {
                this.source = source;
                currentSound = GameSound.SOUND_NONE;
                loading = false;
            }

            public void PreparePlay(GameSound sound, AudioMixerGroup group, Action callback, bool loop)
            {
                this.callback = callback;
                this.loop = loop;
                loading = true;
                source.outputAudioMixerGroup = group;
                currentSound = sound;
            }

            public void ReceiveLoadedClip(AudioClip clip)
            {
                if(loading)
                {
                    source.clip = clip;
                    source.loop = loop;
                    loading = false;
                    source.Play();
                }
            }

            public void Stop()
            {
                source.Stop();
                source.clip = null;
                currentSound = GameSound.SOUND_NONE;
                callback = null;
                loading = false;
            }
        }

        private static SoundMasterClass _singleton;
        private GameSound actualMusic;
        private Queue<PooledAudioSource> availableSources;
        private List<PooledAudioSource> usedSources;
        private HashSet<GameSound> loadedSounds;
        private bool applicationPaused;


        public static void PlaySoundService(GameSound sound, Action callback, bool loop)
        {
            if(_singleton != null)
            {
                if (_singleton.availableSources.TryDequeue(out PooledAudioSource source))
                {
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
                        case SoundEffect.EFFECT_REVERB:
                            group = _singleton.reverbmixer;
                            break;
                        default:
                            group = _singleton.normalMixer;
                            break;
                    }

                    source.PreparePlay(sound, group, callback, loop);
                    _singleton.usedSources.Add(source);

                    VARMAP_SoundMaster.LOAD_ADDITIONAL_SOUND(true, sound, source.ReceiveLoadedClip);
                    _singleton.loadedSounds.Add(sound);
                }
            }
        }

        public static void StopSoundService(GameSound sound)
        {
            if ((_singleton != null) && (sound != GameSound.SOUND_NONE))
            {
                for(int i = 0; i < _singleton.usedSources.Count; ++i)
                {
                    PooledAudioSource audio = _singleton.usedSources[i];
                    if (audio.Sound == sound)
                    {
                        audio.Stop();
                        _singleton.usedSources.RemoveAt(i);
                        _singleton.availableSources.Enqueue(audio);
                        break;
                    }
                }

                if(_singleton.usedSources.Count == 0)
                {
                    _singleton.UnloadLoadedSounds();
                }
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
            loadedSounds = new(pooledSources.Length);

            applicationPaused = false;
        }

        // Start is called before the first frame update
        private void Start()
        {
            VARMAP_SoundMaster.REG_GAMESTATUS(_GameStatusChanged);
            

            for (int i = 0; i < pooledSources.Length; i++)
            {
                AudioSource source = pooledSources[i];
                if (source != null)
                {
                    availableSources.Enqueue(new PooledAudioSource(source));
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
                availableSources.Enqueue(usedSources[0]);
                usedSources.RemoveAt(0);
            }

            UnloadLoadedSounds();
        }

        private void ChangingRoom()
        {
            GameSound nextBgMusic = GetRoomMusic(VARMAP_SoundMaster.GET_ACTUAL_ROOM());

            if((nextBgMusic != actualMusic) || (nextBgMusic == GameSound.SOUND_NONE))
            {
                StopMusic();
            }

            StopAllSounds();
        }

        private GameSound GetRoomMusic(Room room)
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(VARMAP_SoundMaster.GET_ACTUAL_ROOM());

            GameSound nextBgMusic;
            MomentType actualMoment = VARMAP_SoundMaster.GET_DAY_MOMENT();

            if (roomInfo.BackgroundMusic.Length > 1)
            {
                nextBgMusic = roomInfo.BackgroundMusic[(int)actualMoment];
            }
            else
            {
                nextBgMusic = roomInfo.BackgroundMusic[0];
            }

            return nextBgMusic;
        }

        private void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
            actualMusic = GameSound.SOUND_NONE;
        }


        private void NewSceneStarted()
        {
            /* Evaluate if room has background music and if it is different from actual music */
            GameSound bgMusic = GetRoomMusic(VARMAP_SoundMaster.GET_ACTUAL_ROOM());

            if((bgMusic != GameSound.SOUND_NONE) && (bgMusic != actualMusic))
            {
                musicSource.clip = ResourceSoundsClass.GetSound(bgMusic);
                musicSource.Play();
            }

            actualMusic = bgMusic;
        }

        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
                StopAllSounds();
                StopMusic();

                VARMAP_SoundMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void OnApplicationPause(bool pause)
        {
            applicationPaused = pause;
        }

        private void Update()
        {
            if (!applicationPaused)
            {
                bool removed = false;
                for (int i = usedSources.Count - 1; i >= 0; --i)
                {
                    PooledAudioSource usedSource = usedSources[i];
                    if (!(usedSource.IsPlaying || usedSource.IsLoading))
                    {
                        usedSource.Callback?.Invoke();
                        usedSource.Stop();
                        availableSources.Enqueue(usedSource);
                        usedSources.RemoveAt(i);
                        removed = true;
                    }
                }

                if (removed && (usedSources.Count == 0))
                {
                    UnloadLoadedSounds();
                }
            }
        }

        private void UnloadLoadedSounds()
        {
            foreach(GameSound sound in loadedSounds)
            {
                VARMAP_SoundMaster.LOAD_ADDITIONAL_SOUND(false, sound, null);
            }
            loadedSounds.Clear();
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
                        break;
                    /* Not always coming from loading, could come from Inventory or Dialog */
                    case Game_Status.GAME_STATUS_PLAY:
                        if (oldval == Game_Status.GAME_STATUS_LOADING)
                        {
                            NewSceneStarted();
                        }
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
