using CQuark;
using System;
using System.Collections.Generic;

namespace CQuark {
    public enum BreakType {
        None = 0,
        Continue = 1,
        Break = 1,
        Return = 10,
        YieldBreak = 11,
        YieldReturn = 12,
    }

    /// <summary>
    /// 西瓜的值
    /// </summary>
    public struct CQ_Value {

        public Type m_type;
        public Class_CQuark m_stype;
        private object m_value;
        private double _num;
        private bool _isNum;
        public BreakType m_breakBlock;//= BreakType.None;

        public TypeBridge typeBridge {
            get {
                if(m_type != null)
                    return m_type;
                if(m_stype != null)
                    return m_stype;
                return null;
            }
        }

        public void SetCQType (TypeBridge typeBridge) {//TODO 这些调用都要被废除
            if(typeBridge == null) {
                m_type = null;
                m_stype = null;
            }
            else if(typeBridge.type != null) {
                m_type = typeBridge.type;
            }
            else if(typeBridge.stype != null) {
                m_stype = typeBridge.stype;
            }
            else {
                m_type = null;
                m_stype = null;
            }
        }

        public bool TypeIsEmpty {
            get {
                return m_type == null && m_stype == null;
            }
        }

        public object GetValue () {
            if(_isNum)
                return _num;
            return m_value;
        }

        public void SetValue (Object obj) {
            m_value = obj;
        }

        public void SetDouble (double num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetFloat (float num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetLong (long num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetULong (ulong num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetInt (int num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetUInt (uint num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetShort (short num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }
        public void SetUShort (ushort num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetSByte (sbyte num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetByte (byte num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetChar (char num) {
            _isNum = true;
            _num = num;
            m_value = null;
        }

        public void SetBool (bool num) {
            _isNum = true;
            _num = num ? 1 : 0;
            m_value = null;
        }
        public double GetDouble () {
            if(_isNum)
                return _num;
            return (double)ConvertTo(typeof(double));
        }

        public float GetFloat () {
            if(_isNum)
                return (float)_num;
            return (float)ConvertTo(typeof(float));
        }

        public long GetLong () {
            if(_isNum)
                return (long)_num;
            return (long)ConvertTo(typeof(long));
        }

        public ulong GetULong () {
            if(_isNum)
                return (ulong)_num;
            return (ulong)ConvertTo(typeof(ulong));
        }

        public int GetInt () {
            if(_isNum)
                return (int)_num;
            return (int)ConvertTo(typeof(int));
        }

        public uint GetUInt () {
            if(_isNum)
                return (uint)_num;
            return (uint)ConvertTo(typeof(uint));
        }

        public long GetShort () {
            if(_isNum)
                return (short)_num;
            return (short)ConvertTo(typeof(short));
        }

        public sbyte GetSByte () {
            if(_isNum)
                return (sbyte)_num;
            return (sbyte)ConvertTo(typeof(sbyte));
        }

        public byte GetByte () {
            if(_isNum)
                return (byte)_num;
            return (byte)ConvertTo(typeof(byte));
        }

        public char GetChar () {
            if(_isNum)
                return (char)_num;
            return (char)ConvertTo(typeof(char));
        }
        public bool GetBool () {
            if(_isNum)
                return _num == 1;
            return (bool)ConvertTo(typeof(bool));
        }

        public void CopyValue (CQ_Value val) {
            m_value = val.m_value;
        }

        public static CQ_Value One {
            get {
                CQ_Value g_one = new CQ_Value();
                g_one.m_type = (typeof(int));
                g_one.m_value = (int)1;

                return g_one;
            }
        }
        public static CQ_Value OneMinus {
            get {
                CQ_Value g_oneM = new CQ_Value();
                g_oneM.m_type = (typeof(int));
                g_oneM.m_value = (int)-1;

                return g_oneM;
            }
        }
        public static CQ_Value Null {
            get {
                return new CQ_Value();
            }
        }


        public override string ToString () {
            if(m_type != null)
                return "<" + m_type.ToString() + ">" + m_value;
            else if(m_stype != null)
                return "<" + m_stype.ToString() + ">" + m_value;
            return "<null>" + m_value;
        }

        public object ConvertTo (TypeBridge targetType) {
            if(m_value == null)
                return m_value;
            if(m_type == targetType.type && m_stype == targetType.stype)
                return m_value;
            //TODO 这个流程太长了，最好简化
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(m_value, targetType);
            else if(m_stype != null)
                return AppDomain.GetITypeByClassCQ(m_stype).ConvertTo(m_value, targetType);
            return null;
        }

        public object ConvertTo (Type targetType) {
            if(m_value == null)
                return m_value;
            if(m_type == targetType)
                return m_value;
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(m_value, targetType);

            return null;
        }

        //类型是否等于targetType
        public bool EqualType (Type targetType) {
            if(m_value == null && !targetType.IsValueType)
                return true;

            if(m_type == targetType)
                return true;

            return false;
        }

        //类型是否可以隐式转换成targetType
        public bool ImplicateType (Type targetType) {
            Type from = m_type;
            //if(from == targetType)
            //    return true;

            //数值类型
            if(from == typeof(sbyte)) {
                return (targetType == typeof(short) || targetType == typeof(int) || targetType == typeof(long) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(byte)) {
                return (targetType == typeof(short) || targetType == typeof(ushort) || targetType == typeof(int) || targetType == typeof(uint) || targetType == typeof(long) || targetType == typeof(ulong) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(short)) {
                return (targetType == typeof(int) || targetType == typeof(long) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(ushort)) {
                return (targetType == typeof(int) || targetType == typeof(uint) || targetType == typeof(long) || targetType == typeof(ulong) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(int)) {
                return (targetType == typeof(long)) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal);
            }
            else if(from == typeof(uint)) {
                return (targetType == typeof(long) || targetType == typeof(ulong) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(long)) {
                return (targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(char)) {
                return (targetType == typeof(ushort) || targetType == typeof(int) || targetType == typeof(uint) || targetType == typeof(long) || targetType == typeof(ulong) || targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }
            else if(from == typeof(float)) {
                return (targetType == typeof(double));
            }
            else if(from == typeof(ulong)) {
                return (targetType == typeof(double) || targetType == typeof(float) || targetType == typeof(decimal));
            }

            //继承
            return (targetType.IsAssignableFrom(from));
        }

        public bool EqualOrImplicateType (Type targetType) {
            return EqualType(targetType) || ImplicateType(targetType);
        }

        public static bool operator == (CQ_Value a, CQ_Value b) {
            return a.m_type == b.m_type && a.m_stype == b.m_stype && a.m_value == b.m_value;
        }

        public static bool operator != (CQ_Value a, CQ_Value b) {
            return a.m_type != b.m_type || a.m_stype != b.m_stype || a.m_value != b.m_value;
        }
    }
}

