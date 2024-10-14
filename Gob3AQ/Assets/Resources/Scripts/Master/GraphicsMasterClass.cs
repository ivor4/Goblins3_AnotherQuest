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
    [System.Serializable]
    public class GraphicsMasterClass : MonoBehaviour
    {
        [SerializeField]
        public GameObject background;

        private static SpriteRenderer background_spr;

        private static GraphicsMasterClass _singleton;

        private static Bounds _levelBounds;
        private static Bounds _cameraCenterLimitBounds;
        private static Bounds _cameraBounds;
        
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
                background_spr = background.GetComponent<SpriteRenderer>();
                _levelBounds = background_spr.bounds;

                
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;
            _cameraBounds = new Bounds();

            _cameraBounds.min = mainCamera.ScreenToWorldPoint(Vector3.zero);
            _cameraBounds.max = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            _cameraCenterLimitBounds = _levelBounds;
            _cameraCenterLimitBounds.extents -= _cameraBounds.extents;


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
            bool itemMenuOpened = VARMAP_GraphicsMaster.GET_ITEM_MENU_ACTIVE();

            /* If not in menu zone */
            if((screenzone.y < GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT)&&(!itemMenuOpened))
            {
                /* Expand Y of game zone to 100% */
                Vector2 szone = new Vector2(screenzone.x, screenzone.y * GameFixedConfig.GAME_ZONE_HEIGHT_FACTOR);

                Vector3 moveCameraDelta;
                Vector3 cameraNewPosition;

                float deltaX;
                float deltaY;

                if(szone.x < GameFixedConfig.GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR)
                {
                    deltaX = -GameFixedConfig.MOVE_CAMERA_SPEED;
                }
                else if(szone.x > GameFixedConfig.GAME_ZONE_CURSOR_MOVE_CAMERA_1MFACTOR)
                {
                    deltaX = GameFixedConfig.MOVE_CAMERA_SPEED;
                }
                else
                {
                    deltaX = 0f;
                }

                if (szone.y < GameFixedConfig.GAME_ZONE_CURSOR_MOVE_CAMERA_FACTOR)
                {
                    deltaY = -GameFixedConfig.MOVE_CAMERA_SPEED;
                }
                else if (szone.y > GameFixedConfig.GAME_ZONE_CURSOR_MOVE_CAMERA_1MFACTOR)
                {
                    deltaY = GameFixedConfig.MOVE_CAMERA_SPEED;
                }
                else
                {
                    deltaY = 0f;
                }

                /* Create movement vector and propose new camera center position */
                moveCameraDelta = new(deltaX, deltaY);
                moveCameraDelta *= Time.deltaTime;
                cameraNewPosition = mainCameraTransform.position + moveCameraDelta;

                /* Limit center of camera to stablished level bounds */
                Vector3 deltaFromMin = _cameraCenterLimitBounds.min - cameraNewPosition;
                Vector3 deltaFromMax = _cameraCenterLimitBounds.max - cameraNewPosition;

                deltaFromMin = Vector2.Max(deltaFromMin, Vector2.zero);
                deltaFromMax = Vector2.Min(deltaFromMax, Vector2.zero);

                /* Correct limited position in case deltaFromMin or deltaFromMax differ from 0 */
                cameraNewPosition += deltaFromMin + deltaFromMax;

                mainCameraTransform.position = cameraNewPosition;
            }
        }

        private static void _OnPickedItemChanged(ChangedEventType evtype, ref GameItem oldval, ref GameItem newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                if (newval == GameItem.ITEM_NONE)
                {
                    cursor_spr.sprite = cursor_orig_spr;
                }
                else
                {
                    cursor_spr.sprite = ResourceAtlasClass.GetPickableAvatarSpriteFromItem(newval);
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
