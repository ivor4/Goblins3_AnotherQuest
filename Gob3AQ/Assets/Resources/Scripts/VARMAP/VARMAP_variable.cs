
using System;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Safe;
using Gob3AQ.VARMAP.Enum;
using UnityEngine;
using System.IO;
using System.Collections;
using Gob3AQ.VARMAP.Variable.IstreamableNamespace;
using System.Collections.Generic;
using Gob3AQ.Libs.CRC32;
using Gob3AQ.Libs.Arith;

namespace Gob3AQ.VARMAP.Variable
{

    public abstract class VARMAP_Variable_Indexable
    {
        /// <summary>
        /// Commits shadow values into official ones. Should be done by GameMaster at the end/beginning of a tick.
        /// </summary>
        public abstract void Commit();

        public abstract void ClearChangeEvent();

        public abstract uint CalcCRC32();

        public abstract void ParseToBytes(ref Span<byte> streamwriter);

        public abstract void ParseFromBytes(ref ReadOnlySpan<byte> streamreader);

        public abstract int GetElemSize();

#if UNITY_EDITOR

        public abstract VARMAP_Variable_ID GetID();


        public abstract string[] GetDebugValues();
#endif

    }

    public abstract class VARMAP_Variable_Interface<T> : VARMAP_Variable_Indexable
    {
        public delegate void ParseTypeToBytes(in T refvalue, ref Span<byte> writer);
        public delegate void ParseTypeFromBytes(ref T refvalue, ref ReadOnlySpan<byte> writer);
        public delegate T ConstructorOfType();


        protected ParseTypeToBytes ParseToBytesFunction;
        protected ParseTypeFromBytes ParseFromBytesFunction;
        protected ConstructorOfType ConstructorFunction;

        

        public abstract ref readonly T GetValue();
        public abstract ref readonly T GetShadowValue();
        public abstract void SetValue(in T newvalue);
        public abstract ReadOnlySpan<T> GetListCopy();
        public abstract ReadOnlySpan<T> GetShadowListCopy();
        public abstract void SetListValues(List<T> newList);
        public abstract int GetListSize();
        public abstract void SetListElem(int pos, in T newvalue);
        public abstract ref readonly T GetListElem(int pos);
        public abstract ref readonly T GetShadowListElem(int pos);

        public abstract void InitializeListElems(in T defaultValue);
        public abstract void RegisterChangeEvent(VARMAPValueChangedEvent<T> callback);
        public abstract void UnregisterChangeEvent(VARMAPValueChangedEvent<T> callback);
    }



    public sealed class VARMAP_SafeArray<T> : VARMAP_Array<T>
    {
        private bool _highSec;
        private uint _IDSec;
        private uint _IDSecShadow;

