using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_Construction : ICQ_Expression {
        public CQ_Expression_Construction (int tbegin, int tend, int lbegin, int lend) {
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
                if(_expressions == null || _expressions.Count == 0)
                    return false;
                foreach(ICQ_Expression expr in _expressions) {
                    if(expr.hasCoroutine)
                        return true;
                }
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            CQ_Value[] parameters = new CQ_Value[_expressions.Count];
            for(int i = 0; i < _expressions.Count; i++) {
                parameters[i] = _expressions[i].ComputeValue(content);
            }

            CQ_Value value = CQ_Value.Null;

            //这几行是为了快速获取Unity的静态变量，而不需要反射
            if(!Wrap.New(type.typeBridge.type, parameters, out value)) {
                value = type._class.New(content, parameters);
			}

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;

        }
		public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("new function不支持套用协程");
        }
        public CQuark.IType type;

        public override string ToString () {
            return "new|" + type.keyword + "(function[" + _expressions.Count + ")";
        }
    }
}