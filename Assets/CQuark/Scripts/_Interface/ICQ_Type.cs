using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	public interface ICQ_Type
	{
		string keyword
		{
			get;
		}
		string _namespace
		{
			get;
		}
		CQType type
		{
			get;
		}
		object DefValue
		{
			get;
		}

		ICQ_Value MakeValue(object value);
		//自动转型能力
		object ConvertTo(object src, CQType targetType);

		//数学计算能力
		object Math2Value(char code, object left, CQ_Content.Value right, out CQType returntype);

		//逻辑计算能力
		bool MathLogic(LogicToken code, object left, CQ_Content.Value right);

		ICQ_TypeFunction function
		{
			get;
		}

	}

    //public interface ICQ_Type_Dele : ICQ_Type
    //{
		
    //}
}
