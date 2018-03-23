using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_StaticFind : ICQ_Expression {
        public CQ_Expression_StaticFind (int tbegin, int tend, int lbegin, int lend) {
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
            get {
                return null;
            }
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
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif

            CQ_Value value = null;
#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
            //这几行是为了快速获取Unity的静态变量，而不需要反射
            value = UnityWrap.StaticValueGet(type, staticmembername);
            if(value != null)
                return value;
#endif

            value = type._class.StaticValueGet(content, staticmembername);
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return value;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, ICoroutine coroutine) {
            throw new Exception("A.member不支持套用协程");
        }


        public IType type;
        public string staticmembername;

        public override string ToString () {
            return "StaticFind|" + type.keyword + "." + staticmembername;
        }
    }
}