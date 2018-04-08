using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_Coroutine : ICQ_Expression {
        public CQ_Expression_Coroutine (int tbegin, int tend, int lbegin, int lend) {
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
                return true;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif
            List<CQ_Value> list = new List<CQ_Value>();
            foreach(ICQ_Expression p in _expressions) {
                if(p != null) {
                    list.Add(p.ComputeValue(content));
                }
            }

#if CQUARK_DEBUG
				content.OutStack(this);
#endif
            //TODO 最好不要支持这种操作
            return null;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            List<CQ_Value> list = new List<CQ_Value>();
            foreach(ICQ_Expression p in _expressions) {
                if(p != null) {
                    //暂时不支持协程套用协程
                    list.Add(p.ComputeValue(content));
                }
            }

            IMethod func = CQuark.AppDomain.GetMethod(funcname);
            yield return coroutine.StartCoroutine(func.Call(content, list).value as IEnumerator);
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
        }
        public string funcname;

        public override string ToString () {
            return "Coroutine |" + funcname + "(params[" + _expressions.Count + ")";
        }
    }
}