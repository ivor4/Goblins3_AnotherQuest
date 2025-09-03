using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Gob3AQ.GameMenu.Dialog
{

    [System.Serializable]
    public class DialogOptionButtonClass : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public delegate void DIALOG_OPTION_CLICK_DELEGATE(int optionIndex);

        [SerializeField]
        private int optionIndex;

        private TMP_Text optionText;
        private GameObject optionParent;
        private DIALOG_OPTION_CLICK_DELEGATE clickDelegate;



        public void SetOptionText(in string text)
        {
            optionText.text = text;
        }

        public void SetActive(bool active)
        {
            optionText.color = Color.white;
            optionParent.SetActive(active);
        }

        public void SetClickDelegate(DIALOG_OPTION_CLICK_DELEGATE dlg)
        {
            clickDelegate = dlg;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickDelegate?.Invoke(optionIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            optionText.color = Color.yellow;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            optionText.color = Color.white;
        }

        void Awake()
        {
            optionParent = transform.parent.gameObject;
            optionText = transform.parent.Find("OptionText").GetComponent<TMP_Text>();
        }

        
    }
}
