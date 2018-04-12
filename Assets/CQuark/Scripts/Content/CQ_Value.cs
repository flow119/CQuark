using CQuark;
using System;

namespace CQuark
{
	public enum BreakType{
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
    public class CQ_Value
    {
        public CQ_Type cq_type {
            get {
                if(type != null)
                    return type;
                if(stype != null)
                    return type;
                return null;
            }
            set {
                if(value == null) {
                    type = null;
                    stype = null;
                }
                else if(value.type != null) {
                    type = value.type;
                }
                else if(value.stype != null) {
                    stype = value.stype;
                }
                else{
                    type = null;
                    stype = null;
                }
            }
        }

        public Type type;
        public Class_CQuark stype;

        public object value;

		public BreakType breakBlock = BreakType.None;

        public static CQ_Value FromICQ_Value(ICQ_Expression_Value value)
        {
            CQ_Value v = new CQ_Value();
            v.cq_type = value.type;
            v.value = value.value;
            return v;
        }
        public static CQ_Value One
        {
            get
            {
                if (g_one == null)
                {
                    g_one = new CQ_Value();
                    g_one.cq_type = typeof(int);
                    g_one.value = (int)1;
                }
                return g_one;
            }
        }
        public static CQ_Value OneMinus
        {
            get
            {
                if (g_oneM == null)
                {
                    g_oneM = new CQ_Value();
                    g_oneM.cq_type = typeof(int);
                    g_oneM.value = (int)-1;
                }
                return g_oneM;
            }
        }
        public static CQ_Value Void
        {
            get
            {
                if (g_void == null)
                {
                    g_void = new CQ_Value();
                    g_void.cq_type = typeof(void);
                    g_void.value = null;
                }
                return g_void;
            }
        }
        static CQ_Value g_one = null;
        static CQ_Value g_oneM = null;
        static CQ_Value g_void = null;

        public override string ToString()
        {
            if (cq_type == null)
            {
                return "<null>" + value;
            }
            return "<" + cq_type.ToString() + ">" + value;
        }

		public object ConvertTo(CQ_Type targetType){
            if(value == null )
                return value;
            if(cq_type.type == targetType.type && cq_type.stype == targetType.stype)
                return value;
            //TODO 这个流程太长了，最好简化
			return AppDomain.GetITypeByCQType (cq_type).ConvertTo (value, targetType);
		}

        //类型是否等于targetType
        public bool EqualType (Type targetType) {
            if(value == null && !targetType.IsValueType)
                return true;

            Type from = cq_type;
            if(from == targetType)
                return true;

            return false;
        }

        //类型是否可以隐式转换成targetType
        public bool ImplicateType (Type targetType) {
            Type from = cq_type;
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
            return (from.IsAssignableFrom(targetType));
        }

        public bool EqualOrImplicateType (Type targetType) {
            return EqualType(targetType) || ImplicateType(targetType);            
        }
    }
}

