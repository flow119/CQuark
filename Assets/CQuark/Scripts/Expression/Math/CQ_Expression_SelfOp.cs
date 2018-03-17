using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_SelfOp : ICQ_Expression
    {
        public CQ_Expression_SelfOp(int tbegin, int tend, int lbegin, int lend)
        {
            tokenBegin = tbegin;
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
            get
            {
                return null;
            }
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
				return false;
			}
		}
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);

            var v = content.Get(value_name);
            IType type = CQuark.AppDomain.GetType(v.type);
            CQ_Type returntype;
            object value = type.Math2Value(mathop, v.value, CQ_Value.One, out returntype);
            value = type.ConvertTo( value, v.type);
            content.Set(value_name, value);

            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);

            return content.Get(value_name);
        }

		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

  
        public string value_name;
        public char mathop;
  
        public override string ToString()
        {
            return "MathSelfOp|" + value_name + mathop;
        }
    }
}