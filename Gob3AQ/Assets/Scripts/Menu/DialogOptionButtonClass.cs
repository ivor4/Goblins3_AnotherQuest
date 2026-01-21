using Gob3AQ.VARMAP.Types;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Gob3AQ.GameMenu.Dialog
{
    public delegate void DIALOG_OPTION_CLICK_DELEGATE(DialogOption option, DialogPhrase phrase);

    public class DialogOptionButtonClass : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private DialogOption dialogOption;
        private DialogPhrase dialogPhrase;
        private TMP_Text optionText;
        private GameObject optionParent;
        private DIALOG_OPTION_CLICK_DELEGATE clickDelegate;

        public void SetDialogOption(DialogOption option)
        {
            dialogOption = option;
        }

        public void SetDialogPhrase(DialogPhrase phrase, string text)
        {
            dialogPhrase = phrase;
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
            clickDelegate?.Invoke(dialogOption, dialogPhrase);
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
