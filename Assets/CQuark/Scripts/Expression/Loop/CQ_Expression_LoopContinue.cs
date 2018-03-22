using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_LoopContinue : ICQ_Expression {
        public CQ_Expression_LoopContinue (int tbegin, int tend, int lbegin, int lend) {
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
            get {
                return null;
            }
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
            CQ_Value rv = new CQ_Value();
            rv.breakBlock = 1;
            //跳出逻辑
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return rv;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, ICoroutine coroutine) {
            throw new Exception("continue不支持套用协程");
        }

        public override string ToString () {
            return "continue;";
        }
    }
}