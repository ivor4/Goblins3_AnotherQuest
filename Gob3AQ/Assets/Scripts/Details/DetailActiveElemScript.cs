using UnityEngine;
using UnityEngine.EventSystems;

namespace Gob3AQ.GameMenu.DetailActiveElem
{
    public delegate void DETAIL_ACTIVE_ELEM_CLICK();
    public delegate void DETAIL_ACTIVE_ELEM_HOVER(bool enter);

    public class DetailActiveElemScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private DETAIL_ACTIVE_ELEM_CLICK _call_click;
        private DETAIL_ACTIVE_ELEM_HOVER _call_hover;

        public void OnPointerClick(PointerEventData eventData)
        {
            eventData.Use();
            _call_click?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.Use();
            _call_hover?.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            eventData.Use();
            _call_hover?.Invoke(false);
        }



        public void SetClickCall(DETAIL_ACTIVE_ELEM_CLICK call_click)
        {
            _call_click = call_click;
        }

        public void SetHoverCall(DETAIL_ACTIVE_ELEM_HOVER call_hover)
        {
            _call_hover = call_hover;
        }
    }
}
