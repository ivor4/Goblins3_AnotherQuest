using System;
using System.IO;


namespace MVerse.Libs.CRC32
{
    public struct Digest
    {
        private uint result;

        public static Digest BLANK_DIGEST => new Digest() { result = 0xFFFFFFFF};

        public uint PartialResult => result;
        public uint CRC32_Result => ~result;

        public void UpdateResult(uint res)
        {
            result = res;
        }
    }

    public static class CRC32
    {
        private static uint[] ChecksumTable;
        private const uint Polynomial = 0xEDB88320;

        public static Digest CreateDigest => Digest.BLANK_DIGEST;


        public static void Initialize()
        {
            ChecksumTable = new uint[0x100];

            for (uint index = 0; index < 0x100; ++index)
            {
                uint item = index;
                for (int bit = 0; bit < 8; ++bit)
                    item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
                ChecksumTable[index] = item;
            }
        }

        public static void UpdateDigest(ref Digest digest, ref ReadOnlySpan<byte> stream, int size)
        {
            uint result = digest.PartialResult;

            int cycleSize = 0;

            while (cycleSize < size)
            {
                result = ChecksumTable[(result & 0xFF) ^ stream[cycleSize]] ^ (result >> 8);
                cycleSize++;
            }

            digest.UpdateResult(result);
        }

        public static uint ComputeHash(ref ReadOnlySpan<byte> stream, int size)
        {
            uint result = 0xFFFFFFFFU;

            int processedSize = 0;

            while (processedSize < size)
            {
                result = ChecksumTable[(result & 0xFF) ^ stream[processedSize]] ^ (result >> 8);
                processedSize++;
            }

            return ~result;
        }
    }
}
