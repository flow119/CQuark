using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_NegativeLogic : ICQ_Expression {
        public CQ_Expression_NegativeLogic (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            tokenBegin = tbegin;
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

            CQ_Value r = _expressions[0].ComputeValue(content);


            CQ_Value r2 = new CQ_Value();
            r2.type = r.type;
            r2.breakBlock = r.breakBlock;
            r2.value = !(bool)r.value;
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return r2;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, ICoroutine coroutine) {
            throw new Exception("! 不支持套用协程");
        }


        public override string ToString () {
            return "(!)|";
        }
    }
}