using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    //改为DeleEvent
    public class DeleEvent //指向系统中的事件委托
    {
        public DeleEvent(object source, System.Reflection.EventInfo _event)
        {
            this.source = source;
            this._event = _event;
        }
        //public DeleEvent(Delegate instance, CQuark.CQ_Content content)
        //{
        //    deleInstance = instance;
        //    deleContent = content;
        //}
        //public object Call(CQuark.CQ_Content parentContent, IList<CQuark.CQ_Content.Value> _params)
        //{
        //    object[] plist = new object[_params.Count];
        //    for (int i = 0; i < _params.Count; i++)
        //    {
        //        plist[i] = _params[i].value;
        //    }
        //    if (deleInstance != null)
        //    {
        //        if (parentContent != null)
        //            parentContent.InStack(deleContent);
        //        object returnv = deleInstance.Method.Invoke(deleInstance.Target, plist);
        //        if (parentContent != null)
        //            parentContent.OutStack(deleContent);
        //        return returnv;
        //    }
        //    else
        //    {
        //        throw new Exception("事件不能調用");
        //    }
        //    return null;
        //}
        //如果是事件有这两个参数
        public object source;
        public System.Reflection.EventInfo _event;
    }
}
