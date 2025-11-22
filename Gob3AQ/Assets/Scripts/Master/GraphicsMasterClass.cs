using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

namespace Gob3AQ.GraphicsMaster
{
    [System.Serializable]
    public class GraphicsMasterClass : MonoBehaviour
    {
        [SerializeField]
        private GameSprite backgroundGameSprite;

        [SerializeField]
        private GameObject UICanvas;

        private static GraphicsMasterClass _singleton;

        private UICanvasClass uicanvas_cls;

        private static float _maxCameraOrthographicSize;
        private Vector2 _mouseStartedCameraDrag;
        private Vector3 _cameraPosStartedCameraDrag;
        private Vector3 _screenToWorldFactor;
        private bool _mouseDraggingCamera;
        private Bounds _levelBounds;
        private Bounds _cameraCenterLimitBounds;
        private Bounds _mouseScreenZoneLimit;

        private Camera mainCamera;
        private Transform mainCameraTransform;
        private GameObject background;
        private SpriteRenderer background_spr;

        private bool isPickableSelected;
        private bool isDoorHovered;

        private bool _loaded;



        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                background = transform.Find("Background").gameObject;
                background_spr = background.GetComponent<SpriteRenderer>();
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;

            uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _mouseScreenZoneLimit = new()
            {
                min = Vector2.zero,
                max = Vector2.one
            };

            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_GraphicsMaster.REG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);
            VARMAP_GraphicsMaster.REG_ITEM_HOVER(_OnHoverItemChanged);
            VARMAP_GraphicsMaster.REG_USER_INPUT_INTERACTION(_OnUserInputInteractionChanged);


            _loaded = false;

