using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceDialogs;
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

        private Vector2 _mouseStartedCameraDrag;
        private bool _mouseDraggingCamera;
        private Bounds _levelBounds;
        private Bounds _cameraCenterLimitBounds;
        private Bounds _cameraBounds;
        private Bounds _mouseScreenBounds;

        private Camera mainCamera;
        private Transform mainCameraTransform;
        private GameObject background;
        private SpriteRenderer background_spr;

        private Game_Status cachedGameStatus;
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
            _cameraBounds = new Bounds();

            uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _cameraBounds.min = mainCamera.ScreenToWorldPoint(Vector3.zero);
            _cameraBounds.max = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            _mouseScreenBounds = new Bounds();
            _mouseScreenBounds.min = Vector2.zero;
            _mouseScreenBounds.max = Vector2.one;

            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_GraphicsMaster.REG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);
            VARMAP_GraphicsMaster.REG_ITEM_HOVER(_OnHoverItemChanged);

            cachedGameStatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            _loaded = false;

            _mouseDraggingCamera = false;
        }

        // Update is called once per frame
        void Update()
        {
            ref readonly MousePropertiesStruct mouse = ref VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();

            uicanvas_cls.MoveCursor(mouse.pos1);

            switch (cachedGameStatus)
            {
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading();
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    FollowMouseWithCamera(in mouse);
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

                    background_spr.sprite = ResourceSpritesClass.GetSprite(_singleton.backgroundGameSprite);
                    _levelBounds = background_spr.bounds;

                    _cameraCenterLimitBounds = _levelBounds;
                    _cameraCenterLimitBounds.extents -= _cameraBounds.extents;

                    candidatePos.z = mainCameraTransform.position.z;

                    MoveCameraToPosition(in candidatePos);

                    VARMAP_GraphicsMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GraphicsMaster);
                    _loaded = true;
                }
            }
        }


        private void FollowMouseWithCamera(in MousePropertiesStruct mouse)
        {
            Vector2 screenzone_orig = new(mouse.posPixels.x / Screen.safeArea.width, mouse.posPixels.y / Screen.safeArea.height);
            Vector2 szone = new(screenzone_orig.x, screenzone_orig.y * GameFixedConfig.GAME_ZONE_HEIGHT_FACTOR);

            if (_mouseScreenBounds.Contains(szone))
            {
                if (_mouseDraggingCamera && (mouse.mouseThird == ButtonState.BUTTON_STATE_PRESSING))
                {
                    Vector3 moveCameraDelta;
                    Vector3 cameraNewPosition;

                    Vector2 deltaszone = szone - _mouseStartedCameraDrag;
                    float distance = Mathf.Min(deltaszone.magnitude, GameFixedConfig.GAME_ZONE_CURSOR_PERCENT_MAX_SPEED); /* 25% of screen */
                    deltaszone = GameFixedConfig.GAME_ZONE_CURSOR_PERCENT_MAX_SPEED_DIV * GameFixedConfig.MOVE_CAMERA_SPEED * distance * deltaszone.normalized;
                    

                    /* Create movement vector and propose new camera center position */
                    moveCameraDelta = deltaszone;
                    moveCameraDelta *= Time.deltaTime;
                    cameraNewPosition = mainCameraTransform.position + moveCameraDelta;

                    MoveCameraToPosition(in cameraNewPosition);
                }
                else
                {
                    if(mouse.mouseThird == ButtonState.BUTTON_STATE_PRESSED)
                    {
                        _mouseDraggingCamera = true;
                        _mouseStartedCameraDrag = szone;
                    }
                    else
                    {
                        _mouseDraggingCamera = false;
                    }
                }
            }
            else
            {
                _mouseDraggingCamera = false;
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
        }

        private void _OnPickedItemChanged(ChangedEventType evtype, in GameItem oldval, in GameItem newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                uicanvas_cls.SetCursorItem(newval);
            }
        }

        private void _OnHoverItemChanged(ChangedEventType evtype, in GameItem oldval, in GameItem newval)
        {
            _ = evtype;

            if(oldval != newval)
            {
                uicanvas_cls.SetCursorLabel(newval);
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;
            _ = oldval;

            if (oldval != newval)
            {
                _mouseDraggingCamera = false;

                switch (newval)
                {
                    case Game_Status.GAME_STATUS_PLAY:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_NONE);
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_LOADING);
                        break;
                    case Game_Status.GAME_STATUS_PLAY_DIALOG:
                        uicanvas_cls.SetDisplayMode(DisplayMode.DISPLAY_MODE_DIALOG);
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

            cachedGameStatus = newval;
        }
    }
}
