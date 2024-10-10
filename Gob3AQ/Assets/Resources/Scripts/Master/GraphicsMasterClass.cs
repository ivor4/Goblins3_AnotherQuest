using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;


namespace Gob3AQ.GraphicsMaster
{
    public class GraphicsMasterClass : MonoBehaviour
    {
        private static GraphicsMasterClass _singleton;
        
        private static Camera mainCamera;
        private static Transform mainCameraTransform;
        private static GameObject cursor;
        private static SpriteRenderer cursor_spr;
        private static Sprite cursor_orig_spr;


        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                cursor = transform.Find("Cursor").gameObject;
                cursor_spr = cursor.GetComponent<SpriteRenderer>();
                cursor_orig_spr = cursor_spr.sprite;
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;


            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_GraphicsMaster.REG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);

        }

        // Update is called once per frame
        void Update()
        {
            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            MousePropertiesStruct mouse = VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();

            Vector2 screenzone = new Vector2((float)mouse.posPixels.x / Screen.width, (float)mouse.posPixels.y / Screen.height);

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    MoveCursor(ref mouse.pos1);
                    FollowMouseWithCamera(ref screenzone);
                    break;
                default:
                    break;
            }
        }


        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;

                VARMAP_GraphicsMaster.UNREG_GAMESTATUS(_GameStatusChanged);
                VARMAP_GraphicsMaster.UNREG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);
            }
        }


        private void MoveCursor(ref Vector2 mousePos)
        {
            cursor.transform.position = mousePos;
        }

        private void FollowMouseWithCamera(ref Vector2 screenzone)
        {
            /* If not in menu zone */
            if(screenzone.y < GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT)
            {
                /* Expand Y of game zone to 100% */
                Vector2 szone = new Vector2(screenzone.x, screenzone.y * GameFixedConfig.GAME_ZONE_HEIGHT_FACTOR);

                
            }
        }

        private static void _OnPickedItemChanged(ChangedEventType evtype, ref GamePickableItem oldval, ref GamePickableItem newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                if (newval == GamePickableItem.ITEM_PICK_NONE)
                {
                    cursor_spr.sprite = cursor_orig_spr;
                }
                else
                {
                    cursor_spr.sprite = ResourceAtlasClass.GetSpriteFromPickableItem(newval);
                }
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, ref Game_Status oldval, ref Game_Status newval)
        {
            if(newval == Game_Status.GAME_STATUS_PAUSE)
            {
                //paused_text.gameObject.SetActive(true);
            }
            else
            {
                //paused_text.gameObject.SetActive(false);
            }
        }
    }
}