            _mouseDraggingCamera = false;
        }

        // Update is called once per frame
        void Update()
        {
            ref readonly MousePropertiesStruct mouse = ref VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();
            ref readonly KeyStruct keys = ref VARMAP_GraphicsMaster.GET_PRESSED_KEYS();

            uicanvas_cls.MoveCursor(mouse.pos1);
            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading();
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    FollowMouseWithCamera(in mouse, in keys);
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
                VARMAP_GraphicsMaster.UNREG_ITEM_HOVER(_OnHoverItemChanged);
                VARMAP_GraphicsMaster.UNREG_USER_INPUT_INTERACTION(_OnUserInputInteractionChanged);
            }
        }

        private void Execute_Loading()
        {
            if (!_loaded)
            {
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out bool gamemasterLoaded);
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_LevelMaster, out bool levelmasterLoaded);
                VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_PlayerMaster, out bool playermasterLoaded);

                if (gamemasterLoaded && levelmasterLoaded && playermasterLoaded)
                {
                    background_spr.sprite = ResourceSpritesClass.GetSprite(_singleton.backgroundGameSprite);
                    _levelBounds = background_spr.bounds;

                    float constrainedByWidth = _levelBounds.extents.x / mainCamera.aspect;
                    float constrainedByHeight = _levelBounds.extents.y;

                    _maxCameraOrthographicSize = Mathf.Min(constrainedByWidth, constrainedByHeight);

                    ref readonly CameraDispositionStruct cameradisp = ref VARMAP_GraphicsMaster.GET_CAMERA_DISPOSITION();
                    Room actualRoom = VARMAP_GraphicsMaster.GET_ACTUAL_ROOM();

                    Vector3 cameraPosition;

                    if (cameradisp.room == actualRoom)
                    {
                        mainCamera.orthographicSize = cameradisp.orthoSize;

                        UpdateCameraBounds();

                        cameraPosition = cameradisp.position;
                        cameraPosition.z = mainCameraTransform.position.z;
                    }
                    else
                    {
                        UpdateCameraBounds();
                        cameraPosition = mainCameraTransform.position;
                    }

                    MoveCameraToPosition(in cameraPosition);

                    VARMAP_GraphicsMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GraphicsMaster);

                    UpdateCursorBaseSprite();
                    _loaded = true;
                }
            }
        }

        private void UpdateCameraBounds()
        {
            Vector2 src = mainCamera.ScreenToWorldPoint(new(0, 0, 0));
            Vector2 dst = mainCamera.ScreenToWorldPoint(new(1, 1, 0));
            _screenToWorldFactor = dst - src;

            _cameraCenterLimitBounds = _levelBounds;
            _cameraCenterLimitBounds.size -= new Vector3(_screenToWorldFactor.x * Screen.safeArea.width, _screenToWorldFactor.y * Screen.safeArea.height);
        }


        private void FollowMouseWithCamera(in MousePropertiesStruct mouse, in KeyStruct keys)
        {
            Vector2 screenzone_orig = new(mouse.posPixels.x / Screen.safeArea.width, mouse.posPixels.y / Screen.safeArea.height);
            Vector2 szone = new(screenzone_orig.x, screenzone_orig.y * GameFixedConfig.GAME_ZONE_HEIGHT_FACTOR);
            bool mouseDraggingChanged = _mouseDraggingCamera;

            if (_mouseScreenZoneLimit.Contains(szone))
            {
                bool thirdPressed = keys.isKeyBeingPressed(KeyFunctions.KEYFUNC_DRAG);
                if (_mouseDraggingCamera && thirdPressed)
                {
                    Vector3 moveCameraDelta;
                    Vector3 cameraNewPosition;

                    Vector2 deltaPixels = _mouseStartedCameraDrag - mouse.posPixels;

                    /* Create movement vector and propose new camera center position */
                    moveCameraDelta = new(deltaPixels.x * _screenToWorldFactor.x, deltaPixels.y * _screenToWorldFactor.y);
                    cameraNewPosition = _cameraPosStartedCameraDrag + moveCameraDelta;

                    MoveCameraToPosition(in cameraNewPosition);
                }
                else
                {
                    if (thirdPressed)
                    {
                        _mouseDraggingCamera = true;
                        _mouseStartedCameraDrag = mouse.posPixels;
                        _cameraPosStartedCameraDrag = mainCameraTransform.position;
                    }
                    else
                    {
                        _mouseDraggingCamera = false;
                        bool zoomApplied;

                        if(keys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_UP))
                        {
                            mainCamera.orthographicSize = Math.Max(mainCamera.orthographicSize - 0.1f, GameFixedConfig.MIN_CAMERA_ORTHO_SIZE);
                            zoomApplied = true;
                        }
                        else if(keys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_DOWN))
                        {
                            mainCamera.orthographicSize = Math.Min(mainCamera.orthographicSize + 0.1f, _maxCameraOrthographicSize);
                            zoomApplied = true;
                        }
                        else
                        {
                            zoomApplied = false;
                        }

                        if(zoomApplied)
                        {
                            UpdateCameraBounds();

                            /* Keep world point where cursor is at the exact same point in Screen */
                            Vector2 pixelsDistance = new(Screen.safeArea.width * 0.5f - mouse.posPixels.x, Screen.safeArea.height * 0.5f - mouse.posPixels.y);
                            Vector2 worldDistance = new(pixelsDistance.x * _screenToWorldFactor.x, pixelsDistance.y * _screenToWorldFactor.y);


                            Vector3 CameraNewPosition = new(mouse.pos1.x + worldDistance.x, mouse.pos1.y + worldDistance.y, mainCameraTransform.position.z);
                            MoveCameraToPosition(in CameraNewPosition);
                        }
                    }
                }
            }
            else
            {
                _mouseDraggingCamera = false;
            }

            mouseDraggingChanged ^= _mouseDraggingCamera;

            if(mouseDraggingChanged)
            {
                UpdateCursorBaseSprite();
            }
        }

        private void MoveCameraToPosition(in Vector3 position)
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

            Room actualRoom = VARMAP_GraphicsMaster.GET_ACTUAL_ROOM();

            CameraDispositionStruct cameradisp = new() { position = mainCameraTransform.position,
                   orthoSize = mainCamera.orthographicSize, room = actualRoom };

            VARMAP_GraphicsMaster.SET_CAMERA_DISPOSITION(in cameradisp);
        }

        private void UpdateCursorBaseSprite()
        {
            GameSprite cursorSprite;

            if(_mouseDraggingCamera)
            {
                cursorSprite = GameSprite.SPRITE_CURSOR_DRAG;
            }
            else if(isDoorHovered)
            {
                cursorSprite = GameSprite.SPRITE_UI_CURSOR_DOOR;
            }
            else if (isPickableSelected)
            {
                cursorSprite = GameSprite.SPRITE_CURSOR_USING;
            }
            else
            {
                cursorSprite = GameSprite.SPRITE_CURSOR_NORMAL;
            }

            uicanvas_cls.SetCursorBaseSprite(cursorSprite);
        }

        private void _OnPickedItemChanged(ChangedEventType evtype, in GameItem oldval, in GameItem newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                isPickableSelected = newval != GameItem.ITEM_NONE;

                UpdateCursorBaseSprite();
                uicanvas_cls.SetCursorItem(newval);
            }
        }

        private void _OnHoverItemChanged(ChangedEventType evtype, in GameItem oldval, in GameItem newval)
        {
            _ = evtype;

            if(oldval != newval)
            {
                ref readonly ItemInfo itemInfo = ref ItemInfo.EMPTY;

                if (newval != GameItem.ITEM_NONE)
                {
                    itemInfo = ref ItemsInteractionsClass.GetItemInfo(newval);
                }

                isDoorHovered = itemInfo.family == GameItemFamily.ITEM_FAMILY_TYPE_DOOR;

                UpdateCursorBaseSprite();

                uicanvas_cls.SetCursorLabel(newval, in itemInfo);
            }
        }

        private void _OnUserInputInteractionChanged(ChangedEventType evtype, in UserInputInteraction oldval, in UserInputInteraction newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                uicanvas_cls.AnimateNewUserInteraction(newval);
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                _mouseDraggingCamera = false;
                UpdateCursorBaseSprite();

                switch (newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_NONE);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DIALOG:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_DIALOG);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_MEMENTO:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_MEMENTO);
                        break;
                    case Game_Status.GAME_STATUS_PAUSE:
                        //paused_text.gameObject.SetActive(true);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_ITEM_MENU:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_INVENTORY);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
