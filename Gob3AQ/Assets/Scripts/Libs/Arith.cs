using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MVerse.Libs.Arith
{
    public static class Sorting<T> where T : IComparable
    {
        public static void SortArray(Span<T> span)
        {
            for(int i=0;i<(span.Length-1);i++)
            {
                for(int e=0;e<(span.Length-i-1);e++)
                {
                    if(span[e].CompareTo(span[e+1]) > 0)
                    {
                        T temp = span[e + 1];
                        span[e + 1] = span[e];
                        span[e] = temp;
                    }
                }
            }
        }
    }

    public struct ReadOnlyArray<T>
    {
        private T[] array;

        public int Length => array.Length;

        public ReadOnlyArray(T[] array)
        {
            this.array = array;
        }

        public T this[int index]
        {
            get
            {
                return array[index];
            }
        }
    }

    public ref struct ClearableArray<T>
    {
        public Span<T> span;
        public int Length => span.Length;
        
        public T this[int index]
        {
            get
            {
                return span[index];
            }

            set
            {
                span[index] = value;
            }
        }

        public ClearableArray(Span<T> pointer)
        {
            span = pointer;
        }

        public void Clear(T value)
        {
            for(int i=0;i<span.Length;i++)
            {
                span[i] = value;
            }
        }
    }

    public ref struct ReadStreamSpan<T>
    {
        private ReadOnlySpan<T> _readSpan;
        private ReadOnlySpan<T> _readZone;
        private int _index;

        public ReadStreamSpan(ReadOnlySpan<T> readspan)
        {
            _readSpan = readspan;
            _readZone = readspan;
            _index = 0;
        }

        public ReadOnlySpan<T> ReadNext(int length)
        {
            _readZone = _readSpan.Slice(_index);
            ReadOnlySpan<T> retVal = _readZone;
            _index += length;
            return retVal;
        }

        public void Seek(int index)
        {
            _index = index;
        }
    }

    public ref struct WriteStreamSpan<T>
    {
        private Span<T> _readSpan;
        private Span<T> _readZone;
        private int _index;

        public WriteStreamSpan(Span<T> readspan)
        {
            _readSpan = readspan;
            _readZone = readspan;
            _index = 0;
        }

        public Span<T> WriteNext(int length)
        {
            _readZone = _readSpan.Slice(_index);
            Span<T> retVal = _readZone;
            _index += length;
            return retVal;
        }

        public void Seek(int index)
        {
            _index = index;
        }
    }




    public ref struct SpecialTriangulatorStackArray
    {
        private Span<Vector3> span;
        private bool twistYZ;


        public Vector3 this[int index] => TwistYZ(span[index]);
        public int Length => span.Length;


        public SpecialTriangulatorStackArray(Span<Vector3> spanPointer, bool twist)
        {
            span = spanPointer;
            twistYZ = twist;
        }

        private Vector3 TwistYZ(Vector3 input)
        {
            if (twistYZ)
            {
                return new Vector3(input.x, input.z, input.y);
            }
            else
            {
                return input;
            }
        }
    }
    public ref struct GrowingStackArray<T>
    {
        public Span<T> span;
        public int spanAddedElems;
        public T this[int index]
        {
            get
            {
                return span[index];
            }
            set
            {
                span[index] = value;
            }
        }

        public int Length => span.Length;
        public int Count => spanAddedElems;

        public GrowingStackArray(Span<T> assign, bool clear)
        {
            span = assign;
            spanAddedElems = 0;

            if(clear)
            {
                Clear();
            }
        }

        public void Add(T value)
        {
           span[spanAddedElems++] = value;
        }

        public void Clear()
        {
            Clear(true, default(T));
        }

        public void Clear(bool realClear, T clearValue)
        {
            if (realClear)
            {
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = clearValue;
                }
            }

            spanAddedElems = 0;
        }
    }

    public struct MinMax
    {
        public float min;
        public float max;

        public static MinMax PreparedStruct => new MinMax() { min = float.MaxValue, max = float.MinValue };

        public void Witness(float newvalue)
        {
            if(newvalue < min)
            {
                min = newvalue;
            }

            if(newvalue > max)
            {
                max = newvalue;
            }
        }
    }

    public struct MinMaxVector3
    {
        public Vector3 min;
        public Vector3 max;
        public static MinMaxVector3 PreparedStruct => new MinMaxVector3() { min = float.MaxValue * Vector3.one, max = float.MinValue * Vector3.one };

        public void Witness(Vector3 newvalue)
        {
            if (newvalue.x < min.x)
            {
                min.x = newvalue.x;
            }

            if (newvalue.x > max.x)
            {
                max.x = newvalue.x;
            }

            if (newvalue.y < min.y)
            {
                min.y = newvalue.y;
            }

            if (newvalue.y > max.y)
            {
                max.y = newvalue.y;
            }

            if (newvalue.z < min.z)
            {
                min.z = newvalue.z;
            }

            if (newvalue.z > max.z)
            {
                max.z = newvalue.z;
            }
        }
    }
}
