using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_SelfOpWithValue : ICQ_Expression {
        public CQ_Expression_SelfOpWithValue (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
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
                //if(_expressions == null || _expressions.Count == 0)
                //    return false;
                //foreach(ICQ_Expression expr in _expressions) {
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

            CQ_Value left = _expressions[0].ComputeValue(content);
            CQ_Value right = _expressions[1].ComputeValue(content);
            IType type = CQuark.AppDomain.GetITypeByCQValue(left);

            CQ_Value val = type.Math2Value(mathop, left, right);
            //val.SetValue(left.typeBridge, type.ConvertTo(val.GetValue(), left.typeBridge));
            left.UsingValue(val);
            
            if(_expressions[0] is CQ_Expression_MemberValueGet) {
                CQ_Expression_MemberValueGet f = _expressions[0] as CQ_Expression_MemberValueGet;

                var parent = f._expressions[0].ComputeValue(content);
                if(parent == CQ_Value.Null) {
                    throw new Exception("调用空对象的方法:" + f._expressions[0].ToString() + ":" + ToString());
                }

				//这几行是为了快速获取Unity的静态变量，而不需要反射
                if(!Wrap.MemberValueSet(parent.m_type, parent.GetValue(), f.membername, val)) {
                    var ptype = CQuark.AppDomain.GetITypeByCQValue(parent);
                    ptype._class.MemberValueSet(content, parent.GetValue(), f.membername, val);
				}
            }
            else if(_expressions[0] is CQ_Expression_StaticValueGet) {
                CQ_Expression_StaticValueGet f = _expressions[0] as CQ_Expression_StaticValueGet;

				//这几行是为了快速获取Unity的静态变量，而不需要反射
				if(!Wrap.StaticValueSet(type.typeBridge.type, f.staticmembername, val)){
                    f.type._class.StaticValueSet(content, f.staticmembername, val);
				}
            }
            else if(_expressions[0] is CQ_Expression_GetValue) {
                CQ_Expression_GetValue f = _expressions[0] as CQ_Expression_GetValue;
                content.Set(f.value_name, left);
			}else if(_expressions[0] is CQ_Expression_IndexGet){
				CQ_Expression_IndexGet f = _expressions[0] as CQ_Expression_IndexGet;
				CQ_Value parent = f._expressions[0].ComputeValue(content);
				object obj = parent.GetValue();
				CQ_Value key = f._expressions[1].ComputeValue(content);

				IType parenttype = CQuark.AppDomain.GetITypeByCQValue(parent);
				parenttype._class.IndexSet(content, parent.GetValue(), key.GetValue(), left.GetValue());

				CQ_Expression_GetValue g = f._expressions[0] as CQ_Expression_GetValue;
				content.Set(g.value_name, parent);
			}

            
            
#if CQUARK_DEBUG
            content.OutStack(this);
#endif

            return CQ_Value.Null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("SelfOp=不支持套用协程");
        }

        //public string value_name;
        public char mathop;

        public override string ToString () {
            return "MathSelfOp|" + mathop;
        }
    }
}