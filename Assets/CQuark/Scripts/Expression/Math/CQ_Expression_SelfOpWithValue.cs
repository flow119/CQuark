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
            listParam = new List<ICQ_Expression>();
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
        public List<ICQ_Expression> listParam
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
				if(listParam == null || listParam.Count == 0)
					return false;
				foreach(ICQ_Expression expr in listParam){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);


            var left = listParam[0].ComputeValue(content);
            var right = listParam[1].ComputeValue(content);
            ICQ_Type type = CQuark.AppDomain.GetType(left.type);
            //if (mathop == "+=")

            {
                CQType returntype;
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
                    if (listParam[0] is CQ_Expression_MemberFind)
                    {
                        CQ_Expression_MemberFind f = listParam[0] as CQ_Expression_MemberFind;

                        var parent = f.listParam[0].ComputeValue(content);
                        if (parent == null)
                        {
                            throw new Exception("调用空对象的方法:" + f.listParam[0].ToString() + ":" + ToString());
                        }
                        var ptype = CQuark.AppDomain.GetType(parent.type);
                        ptype.function.MemberValueSet(content, parent.value, f.membername, value);
                    }
                    if (listParam[0] is CQ_Expression_StaticFind)
                    {
                        CQ_Expression_StaticFind f = listParam[0] as CQ_Expression_StaticFind;
                        f.type.function.StaticValueSet(content, f.staticmembername, value);
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