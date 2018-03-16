using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_FunctionNew: ICQ_Expression
    {
        public CQ_Expression_FunctionNew(int tbegin, int tend, int lbegin, int lend)
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
            List<CQ_Content.Value> list = new List<CQ_Content.Value>();
            foreach(ICQ_Expression p in listParam)
            {
                if(p!=null)
                {
                    list.Add(p.ComputeValue(content));
                }
            }
            var value= type.function.New(content,list);
            content.OutStack(this);
            return value;

        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
        public CQuark.IType type;
  
        public override string ToString()
        {
            return "new|" + type.keyword + "(params[" + listParam.Count + ")";
        }
    }
}