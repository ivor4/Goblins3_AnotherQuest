
using System;
using MVerse.VARMAP.Types;
using MVerse.VARMAP.Safe;
using MVerse.VARMAP.Enum;
using UnityEngine;
using System.IO;
using System.Collections;
using MVerse.VARMAP.Variable.IstreamableNamespace;
using System.Collections.Generic;
using MVerse.Libs.Cast;
using MVerse.Libs.CRC32;
using MVerse.Libs.Arith;

namespace MVerse.VARMAP.Variable
{

    public abstract class VARMAP_Variable_Indexable : IStreamable
    {
        public abstract void ClearChangeEvent();

        public abstract uint CalcCRC32();

        public abstract void ParseToBytes(ref Span<byte> streamwriter);

        public abstract void ParseFromBytes(ref ReadOnlySpan<byte> streamreader);

        public abstract int GetElemSize();

        public abstract VARMAP_Variable_ID GetID();

        
    }

    public abstract class VARMAP_Variable_Interface<T> : VARMAP_Variable_Indexable
    {
        public delegate void ParseTypeToBytes(ref T refvalue, ref Span<byte> writer);
        public delegate void ParseTypeFromBytes(ref T refvalue, ref ReadOnlySpan<byte> writer);
        public delegate T ConstructorOfType();


        protected ParseTypeToBytes ParseToBytesFunction;
        protected ParseTypeFromBytes ParseFromBytesFunction;
        protected ConstructorOfType ConstructorFunction;

        

        public abstract T GetValue();
        public abstract void SetValue(T newvalue);
        public abstract ReadOnlySpan<T> GetListCopy();
        public abstract void SetListValues(List<T> newList);
        public abstract int GetListSize();
        public abstract void SetListElem(int pos, T newvalue);
        public abstract T GetListElem(int pos);

        public abstract void InitializeListElems(T defaultValue);
        public abstract void RegisterChangeEvent(VARMAPValueChangedEvent<T> callback);
        public abstract void UnregisterChangeEvent(VARMAPValueChangedEvent<T> callback);
    }



    public class VARMAP_SafeArray<T> : VARMAP_Array<T>
    {
        private bool _highSec;
        private uint _IDSec;
        public VARMAP_SafeArray(VARMAP_Variable_ID id, int elems, bool highSecurity, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null) : base(id, elems, parseFromBytesDelegate, parseToBytesDelegate, constructor)
        {
            Common_Constructor(highSecurity);
        }

        private void Common_Constructor(bool highSecurity)
        {
            _highSec = highSecurity;
            _IDSec = VARMAP_Safe.RegisterSecureVariable();
            SecureNewValue();
        }

        public override uint CalcCRC32()
        {
            int crclength = GetElemSize();
            Span<byte> writeZone = stackalloc byte[crclength];
            ReadOnlySpan<byte> readZone = writeZone;

            base.ParseToBytes(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, crclength);

            return crc;
        }

        private void SecureNewValue()
        {
            VARMAP_Safe.SecureNewValue(_IDSec, CalcCRC32(), _highSec);
        }

        private void SecureNewValue(ref ReadOnlySpan<byte> stream)
        {
            uint crc = CRC32.ComputeHash(ref stream, GetElemSize());
            VARMAP_Safe.SecureNewValue(_IDSec, crc, _highSec);
        }

        private bool CheckValue()
        {
            bool retVal;

            if(VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
            {
                retVal = true;
            }
            else
            {
                uint crc;
                crc = CalcCRC32();
                retVal = VARMAP_Safe.CheckSafeValue(_IDSec, crc);
            }

            return retVal;
        }

        private bool CheckValue(ref ReadOnlySpan<byte> stream)
        {
            bool retVal;

            if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
            {
                retVal = true;
            }
            else
            {
                uint crc;
                crc = CRC32.ComputeHash(ref stream, GetElemSize());
                retVal = VARMAP_Safe.CheckSafeValue(_IDSec, crc);
            }

            return retVal;
        }

        public override T GetListElem(int pos)
        {
            T newVal = base.GetListElem(pos);
            if(!CheckValue())
            {
                newVal = default;
            }
            return newVal;
        }

        public override void SetListElem(int pos, T newvalue)
        {
            base.SetListElem(pos, newvalue);
            SecureNewValue();
        }

        public override ReadOnlySpan<T> GetListCopy()
        {
            if (CheckValue())
            {
                return base.GetListCopy();
            }
            else
            {
                return null;
            }
        }

        public override void SetListValues(List<T> newList)
        {
            base.SetListValues(newList);
            SecureNewValue();
        }

        public override void InitializeListElems(T defaultValue)
        {
            base.InitializeListElems(defaultValue);
            SecureNewValue();
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            base.ParseToBytes(ref streamwriter);
            ReadOnlySpan<byte> streamreader = streamwriter;

            CheckValue(ref streamreader);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            base.ParseFromBytes(ref streamreader);
            SecureNewValue(ref streamreader);
        }
    }
   

