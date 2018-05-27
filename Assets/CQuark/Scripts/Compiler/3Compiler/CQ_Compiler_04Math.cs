using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark {
    public partial class CQ_Expression_Compiler {

        public static ICQ_Expression Compiler_Expression_Math (IList<Token> tlist, int pos, int posend) {
			string debug = "";
			for(int i = pos; i <= posend; i++){
				debug += tlist[i].text;
			}
            IList<int> sps = SplitExpressionWithOp(tlist, pos, posend);
            int oppos = GetLowestMathOp(tlist, sps);
            if(oppos < 0) {

                if(tlist[pos + 1].type == TokenType.PUNCTUATION && tlist[pos + 1].text == "(")//函数表达式
                {
                    return Compiler_Expression_Function(tlist, pos, posend);
                }

                return null;
            }
            Token tkCur = tlist[oppos];
            if(tkCur.text == "=>") {
                //lambda
                return Compiler_Expression_Lambda(tlist, pos, posend);
            }
            else if(tkCur.text == "." && pos == oppos - 1 && tlist[pos].type == TokenType.TYPE) {
                int right = oppos + 1;
                int rightend = posend;

                ICQ_Expression valueright;
                bool succ2 = Compiler_Expression(tlist, right, rightend, out valueright);
                if(succ2) {
                    CQ_Expression_GetValue vg = valueright as CQ_Expression_GetValue;
                    CQ_Expression_FunctionCQ vf = valueright as CQ_Expression_FunctionCQ;
                    if(vg != null) {
                        CQ_Expression_StaticValueGet value = new CQ_Expression_StaticValueGet(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.staticmembername = vg.value_name;
                        value.type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text);
                        return value;
                    }
                    else if(vf != null) {
                        CQ_Expression_StaticFunction value = new CQ_Expression_StaticFunction(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.functionName = vf.funcname;
                        value.type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text);
                        //value._expressions.Add(valueleft);
                        value._expressions.AddRange(vf._expressions.ToArray());
                        return value;
                    }
                    else if(valueright is CQ_Expression_SelfOp) {
                        CQ_Expression_SelfOp vr = valueright as CQ_Expression_SelfOp;

                        CQ_Expression_StaticValueOp value = new CQ_Expression_StaticValueOp(pos, rightend, tlist[pos].line, tlist[rightend].line);
                        value.type = CQuark.AppDomain.GetTypeByKeyword(tlist[pos].text);
                        value.staticmembername = vr.value_name;
                        value.mathop = vr.mathop;
                        return value;
                    }
                    else {
                        throw new Exception("不可识别的表达式:" + tkCur.ToString() + tkCur.SourcePos());
                    }
                }
                else {
                    throw new Exception("不可识别的表达式:" + tkCur.ToString() + tkCur.SourcePos());
                }
            }
            else {
                int left = pos;
                int leftend = oppos - 1;
                int right = oppos + 1;
                int rightend = posend;
                if(tkCur.text == "(") {
                    ICQ_Expression v;
                    if(!Compiler_Expression(tlist, oppos + 3, posend, out v)) {
                        LogError(tlist, "编译表达式失败", right, rightend);
                        return null;
                    }
                    CQ_Expression_TypeConvert convert = new CQ_Expression_TypeConvert(pos, posend, tlist[pos].line, tlist[posend].line);
                    convert._expressions.Add(v);
                    convert.targettype = CQuark.AppDomain.GetTypeByKeyword(tlist[oppos + 1].text).typeBridge;

                    return convert;
                }
                ICQ_Expression valueleft;
                bool succ1 = Compiler_Expression(tlist, left, leftend, out valueleft);
                ICQ_Expression valueright;
                if(tkCur.text == "[") {
                    rightend--;
                    if(!Compiler_Expression(tlist, right, rightend, out valueright)) {
                        LogError(tlist, "编译表达式失败", right, rightend);
                        return null;
                    }
                    CQ_Expression_IndexGet value = new CQ_Expression_IndexGet(left, rightend, tlist[left].line, tlist[rightend].line);
                    value._expressions.Add(valueleft);
                    value._expressions.Add(valueright);
                    return value;
                }
                else if(tkCur.text == "as") {
                    CQ_Expression_TypeConvert convert = new CQ_Expression_TypeConvert(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    convert._expressions.Add(valueleft);
                    convert.targettype = CQuark.AppDomain.GetTypeByKeyword(tlist[oppos + 1].text).typeBridge;

                    return convert;
                }
                else if(tkCur.text == "is") {
                    CQ_Expression_TypeCheck check = new CQ_Expression_TypeCheck(left, oppos + 1, tlist[left].line, tlist[oppos + 1].line);
                    check._expressions.Add(valueleft);
                    check.targettype = CQuark.AppDomain.GetTypeByKeyword(tlist[oppos + 1].text).typeBridge;


                    return check;
                }
                bool succ2 = Compiler_Expression(tlist, right, rightend, out valueright);
                if(succ1 && succ2 && valueright != null && valueleft != null) {
                    if(tkCur.text == "=") {
                        //member set

                        CQ_Expression_MemberValueGet mfinde = valueleft as CQ_Expression_MemberValueGet;
                        CQ_Expression_StaticValueGet sfinde = valueleft as CQ_Expression_StaticValueGet;
                        CQ_Expression_IndexGet ifinde = valueleft as CQ_Expression_IndexGet;
                        if(mfinde != null) {
                            CQ_Expression_MemberValueSet value = new CQ_Expression_MemberValueSet(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.membername = mfinde.membername;
                            value._expressions.Add(mfinde._expressions[0]);
                            value._expressions.Add(valueright);
                            return value;
                        }
                        else if(sfinde != null) {
                            CQ_Expression_StaticValueSet value = new CQ_Expression_StaticValueSet(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.staticmembername = sfinde.staticmembername;
                            value.type = sfinde.type;
                            //value._expressions.Add(mfinde._expressions[0]);
                            value._expressions.Add(valueright);
                            return value;
                        }
                        else if(ifinde != null) {
                            CQ_Expression_IndexSet value = new CQ_Expression_IndexSet(left, rightend, tlist[left].line, tlist[rightend].line);
                            value._expressions.Add(ifinde._expressions[0]);
                            value._expressions.Add(ifinde._expressions[1]);
                            value._expressions.Add(valueright);
                            return value;
                        }
                        else {
                            throw new Exception("非法的Member Set表达式" + valueleft);
                        }
                    }
                    else if(tkCur.text == ".") {
                        //FindMember
                        CQ_Expression_GetValue vg = valueright as CQ_Expression_GetValue;
                        CQ_Expression_FunctionCQ vf = valueright as CQ_Expression_FunctionCQ;

                        if(vg != null) {
                            CQ_Expression_MemberValueGet value = new CQ_Expression_MemberValueGet(left, rightend, tlist[left].line, tlist[rightend].line);
                            value._expressions.Add(valueleft);
                            value.membername = vg.value_name;
                            return value;
                        }
                        else if(vf != null) {
                            CQ_Expression_MemberFunction value = new CQ_Expression_MemberFunction(left, rightend, tlist[left].line, tlist[rightend].line);
                            value.functionName = vf.funcname;
                            value._expressions.Add(valueleft);
                            value._expressions.AddRange(vf._expressions.ToArray());
                            return value;
                        }

                        else if(valueright is CQ_Expression_SelfOp) {
                            CQ_Expression_SelfOp vr = valueright as CQ_Expression_SelfOp;

                            CQ_Expression_MemberValueOp value = new CQ_Expression_MemberValueOp(left, rightend, tlist[left].line, tlist[rightend].line);
                            value._expressions.Add(valueleft);
                            value.membername = vr.value_name;
                            value.mathop = vr.mathop;
                            return value;
                        }

                        throw new Exception("不可识别的表达式" + valueleft + "." + valueright);
                        //value._expressions.Add(valueright);
                    }
                    else if(tkCur.text == "+=" || tkCur.text == "-=" || tkCur.text == "*=" || tkCur.text == "/=" || tkCur.text == "%=") {
                        {
                            CQ_Expression_SelfOpWithValue value = new CQ_Expression_SelfOpWithValue(left, rightend, tlist[left].line, tlist[rightend].line);
                            //value.value_name = ((CQ_Expression_GetValue)valueleft).value_name;
                            value._expressions.Add(valueleft);
                            value._expressions.Add(valueright);
                            value.mathop = tkCur.text[0];
                            return value;
                        }
                    }
                    else if(tkCur.text == "&&" || tkCur.text == "||") {
                        CQ_Expression_Math2ValueAndOr value = new CQ_Expression_Math2ValueAndOr(left, rightend, tlist[left].line, tlist[rightend].line);
                        value._expressions.Add(valueleft);
                        value._expressions.Add(valueright);
                        value.mathop = tkCur.text[0];
                        return value;
                    }
                    else if(tkCur.text == ">" || tkCur.text == ">=" || tkCur.text == "<" || tkCur.text == "<=" || tkCur.text == "==" || tkCur.text == "!=") {
                        CQ_Expression_Math2ValueLogic value = new CQ_Expression_Math2ValueLogic(left, rightend, tlist[left].line, tlist[rightend].line);
                        value._expressions.Add(valueleft);
                        value._expressions.Add(valueright);
                        LogicToken token = LogicToken.not_equal;
                        if(tkCur.text == ">") {
                            token = LogicToken.greater;
                        }
                        else if(tkCur.text == ">=") {
                            token = LogicToken.greater_equal;
                        }
                        else if(tkCur.text == "<") {
                            token = LogicToken.less;
                        }
                        else if(tkCur.text == "<=") {
                            token = LogicToken.less_equal;
                        }
                        else if(tkCur.text == "==") {
                            token = LogicToken.equal;
                        }
                        else if(tkCur.text == "!=") {
                            token = LogicToken.not_equal;
                        }
                        value.mathop = token;
                        return value;
                    }
                    else {
                        char mathop = tkCur.text[0];
                        if(mathop == '?') {
                            CQ_Expression_Math3Value value = new CQ_Expression_Math3Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value._expressions.Add(valueleft);

                            CQ_Expression_Math2Value vvright = valueright as CQ_Expression_Math2Value;
                            if(vvright.mathop != ':')
                                throw new Exception("三元表达式异常" + tkCur.ToString() + tkCur.SourcePos());
                            value._expressions.Add(vvright._expressions[0]);
                            value._expressions.Add(vvright._expressions[1]);
                            return value;
                        }
                        else {
                            CQ_Expression_Math2Value value = new CQ_Expression_Math2Value(left, rightend, tlist[left].line, tlist[rightend].line);
                            value._expressions.Add(valueleft);
                            value._expressions.Add(valueright);
                            value.mathop = mathop;
                            return value;
                        }

                    }
                }
                else {
                    LogError(tlist, "编译表达式失败", right, rightend);
                }
            }

            return null;
        }
        public static ICQ_Expression Compiler_Expression_MathSelf (IList<Token> tlist, int pos, int posend) {
            CQ_Expression_SelfOp value = new CQ_Expression_SelfOp(pos, posend, tlist[pos].line, tlist[posend].line);
            value.value_name = tlist[pos].text;
            value.mathop = tlist[pos + 1].text[0];

            return value;
        }
    }
}
