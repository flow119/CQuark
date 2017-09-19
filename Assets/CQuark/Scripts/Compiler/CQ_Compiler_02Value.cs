using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler : ICQ_Expression_Compiler
    {

        public ICQ_Expression Compiler_Expression_Value(Token value, int pos)
        {
            if (value.type == TokenType.VALUE)
            {
                if (value.text[value.text.Length - 1] == 'f')
                {
                    CQ_Value_Value<float> number = new CQ_Value_Value<float>();
                    number.value_value = float.Parse(value.text.Substring(0, value.text.Length - 1));
                    return number;
                }
                else if (value.text.Contains("."))
                {
                    CQ_Value_Value<double> number = new CQ_Value_Value<double>();
                    number.value_value = double.Parse(value.text);
                    return number;
                }
                else
                {
                    if (value.text.Contains("'"))
                    {
                        CQ_Value_Value<char> number = new CQ_Value_Value<char>();
                        number.value_value = (char)value.text[1];
                        return number;
                    }

                    else
                    {
                        ulong lv = ulong.Parse(value.text);
                        if (lv > uint.MaxValue)
                        {
                            CQ_Value_Value<long> number = new CQ_Value_Value<long>();
                            number.value_value = (long)lv;
                            return number;
                        }
                        else
                        {

                            CQ_Value_Value<int> number = new CQ_Value_Value<int>();
                            number.value_value = (int)lv;
                            return number;

                        }
                    }

                }
            }
            else if (value.type == TokenType.STRING)
            {
                CQ_Value_Value<string> str = new CQ_Value_Value<string>();
                str.value_value = value.text.Substring(1, value.text.Length - 2);
                return str;
            }
            else if (value.type == TokenType.IDENTIFIER)
            {
                CQ_Expression_GetValue getvalue = new CQ_Expression_GetValue(pos, pos, value.line, value.line);
                getvalue.value_name = value.text;
                return getvalue;
            }
            else if (value.type == TokenType.TYPE)
            {
                CQ_Expression_GetValue getvalue = new CQ_Expression_GetValue(pos, pos, value.line, value.line);
                int l = value.text.LastIndexOf('.');
                if (l >= 0)
                {
                    getvalue.value_name = value.text.Substring(l + 1);
                }
                else
                    getvalue.value_name = value.text;
                return getvalue;
            }
            else
            {
                logger.Log_Error("无法识别的简单表达式" + value);
                return null;
            }
        }

        public ICQ_Expression Compiler_Expression_SubValue(Token value)
        {
            if (value.type == TokenType.VALUE)
            {
                if (value.text[value.text.Length - 1] == 'f')
                {
                    CQ_Value_Value<float> number = new CQ_Value_Value<float>();
                    number.value_value = -float.Parse(value.text.Substring(0, value.text.Length - 1));
                    return number;
                }
                else if (value.text.Contains("."))
                {
                    CQ_Value_Value<double> number = new CQ_Value_Value<double>();
                    number.value_value = -double.Parse(value.text);
                    return number;
                }
                else
                {
                    ulong lv = ulong.Parse(value.text);
                    if (lv > uint.MaxValue)
                    {
                        CQ_Value_Value<long> number = new CQ_Value_Value<long>();
                        number.value_value = -(long)lv;
                        return number;
                    }
                    else
                    {

                        CQ_Value_Value<int> number = new CQ_Value_Value<int>();
                        number.value_value = -(int)lv;
                        return number;

                    }
                }
            }
            else
            {
                logger.Log_Error("无法识别的简单表达式" + value);
                return null;
            }
        }
        public ICQ_Expression Compiler_Expression_NegativeValue(IList<Token> tlist, ICQ_Environment content, int pos, int posend)
        {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);
            if (expend2 != posend)
            {
                LogError(tlist, "无法识别的负号表达式:", expbegin, posend);
                return null;
            }
            else
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, content, expbegin, expend2, out subvalue);
                if (succ && subvalue != null)
                {
                    CQ_Expression_NegativeValue v = new CQ_Expression_NegativeValue(pos, expend2, tlist[pos].line, tlist[expend2].line);
                    v.listParam.Add(subvalue);
                    return v;
                }
                else
                {
                    LogError(tlist, "无法识别的负号表达式:", expbegin, posend);
                    return null;
                }
            }
        }
        public ICQ_Expression Compiler_Expression_NegativeLogic(IList<Token> tlist, ICQ_Environment content, int pos, int posend)
        {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);
            if (expend2 != posend)
            {
                //expend2 = posend;
                LogError(tlist, "无法识别的取反表达式:", expbegin, posend);
                return null;
            }
            //else
            {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, content, expbegin, expend2, out subvalue);
                if (succ && subvalue != null)
                {
                    if (subvalue is CQ_Expression_Math2Value || subvalue is CQ_Expression_Math2ValueAndOr || subvalue is CQ_Expression_Math2ValueLogic)
                    {
                        var pp = subvalue.listParam[0];
                        CQ_Expression_NegativeLogic v = new CQ_Expression_NegativeLogic(pp.tokenBegin, pp.tokenEnd, pp.lineBegin, pp.lineEnd);
                        v.listParam.Add(pp);
                        subvalue.listParam[0] = v;
                        return subvalue;
                    }
                    else
                    {
                        CQ_Expression_NegativeLogic v = new CQ_Expression_NegativeLogic(pos, expend2, tlist[pos].line, tlist[expend2].line);
                        v.listParam.Add(subvalue);
                        return v;
                    }
                }
                else
                {
                    LogError(tlist, "无法识别的取反表达式:", expbegin, posend);
                    return null;
                }
            }
        }
    }
}