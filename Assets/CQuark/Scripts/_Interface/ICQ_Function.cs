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

        CQ_Content.Value Call(CQ_Content content, IList<CQ_Content.Value> param);
    }

    public interface ICQ_Function_Member
    {
        string keyword
        {
            get;
        }
        CQ_Content.Value Call(CQ_Content content, object objthis, IList<CQ_Content.Value> param);
    }
}
