using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class CQ_Type_Bool : ICQ_Type
    {
        public class var
        {

        }
        public string keyword
        {
            get { return "bool"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQType type
        {
            get { return (typeof(bool)); }
        }

        public ICQ_Value MakeValue(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(CQ_Content env, object src, CQType targetType)
        {
            throw new NotImplementedException();
        }

        public object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(CQ_Content env, LogicToken code, object left, CQ_Content.Value right)
        {
            throw new NotImplementedException();
        }

        public ICQ_TypeFunction function
        {
            get { throw new NotImplementedException(); }
        }
        public object DefValue
        {
            get { return false; }
        }
    }
}
