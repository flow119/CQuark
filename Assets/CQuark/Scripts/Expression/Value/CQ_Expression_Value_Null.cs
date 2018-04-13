using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CQuark;

namespace CQuark {
    public class CQ_Expression_Value_Null : ICQ_Expression_Value {

        public CQ_Value cq_value = CQ_Value.Null;

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
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
            return cq_value;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("null不支持套用协程");
        }
    }
}
