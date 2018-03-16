using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Method : IMethod
    {
        Delegate dele;
		List<object> defvalues = new List<object>();
		List<Type> paramtype = new List<Type>();

		public string keyword
		{
			get;
			private set;
		}
		public Type returntype {
			get;
			private set;
		}
        
        public CQ_Value Call(CQ_Content content, IList<CQ_Value> param)
        {
            CQ_Value v = new CQ_Value();
            List<object> objs = new List<object>();
           
            for (int i = 0; i < this.defvalues.Count; i++)
            {
                if (i >= param.Count)
                {
                    objs.Add(defvalues[i]);
                }
                else
                {
                    if (this.paramtype[i] == (Type)param[i].type)
                    {
                        objs.Add(param[i].value);
                    }
                    else
                    {
                        object conv = CQuark.AppDomain.GetType(param[i].type).ConvertTo( param[i].value, paramtype[i]);
                        objs.Add(conv);
                    }
                }
            }
            v.type = this.returntype;
            v.value = dele.DynamicInvoke(objs.ToArray());
            return v;
        }

		public Method(Delegate dele)
		{
			this.dele = dele;

			this.keyword = dele.Method.Name;

			returntype = dele.Method.ReturnType;
			foreach (System.Reflection.ParameterInfo p in dele.Method.GetParameters())
			{
				defvalues.Add(p.DefaultValue);
				paramtype.Add(p.ParameterType);
			}
		}
		public Method(Delegate dele,string setkeyword)
		{
			this.dele = dele;
			if (setkeyword != null)
			{
				this.keyword = setkeyword;
			}
			else
			{
				this.keyword = dele.Method.Name;
			}
			returntype = dele.Method.ReturnType;
			foreach (System.Reflection.ParameterInfo p in dele.Method.GetParameters())
			{
				defvalues.Add(p.DefaultValue);
				paramtype.Add(p.ParameterType);
			}
		}
    }
}
