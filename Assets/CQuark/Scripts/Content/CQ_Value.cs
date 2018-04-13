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
        public object value;
        public BreakType breakBlock;//= BreakType.None;

        public CQ_Type cq_type {
            get {
                if(m_type != null)
                    return m_type;
                if(m_stype != null)
                    return m_stype;
                return null;
            }
        }

        public void SetCQType (CQ_Type type) {//TODO 这些调用都要被废除
            if(type == null) {
                m_type = null;
                m_stype = null;
            }
            else if(type.type != null) {
                m_type = type.type;
            }
            else if(type.stype != null) {
                m_stype = type.stype;
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

        public static CQ_Value FromICQ_Value (CQ_Expression_Value_ScriptValue value) {
            return value.ComputeValue(null);
        }

        public static CQ_Value One {
            get {
                CQ_Value g_one = new CQ_Value();
                g_one.m_type = (typeof(int));
                g_one.value = (int)1;

                return g_one;
            }
        }
        public static CQ_Value OneMinus {
            get {
                CQ_Value g_oneM = new CQ_Value();
                g_oneM.m_type = (typeof(int));
                g_oneM.value = (int)-1;

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
                return "<" + m_type.ToString() + ">" + value;
            else if(m_stype != null)
                return "<" + m_stype.ToString() + ">" + value;
            return "<null>" + value;
        }

        public object ConvertTo (CQ_Type targetType) {
            if(value == null)
                return value;
            if(m_type == targetType.type && m_stype == targetType.stype)
                return value;
            //TODO 这个流程太长了，最好简化
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(value, targetType);
            else if(m_stype != null)
                return AppDomain.GetITypeByCQType(m_stype).ConvertTo(value, targetType);
            return null;
            //			return AppDomain.GetITypeByCQType (cq_type).ConvertTo (value, targetType);
        }

        public object ConvertTo (Type targetType) {
            if(value == null)
                return value;
            if(m_type == targetType)
                return value;
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(value, targetType);

            return null;
        }

        //类型是否等于targetType
        public bool EqualType (Type targetType) {
            if(value == null && !targetType.IsValueType)
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
            return a.m_type == b.m_type && a.m_stype == b.m_stype && a.value == b.value;
        }

        public static bool operator != (CQ_Value a, CQ_Value b) {
            return a.m_type != b.m_type || a.m_stype != b.m_stype || a.value != b.value;
        }
    }
}

