using CQuark;
using System;

namespace CQuark{
	public class NumberUtil  {

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
		//把value的精度匹配到dsttype
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



		/// <summary>
		/// 获取Math2Value的返回类型.
		/// 这里并没有严格仿照C#的类型系统进行数学计算时的返回类型。
		/// </summary>
		public static Type GetReturnType_Math2Value (Type leftType, Type rightType) {

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

		public static bool NumberMathLogic (LogicToken logicCode, CQ_Value left, CQ_Value right, out bool mathLogicSuccess) {
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
	}
}
