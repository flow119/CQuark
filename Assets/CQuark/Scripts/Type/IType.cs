﻿using System;
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
		CQ_Type typeBridge
		{
			get;
		}
		object defaultValue
		{
			get;
		}
        IClass _class
        {
            get;
        }

		ICQ_Expression_Value MakeValue(object value);
		//自动转型能力
		object ConvertTo(object src, CQ_Type targetType);
		//数学计算能力
		object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype);

		//逻辑计算能力
		bool MathLogic(LogicToken code, object left, CQ_Value right);
	}
}