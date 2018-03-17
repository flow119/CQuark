using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace CQuark
{

    public class CQ_Expression_MemberFind : ICQ_Expression
    {
        public CQ_Expression_MemberFind(int tbegin, int tend, int lbegin, int lend)
        {
           _expressions= new List<ICQ_Expression>();
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
            var parent = _expressions[0].ComputeValue(content);
            if (parent == null)
            {
                throw new Exception("调用空对象的方法:" + _expressions[0].ToString() + ":" + ToString());
            }
            IClass typefunction = CQuark.AppDomain.GetType(parent.type).function;
            if(parent.type is object)
            {
                SInstance s =parent.value as SInstance;
                if(s!=null)
                {
                    typefunction = s.type;
                }
            }
            //var type = CQuark.AppDomain.GetType(parent.type);
            var value=typefunction.MemberValueGet(content, parent.value, membername);
            content.OutStack(this);
            return value;
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;

        }

		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

   
        public string membername;
   
        public override string ToString()
        {
            return "MemberFind|a." + membername;
        }
    }
}