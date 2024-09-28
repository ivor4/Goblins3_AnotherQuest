using MVerse.Libs.CRC32;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MVerse.VARMAP.Variable.IstreamableNamespace
{
    public delegate IStreamable StreamableNewInstanceDelegate();
    public interface IStreamable
    {
        public void ParseToBytes(ref Span<byte> streamwriter);
        public void ParseFromBytes(ref ReadOnlySpan<byte> streamreader);
        public int GetElemSize();

        public IStreamable CreateNewInstance()
        {
            throw new Exception("Not implemented, must be implemented on structs/classes which use this interface");
        }

       
    }
}
