using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_MemberFunction : ICQ_Expression {
        public CQ_Expression_MemberFunction (int tbegin, int tend, int lbegin, int lend) {
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
                if(_expressions == null || _expressions.Count == 0)
                    return false;
                foreach(ICQ_Expression expr in _expressions) {
                    if(expr.hasCoroutine)
                        return true;
                }
                return false;
            }
        }
        MethodCache cache = null;

        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            CQ_Value parent = _expressions[0].ComputeValue(content);

#if CQUARK_DEBUG
            if(parent == CQ_Value.Null) {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
#endif


            FixedList<CQ_Value> _params = new FixedList<CQ_Value>(_expressions.Count);
            for(int i = 1; i < _expressions.Count; i++) {
                _params.Add(_expressions[i].ComputeValue(content));
            }

            CQ_Value value = CQ_Value.Null;

            //这几行是为了快速获取Unity的静态变量，而不需要反射
            if(!Wrap.MemberCall(parent.m_type, parent.value, functionName, _params, out value)) {
                var iclass = CQuark.AppDomain.GetITypeByCQValue(parent)._class;
                if(cache == null || cache.cachefail) {
                    cache = new MethodCache();
                    value = iclass.MemberCall(content, parent.value, functionName, _params, cache);
                }
                else {
                    value = iclass.MemberCallCache(content, parent.value, _params, cache);
                }
            }


#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("暂时不支持套用协程");
        }

        public string functionName;

        public override string ToString () {
            return "MemberCall|a." + functionName;
        }
    }
}