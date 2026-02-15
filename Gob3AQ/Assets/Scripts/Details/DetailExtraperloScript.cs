using UnityEngine;
using UnityEngine.UI;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.GameMenu.DetailActiveElem;

public class DetailExtraperloScript : MonoBehaviour
{
    private GameObject forward;
    private GameObject backward;
    private Button flipButton;
    private bool isFlipped;
    private DetailActiveElemScript activeElemScript;

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

    private void ObserveElemClick()
    {
        Debug.Log("Click");
    }

    private void ObserveElemHover(bool enter)
    {
        Debug.Log("Hover: " + enter);
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        forward.SetActive(!isFlipped);
        backward.SetActive(isFlipped);
    }
}
