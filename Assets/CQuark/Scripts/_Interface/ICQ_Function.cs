using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public interface ICQ_Function
    {
        string keyword
        {
            get;
        }
		Type returntype {
			get;
		}

        CQ_Content.Value Call(CQ_Content content, IList<CQ_Content.Value> param);
    }
}
