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
            new(SoundEffect.EFFECT_NONE,"MUSIC_GARDEN"), /* MUSIC_GARDEN */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL_SPANISH"), /* SOUND_OBSERVE_ITEM_EXTRAPERLO_INVITATION_DETAIL */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_OBSERVE_ITEM_SHOELACE_SPANISH"), /* SOUND_OBSERVE_ITEM_SHOELACE */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FART_1"), /* SOUND_FART_1 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_POOR_MAN_BCKG_1"), /* SOUND_POOR_MAN_BCKG_1 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_POOR_MAN_BCKG_2"), /* SOUND_POOR_MAN_BCKG_2 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_POOR_MAN_BCKG_3"), /* SOUND_POOR_MAN_BCKG_3 */ 
            new(SoundEffect.EFFECT_NONE,"MUSIC_PHARMACY"), /* MUSIC_PHARMACY */ 
            new(SoundEffect.EFFECT_NONE,"MUSIC_SOUTH_NEIGH"), /* MUSIC_SOUTH_NEIGH */ 
            new(SoundEffect.EFFECT_NONE,"MUSIC_MANYO"), /* MUSIC_MANYO */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_AMBIENCE_MANYO_NIGHT"), /* SOUND_AMBIENCE_MANYO_NIGHT */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_AMBIENCE_CITY_DAY"), /* SOUND_AMBIENCE_CITY_DAY */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_AMBIENCE_CITY_NIGHT"), /* SOUND_AMBIENCE_CITY_NIGHT */ 
            new(SoundEffect.EFFECT_NONE,"MUSIC_INN"), /* MUSIC_INN */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FIK_INTRO_1_SPANISH"), /* SOUND_FIK_INTRO_1 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FIK_INTRO_2_SPANISH"), /* SOUND_FIK_INTRO_2 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FIK_1_OP_0_1_SPANISH"), /* SOUND_FIK_1_OP_0_1 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FIK_1_OP_1_1_SPANISH"), /* SOUND_FIK_1_OP_1_1 */ 
            new(SoundEffect.EFFECT_NONE,"SOUND_FIK_1_OP_2_1_SPANISH"), /* SOUND_FIK_1_OP_2_1 */ 
            new(SoundEffect.EFFECT_NONE,""), /* SOUND_LAST */ 
            /* > ATG 1 END < */
        };
    }
}