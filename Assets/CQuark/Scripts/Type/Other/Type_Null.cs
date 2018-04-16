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
            if (right.m_type == typeof(string))
            {
                CQ_Value returnValue = new CQ_Value();
                returnValue.m_type = typeof(String);
                returnValue.m_value = "null" + right.m_value;
                return returnValue;
            }
            throw new NotImplementedException();
        }
        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            if (code == LogicToken.equal)
            {
                return null == right.m_value;
            }
            else if(code== LogicToken.not_equal)
            {
                return null != right.m_value;
            }
            throw new NotImplementedException();
        }
    }
}
