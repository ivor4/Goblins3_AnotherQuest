using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using Gob3AQ.Libs.Arith;
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
        private static readonly Color DETAIL_SHOWN = new(1.0f, 1.0f, 1.0f, 1.0f);
        private static readonly Color DETAIL_HIDDEN = new(1.0f, 1.0f, 1.0f, 0.1f);

        private GameObject forward;
        private GameObject forward_folded;
        private GameObject backward;
        private Button flipButton;
        private Image activeElemImage;
        private bool isFlipped;
        private DetailActiveElemScript inscriptionScript;
        private DISPLAYED_ITEM_CLICK itemClickAction;
        private DISPLAYED_ITEM_HOVER itemHoverAction;
        private DetailActiveElemScript[] activeCornerScripts;

        private bool doFolded;

        void Start()
        {
            activeCornerScripts = new DetailActiveElemScript[4];

            isFlipped = false;
            forward = transform.Find("FORWARD").gameObject;
            forward_folded = transform.Find("FORWARD_FOLDED").gameObject;
            backward = transform.Find("BACKWARD").gameObject;
            flipButton = transform.Find("FlipButton").GetComponent<Button>();
            inscriptionScript = backward.transform.Find("ObserveElem/Active").GetComponent<DetailActiveElemScript>();
            activeElemImage = backward.transform.Find("ObserveElem").GetComponent<Image>();

            activeCornerScripts[0] = forward.transform.Find("BackgroundUnfolded/Corner1").GetComponent<DetailActiveElemScript>();
            activeCornerScripts[1] = forward.transform.Find("BackgroundUnfolded/Corner2").GetComponent<DetailActiveElemScript>();
            activeCornerScripts[2] = forward.transform.Find("BackgroundUnfolded/Corner3").GetComponent<DetailActiveElemScript>();
            activeCornerScripts[3] = forward.transform.Find("BackgroundUnfolded/Corner4").GetComponent<DetailActiveElemScript>();

            Span<GameEventCombi> span = RentedSpan<GameEventCombi>.GetSpanOfSize(1);
            span[0] = new(GameEvent.EVENT_EXTRAPERLO_SHOWN_OLD_INVITATION, false);

            VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(span, out bool occurred);

            if(occurred)
            {
                foreach(var activeCorner in activeCornerScripts)
                {
                    activeCorner.SetClickCall(ObserveElemClick);
                    activeCorner.SetHoverCall(ObserveElemHover);
                }
            }

            flipButton.onClick.AddListener(Flip);

            forward.SetActive(true);
            forward_folded.SetActive(false);
            backward.SetActive(false);

            inscriptionScript.SetClickCall(ObserveElemClick);
            inscriptionScript.SetHoverCall(ObserveElemHover);

            RefreshActiveElemDetail();

            doFolded = false;
        }

        void Update()
        {
            if (!doFolded)
            {
                Span<GameEventCombi> span = RentedSpan<GameEventCombi>.GetSpanOfSize(1);
                span[0] = new(GameEvent.EVENT_EXTRAPERLO_INV_FOLDED_CORNERS, false);

                VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(span, out bool occurred);

                if (occurred)
                {
                    forward.SetActive(false);
                    forward_folded.SetActive(true);
                    backward.SetActive(false);

                    flipButton.gameObject.SetActive(false);

                    itemHoverAction(GameItem.ITEM_NONE, false);

                    doFolded = true;
                }
            }
        }

        private void ObserveElemClick(GameItem item)
        {
            itemClickAction(item);
        }

        private void RefreshActiveElemDetail()
        {
            Span<GameEventCombi> gameEventCombi = RentedSpan<GameEventCombi>.GetSpanOfSize(1);
            gameEventCombi[0] = new(GameEvent.EVENT_INVITATION_REVEALED, false);
            VARMAP_GameMenu.IS_EVENT_COMBI_OCCURRED(gameEventCombi, out bool occurred);


            activeElemImage.color = occurred ? DETAIL_SHOWN : DETAIL_HIDDEN;
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