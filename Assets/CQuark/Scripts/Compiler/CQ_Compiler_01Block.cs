using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler 
    {
        public static void LogError(IList<Token> tlist, string text, int pos, int posend)
        {
            string str = "";
            for (int i = pos; i <= posend;i++ )
            {
                str += tlist[i].text + " ";
            }
            DebugUtil.LogError(text+":" + str + "(" + pos + "-" + posend + ")");
        }
        //可以搞出Block
        public static bool Compiler_Expression_Block(IList<Token> tlist, CQ_Environment env, int pos, int posend, out ICQ_Expression value)
        {
            int begin = pos;
            value = null;
            List<ICQ_Expression> values = new List<ICQ_Expression>();
            int end = 0;
            do
            {
                if (tlist[begin].type == TokenType.COMMENT)
                {
                    begin++;
                    continue;
                }
                if (tlist[begin].type == TokenType.PUNCTUATION && tlist[begin].text == ";")
                {
                    begin++;
                    continue;
                }
                int bdep;
                //脱一次壳
                end = FindCodeInBlock(tlist, ref begin, out bdep);

                if (end > posend)
                {
                    end = posend;
                }
                int expend = end;
                int expbegin = begin;
                if (expbegin > expend)
                {
                    //LogError(tlist, "括号块识别失败", expbegin, expend);
                    return true;
                }
                if (bdep == 2) //编译块表达式
                {
                    expbegin++;
                    expend--;
                    ICQ_Expression subvalue;
                    bool bsucc = Compiler_Expression_Block(tlist,env, expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        LogError(tlist, "表达式编译失败", expbegin, expend);
                        return false;
                    }
                }
                else
                {
                    ICQ_Expression subvalue;
                    bool bsucc = Compiler_Expression(tlist, env,expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        LogError(tlist, "表达式编译失败", expbegin, expend);
                        return false;
                    }
                }




                begin = end + 1;
            }
            while (begin <= posend);
            if (values.Count == 1)
            {
                value = values[0];
            }
            else if (values.Count > 1)
            {
                CQ_Expression_Block block = new CQ_Expression_Block(pos, end, tlist[pos].line, tlist[end].line);
                foreach (var v in values)
                    block.listParam.Add(v);
                value = block;
            }
            return true;
        }

        //不出Block,必须一次解析完,括号为优先级
        public static bool Compiler_Expression(IList<Token> tlist, CQ_Environment env, int pos, int posend, out ICQ_Expression value)
        {
            if(pos>posend)
            {
                value = null;
                return false;
            }
            int begin = pos;
            value = null;
            List<ICQ_Expression> values = new List<ICQ_Expression>();
            do
            {
                if (tlist[begin].type == TokenType.COMMENT)
                {
                    begin++;
                    continue;
                }
                if (tlist[begin].type == TokenType.PUNCTUATION && tlist[begin].text == ";")
                {
                    begin++;
                    continue;
                }
                int bdep;
                //脱一次壳
                int end = FindCodeAny(tlist, ref begin, out bdep);

                if (end > posend)
                {
                    end = posend;
                }

                else if (end < posend)
                {
                    bool bMath = false;
                    for (int i = end + 1; i <= posend; i++)
                    {
                        if (tlist[i].type == TokenType.COMMENT) continue;
                        if (tlist[i].type == TokenType.PUNCTUATION && tlist[i].text == ";") continue;
                        bMath = true;
                        break;
                    }
                    if (bMath)
                    {
                        end = posend;
                        //如果表达式一次搞不完，那肯定是优先级问题
                        value = Compiler_Expression_Math(tlist,env, begin, posend);
                        return true;
                    }
                }
                //else
                //{
                //    IList<int> i = SplitExpressionWithOp(tlist, begin, end);
                //    if (i != null && i.Count > 0)
                //    {
                //        value = Compiler_Expression_Math(tlist, begin, posend);
                //        return true;
                //    }
                //}
                int expend = end;
                int expbegin = begin;
                if (expbegin > expend) return true;
                if (expend == expbegin)
                {//simple
                    if (tlist[expbegin].type == TokenType.KEYWORD)
                    {
                        if (tlist[expbegin].text == "return")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Return(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "表达式编译失败", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);

                        }
                        else if (tlist[expbegin].text == "break")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Break(tlist, expbegin);
                            if (null == subvalue)
                            {
                                //LogError(tlist, "表达式编译失败", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "continue")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Continue(tlist, expbegin);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "true")
                        {
                            CQ_Value_Value<bool> subvalue = new CQ_Value_Value<bool>();
                            subvalue.value_value = true;
                            values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "false")
                        {
                            CQ_Value_Value<bool> subvalue = new CQ_Value_Value<bool>();
                            subvalue.value_value = false;
                            values.Add(subvalue);
                        }
                        else if (tlist[expbegin].text == "null")
                        {
                            CQ_Value_Null subvalue = new CQ_Value_Null();
                            values.Add(subvalue);
                        }

                    }
                    else
                    {
                        ICQ_Expression subvalue = Compiler_Expression_Value(tlist[expbegin],expbegin);
                        if (null == subvalue) return false;
                        else
                            values.Add(subvalue);
                    }
                }
                else if (bdep == 1) //深层表达式
                {
                    expbegin++;
                    expend--;
                    ICQ_Expression subvalue;
                    bool bsucc = Compiler_Expression(tlist,env, expbegin, expend, out subvalue);
                    if (bsucc)
                    {
                        if (subvalue != null)
                            values.Add(subvalue);
                    }
                    else
                    {
                        LogError(tlist, "表达式编译失败", expbegin, expend);
                        return false;
                    }
                }
                else             //尝试各种表达式
                {
                    bool bTest = false;
                    //取反表达式
                    if (tlist[expbegin].type == TokenType.PUNCTUATION && tlist[expbegin].text == "-")
                    {
                        if (tlist[expend].type == TokenType.VALUE)
                        {//负数
                            if (expend == expbegin + 1)
                            {
                                ICQ_Expression subvalue = Compiler_Expression_SubValue(tlist[expend]);
                                if (null == subvalue)
                                {
                                    return false;
                                }
                                else
                                    values.Add(subvalue);
                            }
                            else
                            {
                                ICQ_Expression subvalue = Compiler_Expression_Math(tlist, env, begin, posend);
                                if (null == subvalue)
                                {
                                    LogError(tlist, "表达式编译失败", begin, posend);
                                    return false;
                                }
                                else
                                    values.Add(subvalue);
                            }
                        }
                        else
                        {//负数表达式

                            ICQ_Expression subvalue = Compiler_Expression_NegativeValue(tlist, env, expbegin + 1, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "表达式编译失败", expbegin + 1, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);

                        }
                        bTest = true;
                    }
                    if (tlist[expbegin].type == TokenType.PUNCTUATION && tlist[expbegin].text == "!")
                    {//逻辑反表达式
                        ICQ_Expression subvalue = Compiler_Expression_NegativeLogic(tlist, env, expbegin + 1, expend);
                        if (null == subvalue)
                        {
                            LogError(tlist, "表达式编译失败", expbegin + 1, expend);
                            return false;
                        }
                        else
                            values.Add(subvalue);
                        bTest = true;
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.TYPE)
                    {

                        if (tlist[expbegin + 1].type == TokenType.IDENTIFIER)//定义表达式或者定义并赋值表达式
                        {
                            if (expend == expbegin + 1)//定义表达式
                            {
                                ICQ_Expression subvalue = Compiler_Expression_Define(tlist, env, expbegin, expend);
                                if (null == subvalue)
                                    return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else if (expend > expbegin + 2 && tlist[expbegin + 2].type == TokenType.PUNCTUATION && tlist[expbegin + 2].text == "=")
                            {//定义并赋值表达式
                                ICQ_Expression subvalue = Compiler_Expression_DefineAndSet(tlist, env, expbegin, expend);
                                if (null == subvalue)
                                    return false;
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else
                            {
                                LogError(tlist,"无法识别的表达式:", expbegin ,expend);
                                return false;
                            }
                        }
                        else if (tlist[expbegin + 1].text == "[" && tlist[expbegin + 2].text == "]" && tlist[expbegin + 3].type == TokenType.IDENTIFIER)//定义表达式或者定义并赋值表达式
                        {
                            if (expend == expbegin + 3)//定义表达式
                            {
                                ICQ_Expression subvalue = Compiler_Expression_DefineArray(tlist, env, expbegin, expend);
                                if (null == subvalue)
                                {
                                    LogError(tlist, "无法识别的数组:", expbegin, expend);
                                    return false;
                                }
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else if (expend > expbegin + 4 && tlist[expbegin + 4].type == TokenType.PUNCTUATION && tlist[expbegin + 4].text == "=")
                            {//定义并赋值表达式
                                ICQ_Expression subvalue = Compiler_Expression_DefineAndSetArray(tlist, env, expbegin, expend);
                                if (null == subvalue)
                                {
                                    LogError(tlist, "无法识别的数组:", expbegin, expend);
                                    return false;
                                }
                                else
                                    values.Add(subvalue);
                                bTest = true;
                            }
                            else
                            {
                                LogError(tlist, "无法识别的表达式:", expbegin, expend);
                                return false;
                            }
                        }
                        else if (tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == ".")
                        {//静态调用表达式
                            //if (expend - expbegin > 2)
                            {
                                ICQ_Expression subvalue = Compiler_Expression_Math(tlist, env, expbegin, expend);
                                if (subvalue != null)
                                {
                                    //subvalue.listParam.Add(subparam);
                                    values.Add(subvalue);
                                    bTest = true;
                                }
                                else
                                {
                                    LogError(tlist, "无法识别的表达式:", expbegin, expend);
                                    return false;
                                }
                            }
                        }
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.IDENTIFIER)
                    {
                        if (expend == expbegin + 1)//一元表达式
                        {
                            ICQ_Expression subvalue = Compiler_Expression_MathSelf(tlist, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "无法识别的表达式:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        if (!bTest && tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == "=")//赋值表达式
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Set(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "无法识别的表达式:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        //if (!bTest && tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == "(")//函数表达式
                        //{
                        //    ICQ_Expression subvalue = Compiler_Expression_Function(tlist,content, expbegin, expend);
                        //    if (null == subvalue) return false;
                        //    else
                        //        values.Add(subvalue);
                        //    bTest = true;
                        //}
                        //if (!bTest && tlist[expbegin + 1].type == TokenType.PUNCTUATION && tlist[expbegin + 1].text == "[")//函数表达式
                        //{
                        //    ICQ_Expression subvalue = Compiler_Expression_IndexFind(tlist, content, expbegin, expend);
                        //    if (null == subvalue) return false;
                        //    else
                        //        values.Add(subvalue);
                        //    bTest = true;
                        //}

                    }
                    if (!bTest && (tlist[expbegin].type == TokenType.IDENTIFIER || tlist[expbegin].type == TokenType.VALUE || tlist[expbegin].type == TokenType.STRING))
                    {
                        //算数表达式
                        ICQ_Expression subvalue = Compiler_Expression_Math(tlist,env, expbegin, expend);
                        if (null != subvalue)
                        {
                            values.Add(subvalue);
                            bTest = true;
                        }
                    }
                    if (!bTest && tlist[expbegin].type == TokenType.KEYWORD)
                    {
                        if (tlist[expbegin].text == "for")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_For(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "不可识别的For头:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
						else if(tlist[expbegin].text == "switch"){
//							UnityEngine.Debug.Log("SwitchCase : " + GetCodeKeyString(tlist, expbegin, expend));
							ICQ_Expression subvalue = Compiler_Expression_Loop_SwitchCase(tlist, env, expbegin, expend);
							if(null == subvalue){
								LogError(tlist, "不可识别的switch case：", expbegin, expend);
								return false;
							}
							else
								values.Add(subvalue);
							bTest = true;
						}
                        else if (tlist[expbegin].text == "foreach")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_ForEach(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "不可识别的ForEach头:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "while")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_While(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "不可识别的while头:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "do")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Dowhile(tlist, env, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "if")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_If(tlist, env, expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "不可识别的if判断:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "try")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Try(tlist, env, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "return")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_Loop_Return(tlist, env,expbegin, expend);
                            if (null == subvalue)
                            {
                                LogError(tlist, "不可识别的return:", expbegin, expend);
                                return false;
                            }
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "trace")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_FunctionTrace(tlist,env, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if (tlist[expbegin].text == "throw")
                        {
                            ICQ_Expression subvalue = Compiler_Expression_FunctionThrow(tlist, env, expbegin, expend);
                            if (null == subvalue) return false;
                            else
                                values.Add(subvalue);
                            bTest = true;
                        }
                        else if(tlist[expbegin].text=="true"||tlist[expbegin].text=="false"||tlist[expbegin].text=="null")
                        {
                            //算数表达式
                            ICQ_Expression subvalue = Compiler_Expression_Math(tlist, env, expbegin, expend);
                            if (null != subvalue)
                            {
                                values.Add(subvalue);
                                bTest = true;
                            }
                        }
                        else if(tlist[expbegin].text=="new")
                        {
                            //new 表达式
                            if (tlist[expbegin + 1].type == TokenType.TYPE)
                            {
                                ICQ_Expression subvalue = Compiler_Expression_FunctionNew(tlist, env, pos, posend);
                                values.Add(subvalue);
                                bTest = true;
                            }
                        }
                        else
                        {
                            LogError(tlist, "无法识别的表达式:", expbegin, expend);
                            return false;
                        }
                    }
                    if (!bTest)
                    {
                        LogError(tlist, "无法识别的表达式:", expbegin, expend);
                        return false;
                    }
                }




                begin = end + 1;
            }
            while (begin <= posend);
            if (values.Count == 1)
            {
                value = values[0];
            }
            else if (values.Count > 1)
            {
                LogError(tlist, "异常表达式", pos, posend);
            }
            return true;

        }
    }
}