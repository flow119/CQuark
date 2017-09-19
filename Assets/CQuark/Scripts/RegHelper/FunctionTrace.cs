using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class FunctionTrace : ICQ_Function
    {
        public string keyword
        {
            get { return "trace"; }
        }

        public CQ_Content.Value Call(CQ_Content content, IList<CQ_Content.Value> param)
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
            content.environment.logger.Log(output);
            return CQ_Content.Value.Void;
        }
        
    }

}
