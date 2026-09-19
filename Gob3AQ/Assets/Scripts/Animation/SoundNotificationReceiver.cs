using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using UnityEngine;
using UnityEngine.Playables;

namespace Gob3AQ.GameElement.Notification
{
    public class SoundNotificationReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            _ = context;
            _ = origin;

            if (notification is SoundMarker soundMarker)
            {
                VARMAP_ItemMaster.PLAY_SOUND(soundMarker.sound, null, false);
            }
        }
    }
}