using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	//类型
	public interface ICQ_Expression_Value : ICQ_Expression
	{
		CQ_Type type
		{
			get;
		}
		object value
		{
			get;
		}
	}
}
