
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.VARMAP.DefaultValues
{

    public abstract partial class VARMAP_DefaultValues : VARMAP
    {
        public static GameOptionsStruct GameOptionsStruct_Default => new()
        {
            keyOptions = new KeyOptions()
            {
                changeActionKey = KeyCode.Tab,
                selectKey = KeyCode.Mouse0,
                inventoryKey = KeyCode.Mouse1,
                dragKey = KeyCode.Mouse2,
                zoomUpKey = KeyCode.WheelUp,
                zoomDownKey = KeyCode.WheelDown,
                pauseKey = KeyCode.P
            },
            timeMultiplier = 60f*24f,
            rectangleSelectionColor = new Color(0,0,1,0.12f)
        };


        public static KeyStruct KeyStruct_Default => new()
        {
            pressedKeys = 0,
            cyclepressedKeys = 0,
            cyclereleasedKeys = 0
        };

        public static CameraDispositionStruct CameraDispositionStruct_Default => new()
        {
            position = Vector2.zero,
            orthoSize = 5f
        };

        public static MousePropertiesStruct MouseProperties_Default => new MousePropertiesStruct() { pos1 = Vector2.zero, pos2 = Vector2.zero };
    }
}
