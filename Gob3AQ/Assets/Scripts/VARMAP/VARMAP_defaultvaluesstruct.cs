
using MVerse.VARMAP.Types;
using System.Collections.Generic;
using UnityEngine;

namespace MVerse.VARMAP.DefaultValues
{

    public abstract partial class VARMAP_DefaultValues : VARMAP
    {
        public static GameOptionsStruct GameOptionsStruct_Default => new GameOptionsStruct()
        {
            keyOptions = new KeyOptions()
            {
                upKey = KeyCode.UpArrow,
                downKey = KeyCode.DownArrow,
                leftKey = KeyCode.LeftArrow,
                rightKey = KeyCode.RightArrow,
                jumpKey = KeyCode.L,
                attackKey = KeyCode.K,
                spellKey = KeyCode.J,
                pauseKey = KeyCode.P
            },
            timeMultiplier = 60f*24f,
            rectangleSelectionColor = new Color(0,0,1,0.12f)
        };


        public static KeyStruct KeyStruct_Default => new KeyStruct()
        {
            pressedKeys = 0
        };

        public static Vector3Struct Vector3Struct_Default => new Vector3Struct()
        {
            position = Vector3.zero
        };

        public static MousePropertiesStruct MouseProperties_Default => new MousePropertiesStruct() { pos1 = Vector2.zero, pos2 = Vector2.zero };
    }
}
