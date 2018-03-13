using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler
    {
        public ICQ_Expression Compiler_Expression_Coroutine(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
			CQ_Expression_Coroutine func = new CQ_Expression_Coroutine(pos, posend, tlist[pos].line, tlist[posend].line);

            func.funcname = tlist[pos].text;
            int begin = pos + 2;
            int dep;
            int end = FindCodeAnyInFunc(tlist, ref begin, out dep);

            if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(")
            {
                do
                {
                    ICQ_Expression param;
                    bool succ = Compiler_Expression(tlist, env, begin, end, out param);
                    if (succ && param != null)
                    {
                        func.listParam.Add(param);
                        func.tokenEnd = end;
                        func.lineEnd = tlist[end].line;
                    }
                    begin = end + 2;
                    end = FindCodeAnyInFunc(tlist, ref begin, out dep);

                }
                while (end < posend && begin <= end);


                return func;
            }
            //一般函数
            return null;
        }
	}  
}