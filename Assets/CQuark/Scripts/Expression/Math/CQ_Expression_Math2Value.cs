using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_Math2Value : ICQ_Expression {
        public CQ_Expression_Math2Value (int tbegin, int tend, int lbegin, int lend) {
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

            var left = _expressions[0].ComputeValue(content);
            var right = _expressions[1].ComputeValue(content);
            CQ_Value result = CQuark.AppDomain.GetITypeByCQValue(left).Math2Value(mathop, left.value, right);

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return result;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("a+b不支持套用协程");
        }

        public char mathop;


        public override string ToString () {
            return "Math2Value|a" + mathop + "b";
        }
    }
}