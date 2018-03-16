using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_Math2ValueLogic : ICQ_Expression
    {
        public CQ_Expression_Math2ValueLogic(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
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
            CQ_Value result = new CQ_Value();


            //if(mathop=="<"||mathop=="<="||mathop==">"||mathop==">="||mathop=="=="||mathop=="!=")
            {
                result.type = typeof(bool);
                var left = listParam[0].ComputeValue(content);
                var right = listParam[1].ComputeValue(content);
                if(left.type==null||right.type==null)
                {
                    if (mathop == LogicToken.equal)
                    {
                        result.value = left.value == right.value;
                    }
                    if(mathop== LogicToken.not_equal)
                    {
                        result.value = left.value != right.value;
                    }
                }
                else if ((Type)left.type == typeof(bool) && (Type)right.type == typeof(bool))
                {
                    if (mathop == LogicToken.equal)
                    {
                        result.value = (bool)left.value == (bool)right.value;
                        //return result;
                    }
                    else if (mathop == LogicToken.not_equal)
                    {
                        result.value = (bool)left.value != (bool)right.value;
                        //return result;
                    }
                    else
                    {
                        throw new Exception("bool 不支持此运算符");
                    }
                }
                else
                {

                    result.value = CQuark.AppDomain.GetType(left.type).MathLogic(mathop, left.value, right);

                }
            }
            content.OutStack(this);

            return result;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public LogicToken mathop;

        public override string ToString()
        {
            return "Math2ValueLogic|a" + mathop + "b";
        }
    }
}