    public class VARMAP_Array<T> : VARMAP_Variable_Interface<T>
    {
        protected const int LIST_SIZE_SIZE = sizeof(int);

        protected Type _type;
        protected int _elems;
        protected T[] _values;
        protected VARMAP_Variable_ID _ID;
        protected bool _streamable;
        protected bool _isIStreamable;
        protected int _elemSize;
        protected VARMAPValueChangedEvent<T> _changedevents;


        /// <summary>
        /// Constructor for a VARMAP_Array type
        /// </summary>
        /// <param name="id">Unique ID for this variable</param>
        /// <param name="newInstanceFunc">If T is Streamable and not a primitive, its CreateNewInstance function must be given</param>
        public VARMAP_Array(VARMAP_Variable_ID id, int elems, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null)
        {
            if(elems <= 0)
            {
                throw new Exception("Elements is 0 or negative");
            }

            _elems = elems;

            ParseFromBytesFunction = parseFromBytesDelegate;
            ParseToBytesFunction = parseToBytesDelegate;
            ConstructorFunction = constructor;

            Constructor(id);
        }

        
        private void Constructor(VARMAP_Variable_ID id)
        {
            _ID = id;


            _isIStreamable = typeof(IStreamable).IsAssignableFrom(typeof(T));

            _values = new T[_elems];

            _type = typeof(T);

            if (typeof(VARMAP_Variable_Indexable).IsAssignableFrom(_type))
            {
                throw new Exception("A VARMAP Variable cannot contain another VARMAP Variable");
            }
            else if (_type.IsEnum)
            {
                _streamable = true;
                _elemSize = sizeof(int);
            }
            else if (_type == typeof(bool))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if (_type == typeof(int))
            {
                _streamable = true;
                _elemSize = sizeof(int);
            }
            else if (_type == typeof(long))
            {
                _streamable = true;
                _elemSize = sizeof(long);
            }
            else if (_type == typeof(byte))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if (_type == typeof(sbyte))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if (_type == typeof(short))
            {
                _streamable = true;
                _elemSize = sizeof(short);
            }
            else if (_type == typeof(uint))
            {
                _streamable = true;
                _elemSize = sizeof(uint);
            }
            else if (_type == typeof(ulong))
            {
                _streamable = true;
                _elemSize = sizeof(ulong);
            }
            else if (_type == typeof(ushort))
            {
                _streamable = true;
                _elemSize = sizeof(ushort);
            }
            else if (_type == typeof(float))
            {
                _streamable = true;
                _elemSize = sizeof(float);
            }
            else if (_type == typeof(double))
            {
                _streamable = true;
                _elemSize = sizeof(double);
            }
            else if (_isIStreamable)
            {
                _streamable = true;
                _elemSize = ((IStreamable)default(T)).GetElemSize();
            }
            else
            {
                _streamable = false;
                _elemSize = 0;
                ParseToBytesFunction = null;
            }

            if (!_streamable)
            {
                throw new Exception("Type is not streamable " + _type.Name);
            }

            _changedevents = null;
        }

        public override VARMAP_Variable_ID GetID() => _ID;
        public override T GetValue()
        {
            throw new Exception("Not single element VARMAP Variable");
        }
        public override void SetValue(T newval)
        {
            throw new Exception("Not single element VARMAP Variable");
        }

        public override int GetElemSize()
        {
            return  _elems *_elemSize;
        }

        public override void RegisterChangeEvent(VARMAPValueChangedEvent<T> callback)
        {
           _changedevents += (VARMAPValueChangedEvent<T>)(Delegate)callback;
        }

        public override void UnregisterChangeEvent(VARMAPValueChangedEvent<T> callback)
        {
            _changedevents -= (VARMAPValueChangedEvent<T>)(Delegate)callback;
        }

        public override void ClearChangeEvent()
        {
            _changedevents = null;
        }

