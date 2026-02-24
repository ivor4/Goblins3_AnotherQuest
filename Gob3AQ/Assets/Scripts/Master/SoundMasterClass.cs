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
    public class SoundMasterClass : MonoBehaviour
    {
        private static SoundMasterClass _singleton;
        private bool newRoomPending;
        private AudioSource audioSource;
        private GameSound actualMusic;


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

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            actualMusic = GameSound.SOUND_NONE;
        }

        // Start is called before the first frame update
        private void Start()
        {
            VARMAP_SoundMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_SoundMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_SoundMaster);
            newRoomPending = true;
        }

        private void ChangingRoom()
        {
            ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(VARMAP_SoundMaster.GET_ACTUAL_ROOM());

            if((roomInfo.backgroundMusic != actualMusic)||(roomInfo.backgroundMusic == GameSound.SOUND_NONE))
            {
                audioSource.Stop();
                audioSource.clip = null;
                actualMusic = GameSound.SOUND_NONE;
            }
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
                    audioSource.clip = ResourceSoundsClass.GetSound(bgMusic);
                    audioSource.Play();
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
                    default:
                        break;
                }
            }
        }
    }
}
