using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.ResourceSoundsAtlas
{
    public static class ResourceSoundsAtlasClass
    {
        public static ref readonly SoundConfig GetSoundConfig(GameSound sound)
        {
            if ((uint)sound < (uint)GameSound.SOUND_TOTAL)
            {
                return ref _SoundConfig[(int)sound];
            }
            else
            {
                Debug.LogError($"Trying to get SoundConfig for invalid sound {sound}");
                return ref SoundConfig.EMPTY;
            }
        }


        private static readonly SoundConfig[] _SoundConfig = new SoundConfig[(int)GameSound.SOUND_TOTAL]
        {
            /* > ATG 1 START < */
            new(""), /* SOUND_LAST */ 
            /* > ATG 1 END < */
        };
    }
}