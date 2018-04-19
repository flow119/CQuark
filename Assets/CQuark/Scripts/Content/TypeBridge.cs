using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    //把System的Type或西瓜脚本的SType统一管理的桥梁
    public class TypeBridge {
        //每个类如果实例多个就不再是相同的了，因此把他们存在静态字典里
        #region Type
        static Dictionary<Type, TypeBridge> dicType_CQType = new Dictionary<Type, TypeBridge>();
        public Type type {
            get;
            private set;
        }
        private TypeBridge (Type type) {
            this.type = type;
        }
        public static implicit operator Type (TypeBridge m) {
            if(m == null)
                return null;
            return m.type;
        }
        public static implicit operator TypeBridge (Type type) {
            TypeBridge retT = null;
            if(dicType_CQType.TryGetValue(type, out retT)) {
                return retT;
            }
            else {
                var ct = new TypeBridge(type);
                dicType_CQType[type] = ct;
                return ct;
            }
        }
        #endregion

        #region SType
        static Dictionary<Class_CQuark, TypeBridge> dicSType_CQType = new Dictionary<Class_CQuark, TypeBridge>();
        public Class_CQuark stype {
			get;
			private set;
		}
        private TypeBridge (Class_CQuark type) {
            this.stype = type;
        }
        public static implicit operator Class_CQuark (TypeBridge m) {
            if(m == null)
                return null;
            return m.stype;
        }
        public static implicit operator TypeBridge (Class_CQuark type) {
            TypeBridge retST = null;
            if(dicSType_CQType.TryGetValue(type, out retST)) {
                return retST;
            }
            else {
                var ct = new TypeBridge(type);
                dicSType_CQType[type] = ct;
                return ct;
            }
        }
        #endregion

        public override string ToString () {
            if(type != null)
                return type.ToString();
            return stype.ToString();
        }
        public string Name {
            get {
                if(type != null)
                    return type.Name;
                else
                    return stype.Name;
            }
        }
        public string NameSpace {
            get {
                if(type != null)
                    return type.Namespace;
                else
                    return stype.Namespace;
            }
        }
    }
}
