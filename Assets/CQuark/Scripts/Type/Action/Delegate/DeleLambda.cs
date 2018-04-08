using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	public class DeleLambda// : IDeleBase //指向Lambda表达式
	{
		public DeleLambda(CQ_Content content, IList<ICQ_Expression> param, ICQ_Expression func)
		{
			this.content = content.Clone();
			this.expr_func = func;
			foreach (var p in param)
			{
				CQ_Expression_GetValue v1 = p as CQ_Expression_GetValue;
				CQ_Expression_Define v2 = p as CQ_Expression_Define;
				if (v1 != null)
				{
					paramTypes.Add(null);
					paramNames.Add(v1.value_name);
				}
				else if (v2 != null)
				{
					paramTypes.Add(v2.value_type);
					paramNames.Add(v2.value_name);
				}
				else
				{
					throw new Exception("DeleLambda 参数不正确");
				}
			}
		}

		public string GetKey()
		{
			string key = "";
			foreach (string item in paramNames)
			{
				key += item + "_";
			}
			key += expr_func.tokenBegin + "_" + expr_func.tokenEnd + "_" + expr_func.lineBegin + "_" + expr_func.lineEnd;

			return key;
		}

		public List<Type> paramTypes = new List<Type>();
		public List<string> paramNames = new List<string>();
		public CQ_Content content;
		public ICQ_Expression expr_func;
	}
}
