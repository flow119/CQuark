using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CQuark;

namespace CQuark {
    public class CQ_Expression_Value_Null : ICQ_Expression_Value {
        public CQ_Type type {
            get { return null; }
        }

        public string Dump () {
            return "<unknown> null";
        }

        public object value {
            get {
                return null;
            }
        }
        public override string ToString () {
            return "<unknown> null";
        }


        public List<ICQ_Expression> _expressions {
            get { return null; }
        }
        public int tokenBegin {
            get;
            set;
        }
        public int tokenEnd {
            get;
            set;
        }
        public int lineBegin {
            get;
            set;
        }
        public int lineEnd {
            get;
            set;
        }
        public bool hasCoroutine {
            get {
                //				if(_expressions == null || _expressions.Count == 0)
                //					return false;
                //				foreach(ICQ_Expression expr in _expressions){
                //					if(expr.hasCoroutine)
                //						return true;
                //				}
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            CQ_Value v = new CQ_Value();
            v.SetCQType(this.type);
            v.value = null;
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
            return v;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("null不支持套用协程");
        }
    }
}
