using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	public interface ICQ_Function
	{
		CQ_Value New(CQ_Content content, IList<CQ_Value> _params);

		CQ_Value StaticCall(CQ_Content content, string function, IList<CQ_Value> _params);
		CQ_Value StaticCall(CQ_Content content, string function, IList<CQ_Value> _params, MethodCache cache);
		CQ_Value StaticCallCache(CQ_Content content, IList<CQ_Value> _params, MethodCache cache);

		CQ_Value StaticValueGet(CQ_Content content, string valuename);
		bool StaticValueSet(CQ_Content content, string valuename, object value);

		bool HasFunction(string key);
		CQ_Value MemberCall(CQ_Content content, object object_this, string func, IList<CQ_Value> _params);
		IEnumerator CoroutineCall(CQ_Content content, object object_this, string func, IList<CQ_Value> _params, ICoroutine coroutin);
		CQ_Value MemberCall(CQ_Content content, object object_this, string func, IList<CQ_Value> _params, MethodCache cache);
		CQ_Value MemberCallCache(CQ_Content content, object object_this, IList<CQ_Value> _params, MethodCache cache);

		CQ_Value MemberValueGet(CQ_Content content, object object_this, string valuename);
		bool MemberValueSet(CQ_Content content, object object_this, string valuename, object value);

		CQ_Value IndexGet(CQ_Content content, object object_this, object key);
		void IndexSet(CQ_Content content, object object_this, object key, object value);
	}
}
