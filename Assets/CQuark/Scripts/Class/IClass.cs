using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
	public interface IClass
	{
        CQ_Value New (CQ_Content content, CQ_Value[] _params);

        CQ_Value StaticCall (CQ_Content content, string function, CQ_Value[] _params);
        CQ_Value StaticCall (CQ_Content content, string function, CQ_Value[] _params, MethodCache cache);
        CQ_Value StaticCallCache (CQ_Content content, CQ_Value[] _params, MethodCache cache);

		CQ_Value StaticValueGet(CQ_Content content, string valuename);
        bool StaticValueSet (CQ_Content content, string valuename, CQ_Value value);

        CQ_Value MemberCall (CQ_Content content, object object_this, string func, CQ_Value[] _params);
        IEnumerator CoroutineCall (CQ_Content content, object object_this, string func, CQ_Value[] _params, UnityEngine.MonoBehaviour coroutin);
        CQ_Value MemberCall (CQ_Content content, object object_this, string func, CQ_Value[] _params, MethodCache cache);
        CQ_Value MemberCallCache (CQ_Content content, object object_this, CQ_Value[] _params, MethodCache cache);

		CQ_Value MemberValueGet(CQ_Content content, object object_this, string valuename);
		bool MemberValueSet(CQ_Content content, object object_this, string valuename, CQ_Value value);

		CQ_Value IndexGet(CQ_Content content, object object_this, object key);
		void IndexSet(CQ_Content content, object object_this, object key, object value);
	}
}
