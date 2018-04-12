using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_IndexSet : ICQ_Expression {
        public CQ_Expression_IndexSet (int tbegin, int tend, int lbegin, int lend) {
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
            var key = _expressions[1].ComputeValue(content);
            var value = _expressions[2].ComputeValue(content);
            //object setv=value.value;
            //if(value.type!=parent.type)
            //{
            //    var vtype = CQuark.AppDomain.GetType(value.type);
            //    setv = vtype.ConvertTo(CQuark.AppDomain, setv, parent.type);
            //}

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.IndexSet(parent.m_type, parent.value, key, value)){
                var type = CQuark.AppDomain.GetITypeByCQValue(parent);
				type._class.IndexSet(content, parent.value, key.value, value.value);
			}

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("IndexSet[]不支持套用协程");
        }

        public override string ToString () {
            return "IndexSet[]=|";
        }
    }
}