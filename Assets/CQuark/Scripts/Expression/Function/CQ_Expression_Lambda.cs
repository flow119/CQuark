using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_Lambda : ICQ_Expression {
        public CQ_Expression_Lambda (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions {
            get;
            private set;
        }
        public List<ICQ_Expression> lambdaParams = new List<ICQ_Expression>();
        public int tokenBegin {
            get;
            private set;
        }
        public int tokenEnd {
            get;
            private set;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
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
            //List<CQ_Content.Value> list = new List<CQ_Content.Value>();
            CQ_Value value = new CQ_Value();
            value.type = typeof(DeleLambda);
            value.value = new DeleLambda(content, (this._expressions[0] as CQ_Expression_Block)._expressions, this._expressions[1]);

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("Lambda表达式内不支持套用协程");
        }

        public override string ToString () {
            return "()=>{}|";
        }
    }
}