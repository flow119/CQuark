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
		CQ_Content.Value ComputeValue(CQ_Content content);
		IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine);
    }

    //public interface ICQ_Expression_Compiler
    //{
    //    ICQ_Expression Compile(IList<Token> tlist, CQ_Environment env);//语句
    //    ICQ_Expression Compile_NoBlock(IList<Token> tlist, CQ_Environment env);//表达式，一条语句


    //    IList<ICQ_Type> FileCompile(CQ_Environment env, string filename, IList<Token> tlist, bool embDebugToken);
    //    IList<ICQ_Type> FilePreCompile(CQ_Environment env, string filename, IList<Token> tlist);
    //}
}