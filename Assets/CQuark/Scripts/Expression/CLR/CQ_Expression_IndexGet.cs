using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_IndexGet : ICQ_Expression {
        public CQ_Expression_IndexGet (int tbegin, int tend, int lbegin, int lend) {
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
            set;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            set;
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
			CQ_Value parent = _expressions[0].ComputeValue(content);
			object obj = parent.GetValue();
            if(parent == CQ_Value.Null) {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
            var key = _expressions[1].ComputeValue(content);

            CQ_Value value = CQ_Value.Null;
			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.IndexGet(parent.m_type, obj, key, out value)) {
                IType type = CQuark.AppDomain.GetITypeByCQValue(parent);
				value = type._class.IndexGet(content, obj, key.GetValue());
			}
           
#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("IndexFind[]不支持套用协程");
        }

        public override string ToString () {
            return "IndexFind[]|";
        }
    }
}