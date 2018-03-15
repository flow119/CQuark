using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
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
}
