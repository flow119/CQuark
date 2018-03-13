using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    public enum logictoken
    {
        less,           //<
        less_equal,     //<=
        more,           //>
        more_equal,     //>=
        equal,          //==
        not_equal       //!=

    }
    public class MethodCache
    {
        public System.Reflection.MethodInfo info;
        public bool cachefail = false;
        public bool slow = false;
    }
    public interface ICQ_TypeFunction
    {
        CQ_Content.Value New(CQ_Content environment, IList<CQ_Content.Value> _params);
        
		CQ_Content.Value StaticCall(CQ_Content environment, string function, IList<CQ_Content.Value> _params);
        CQ_Content.Value StaticCall(CQ_Content environment, string function, IList<CQ_Content.Value> _params, MethodCache cache);
        CQ_Content.Value StaticCallCache(CQ_Content environment, IList<CQ_Content.Value> _params, MethodCache cache);

        CQ_Content.Value StaticValueGet(CQ_Content environment, string valuename);
        bool StaticValueSet(CQ_Content environment, string valuename, object value);
        
		bool HasFunction(string key);
		CQ_Content.Value MemberCall(CQ_Content environment, object object_this, string func, IList<CQ_Content.Value> _params);
		IEnumerator CoroutineCall(CQ_Content enviroment, object object_this, string func, IList<CQ_Content.Value> _params, ICoroutine coroutin);
        CQ_Content.Value MemberCall(CQ_Content environment, object object_this, string func, IList<CQ_Content.Value> _params, MethodCache cache);
		CQ_Content.Value MemberCallCache(CQ_Content environment, object object_this, IList<CQ_Content.Value> _params, MethodCache cache);

		CQ_Content.Value MemberValueGet(CQ_Content environment, object object_this, string valuename);
        bool MemberValueSet(CQ_Content environment, object object_this, string valuename, object value);

        CQ_Content.Value IndexGet(CQ_Content environment, object object_this, object key);
        void IndexSet(CQ_Content environment, object object_this, object key, object value);
    }
    public class CQType
    {
		Type type;
		SType stype = null;

        private CQType(Type type)
        {
            this.type = type;
        }
        private CQType(SType type)
        {
            this.stype = type;
        }
        public static implicit operator Type(CQType m)
        {
            if (m == null) return null;

            return m.type;
        }
        public static implicit operator SType(CQType m)
        {
            if (m == null) return null;

            return m.stype;
        }
        static Dictionary<Type, CQType> types = new Dictionary<Type, CQType>();
        static Dictionary<SType, CQType> stypes = new Dictionary<SType, CQType>();

        public static implicit operator CQType(Type type)
        {
            CQType retT = null;
            bool bRet = types.TryGetValue(type, out retT);
            if (bRet)
                return retT;
            else
            {
                var ct = new CQType(type);
                types[type] = ct;
                return ct;
            }
        }
        public static implicit operator CQType(SType type)
        {
            CQType retST = null;
            bool bRet = stypes.TryGetValue(type, out retST);
            if (bRet)
                return retST;
            else
            {
                var ct = new CQType(type);
                stypes[type] = ct;
                return ct;
            }
        }
        //public static bool operator ==(CQType left, Type right)
        //{
        //    return left.type == right;
        //}
        //public static bool operator !=(CQType left, Type right)
        //{
        //    return left != right.type;
        //}

        public override string ToString()
        {
            if (type != null) return type.ToString();
            return stype.ToString();
        }
       
        public string Name
        {
            get
            {
                if (type != null) return type.Name;
                else return stype.Name;
            }
        }
        public string NameSpace
        {
            get
            {
                if (type != null) return type.Namespace;
                else return stype.Namespace;
            }
        }
    }
    public interface ICQ_Type
    {
        string keyword
        {
            get;
        }
        string _namespace
        {
            get;
        }
        CQType type
        {
            get;
        }
        object DefValue
        {
            get;
        }

        ICQ_Value MakeValue(object value);
        //自动转型能力
        object ConvertTo(CQ_Content env, object src, CQType targetType);

        //数学计算能力
        object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype);

        //逻辑计算能力
        bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right);

        ICQ_TypeFunction function
        {
            get;
        }

    }

    public interface ICQ_Type_WithBase : ICQ_Type
    {
        void SetBaseType(IList<ICQ_Type> types);

    }
    public interface ICQ_Type_Dele : ICQ_Type
    {
        //string GetParamSign(ICQ_Environment env);
        //Delegate CreateDelegate(ICQ_Environment env, SType calltype, SInstance callthis, string function);

        Delegate CreateDelegate(CQ_Environment env, DeleFunction lambda);

        Delegate CreateDelegate(CQ_Environment env, DeleLambda lambda);
    }
}
