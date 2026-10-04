using Gob3AQ.GameElement.Notification;
using Gob3AQ.VARMAP.DialogMaster;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Gob3AQ.GameElement.Animation
{
    [System.Serializable]
    public class AnimationDirectorClass : MonoBehaviour, INotificationReceiver
    {
        [SerializeField]
        private GameAnimation ownedAnimation;

        private PlayableDirector director;
        private Action endedCallback;

        public void Play(Action callback)
        {
            if (director.state == PlayState.Paused)
            {
                switch (ownedAnimation)
                {
                    case GameAnimation.ANIMATION_PLAYER_POUR_GREEN_MIX:
                    case GameAnimation.ANIMATION_PLAYER_POUR_RED_MIX:
                    case GameAnimation.ANIMATION_PLAYER_GRAB_FRONT:
                    case GameAnimation.ANIMATION_ROACH_MOB:
                        VARMAP_DialogMaster.OBTAIN_SCENARIO_ITEMS(out IReadOnlyDictionary<GameItem, GameElementClass> instances);
                        GameElementClass playerInstance = instances[GameItem.ITEM_PLAYER_MAIN];

                        RebindCharacter(playerInstance.transform.parent.gameObject);
                        break;
                    default:
                        break;
                }
                director.Play();

                endedCallback = callback;
                director.stopped += AnimationEnded;
            }
        }

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            VARMAP_DialogMaster.DIRECTOR_REGISTER(ownedAnimation, this, true);
        }


        private void OnDestroy()
        {
            VARMAP_DialogMaster.DIRECTOR_REGISTER(ownedAnimation, this, false);
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            _ = context;
            _ = origin;

            if (notification is SoundMarker soundMarker)
            {
                VARMAP_DialogMaster.PLAY_SOUND(soundMarker.sound, null, false);
            }
            else if(notification is SoundStopMarker soundStopMarker)
            {
                VARMAP_DialogMaster.STOP_SOUND(soundStopMarker.sound);
            }
            else if(notification is ZoomMarker zoomMarker)
            {
                GameObject foundZoomObject = GameObject.Find(zoomMarker.zoomObjectName);
                if (foundZoomObject)
                {
                    Bounds bounds = foundZoomObject.GetComponent<BoxCollider2D>().bounds;
                    VARMAP_GameEventMaster.ACTIVATE_FORCED_ZOOM_MODE(true, false, bounds);
                }
                else
                {
                    Debug.LogError($"Zoom object {zoomMarker.zoomObjectName} not found");
                }
            }
            /* Animation continues but callback is called now */
            else if(notification is AnimationPrematureEndMarker)
            {
                endedCallback?.Invoke();
                endedCallback = null;
                director.stopped -= AnimationEnded;
            }
        }

        private void AnimationEnded(PlayableDirector dir)
        {
            director.stopped -= AnimationEnded;

            endedCallback?.Invoke();
            endedCallback = null;
        }

        private void RebindCharacter(GameObject instantiatedCharacter)
        {
            foreach (var output in director.playableAsset.outputs)
            {
                if (output.streamName.Contains("PlayerAnimation"))
                {
                    Animator characterAnimator = instantiatedCharacter.GetComponent<Animator>();
                    if (characterAnimator != null)
                    {
                        director.SetGenericBinding(output.sourceObject, characterAnimator);
                    }
                    else
                    {
                        Debug.LogError($"Could not find Animator component on instantiated character: {instantiatedCharacter.name}");
                    }
                    break;
                }
            }
        }
    }
}
