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
            get { return cqType.NameSpace; }
        }
        public CQ_Type cqType
        {
            get;
            protected set;
        }
        public virtual object defaultValue
        {
            get { return null; }
        }
        public IClass _class
        {
            get;
            protected set;
        }

        public Type _type;

        public Type_Numeric(Type type, string setkeyword, bool dele)
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
            this.cqType = type;
            this._type = type;
        }

		/// <summary>
		/// 类型转换.
		/// </summary>
		protected static object TryConvertTo<OriginalType> (object src, CQ_Type targetType, out bool convertSuccess) where OriginalType : struct {
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
		private static double GetDouble (Type type, object v) {
			if(type == typeof(double))
				return (double)v;
			if(type == typeof(float))
				return (float)v;
			if(type == typeof(long))
				return (long)v;
			if(type == typeof(ulong))
				return (ulong)v;
			if(type == typeof(int))
				return (int)v;
			if(type == typeof(uint))
				return (uint)v;
			if(type == typeof(short))
				return (short)v;
			if(type == typeof(ushort))
				return (ushort)v;
			if(type == typeof(sbyte))
				return (sbyte)v;
			if(type == typeof(byte))
				return (byte)v;
			if(type == typeof(char))
				return (char)v;
			return (double)v;
		}
		private static object Double2TargetType (Type type, double value) {
			if(type == typeof(double))
				return (double)value;
			if(type == typeof(float))
				return (float)value;
			if(type == typeof(long))
				return (long)value;
			if(type == typeof(ulong))
				return (ulong)value;
			if(type == typeof(int))
				return (int)value;
			if(type == typeof(uint))
				return (uint)value;
			if(type == typeof(short))
				return (short)value;
			if(type == typeof(ushort))
				return (ushort)value;
			if(type == typeof(sbyte))
				return (sbyte)value;
			if(type == typeof(byte))
				return (byte)value;
			if(type == typeof(char))
				return (char)value;

			throw new Exception("unknown target type...");
		}

		//快速计算
		protected static object Math2Value<LeftType> (char opCode, object left, CQ_Value right, out CQ_Type returntype, out bool math2ValueSuccess) where LeftType : struct {
			try {
				math2ValueSuccess = true;
				returntype = GetReturnType_Math2Value(typeof(LeftType), right.cq_type);
				double leftValue = GetDouble(typeof(LeftType), left);
				double rightValue = GetDouble(right.cq_type, right.value);
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

				return Double2TargetType(returntype, finalValue);

			}
			catch(Exception) {
				math2ValueSuccess = false;
				returntype = null;
				return null;
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
		protected static bool MathLogic<LeftType> (LogicToken logicCode, object left, CQ_Value right, out bool mathLogicSuccess) {
			mathLogicSuccess = true;

			try {
				double leftValue = GetDouble(typeof(LeftType), left);
				double rightValue = GetDouble(right.cq_type, right.value);

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



        public virtual object ConvertTo(object src, CQ_Type targetType)
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
		public virtual object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            returntype = cqType;
            MethodInfo call = null;
            //var m = ((Type)type).GetMembers();
            Type rightType = (Type)right.cq_type;
            if(rightType == typeof(string) && code == '+') {
                returntype = typeof(string);
                return left.ToString() + right.value as string;
            }

            //会走到这里说明不是简单的数学计算了
            //我们这里开始使用Wrap，如果再不行再走反射
            CQ_Value leftcq = new CQ_Value();
            leftcq.cq_type = this.cqType;
            leftcq.value = left;
            CQ_Value ret = null;
            if(code == '+') {
                if(Wrap.OpAddition(leftcq, right, out ret)) {
                    returntype = ret.cq_type;
                    return ret.value;
                }
                else {
                    call = _type.GetMethod("op_Addition", new Type[] { this.cqType, rightType });
                }
            }

            else if(code == '-'){
                if(Wrap.OpSubtraction(leftcq, right, out ret)) {
                    returntype = ret.cq_type;
                    return ret.value;
                }
                else {
                    call = _type.GetMethod("op_Subtraction", new Type[] { this.cqType, rightType });
                }
            }
            else if(code == '*'){
                if(Wrap.OpMultiply(leftcq, right, out ret)) {
                    returntype = ret.cq_type;
                    return ret.value;
                }
                else {
                    call = _type.GetMethod("op_Multiply", new Type[] { this.cqType, rightType });
                }
            }
            else if(code == '/'){
                if(Wrap.OpDivision(leftcq, right, out ret)) {
                    returntype = ret.cq_type;
                    return ret.value;
                }
                else {
                    call = _type.GetMethod("op_Division", new Type[] { this.cqType, rightType });
                }
            }
            else if(code == '%') {
                if(Wrap.OpModulus(leftcq, right, out ret)) {
                    returntype = ret.cq_type;
                    return ret.value;
                }
                else {
                    call = _type.GetMethod("op_Modulus", new Type[] { this.cqType, rightType });
                }
            }

            var obj = call.Invoke(null, new object[] { left, right.value });
            //function.StaticCall(env,"op_Addtion",new List<ICL>{})
            return obj;
        }
		public virtual bool MathLogic(LogicToken code, object left, CQ_Value right)
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
                if (left == null || right.cq_type == null)
                {
                    return left == right.value;
                }

                call = _type.GetMethod("op_Equality");
                if (call == null)
                {
                    return left.Equals(right.value);
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.cq_type == null)
                {
                    return left != right.value;
                }
                call = _type.GetMethod("op_Inequality");
                if (call == null)
                {
                    return !left.Equals(right.value);
                }
            }
            var obj = call.Invoke(null, new object[] { left, right.value });
            return (bool)obj;
        }
    }
}
