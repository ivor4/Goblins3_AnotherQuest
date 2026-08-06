using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;

public class SoundEventClass : MonoBehaviour
{
    public void PlayAnimationSound(GameSound sound)
    {
        VARMAP_ItemMaster.PLAY_SOUND(sound, null, false);
    }
}
