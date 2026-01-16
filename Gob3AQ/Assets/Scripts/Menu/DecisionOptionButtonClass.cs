using Gob3AQ.VARMAP.Types;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Gob3AQ.GameMenu.Decision
{
    public delegate void DECISION_OPTION_CLICK_DELEGATE(DecisionOption option);

    public class DecisionOptionButtonClass : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private DecisionOption decisionOption;
        private TMP_Text optionText;
        private GameObject optionParent;
        private DECISION_OPTION_CLICK_DELEGATE clickDelegate;

        public void SetDecisionOption(DecisionOption option)
        {
            decisionOption = option;
        }

        public void SetOptionText(in string text)
        {
            optionText.text = text;
        }

        public void SetActive(bool active)
        {
            optionText.color = Color.white;
            optionParent.SetActive(active);
        }

        public void SetClickDelegate(DECISION_OPTION_CLICK_DELEGATE dlg)
        {
            clickDelegate = dlg;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickDelegate?.Invoke(decisionOption);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            optionText.color = Color.yellow;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            optionText.color = Color.white;
        }

        private void Awake()
        {
            optionParent = transform.parent.gameObject;
            optionText = transform.parent.Find("OptionText").GetComponent<TMP_Text>();
        }

        
    }
}
