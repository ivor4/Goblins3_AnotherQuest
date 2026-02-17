using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.GameMenu.DetailActiveElem
{
    [System.Serializable]
    public class DetailActiveElemScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private GameItem item;

        private DISPLAYED_ITEM_CLICK _call_click;
        private DISPLAYED_ITEM_HOVER _call_hover;

        public void OnPointerClick(PointerEventData eventData)
        {
            eventData.Use();
            _call_click?.Invoke(item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.Use();
            _call_hover?.Invoke(item, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            eventData.Use();
            _call_hover?.Invoke(item, false);
        }



        public void SetClickCall(DISPLAYED_ITEM_CLICK call_click)
        {
            _call_click = call_click;
        }

        public void SetHoverCall(DISPLAYED_ITEM_HOVER call_hover)
        {
            _call_hover = call_hover;
        }
    }
}
