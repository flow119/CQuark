using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler {

        public static ICQ_Expression Compiler_Expression_Loop_For(IList<Token> tlist, int pos, int posend)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            CQ_Expression_LoopFor value = new CQ_Expression_LoopFor(pos, posend, tlist[pos].line, tlist[posend].line);

            int testbegin = fs1 + 1;
            if (b1 != 1)
            {
                return null;
            }
            do
            {

                int fe2 = FindCodeAny(tlist, ref testbegin, out b1);


                ICQ_Expression subvalue;
//                bool succ = Compiler_Expression(tlist, content, testbegin, fe2, out subvalue);
				Compiler_Expression(tlist, testbegin, fe2, out subvalue);
                //if (!succ) return null;
//                if (subvalue != null)
//                {
                    value.listParam.Add(subvalue);
                    testbegin = fe2 + 2;
//                }
//                else
//                {
//                    value.listParam.Add(null);
//                    testbegin = fe2 + 2;
//                }
            }
            while (testbegin <= fe1);

            if (value.listParam.Count != 3)
            {
                return null;
            }
            ICQ_Expression subvalueblock;

            int b2;
            int fs2 = fe1 + 1;
            int fecode = FindCodeAny(tlist, ref fs2, out b2);
            bool succ2 = Compiler_Expression_Block(tlist, fs2, fecode, out subvalueblock);
            if (succ2)
            {
                value.tokenEnd = fecode;
                value.lineEnd = tlist[fecode].line;
                value.listParam.Add(subvalueblock);
                return value;
            }
            return null;
        }

        public static ICQ_Expression Compiler_Expression_Loop_SwitchCase(IList<Token> tlist, int pos, int posend)
		{
//			UnityEngine.Debug.Log("CompilerLoop : " + GetCodeKeyString(tlist, pos, posend));
			int b1;
			int fs1 = pos + 1;
			int fe1 = FindCodeAny(tlist, ref fs1, out b1);
			CQ_Expression_LoopSwitchCase value = new CQ_Expression_LoopSwitchCase(pos, posend, tlist[pos].line, tlist[posend].line);

//			UnityEngine.Debug.Log("switch : " + GetCodeKeyString(tlist,pos, fe1));
			//switch(xxx)
			ICQ_Expression subvalueblock;
            bool succ = Compiler_Expression_Block(tlist, fs1, fe1, out subvalueblock);
			if(!succ)
				return null;

			value.listParam.Add(subvalueblock);

			int caseBegin = fe1 + 2;

			while(caseBegin < posend){
				if(tlist[caseBegin].type == TokenType.KEYWORD){
					int poscolon = caseBegin;
					for(; poscolon < posend; poscolon++){
						if(tlist[poscolon].type == TokenType.PUNCTUATION && tlist[poscolon].text == ":"){
							break;
						}
					}
					int sexpr = caseBegin;
					if(tlist[caseBegin].text == "case"){
						//case xxx:
						caseBegin ++;
//						UnityEngine.Debug.Log(GetCodeKeyString(tlist,caseBegin, poscolon - 1));
                        bool succ2 = Compiler_Expression_Block(tlist, caseBegin, poscolon - 1, out subvalueblock);
						if(succ2){
							value.listParam.Add(subvalueblock);
							sexpr = poscolon + 1;
						}else{
							return null;
						}
					}else if(tlist[caseBegin].text == "default"){
						//default:
						value.listParam.Add(null);
						sexpr = poscolon + 1;
					}
					else{
						return null;
					}

					if(tlist[sexpr].type == TokenType.KEYWORD && tlist[sexpr].text == "case"){
						//switch(..){case ...:case ...}  //(no break)
						value.listParam.Add(null);
						caseBegin = poscolon + 1;
						continue;
					}else{
						//找到下一个深度为0的case或default或深度为-1的“}”
						int eexpr = sexpr + 1;
//						UnityEngine.Debug.Log("case do ~ : " + GetCodeKeyString(tlist, sexpr, posend));
						int bracedepth = 0;
						for(;eexpr < posend; eexpr++){
							if(tlist[eexpr].type == TokenType.PUNCTUATION && tlist[eexpr].text == "{"){
								bracedepth++;
							}else{
								if(tlist[eexpr].type == TokenType.KEYWORD && (tlist[eexpr].text == "case" || tlist[eexpr].text == "default")){
									if(bracedepth == 0)
										break;
								}
								if(tlist[eexpr].type == TokenType.PUNCTUATION && tlist[eexpr].text == "}"){
									bracedepth--;
									if(bracedepth == -1)
										break;
								}
							}
						}
//						UnityEngine.Debug.Log("case do ~ break" + GetCodeKeyString(tlist, sexpr, eexpr - 1));
                        bool succ3 = Compiler_Expression_Block(tlist, sexpr, eexpr - 1, out subvalueblock);
						if(succ3){
							value.listParam.Add(subvalueblock);
							caseBegin = eexpr;
						}
						else{
							return null;
						}
					}
				}else
					return null;
			}
			return value;
		}

        public static ICQ_Expression Compiler_Expression_Loop_ForEach(IList<Token> tlist, int pos, int posend)
        {

            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            CQ_Expression_LoopForEach value = new CQ_Expression_LoopForEach(pos, fe1, tlist[pos].line, tlist[fe1].line);
            //int testbegin = fs1 + 1;
            if (b1 != 1)
            {
                return null;
            }
            for (int i = fs1 + 1; i <= fe1 - 1; i++)
            {
                if (tlist[i].text == "in" && tlist[i].type == TokenType.KEYWORD)
                {
                    //添加 foreach 定义变量部分
                    {
                        ICQ_Expression subvalue;
                        bool succ = Compiler_Expression(tlist, fs1 + 1, i - 1, out subvalue);
                        if (!succ) return null;
                        if (subvalue != null)
                        {
                            value.listParam.Add(subvalue);
                        }
                    }
                    //添加 foreach 列表部分
                    {
                        ICQ_Expression subvalue;
                        bool succ = Compiler_Expression(tlist, i + 1, fe1 - 1, out subvalue);
                        if (!succ) return null;
                        if (subvalue != null)
                        {

                            value.listParam.Add(subvalue);
                        }
                    }
                    break;
                }
            }

            ICQ_Expression subvalueblock;

            int b2;
            int fs2 = fe1 + 1;
            int fecode = FindCodeAny(tlist, ref fs2, out b2);
            bool succ2 = Compiler_Expression_Block(tlist, fs2, fecode, out subvalueblock);
            if (succ2)
            {
                value.tokenEnd = fecode;
                value.lineEnd = tlist[fecode].line;
                value.listParam.Add(subvalueblock);
                return value;
            }
            return null;
        }
        public static ICQ_Expression Compiler_Expression_Loop_While(IList<Token> tlist, int pos, int posend)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            CQ_Expression_LoopWhile value = new CQ_Expression_LoopWhile(pos, fe1, tlist[pos].line, tlist[fe1].line);


            //while(xxx)
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, fs1, fe1, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe1;
                    value.lineEnd = tlist[fe1].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            //while(...){yyy}

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tlist, ref fs2, out b2);
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression_Block(tlist, fs2, fe2, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe2;
                    value.lineEnd = tlist[fe2].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }
            return value;
        }
        public static ICQ_Expression Compiler_Expression_Loop_Dowhile(IList<Token> tlist, int pos, int posend)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            CQ_Expression_LoopDowhile value = new CQ_Expression_LoopDowhile(pos, fe1, tlist[pos].line, tlist[fe1].line);

            //do(xxx)while(...)
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression_Block(tlist, fs1, fe1, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe1;
                    value.lineEnd = tlist[fe1].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            //do{...]while(yyy);
            if (tlist[fe1 + 1].text != "while") return null;
            int b2;
            int fs2 = fe1 + 2;
            int fe2 = FindCodeAny(tlist, ref fs2, out b2);
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, fs2, fe2, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe2;
                    value.lineEnd = tlist[fe2].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }
            return value;
        }

        public static ICQ_Expression Compiler_Expression_Loop_If(IList<Token> tlist, int pos, int posend)
        {

            CQ_Expression_LoopIf value = new CQ_Expression_LoopIf(pos, posend, tlist[pos].line, tlist[posend].line);
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            if (b1 != 1)
            {
                return null;
            }

            //if(xxx)
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, fs1, fe1, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe1;
                    value.lineEnd = tlist[fe1].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            //if(...){yyy}
            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tlist, ref fs2, out b2);
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression_Block(tlist, fs2, fe2, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe2;
                    value.lineEnd = tlist[fe2].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            int nelse = fe2 + 1;
            if (b2 == 0) nelse++;
            FindCodeAny(tlist, ref nelse, out b2);
            if (tlist.Count > nelse)
            {
                if (tlist[nelse].type == TokenType.KEYWORD && tlist[nelse].text == "else")
                { //if(...){...}else{zzz}
                    int b3;
                    int fs3 = nelse + 1;
                    int fe3 = FindCodeAny(tlist, ref fs3, out b3);
                    ICQ_Expression subvalue;
                    bool succ = Compiler_Expression_Block(tlist, fs3, fe3, out subvalue);
                    if (succ)
                    {
                        value.tokenEnd = fe3;
                        value.lineEnd = tlist[fe3].line;
                        value.listParam.Add(subvalue);
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            return value;
        }
        public static ICQ_Expression Compiler_Expression_Loop_Try(IList<Token> tlist, int pos, int posend)
        {

            CQ_Expression_LoopTry value = new CQ_Expression_LoopTry(pos, posend, tlist[pos].line, tlist[posend].line);
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            if (b1 != 2)
            {
                return null;
            }

            //try
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression_Block(tlist, fs1, fe1, out subvalue);
                if (succ)
                {
                    value.tokenEnd = fe1;
                    value.lineEnd = tlist[fe1].line;
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            while (fe1 < posend && tlist[fe1 + 1].text == "catch")
            {
                //catch(...)

                int b2;
                int fs2 = fe1 + 2;
                int fe2 = FindCodeAny(tlist, ref fs2, out b2);
                {
                    if (b2 != 1)
                    {
                        return null;
                    }
                    ICQ_Expression subvalue;
                    bool succ = Compiler_Expression(tlist, fs2, fe2, out subvalue);
                    if (succ)
                    {
                        value.tokenEnd = fe2;
                        value.lineEnd = tlist[fe2].line;
                        value.listParam.Add(subvalue);
                    }
                    else
                    {
                        return null;
                    }
                }
                //catch(){...}

                {
                    int b3;
                    int fs3 = fe2 + 1;
                    int fe3 = FindCodeAny(tlist, ref fs3, out b3);
                    if (b3 != 2)
                    {
                        return null;
                    }

                    ICQ_Expression subvalue;
                    bool succ = Compiler_Expression_Block(tlist, fs3, fe3, out subvalue);
                    if (succ)
                    {
                        value.tokenEnd = fe3;
                        value.lineEnd = tlist[fe3].line;
                        value.listParam.Add(subvalue);
                    }
                    else
                    {
                        return null;
                    }
                    fe1 = fe3;
                }
            }


            return value;
        }
        public static ICQ_Expression Compiler_Expression_Loop_Return(IList<Token> tlist, int pos, int posend)
        {
            CQ_Expression_LoopReturn value = new CQ_Expression_LoopReturn(pos, posend, tlist[pos].line, tlist[posend].line);

            ICQ_Expression subvalue;
            bool succ = Compiler_Expression(tlist, pos + 1, posend, out subvalue);
            if (succ)
            {
                value.listParam.Add(subvalue);
            }

            return value;
        }
        public static ICQ_Expression Compiler_Expression_Loop_Break(IList<Token> tlist, int pos)
        {
            CQ_Expression_LoopBreak value = new CQ_Expression_LoopBreak(pos, pos, tlist[pos].line, tlist[pos].line);
            return value;
        }
        public static ICQ_Expression Compiler_Expression_Loop_Continue(IList<Token> tlist, int pos)
        {
            CQ_Expression_LoopContinue value = new CQ_Expression_LoopContinue(pos, pos, tlist[pos].line, tlist[pos].line);
            return value;
        }
    }
}
