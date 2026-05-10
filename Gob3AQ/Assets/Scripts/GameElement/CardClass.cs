using Gob3AQ.VARMAP.Types.Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Gob3AQ.GameElement.Item.Card
{

    public class CardClass : MonoBehaviour, IPointerClickHandler
    {
        public delegate void CARD_CLICK_CALLBACK(CardClass card);


        private Image myImage;
        private RectTransform rectTransform;
        private Sprite frontalSprite;
        private Sprite backSprite;
        private CARD_CLICK_CALLBACK onCardClickCallback;

        /* ANIMATION VARIABLES */
        private bool isAnimatingPosition;
        private bool isAnimatingRotation;
        private bool isAnimatingFlip;
        private Vector2 startPos;
        private Vector2 targetPos;
        private Quaternion startRot;
        private Quaternion targetRot;
        private float animPosDuration;
        private float animPosElapsed;
        private float animRotDuration;
        private float animRotElapsed;
        private float animFlipDuration;
        private float animFlipElapsed;
        private bool flipTargetState;
        

        public bool IsClickable { get; private set; }
        private bool isFaceUp;
        public Vector2 AnchoredPosition => rectTransform.anchoredPosition;
        public Quaternion ActualQuaternion => rectTransform.localRotation;

        public bool IsAnimating => isAnimatingPosition || isAnimatingRotation || isAnimatingFlip;

        public void DoFlip(bool toFrontal, float duration)
        {
            flipTargetState = toFrontal;

            if (duration <= 0f)
            {
                isAnimatingFlip = false;
                isFaceUp = toFrontal;
                SetFrontalInternal(toFrontal);
            }
            else
            {
                isAnimatingFlip = true;
                animFlipDuration = duration;
                animFlipElapsed = 0f;
            }
        }

        public void SetPositionAndRotation(Vector2 position, Quaternion rotation)
        {
            rectTransform.anchoredPosition = position;
            rectTransform.localRotation = rotation;
            isAnimatingPosition = false;
            isAnimatingRotation = false;
        }

        public void SetTargetPositionAndRotation(Vector2 finalPos, Quaternion finalRotation, float motionTime)
        {
            if (motionTime <= 0f)
            {
                SetPositionAndRotation(finalPos, finalRotation);
            }
            else
            {
                startPos = rectTransform.anchoredPosition;
                targetPos = finalPos;
                animPosDuration = motionTime;
                animPosElapsed = 0f;
                isAnimatingPosition = true;

                startRot = rectTransform.localRotation;
                targetRot = finalRotation;
                animRotDuration = motionTime;
                animRotElapsed = 0f;
                isAnimatingRotation = true;
            }
        }

        public void SetFrontalAndStopMotion(bool faceUp)
        {
            isFaceUp = faceUp;

            myImage.sprite = faceUp ? frontalSprite : backSprite;

            rectTransform.localScale = Vector3.one;
            isAnimatingFlip = false;
            isAnimatingPosition = false;
            isAnimatingRotation = false;
        }

        public void SetVisible(bool visible)
        {
            myImage.enabled = visible;
            IsClickable = visible;
        }

        public void SetOnClickCallback(CARD_CLICK_CALLBACK callback)
        {
            onCardClickCallback = callback;
        }

        public void SetSprites(Sprite spriteFrontal, Sprite spriteBack)
        {
            frontalSprite = spriteFrontal;
            backSprite = spriteBack;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onCardClickCallback?.Invoke(this);
            }
        }

        private void Awake()
        {
            myImage = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            
            isFaceUp = false;
            IsClickable = false;
            isAnimatingPosition = false;
            isAnimatingRotation = false;
            isAnimatingFlip = false;
        }

        public void DoAnimationStep()
        {
            if (isAnimatingPosition)
            {
                animPosElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(animPosElapsed / animPosDuration);
                // Smoothstep
                t = t * t * (3f - 2f * t);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

                if (animPosElapsed >= animPosDuration)
                {
                    isAnimatingPosition = false;
                    rectTransform.anchoredPosition = targetPos;
                }
            }

            if (isAnimatingRotation)
            {
                animRotElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(animRotElapsed / animRotDuration);
                // Smoothstep
                t = t * t * (3f - 2f * t);
                rectTransform.localRotation = Quaternion.Slerp(startRot, targetRot, t);

                if (animRotElapsed >= animRotDuration)
                {
                    isAnimatingRotation = false;
                    rectTransform.localRotation = targetRot;
                }
            }

            if (isAnimatingFlip)
            {
                animFlipElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(animFlipElapsed / animFlipDuration);

                // We flip by scaling X from 1 to 0, swapping sprite, then scaling X from 0 to 1
                float scaleX;
                if (t < 0.5f)
                {
                    // First half: 1 to 0
                    scaleX = Mathf.Lerp(1f, 0f, t * 2f);
                    if (isFaceUp == flipTargetState) // If current state is not the "back" of the target state
                    {
                        // Ensure we show the sprite that corresponds to the "back" of the target state
                        // e.g., if flipping to frontal, show back sprite during first half
                        // if flipping to back, show frontal sprite during first half (this is the current state)
                        SetFrontalInternal(flipTargetState);
                        isFaceUp = flipTargetState;
                    }
                }
                else
                {
                    // Second half: 0 to 1
                    scaleX = Mathf.Lerp(0f, 1f, (t - 0.5f) * 2f);
                    if (isFaceUp != flipTargetState) // If current state is not the target state
                    {
                        // Swap sprite in the middle
                        SetFrontalInternal(flipTargetState);
                        isFaceUp = flipTargetState;
                    }
                }

                rectTransform.localScale = new Vector3(scaleX, 1f, 1f);

                if (animFlipElapsed >= animFlipDuration)
                {
                    isAnimatingFlip = false;
                    rectTransform.localScale = Vector3.one; // Ensure scale is reset to 1
                    SetFrontalInternal(flipTargetState); // Ensure final sprite is set
                    isFaceUp = flipTargetState; // Update the actual face-up state
                }
            }
        }

        private void SetFrontalInternal(bool frontal)
        {
            // This only controls which sprite is shown, not the logical face-up state
            myImage.sprite = frontal ? frontalSprite : backSprite;
        }
    }
}
