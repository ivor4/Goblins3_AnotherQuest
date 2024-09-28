using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVerse.VARMAP.InputMaster;
using MVerse.VARMAP.Types;
using MVerse.FixedConfig;
using System;
using MVerse.Libs.Arith;

namespace MVerse.InputMaster
{
    public class InputMasterClass : MonoBehaviour
    {
        private static InputMasterClass _singleton;
        private KeyOptions cachedKeyOptions;
        private KeyStruct cachedPressedKeys;
        private KeyFunctions accumulatedDownkeys;
        private float ellapsedMillis;
        private float comboTimeoutMillis;
        private KeyFunctions[] keyCombo;
        private int keyComboWritten;
        private int keyComboWriteIndex;
        private int keyComboReadIndex;
        private bool keyComboInProgress;


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
            ellapsedMillis = 0f;
            accumulatedDownkeys = 0;

            keyCombo = new KeyFunctions[GameFixedConfig.COMBO_MAX_KEYS];
            ClearCombo();

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

                bool observeComboTimeout = keyComboInProgress;
                
                cachedPressedKeys.activeCombo = KeyCombo.KEY_COMBO_NONE;

                if ((accumulationCycle && (accumulatedDownkeys != cachedPressedKeys.pressedKeys)) || (pressedandreleasedKeys != 0))
                {
                    cachedPressedKeys.cyclepressedKeys = (accumulatedDownkeys ^ cachedPressedKeys.pressedKeys) & accumulatedDownkeys;
                    cachedPressedKeys.cyclereleasedKeys = (accumulatedDownkeys ^ cachedPressedKeys.pressedKeys) & cachedPressedKeys.pressedKeys;

                    /* If some key is pressed, insert in combo list */
                    if(cachedPressedKeys.cyclepressedKeys != KeyFunctions.KEYFUNC_NONE)
                    {
                        UpdateCombo(cachedPressedKeys.cyclepressedKeys);
                        observeComboTimeout = false;
                    }

                    cachedPressedKeys.pressedKeys = accumulatedDownkeys;
                }

                ManageComboTimeout(observeComboTimeout, deltaTime);

                if (accumulationCycle)
                {
                    ellapsedMillis = 0f;
                    accumulatedDownkeys = 0;
                }



                MousePropertiesStruct mouseProps = new MousePropertiesStruct();
                Vector2 mousePosition = Input.mousePosition;
                bool mousenowpressed = Input.GetMouseButton(0);
                bool secondarynowpressed = Input.GetMouseButton(1);

                mouseProps.pos2 = mousePosition;
                mouseProps.pos1 = mousePosition;


                VARMAP_InputMaster.SET_PRESSED_KEYS(cachedPressedKeys);
                VARMAP_InputMaster.SET_MOUSE_PROPERTIES(mouseProps);
            }

        }

        private void ClearCombo()
        {
            keyComboInProgress = false;
            keyComboReadIndex = 0;
            keyComboWriteIndex = 0;
            keyComboWritten = 0;
            comboTimeoutMillis = 0f;
        }

        private void UpdateCombo(KeyFunctions newPressedKeys)
        {
            /* Update array and increase writeIndex */
            int initialKeyComboWriteIndex = keyComboWriteIndex;
            keyCombo[keyComboWriteIndex] = newPressedKeys;
            keyComboWriteIndex = (keyComboWriteIndex + 1) & GameFixedConfig.COMBO_MAX_MASK;
            comboTimeoutMillis = 0f;
            keyComboInProgress = true;

            if(keyComboWritten < GameFixedConfig.COMBO_MAX_KEYS)
            {
                keyComboWritten++;
            }
            
            /* ReadIndex should always be "behind" write index. It will read until WriteIndex. If WriteIndex reaches Read, increase read one position to be N-1 positions behind (circular array) */
            if ((initialKeyComboWriteIndex == keyComboReadIndex)&&(keyComboWritten == GameFixedConfig.COMBO_MAX_KEYS))
            {
                keyComboReadIndex = (keyComboReadIndex + 1) & GameFixedConfig.COMBO_MAX_MASK;
            }

            /* Analyze combo */
            Span<KeyFunctions> observedComboSpan = stackalloc KeyFunctions[GameFixedConfig.COMBO_MAX_KEYS];
            GrowingStackArray<KeyFunctions> observedCombo = new GrowingStackArray<KeyFunctions>(observedComboSpan, false);

            int tempReadIndex = keyComboReadIndex;
            int tempreadIndexes = 0;

            /* Populate actual ongoing combo */

            while (tempreadIndexes < keyComboWritten)
            {
                observedCombo.Add(keyCombo[tempReadIndex]);
                tempReadIndex = (tempReadIndex + 1) & GameFixedConfig.COMBO_MAX_MASK;
                tempreadIndexes++;
            }

            bool foundCombo = false;
            int advancedKeys = 0;
            int length = observedCombo.Count;

            while ((!foundCombo)&&(length >= 2))
            {
                int availableCombosForLength = ComboHolder.CombosForKeyCombination(length);

                for (int i=0;(i<availableCombosForLength)&&(!foundCombo);i++)
                {
                    ReadOnlySpan<KeyFunctions> comboArray = ComboHolder.GetComboArray(length, i, out KeyCombo comboName);

                    foundCombo = true;

                    for(int e=0;(e < comboArray.Length) && foundCombo;e++)
                    {
                        if(observedCombo[advancedKeys+e] != comboArray[e])
                        {
                            foundCombo = false;
                        }
                    }

                    if(foundCombo)
                    {
                        cachedPressedKeys.activeCombo = comboName;
                        ClearCombo();
                    }
                }

                if(!foundCombo)
                {
                    advancedKeys++;
                    length = observedCombo.Count - advancedKeys;
                }
            }
        }

        private void ManageComboTimeout(bool observeTimeout, float deltaTime)
        {
            if (observeTimeout)
            {
                comboTimeoutMillis += deltaTime;

                if (comboTimeoutMillis >= GameFixedConfig.KEY_COMBO_TIMEOUT_SECONDS)
                {
                    ClearCombo();
                }
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
