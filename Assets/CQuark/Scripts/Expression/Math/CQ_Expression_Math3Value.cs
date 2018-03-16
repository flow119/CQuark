using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_Math3Value : ICQ_Expression
    {
        public CQ_Expression_Math3Value(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
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
            CQ_Value result = null;


            {
                result = new CQ_Value();
                var value = listParam[0].ComputeValue(content);
                if ((Type)value.type != typeof(bool))
                {
                    throw new Exception("三元表达式要求条件为bool型");
                }
                bool bv = (bool)value.value;
                if (bv)
                    result = listParam[1].ComputeValue(content);
                else
                    result = listParam[2].ComputeValue(content);
            }
            content.OutStack(this);
            return result;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public override string ToString()
        {
            return "Math3Value|a?b:c";
        }
    }
}