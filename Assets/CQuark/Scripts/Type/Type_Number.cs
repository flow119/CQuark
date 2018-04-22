using System.Collections;
using System.Collections.Generic;
using CQuark;
using System;

namespace CQuark{
	public class Type_Number : Type_Generic {

		public Type_Number(Type t, string key, object defaultV) : base(t, key, defaultV){}

		public override object ConvertTo(object src, TypeBridge targetType)
		{
			if(NumberUtil.IsNumberType(_type) && NumberUtil.IsNumberType(targetType.type)){
				double srcValue = NumberUtil.GetDouble(_type, src);
				return NumberUtil.Double2TargetType(targetType, srcValue);
			}
			else {
				return base.ConvertTo(src, targetType);
			}
		}

		public override CQ_Value Math2Value (char code, CQ_Value left, CQ_Value right) {
			CQ_Value returnValue = CQ_Value.Null;

			if(NumberMath2Value(code, left, right, out returnValue)) {
				return returnValue;
			}

			return base.Math2Value(code, left, right);
		}


		//快速计算
		protected static bool NumberMath2Value (char opCode, CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
			try {
				returnValue = new CQ_Value();
				Type retType = NumberUtil.GetReturnType_Math2Value(left.m_type, right.m_type);

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
				if(NumberUtil.IsNumberType(retType))
					returnValue.SetNumber(retType,  NumberUtil.ConvertNumber(finalValue, retType));
				return true;

			}
			catch(Exception) {
				returnValue = CQ_Value.Null;
				return false;
			}
		}

		public override bool MathLogic(LogicToken code, CQ_Value left, CQ_Value right)
		{
			bool mathLogicSuccess = false;
			bool value = NumberUtil.NumberMathLogic(code, left, right, out mathLogicSuccess);
			if (mathLogicSuccess) {
				return value;
			}

			return base.MathLogic(code, left, right);
		}
	}
}
