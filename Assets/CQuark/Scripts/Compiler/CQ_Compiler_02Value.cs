using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark {
    public partial class CQ_Expression_Compiler {
        public static ICQ_Expression Compiler_Expression_Value (Token value, int pos) {
            if(value.type == TokenType.VALUE) {
                if(value.text[value.text.Length - 1] == 'f') {
                    CQ_Value v = new CQ_Value();
					v.SetNumber(typeof(float), float.Parse(value.text.Substring(0, value.text.Length - 1)));
                    return new CQ_Expression_Value(v);
                }
                else if(value.text.Contains(".")) {
                    CQ_Value v = new CQ_Value();
					v.SetNumber(typeof(double), double.Parse(value.text));
                    return new CQ_Expression_Value(v);
                }
                else {
                    if(value.text.Contains("'")) {
                        CQ_Value v = new CQ_Value();
						v.SetNumber(typeof(char), (char)value.text[1]);
                        return new CQ_Expression_Value(v);
                    }

                    else {
                        ulong lv = ulong.Parse(value.text);
                        if(lv > uint.MaxValue) {
                            CQ_Value v = new CQ_Value();
							v.SetNumber(typeof(long), (long)lv);
                            return new CQ_Expression_Value(v);
                        }
                        else {
                            CQ_Value v = new CQ_Value();
							v.SetNumber(typeof(int), (int)lv);
                            return new CQ_Expression_Value(v);
                        }
                    }

                }
            }
            else if(value.type == TokenType.STRING) {
                CQ_Value v = new CQ_Value();
                v.SetObject(typeof(string), value.text.Substring(1, value.text.Length - 2));
                return new CQ_Expression_Value(v);
            }
			else if(value.type == TokenType.IDENTIFIER || value.type == TokenType.PROPERTY) {
                CQ_Expression_GetValue getvalue = new CQ_Expression_GetValue(pos, pos, value.line, value.line);
                getvalue.value_name = value.text;
                return getvalue;
            }
            else if(value.type == TokenType.TYPE) {
                CQ_Expression_GetValue getvalue = new CQ_Expression_GetValue(pos, pos, value.line, value.line);
                int l = value.text.LastIndexOf('.');
                if(l >= 0) {
                    getvalue.value_name = value.text.Substring(l + 1);
                }
                else
                    getvalue.value_name = value.text;
                return getvalue;
            }
            else {
                DebugUtil.LogError("无法识别的简单表达式" + value);
                return null;
            }
        }

        public static ICQ_Expression Compiler_Expression_SubValue (Token value) {
            if(value.type == TokenType.VALUE) {
                if(value.text[value.text.Length - 1] == 'f') {
                    CQ_Value v = new CQ_Value();
					v.SetNumber(typeof(float), -float.Parse(value.text.Substring(0, value.text.Length - 1)));
                    return new CQ_Expression_Value(v);
                }
                else if(value.text.Contains(".")) {
                    CQ_Value v = new CQ_Value();
					v.SetNumber(typeof(double), -double.Parse(value.text));
                    return new CQ_Expression_Value(v);
                }
                else {
                    ulong lv = ulong.Parse(value.text);
                    if(lv > uint.MaxValue) {
                        CQ_Value v = new CQ_Value();
						v.SetNumber(typeof(long), -(long)lv);
                        return new CQ_Expression_Value(v);
                    }
                    else {
                        CQ_Value v = new CQ_Value();
						v.SetNumber(typeof(int), -(int)lv);
                        return new CQ_Expression_Value(v);

                    }
                }
            }
            else {
                DebugUtil.LogError("无法识别的简单表达式" + value);
                return null;
            }
        }
        public static ICQ_Expression Compiler_Expression_NegativeValue (IList<Token> tlist, int pos, int posend) {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);
            if(expend2 != posend) {
                LogError(tlist, "无法识别的负号表达式:", expbegin, posend);
                return null;
            }
            else {
                ICQ_Expression subvalue;
                bool succ = Compiler_Expression(tlist, expbegin, expend2, out subvalue);
                if(succ && subvalue != null) {
                    CQ_Expression_NegativeValue v = new CQ_Expression_NegativeValue(pos, expend2, tlist[pos].line, tlist[expend2].line);
                    v._expressions.Add(subvalue);
                    return v;
                }
                else {
                    LogError(tlist, "无法识别的负号表达式:", expbegin, posend);
                    return null;
                }
            }
        }
        public static ICQ_Expression Compiler_Expression_NegativeLogic (IList<Token> tlist, int pos, int posend) {
            int expbegin = pos;
            int bdep;
            int expend2 = FindCodeAny(tlist, ref expbegin, out bdep);
            if(expend2 != posend) {
                //expend2 = posend;
                LogError(tlist, "无法识别的取反表达式:", expbegin, posend);
                return null;
            }

            ICQ_Expression subvalue;
            bool succ = Compiler_Expression(tlist, expbegin, expend2, out subvalue);
            if(succ && subvalue != null) {
                if(subvalue is CQ_Expression_Math2Value || subvalue is CQ_Expression_Math2ValueAndOr || subvalue is CQ_Expression_Math2ValueLogic) {
                    var pp = subvalue._expressions[0];
                    CQ_Expression_NegativeLogic v = new CQ_Expression_NegativeLogic(pp.tokenBegin, pp.tokenEnd, pp.lineBegin, pp.lineEnd);
                    v._expressions.Add(pp);
                    subvalue._expressions[0] = v;
                    return subvalue;
                }
                else {
                    CQ_Expression_NegativeLogic v = new CQ_Expression_NegativeLogic(pos, expend2, tlist[pos].line, tlist[expend2].line);
                    v._expressions.Add(subvalue);
                    return v;
                }
            }
            else {
                LogError(tlist, "无法识别的取反表达式:", expbegin, posend);
                return null;
            }
        }
    }
}