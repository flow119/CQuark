using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_Block : ICQ_Expression
    {
        public CQ_Expression_Block(int tbegin,int tend,int lbegin,int lend)
        {
            listParam = new List<ICQ_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
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
            content.DepthAdd();
            CQ_Content.Value value = null;
			foreach (ICQ_Expression i in listParam)
			{
				ICQ_Expression e =i  as ICQ_Expression;
				if (e != null)
					value =e.ComputeValue(content);

				if (value!=null&&value.breakBlock != 0) break;
			} 
            content.DepthRemove();
            content.OutStack(this);
			return value;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			content.DepthAdd();
			CQ_Content.Value value = null;
			foreach (ICQ_Expression i in listParam)
			{
				ICQ_Expression e =i  as ICQ_Expression;
				if (e != null){
					if(e.hasCoroutine){
						yield return coroutine.StartNewCoroutine(e.CoroutineCompute(content, coroutine));
					}else{
						value =e.ComputeValue(content);
						if (value != null && value.breakBlock != 0)
							yield break;
					}
				}
			}
			content.DepthRemove();
			content.OutStack(this);
			yield break;
		}

  
        public override string ToString()
        {
            return "Block|";
        }
    }
}