        public override ReadOnlySpan<T> GetListCopy()
        {
            return _values;
        }

        public override int GetListSize()
        {
            return _elems;
        }

        public override void SetListElem(int pos, T newvalue)
        {
            if((pos >= 0) && (pos < _elems))
            {
                T oldval = _values[pos];

                _values[pos] = newvalue;

                T newval = _values[pos];

                _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET_LIST_ELEM, ref oldval, ref newval);
            }
            else
            {
                throw new Exception("Pos " + pos + " is not reachable in array");
            }
        }

        public override T GetListElem(int pos)
        {
            if((pos >= 0) && (pos < _elems))
            {
                return _values[pos];
            }
            else
            {
                throw new Exception("Pos " + pos + " is not reachable in array");
            }
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(streamwriter);

            for (int i = 0; i < _elems; i++)
            {
                Span<byte> tempspan = writeZone.WriteNext(_elemSize);
                T _exchange = _values[i];
                ParseToBytesFunction(ref _exchange, ref tempspan);
                _values[i] = _exchange;
            }
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(streamreader);
            int listSize = _elems;

            T _exchange;

            for (int i = 0; i < listSize; i++)
            {
                ReadOnlySpan<byte> tempspan = readZone.ReadNext(_elemSize);

                if(ConstructorFunction != null)
                {
                    _exchange = ConstructorFunction();
                }
                else
                {
                    _exchange = default;
                }

                ParseFromBytesFunction(ref _exchange, ref tempspan);

                _values[i] = _exchange;
            }
        }

        public override uint CalcCRC32()
        {
            int crclength = GetElemSize();
            Span<byte> writeZone = stackalloc byte[crclength];
            ReadOnlySpan<byte> readZone = writeZone;

            ParseToBytes(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, crclength);

            return crc;
        }

        public override void SetListValues(List<T> newList)
        {
            newList.CopyTo(0, _values, 0, _elems);
            
            T defval = default;
            _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET, ref defval, ref defval);
        }

