using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_Value_Object : ICQ_Expression_Value {
        public CQ_Expression_Value_Object (Type type) {
            this.type = type;
            this.value_value = null;
        }

        public CQ_Type type {
            get;
            private set;
        }

        public object value_value;
        public object value {
            get {
                return value_value;
            }
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
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            CQ_Value v = new CQ_Value();

            v.type = this.type;
            v.value = this.value_value;
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
            return v;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("不支持套用协程");
        }
    }
}
