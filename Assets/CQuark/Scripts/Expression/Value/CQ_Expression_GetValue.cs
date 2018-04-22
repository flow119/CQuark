using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_GetValue : ICQ_Expression {
        public CQ_Expression_GetValue (int tbegin, int tend, int lbegin, int lend) {
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
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
			CQ_Value value = content.Get(value_name);
#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            //从上下文取值

            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            yield return ComputeValue(content);
        }

        public string value_name;

        public override string ToString () {
            return "GetValue|" + value_name;
        }
    }
}