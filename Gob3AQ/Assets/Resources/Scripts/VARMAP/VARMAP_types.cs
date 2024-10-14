using Gob3AQ.VARMAP.Variable.IstreamableNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Gob3AQ.Libs.Arith;

namespace Gob3AQ.VARMAP.Types
{
    public delegate void VARMAPValueChangedEvent<T>(ChangedEventType eventType, ref T oldval, ref T newval);
    public delegate T GetVARMAPValueDelegate<T>();
    public delegate void SetVARMAPValueDelegate<T>(T newValue);
    public delegate void ReUnRegisterVARMAPValueChangeEventDelegate<T>(VARMAPValueChangedEvent<T> callback);
    public delegate T GetVARMAPArrayElemValueDelegate<T>(int pos);
    public delegate void SetVARMAPArrayElemValueDelegate<T>(int pos, T newval);
    public delegate int GetVARMAPArraySizeDelegate();
    public delegate ReadOnlySpan<T> GetVARMAPArrayDelegate<T>();
    public delegate void SetVARMAPArrayDelegate<T>(List<T> newvals);


    public enum ChangedEventType
    {
        CHANGED_EVENT_NONE,
        CHANGED_EVENT_SET,
        CHANGED_EVENT_SET_LIST_ELEM
    }




    public enum Room
    {
        NONE,
        ROOM_1_ORIGIN,
        
        ROOM_SPACE_MAMA,

        TOTAL_ROOMS
    }



     public enum KeyFunctions
    {
        KEYFUNC_NONE = 0,
        KEYFUNC_UP = 1<<0,
        KEYFUNC_DOWN = 1<<1,
        KEYFUNC_LEFT = 1<<2,
        KEYFUNC_RIGHT = 1<<3,
        KEYFUNC_JUMP = 1<<4,
        KEYFUNC_ATTACK = 1<<5,
        KEYFUNC_SPELL = 1<<6,
        KEYFUNC_PAUSE = 1<<7
    }



    public enum Game_Status
    {
        GAME_STATUS_STOPPED,
        GAME_STATUS_PLAY,
        GAME_STATUS_PLAY_FREEZE,
        GAME_STATUS_PAUSE,
        GAME_STATUS_LOADING
    }

    public enum InteractionItemType
    {
        INTERACTION_NONE,
        INTERACTION_TAKE,
        INTERACTION_USE
    }

    public enum CharacterType
    {
        CHARACTER_NONE,
        CHARACTER_MAIN,
        CHARACTER_PARROT,
        CHARACTER_SNAKE,

        CHARACTER_TOTAL
    }

    

    public enum GameItem
    {
        ITEM_NONE,

        ITEM_POTION,
        ITEM_FOUNTAIN,

        ITEM_TOTAL
    }

    public enum GamePickableItem
    {
        ITEM_PICK_NONE,

        ITEM_PICK_POTION,

        ITEM_PICK_TOTAL
    }

    public enum ItemUsageType
    {
        PLAYER_WITH_ITEM,
        ITEM_WITH_ITEM,
        ITEM_WITH_PLAYER,
        ITEM_WITH_NPC
    }

    public enum GameEvent
    {
        GEVENT_DOOR1_OPENED,
        GEVENT_TALK_MAN
    }

    public readonly struct ItemUsage
    {
        public readonly ItemUsageType type;
        public readonly CharacterType playerSource;
        public readonly GameItem itemSource;
        public readonly CharacterType playerDest;
        public readonly CharacterType npcDest;
        public readonly GameItem itemDest;

        public ItemUsage(ItemUsageType type, CharacterType playerSource, GameItem itemSource,
            CharacterType playerDest, CharacterType npcDest, GameItem itemDest)
        {
            this.type = type;
            this.playerSource = playerSource;
            this.itemSource = itemSource;
            this.playerDest = playerDest;
            this.npcDest = npcDest;
            this.itemDest = itemDest;
        }
    }

    public struct MousePropertiesStruct : IStreamable
    {
        public const int STRUCT_SIZE = 2*2*sizeof(float) + 1*2 * sizeof(int) + 6*sizeof(bool);

        public Vector2 pos1;
        public Vector2 pos2;

        public Vector2Int posPixels;

        public bool primaryPressed;
        public bool primaryPressing;
        public bool primaryReleased;
        public bool secondaryPressed;
        public bool secondaryPressing;
        public bool secondaryReleased;


        public static void StaticParseFromBytes(ref MousePropertiesStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.pos1 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.pos2 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.posPixels = new Vector2Int(BitConverter.ToInt32(readZone.ReadNext(sizeof(int))), BitConverter.ToInt32(readZone.ReadNext(sizeof(int))));
            gstruct.primaryPressed = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.primaryPressing = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.primaryReleased = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryPressed = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryPressing = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryReleased = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
        }

