using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    //值


    //类型
    public interface ICQ_Value : ICQ_Expression
    {
        CQType type
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
    public interface ICQ_Environment
    {
        string version
        {
            get;
        }
        //bool useNamespace
        //{
        //    get;
        //}
        void RegType(ICQ_Type type);
        //void RegDeleType(ICQ_Type_Dele type);
        ICQ_Type GetType(CQType type);
        //ICQ_Type_Dele GetDeleTypeBySign(string sign);
        ICQ_Type GetTypeByKeyword(string keyword);
        ICQ_Type GetTypeByKeywordQuiet(string keyword);

        void RegFunction(ICQ_Function func);
        ICQ_Function GetFunction(string name);

        ICQ_Logger logger
        {
            get;
        }
        //public ICQ_Debugger debugger;
        ICQ_TokenParser tokenParser
        {
            get;
        }
    }
    public interface ICQ_Environment_Compiler
    {

        [Obsolete("use tokenParser.Parse instead.")]
        IList<Token> ParserToken(string code);

        ICQ_Expression Expr_CompileToken(IList<Token> listToken, bool SimpleExpression);

        [Obsolete("use Expr_CompileToken instead.")]
        ICQ_Expression Expr_CompilerToken(IList<Token> listToken, bool SimpleExpression);

        //CQ_Content contentGloabl = null;
        ICQ_Expression Expr_Optimize(ICQ_Expression old);

        CQ_Content CreateContent();


        CQ_Content.Value Expr_Execute(ICQ_Expression expr, CQ_Content content);


        void Project_Compile(Dictionary<string, IList<Token>> project, bool embDebugToken);

        void File_PreCompileToken(string filename, IList<Token> listToken);

        void File_CompileToken(string filename, IList<Token> listToken, bool embDebugToken);

        void Project_PacketToStream(Dictionary<string, IList<Token>> project, System.IO.Stream outstream);

        Dictionary<string, IList<Token>> Project_FromPacketStream(System.IO.Stream instream);
    }

    public interface ICQ_Expression_Compiler
    {
        ICQ_Expression Compile(IList<Token> tlist, ICQ_Environment content);//语句
        ICQ_Expression Compile_NoBlock(IList<Token> tlist, ICQ_Environment content);//表达式，一条语句
        ICQ_Expression Optimize(ICQ_Expression value, ICQ_Environment content);

        IList<ICQ_Type> FileCompile(ICQ_Environment env, string filename, IList<Token> tlist, bool embDebugToken);
        IList<ICQ_Type> FilePreCompile(ICQ_Environment env, string filename, IList<Token> tlist);

    }

}