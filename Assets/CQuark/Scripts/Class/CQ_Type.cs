﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	//把System的Type或西瓜脚本的SType统一管理的桥梁
    public class CQ_Type
    {
		//每个类如果实例多个就不再是相同的了，因此把他们存在静态字典里
		#region Type
		static Dictionary<Type, CQ_Type> dicType_CQType = new Dictionary<Type, CQ_Type>();
		Type type;
		private CQ_Type(Type type)
		{
			this.type = type;
		}
		public static implicit operator Type(CQ_Type m)
		{
			if (m == null)
				return null;
			return m.type;
		}
		public static implicit operator CQ_Type(Type type)
		{
			CQ_Type retT = null;
			bool bRet = dicType_CQType.TryGetValue(type, out retT);
			if (bRet)
				return retT;
			else
			{
				var ct = new CQ_Type(type);
				dicType_CQType[type] = ct;
				return ct;
			}
		}
		#endregion

		#region SType
		static Dictionary<Class_CQuark, CQ_Type> dicSType_CQType = new Dictionary<Class_CQuark, CQ_Type>();
		Class_CQuark stype = null;
        private CQ_Type(Class_CQuark type)
        {
            this.stype = type;
        }
        public static implicit operator Class_CQuark(CQ_Type m)
        {
            if (m == null) 
                return null;
            return m.stype;
        }
        public static implicit operator CQ_Type(Class_CQuark type)
        {
            CQ_Type retST = null;
            bool bRet = dicSType_CQType.TryGetValue(type, out retST);
            if (bRet)
                return retST;
            else
            {
                var ct = new CQ_Type(type);
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