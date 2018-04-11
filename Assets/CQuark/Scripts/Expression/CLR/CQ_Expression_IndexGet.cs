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
            var parent = _expressions[0].ComputeValue(content);
            if(parent == null) {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
            var key = _expressions[1].ComputeValue(content);

			CQ_Value value = null;
			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.IndexGet(parent.cq_type.type, parent.value, key, out value)){
				IType type = CQuark.AppDomain.GetITypeByCQType(parent.cq_type);
				value = type._class.IndexGet(content, parent.value, key.value);
			}
           
#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            //IndexGet返回的值类型是 System.Object.
            //在这里需要将类型明确化.
            //value.type = value.value.GetType();

            return value;
            //return type.function.MemberValueGet(CQuark.AppDomain, parent.value, membername);
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("IndexFind[]不支持套用协程");
        }

        public override string ToString () {
            return "IndexFind[]|";
        }
    }
}