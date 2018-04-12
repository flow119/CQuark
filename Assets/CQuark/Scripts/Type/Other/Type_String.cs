using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_String : IType
    {
       
        public string keyword
        {
            get { return "string"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQ_Type cqType
        {
            get { return typeof(string); }
        }
        public IClass _class
        {
            get;
            private set;
        }
        public object defaultValue
        {
            get { return null; }
        }

        public Type_String()
        {
            _class = new Class_System(typeof(string));
        }

        public object ConvertTo(object src, CQ_Type targetType)
        {
            if ((Type)targetType == typeof(string)) return src;
            if ((Type)targetType == typeof(void))
            {
                return null;
            }
            if (((Type)targetType).IsAssignableFrom(typeof(string)))
            //if((Type)targetType== typeof(object))
            {
                return src;
            }
            return null;
        }

        public CQ_Value Math2Value (char code, object left, CQ_Value right) {
            if (code == '+')
            {
                CQ_Value returnValue = new CQ_Value();
                returnValue.cq_type = typeof(string);
                if (right.value == null)
                {
                    returnValue.value = (string)left + "null";
                }
                else
                {
                    returnValue.value = (string)left + right.value.ToString();
                }
                return returnValue;
            }
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            if (code == LogicToken.equal)
            {
                return (string)left == (string)right.value;
            }
            else if(code== LogicToken.not_equal)
            {
                return (string)left != (string)right.value;
            }
            throw new NotImplementedException();
        }
    }
}
