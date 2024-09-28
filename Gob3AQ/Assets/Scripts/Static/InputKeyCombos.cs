using UnityEngine;
using MVerse.VARMAP.Types;
using System;

namespace MVerse.InputMaster
{
    public struct KeyComboAndCombo
    {
        public KeyFunctions[] comboArray;
        public KeyCombo comboName;
    }

    public static class ComboHolder
    {
        private static readonly KeyFunctions[] ObserveOtherWorld =
        {
            KeyFunctions.KEYFUNC_DOWN,
            KeyFunctions.KEYFUNC_UP,
            KeyFunctions.KEYFUNC_SPELL
        };

        private static readonly KeyFunctions[] ChangeToOtherWorld =
        {
            KeyFunctions.KEYFUNC_DOWN,
            KeyFunctions.KEYFUNC_UP,
            KeyFunctions.KEYFUNC_UP,
            KeyFunctions.KEYFUNC_SPELL
        };

        private static readonly KeyFunctions[] RightSuperAttackArray =
        {
            KeyFunctions.KEYFUNC_DOWN,
            KeyFunctions.KEYFUNC_LEFT,
            KeyFunctions.KEYFUNC_RIGHT,
            KeyFunctions.KEYFUNC_ATTACK
        };

        private static readonly KeyFunctions[] RightRollArray =
        {
            KeyFunctions.KEYFUNC_DOWN,
            KeyFunctions.KEYFUNC_LEFT,
            KeyFunctions.KEYFUNC_RIGHT,
            KeyFunctions.KEYFUNC_RIGHT
        };

        private static readonly KeyFunctions[] LeftSuperAttackArray =
        {
            KeyFunctions.KEYFUNC_DOWN,
            KeyFunctions.KEYFUNC_RIGHT,
            KeyFunctions.KEYFUNC_LEFT,
            KeyFunctions.KEYFUNC_ATTACK
        };

        private static readonly KeyFunctions[] RightFastForwardArray =
        {
            KeyFunctions.KEYFUNC_RIGHT,
            KeyFunctions.KEYFUNC_RIGHT
        };

        private static readonly KeyFunctions[] LeftFastForwardArray =
        {
            KeyFunctions.KEYFUNC_LEFT,
            KeyFunctions.KEYFUNC_LEFT
        };

        private static readonly KeyFunctions[] ClimbWallRight =
        {
            KeyFunctions.KEYFUNC_UP,
            KeyFunctions.KEYFUNC_RIGHT,
            KeyFunctions.KEYFUNC_JUMP
        };

        private static readonly KeyFunctions[] ClimbWallLeft =
        {
            KeyFunctions.KEYFUNC_UP,
            KeyFunctions.KEYFUNC_LEFT,
            KeyFunctions.KEYFUNC_JUMP
        };

        public static int CombosForKeyCombination(int combokeys)
        {
            int combos;

            int inputminus1 = combokeys - 2;
            if (KeyCombos[inputminus1] != null)
            {
                combos = KeyCombos[inputminus1].Length;
            }
            else
            {
                combos = 0;
            }
            

            return combos;
        }

        public static ReadOnlySpan<KeyFunctions> GetComboArray(int combokeys, int index, out KeyCombo comboName)
        {
            ReadOnlySpan<KeyFunctions> retVal = null;
            comboName = KeyCombo.KEY_COMBO_NONE;

            int inputminus1 = combokeys - 2;

            KeyComboAndCombo[] combosForKeys = KeyCombos[inputminus1];

            if (combosForKeys != null)
            {
                retVal = combosForKeys[index].comboArray;
                comboName = combosForKeys[index].comboName;
            }

            return retVal;
        }

        private static readonly KeyComboAndCombo[] KeyCombos4 =
        {
            /* Super attack Left */
            new KeyComboAndCombo()
            {
                comboArray = LeftSuperAttackArray,
                comboName = KeyCombo.KEY_COMBO_SUPER_ATTACK_LEFT
            },
            /* Super attack Right */
            new KeyComboAndCombo()
            {
                comboArray = RightSuperAttackArray,
                comboName = KeyCombo.KEY_COMBO_SUPER_ATTACK_RIGHT
            },
            /* Roll Right */
            new KeyComboAndCombo()
            {
                comboArray = RightRollArray,
                comboName = KeyCombo.KEY_COMBO_ROLL_RIGHT
            },
            /* Change to Otherworld (Dimension change) */
            new KeyComboAndCombo()
            {
                comboArray = ChangeToOtherWorld,
                comboName = KeyCombo.KEY_COMBO_OTHER_WORLD
            },
        };

        private static readonly KeyComboAndCombo[] KeyCombos3 =
        {
            /* Visit to Otherworld (Static Dimension change) */
            new KeyComboAndCombo()
            {
                comboArray = ObserveOtherWorld,
                comboName = KeyCombo.KEY_COMBO_OBSERVE_OTHER_WORLD
            },
            /* Climb wall left (jump from wall) */
            new KeyComboAndCombo()
            {
                comboArray = ClimbWallLeft,
                comboName = KeyCombo.KEY_COMBO_CLIMB_WALL_LEFT
            },

            /* Climb wall right (jump from wall) */
            new KeyComboAndCombo()
            {
                comboArray = ClimbWallRight,
                comboName = KeyCombo.KEY_COMBO_CLIMB_WALL_RIGHT
            }
        };

        private static readonly KeyComboAndCombo[] KeyCombos2 =
        {
            /* Fast forward Left */
            new KeyComboAndCombo()
            {
                comboArray = LeftFastForwardArray,
                comboName = KeyCombo.KEY_COMBO_FAST_FORWARD_LEFT
            },
            /* Fast forward Right */
            new KeyComboAndCombo()
            {
                comboArray = RightFastForwardArray,
                comboName = KeyCombo.KEY_COMBO_FAST_FORWARD_RIGHT
            }
        };

        private static readonly KeyComboAndCombo[][] KeyCombos =
        {
            KeyCombos2,
            KeyCombos3,
            KeyCombos4
        };

    }
}