using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.InputMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;

namespace Gob3AQ.InputMaster
{
    public class InputMasterClass : MonoBehaviour
    {
        private static InputMasterClass _singleton;
        private KeyOptions cachedKeyOptions;
        private KeyStruct cachedPressedKeys;
        private MousePropertiesStruct cachedMouseProps;
        private Camera mainCamera;
        private int wheelDebounce;



        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
            }
        }



        private void Start()
        {
            cachedPressedKeys = default;
            cachedKeyOptions = VARMAP_InputMaster.GET_GAME_OPTIONS().keyOptions;
            cachedMouseProps = default;
            mainCamera = Camera.main;


            VARMAP_InputMaster.REG_GAME_OPTIONS(_GameOptionsChanged);
            VARMAP_InputMaster.SET_PRESSED_KEYS(new KeyStruct() { pressedKeys = 0 });
            VARMAP_InputMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_InputMaster);
        }

        private void Update()
        {
            if (GameFixedConfig.PERIPH_PC)
            {
                bool wheelUp;
                bool wheelDown;
                KeyFunctions cycleKeys = 0;

                cycleKeys |= Input.GetKey(cachedKeyOptions.changeActionKey) ? KeyFunctions.KEYFUNC_CHANGEACTION : 0;
                cycleKeys |= Input.GetKey(cachedKeyOptions.selectKey) ? KeyFunctions.KEYFUNC_SELECT : 0;
                cycleKeys |= Input.GetKey(cachedKeyOptions.inventoryKey) ? KeyFunctions.KEYFUNC_INVENTORY : 0;
                cycleKeys |= Input.GetKey(cachedKeyOptions.dragKey) ? KeyFunctions.KEYFUNC_DRAG : 0;

                float mouseWheelFloat = Input.GetAxisRaw("Mouse ScrollWheel");

                if (mouseWheelFloat > 0.0f)
                {
                    wheelUp = true;
                    wheelDown = false;
                    wheelDebounce = 20;
                }
                else if (mouseWheelFloat < 0.0f)
                {
                    wheelUp = false;
                    wheelDown = true;
                    wheelDebounce = 20;
                }
                else
                {
                    if (wheelDebounce > 0)
                    {
                        wheelDebounce--;
                        wheelUp = cachedPressedKeys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_UP);
                        wheelDown = cachedPressedKeys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_DOWN);
                    }
                    else
                    {
                        wheelUp = false;
                        wheelDown = false;
                    }
                }

                cycleKeys |= wheelUp ? KeyFunctions.KEYFUNC_ZOOM_UP : 0;
                cycleKeys |= wheelDown ? KeyFunctions.KEYFUNC_ZOOM_DOWN : 0;

                KeyFunctions diffKeys = cycleKeys ^ cachedPressedKeys.pressedKeys;

                cachedPressedKeys.cyclepressedKeys = diffKeys & cycleKeys;
                cachedPressedKeys.cyclereleasedKeys = diffKeys & cachedPressedKeys.pressedKeys;
                cachedPressedKeys.pressedKeys = cycleKeys;


                Vector2 mousePosition = Input.mousePosition;
                Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(mousePosition);

                cachedMouseProps.pos1 = mouseWorld;
                cachedMouseProps.pos2 = mouseWorld;
                cachedMouseProps.posPixels = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);


                VARMAP_InputMaster.SET_PRESSED_KEYS(in cachedPressedKeys);
                VARMAP_InputMaster.SET_MOUSE_PROPERTIES(in cachedMouseProps);
            }

        }



        private int CountPressedKeys(KeyFunctions keys)
        {
            int count = 0;
            uint ukeys = (uint)keys;

            while(ukeys != 0)
            {
                ukeys &= ukeys - 1u;
                count++;
            }

            return count;
        }

        private void _GameOptionsChanged(ChangedEventType evtype, in GameOptionsStruct oldval, in GameOptionsStruct newval)
        {
            if (evtype == ChangedEventType.CHANGED_EVENT_SET)
            {
                cachedKeyOptions = newval.keyOptions;
            }
        }

        public void ResetPressedKeysService()
        {
            cachedPressedKeys.pressedKeys = 0;
            cachedPressedKeys.cyclepressedKeys = 0;
            cachedPressedKeys.cyclereleasedKeys = 0;
        }

        private void OnDestroy()
        {
            if (_singleton == this)
            {
                VARMAP_InputMaster.UNREG_GAME_OPTIONS(_GameOptionsChanged);
                _singleton = null;
            }
        }

    }
}
