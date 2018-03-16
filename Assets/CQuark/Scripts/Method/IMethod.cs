using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public interface IMethod
    {
        string keyword
        {
            get;
        }
		Type returntype {
			get;
		}

        CQ_Value Call(CQ_Content content, IList<CQ_Value> param);
    }
}
