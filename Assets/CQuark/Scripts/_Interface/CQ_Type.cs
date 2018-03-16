using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	//由System的Type或西瓜脚本的SType转过来
    public class CQType
    {
		#region Type
		static Dictionary<Type, CQType> dicType_CQType = new Dictionary<Type, CQType>();
		Type type;
		private CQType(Type type)
		{
			this.type = type;
		}
		public static implicit operator Type(CQType m)
		{
			if (m == null)
				return null;
			return m.type;
		}
		public static implicit operator CQType(Type type)
		{
			CQType retT = null;
			bool bRet = dicType_CQType.TryGetValue(type, out retT);
			if (bRet)
				return retT;
			else
			{
				var ct = new CQType(type);
				dicType_CQType[type] = ct;
				return ct;
			}
		}
		#endregion

		#region SType
		static Dictionary<SType, CQType> dicSType_CQType = new Dictionary<SType, CQType>();
		SType stype = null;
        private CQType(SType type)
        {
            this.stype = type;
        }
        public static implicit operator SType(CQType m)
        {
            if (m == null) 
                return null;
            return m.stype;
        }
        public static implicit operator CQType(SType type)
        {
            CQType retST = null;
            bool bRet = dicSType_CQType.TryGetValue(type, out retST);
            if (bRet)
                return retST;
            else
            {
                var ct = new CQType(type);
                dicSType_CQType[type] = ct;
                return ct;
            }
        }
		#endregion

        public override string ToString()
        {
            if (type != null) 
				return type.ToString();
            return stype.ToString();
        }
        public string Name
        {
            get
            {
                if (type != null) 
					return type.Name;
                else 
					return stype.Name;
            }
        }
        public string NameSpace
        {
            get
            {
                if (type != null)
					return type.Namespace;
                else 
					return stype.Namespace;
            }
        }
    }
}
