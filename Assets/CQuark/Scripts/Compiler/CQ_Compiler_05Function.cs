using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark {
    public partial class CQ_Expression_Compiler {
        public static ICQ_Expression Compiler_Expression_Function (IList<Token> tlist, int pos, int posend) {
            CQ_Expression_FunctionCQ func = new CQ_Expression_FunctionCQ(pos, posend, tlist[pos].line, tlist[posend].line);

            func.funcname = tlist[pos].text;
            int begin = pos + 2;
            int dep;
            int end = FindCodeAnyInFunc(tlist, ref begin, out dep);

            if(tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(") {
                do {
                    ICQ_Expression param;
                    bool succ = Compiler_Expression(tlist, begin, end, out param);
                    if(succ && param != null) {
                        func._expressions.Add(param);
                        func.tokenEnd = end;
                        func.lineEnd = tlist[end].line;
                    }
                    begin = end + 2;
                    end = FindCodeAnyInFunc(tlist, ref begin, out dep);

                }
                while(end < posend && begin <= end);

                return func;
            }
            //一般函数
            return null;
        }

        public static ICQ_Expression Compiler_Expression_FunctionThrow (IList<Token> tlist, int pos, int posend) {
            CQ_Expression_Throw func = new CQ_Expression_Throw(pos, posend, tlist[pos].line, tlist[posend].line);

            ICQ_Expression subvalue;
            bool succ = Compiler_Expression(tlist, pos + 1, posend, out subvalue);
            if(succ) {
                func._expressions.Add(subvalue);
            }


            return func;
        }

        public static ICQ_Expression Compiler_Expression_FunctionNew (IList<Token> tlist, int pos, int posend) {
            int begin = pos + 3;
            int dep;
            int end = FindCodeAnyInFunc(tlist, ref begin, out dep);

            if(tlist[pos + 2].type == TokenType.PUNCTUATION && tlist[pos + 2].text == "(") {
                //一般函数
                CQ_Expression_Construction func = new CQ_Expression_Construction(pos, posend, tlist[pos].line, tlist[posend].line);
                func.type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos + 1].text);

                do {
                    ICQ_Expression param;
                    bool succ = Compiler_Expression(tlist, begin, end, out param);
                    if(succ && param != null) {
                        func._expressions.Add(param);
                    }
                    begin = end + 2;
                    end = FindCodeAnyInFunc(tlist, ref begin, out dep);

                }
                while(end < posend && begin <= end);


                return func;
            }
            else if(tlist[pos + 2].type == TokenType.PUNCTUATION && tlist[pos + 2].text == "[")//数组实例化表达式
            {
                CQ_Expression_NewArray func = new CQ_Expression_NewArray(pos, posend, tlist[pos].line, tlist[posend].line);
                func.type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos + 1].text + "[]");

                int valuebegin = 0;
                ICQ_Expression count = null;
                if(tlist[pos + 3].text == "]") {
                    valuebegin = pos + 4;
                }
                else {
                    int nbegin = pos + 3;
                    int dep2;
                    int end2 = FindCodeAny(tlist, ref nbegin, out dep2);

                    bool succ = Compiler_Expression(tlist, nbegin, end2, out count);
                    if(!succ) {
                        throw new Exception("数组数量无法识别:" + tlist[pos].ToString() + tlist[pos].SourcePos());
                    }
                    valuebegin = end2 + 2;
                }
                func._expressions.Add(count);
                if(tlist[valuebegin].text == "{")//InitValue
                {
                    int nbegin = valuebegin + 1;
                    do {
                        int dep2;
                        int nend = FindCodeAny(tlist, ref nbegin, out dep2);
                        ICQ_Expression valueI;
                        bool succ = Compiler_Expression(tlist, nbegin, nend, out valueI);
                        if(succ) {
                            func._expressions.Add(valueI);
                        }
                        if(tlist[nend + 1].text != ",")
                            break;
                        nbegin = nend + 2;
                    }
                    while(nbegin >= pos && nbegin < posend);
                }
                return func;
            }
            return null;
        }
    }
}