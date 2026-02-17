using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gob3AQ.GameMenu.DetailActiveElem
{
    public interface IDetailScript
    {
        public void SetItemClickAction(DISPLAYED_ITEM_CLICK action);
        public void SetItemHoverAction(DISPLAYED_ITEM_HOVER action);

    }
    public class DetailExtraperloScript : MonoBehaviour, IDetailScript
    {
        private GameObject forward;
        private GameObject backward;
        private Button flipButton;
        private bool isFlipped;
        private DetailActiveElemScript activeElemScript;
        private DISPLAYED_ITEM_CLICK itemClickAction;
        private DISPLAYED_ITEM_HOVER itemHoverAction;

        void Start()
        {
            isFlipped = false;
            forward = transform.Find("FORWARD").gameObject;
            backward = transform.Find("BACKWARD").gameObject;
            flipButton = transform.Find("FlipButton").GetComponent<Button>();
            activeElemScript = backward.transform.Find("ObserveElem").GetComponent<DetailActiveElemScript>();

            flipButton.onClick.AddListener(Flip);

            forward.SetActive(true);
            backward.SetActive(false);

            activeElemScript.SetClickCall(ObserveElemClick);
            activeElemScript.SetHoverCall(ObserveElemHover);
        }

        private void ObserveElemClick(GameItem item)
        {
            itemClickAction(item);
        }

        private void ObserveElemHover(GameItem item, bool enter)
        {
            itemHoverAction(item, enter);
        }

        private void Flip()
        {
            isFlipped = !isFlipped;
            forward.SetActive(!isFlipped);
            backward.SetActive(isFlipped);
        }

        public void SetItemClickAction(DISPLAYED_ITEM_CLICK action)
        {
            itemClickAction = action;
        }

        public void SetItemHoverAction(DISPLAYED_ITEM_HOVER action)
        {
            itemHoverAction = action;
        }
    }
}