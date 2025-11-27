using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.InputMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.Types.Delegates;

namespace Gob3AQ.InputMaster
{
    public class InputMasterClass : MonoBehaviour
    {
        private const string MOUSE_WHEEL = "Mouse ScrollWheel";
        private static InputMasterClass _singleton;
        private KEY_SUBSCRIPTION_CALL_DELEGATE[] keySubscription;
        private Camera mainCamera;
        private int wheelDebounce;
        private bool keysHaveChanged;


        public static void KeySubscriptionService(KeyFunctionsIndex key, KEY_SUBSCRIPTION_CALL_DELEGATE func, bool add)
        {
            if (_singleton != null)
            {
                if (add)
                {
                    _singleton.keySubscription[(int)key] += func;
                }
                else
                {
                    _singleton.keySubscription[(int)key] -= func;
                }
            }
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

                keySubscription = new KEY_SUBSCRIPTION_CALL_DELEGATE[(int)KeyFunctionsIndex.KEYFUNC_INDEX_TOTAL];
            }
        }



        private void Start()
        {
            keysHaveChanged = false;
            mainCamera = Camera.main;

            VARMAP_InputMaster.SET_PRESSED_KEYS(in KeyStruct.KEY_EMPTY);
            VARMAP_InputMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_InputMaster);
            VARMAP_InputMaster.REG_GAMESTATUS(_GameStatusChanged);
        }

        private void Update()
        {
            ref readonly KeyStruct prevCycleKeys = ref VARMAP_InputMaster.GET_PRESSED_KEYS();
            ref readonly GameOptionsStruct gameOptions = ref VARMAP_InputMaster.GET_GAME_OPTIONS();
            ref readonly KeyOptions keyOptions = ref gameOptions.keyOptions;


            ManageKeyEventsSubscription(in prevCycleKeys);

            if (GameFixedConfig.PERIPH_PC)
            {
                bool wheelUp;
                bool wheelDown;
                KeyFunctions cycleKeys = 0;

                cycleKeys |= Input.GetKey(keyOptions.changeActionKey) ? KeyFunctions.KEYFUNC_CHANGEACTION : 0;
                cycleKeys |= Input.GetKey(keyOptions.selectKey) ? KeyFunctions.KEYFUNC_SELECT : 0;
                cycleKeys |= Input.GetKey(keyOptions.inventoryKey) ? KeyFunctions.KEYFUNC_INVENTORY : 0;
                cycleKeys |= Input.GetKey(keyOptions.dragKey) ? KeyFunctions.KEYFUNC_DRAG : 0;

                float mouseWheelFloat = Input.GetAxisRaw(MOUSE_WHEEL);

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
                        wheelUp = prevCycleKeys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_UP);
                        wheelDown = prevCycleKeys.isKeyBeingPressed(KeyFunctions.KEYFUNC_ZOOM_DOWN);
                    }
                    else
                    {
                        wheelUp = false;
                        wheelDown = false;
                    }
                }

                cycleKeys |= wheelUp ? KeyFunctions.KEYFUNC_ZOOM_UP : 0;
                cycleKeys |= wheelDown ? KeyFunctions.KEYFUNC_ZOOM_DOWN : 0;

                KeyFunctions diffKeys = cycleKeys ^ prevCycleKeys.pressedKeys;

                keysHaveChanged = (diffKeys | prevCycleKeys.cyclepressedKeys | prevCycleKeys.cyclereleasedKeys) != 0;

                if (keysHaveChanged)
                {
                    KeyStruct actualOutputKeys;
                    actualOutputKeys.cyclepressedKeys = diffKeys & cycleKeys;
                    actualOutputKeys.cyclereleasedKeys = diffKeys & prevCycleKeys.pressedKeys;
                    actualOutputKeys.pressedKeys = cycleKeys;
                    VARMAP_InputMaster.SET_PRESSED_KEYS(in actualOutputKeys);
                }

                Vector2 mousePosition = Input.mousePosition;
                Vector2Int posPixels = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);


                MousePropertiesStruct mouseProps;
                Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(mousePosition);
                mouseProps.posPixels = posPixels;
                mouseProps.pos1 = mouseWorld;
                mouseProps.pos2 = mouseWorld;
                VARMAP_InputMaster.SET_MOUSE_PROPERTIES(in mouseProps);
            }

        }


#if (false)
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
#endif



        private void ManageKeyEventsSubscription(in KeyStruct prevCycleKeys)
        {
            if (keysHaveChanged)
            {
                /* Manage events */
                int pressed = (int)prevCycleKeys.cyclepressedKeys;
                int released = (int)prevCycleKeys.cyclereleasedKeys;
                int combiPressedReleased = pressed | released;
                int keyIndex = 0;

                while (combiPressedReleased != 0)
                {
                    bool callEvent;
                    bool isPressed;
                    int amountToShift;

                    /* If last bit has info */
                    if ((pressed & 0x1) != 0)
                    {
                        callEvent = true;
                        isPressed = true;
                    }
                    else if ((released & 0x1) != 0)
                    {
                        callEvent = true;
                        isPressed = false;
                    }
                    else
                    {
                        callEvent = false;
                        isPressed = false;
                    }

                    if (callEvent)
                    {
                        keySubscription[keyIndex]?.Invoke((KeyFunctionsIndex)keyIndex, isPressed);
                    }

                    /* Shift one element if next has info, or two if next has no info */
                    amountToShift = 1;
                    amountToShift += 0x1 & (~(combiPressedReleased >> 1));

                    combiPressedReleased >>= amountToShift;
                    pressed >>= amountToShift;
                    released >>= amountToShift;
                    keyIndex += amountToShift;
                }
            }
        }


        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
                VARMAP_InputMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_InputMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_InputMaster);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
