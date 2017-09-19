using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_StaticMath : ICQ_Expression
    {
        public CQ_Expression_StaticMath(int tbegin, int tend, int lbegin, int lend)
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
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);


            var getvalue=type.function.StaticValueGet(content, staticmembername);

            CQ_Content.Value vright = CQ_Content.Value.One;
            if (listParam.Count > 0)
            {
                vright = listParam[0].ComputeValue(content);
            }
            CQ_Content.Value vout = new CQ_Content.Value();
            var mtype = content.environment.GetType(getvalue.type);
            vout.value = mtype.Math2Value(content, mathop, getvalue.value, vright, out vout.type);

            type.function.StaticValueSet(content, staticmembername, vout.value);

            content.OutStack(this);
            return vout;
        }

		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

   
        public ICQ_Type type;
        public string staticmembername;
        public char mathop;
        public override string ToString()
        {
            return "StaticMath|" + type.keyword + "." + staticmembername +" |"+mathop;
        }
    }
}