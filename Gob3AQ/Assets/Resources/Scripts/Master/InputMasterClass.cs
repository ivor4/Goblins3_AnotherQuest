using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.VARMAP.InputMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.DefaultValues;

namespace Gob3AQ.InputMaster
{
    public class InputMasterClass : MonoBehaviour
    {
        private static InputMasterClass _singleton;
        private KeyOptions cachedKeyOptions;
        private KeyStruct cachedPressedKeys;
        private KeyFunctions accumulatedDownkeys;
        private MousePropertiesStruct cachedMouseProps;
        private float ellapsedMillis;


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
            cachedPressedKeys = new KeyStruct() { cyclepressedKeys = 0, cyclereleasedKeys = 0, pressedKeys = 0};
            cachedKeyOptions = VARMAP_InputMaster.GET_GAME_OPTIONS().keyOptions;
            cachedMouseProps = VARMAP_DefaultValues.MouseProperties_Default;
            ellapsedMillis = 0f;
            accumulatedDownkeys = 0;


            VARMAP_InputMaster.REG_GAME_OPTIONS(_GameOptionsChanged);
            VARMAP_InputMaster.SET_PRESSED_KEYS(new KeyStruct() { pressedKeys = 0 });
        }

        private void Update()
        {
            if (GameFixedConfig.PERIPH_PC)
            {
                KeyFunctions pressedandreleasedKeys;
                bool accumulationCycle;
                float deltaTime;

                deltaTime = Time.deltaTime;

                ellapsedMillis += deltaTime;

                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.upKey) ? KeyFunctions.KEYFUNC_UP : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.downKey) ? KeyFunctions.KEYFUNC_DOWN : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.leftKey) ? KeyFunctions.KEYFUNC_LEFT : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.rightKey) ? KeyFunctions.KEYFUNC_RIGHT : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.jumpKey) ? KeyFunctions.KEYFUNC_JUMP : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.attackKey) ? KeyFunctions.KEYFUNC_ATTACK : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.spellKey) ? KeyFunctions.KEYFUNC_SPELL : 0;
                accumulatedDownkeys |= Input.GetKey(cachedKeyOptions.pauseKey) ? KeyFunctions.KEYFUNC_PAUSE : 0;


                pressedandreleasedKeys = cachedPressedKeys.cyclepressedKeys | cachedPressedKeys.cyclereleasedKeys;
                accumulationCycle = ellapsedMillis >= GameFixedConfig.KEY_REFRESH_TIME_SECONDS;
                

                if ((accumulationCycle && (accumulatedDownkeys != cachedPressedKeys.pressedKeys)) || (pressedandreleasedKeys != 0))
                {
                    cachedPressedKeys.cyclepressedKeys = (accumulatedDownkeys ^ cachedPressedKeys.pressedKeys) & accumulatedDownkeys;
                    cachedPressedKeys.cyclereleasedKeys = (accumulatedDownkeys ^ cachedPressedKeys.pressedKeys) & cachedPressedKeys.pressedKeys;
                    cachedPressedKeys.pressedKeys = accumulatedDownkeys;
                }

                if (accumulationCycle)
                {
                    ellapsedMillis = 0f;
                    accumulatedDownkeys = 0;
                }


                Vector2 mousePosition = Input.mousePosition;
                bool mousenowpressed = Input.GetMouseButton(0);

                cachedMouseProps.primaryPressed = (!cachedMouseProps.primaryPressing) & mousenowpressed;
                cachedMouseProps.primaryReleased = cachedMouseProps.primaryPressing & (!mousenowpressed);
                cachedMouseProps.primaryPressing = mousenowpressed;

                cachedMouseProps.pos1 = mousePosition;
                cachedMouseProps.pos2 = mousePosition;


                VARMAP_InputMaster.SET_PRESSED_KEYS(cachedPressedKeys);
                VARMAP_InputMaster.SET_MOUSE_PROPERTIES(cachedMouseProps);
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

        private static void _GameOptionsChanged(ChangedEventType evtype, ref GameOptionsStruct oldval, ref GameOptionsStruct newval)
        {
            if (evtype == ChangedEventType.CHANGED_EVENT_SET)
            {
                _singleton.cachedKeyOptions = newval.keyOptions;
            }
        }

        public static void ResetPressedKeysService()
        {
            _singleton.cachedPressedKeys.pressedKeys = 0;
            _singleton.cachedPressedKeys.cyclepressedKeys = 0;
            _singleton.cachedPressedKeys.cyclereleasedKeys = 0;
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
