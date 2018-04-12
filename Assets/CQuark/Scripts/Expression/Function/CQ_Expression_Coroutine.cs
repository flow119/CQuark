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
                    //if(p.hasCoroutine) {
                    //    yield return p.CoroutineCompute(content, coroutine);
                    //}
                    //else {
                        list.Add(p.ComputeValue(content));
                    //}
                }
            }

            CQ_Value v = null;
            Class_CQuark.Function retFunc = null;
            bool bFind = false;
            if(content.CallType != null)
                bFind = content.CallType.functions.TryGetValue(funcname, out retFunc);

            if(bFind) {
                if(retFunc.bStatic) {
                    v = content.CallType.StaticCall(content, funcname, list);
                }
                else {
                    v = content.CallType.MemberCall(content, content.CallThis, funcname, list);
                }
            }
            else {
                v = content.GetQuiet(funcname);
                if(v != null && v.value is Delegate) {
                    //if(v.value is Delegate)
                    {
                        Delegate d = v.value as Delegate;
                        v = new CQ_Value();
                        object[] obja = new object[list.Count];
                        for(int i = 0; i < list.Count; i++) {
                            obja[i] = list[i].value;
                        }
                        v.value = d.DynamicInvoke(obja);
                        if(v.value == null) {
                            v.SetCQType(null);
                        }
                        else {
                            v.m_type = v.value.GetType();
                        }
                    }
                    //else
                    //{
                    //    throw new Exception(funcname + "不是函数");
                    //}
                }
                else {
                    throw new Exception(funcname + "没有这样的方法");
                    //v = CQuark.AppDomain.GetMethod(funcname).Call(content, list);
                }
            }

            yield return coroutine.StartCoroutine(v.value as IEnumerator);
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