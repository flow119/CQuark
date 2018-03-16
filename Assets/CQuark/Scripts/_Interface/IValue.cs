using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	//类型
	public interface IValue : ICQ_Expression
	{
		TypeBridge type
		{
			get;
		}
		object value
		{
			get;
		}
		new int tokenBegin
		{
			get;
			set;
		}
		new int tokenEnd
		{
			get;
			set;
		}
		new int lineBegin
		{
			get;
			set;
		}
		new int lineEnd
		{
			get;
			set;
		}
	}
}
