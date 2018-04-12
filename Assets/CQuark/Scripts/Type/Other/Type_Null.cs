using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_NULL : IType
    {
        public string keyword
        {
            get { return "null"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQ_Type cqType
        {
            get { return null; }
        }
        public object defaultValue
        {
            get { return null; }
        }
		public IClass _class
        {
            get { throw new NotImplementedException(); }
        }

        public object ConvertTo(object src, CQ_Type targetType)
        {
            return null;
        }
        public CQ_Value Math2Value (char code, object left, CQ_Value right) {
            if ((Type)right.cq_type == typeof(string))
            {
                CQ_Value returnValue = new CQ_Value();
                returnValue.cq_type = typeof(String);
                returnValue.value = "null" + right.value;
                return returnValue;
            }
            throw new NotImplementedException();
        }
        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            if (code == LogicToken.equal)
            {
                return null == right.value;
            }
            else if(code== LogicToken.not_equal)
            {
                return null != right.value;
            }
            throw new NotImplementedException();
        }
    }
}
