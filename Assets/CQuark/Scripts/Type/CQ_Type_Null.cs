using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_NULL : ICQ_Type
    {
        public string keyword
        {
            get { return "null"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQType type
        {
            get { return null; }
        }

        public ICQ_Value MakeValue(object value)
        {
            CQ_Value_Null v = new CQ_Value_Null();
       
            return v;

        }

        public object ConvertTo(CQ_Content env, object src, CQType targetType)
        {
            return null;
        }

        public object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
           
            if ((Type)right.type == typeof(string))
            {
                returntype = typeof(String);
                return "null" + right.value;
            }
            throw new NotImplementedException();
        }

        public bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                return null == right.value;
            }
            else if(code== logictoken.not_equal)
            {
                return null != right.value;
            }
            throw new NotImplementedException();
        }



        public ICQ_TypeFunction function
        {
            get { throw new NotImplementedException(); }
        }
        public object DefValue
        {
            get { return null; }
        }
    }
}
