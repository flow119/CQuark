using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQuark {
    /// <summary>
    /// 数值类型系列类的公用工具函数.
    /// 因为这些函数逻辑都是固定的，不存在多态行为，不适合放在现有的继承结构中去实现，故而独立出来。
    /// </summary>
    public class NumericTypeUtils {

        /// <summary>
        /// 类型转换.
        /// </summary>
        public static object TryConvertTo<OriginalType> (object src, CQ_Type targetType, out bool convertSuccess) where OriginalType : struct {

            convertSuccess = true;

            try {
                decimal srcValue = GetDecimalValue(typeof(OriginalType), src);
                return Decimal2TargetType(targetType, srcValue);
            }
            catch(Exception) {
                convertSuccess = false;
                return null;
            }
        }

        /// <summary>
        /// Math2Value.
        /// </summary>

        //快速计算
        public static object Math2Value<LeftType> (char opCode, object left, CQ_Value right, out CQ_Type returntype, out bool math2ValueSuccess) where LeftType : struct {

            try {
                math2ValueSuccess = true;
                returntype = GetReturnType_Math2Value(typeof(LeftType), right.type);
                double leftValue = GetDouble(typeof(LeftType), left);
                double rightValue = GetDouble(right.type, right.value);
                double finalValue;

                if(typeof(LeftType) == typeof(int)) {
                    leftValue = (int)left;
                }
                else if(typeof(LeftType) == typeof(float)) {
                    leftValue = (float)left;
                }
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
            catch(Exception e) {
                math2ValueSuccess = false;
                returntype = null;
                return null;
            }
        }

       static double GetDouble(Type type, object v){
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

        public static bool MathLogic<LeftType> (LogicToken logicCode, object left, CQ_Value right, out bool mathLogicSuccess) {

            mathLogicSuccess = true;

            try {
                double leftValue = GetDouble(typeof(LeftType), left);
                double rightValue = GetDouble(right.type, right.value);

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

        private static decimal GetDecimalValue (Type type, object value) {

            if(type == typeof(double))
                return (decimal)Convert.ToDouble(value);
            if(type == typeof(float))
                return (decimal)Convert.ToSingle(value);
            if(type == typeof(long))
                return Convert.ToInt64(value);
            if(type == typeof(ulong))
                return Convert.ToUInt64(value);
            if(type == typeof(int))
                return Convert.ToInt32(value);
            if(type == typeof(uint))
                return Convert.ToUInt32(value);
            if(type == typeof(short))
                return Convert.ToInt16(value);
            if(type == typeof(ushort))
                return Convert.ToUInt16(value);
            if(type == typeof(sbyte))
                return Convert.ToSByte(value);
            if(type == typeof(byte))
                return Convert.ToByte(value);
            if(type == typeof(char))
                return Convert.ToChar(value);

            throw new Exception("unknown decimal type...");
        }

       

        private static object ObjectTargetType (Type type, object value) {
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
            throw new Exception("unknown target type...");
        }

        private static object Decimal2TargetType (Type type, decimal value) {
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

    }
}