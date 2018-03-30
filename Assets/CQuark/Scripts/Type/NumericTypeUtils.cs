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
                double srcValue = GetDouble(typeof(OriginalType), src);
                return Double2TargetType(targetType, srcValue);
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
        public static double GetDouble (Type type, object v) {
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

        public static float GetFloat (Type type, object v) {
            if(type == typeof(double))
                return (float)((double)v);
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
            return (float)v;
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

        public static bool CanImplicate (Type from, Type to) {
            //if(from == to)
            //    return true;
            //数值类型
            if(from == typeof(sbyte)) {
                return (to == typeof(short) || to == typeof(int) || to == typeof(long) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            else if(from == typeof(byte)) {
                return (to == typeof(short) || to == typeof(ushort) || to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            else if(from == typeof(short)) {
                return (to == typeof(int) || to == typeof(long) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            else if(from == typeof(ushort)) {
                return (to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            else if(from == typeof(int) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) {
                return (to == typeof(long));
            }
            else if(from == typeof(uint)) {
                return (to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) ;
            }
            else if(from == typeof(long)) {
                return (to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            else if(from == typeof(char)) {
                return (to == typeof(ushort) || to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) ;
            }
            else if(from == typeof(float)) {
                return (to == typeof(double));
            }
            else if(from == typeof(ulong)){
                return (to == typeof(double) || to == typeof(float) || to == typeof(decimal));
            }
            
            //继承
            return(from.IsAssignableFrom(to));
        }
    }
}