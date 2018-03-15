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
		object ConvertTo(CQ_Content content, object src, CQType targetType);

		//数学计算能力
		object Math2Value(CQ_Content content, char code, object left, CQ_Content.Value right, out CQType returntype);

		//逻辑计算能力
		bool MathLogic(CQ_Content content, LogicToken code, object left, CQ_Content.Value right);

		ICQ_TypeFunction function
		{
			get;
		}

	}

	public interface ICQ_Type_WithBase : ICQ_Type
	{
		void SetBaseType(IList<ICQ_Type> types);

	}
	public interface ICQ_Type_Dele : ICQ_Type
	{
		Delegate CreateDelegate(DeleFunction lambda);

		Delegate CreateDelegate(DeleLambda lambda);
	}
}
