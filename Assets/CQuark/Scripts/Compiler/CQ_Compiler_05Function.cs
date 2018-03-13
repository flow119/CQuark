using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler : ICQ_Expression_Compiler
    {
        public ICQ_Expression Compiler_Expression_Function(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            CQ_Expression_Function func = new CQ_Expression_Function(pos, posend, tlist[pos].line, tlist[posend].line);

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
        public ICQ_Expression Compiler_Expression_FunctionTrace(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(")
                return Compiler_Expression_Function(tlist, env, pos, posend);
            int begin = pos + 1;
            int dep;
            int end = FindCodeAnyInFunc(tlist, ref begin, out dep);
            if (end != posend)
            {
                return null;
            }
            CQ_Expression_Function func = new CQ_Expression_Function(pos, end, tlist[pos].line, tlist[end].line);
            func.funcname = "trace";

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

            //ICQ_Expression param0;
            //bool succ = Compiler_Expression(tlist,content, begin, end, out param0);
            //if(succ&&param0!=null)
            //{
            //    func.listParam.Add(param0);
            //    return func;

            //}
            return func;
            //trace ,单值直接dump,否则按逗号分隔的表达式处理

            //return null;
        }
        public ICQ_Expression Compiler_Expression_FunctionThrow(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            CQ_Expression_Throw func = new CQ_Expression_Throw(pos, posend, tlist[pos].line, tlist[posend].line);

            ICQ_Expression subvalue;
            bool succ = Compiler_Expression(tlist, env, pos + 1, posend, out subvalue);
            if (succ)
            {
                func.listParam.Add(subvalue);
            }


            return func;
            //trace ,单值直接dump,否则按逗号分隔的表达式处理

            //return null;
        }

        public ICQ_Expression Compiler_Expression_FunctionNew(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            int begin = pos + 3;
            int dep;
            int end = FindCodeAnyInFunc(tlist, ref begin, out dep);

            if (tlist[pos + 2].type == TokenType.PUNCTUATION && tlist[pos + 2].text == "(")
            {
                //一般函数
                CQ_Expression_FunctionNew func = new CQ_Expression_FunctionNew(pos, posend, tlist[pos].line, tlist[posend].line);
                func.type = env.GetTypeByKeyword(tlist[pos + 1].text);

                do
                {
                    ICQ_Expression param;
                    bool succ = Compiler_Expression(tlist, env, begin, end, out param);
                    if (succ && param != null)
                    {
                        func.listParam.Add(param);
                    }
                    begin = end + 2;
                    end = FindCodeAnyInFunc(tlist, ref begin, out dep);

                }
                while (end < posend && begin <= end);


                return func;
            }
            else if (tlist[pos + 2].type == TokenType.PUNCTUATION && tlist[pos + 2].text == "[")//数组实例化表达式
            {
                CQ_Expression_FunctionNewArray func = new CQ_Expression_FunctionNewArray(pos, posend, tlist[pos].line, tlist[posend].line);
                func.type = env.GetTypeByKeyword(tlist[pos + 1].text + "[]");

                int valuebegin = 0;
                ICQ_Expression count = null;
                if (tlist[pos + 3].text == "]")
                {
                    valuebegin = pos + 4;
                }
                else
                {
                    int nbegin = pos + 3;
                    int dep2;
                    int end2 = FindCodeAny(tlist, ref nbegin, out dep2);

                    bool succ = Compiler_Expression(tlist, env, nbegin, end2, out count);
                    if (!succ)
                    {
                        throw new Exception("数组数量无法识别:" + tlist[pos].ToString() + tlist[pos].SourcePos());
                    }
                    valuebegin = end2 + 2;
                }
                func.listParam.Add(count);
                if (tlist[valuebegin].text == "{")//InitValue
                {
                    int nbegin = valuebegin + 1;
                    do
                    {
                        int dep2;
                        int nend = FindCodeAny(tlist, ref nbegin, out dep2);
                        ICQ_Expression valueI;
                        bool succ = Compiler_Expression(tlist, env, nbegin, nend, out valueI);
                        if (!succ)
                        {
                            //throw new Exception("数组初始值无法识别");
                        }
                        else
                        {
                            func.listParam.Add(valueI);
                        }
                        if (tlist[nend + 1].text != ",")
                            break;
                        nbegin = nend + 2;
                    }
                    while (nbegin >= pos && nbegin < posend);
                }
                return func;
            }
            return null;
        }

        public ICQ_Expression Compiler_Expression_FunctionStatic(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            CQ_Expression_Function func = new CQ_Expression_Function(pos, posend, tlist[pos].line, tlist[posend].line);
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


        public ICQ_Expression Compiler_Expression_IndexFind(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            CQ_Expression_IndexFind func = new CQ_Expression_IndexFind(pos, posend, tlist[pos].line, tlist[posend].line);
            ICQ_Expression lefv;
            bool b = Compiler_Expression(tlist, env, pos, pos, out lefv);
            if (b) 
            {
                func.listParam.Add(lefv);
            }
            //func.funcname = tlist[pos].text;
            int begin = pos + 2;
            int dep;
            int end = FindCodeAny(tlist, ref begin, out dep);

            if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "[")
            {
                do
                {
                    ICQ_Expression param;
                    bool succ = Compiler_Expression(tlist, env, begin, end, out param);
                    if (succ && param != null)
                    {
                        func.tokenEnd = end;
                        func.lineEnd = tlist[end].line;
                        func.listParam.Add(param);
                    }
                    begin = end + 2;
                    end = FindCodeAny(tlist, ref begin, out dep);

                }
                while (end < posend && begin <= end);


                return func;
            }
            //一般函数
            return null;
        }
    }
}