using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_Math2ValueAndOr : ICQ_Expression {
        public CQ_Expression_Math2ValueAndOr (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
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

            //if (mathop == "&&" || mathop == "||")
            {
                bool bleft = false;
                bool bright = false;

                bleft = (bool)_expressions[0].ComputeValue(content).GetValue();

                result.m_type = typeof(bool);
                if(mathop == '&') {
                    if(!bleft) {
                        result.SetValue(false);
                    }
                    else {
                        bright = (bool)_expressions[1].ComputeValue(content).GetValue();
                        result.SetValue((bool)(bleft && bright));
                    }
                }
                else if(mathop == '|') {
                    if(bleft) {
                        result.SetValue(true);
                    }
                    else {
                        bright = (bool)_expressions[1].ComputeValue(content).GetValue();
                        result.SetValue((bool)(bleft || bright));
                    }

                }

            }
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return result;

        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("暂时不支持套用协程");
        }


        public char mathop;

        public override string ToString () {
            return "Math2ValueAndOr|a" + mathop + "b";
        }
    }
}