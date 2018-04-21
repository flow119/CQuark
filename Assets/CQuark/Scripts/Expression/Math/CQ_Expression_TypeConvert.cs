using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_TypeConvert : ICQ_Expression {
        public CQ_Expression_TypeConvert (int tbegin, int tend, int lbegin, int lend) {
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
            var right = _expressions[0].ComputeValue(content);
            IType type = CQuark.AppDomain.GetITypeByCQValue(right);
            CQ_Value value = new CQ_Value();

            if(targettype.type != null) {
                value.SetObject(targettype.type, type.ConvertTo(right.GetObject(), targettype));
            }
            else if(targettype.stype != null) {
                value.SetObject(targettype.stype, type.ConvertTo(right.GetObject(), targettype));
            }


#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("as 不支持套用协程");
        }

        public TypeBridge targettype;

        public override string ToString () {
            return "convert<" + targettype.Name + ">";
        }
    }
}