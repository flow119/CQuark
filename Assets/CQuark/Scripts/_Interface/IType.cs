using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	public interface IType
	{
		string keyword
		{
			get;
		}
		string _namespace
		{
			get;
		}
		TypeBridge typeBridge
		{
			get;
		}
		object defaultValue
		{
			get;
		}
        ICQ_TypeFunction function
        {
            get;
        }

		IValue MakeValue(object value);
		//自动转型能力
		object ConvertTo(object src, TypeBridge targetType);
		//数学计算能力
		object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype);

		//逻辑计算能力
		bool MathLogic(LogicToken code, object left, CQ_Content.Value right);
	}

    //public interface ICQ_Type_Dele : ICQ_Type
    //{
		
    //}
}
