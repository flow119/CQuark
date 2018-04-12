using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_StaticValueSet : ICQ_Expression {
        public CQ_Expression_StaticValueSet (int tbegin, int tend, int lbegin, int lend) {
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
            var value = _expressions[0].ComputeValue(content);

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.StaticValueSet(type.cqType.type, staticmembername, value)){
				type._class.StaticValueSet(content, staticmembername, value.value);
			}
            
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return CQ_Value.Null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("StaticSet不支持套用协程");
        }
        public IType type;
        public string staticmembername;

        public override string ToString () {
            return "StaticSetvalue|" + type.keyword + "." + staticmembername + "=";
        }
    }
}