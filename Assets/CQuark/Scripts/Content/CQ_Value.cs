using CQuark;

namespace CQuark
{
	/// <summary>
	/// 西瓜的值
	/// </summary>
    public class CQ_Value
    {
        public CQ_Type type;
        public object value;
        public int breakBlock = 0;//是否是块结束

        public static CQ_Value FromICQ_Value(ICQ_Expression_Value value)
        {
            CQ_Value v = new CQ_Value();
            v.type = value.type;
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
                    g_one.type = typeof(int);
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
                    g_oneM.type = typeof(int);
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
                    g_void.type = typeof(void);
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
            if (type == null)
            {
                return "<null>" + value;
            }
            return "<" + type.ToString() + ">" + value;
        }
    }
}

