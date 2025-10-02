using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.ResourceSpritesAtlas;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gob3AQ.GraphicsMaster
{
    [System.Serializable]
    public class GraphicsMasterClass : MonoBehaviour
    {
        [SerializeField]
        private GameObject background;

        [SerializeField]
        private GameObject UICanvas;

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
        private static Game_Status cachedGameStatus;

        

        private static GameObject UICanvas_loadingObj;
        private static GameObject UICanvas_dialogObj;
        private static GameObject UICanvas_itemMenuObj;

        private static bool _loaded;




        

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

                UICanvas_loadingObj = UICanvas.transform.Find("LoadingObj").gameObject;
                UICanvas_dialogObj = UICanvas.transform.Find("DialogObj").gameObject;
                UICanvas_itemMenuObj = UICanvas.transform.Find("ItemMenuObj").gameObject;
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

            cachedGameStatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            _loaded = false;
        }

        // Update is called once per frame
        void Update()
        {
            ref readonly MousePropertiesStruct mouse = ref VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();

            Vector2 screenzone = new Vector2(mouse.posPixels.x / Screen.safeArea.width, mouse.posPixels.y / Screen.safeArea.height);

            MoveCursor(in mouse.pos1);

            switch (cachedGameStatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading();
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    FollowMouseWithCamera(in screenzone);
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

        private static void Execute_Loading()
        {
            if (!_loaded)
            {
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_LevelMaster, out bool levelmasterLoaded);
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_PlayerMaster, out bool playermasterLoaded);

                if (levelmasterLoaded && playermasterLoaded)
                {
                    VARMAP_GraphicsMaster.GET_PLAYER_LIST(out ReadOnlySpan<PlayableCharScript> playerlist);
                    Vector3 candidatePos = mainCameraTransform.position;

                    for (int i = 0; i < playerlist.Length; i++)
                    {
                        if (playerlist[i] != null)
                        {
                            candidatePos = playerlist[i].transform.position;
                            break;
                        }
                    }

                    candidatePos.z = mainCameraTransform.position.z;

                    MoveCameraToPosition(in candidatePos);

                    VARMAP_GraphicsMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GraphicsMaster);
                    _loaded = true;
                }
            }
        }

        private void MoveCursor(in Vector2 mousePos)
        {
            cursor.transform.position = mousePos;
        }

        private void FollowMouseWithCamera(in Vector2 screenzone)
        {
            if(screenzone.y < GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT)
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

                MoveCameraToPosition(in cameraNewPosition);
            }
        }

        private static void MoveCameraToPosition(in Vector3 position)
        {
            Vector3 cameraNewPosition = position;

            /* Limit center of camera to stablished level bounds */
            Vector3 deltaFromMin = _cameraCenterLimitBounds.min - cameraNewPosition;
            Vector3 deltaFromMax = _cameraCenterLimitBounds.max - cameraNewPosition;

            deltaFromMin = Vector2.Max(deltaFromMin, Vector2.zero);
            deltaFromMax = Vector2.Min(deltaFromMax, Vector2.zero);

            /* Correct limited position in case deltaFromMin or deltaFromMax differ from 0 */
            cameraNewPosition += deltaFromMin + deltaFromMax;

            mainCameraTransform.position = cameraNewPosition;
        }

        private static void _OnPickedItemChanged(ChangedEventType evtype, in GameItem oldval, in GameItem newval)
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
                    ref readonly ItemInfo info = ref ItemsInteractionsClass.GetItemInfo(newval);
                    GameSprite sprID;
                    GamePickableItem pickable = info.pickableItem;
                    sprID = info.pickableSprite;
                    cursor_spr.sprite = ResourceSpritesClass.GetSprite(sprID);
                }
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;
            _ = oldval;

            switch (newval)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    UICanvas.SetActive(false);
                    break;
                case Game_Status.GAME_STATUS_LOADING:
                case Game_Status.GAME_STATUS_CHANGING_ROOM:
                    UICanvas.SetActive(true);
                    UICanvas_loadingObj.SetActive(true);
                    UICanvas_dialogObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;
                case Game_Status.GAME_STATUS_PLAY_DIALOG:
                    UICanvas.SetActive(true);
                    UICanvas_dialogObj.SetActive(true);
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_itemMenuObj.SetActive(false);
                    break;
                case Game_Status.GAME_STATUS_PAUSE:
                    //paused_text.gameObject.SetActive(true);
                    break;
                case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                    UICanvas.SetActive(true);
                    UICanvas_itemMenuObj.SetActive(true);
                    UICanvas_loadingObj.SetActive(false);
                    UICanvas_dialogObj.SetActive(false);
                    break;
                default:
                    break;
            }

            cachedGameStatus = newval;
        }
    }
}
