using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_SelfOpWithValue : ICQ_Expression
    {
        public CQ_Expression_SelfOpWithValue(int tbegin, int tend, int lbegin, int lend)
        {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            private set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            private set;
        }
		public bool hasCoroutine{
			get{
				if(_expressions == null || _expressions.Count == 0)
					return false;
				foreach(ICQ_Expression expr in _expressions){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);


            var left = _expressions[0].ComputeValue(content);
            var right = _expressions[1].ComputeValue(content);
            IType type = CQuark.AppDomain.GetType(left.type);
            //if (mathop == "+=")

            {
                CQ_Type returntype;
                object value = type.Math2Value(mathop, left.value, right, out returntype);
                value = type.ConvertTo( value, left.type);
                left.value = value;

//                Type t = right.type;
                //if(t.IsSubclassOf(typeof(MulticastDelegate))||t.IsSubclassOf(typeof(Delegate)))
                //{

                //}
                ////content.Set(value_name, value);
                //else if (t == typeof(CQuark.DeleLambda) || t == typeof(CQuark.DeleFunction) || t == typeof(CQuark.DeleEvent))
                //{

                //}
                //else
                {
                    if (_expressions[0] is CQ_Expression_MemberFind)
                    {
                        CQ_Expression_MemberFind f = _expressions[0] as CQ_Expression_MemberFind;

                        var parent = f._expressions[0].ComputeValue(content);
                        if (parent == null)
                        {
                            throw new Exception("调用空对象的方法:" + f._expressions[0].ToString() + ":" + ToString());
                        }
                        var ptype = CQuark.AppDomain.GetType(parent.type);
						ptype._class.MemberValueSet(content, parent.value, f.membername, value);
                    }
                    if (_expressions[0] is CQ_Expression_StaticFind)
                    {
                        CQ_Expression_StaticFind f = _expressions[0] as CQ_Expression_StaticFind;
						f.type._class.StaticValueSet(content, f.staticmembername, value);
                    }
                }
            }


            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);

            return null;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        //public string value_name;
        public char mathop;

        public override string ToString()
        {
            return "MathSelfOp|" + mathop;
        }
    }
}