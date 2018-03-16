using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    //表达式是一个值
    public interface ICQ_Expression
    {
        List<ICQ_Expression> listParam
        {
            get;
        }
        int tokenBegin
        {
            get;
        }
        int tokenEnd
        {
            get;
        }
        int lineBegin
        {
            get;
        }
        int lineEnd
        {
            get;
        }
		bool hasCoroutine
		{
			get;
		}
		CQ_Value ComputeValue(CQ_Content content);
		IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine);
    }
}