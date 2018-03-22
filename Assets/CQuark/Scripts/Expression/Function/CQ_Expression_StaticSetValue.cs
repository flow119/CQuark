using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_StaticSetValue : ICQ_Expression {
        public CQ_Expression_StaticSetValue (int tbegin, int tend, int lbegin, int lend) {
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
            //var parent = _expressions[0].ComputeValue(content);
            var value = _expressions[0].ComputeValue(content);

            type._class.StaticValueSet(content, staticmembername, value.value);
            //做数学计算
            //从上下文取值
            //_value = null;
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, ICoroutine coroutine) {
            throw new Exception("StaticSet不支持套用协程");
        }
        public IType type;
        public string staticmembername;

        public override string ToString () {
            return "StaticSetvalue|" + type.keyword + "." + staticmembername + "=";
        }
    }
}