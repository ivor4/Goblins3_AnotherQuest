using System;
using UnityEditor;
using UnityEngine;

namespace Gob3AQ.VARMAP.Types.Parsers
{
    public static class VARMAP_parsers
    {
        public static void CharacterType_ParseToBytes(in CharacterType value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (char)value);
        }
        public static void CharacterType_ParseFromBytes(ref CharacterType value, ref ReadOnlySpan<byte> reader)
        {
            value = (CharacterType)BitConverter.ToChar(reader);
        }

        public static void GamePickableItem_ParseToBytes(in GamePickableItem value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void GamePickableItem_ParseFromBytes(ref GamePickableItem value, ref ReadOnlySpan<byte> reader)
        {
            value = (GamePickableItem)BitConverter.ToInt32(reader);
        }

        public static void GameItem_ParseToBytes(in GameItem value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void GameItem_ParseFromBytes(ref GameItem value, ref ReadOnlySpan<byte> reader)
        {
            value = (GameItem)BitConverter.ToInt32(reader);
        }

        public static void GameEvent_ParseToBytes(in GameEvent value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void GameEvent_ParseFromBytes(ref GameEvent value, ref ReadOnlySpan<byte> reader)
        {
            value = (GameEvent)BitConverter.ToInt32(reader);
        }

        public static void Game_Status_ParseToBytes(in Game_Status value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Game_Status_ParseFromBytes(ref Game_Status value, ref ReadOnlySpan<byte> reader)
        {
            value = (Game_Status)BitConverter.ToInt32(reader);
        }

        public static void UserInputInteraction_ParseToBytes(in UserInputInteraction value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void UserInputInteraction_ParseFromBytes(ref UserInputInteraction value, ref ReadOnlySpan<byte> reader)
        {
            value = (UserInputInteraction)BitConverter.ToInt32(reader);
        }


        public static void Room_ParseToBytes(in Room value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Room_ParseFromBytes(ref Room value, ref ReadOnlySpan<byte> reader)
        {
            value = (Room)BitConverter.ToInt32(reader);
        }


        public static void byte_ParseToBytes(in byte value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (char)value);
        }

        public static void byte_ParseFromBytes(ref byte value, ref ReadOnlySpan<byte> reader)
        {
            value = (byte)BitConverter.ToChar(reader);
        }

        public static void sbyte_ParseToBytes(in sbyte value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (char)value);
        }

        public static void sbyte_ParseFromBytes(ref sbyte value, ref ReadOnlySpan<byte> reader)
        {
            value = (sbyte)BitConverter.ToChar(reader);
        }

        public static void bool_ParseToBytes(in bool value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void bool_ParseFromBytes(ref bool value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToBoolean(reader);
        }

        public static void ushort_ParseToBytes(in ushort value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void ushort_ParseFromBytes(ref ushort value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt16(reader);
        }

        public static void short_ParseToBytes(in short value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void short_ParseFromBytes(ref short value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt16(reader);
        }

        public static void uint_ParseToBytes(in uint value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void uint_ParseFromBytes(ref uint value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt32(reader);
        }

        public static void int_ParseToBytes(in int value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void int_ParseFromBytes(ref int value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt32(reader);
        }


        public static void ulong_ParseToBytes(in ulong value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void ulong_ParseFromBytes(ref ulong value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt64(reader);
        }

        public static void long_ParseToBytes(in long value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void long_ParseFromBytes(ref long value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt64(reader);
        }

        public static void float_ParseToBytes(in float value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void float_ParseFromBytes(ref float value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToSingle(reader);
        }

        public static void double_ParseToBytes(in double value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void double_ParseFromBytes(ref double value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToDouble(reader);
        }
    }
}