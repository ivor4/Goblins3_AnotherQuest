using Gob3AQ.GameElement.Notification;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System;
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
            VARMAP_ItemMaster.DIRECTOR_REGISTER(ownedAnimation, this, true);
        }


        private void OnDestroy()
        {
            VARMAP_ItemMaster.DIRECTOR_REGISTER(ownedAnimation, this, false);
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            _ = context;
            _ = origin;

            if (notification is SoundMarker soundMarker)
            {
                VARMAP_ItemMaster.PLAY_SOUND(soundMarker.sound, null, false);
            }
            else if(notification is SoundStopMarker soundStopMarker)
            {
                VARMAP_ItemMaster.STOP_SOUND(soundStopMarker.sound);
            }
        }

        private void AnimationEnded(PlayableDirector dir)
        {
            director.stopped -= AnimationEnded;

            endedCallback?.Invoke();
        }
    }
}
