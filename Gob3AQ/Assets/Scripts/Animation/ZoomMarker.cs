using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gob3AQ.GameElement.Notification
{
    public class ZoomMarker : Marker, INotification
    {
        public PropertyName id => new PropertyName("ZoomMarker");
        public string zoomObjectName;
    }
}
