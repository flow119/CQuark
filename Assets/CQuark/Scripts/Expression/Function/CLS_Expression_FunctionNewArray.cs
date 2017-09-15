using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{

    public class CLS_Expression_FunctionNewArray : ICLS_Expression
    {
        public CLS_Expression_FunctionNewArray(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        //0 count;
        //1~where,first value;
        public List<ICLS_Expression> listParam
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
				foreach(ICLS_Expression expr in listParam){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            List<object> list = new List<object>();
            int count = listParam[0] == null ? (listParam.Count - 1) : (int)listParam[0].ComputeValue(content).value;
            if (count == 0)
                throw new Exception("不能创建0长度数组");
            CLS_Content.Value vcount = new CLS_Content.Value();
            vcount.type = typeof(int);
            vcount.value = count;
            for (int i = 1; i < listParam.Count; i++)
            {
                //if (listParam[i] != null)
                {
                    list.Add(listParam[i].ComputeValue(content).value);
                }
            }
            List<CLS_Content.Value> p = new List<CLS_Content.Value>();
            p.Add(vcount);
            var outvalue = type.function.New(content, p);
            for (int i = 0; i < list.Count; i++)
            {
                type.function.IndexSet(content, outvalue.value, i, list[i]);
            }
            content.OutStack(this);
            return outvalue;

        }
		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
        public CSLE.ICLS_Type type;

        public override string ToString()
        {
            return "new|" + type.keyword + "(params[" + listParam.Count + ")";
        }
    }
}