using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_TypeConvert : ICQ_Expression
    {
        public CQ_Expression_TypeConvert(int tbegin, int tend, int lbegin, int lend)
        {
            _expressions = new List<ICQ_Expression>();
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

            var right = _expressions[0].ComputeValue(content);
            IType type = CQuark.AppDomain.GetType(right.type);
            CQ_Value value = new CQ_Value();
            value.type = targettype;
            value.value = type.ConvertTo( right.value, targettype);

            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);

            return value;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public CQ_Type type
        {
            get { return null; }
        }
        public CQ_Type targettype;

        public override string ToString()
        {
            return "convert<" + targettype.Name + ">";
        }
    }
}