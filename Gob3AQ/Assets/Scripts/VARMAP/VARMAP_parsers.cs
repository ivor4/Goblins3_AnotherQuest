using System;
using UnityEditor;
using UnityEngine;

namespace MVerse.VARMAP.Types.Parsers
{
    public static class VARMAP_parsers
    {
        public static void Game_Status_ParseToBytes(ref Game_Status value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Game_Status_ParseFromBytes(ref Game_Status value, ref ReadOnlySpan<byte> reader)
        {
            value = (Game_Status)BitConverter.ToInt32(reader);
        }

        public static void OtherWorldMode_ParseToBytes(ref OtherWorldMode value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void OtherWorldMode_ParseFromBytes(ref OtherWorldMode value, ref ReadOnlySpan<byte> reader)
        {
            value = (OtherWorldMode)BitConverter.ToInt32(reader);
        }


        public static void KeyCombo_ParseToBytes(ref KeyCombo value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void KeyCombo_ParseFromBytes(ref KeyCombo value, ref ReadOnlySpan<byte> reader)
        {
            value = (KeyCombo)BitConverter.ToInt32(reader);
        }

        public static void Room_ParseToBytes(ref Room value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Room_ParseFromBytes(ref Room value, ref ReadOnlySpan<byte> reader)
        {
            value = (Room)BitConverter.ToInt32(reader);
        }

        public static void Charm_ParseToBytes(ref Charm value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Charm_ParseFromBytes(ref Charm value, ref ReadOnlySpan<byte> reader)
        {
            value = (Charm)BitConverter.ToInt32(reader);
        }

        public static void Powers_ParseToBytes(ref Powers value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (int)value);
        }

        public static void Powers_ParseFromBytes(ref Powers value, ref ReadOnlySpan<byte> reader)
        {
            value = (Powers)BitConverter.ToInt32(reader);
        }

        public static void byte_ParseToBytes(ref byte value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (char)value);
        }

        public static void byte_ParseFromBytes(ref byte value, ref ReadOnlySpan<byte> reader)
        {
            value = (byte)BitConverter.ToChar(reader);
        }

        public static void sbyte_ParseToBytes(ref sbyte value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, (char)value);
        }

        public static void sbyte_ParseFromBytes(ref sbyte value, ref ReadOnlySpan<byte> reader)
        {
            value = (sbyte)BitConverter.ToChar(reader);
        }

        public static void bool_ParseToBytes(ref bool value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void bool_ParseFromBytes(ref bool value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToBoolean(reader);
        }

        public static void ushort_ParseToBytes(ref ushort value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void ushort_ParseFromBytes(ref ushort value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt16(reader);
        }

        public static void short_ParseToBytes(ref short value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void short_ParseFromBytes(ref short value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt16(reader);
        }

        public static void uint_ParseToBytes(ref uint value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void uint_ParseFromBytes(ref uint value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt32(reader);
        }

        public static void int_ParseToBytes(ref int value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void int_ParseFromBytes(ref int value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt32(reader);
        }


        public static void ulong_ParseToBytes(ref ulong value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void ulong_ParseFromBytes(ref ulong value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToUInt64(reader);
        }

        public static void long_ParseToBytes(ref long value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void long_ParseFromBytes(ref long value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToInt64(reader);
        }

        public static void float_ParseToBytes(ref float value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void float_ParseFromBytes(ref float value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToSingle(reader);
        }

        public static void double_ParseToBytes(ref double value, ref Span<byte> writer)
        {
            BitConverter.TryWriteBytes(writer, value);
        }

        public static void double_ParseFromBytes(ref double value, ref ReadOnlySpan<byte> reader)
        {
            value = BitConverter.ToDouble(reader);
        }
    }
}