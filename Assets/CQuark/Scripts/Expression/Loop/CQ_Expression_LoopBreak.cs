using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_LoopBreak : ICQ_Expression
    {
        public CQ_Expression_LoopBreak(int tbegin, int tend, int lbegin, int lend)
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
        public List<ICQ_Expression> listParam
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
//				if(listParam == null || listParam.Count == 0)
//					return false;
//				foreach(ICQ_Expression expr in listParam){
//					if(expr.hasCoroutine)
//						return true;
//				}
				return false;
			}
		}
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Value rv = new CQ_Value();
            rv.breakBlock = 2;
            //跳出逻辑
            content.OutStack(this);
            return rv;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public override string ToString()
        {
            return "break;";
        }
    }
}