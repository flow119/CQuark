using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	public interface ICQ_Environment
	{
		//bool useNamespace
		//{
		//    get;
		//}

		ICQ_Logger logger
		{
			get;
		}
		//public ICQ_Debugger debugger;
		ICQ_TokenParser tokenParser
		{
			get;
		}

		void RegType(ICQ_Type type);
		//void RegDeleType(ICQ_Type_Dele type);
		ICQ_Type GetType(CQType type);
		//ICQ_Type_Dele GetDeleTypeBySign(string sign);
		ICQ_Type GetTypeByKeyword(string keyword);
		ICQ_Type GetTypeByKeywordQuiet(string keyword);

		void RegFunction(ICQ_Function func);
		ICQ_Function GetFunction(string name);

		bool IsCoroutine(string name);
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
}
