using Gob3AQ.VARMAP.Types;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gob3AQ.GameElement.Notification
{ 
    public class ChangeSortingEmiter : Marker, INotification
    {
        public PropertyName id => new PropertyName("SortingLayerMarker");
        public NotifSortingLayer sortingLayer;
    }
}
