using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	//把System的Type或西瓜脚本的SType统一管理的桥梁
    public class TypeBridge
    {
		#region Type
		static Dictionary<Type, TypeBridge> dicType_CQType = new Dictionary<Type, TypeBridge>();
		Type type;
		private TypeBridge(Type type)
		{
			this.type = type;
		}
		public static implicit operator Type(TypeBridge m)
		{
			if (m == null)
				return null;
			return m.type;
		}
		public static implicit operator TypeBridge(Type type)
		{
			TypeBridge retT = null;
			bool bRet = dicType_CQType.TryGetValue(type, out retT);
			if (bRet)
				return retT;
			else
			{
				var ct = new TypeBridge(type);
				dicType_CQType[type] = ct;
				return ct;
			}
		}
		#endregion

		#region SType
		static Dictionary<CQ_Type, TypeBridge> dicSType_CQType = new Dictionary<CQ_Type, TypeBridge>();
		CQ_Type stype = null;
        private TypeBridge(CQ_Type type)
        {
            this.stype = type;
        }
        public static implicit operator CQ_Type(TypeBridge m)
        {
            if (m == null) 
                return null;
            return m.stype;
        }
        public static implicit operator TypeBridge(CQ_Type type)
        {
            TypeBridge retST = null;
            bool bRet = dicSType_CQType.TryGetValue(type, out retST);
            if (bRet)
                return retST;
            else
            {
                var ct = new TypeBridge(type);
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
