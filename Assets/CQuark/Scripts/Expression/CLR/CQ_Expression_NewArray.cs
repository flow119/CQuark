using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_NewArray : ICQ_Expression {
        public CQ_Expression_NewArray (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        //0 count;
        //1~where,first value;
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
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
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
            List<object> list = new List<object>();
            int count = _expressions[0] == null ? (_expressions.Count - 1) : (int)_expressions[0].ComputeValue(content).value;
            if(count == 0)
                throw new Exception("不能创建0长度数组");
            CQ_Value vcount = new CQ_Value();
            vcount.cq_type = typeof(int);
            vcount.value = count;
            for(int i = 1; i < _expressions.Count; i++) {
                //if (_expressions[i] != null)
                {
                    list.Add(_expressions[i].ComputeValue(content).value);
                }
            }
			List<CQ_Value> param = new List<CQ_Value>();
			param.Add(vcount);
			CQ_Value outvalue = null;


			//这几行是为了快速获取Unity的静态变量，而不需要反射
			if(!Wrap.New(type.cqType.type, param, out outvalue)){
				outvalue = type._class.New(content, param);
			}

            for(int i = 0; i < list.Count; i++) {
                type._class.IndexSet(content, outvalue.value, i, list[i]);
            }

#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return outvalue;

        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("new params[]不支持协程");
        }
        public CQuark.IType type;

        public override string ToString () {
            return "new|" + type.keyword + "(params[" + _expressions.Count + ")";
        }
    }
}