        public VARMAP_SafeArray(VARMAP_Variable_ID id, int elems, bool highSecurity, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null) : base(id, elems, parseFromBytesDelegate, parseToBytesDelegate, constructor)
        {
            _highSec = highSecurity;
            _IDSec = VARMAP_Safe.RegisterSecureVariable();
            _IDSecShadow = VARMAP_Safe.RegisterSecureVariable();
            SecureNewValue(false);
            SecureNewValue(true);
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

        private uint CalcCRC32Shadow()
        {
            int crclength = GetElemSize();
            Span<byte> writeZone = stackalloc byte[crclength];
            ReadOnlySpan<byte> readZone = writeZone;

            ParseToBytesShadow(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, crclength);
            return crc;
        }

        private void SecureNewValue(bool shadow)
        {
            if (shadow)
            {
                VARMAP_Safe.SecureNewValue(_IDSecShadow, CalcCRC32Shadow(), _highSec);
            }
            else
            {
                VARMAP_Safe.SecureNewValue(_IDSec, CalcCRC32(), _highSec);
            }
        }


        private void SecureNewValue(ref ReadOnlySpan<byte> stream, bool shadow)
        {
            uint crc = CRC32.ComputeHash(ref stream, GetElemSize());

            if (shadow)
            {
                VARMAP_Safe.SecureNewValue(_IDSecShadow, crc, _highSec);
            }
            else
            {
                VARMAP_Safe.SecureNewValue(_IDSec, crc, _highSec);
            }
                
        }

        private bool CheckValue(bool shadow)
        {
            bool retVal;

            if (shadow)
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSecShadow))
                {
                    retVal = true;
                }
                else
                {
                    uint crc;
                    crc = CalcCRC32Shadow();
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSecShadow, crc);
                }
            }
            else
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
                {
                    retVal = true;
                }
                else
                {
                    uint crc;
                    crc = CalcCRC32();
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSec, crc);
                }
            }

            return retVal;
        }

        private bool CheckValue(ref ReadOnlySpan<byte> stream, bool shadow)
        {
            bool retVal;

            if (shadow)
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSecShadow))
                {
                    retVal = true;
                }
                else
                {
                    uint crc;
                    crc = CRC32.ComputeHash(ref stream, GetElemSize());
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSecShadow, crc);
                }
            }
            else
            {
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
            }

            return retVal;
        }

        public override ref readonly T GetListElem(int pos)
        {
            if(!CheckValue(false))
            {
                base.SetListElem(pos, default);
            }
            return ref base.GetListElem(pos);
        }

        public override ref readonly T GetShadowListElem(int pos)
        {
            if (!CheckValue(true))
            {
                base.SetListElem(pos, default);
            }
            return ref base.GetShadowListElem(pos);
        }

        public override void SetListElem(int pos, in T newvalue)
        {
            base.SetListElem(pos, newvalue);
            SecureNewValue(true);
        }

        public override ReadOnlySpan<T> GetListCopy()
        {
            if (CheckValue(false))
            {
                return base.GetListCopy();
            }
            else
            {
                return null;
            }
        }

        public override ReadOnlySpan<T> GetShadowListCopy()
        {
            if (CheckValue(true))
            {
                return base.GetShadowListCopy();
            }
            else
            {
                return null;
            }
        }

        public override void SetListValues(List<T> newList)
        {
            base.SetListValues(newList);
            SecureNewValue(true);
        }

        public override void InitializeListElems(in T defaultValue)
        {
            base.InitializeListElems(defaultValue);
            SecureNewValue(false);
            SecureNewValue(true);
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            base.ParseToBytes(ref streamwriter);
            ReadOnlySpan<byte> streamreader = streamwriter;

            CheckValue(ref streamreader, false);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            base.ParseFromBytes(ref streamreader);
            SecureNewValue(ref streamreader, true);
        }

        private void ParseToBytesShadow(ref Span<byte> streamwriter)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(streamwriter);

            for (int i = 0; i < _elems; i++)
            {
                Span<byte> tempspan = writeZone.WriteNext(_elemSize);
                ParseToBytesFunction(in _shadowValues[i], ref tempspan);
            }
        }

        public override void Commit()
        {
            if (_dirty)
            {
                if(CheckValue(true))
                {
                    base.Commit();
                    SecureNewValue(false);
                }
            }
        }
    }
   

    public class VARMAP_Array<T> : VARMAP_Variable_Interface<T>
    {
        protected const int LIST_SIZE_SIZE = sizeof(int);

        protected Type _type;
        protected int _elems;
        protected T[] _values;
        protected T[] _shadowValues;
        protected VARMAP_Variable_ID _ID;
        protected bool _streamable;
        protected bool _isIStreamable;
        protected bool _dirty;
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

            _ID = id;

            _isIStreamable = typeof(IStreamable).IsAssignableFrom(typeof(T));

            _values = new T[_elems];

            _shadowValues = new T[_elems];

            _type = typeof(T);

            _dirty = false;

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



        
        public override ref readonly T GetValue()
        {
            throw new Exception("Not single element VARMAP Variable");
        }
        public override ref readonly T GetShadowValue()
        {
            throw new Exception("Not single element VARMAP Variable");
        }
        public override void SetValue(in T newval)
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

        public override ReadOnlySpan<T> GetShadowListCopy()
        {
            return _shadowValues;
        }

        public override int GetListSize()
        {
            return _elems;
        }

        public override void SetListElem(int pos, in T newvalue)
        {
            if((pos >= 0) && (pos < _elems))
            {
                _shadowValues[pos] = newvalue;
                _dirty = true;
            }
            else
            {
                throw new Exception("Pos " + pos + " is not reachable in array");
            }
        }

        public override ref readonly T GetListElem(int pos)
        {
            if((pos >= 0) && (pos < _elems))
            {
                return ref _values[pos];
            }
            else
            {
                throw new Exception("Pos " + pos + " is not reachable in array");
            }
        }

        public override ref readonly T GetShadowListElem(int pos)
        {
            if ((pos >= 0) && (pos < _elems))
            {
                return ref _shadowValues[pos];
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
                ParseToBytesFunction(in _values[i], ref tempspan);
            }
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(streamreader);
            int listSize = _elems;


            for (int i = 0; i < listSize; i++)
            {
                ReadOnlySpan<byte> tempspan = readZone.ReadNext(_elemSize);

                if(ConstructorFunction != null)
                {
                    _shadowValues[i] = ConstructorFunction();
                }
                else
                {
                    _shadowValues[i] = default;
                }

                ParseFromBytesFunction(ref _shadowValues[i], ref tempspan);

                _dirty = true;
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
            newList.CopyTo(0, _shadowValues, 0, _elems);
            _dirty = true;
        }

        public override void InitializeListElems(in T defaultValue)
        {
            for(int i=0;i<_elems;i++)
            {
                _values[i] = defaultValue;
                _shadowValues[i] = defaultValue;
            }

            _dirty = false;
        }

        public override void Commit()
        {
            if(_dirty)
            {
                _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET_LIST_ELEM, in _shadowValues[0], in _values[0]);
                _shadowValues.CopyTo(_values, 0);
            }

            _dirty = false;
        }

#if UNITY_EDITOR
        public override VARMAP_Variable_ID GetID() => _ID;
        public override string[] GetDebugValues()
        {
            string[] retVal = new string[_elems];

            for(int i=0;i<retVal.Length;i++)
            {
                retVal[i] = _values[i].ToString();
            }

            return retVal;
        }
#endif
    }


    public sealed class VARMAP_SafeVariable<T> : VARMAP_Variable<T>
    {
        private bool _highSec;
        private uint _IDSec;
        private uint _IDSecShadow;

        public VARMAP_SafeVariable(VARMAP_Variable_ID id, bool highSecurity, ParseTypeFromBytes parseFromBytesDelegate, ParseTypeToBytes parseToBytesDelegate, ConstructorOfType constructor = null) : base (id, parseFromBytesDelegate, parseToBytesDelegate, constructor)
        {
            _highSec = highSecurity;
            _IDSec = VARMAP_Safe.RegisterSecureVariable();
            _IDSecShadow = VARMAP_Safe.RegisterSecureVariable();
            SecureNewValue(false);
            SecureNewValue(true);
        }


        public override ref readonly T GetValue()
        {
            if(!CheckValue(false))
            {
                _value = default(T);
            }

            return ref _value;
        }

        public override ref readonly T GetShadowValue()
        {
            if (!CheckValue(true))
            {
                _shadowValue = default(T);
            }
            return ref _shadowValue;
        }

        public override void SetValue(in T newval)
        {
            base.SetValue(in newval);

            SecureNewValue(true);
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            base.ParseToBytes(ref streamwriter);
            ReadOnlySpan<byte> streamreader = streamwriter;

            CheckValue(ref streamreader, false);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            base.ParseFromBytes(ref streamreader);
            SecureNewValue(ref streamreader, true);
        }

        private void ParseToBytesShadow(ref Span<byte> streamwriter)
        {
            ParseToBytesFunction(in _shadowValue, ref streamwriter);
        }

        public override uint CalcCRC32()
        {
            Span<byte> writeZone = stackalloc byte[_elemSize];
            ReadOnlySpan<byte> readZone = writeZone;

            base.ParseToBytes(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, _elemSize);

            return crc;
        }

        private uint CalcCRC32Shadow()
        {
            Span<byte> writeZone = stackalloc byte[_elemSize];
            ReadOnlySpan<byte> readZone = writeZone;

            ParseToBytesShadow(ref writeZone);
            uint crc = CRC32.ComputeHash(ref readZone, _elemSize);

            return crc;
        }

        private void SecureNewValue(bool shadow)
        {
            if (shadow)
            {
                VARMAP_Safe.SecureNewValue(_IDSecShadow, CalcCRC32Shadow(), _highSec);
            }
            else
            {
                VARMAP_Safe.SecureNewValue(_IDSec, CalcCRC32(), _highSec);
            }
        }

        private void SecureNewValue(ref ReadOnlySpan<byte> stream, bool shadow)
        {
            uint crc = CRC32.ComputeHash(ref stream, _elemSize);

            if (shadow)
            {
                VARMAP_Safe.SecureNewValue(_IDSecShadow, crc, _highSec);
            }
            else
            {
                VARMAP_Safe.SecureNewValue(_IDSec, crc, _highSec);
            }
        }

        private bool CheckValue(bool shadow)
        {
            bool retVal;

            if (shadow)
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSecShadow))
                {
                    retVal = true;
                }
                else
                {
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSecShadow, CalcCRC32Shadow());
                }
            }
            else
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSec))
                {
                    retVal = true;
                }
                else
                {
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSec, CalcCRC32());
                }
            }

            return retVal;
        }

        private bool CheckValue(ref ReadOnlySpan<byte> stream, bool shadow)
        {
            bool retVal;

            if (shadow)
            {
                if (VARMAP_Safe.IsSafeVariableCheckedInTick(_IDSecShadow))
                {
                    retVal = true;
                }
                else
                {
                    uint crc;
                    crc = CRC32.ComputeHash(ref stream, _elemSize);
                    retVal = VARMAP_Safe.CheckSafeValue(_IDSecShadow, crc);
                }
            }
            else
            {
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
            }

            return retVal;
        }

        public override void Commit()
        {
            if (_dirty)
            {
                if (CheckValue(true))
                {
                    base.Commit();
                    SecureNewValue(false);
                }
            }
        }
    }

    

    public class VARMAP_Variable<T> : VARMAP_Variable_Interface<T>
    {
        protected Type _type;
        protected T _value;
        protected T _shadowValue;

        protected VARMAP_Variable_ID _ID;
        protected bool _streamable;
        protected bool _isIStreamable;
        protected bool _dirty;
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

            _dirty = false;


            if (typeof(VARMAP_Variable_Indexable).IsAssignableFrom(_type))
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

        
        public override ref readonly T GetValue() => ref _value;

        public override ref readonly T GetShadowValue() => ref _shadowValue;

        public override void SetValue(in T newval)
        {
            _shadowValue = newval;
            _dirty = true;
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

        public override ReadOnlySpan<T> GetShadowListCopy()
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void ParseToBytes(ref Span<byte> streamwriter)
        {
            ParseToBytesFunction(in _value, ref streamwriter);
        }

        public override void ParseFromBytes(ref ReadOnlySpan<byte> streamreader)
        {
            ParseFromBytesFunction(ref _shadowValue, ref streamreader);
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

        public override void SetListElem(int pos, in T newvalue)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override ref readonly T GetListElem(int pos)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override ref readonly T GetShadowListElem(int pos)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void InitializeListElems(in T defaultValue)
        {
            throw new Exception("Not array VARMAP variable");
        }

        public override void Commit()
        {
            if (_dirty)
            {
                /* Trigger Changed Event (before updating Varmap value) */
                _changedevents?.Invoke(ChangedEventType.CHANGED_EVENT_SET, in _value, in _shadowValue);

                _value = _shadowValue;
            }

            _dirty = false;
        }

#if UNITY_EDITOR
        public override VARMAP_Variable_ID GetID() => _ID;
        public override string[] GetDebugValues()
        {
            string[] retVal = new string[1];
            retVal[0] = _value.ToString();
            return retVal;
        }
#endif
    }
}
