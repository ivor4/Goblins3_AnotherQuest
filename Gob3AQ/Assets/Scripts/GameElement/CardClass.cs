using Gob3AQ.VARMAP.Types.Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardClass : MonoBehaviour, IPointerClickHandler
{
    public delegate void CARD_CLICK_CALLBACK(CardClass card);

    private static readonly int ANIM_TRIGGER_FLIP = Animator.StringToHash("Tr_Flip");

    private Animator myAnimator;
    private GenericAnimBehavior animBehavior;
    private Image myImage;
    private RectTransform rectTransform;
    private bool isFrontal;
    private bool currentFlipToFrontal;
    private bool isClickable;
    private CardType cardType;
    private Sprite frontalSprite;
    private Sprite backSprite;
    private CARD_CLICK_CALLBACK onCardClickCallback;

    public CardType CardType => cardType;
    public bool IsClickable => isClickable;

    public void DoFlip(bool toFrontal)
    {
        myAnimator.SetTrigger(ANIM_TRIGGER_FLIP);
        currentFlipToFrontal = toFrontal;
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        rectTransform.anchoredPosition = position;
        rectTransform.localRotation = rotation;
    }

    public void SetFrontal(bool frontal)
    {
        isFrontal = frontal;
        currentFlipToFrontal = frontal;

        if (isFrontal)
        {
            myImage.sprite = frontalSprite;

        }
        else
        {
            myImage.sprite = backSprite;
        }
    }

    public void SetVisible(bool visible)
    {
        myImage.enabled = visible;
        isClickable = visible;
    }

    public void SetOnClickCallback(CARD_CLICK_CALLBACK callback)
    {
        onCardClickCallback = callback;
    }

    public void SetCardType(CardType cardType, Sprite spriteFrontal, Sprite spriteBack)
    {
        this.cardType = cardType;
        frontalSprite = spriteFrontal;
        backSprite = spriteBack;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            onCardClickCallback?.Invoke(this);
        }
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        animBehavior = myAnimator.GetBehaviour<GenericAnimBehavior>();
        myImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        animBehavior.SetOnStartEndCallback(OnAnimationStart, OnAnimationEnd);

        isFrontal = false;
        isClickable = false;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    private void OnAnimationStart()
    {

    }

    private void OnAnimationEnd()
    {
        SetFrontal(currentFlipToFrontal);
    }
}
