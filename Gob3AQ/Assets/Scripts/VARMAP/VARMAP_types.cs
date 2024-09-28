using MVerse.VARMAP.Variable.IstreamableNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MVerse.Libs.Arith;

namespace MVerse.VARMAP.Types
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
    public delegate void AddVARMAPArrayElemDelegate<T>(T newval);
    public delegate void RemoveVARMAPArrayElemDelegate<T>(T val);
    public delegate void RemoveVARMAPArrayElemAtPosDelegate(int pos);
    public delegate void InsertVARMAPArrayElemDelegate<T>(int pos, T newval);
    public delegate void ClearVARMAPArrayDelegate();


    public enum ChangedEventType
    {
        CHANGED_EVENT_NONE,
        CHANGED_EVENT_SET,
        CHANGED_EVENT_SET_LIST_ELEM
    }

    public enum OtherWorldMode
    {
        OTHER_WORLD_OBSERVE,
        OTHER_WORLD_STAY
    }

    public enum KeyCombo
    {
        KEY_COMBO_NONE,
        KEY_COMBO_SUPER_ATTACK_LEFT,
        KEY_COMBO_SUPER_ATTACK_RIGHT,
        KEY_COMBO_FAST_FORWARD_LEFT,
        KEY_COMBO_FAST_FORWARD_RIGHT,
        KEY_COMBO_ROLL_RIGHT,
        KEY_COMBO_OBSERVE_OTHER_WORLD,
        KEY_COMBO_OTHER_WORLD,
        KEY_COMBO_CLIMB_WALL_LEFT,
        KEY_COMBO_CLIMB_WALL_RIGHT,

        KEY_COMBO_TOTAL
    }

    public enum Charm
    {
        CHARM_NONE,
        CHARM_INPU_OTHER_WORLD = 1,
        CHARM_NEIT_SHOT = 2,
        CHARM_BASTET_CLIMB = 4
    }

    public enum Powers_Index
    {
        POWER_INDEX_SHOT,
        POWER_INDEX_DOUBLE_JUMP,
        POWER_INDEX_CLIMB
    }

    public enum Powers
    {
        POWER_NONE = 0,
        POWER_SHOT = 1 << Powers_Index.POWER_INDEX_SHOT,
        POWER_DOUBLE_JUMP = 1 << Powers_Index.POWER_INDEX_DOUBLE_JUMP,
        POWER_CLIMB = 1 << Powers_Index.POWER_INDEX_CLIMB
    }

    public enum Room
    {
        NONE,
        ROOM_1_ORIGIN,
        
        ROOM_SPACE_MAMA,

        TOTAL_ROOMS
    }

    public enum OBFight
    {
        NONE,
        OBF_SPACE_MAMA,

        TOTAL_OBF
    }

    public enum CollisionLayer
    {
        LAYER_SOLID = 6,
        LAYER_PLAYER = 7,
        LAYER_ENEMY = 8,
        LAYER_ENEMY_AS_SOLID = 9,
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




    public struct MousePropertiesStruct : IStreamable
    {
        public const int STRUCT_SIZE = 2*2*sizeof(float);

        public Vector2 pos1;
        public Vector2 pos2;

        public static void StaticParseFromBytes(ref MousePropertiesStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.pos1 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.pos2 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
        }

        public static void StaticParseToBytes(ref MousePropertiesStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.y);
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
        public KeyCombo activeCombo;



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
            gstruct.activeCombo = (KeyCombo)BitConverter.ToInt32(readZone.ReadNext(sizeof(int)));
        }

        public static void StaticParseToBytes(ref KeyStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.pressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclepressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclereleasedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(int)), (int)gstruct.activeCombo);
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
