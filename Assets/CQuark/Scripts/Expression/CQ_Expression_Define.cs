using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{
    public class CQ_Expression_Define : ICQ_Expression
    {
        public CQ_Expression_Define(int tbegin, int tend, int lbegin, int lend)
        {
            //listParam = new List<ICQ_Value>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        List<ICQ_Expression> _listParam = null;
        public List<ICQ_Expression> listParam
        {
            get
            {
                if (_listParam == null)
                {
                    _listParam = new List<ICQ_Expression>();
                }
                return _listParam;
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
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);

            if (_listParam != null && _listParam.Count > 0)
            {

                CQ_Value v = _listParam[0].ComputeValue(content);
                object val = v.value;
                if ((Type)value_type == typeof(Type_Var.var))
                {
                    if(v.type!=null)
                        value_type = v.type;
                    
                }
                else if (v.type != value_type)
                {
					val = CQuark.AppDomain.GetType(v.type).ConvertTo(v.value, value_type);
                   
                }

                content.DefineAndSet(value_name, value_type, val);
            }
            else
            {
                content.Define(value_name, value_type);
            }
            //设置环境变量为
            content.OutStack(this);

            return null;
		}
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			
			if (_listParam != null && _listParam.Count > 0)
			{
				if(_listParam[0].hasCoroutine){
					yield return coroutine.StartNewCoroutine(CoroutineCompute(content, coroutine));
				}else{
					CQ_Value v = _listParam[0].ComputeValue(content);
					object val = v.value;
					if ((Type)value_type == typeof(Type_Var.var))
					{
						if(v.type!=null)
							value_type = v.type;
						
					}
					else if (v.type != value_type)
					{
						val = CQuark.AppDomain.GetType(v.type).ConvertTo(v.value, value_type);
						
					}
					
					content.DefineAndSet(value_name, value_type, val);
				}
			}
			else
			{
				content.Define(value_name, value_type);
			}
			//设置环境变量为
			content.OutStack(this);
		}


        public string value_name;
        public TypeBridge value_type;
        public override string ToString()
        {
            string outs = "Define|" + value_type.Name + " " + value_name;
            if (_listParam != null)
            {
                if (_listParam.Count > 0)
                {
                    outs += "=";
                }
            }
            return outs;
        }
    }
}