using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler 
    {

        public ICQ_Expression Compiler_Expression_Math(IList<Token> tlist, CQ_Environment env, int pos, int posend)
        {
            IList<int> sps = SplitExpressionWithOp(tlist, pos, posend);
            int oppos = GetLowestMathOp(tlist, sps);
            if (oppos < 0)
            {
                ////也有可能是类型转换
                //if (posend >= pos + 3 && tlist[pos].text == "(" && tlist[pos].type == TokenType.PUNCTUATION && tlist[pos + 2].text == ")" && tlist[pos + 2].type == TokenType.PUNCTUATION
                //    && tlist[pos + 1].type == TokenType.TYPE
                //    )
                //{
                //    ICQ_Expression v;
                //    bool succ = Compiler_Expression(tlist, content, pos + 3, posend, out v);
                //    CQ_Expression_TypeConvert convert = new CQ_Expression_TypeConvert();
                //    convert.listParam.Add(v);
                //    convert.targettype = content.environment.GetTypeByKeyword(tlist[pos + 1].text).type;


                //    return convert;
                //}
                //else if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "[")//函数表达式
                //{
                //    return Compiler_Expression_IndexFind(tlist, content, pos, posend);
                //}
                if (tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(")//函数表达式
                {
					if(env.IsCoroutine(tlist[pos].text))
//					if(tlist[pos].text == "YieldWaitForSecond")
						return Compiler_Expression_Coroutine(tlist, env, pos, posend);
					else
	                    return Compiler_Expression_Function(tlist, env, pos, posend);
                }
                else
                {
                    //if (!bTest && tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == "(")//函数表达式
                    //{
                    //    ICQ_Expression subvalue = Compiler_Expression_Function(tlist,content, expbegin, expend);
                    //    if (null == subvalue) return false;
                    //    else
                    //        values.Add(subvalue);
                    //    bTest = true;
                    //}
                }
                return null;
            }
            Token tkCur = tlist[oppos];
            if (tkCur.text == "=>")
            {
				//lambda
                return Compiler_Expression_Lambda(tlist, env, pos, posend);
            }
            else if (tkCur.text == "." && pos == oppos - 1 && tlist[pos].type == TokenType.TYPE)
            {
                int right = oppos + 1;
                int rightend = posend;

                ICQ_Expression valueright;
                bool succ2 = Compiler_Expression(tlist, env, right, rightend, out valueright);
                if (succ2)
                {
                    CQ_Expression_GetValue vg = valueright as CQ_Expression_GetValue;
                    CQ_Expression_Function vf = valueright as CQ_Expression_Function;
                    if (vg != null)
                    {
                        CQ_Expression_StaticFind value = new CQ_Expression_StaticFind(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.staticmembername = vg.value_name;
                        value.type = env.GetTypeByKeyword(tlist[pos].text);
                        return value;
                    }
                    else if (vf != null)
                    {
                        CQ_Expression_StaticFunction value = new CQ_Expression_StaticFunction(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.functionName = vf.funcname;
                        value.type = env.GetTypeByKeyword(tlist[pos].text);
                        //value.listParam.Add(valueleft);
                        value.listParam.AddRange(vf.listParam.ToArray());
                        return value;
                    }
                    else if (valueright is CQ_Expression_SelfOp)
                    {
                        CQ_Expression_SelfOp vr = valueright as CQ_Expression_SelfOp;

                        CQ_Expression_StaticMath value = new CQ_Expression_StaticMath(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.type = env.GetTypeByKeyword(tlist[pos].text);
                        value.staticmembername = vr.value_name;
                        value.mathop = vr.mathop;
                        return value;
                    }
                    else
                    {
                        throw new Exception("不可识别的表达式:" + tkCur.ToString() + tkCur.SourcePos());
                    }
                }
                else
                {
                    throw new Exception("不可识别的表达式:" + tkCur.ToString() + tkCur.SourcePos());
                }
            }
            else
            {
                int left = pos;
                int leftend = oppos - 1;
                int right = oppos + 1;
                int rightend = posend;
                if (tkCur.text == "(")
                {
                    ICQ_Expression v;
                    if (!Compiler_Expression(tlist, env, oppos + 3, posend, out v))
                    {
                        LogError(tlist, "编译表达式失败", right, rightend);
                        return null;
                    }
                    CQ_Expression_TypeConvert convert = new CQ_Expression_TypeConvert(pos, posend, tlist[pos].line, tlist[posend].line);
                    convert.listParam.Add(v);
                    convert.targettype = env.GetTypeByKeyword(tlist[oppos + 1].text).type;

                    return convert;
                }
                ICQ_Expression valueleft;
                bool succ1 = Compiler_Expression(tlist, env, left, leftend, out valueleft);
                ICQ_Expression valueright;
                if (tkCur.text == "[")
                {
                    rightend--;
                    if (!Compiler_Expression(tlist, env, right, rightend, out valueright))
                    {
                        LogError(tlist, "编译表达式失败", right, rightend);
                        return null;
                    }
                    CQ_Expression_IndexFind value = new CQ_Expression_IndexFind(left, rightend, tlist[left].line, tlist[rightend].line);
                    value.listParam.Add(valueleft);
                    value.listParam.Add(valueright);
                    return value;
                }
                else if (tkCur.text == "as")
                {
                    CQ_Expression_TypeConvert convert = new CQ_Expression_TypeConvert(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    convert.listParam.Add(valueleft);
                    convert.targettype = env.GetTypeByKeyword(tlist[oppos + 1].text).type;


                    return convert;
                }
                else if (tkCur.text == "is")
                {
                    CQ_Expression_TypeCheck check = new CQ_Expression_TypeCheck(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    check.listParam.Add(valueleft);
                    check.targettype = env.GetTypeByKeyword(tlist[oppos + 1].text).type;


                    return check;
                }
                bool succ2 = Compiler_Expression(tlist, env, right, rightend, out valueright);
                if (succ1 && succ2 && valueright != null && valueleft != null)
                {
                    if (tkCur.text == "=")
                    {
                        //member set

                        CQ_Expression_MemberFind mfinde = valueleft as CQ_Expression_MemberFind;
                        CQ_Expression_StaticFind sfinde = valueleft as CQ_Expression_StaticFind;
                        CQ_Expression_IndexFind ifinde = valueleft as CQ_Expression_IndexFind;
                        if (mfinde != null)
                        {
                            CQ_Expression_MemberSetValue value = new CQ_Expression_MemberSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.membername = mfinde.membername;
                            value.listParam.Add(mfinde.listParam[0]);
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else if (sfinde != null)
                        {
                            CQ_Expression_StaticSetValue value = new CQ_Expression_StaticSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.staticmembername = sfinde.staticmembername;
                            value.type = sfinde.type;
                            //value.listParam.Add(mfinde.listParam[0]);
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else if (ifinde != null)
                        {
                            CQ_Expression_IndexSetValue value = new CQ_Expression_IndexSetValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(ifinde.listParam[0]);
                            value.listParam.Add(ifinde.listParam[1]);
                            value.listParam.Add(valueright);
                            return value;
                        }
                        else
                        {
                            throw new Exception("非法的Member Set表达式" + valueleft);
                        }
                    }
                    else if (tkCur.text == ".")
                    {
                        //FindMember
                        CQ_Expression_GetValue vg = valueright as CQ_Expression_GetValue;
                        CQ_Expression_Function vf = valueright as CQ_Expression_Function;

                        if (vg != null)
                        {
                            CQ_Expression_MemberFind value = new CQ_Expression_MemberFind(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.membername = vg.value_name;
                            return value;
                        }
                        else if (vf != null)
                        {
                            CQ_Expression_MemberFunction value = new CQ_Expression_MemberFunction(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.functionName = vf.funcname;
                            value.listParam.Add(valueleft);
                            value.listParam.AddRange(vf.listParam.ToArray());
                            return value;
                        }

                        else if (valueright is CQ_Expression_SelfOp)
                        {
                            CQ_Expression_SelfOp vr = valueright as CQ_Expression_SelfOp;

                            CQ_Expression_MemberMath value = new CQ_Expression_MemberMath(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.membername = vr.value_name;
                            value.mathop = vr.mathop;
                            return value;
                        }

                        throw new Exception("不可识别的表达式" + valueleft + "." + valueright);
                        //value.listParam.Add(valueright);
                    }
                    else if (tkCur.text == "+=" || tkCur.text == "-=" || tkCur.text == "*=" || tkCur.text == "/=" || tkCur.text == "%=")
                    {

                        //if (valueleft is CQ_Expression_MemberFind)
                        //{
                        //    CQ_Expression_MemberFind vf = valueleft as CQ_Expression_MemberFind;
                        //    CQ_Expression_MemberMath value = new CQ_Expression_MemberMath(left, rightend, tlist[left].line, tlist[rightend].line);
                        //    value.listParam.Add(vf.listParam[0]);
                        //    value.membername = vf.membername;
                        //    value.mathop = tlist[oppos].text[0];
                        //    value.listParam.Add(valueright);
                        //    return value;
                        //}
                        //if ((valueright is CQ_Expression_Lambda ==false) && valueleft is CQ_Expression_StaticFind)
                        //{
                        //    CQ_Expression_StaticFind vf = valueleft as CQ_Expression_StaticFind;
                        //    CQ_Expression_StaticMath value = new CQ_Expression_StaticMath(left, rightend, tlist[left].line, tlist[rightend].line);
                        //    value.type = vf.type;
                        //    value.staticmembername = vf.staticmembername;
                        //    value.mathop = tlist[oppos].text[0];
                        //    value.listParam.Add(valueright);
                        //    return value;
                        //}
                        //else
                        {
                            CQ_Expression_SelfOpWithValue value = new CQ_Expression_SelfOpWithValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            //value.value_name = ((CQ_Expression_GetValue)valueleft).value_name;
                            value.listParam.Add(valueleft);
                            value.listParam.Add(valueright);
                            value.mathop = tkCur.text[0];
                            return value;
                        }
                    }
                    else if (tkCur.text == "&&" || tkCur.text == "||")
                    {
                        CQ_Expression_Math2ValueAndOr value = new CQ_Expression_Math2ValueAndOr(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        value.mathop = tkCur.text[0];
                        return value;
                    }
                    else if (tkCur.text == ">" || tkCur.text == ">=" || tkCur.text == "<" || tkCur.text == "<=" || tkCur.text == "==" || tkCur.text == "!=")
                    {
                        CQ_Expression_Math2ValueLogic value = new CQ_Expression_Math2ValueLogic(left, rightend, tlist[left].line, tlist[rightend].line);
                        value.listParam.Add(valueleft);
                        value.listParam.Add(valueright);
                        logictoken token = logictoken.not_equal;
                        if (tkCur.text == ">")
                        {
                            token = logictoken.more;
                        }
                        else if (tkCur.text == ">=")
                        {
                            token = logictoken.more_equal;
                        }
                        else if (tkCur.text == "<")
                        {
                            token = logictoken.less;
                        }
                        else if (tkCur.text == "<=")
                        {
                            token = logictoken.less_equal;
                        }
                        else if (tkCur.text == "==")
                        {
                            token = logictoken.equal;
                        }
                        else if (tkCur.text == "!=")
                        {
                            token = logictoken.not_equal;
                        }
                        value.mathop = token;
                        return value;
                    }
                    else
                    {
                        char mathop = tkCur.text[0];
                        if (mathop == '?')
                        {
                            CQ_Expression_Math3Value value = new CQ_Expression_Math3Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);

                            CQ_Expression_Math2Value vvright = valueright as CQ_Expression_Math2Value;
                            if (vvright.mathop != ':')
                                throw new Exception("三元表达式异常" + tkCur.ToString() + tkCur.SourcePos());
                            value.listParam.Add(vvright.listParam[0]);
                            value.listParam.Add(vvright.listParam[1]);
                            return value;
                        }
                        else
                        {
                            CQ_Expression_Math2Value value = new CQ_Expression_Math2Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.listParam.Add(valueleft);
                            value.listParam.Add(valueright);
                            value.mathop = mathop;
                            return value;
                        }

                    }
                }
                else
                {
                    LogError(tlist, "编译表达式失败", right, rightend);
                }
            }

            return null;
        }
        public ICQ_Expression Compiler_Expression_MathSelf(IList<Token> tlist, int pos, int posend)
        {
            CQ_Expression_SelfOp value = new CQ_Expression_SelfOp(pos, posend, tlist[pos].line, tlist[posend].line);
            value.value_name = tlist[pos].text;
            value.mathop = tlist[pos + 1].text[0];

            return value;
        }
    }
}
