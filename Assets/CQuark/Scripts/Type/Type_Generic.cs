using System;
using System.Reflection;

namespace CQuark
{
    public class Type_Generic : IType
    {
        public string keyword
        {
            get;
            protected set;
        }
        public string _namespace
        {
            get { return typeBridge.NameSpace; }
        }
        public TypeBridge typeBridge
        {
            get;
            protected set;
        }
        public virtual object defaultValue
        {
			get ;
			protected set;
        }
        public IClass _class
        {
            get;
            protected set;
        }

        public Type _type;

		public Type_Generic(Type type, string setkeyword, object defaultVal)
        {
            _class = new Class_System(type);
            if (setkeyword != null)
            {
                keyword = setkeyword.Replace(" ", "");
            }
            else
            {
                keyword = type.Name;
            }
            this.typeBridge = type;
            this._type = type;
			this.defaultValue = defaultVal;
        }

        public virtual object ConvertTo(object src, TypeBridge targetType)
        {
            Type targettype = (Type)targetType;
            if (this._type == targettype) 
				return src;

            if (_type.IsEnum)
            {
				if (targettype == typeof(int))
                    return System.Convert.ToInt32(src);
				else if (targettype == typeof(uint))
                    return System.Convert.ToUInt32(src);
				else if (targettype == typeof(short))
                    return System.Convert.ToInt16(src);
				else if (targettype == typeof(ushort))
                    return System.Convert.ToUInt16(src);
                else
                {
                    return System.Convert.ToInt32(src);
                }
            }
            else if (targettype != null && targettype.IsEnum)
            {
                return Enum.ToObject(targettype, src);

            }
            MethodInfo[] ms = _type.GetMethods();
            foreach(MethodInfo m in ms)
            {
                if ((m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == targettype)
                {
                    return m.Invoke(null, new object[] { src });
                }
            }
            if (targettype != null)
            {
                if (targettype.IsAssignableFrom(_type))
                    return src;
                if (src != null && targettype.IsInstanceOfType(src))
                    return src;
            }
            else
            {
                return src;
            }

            return null;
        }

        public virtual CQ_Value Math2Value (char code, CQ_Value left, CQ_Value right)
        {
			//会走到这里说明不是简单的数学计算了
            Type rightType = right.m_type;
            if(rightType == typeof(string) && code == '+') {
                CQ_Value returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), left.ToString() + right.ToString());
                return returnValue;
            }
            else {
                CQ_Value returnValue = CQ_Value.Null;
                MethodInfo call = null;

				//我们这里开始使用Wrap，如果再不行再走反射
                if(code == '+') {
                    if(Wrap.OpAddition(left, right, out returnValue)) {
                        return returnValue;
                    }
                    else {
                        call = _type.GetMethod("op_Addition", new Type[] { this.typeBridge, rightType });
                    }
                }

                else if(code == '-') {
                    if(Wrap.OpSubtraction(left, right, out returnValue)) {
                        return returnValue;
                    }
                    else {
                        call = _type.GetMethod("op_Subtraction", new Type[] { this.typeBridge, rightType });
                    }
                }
                else if(code == '*') {
                    if(Wrap.OpMultiply(left, right, out returnValue)) {
                        return returnValue;
                    }
                    else {
                        call = _type.GetMethod("op_Multiply", new Type[] { this.typeBridge, rightType });
                    }
                }
                else if(code == '/') {
                    if(Wrap.OpDivision(left, right, out returnValue)) {
                        return returnValue;
                    }
                    else {
                        call = _type.GetMethod("op_Division", new Type[] { this.typeBridge, rightType });
                    }
                }
                else if(code == '%') {
                    if(Wrap.OpModulus(left, right, out returnValue)) {
                        return returnValue;
                    }
                    else {
                        call = _type.GetMethod("op_Modulus", new Type[] { this.typeBridge, rightType });
                    }
                }

                //Wrap没走到，走反射
                returnValue = new CQ_Value();
                returnValue.SetObject(typeBridge.type, call.Invoke(null, new object[] { left.GetObject(), right.GetObject() }));
                //function.StaticCall(env,"op_Addtion",new List<ICL>{})
                return returnValue;
            }
        }

        public virtual bool MathLogic (LogicToken code, CQ_Value left, CQ_Value right)
        {
            System.Reflection.MethodInfo call = null;

            //var m = _type.GetMethods();
            if (code == LogicToken.greater)//[2] = {Boolean op_GreaterThan(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThan");
            else if (code == LogicToken.less)//[4] = {Boolean op_LessThan(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_LessThan");
            else if (code == LogicToken.greater_equal)//[3] = {Boolean op_GreaterThanOrEqual(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThanOrEqual");
            else if (code == LogicToken.less_equal)//[5] = {Boolean op_LessThanOrEqual(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_LessThanOrEqual");
            else if (code == LogicToken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if(left.GetObject() == null || right.TypeIsEmpty)
                {
                    return left.GetObject() == right.GetObject();
                }

                call = _type.GetMethod("op_Equality");
                if (call == null)
                {
                    return left.GetObject().Equals(right.GetObject());
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if(left.GetObject() == null || right.TypeIsEmpty)
                {
                    return left.GetObject() != right.GetObject();
                }
                call = _type.GetMethod("op_Inequality");
                if (call == null)
                {
                    return !left.GetObject().Equals(right.GetObject());
                }
            }
            return (bool) call.Invoke(null, new object[] { left.GetObject(), right.GetObject() });
        }
    }
}
