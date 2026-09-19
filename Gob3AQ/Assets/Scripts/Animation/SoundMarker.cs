using Gob3AQ.VARMAP.Types;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gob3AQ.GameElement.Notification
{
    public class SoundMarker : Marker, INotification
    {
        public PropertyName id => new PropertyName("SoundMarker");
        public GameSound sound;
    }
}
