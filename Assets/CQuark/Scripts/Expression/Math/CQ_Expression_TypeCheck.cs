using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_TypeCheck : ICQ_Expression
    {
        public CQ_Expression_TypeCheck(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
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
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);

            var right = listParam[0].ComputeValue(content);
            IType type = CQuark.AppDomain.GetType(right.type);
            CQ_Value value = new CQ_Value();
            value.type = typeof(bool);
            value.value = type.ConvertTo( right.value, targettype) != null;

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

        public TypeBridge type
        {
            get { return null; }
        }
        public TypeBridge targettype;

        public override string ToString()
        {
            return "convert<" + targettype.Name + ">";
        }
    }
}