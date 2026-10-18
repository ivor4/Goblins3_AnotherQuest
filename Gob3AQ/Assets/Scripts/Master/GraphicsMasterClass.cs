using Gob3AQ.Brain.ItemsInteraction;
using Gob3AQ.Brain.LevelOptions;
using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameMenu.UICanvas;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceDialogsAtlas;
using Gob3AQ.ResourceSprites;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gob3AQ.GraphicsMaster
{
    [System.Serializable]
    public class GraphicsMasterClass : MonoBehaviour
    {

        [SerializeField]
        private GameObject UICanvas;

        private ZOOM_CHANGED_DELEGATE zoomChangedDelegates;

        private static GraphicsMasterClass _singleton;

        private UICanvasClass uicanvas_cls;

        private float _maxCameraOrthographicSize;
        private Vector2 _mouseStartedCameraDrag;
        private Vector3 _cameraPosStartedCameraDrag;
        private Vector3 _screenToWorldFactor;
        private bool _mouseDraggingCamera;
        private Bounds _levelBounds;
        private Bounds _cameraCenterLimitBounds;
        private Bounds _mouseScreenZoneLimit;

        private Camera mainCamera;
        private CameraEffectsClass cameraEffects;
        private Transform mainCameraTransform;
        private GameObject background;
        private SpriteRenderer background_spr;
        private Texture2D gameSnapshot;
        private Sprite gameSnapshot_sprite;
        private bool loadingFadingIn;
        private ulong loadingFadeStartTime;
        private bool loadingFadeOutDone;
        private int load_step;
        private bool loadingFadeLong;

        private bool isPickableSelected;
        private bool isDoorHovered;

        private Vector3 zoomFinalPosition;
        private float zoomFinalOrthoSize;
        private Vector3 zoomInitialPosition;
        private float zoomInitialOrthoSize;
        private ulong zoomTransitionStartTime;
        private ulong zoomTransitionTotalTime;
        private bool zoomActive;
        private bool zoomRoomAcceptsRecommended;

        private Bounds recommendedZoomWorldBounds;
        private ulong lastZoomChangeTimestamp;

        private bool mouseWheelZoomActive;
        private bool mouseWheelZoomDirectionUp;
        private ulong mouseWheelZoomStartTime;
        private float mouseWheelZoomStartOrthoSize;

        private bool prevEffect;


        public static void ZoomSubscriptionService(bool subscribe, ZOOM_CHANGED_DELEGATE callback)
        {
            if (_singleton != null)
            {
                if (subscribe)
                {
                    _singleton.zoomChangedDelegates += callback;
                }
                else
                {
                    _singleton.zoomChangedDelegates -= callback;
                }
            }
        }

        public static void TakeLoadingSnapshotService()
        {
            if(_singleton != null)
            {
                _singleton.TakeGameSnapshot(_singleton.gameSnapshot);
            }
        }

        public static void ActivateForcedZoomMode(bool activate, in Bounds regionOfInterest, ulong zoomDuration)
        {
            if (!_singleton) return;

            _singleton.zoomInitialOrthoSize = _singleton.mainCamera.orthographicSize;
            _singleton.zoomInitialPosition = _singleton.mainCameraTransform.position;
            _singleton.zoomTransitionStartTime = VARMAP_GraphicsMaster.GET_ELAPSED_TIME_MS();
            _singleton.lastZoomChangeTimestamp = _singleton.zoomTransitionStartTime;
            _singleton.zoomActive = true;

            if (activate)
            {
                Bounds calcROI = _singleton.CalculateWorldBoundsROI(in regionOfInterest);

                _singleton.zoomFinalOrthoSize = calcROI.extents.y;
                _singleton.zoomFinalPosition = calcROI.center;
                _singleton.zoomTransitionTotalTime = zoomDuration;
            }
            else
            {
                _singleton.zoomFinalPosition = _singleton.recommendedZoomWorldBounds.center;
                _singleton.zoomFinalOrthoSize = _singleton.recommendedZoomWorldBounds.extents.y;
                _singleton.zoomTransitionTotalTime = GameFixedConfig.ZOOM_OUT_DURATION_MS;
            }
        }

        public static void ChangeRecommendedZoom(in Bounds regionOfInterest, ulong zoomDuration)
        {
            if (!_singleton) return;
            if (!_singleton.zoomRoomAcceptsRecommended) return;

            _singleton.recommendedZoomWorldBounds = _singleton.CalculateWorldBoundsROI(in regionOfInterest);
        }

        public static void LabelElemHoverService(bool activate, in LevelElemLabelInfo labelInfo)
        {
            if (!_singleton) return;

            _singleton.isDoorHovered = labelInfo.family == GameItemFamily.ITEM_FAMILY_TYPE_DOOR;

            _singleton.UpdateCursorBaseSprite();

            _singleton.uicanvas_cls.SetCursorLabel(activate, labelInfo.name);
        }

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
                zoomChangedDelegates = null;
                zoomActive = false;
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;
            cameraEffects = mainCamera.GetComponent<CameraEffectsClass>();

            uicanvas_cls = UICanvas.GetComponent<UICanvasClass>();

            _mouseScreenZoneLimit = new()
            {
                min = Vector2.zero,
                max = Vector2.one
            };

            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_GraphicsMaster.REG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);
            VARMAP_GraphicsMaster.REG_USER_INPUT_INTERACTION(_OnUserInputInteractionChanged);

            StartLoadingFade();

            mouseWheelZoomActive = false;

            _mouseDraggingCamera = false;
            isPickableSelected = false;
            isDoorHovered = false;

            gameSnapshot = new Texture2D(Screen.width/2, Screen.height/2, TextureFormat.RGB24, false);
            gameSnapshot_sprite = Sprite.Create(gameSnapshot, new Rect(0, 0, gameSnapshot.width, gameSnapshot.height), new Vector2(0.5f, 0.5f));

            TakeGameSnapshot(gameSnapshot);

            VARMAP_GraphicsMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GraphicsMaster);
        }

        // Update is called once per frame
        void Update()
        {
            ref readonly MousePropertiesStruct mouse = ref VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();
            ref readonly KeyStruct keys = ref VARMAP_GraphicsMaster.GET_PRESSED_KEYS();

            uicanvas_cls.MoveCursor(mouse.pos1);
            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();
            ulong actualTimestamp = VARMAP_GraphicsMaster.GET_ELAPSED_TIME_MS();

            if (load_step < 3)
            {
                UpdateLoadingFadeProgress(actualTimestamp);
            }

            if(zoomActive)
            {
                ulong elapsedTime = actualTimestamp - zoomTransitionStartTime;

                float ortoSize;
                Vector3 cameraPos;

                if (elapsedTime >= zoomTransitionTotalTime)
                {
                    ortoSize = zoomFinalOrthoSize;
                    cameraPos = zoomFinalPosition;
                    zoomActive = false;

                    VARMAP_GraphicsMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_ZOOM);
                }
                else
                {
                    float expProgress = Mathf.Exp((-5f * elapsedTime) / zoomTransitionTotalTime);
                    ortoSize = Mathf.Lerp(zoomInitialOrthoSize, zoomFinalOrthoSize, 1.0f - expProgress);
                    cameraPos = Vector3.Lerp(zoomInitialPosition, zoomFinalPosition, 1.0f - expProgress);
                }

                UpdateCameraBounds(ortoSize);
                MoveCameraToPosition(in cameraPos);
            }


            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_CHANGING_ROOM:
                    UpdateLoadingFadeProgress(actualTimestamp);
                    break;
                case Game_Status.GAME_STATUS_LOADING:
                    Execute_Loading(actualTimestamp);
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    ExecuteUpdatePlay(in mouse, in keys, actualTimestamp);
                    UpdateCameraEffects();
                    break;
                default:
                    break;
            }
        }



        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;

                VARMAP_GraphicsMaster.UNREG_GAMESTATUS(_GameStatusChanged);
                VARMAP_GraphicsMaster.UNREG_PICKABLE_ITEM_CHOSEN(_OnPickedItemChanged);
                VARMAP_GraphicsMaster.UNREG_USER_INPUT_INTERACTION(_OnUserInputInteractionChanged);
            }
        }

        private void Execute_Loading(ulong timestamp)
        {
            switch (load_step)
            {
                case 0:
                    VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out bool gamemasterLoaded);
                    VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_LevelMaster, out bool levelmasterLoaded);
                    VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_PlayerMaster, out bool playermasterLoaded);
                    VARMAP_GraphicsMaster.IS_MODULE_LOADED(GameModules.MODULE_GameEventMaster, out bool eventmasterLoaded);

                    if (gamemasterLoaded && levelmasterLoaded && playermasterLoaded && eventmasterLoaded)
                    {
                        Room actualRoom = VARMAP_GraphicsMaster.GET_ACTUAL_ROOM();
                        ref readonly RoomInfo roomInfo = ref ResourceAtlasClass.GetRoomInfo(actualRoom);

                        if(LevelOptionsClass.GetRoomUsesDialogZoom(actualRoom))
                        {
                            zoomRoomAcceptsRecommended = true;
                        }
                        else
                        {
                            zoomRoomAcceptsRecommended = false;
                        }

                        GameSprite background_sprite;

                        if (roomInfo.Backgrounds.Length == 1)
                        {
                            background_sprite = roomInfo.Backgrounds[0];
                        }
                        else
                        {
                            background_sprite = roomInfo.Backgrounds[(int)VARMAP_GraphicsMaster.GET_DAY_MOMENT()];
                        }

                        background_spr.sprite = ResourceSpritesClass.GetSprite(background_sprite);
                        _levelBounds = background_spr.bounds;

                        float constrainedByWidth = _levelBounds.extents.x / mainCamera.aspect;
                        float constrainedByHeight = _levelBounds.extents.y;

                        _maxCameraOrthographicSize = Mathf.Min(constrainedByWidth, constrainedByHeight);


                        if (zoomRoomAcceptsRecommended)
                        {
                            CharacterType selectedChar = VARMAP_GraphicsMaster.GET_PLAYER_SELECTED();
                            GameItem selectedItem = ResourceDialogsAtlasClass.GetItemForCharacter(selectedChar);

                            VARMAP_GraphicsMaster.GET_ITEM_SPRITE_BOUNDARIES(selectedItem, out Bounds characterWorldBounds);
                            recommendedZoomWorldBounds = CalculateWorldBoundsROI(in characterWorldBounds);
                        }
                        else
                        {
                            recommendedZoomWorldBounds = new Bounds(new Vector3(_levelBounds.center.x, _levelBounds.center.y, mainCamera.transform.position.z),
                                new Vector3(_maxCameraOrthographicSize * mainCamera.aspect, _maxCameraOrthographicSize, 1f)*2f);
                        }


                        /* Start with timeout elapsed */
                        lastZoomChangeTimestamp = VARMAP_GraphicsMaster.GET_ELAPSED_TIME_MS() - GameFixedConfig.CAMERA_AUTO_RECOMMENDED_TIMEOUT_MS;

                        UpdateCameraBounds(recommendedZoomWorldBounds.extents.y);
                        MoveCameraToPosition(recommendedZoomWorldBounds.center);
                        

                        UpdateCursorBaseSprite();
                        ++load_step;
                    }
                    break;

                case 1:
                    if(loadingFadeOutDone)
                    {
                        loadingFadingIn = true;
                        loadingFadeStartTime = timestamp;
                        ++load_step;
                        VARMAP_GraphicsMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_GraphicsMaster);
                    }
                    break;

                default:
                    break;
            }
        }

        private void UpdateCameraBounds(float orthoSize)
        {
            mainCamera.orthographicSize = Mathf.Clamp(orthoSize, GameFixedConfig.MIN_CAMERA_ORTHO_SIZE, _maxCameraOrthographicSize);

            Vector2 src = mainCamera.ScreenToWorldPoint(new(0, 0, 0));
            Vector2 dst = mainCamera.ScreenToWorldPoint(new(1, 1, 0));

            _screenToWorldFactor = dst - src;

            _cameraCenterLimitBounds = _levelBounds;
            _cameraCenterLimitBounds.size -= new Vector3(_screenToWorldFactor.x * Screen.safeArea.width, _screenToWorldFactor.y * Screen.safeArea.height);

            zoomChangedDelegates?.Invoke(mainCamera.orthographicSize);
        }


        private void ExecuteUpdatePlay(in MousePropertiesStruct mouse, in KeyStruct keys, ulong timestamp)
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

                    zoomActive = false;
                    lastZoomChangeTimestamp = timestamp;
                    VARMAP_GraphicsMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_ZOOM);
                }
                else
                {
                    if (thirdPressed)
                    {
                        _mouseDraggingCamera = true;
                        _mouseStartedCameraDrag = mouse.posPixels;
                        _cameraPosStartedCameraDrag = mainCameraTransform.position;

                        zoomActive = false;
                        lastZoomChangeTimestamp = timestamp;
                        VARMAP_GraphicsMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_ZOOM);
                    }
                    else
                    {
                        _mouseDraggingCamera = false;

                        bool zoomUp = keys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_UP);
                        bool zoomDown = keys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_DOWN);

                        if(zoomUp || zoomDown)
                        {
                            if ((!mouseWheelZoomActive)||(mouseWheelZoomDirectionUp != zoomUp))
                            {
                                mouseWheelZoomActive = true;
                                mouseWheelZoomStartTime = timestamp;
                                mouseWheelZoomStartOrthoSize = mainCamera.orthographicSize;
                                mouseWheelZoomDirectionUp = zoomUp;
                            }

                            ulong elapsedTime = timestamp - mouseWheelZoomStartTime;
                            float increment = 0.0050f * elapsedTime;
                            float ortoSize;

                            if (zoomUp)
                            {
                                ortoSize = mouseWheelZoomStartOrthoSize - increment;
                            }
                            else
                            {
                                ortoSize = mouseWheelZoomStartOrthoSize + increment;
                            }

                            UpdateCameraBounds(ortoSize);

                            /* Keep world point where cursor is at the exact same point in Screen */
                            Vector2 pixelsDistance = new(Screen.safeArea.width * 0.5f - mouse.posPixels.x, Screen.safeArea.height * 0.5f - mouse.posPixels.y);
                            Vector2 worldDistance = new(pixelsDistance.x * _screenToWorldFactor.x, pixelsDistance.y * _screenToWorldFactor.y);


                            Vector3 CameraNewPosition = new(mouse.pos1.x + worldDistance.x, mouse.pos1.y + worldDistance.y, mainCameraTransform.position.z);
                            MoveCameraToPosition(in CameraNewPosition);

                            zoomActive = false;
                            lastZoomChangeTimestamp = timestamp;
                            VARMAP_GraphicsMaster.NOTIFY_ENDED_ACTION(NotifyAction.NOTIFY_ZOOM);
                        }
                        else
                        {
                            mouseWheelZoomActive = false;
                        }
                    }
                }
            }
            else
            {
                _mouseDraggingCamera = false;
            }

            if ((!zoomActive) && ((timestamp - lastZoomChangeTimestamp) >= GameFixedConfig.CAMERA_AUTO_RECOMMENDED_TIMEOUT_MS))
            {
                VARMAP_GraphicsMaster.ACTIVATE_FORCED_ZOOM_MODE(false, default, GameFixedConfig.ZOOM_OUT_DURATION_MS);
            }

            mouseDraggingChanged ^= _mouseDraggingCamera;

            if (mouseDraggingChanged)
            {
                UpdateCursorBaseSprite();
            }
        }

        private void MoveCameraToPosition(in Vector3 position)
        {
            Vector3 cameraNewPosition = new(position.x, position.y, mainCameraTransform.position.z);

            /* Limit center of camera to stablished level bounds */
            Vector3 deltaFromMin = _cameraCenterLimitBounds.min - cameraNewPosition;
            Vector3 deltaFromMax = _cameraCenterLimitBounds.max - cameraNewPosition;

            deltaFromMin = Vector2.Max(deltaFromMin, Vector2.zero);
            deltaFromMax = Vector2.Min(deltaFromMax, Vector2.zero);

            /* Correct limited position in case deltaFromMin or deltaFromMax differ from 0 */
            cameraNewPosition += deltaFromMin + deltaFromMax;

            mainCameraTransform.position = cameraNewPosition;
        }

        private Bounds CalculateWorldBoundsROI(in Bounds characterWorldBounds)
        {
            Bounds roi = new Bounds(new Vector3(characterWorldBounds.center.x, characterWorldBounds.center.y, mainCamera.transform.position.z), characterWorldBounds.size);
            float yExtent = roi.extents.y * GameFixedConfig.GAME_RECOMMENDED_ZOOM_EXTRA_Y_SIZE;


            roi.extents = new Vector3(mainCamera.aspect * yExtent, yExtent, 1f);

            if (roi.extents.y > _maxCameraOrthographicSize)
            {
                roi.extents = new Vector3(_maxCameraOrthographicSize * mainCamera.aspect, _maxCameraOrthographicSize, roi.extents.z);
            }

            roi.center += new Vector3(Mathf.Max(0f, _levelBounds.min.x - roi.min.x), Mathf.Max(0f, _levelBounds.min.y - roi.min.y), 0f);
            roi.center += new Vector3(Mathf.Min(0f, _levelBounds.max.x - roi.max.x), Mathf.Min(0f, _levelBounds.max.y - roi.max.y), 0f);

            return roi;
        }

        private void UpdateCameraEffects()
        {
            GameEventCombi flatEventCombi = new GameEventCombi(GameEvent.EVENT_NONE, false);
            Span<GameEventCombi> eventCombi = MemoryMarshal.CreateSpan(ref flatEventCombi, 1);

            eventCombi[0] = new GameEventCombi(GameEvent.EVENT_DRUNK_STATE, false);

            VARMAP_GraphicsMaster.IS_EVENT_COMBI_OCCURRED(eventCombi, out bool drunk);



            if (drunk)
            {
                eventCombi[0] = new GameEventCombi(GameEvent.EVENT_BEER_THIRD_ROUND, false);
                VARMAP_GraphicsMaster.IS_EVENT_COMBI_OCCURRED(eventCombi, out bool drunk3);
                eventCombi[0] = new GameEventCombi(GameEvent.EVENT_BEER_SECOND_ROUND, false);
                VARMAP_GraphicsMaster.IS_EVENT_COMBI_OCCURRED(eventCombi, out bool drunk2);

                if(drunk3)
                {
                    cameraEffects.ActivateDrunkEffect(3);
                }
                else if(drunk2)
                {
                    cameraEffects.ActivateDrunkEffect(2);
                }
                else
                {
                    cameraEffects.ActivateDrunkEffect(1);
                }

                prevEffect = true;

            }
            else if(VARMAP_GraphicsMaster.GET_ACTUAL_ROOM() is >= Room.DREAM_1_CORRIDOR and <= Room.DREAM_1_KITCHEN_NIGHT)
            {
                cameraEffects.ActivateDreamEffect();
                prevEffect = true;
            }
            else if(prevEffect)
            {
                prevEffect = false;
                cameraEffects.DeactivateEffects();
            }
        }

        private void UpdateCursorBaseSprite()
        {
            GameSprite cursorSprite;

            if (_mouseDraggingCamera)
            {
                cursorSprite = GameSprite.SPRITE_CURSOR_DRAG;
            }
            else if (isDoorHovered)
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

        private void TakeGameSnapshot(Texture2D destTexture)
        {
            RenderTexture prevCameraTarget = mainCamera.targetTexture;

            RenderTexture rt = new RenderTexture(destTexture.width, destTexture.height, 24);
            mainCamera.targetTexture = rt;
            mainCamera.Render();

            mainCamera.targetTexture = prevCameraTarget;

            prevCameraTarget = RenderTexture.active;
            RenderTexture.active = rt;
            destTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            destTexture.Apply();

            Destroy(rt);

            RenderTexture.active = prevCameraTarget;
        }

        private void UpdateLoadingFadeProgress(ulong timestamp)
        {
            float progress = Mathf.Clamp01((timestamp - loadingFadeStartTime) / ((loadingFadeLong && loadingFadingIn)  ? GameFixedConfig.FADE_BLACK_TIMEOUT_LONG_MS : GameFixedConfig.FADE_BLACK_TIMEOUT_MS));
            float revProgress = 1.0f - progress;

            Color tintColor;

            if(loadingFadingIn)
            {
                tintColor = new Color(0f, 0f, 0f, revProgress);
            }
            else
            {
                tintColor = new Color(revProgress, revProgress, revProgress, 1.0f);
            }
            

            if (progress >= 1f)
            {
                loadingFadeOutDone = true;

                if (load_step == 2)
                {
                    ++load_step;
                    uicanvas_cls.HideLoadingObj();
                }
            }
            else
            {
                uicanvas_cls.SetLoadingSprite(gameSnapshot_sprite, true, tintColor);
            }
        }

        private void StartLoadingFade()
        {
            loadingFadeLong = LevelOptionsClass.IS_ROOM_FADE_OUT_LONG.GetValueOrDefault(VARMAP_GraphicsMaster.GET_ACTUAL_ROOM(), false);
            loadingFadeStartTime = VARMAP_GraphicsMaster.GET_ELAPSED_TIME_MS();
            loadingFadingIn = false;
            load_step = 0;
            loadingFadeOutDone = false;
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

        private void _OnUserInputInteractionChanged(ChangedEventType evtype, in UserInputInteraction oldval, in UserInputInteraction newval)
        {
            _ = evtype;

            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            if ((oldval != newval) && (gstatus != Game_Status.GAME_STATUS_CHANGING_ROOM) && (gstatus != Game_Status.GAME_STATUS_LOADING))
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
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        cameraEffects.DeactivateEffects();
                        prevEffect = false;
                        _mouseDraggingCamera = false;
                        isPickableSelected = false;
                        isDoorHovered = false;
                        StartLoadingFade();
                        zoomActive = false;
                        mouseWheelZoomActive = false;

                        uicanvas_cls.SetLoadingSprite(gameSnapshot_sprite, true, Color.white);
                        background_spr.sprite = null;
                        break;
                    default:
                        break;
                }

                switch (oldval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        uicanvas_cls.SetLoadingSprite(null, false, Color.black);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
