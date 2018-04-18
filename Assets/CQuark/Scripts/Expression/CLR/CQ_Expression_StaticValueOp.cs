using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_StaticValueOp : ICQ_Expression {
        public CQ_Expression_StaticValueOp (int tbegin, int tend, int lbegin, int lend) {
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

            CQ_Value getvalue = CQ_Value.Null;

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.StaticValueGet(type.typeBridge.type, staticmembername, out getvalue)){
				getvalue = type._class.StaticValueGet(content, staticmembername);
			}

            CQ_Value vright = CQ_Value.One;
            if(_expressions.Count > 0) {
                vright = _expressions[0].ComputeValue(content);
            }

            var mtype = CQuark.AppDomain.GetITypeByCQValue(getvalue);
            CQ_Value vout = mtype.Math2Value(mathop, getvalue.GetValue(), vright);

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.StaticValueSet(type.typeBridge.type, staticmembername, vout)){
                type._class.StaticValueSet(content, staticmembername, vout.GetValue());
			}
            

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return vout;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("StaticMath不支持套用协程");
        }


        public IType type;
        public string staticmembername;
        public char mathop;
        public override string ToString () {
            return "StaticMath|" + type.keyword + "." + staticmembername + " |" + mathop;
        }
    }
}