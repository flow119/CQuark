using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_LoopReturn : ICQ_Expression
    {
        public CQ_Expression_LoopReturn(int tbegin, int tend, int lbegin, int lend)
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
            CQ_Value rv = new CQ_Value();
            rv.breakBlock = 10;
            if (listParam.Count > 0&&listParam[0]!=null)
            {
                var v = listParam[0].ComputeValue(content);
                {
                    rv.type = v.type;
                    rv.value = v.value;
                }
            }
            else
            {
                rv.type = typeof(void);
            }
            content.OutStack(this);
            return rv;

            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public override string ToString()
        {
            return "return|";
        }
    }
}