using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Gob3AQ.GameMenu.Dialog
{

    [System.Serializable]
    public class DialogOptionButtonClass : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private int optionIndex;

        private TMP_Text optionText;

        public void SetOptionText(string text)
        {
            optionText.text = text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("DialogOptionButtonClass: OnPointerClick - optionIndex: " + optionIndex);
            throw new System.NotImplementedException();
        }

        void Awake()
        {
            optionText = transform.parent.Find("OptionText").GetComponent<TMP_Text>();
        }
    }
}
