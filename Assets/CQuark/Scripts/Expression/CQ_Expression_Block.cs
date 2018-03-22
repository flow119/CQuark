using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_Block : ICQ_Expression {
        public CQ_Expression_Block (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
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
            content.DepthAdd();
            CQ_Value value = null;
            foreach(ICQ_Expression expr in _expressions) {
                if(expr != null)
                    value = expr.ComputeValue(content);

                if(value != null && value.breakBlock != 0)
                    break;
            }

            content.DepthRemove();
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, ICoroutine coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            content.DepthAdd();
            CQ_Value value = null;
            foreach(ICQ_Expression expr in _expressions) {
                if(expr != null) {
                    if(expr.hasCoroutine) {
                        yield return coroutine.StartNewCoroutine(expr.CoroutineCompute(content, coroutine));
                    }
                    else {
                        value = expr.ComputeValue(content);
                        if(value != null && value.breakBlock != 0)
                            break;
                    }
                }
            }
            content.DepthRemove();
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
            yield break;
        }

        public override string ToString () {
            return "Block|";
        }
    }
}