        public override void InitializeListElems(T defaultValue)
        {
            for(int i=0;i<_elems;i++)
            {
                _values[i] = defaultValue;
            }
        }
    }


    public class VARMAP_SafeVariable<T> : VARMAP_Variable<T>
    {
        private bool _highSec;
        private uint _IDSec;

        public VARMAP_SafeVariable(VARMAP_Variable_ID id, bool highSecurity, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null) : base (id, parseFromBytesDelegate, parseToBytesDelegate, constructor)
        {
            _IDSec = Common_Constructor(highSecurity);
        }

        private uint Common_Constructor(bool highSecurity)
        {
            _highSec = highSecurity;
            uint secid = VARMAP_Safe.RegisterSecureVariable();
            SecureNewValue();

            return secid;
        }

        public override T GetValue()
        {
            T baseValue = _value;

            if(!CheckValue())
            {
                baseValue = default(T);
            }

            return baseValue;
        }

        public override void SetValue(T newval)
        {
            base.SetValue(newval);

            SecureNewValue();
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            base.ParseToBytes(ref streamwriter);
            ReadOnlySpan<byte> streamreader = streamwriter;

            CheckValue(ref streamreader);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            base.ParseFromBytes(ref streamreader);
            SecureNewValue(ref streamreader);
        }

        public override uint CalcCRC32()
        {
            Span<byte> writeZone = stackalloc byte[_elemSize];
            ReadOnlySpan<byte> readZone = writeZone;

            base.ParseToBytes(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, _elemSize);

            return crc;
        }

        private void SecureNewValue()
        {
            VARMAP_Safe.SecureNewValue(_IDSec, CalcCRC32(), _highSec);
        }

        private void SecureNewValue(ref ReadOnlySpan<byte> stream)
        {
            uint crc = CRC32.ComputeHash(ref stream, _elemSize);
            VARMAP_Safe.SecureNewValue(_IDSec, crc, _highSec);
        }

        private bool CheckValue()
        {
            bool retVal;

            if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
            {
                retVal = true;
            }
            else
            {
                retVal = VARMAP_Safe.CheckSafeValue(_IDSec, CalcCRC32());
            }

            return retVal;
        }

        private bool CheckValue(ref ReadOnlySpan<byte> stream)
        {
            bool retVal;

            if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
            {
                retVal = true;
            }
            else
            {
                uint crc;
                crc = CRC32.ComputeHash(ref stream, _elemSize);
                retVal = VARMAP_Safe.CheckSafeValue(_IDSec, crc);
            }

            return retVal;
        }
    }

    

    public class VARMAP_Variable<T> : VARMAP_Variable_Interface<T>
    {
        protected Type _type;
        protected T _value;

        protected VARMAP_Variable_ID _ID;
        protected bool _streamable;
        protected bool _isIStreamable;
        protected int _elemSize;
        protected VARMAPValueChangedEvent<T> _changedevents;

        


        public VARMAP_Variable(VARMAP_Variable_ID id, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null)
        {
            ParseFromBytesFunction = parseFromBytesDelegate;
            ParseToBytesFunction = parseToBytesDelegate;
            ConstructorFunction = constructor;

            Constructor(id, default);
        }


        private void Constructor(VARMAP_Variable_ID id, T defaultValue)
        {
            _ID = id;
            _value = defaultValue;

            _type = typeof(T);

            _isIStreamable = typeof(IStreamable).IsAssignableFrom(_type);


            if(typeof(VARMAP_Variable_Indexable).IsAssignableFrom(_type))
            {
                throw new Exception("A VARMAP Variable cannot contain another VARMAP Variable");
            }
            else if(_type.IsEnum)
            {
                _streamable = true;
                _elemSize = sizeof(int);
            }
            else if (_type == typeof(bool))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if (_type == typeof(int))
            {
                _streamable = true;
                _elemSize = sizeof(int);
            }
            else if (_type == typeof(long))
            {
                _streamable = true;
                _elemSize = sizeof(long);
            }
            else if (_type == typeof(byte))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if(_type == typeof(sbyte))
            {
                _streamable = true;
                _elemSize = sizeof(char);
            }
            else if (_type == typeof(short))
            {
                _streamable = true;
                _elemSize = sizeof(short);
            }
            else if (_type == typeof(uint))
            {
                _streamable = true;
                _elemSize = sizeof(uint);
            }
            else if (_type == typeof(ulong))
            {
                _streamable = true;
                _elemSize = sizeof(ulong);
            }
            else if (_type == typeof(ushort))
            {
                _streamable = true;
                _elemSize = sizeof(ushort);
            }
            else if (_type == typeof(float))
            {
                _streamable = true;
                _elemSize = sizeof(float);
            }
            else if (_type == typeof(double))
            {
                _streamable = true;
                _elemSize = sizeof(double);
            }
            else if(_isIStreamable)
            {
                _streamable = true;
                _elemSize = ((IStreamable)default(T)).GetElemSize();
            }
            else
            {
                _streamable = false;
                _elemSize = 0;
                ParseToBytesFunction = null;
            }

            if (!_streamable)
            {
                throw new Exception("Type is not streamable " + _type.Name);
            }

            _changedevents = null;
        }

        public override VARMAP_Variable_ID GetID() => _ID;
        public override T GetValue() => _value;
        public override void SetValue(T newval)
        {
            /* Keep track of old and new values for event */
            T oldvalT = _value;

            _value = newval;

            T newvalT = _value;

            /* Trigger Changed Event */
            _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET, ref oldvalT, ref newvalT);
        }



        public override int GetElemSize()
        {
            return _elemSize;
        }

        public override void RegisterChangeEvent(VARMAPValueChangedEvent<T> callback)
        {
           _changedevents += (VARMAPValueChangedEvent<T>)(Delegate)callback;
        }

        public override void UnregisterChangeEvent(VARMAPValueChangedEvent<T> callback)
        {
           _changedevents -= (VARMAPValueChangedEvent<T>)(Delegate)callback;
        }

        public override void ClearChangeEvent()
        {
            _changedevents = null;
        }

        public override ReadOnlySpan<T> GetListCopy()
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            ParseToBytesFunction(ref _value, ref streamwriter);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            T oldval = _value;
            ParseFromBytesFunction(ref _value, ref streamreader);
            T newval = _value;

            _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET, ref oldval, ref newval);
        }

        public override uint CalcCRC32()
        {
            Span<byte> writeZone = stackalloc byte[_elemSize];
            ReadOnlySpan<byte> readZone = writeZone;

            ParseToBytes(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, _elemSize);

            return crc;
        }

        public override void SetListValues(List<T> newList)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override int GetListSize()
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void SetListElem(int pos, T newvalue)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override T GetListElem(int pos)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void InitializeListElems(T defaultValue)
        {
            throw new Exception("Not array VARMAP variable");
        }
    }
}
