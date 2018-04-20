using CQuark;
using System;
using System.Collections.Generic;

namespace CQuark {
    /// <summary>
    /// 西瓜的值
    /// </summary>
    public struct CQ_Value {
        //类型Type与Class_CQuark取其一
        public Type m_type;
        public Class_CQuark m_stype;
        //值obj与_num取其一
        private object _obj;
        private double _num;	//如果是数字则存在_num里,可以避免装箱的消耗
        private bool _isNum;
		//expression如果需要跳出，暂存在CQ_Value中
        public BreakType m_breakBlock;

		//类型桥，在不确定用Type还是CQClass时用
        public TypeBridge typeBridge {
            get {
                if(m_type != null)
                    return m_type;
                if(m_stype != null)
                    return m_stype;
                return null;
            }
        }
		//没有类型有2种情况，1本身是null，2是一种Action
		public bool TypeIsEmpty {
			get {
				return m_type == null && m_stype == null;
			}
		}


        public void SetValue (Type type, object obj) {
			//TODO 如果是数字类型特殊处理
            m_type = type;
            m_stype = null;
            _obj = obj;
            _isNum = false;
        }

        public void SetValue (Class_CQuark stype, object obj) {
            m_type = null;
            m_stype = stype;
            _obj = obj;
            _isNum = false;
        }

        public void SetValue (TypeBridge cqType, object obj) {
            if(cqType.type != null)
                SetValue(cqType.type, obj);
            else if(cqType.stype != null)
                SetValue(cqType.stype, obj);
            else
                SetNoneTypeValue(obj);
        }
		//没有类型有2种情况，1本身是null，2是一种Action
        public void SetNoneTypeValue (object obj) {
            m_type = null;
            m_stype = null;
            _obj = obj;
            _isNum = false;
        }

		/// <summary>
		/// 调用这个方法必须保证类型与obj匹配，比如m_type = float，存进来的万一是int，就取不出了，所以外面就必须做好转型
		/// </summary>
		public void SetValue (Object obj) {
			if(m_type != null){
				SetValue(m_type, obj);
			}else if(m_stype != null){
				SetValue(m_stype, obj);
			}else{
				if(obj == null)
					_obj = null;
				else
					throw new Exception("不允许在无类型的情况下赋值");
			}
		}

        //保持原有的type，而使用别的CQ_Value的值（一般用于赋值）
        public void UsingValue (CQ_Value val) {
            m_type = val.m_type;
            m_stype = val.m_stype;

            _obj = val._obj;
            _isNum = val._isNum;
            _num = val._num;
            return;

            //TODO这里没有按照以前的转型处理, 如果按照以前的做法，这里还需要对数字转型做无装箱的优化
            if(m_type == val.m_type && m_stype == val.m_stype) {
                _obj = val._obj;
                _isNum = val._isNum;
                _num = val._num;
            }
            else if(val.m_type == null && val.m_stype == null) {
                _obj = val._obj;
                _isNum = val._isNum;
                _num = val._num;
            }
            else {
                object obj = val.GetValue();
                IType itype = AppDomain.GetITypeByCQValue(this);
                if(obj != null && obj.GetType() != (Type)itype.typeBridge) {
                    if(obj is CQ_ClassInstance) {
                        if((obj as CQ_ClassInstance).type != (Class_CQuark)itype.typeBridge) {
                            obj = CQuark.AppDomain.GetITypeByClassCQ((obj as CQ_ClassInstance).type).ConvertTo(obj, itype.typeBridge);
                        }
                    }
                    else if(obj is DeleEvent) {

                    }
                    else {
                        obj = CQuark.AppDomain.ConvertTo(obj, itype.typeBridge);
                    }
                }
                SetValue(obj);
            }
        }

        public object GetValue () {
            if(_isNum)
                return _num;
            return _obj;
        }

//		public T GetValue<T>(){
//
//		}

       

        

        public double GetDouble () {
            if(_isNum)
                return _num;
            return Type_Numeric.GetDouble(m_type, _obj);
        }

       

        public static CQ_Value One {
            get {
                CQ_Value g_one = new CQ_Value();
                g_one.m_type = (typeof(int));
                g_one._obj = (int)1;

                return g_one;
            }
        }
        public static CQ_Value OneMinus {
            get {
                CQ_Value g_oneM = new CQ_Value();
                g_oneM.m_type = (typeof(int));
                g_oneM._obj = (int)-1;

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
                return "<" + m_type.ToString() + ">" + _obj;
            else if(m_stype != null)
                return "<" + m_stype.ToString() + ">" + _obj;
            return "<null>" + _obj;
        }

        public object ConvertTo (TypeBridge targetType) {
            if(_obj == null)
                return _obj;
            if(m_type == targetType.type && m_stype == targetType.stype)
                return _obj;
            //TODO 这个流程太长了，最好简化
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(_obj, targetType);
            else if(m_stype != null)
                return AppDomain.GetITypeByClassCQ(m_stype).ConvertTo(_obj, targetType);
            return null;
        }

        public object ConvertTo (Type targetType) {
            if(_obj == null)
                return _obj;
            if(m_type == targetType)
                return _obj;
            if(m_type != null)
                return AppDomain.GetITypeByType(m_type).ConvertTo(_obj, targetType);

            return null;
        }

        //类型是否等于targetType
        public bool EqualType (Type targetType) {
            if(_obj == null && !targetType.IsValueType)
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
            return a.m_type == b.m_type && a.m_stype == b.m_stype && a._obj == b._obj;
        }

        public static bool operator != (CQ_Value a, CQ_Value b) {
            return a.m_type != b.m_type || a.m_stype != b.m_stype || a._obj != b._obj;
        }
    }
}

