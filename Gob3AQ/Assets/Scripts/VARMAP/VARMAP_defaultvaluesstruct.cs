
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Gob3AQ.VARMAP.DefaultValues
{

    public abstract partial class VARMAP_DefaultValues : VARMAP
    {
        public static GameOptionsStruct GameOptionsStruct_Default => new()
        {
            keyOptions = new KeyOptions()
            {
                changeActionKey = Key.Tab,
                pauseKey = Key.P
            },
            mouseOptions = new MouseOptions()
            {
                selectKey = MouseButton.Left,
                inventoryKey = MouseButton.Right,
                dragKey = MouseButton.Middle,
                zoomUpKey = MouseButton.Forward
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
            orthoSize = 5f,
            room = Room.ROOM_NONE
        };

        public static MousePropertiesStruct MouseProperties_Default => new MousePropertiesStruct() { pos1 = Vector2.zero, pos2 = Vector2.zero };
    }
}
