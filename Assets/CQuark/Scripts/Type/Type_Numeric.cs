using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CQuark
{
    public class Type_Numeric : IType
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

		public Type_Numeric(Type type, string setkeyword, object defaultVal)
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

		/// <summary>
		/// 类型转换.
		/// </summary>
		protected static object TryConvertTo<OriginalType> (object src, TypeBridge targetType, out bool convertSuccess) where OriginalType : struct {
			convertSuccess = true;
			try {
				double srcValue = GetDouble(typeof(OriginalType), src);
				return Double2TargetType(targetType, srcValue);
			}
			catch(Exception) {
				convertSuccess = false;
				return null;
			}
		}
		//虽然写法很奇怪，但是这样是最高效的处理方法
		public static double GetDouble (Type srctype, object v) {
			if(srctype == typeof(double))
				return (double)v;
			if(srctype == typeof(float))
				return (float)v;
			if(srctype == typeof(long))
				return (long)v;
			if(srctype == typeof(ulong))
				return (ulong)v;
			if(srctype == typeof(int))
				return (int)v;
			if(srctype == typeof(uint))
				return (uint)v;
			if(srctype == typeof(short))
				return (short)v;
			if(srctype == typeof(ushort))
				return (ushort)v;
			if(srctype == typeof(sbyte))
				return (sbyte)v;
			if(srctype == typeof(byte))
				return (byte)v;
			if(srctype == typeof(char))
				return (char)v;
			return (double)v;
		}
		public static object Double2TargetType (Type dsttype, double value) {
			if(dsttype == typeof(double))
				return (double)value;
			if(dsttype == typeof(float))
				return (float)value;
			if(dsttype == typeof(long))
				return (long)value;
			if(dsttype == typeof(ulong))
				return (ulong)value;
			if(dsttype == typeof(int))
				return (int)value;
			if(dsttype == typeof(uint))
				return (uint)value;
			if(dsttype == typeof(short))
				return (short)value;
			if(dsttype == typeof(ushort))
				return (ushort)value;
			if(dsttype == typeof(sbyte))
				return (sbyte)value;
			if(dsttype == typeof(byte))
				return (byte)value;
			if(dsttype == typeof(char))
				return (char)value;

			throw new Exception("unknown target type...");
		}

		public static double ConvertNumber(double value, Type dsttype){
			if(dsttype == typeof(double))
				return (double)value;
			if(dsttype == typeof(float))
				return (float)value;
			if(dsttype == typeof(long))
				return (long)value;
			if(dsttype == typeof(ulong))
				return (ulong)value;
			if(dsttype == typeof(int))
				return (int)value;
			if(dsttype == typeof(uint))
				return (uint)value;
			if(dsttype == typeof(short))
				return (short)value;
			if(dsttype == typeof(ushort))
				return (ushort)value;
			if(dsttype == typeof(sbyte))
				return (sbyte)value;
			if(dsttype == typeof(byte))
				return (byte)value;
			if(dsttype == typeof(char))
				return (char)value;

			throw new Exception("unknown target type...");
		}

		public static bool IsNumberType(Type type){
			return(type == typeof(double))
				||(type == typeof(float))
				||(type == typeof(long))
				||(type == typeof(ulong))
				||(type == typeof(int))
				||(type == typeof(uint))
				||(type == typeof(short))
				||(type == typeof(ushort))
				||(type == typeof(sbyte))
				||(type == typeof(byte))
				||(type == typeof(char));
		}



		//快速计算
		protected static bool NumberMath2Value (char opCode, CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
			try {
                returnValue = new CQ_Value();
                Type retType = GetReturnType_Math2Value(left.m_type, right.m_type);
                
				double leftValue = left.GetNumber();// GetDouble(left.m_type, left.GetValue());
				double rightValue = right.GetNumber();// GetDouble(right.m_type, right.GetValue());
				double finalValue;

				switch(opCode) {
				case '+':
					finalValue = leftValue + rightValue;
					break;
				case '-':
					finalValue = leftValue - rightValue;
					break;
				case '*':
					finalValue = leftValue * rightValue;
					break;
				case '/':
					finalValue = leftValue / rightValue;
					break;
				case '%':
					finalValue = leftValue % rightValue;
					break;
				default:
					throw new Exception("Invalid math operation::opCode = " + opCode);
				}
				if(IsNumberType(retType))
					returnValue.SetNumber(retType, finalValue);
                return true;

			}
			catch(Exception) {
                returnValue = CQ_Value.Null;
				return false;
			}
		}
		/// <summary>
		/// 获取Math2Value的返回类型.
		/// 这里并没有严格仿照C#的类型系统进行数学计算时的返回类型。
		/// </summary>
		private static Type GetReturnType_Math2Value (Type leftType, Type rightType) {

			//0. double 和 float 类型优先级最高.
			if(leftType == typeof(double) || rightType == typeof(double)) {
				return typeof(double);
			}
			if(leftType == typeof(float) || rightType == typeof(float)) {
				return typeof(float);
			}

			//1. 整数运算中，ulong 类型优先级最高.
			if(leftType == typeof(ulong) || rightType == typeof(ulong)) {
				return typeof(ulong);
			}

			//2. 整数运算中，除了ulong外，就属 long 类型优先级最高了.
			if(leftType == typeof(long) || rightType == typeof(long)) {
				return typeof(long);
			}

			//3. 注意：int 和 uint 结合会返回 long.
			if((leftType == typeof(int) && rightType == typeof(uint)) || (leftType == typeof(uint) && rightType == typeof(int))) {
				return typeof(long);
			}

			//4. uint 和 非int结合会返回 uint.
			if((leftType == typeof(uint) && rightType != typeof(int)) || (rightType == typeof(uint) && leftType != typeof(int))) {
				return typeof(uint);
			}

			//其他统一返回 int即可.
			//在C#类型系统中，即使是两个 ushort 结合返回的也是int类型。
			return typeof(int);
		}
        protected static bool NumberMathLogic (LogicToken logicCode, CQ_Value left, CQ_Value right, out bool mathLogicSuccess) {
			mathLogicSuccess = true;

			try {
				double leftValue = left.GetNumber();// GetDouble(left.m_type, left.GetValue());
				double rightValue = right.GetNumber();// GetDouble(right.m_type, right.GetValue());

				switch(logicCode) {
				case LogicToken.equal:
					return leftValue == rightValue;
				case LogicToken.less:
					return leftValue < rightValue;
				case LogicToken.less_equal:
					return leftValue <= rightValue;
				case LogicToken.greater:
					return leftValue > rightValue;
				case LogicToken.greater_equal:
					return leftValue >= rightValue;
				case LogicToken.not_equal:
					return leftValue != rightValue;
				default:
					throw new Exception("Invalid logic operation::logicCode = " + logicCode.ToString());
				}
			}
			catch(Exception) {
				mathLogicSuccess = false;
				return false;
			}
		}



        public virtual object ConvertTo(object src, TypeBridge targetType)
        {
            Type targettype = (Type)targetType;
            if (this._type == targettype) return src;

            //type.get

            if (_type.IsEnum)
            {
                if ((Type)targetType == typeof(int))
                    return System.Convert.ToInt32(src);
                else if ((Type)targetType == typeof(uint))
                    return System.Convert.ToUInt32(src);
                else if ((Type)targetType == typeof(short))
                    return System.Convert.ToInt16(src);
                else if ((Type)targetType == typeof(ushort))
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
           
            //var m = ((Type)type).GetMembers();
            Type rightType = right.m_type;
            if(rightType == typeof(string) && code == '+') {
                CQ_Value returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), left.ToString() + right.ToString());
                return returnValue;
            }
            else {
                CQ_Value returnValue = CQ_Value.Null;
                MethodInfo call = null;

                //会走到这里说明不是简单的数学计算了
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
