using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.Dialog;
using Gob3AQ.GameMenu.PickableItemDisplay;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogs;
using Gob3AQ.ResourceSprites;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gob3AQ.GameMenu.UICanvas
{
    
    public class UIUserInteractionSelClass : MonoBehaviour
    {
        private const float THIRD_OF_REVOLUTION = 360f / 3f;
        private GameObject action_take;
        private GameObject action_talk;
        private GameObject action_observe;
        
        private GameObject[] action_target_lookup;
        private GameObject action_target;

        private Image action_take_img;
        private Image action_talk_img;
        private Image action_observe_img;

        private bool animationPending;
        private bool rotationDone;
        private Quaternion targetAngle;
        private float omega;
        private float elapsedTime;

        private void Awake()
        {
            action_take = transform.Find("Take").gameObject;
            action_talk = transform.Find("Talk").gameObject;
            action_observe = transform.Find("Observe").gameObject;

            action_take_img = action_take.GetComponent<Image>();
            action_talk_img = action_talk.GetComponent<Image>();
            action_observe_img = action_observe.GetComponent<Image>();

            omega = 0f;
            elapsedTime = 0f;
            animationPending = false;
            rotationDone = false;

            action_target_lookup = new GameObject[(int)UserInputInteraction.INPUT_INTERACTION_TOTAL];
            action_target_lookup[(int)UserInputInteraction.INPUT_INTERACTION_TAKE] = action_take;
            action_target_lookup[(int)UserInputInteraction.INPUT_INTERACTION_TALK] = action_talk;
            action_target_lookup[(int)UserInputInteraction.INPUT_INTERACTION_OBSERVE] = action_observe;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (animationPending)
            {
                elapsedTime += deltaTime;

                if (!rotationDone)
                {
                    if (elapsedTime >= (GameFixedConfig.USER_INTERACTION_CHANGE_ANIMATION_TIME*0.5f))
                    {
                        rotationDone = true;
                        transform.rotation = targetAngle;
                    }
                    else
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, omega * deltaTime);
                    }

                    action_target.transform.localRotation = Quaternion.Inverse(transform.rotation);
                    action_target.transform.localScale = (1f + elapsedTime/(GameFixedConfig.USER_INTERACTION_CHANGE_ANIMATION_TIME * 0.5f)) * Vector3.one;
                }
                else if(elapsedTime >= GameFixedConfig.USER_INTERACTION_CHANGE_ANIMATION_TIME)
                {
                    animationPending = false;
                    action_target.transform.localScale = Vector3.one;
                    action_target.transform.localRotation = Quaternion.identity;
                    action_target = null;
                }
                else
                {
                    /**/
                }
            }
            
            if (!animationPending)
            {
                Disable();
            }
        }

        public void AnimateNewUserInteraction(UserInputInteraction interaction)
        {
            /* In case a new animation comes before last is ended */
            if(action_target != null)
            {
                action_target.transform.localScale = Vector3.one;
                action_target.transform.localRotation = Quaternion.identity;
            }

            /* By using whole U16 instead of 360 or radians, shortest distance is implicit and no modulo operation needed */
            float fTargetAngle = (float)interaction * THIRD_OF_REVOLUTION;
            targetAngle = Quaternion.Euler(new(0, 0, fTargetAngle));
            action_target = action_target_lookup[(int)interaction];


            omega = Quaternion.Angle(transform.rotation, targetAngle) / (GameFixedConfig.USER_INTERACTION_CHANGE_ANIMATION_TIME*0.5f);
            animationPending = true;
            rotationDone = false;
            elapsedTime = 0f;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void LoadTask()
        {
            action_take_img.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TAKE);
            action_talk_img.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_TALK);
            action_observe_img.sprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_UI_OBSERVE);
        }
    }
}
