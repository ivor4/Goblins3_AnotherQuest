using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.GameMenu;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gob3AQ.VARMAP.GameMenu;
using System.Runtime.InteropServices;
using System;

namespace Gob3AQ.GameMenu.PushNotif
{
    public class PushNotifClass : MonoBehaviour
    {
        private enum NotifState
        {
            NOTIF_STATE_STOPPED,
            NOTIF_STATE_READY,
            NOTIF_STATE_APPEARING,
            NOTIF_STATE_SHOWING,
            NOTIF_STATE_DISAPPEARING,
            NOTIF_STATE_NEXT
        }
        private static readonly Color color_white_transparent = new(1f, 1f, 1f, 0f);
        private static readonly Color color_black_transparent = new(0f, 0f, 0f, 0f);

        private TMP_Text pushNotif_text;
        private Image pushNotif_icon;
        private Image pushNotif_bckg;
        private Queue<PushNotificationInfo> notifQueue;
        private NotifState notifState;
        private ulong startStepTimestamp;

        public void AddNotification(in PushNotificationInfo info)
        {
            notifQueue.Enqueue(info);

            /* Start if machine was stopped */
            if(notifState == NotifState.NOTIF_STATE_STOPPED)
            {
                gameObject.SetActive(true);

                _ = notifQueue.Dequeue();
                StartPushCycle(in info);
            }
        }

        private void Awake()
        {
            pushNotif_bckg = transform.Find("Notif").GetComponent<Image>();
            pushNotif_icon = pushNotif_bckg.transform.Find("Icon").GetComponent<Image>();
            pushNotif_text = pushNotif_bckg.transform.Find("Text").GetComponent<TMP_Text>();

            notifQueue = new(GameFixedConfig.MAX_QUEUED_PUSH_NOTIFS);
            notifState = NotifState.NOTIF_STATE_STOPPED;

            pushNotif_bckg.color = color_white_transparent;
            pushNotif_icon.color = color_white_transparent;
            pushNotif_text.text = string.Empty;
            pushNotif_text.color = color_black_transparent;
        }

        private void Update()
        {
            ulong delta = VARMAP_GameMenu.GET_ELAPSED_TIME_MS() - startStepTimestamp;

            switch (notifState)
            {
                case NotifState.NOTIF_STATE_STOPPED:
                    gameObject.SetActive(false);
                    break;

                case NotifState.NOTIF_STATE_READY:
                    startStepTimestamp = VARMAP_GameMenu.GET_ELAPSED_TIME_MS();
                    notifState = NotifState.NOTIF_STATE_APPEARING;
                    break;

                case NotifState.NOTIF_STATE_APPEARING:
                    {
                        float percent = Mathf.Clamp((float)delta / GameFixedConfig.PUSH_NOTIF_APPEAR_MS, 0f, 1f);

                        Color mix = Color.Lerp(color_black_transparent, Color.black, percent);
                        pushNotif_text.color = mix;

                        mix = Color.Lerp(color_white_transparent, Color.white, percent);
                        pushNotif_icon.color = mix;
                        pushNotif_bckg.color = mix;

                        if (delta >= GameFixedConfig.PUSH_NOTIF_APPEAR_MS)
                        {
                            notifState = NotifState.NOTIF_STATE_SHOWING;
                            VARMAP_GameMenu.PLAY_SOUND(GameSound.SOUND_ANY_ITEM_TAKE, null, false);
                            startStepTimestamp += delta;    /* Put timestamp in NOW */
                        }
                    }
                    break;

                case NotifState.NOTIF_STATE_SHOWING:
                    if (delta >= GameFixedConfig.PUSH_NOTIF_STAY_MS)
                    {
                        notifState = NotifState.NOTIF_STATE_DISAPPEARING;
                        startStepTimestamp += delta;
                    }
                    break;

                case NotifState.NOTIF_STATE_DISAPPEARING:
                    {
                        float percent = Mathf.Clamp((float)delta / GameFixedConfig.PUSH_NOTIF_APPEAR_MS, 0f, 1f);

                        Color mix = Color.Lerp(Color.black, color_black_transparent, percent);
                        pushNotif_text.color = mix;

                        mix = Color.Lerp(Color.white, color_white_transparent, percent);
                        pushNotif_icon.color = mix;
                        pushNotif_bckg.color = mix;

                        if (delta >= GameFixedConfig.PUSH_NOTIF_APPEAR_MS)
                        {
                            notifState = NotifState.NOTIF_STATE_NEXT;
                        }
                    }
                    break;

                default:
                    if(notifQueue.TryDequeue(out PushNotificationInfo info))
                    {
                        StartPushCycle(in info);
                    }
                    else
                    {
                        notifState = NotifState.NOTIF_STATE_STOPPED;
                    }
                    break;
            }
        }

        private void StartPushCycle(in PushNotificationInfo info)
        {
            notifState = NotifState.NOTIF_STATE_READY;
            pushNotif_text.text = info.message;
            pushNotif_text.color = color_black_transparent;

            Sprite iconSprite;

            switch (info.notifType)
            {
                case PushNotificationType.PUSH_NOTIFICATION_EARN_ITEM:
                    iconSprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_PUSH_OBTAIN_ITEM);
                    break;
                case PushNotificationType.PUSH_NOTIFICATION_LOSE_ITEM:
                    iconSprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_PUSH_LOSE_ITEM);
                    break;
                case PushNotificationType.PUSH_NOTIFICATION_EARN_MEMENTO:
                    iconSprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_PUSH_OBTAIN_MEMENTO);
                    break;
                default:
                    iconSprite = ResourceSpritesClass.GetSprite(GameSprite.SPRITE_ICON_PUSH_COMPLETE_MEMENTO);
                    break;
            }

            pushNotif_icon.sprite = iconSprite;
            pushNotif_icon.color = color_white_transparent;
            pushNotif_bckg.color = color_white_transparent;
        }
    }
}