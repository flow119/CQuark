using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark {
    public partial class CQ_Expression_Compiler {
        static int FindCodeBlock (IList<Token> tokens, int pos) {
            int dep = 0;
            for(int i = pos; i < tokens.Count; i++) {

                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{") {
                        dep++;
                    }
                    if(tokens[i].text == "}") {
                        dep--;
                        if(dep < 0)
                            return i - 1;
                    }
                }
            }
            if(dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        static int FindCodeAny (IList<Token> tokens, ref int pos, out int depstyle) {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for(int i = pos; i < tokens.Count; i++) {

                if(tokens[i].type == TokenType.COMMENT) { //注释忽略
                    continue;
                }
                if(start == null) {
                    start = tokens[i];

                    pos = i;
                    if(start.Value.type == TokenType.PUNCTUATION) {
                        if(start.Value.text == "{")
                            depstyle = 2;
                        if(start.Value.text == "(")
                            depstyle = 1;
                        if(start.Value.text == "[")
                            depstyle = 1;
                        //bdepstart = true;
                    }
                    if(start.Value.type == TokenType.KEYWORD) {
                        if(start.Value.text == "new") {
                            return FindCodeKeyWord_New(tokens, i);
                        }
                        int index = FindCodeAnyOnlyKeyword(start.Value, tokens, i);
                        if(index > 0)
                            return index;
                    }
                }

                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{") {
                        dep++;
                    }
                    if(tokens[i].text == "}") {
                        dep--;
                        if(depstyle == 2 && dep == 0) {
                            return i;
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "(") {
                        dep++;
                    }
                    if(tokens[i].text == ")") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "(" && dep == 0) {

                                return i;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "[") {
                        dep++;
                    }
                    if(tokens[i].text == "]") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "[" && dep == 0) {
                                return i;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(depstyle == 0) {
                        if(tokens[i].text == ",") {//，结束的表达式
                            if(dep == 0)
                                return i - 1;
                        }
                        if(tokens[i].text == ";") {
                            if(dep == 0)
                                return i - 1;
                        }
                    }
                }
            }
            if(dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        static int FindCodeInBlock (IList<Token> tokens, ref int pos, out int depstyle) {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for(int i = pos; i < tokens.Count; i++) {

                if(tokens[i].type == TokenType.COMMENT) {//注释忽略
                    continue;
                }
                if(start == null) {
                    start = tokens[i];

                    pos = i;
                    if(start.Value.type == TokenType.PUNCTUATION) {
                        if(start.Value.text == "{")
                            depstyle = 2;
                        if(start.Value.text == "(")
                            depstyle = 1;
                        if(start.Value.text == "[")
                            depstyle = 1;
                        //bdepstart = true;
                    }
                    if(start.Value.type == TokenType.KEYWORD) {
                        int index = FindCodeAnyOnlyKeyword(start.Value, tokens, i);
                        if(index > 0)
                            return index;
                    }
                }

                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{") {
                        dep++;
                    }
                    if(tokens[i].text == "}") {
                        dep--;
                        if(depstyle == 2 && dep == 0) {
                            return i;
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "(") {
                        dep++;
                    }
                    if(tokens[i].text == ")") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "(" && dep == 0) {
                                if(i < tokens.Count && tokens[i + 1].text == ".") {
                                    depstyle = 0;
                                }
                                else {
                                    return i;
                                }
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "[") {
                        dep++;
                    }
                    if(tokens[i].text == "]") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "[" && dep == 0) {
                                return i;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(depstyle == 0) {
                        if(tokens[i].text == ",") {//，结束的表达式
                            if(dep == 0)
                                return i - 1;
                        }
                        if(tokens[i].text == ";") {
                            if(dep == 0)
                                return i - 1;
                        }
                    }
                }
            }
            if(dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        static int FindCodeAnyInFunc (IList<Token> tokens, ref int pos, out int depstyle) {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for(int i = pos; i < tokens.Count; i++) {

                if(tokens[i].type == TokenType.COMMENT) {//注释忽略
                    continue;
                }
                if(start == null) {
                    start = tokens[i];

                    pos = i;
                    if(start.Value.type == TokenType.PUNCTUATION) {
                        if(start.Value.text == "{")
                            depstyle = 2;
                        if(start.Value.text == "(")
                            depstyle = 1;
                        if(start.Value.text == "[")
                            depstyle = 1;
                        //bdepstart = true;
                    }
                    if(start.Value.type == TokenType.KEYWORD) {
                        int index = FindCodeAnyOnlyKeyword(start.Value, tokens, i);
                        if(index > 0)
                            return index;
                    }
                }

                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{") {
                        dep++;
                    }
                    if(tokens[i].text == "}") {
                        dep--;
                        if(depstyle == 2 && dep == 0) {
                            return i;
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "(") {
                        dep++;
                    }
                    if(tokens[i].text == ")") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "(" && dep == 0) {
                                depstyle = 0;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "[") {
                        dep++;
                    }
                    if(tokens[i].text == "]") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "[" && dep == 0) {
                                return i;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(depstyle == 0) {
                        //if (tokens[i].text =="."&& start.Value.type == TokenType.TYPE)
                        //{
                        //    if (dep == 0)
                        //        return i - 1;
                        //}
                        if(tokens[i].text == ",") {//，结束的表达式
                            if(dep == 0)
                                return i - 1;
                        }
                        if(tokens[i].text == ";") {
                            if(dep == 0)
                                return i - 1;
                        }
                    }
                }
            }
            if(dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        static int FindCodeAnyOnlyKeyword (Token start, IList<Token> tokens, int startPos) {
            if(start.text == "for") {
                return FindCodeKeyWord_For(tokens, startPos);
            }
            if(start.text == "foreach") {
                return FindCodeKeyWord_ForEach(tokens, startPos);
            }
            if(start.text == "while") {
                return FindCodeKeyWord_While(tokens, startPos);
            }
            if(start.text == "do") {
                return FindCodeKeyWord_Dowhile(tokens, startPos);
            }
            if(start.text == "if") {
                return FindCodeKeyWord_If(tokens, startPos);
            }
            if(start.text == "switch") {
                return FindCodeKeyWord_SwitchCase(tokens, startPos);
            }
            if(start.text == "return") {
                return FindCodeKeyWord_Return(tokens, startPos);
            }
            if(start.text == "yield") {
                return FindCodeKeyWord_Yield(tokens, startPos);
            }
            return -1;
        }

        static int FindCodeAnyWithoutKeyword (IList<Token> tokens, ref int pos, out int depstyle) {
            int dep = 0;
            Token? start = null;
            depstyle = 0;
            for(int i = pos; i < tokens.Count; i++) {
                if(tokens[i].type == TokenType.COMMENT) {//注释忽略
                    continue;
                }
                if(start == null) {
                    start = tokens[i];
                    pos = i;
                    if(start.Value.type == TokenType.PUNCTUATION) {
                        if(start.Value.text == "{")
                            depstyle = 2;
                        if(start.Value.text == "(")
                            depstyle = 1;
                        //bdepstart = true;
                    }
                }

                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{") {
                        dep++;
                    }
                    if(tokens[i].text == "}") {
                        dep--;
                        if(depstyle == 2 && dep == 0) {
                            return i;
                        }
                        if(dep < 0)
                            return i - 1;
                    }
                    if(tokens[i].text == "(") {
                        dep++;
                    }
                    if(tokens[i].text == ")") {
                        dep--;
                        if(depstyle == 1 && dep == 0) {
                            if(start.Value.text == "(" && dep == 0) {
                                return i;
                            }
                        }
                        if(dep < 0)
                            return i - 1;
                    }

                    if(depstyle == 0) {
                        if(tokens[i].text == ",")//，结束的表达式
                        {
                            if(dep == 0)
                                return i - 1;
                        }
                        if(tokens[i].text == ";") {
                            if(dep == 0)
                                return i - 1;
                        }
                    }
                }
            }
            if(dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }


        static int FindCodeKeyWord_New (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 2;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);
            if(tokens[fe1].text == "]") {
                fs1 = fe1 + 1;
                fe1 = FindCodeAny(tokens, ref fs1, out b1);
            }
            return fe1;
        }
        static int FindCodeKeyWord_For (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }
        static int FindCodeKeyWord_ForEach (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }
        static int FindCodeKeyWord_While (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }
        static int FindCodeKeyWord_Dowhile (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }
        static int FindCodeKeyWord_If (IList<Token> tokens, int pos) {
            int b1;
            int fs1 = pos + 1;
            for(; ; ) {
                int fe1 = FindCodeAny(tokens, ref fs1, out b1);

                int b2;
                int fs2 = fe1 + 1;
                int fe2 = FindCodeAny(tokens, ref fs2, out b2);

                int nelse = fe2 + 1;
                if(b2 == 0)
                    nelse++;
                FindCodeAny(tokens, ref nelse, out b2);

                if(tokens.Count > nelse) {
                    if(tokens[nelse].type == TokenType.KEYWORD && tokens[nelse].text == "else") {
                        if(tokens.Count > nelse + 1 && tokens[nelse + 1].type == TokenType.KEYWORD && tokens[nelse + 1].text == "if") {
                            fs1 = nelse + 2;
                            continue;
                        }
                        else {
                            int b3;
                            int fs3 = nelse + 1;
                            int fe3 = FindCodeAny(tokens, ref fs3, out b3);
                            //							UnityEngine.Debug.Log(GetCodeKeyString(tokens, pos, fe3));
                            return fe3;
                        }
                    }
                }
                //				UnityEngine.Debug.Log(GetCodeKeyString(tokens, pos, fe2));
                return fe2;
            }
        }
        static int FindCodeKeyWord_SwitchCase (IList<Token> tokens, int pos) {
            int braceDepth = 0;
            for(int i = pos; i < tokens.Count; i++) {
                if(tokens[i].type == TokenType.PUNCTUATION) {
                    if(tokens[i].text == "{")
                        braceDepth++;
                    else if(tokens[i].text == "}") {
                        braceDepth--;
                        if(braceDepth < 0) {
                            throw new Exception("无法解析的switch");
                        }
                        if(braceDepth == 0) {
                            //							UnityEngine.Debug.Log("完整的switch case : " + GetCodeKeyString(tokens, pos, i));
                            return i;
                        }
                    }
                }
            }
            return tokens.Count - 1;
        }
        static int FindCodeKeyWord_Return (IList<Token> tokens, int pos) {
            int fs = pos + 1;
            if(tokens[fs].type == TokenType.PUNCTUATION && tokens[fs].text == ";")
                return pos;
            int b;
            fs = pos;
            int fe = FindCodeAnyWithoutKeyword(tokens, ref fs, out b);
            return fe;
        }
        static int FindCodeKeyWord_Yield (IList<Token> tokens, int pos) {
            int fs = pos + 1;
            if(tokens[fs].type == TokenType.PUNCTUATION && tokens[fs].text == ";")
                return pos;
            int b;
            fs = pos;
            int fe = FindCodeAnyWithoutKeyword(tokens, ref fs, out b);
            return fe;
        }
        static IList<int> SplitExpressionWithOp (IList<Token> tokens, int pos, int posend) {
            List<int> list = new List<int>();
            List<int> listt = new List<int>();
            int dep = 0;
            int skip = 0;
            for(int i = pos; i <= posend; i++) {
                if(tokens[i].type == TokenType.PUNCTUATION || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "as") || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "is")) {
                    if(tokens[i].text == "(") {
                        if(dep == 0 && (i == pos || tokens[i - 1].type == TokenType.PUNCTUATION) && i + 1 <= posend && tokens[i + 1].type == TokenType.TYPE) {
                            list.Add(i);
                        }
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if(tokens[i].text == "{") {
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if(tokens[i].text == "[") {
                        if(dep == 0) {
                            list.Add(i);
                        }
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if(tokens[i].text == ")" || tokens[i].text == "}" || tokens[i].text == "]") {
                        dep--;
                        if(dep < 0)
                            return null;
                        continue;
                    }

                }

                if(dep == 0 && i > pos && i < posend && i != skip) {
                    if(tokens[i].type == TokenType.PUNCTUATION || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "as") || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "is")) {
                        if(tokens[i].text == "." && tokens[i - 1].type == TokenType.TYPE) {
                            listt.Add(i);
                        }
                        else {
                            list.Add(i);
                        }
                        skip = i + 1;
                    }

                }
            }
            return list.Count > 0 ? list : listt;
        }

        static int GetLowestMathOp (IList<Token> tokens, IList<int> list) {
            int nmax = int.MaxValue;//优先级
            int npos = -1;//字符
            foreach(int i in list) {
                int max = 0;
                switch(tokens[i].text) {
                    case "?":
                        max = -1;
                        break;
                    case ":":
                        max = 0;
                        break;
                    case "<":
                    case ">":
                    case "<=":
                    case ">=":
                        max = 5;
                        break;
                    case "&&":
                        max = 3;
                        break;
                    case "||":
                        max = 3;
                        break;
                    case "==":
                    case "!=":
                        max = 4;
                        break;
                    case "*":
                        max = 7;
                        break;
                    case "/":
                        max = 7;
                        break;
                    case "%":
                        max = 7;
                        break;
                    case "+":
                    case "-":
                        max = 6;
                        break;
                    case ".":
                        max = 10;
                        break;
                    case "=>":
                        max = 8;
                        break;
                    case "[":
                        max = 10;
                        break;
                    case "(":
                        max = 9;//提高括弧的处理顺序到11，已回滚此修改
                        //表达式识别存在缺陷，并非单纯的改动可以解决，可能造成其他的不明显bug
                        break;
                    case "as":
                        max = 9;
                        break;
                    case "is":
                        max = 9;
                        break;
                }
                if(tokens[i].text == "(")//(int)(xxx) //这种表达式要优先处理前一个
                {
                    if(max < nmax) {
                        nmax = max;
                        npos = i;
                    }
                }
                else {
                    if(max <= nmax) {
                        nmax = max;
                        npos = i;
                    }
                }
            }

            return npos;
        }
    }
}
