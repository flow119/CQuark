using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_Value : ICQ_Expression {

        public CQ_Value value;
        public CQ_Expression_Value (CQ_Value val) {
            value = val;
        }

        public override string ToString () {
            if(value.m_type != null)
                return value.m_type.Name + "|" + value.value.ToString();
            else if(value.m_stype != null)
                return value.m_stype.Name + "|" + value.value.ToString();
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
            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("Value不支持套用协程");
        }
    }
}
