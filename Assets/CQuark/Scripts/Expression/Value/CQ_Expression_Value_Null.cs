using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CQuark;

namespace CQuark
{
	public class CQ_Expression_Value_Null : ICQ_Expression_Value
	{
		public TypeBridge type
		{
			get { return null; }
		}

		public string Dump()
		{
			return "<unknown> null";
		}

		public object value
		{
			get
			{
				return null;
			}
		}
		public override string ToString()
		{
			return "<unknown> null";
		}


		public List<ICQ_Expression> listParam
		{
			get { return null; }
		}
		public int tokenBegin
		{
			get;
			set;
		}
		public int tokenEnd
		{
			get;
			set;
		}
		public int lineBegin
		{
			get;
			set;
		}
		public int lineEnd
		{
			get;
			set;
		}
		public bool hasCoroutine{
			get{
				//				if(listParam == null || listParam.Count == 0)
				//					return false;
				//				foreach(ICQ_Expression expr in listParam){
				//					if(expr.hasCoroutine)
				//						return true;
				//				}
				return false;
			}
		}
		public CQ_Value ComputeValue(CQ_Content content)
		{
			content.InStack(this);
			CQ_Value v = new CQ_Value();
			v.type = this.type;
			v.value = null;
			content.OutStack(this);
			return v;
		}
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
	}
}
