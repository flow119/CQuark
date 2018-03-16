using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class FunctionTrace : IFunction
    {
        public string keyword
        {
            get { return "trace"; }
        }

		public Type returntype
		{
			get {return typeof(void);}
		}

        public CQ_Value Call(CQ_Content content, IList<CQ_Value> param)
        {
            string output = "trace:";
            bool bfirst = true;
            foreach (var p in param)
            {
                if (bfirst) bfirst = false;
                else output += ",";
                if (p.value == null) output += "null";
                else output += p.value.ToString();
            }
            DebugUtil.Log(output);
            return CQ_Value.Void;
        }
        
    }

}
