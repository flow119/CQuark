using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_MemberValueSet : ICQ_Expression {
        public CQ_Expression_MemberValueSet (int tbegin, int tend, int lbegin, int lend) {
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
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            var parent = _expressions[0].ComputeValue(content);
            if(parent == null) {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
            var value = _expressions[1].ComputeValue(content);

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.MemberValueSet(parent.type.type, parent.value, membername, value)){
				var iclass = CQuark.AppDomain.GetITypeByCQType(parent.type)._class;
	            
	            CQClassInstance s = parent.value as CQClassInstance;
	            if(s != null) {
	                iclass = s.type;
	            }
				iclass.MemberValueSet(content, parent.value, membername, value.value);
			}
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("a.b = 不支持协程");
        }

        public string membername;

        public override string ToString () {
            return "MemberSetvalue|a." + membername;
        }
    }
}