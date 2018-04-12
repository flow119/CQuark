using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_Math2ValueLogic : ICQ_Expression {
        public CQ_Expression_Math2ValueLogic (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            private set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions {
            get;
            private set;
        }
        public int tokenBegin {
            get;
            private set;
        }
        public int tokenEnd {
            get;
            private set;
        }
        public bool hasCoroutine {
            get {
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            CQ_Value result = new CQ_Value();


            //if(mathop=="<"||mathop=="<="||mathop==">"||mathop==">="||mathop=="=="||mathop=="!=")
            {
                result.m_type = typeof(bool);
                var left = _expressions[0].ComputeValue(content);
                var right = _expressions[1].ComputeValue(content);
                if(left.TypeIsEmpty|| right.TypeIsEmpty) {
                    if(mathop == LogicToken.equal) {
                        result.value = left.value == right.value;
                    }
                    if(mathop == LogicToken.not_equal) {
                        result.value = left.value != right.value;
                    }
                }
                else if(left.m_type == typeof(bool) && right.m_type == typeof(bool)) {
                    if(mathop == LogicToken.equal) {
                        result.value = (bool)left.value == (bool)right.value;
                        //return result;
                    }
                    else if(mathop == LogicToken.not_equal) {
                        result.value = (bool)left.value != (bool)right.value;
                        //return result;
                    }
                    else {
                        throw new Exception("bool 不支持此运算符");
                    }
                }
                else {

                    result.value = CQuark.AppDomain.GetITypeByCQValue(left).MathLogic(mathop, left.value, right);

                }
            }
#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            return result;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("数学逻辑符不支持套用协程");
        }

        public LogicToken mathop;

        public override string ToString () {
            return "Math2ValueLogic|a" + mathop + "b";
        }
    }
}