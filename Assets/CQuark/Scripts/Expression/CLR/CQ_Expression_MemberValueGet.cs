using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace CQuark {

    public class CQ_Expression_MemberValueGet : ICQ_Expression {
        public CQ_Expression_MemberValueGet (int tbegin, int tend, int lbegin, int lend) {
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
                //if(_expressions == null || _expressions.Count == 0)
                //    return false;
                //foreach(ICQ_Expression expr in _expressions){
                //    if(expr.hasCoroutine)
                //        return true;
                //}
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
			CQ_Value parent = _expressions[0].ComputeValue(content);
            if(parent == null) {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
            
			CQ_Value value = null;

			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.MemberValueGet(parent.cq_type.type, parent.value, membername, out value)){
				IClass iclass = CQuark.AppDomain.GetITypeByCQType(parent.cq_type)._class;
				CQClassInstance s = parent.value as CQClassInstance;
				if(s != null) {
					iclass = s.type;
				}

				value = iclass.MemberValueGet(content, parent.value, membername);
			}

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;

        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            //可能以后需要支持。。。
            throw new Exception("a.Method暂时不支持协程");
        }


        public string membername;

        public override string ToString () {
            return "MemberFind|a." + membername;
        }
    }
}