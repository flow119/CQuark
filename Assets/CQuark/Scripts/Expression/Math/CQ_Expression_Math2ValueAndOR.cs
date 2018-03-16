using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_Math2ValueAndOr : ICQ_Expression
    {
        public CQ_Expression_Math2ValueAndOr(int tbegin, int tend, int lbegin, int lend)
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
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            CQ_Content.Value result = new CQ_Content.Value();

            //if (mathop == "&&" || mathop == "||")
            {
                bool bleft = false;
                bool bright = false;
                if (listParam[0] is IValue)
                {
                    bleft = (bool)((listParam[0] as IValue).value);
                }
                else
                {
                    bleft = (bool)listParam[0].ComputeValue(content).value;
                }
                result.type = typeof(bool);
                if(mathop=='&')
                {
                    if (!bleft)
                    {
                        result.value = false;
                    }
                    else
                    {
                        if (listParam[1] is IValue)
                        {
                            bright = (bool)((listParam[1] as IValue).value);
                        }
                        else
                        {
                            bright = (bool)listParam[1].ComputeValue(content).value;
                        }
                        result.value = (bool)(bleft && bright);
                    }
                }
                else if (mathop == '|')
                {
                    if (bleft)
                    {
                        result.value = true;
                    }
                    else
                    {
                        if (listParam[1] is IValue)
                        {
                            bright = (bool)((listParam[1] as IValue).value);
                        }
                        else
                        {
                            bright = (bool)listParam[1].ComputeValue(content).value;
                        }
                        result.value = (bool)(bleft || bright);
                    }

                }

            }
            content.OutStack(this);
            return result;

        }

		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}


        public char mathop;

        public override string ToString()
        {
            return "Math2ValueAndOr|a" + mathop + "b";
        }
    }
}