        public static void StaticParseToBytes(ref MousePropertiesStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(int)), gstruct.posPixels.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(int)), gstruct.posPixels.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryPressed);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryPressing);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryReleased);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryPressed);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryPressing);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryReleased);
        }

        public static IStreamable CreateNewInstance()
        {
            return new MousePropertiesStruct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }

    public struct Vector3Struct : IStreamable
    {
        public const int STRUCT_SIZE =  3 * sizeof(float);
        public Vector3 position;

        public static void StaticParseFromBytes(ref Vector3Struct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.position.x = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.y = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.z = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(ref Vector3Struct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.z);
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new Vector3Struct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }

    public struct MultiBitFieldStruct : IStreamable
    {
        public const int STRUCT_SIZE = sizeof(ulong);
        public ulong bitfield;

        public static void StaticParseFromBytes(ref MultiBitFieldStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);
            gstruct.bitfield = BitConverter.ToUInt64(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(ref MultiBitFieldStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ulong)), gstruct.bitfield);
        }

        public bool GetIndividualBool(int pos)
        {
            bool retVal;
            int pos_corrected;

            pos_corrected = pos & 0x3F;

            retVal = ((bitfield >> pos_corrected) & 0x1ul) != 0x0ul;


            return retVal;
        }

        public ulong GetValueFromOffset(int offset)
        {
            ulong retVal;
            int offset_corrected;

            offset_corrected = offset & 0x3F;

            retVal = bitfield >> offset_corrected;


            return retVal;
        }

        public void SetIndividualBool(int pos, bool value)
        {
            int pos_corrected;
            ulong setbitval;

            pos_corrected = pos & 0x3F;

            setbitval = 0x1ul << pos_corrected;

            if (value)
            {
                bitfield |= setbitval;
            }
            else
            {
                bitfield &= ~setbitval;
            }
        }

        public void SetValueFromOffset(int offset, ulong value, ulong mask)
        {
            int offset_corrected;

            offset_corrected = offset & 0x3F;

            bitfield &= ~(mask << offset_corrected);
            bitfield |= (value & mask) << offset_corrected;
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new MultiBitFieldStruct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }

        public override string ToString()
        {
            string str = bitfield.ToString("X16");
            return str;
        }
    }

    public struct GameOptionsStruct : IStreamable
    {
        public const int STRUCT_SIZE = KeyOptions.STRUCT_SIZE + sizeof(float) + 4 * sizeof(float);
        public KeyOptions keyOptions;
        public float timeMultiplier; /* From computer to game world */
        public Color rectangleSelectionColor;

        public static void StaticParseFromBytes(ref GameOptionsStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            KeyOptions.StaticParseFromBytes(ref gstruct.keyOptions, ref reader);

            _ = readZone.ReadNext(KeyOptions.STRUCT_SIZE);

            gstruct.timeMultiplier = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));

            gstruct.rectangleSelectionColor.r = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.g = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.b = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.a = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(ref GameOptionsStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            KeyOptions.StaticParseToBytes(ref gstruct.keyOptions, ref writer);

            _ = writeZone.WriteNext(KeyOptions.STRUCT_SIZE);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.timeMultiplier);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.r);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.g);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.b);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.a);
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new GameOptionsStruct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }

    public struct KeyOptions : IStreamable
    {
        public const int STRUCT_SIZE = 8 * sizeof(ushort);
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        public KeyCode attackKey;
        public KeyCode spellKey;
        public KeyCode pauseKey;
            
            

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new KeyOptions();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public static void StaticParseFromBytes(ref KeyOptions gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.upKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.downKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.leftKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.rightKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.jumpKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.attackKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.spellKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.pauseKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
        }

        public static void StaticParseToBytes(ref KeyOptions gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.upKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.downKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.leftKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.rightKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.jumpKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.attackKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.spellKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.pauseKey);
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }
    

    public struct KeyStruct : IStreamable
    {
        public const int STRUCT_SIZE = 3 * sizeof(uint) + sizeof(int);

        public KeyFunctions pressedKeys;
        public KeyFunctions cyclepressedKeys;
        public KeyFunctions cyclereleasedKeys;



        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public static void StaticParseFromBytes(ref KeyStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.pressedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
            gstruct.cyclepressedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
            gstruct.cyclereleasedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
        }

        public static void StaticParseToBytes(ref KeyStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.pressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclepressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclereleasedKeys);
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }
}
