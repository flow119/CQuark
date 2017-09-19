using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_Lambda: ICQ_Expression
    {
        public CQ_Expression_Lambda(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> listParam
        {
            get;
            private set;
        }
        public List<ICQ_Expression> lambdaParams = new List<ICQ_Expression>();
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
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            //List<CQ_Content.Value> list = new List<CQ_Content.Value>();
            CQ_Content.Value value = new CQ_Content.Value();
            value.type = typeof(DeleLambda);
            value.value = new DeleLambda(content,(this.listParam[0] as CQ_Expression_Block).listParam,this.listParam[1]);
            //创建一个匿名方法
            content.OutStack(this);
            return value;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

  
        public override string ToString()
        {
            return "()=>{}|";
        }
    }
}