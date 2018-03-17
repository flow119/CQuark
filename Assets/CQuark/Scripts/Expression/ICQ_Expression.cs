using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    //表达式是一个值
    public interface ICQ_Expression
    {
		/// <summary>
		/// 表达式又由多个表达式组成
		/// </summary>
        List<ICQ_Expression> _expressions
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