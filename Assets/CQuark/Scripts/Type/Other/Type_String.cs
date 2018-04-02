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
        public CQ_Type typeBridge
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

        public object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            returntype = typeof(string);
            if (code == '+')
            {
                if (right.value == null)
                {
                    return (string)left + "null";
                }
                else
                {
                    return (string)left + right.value.ToString();
                }
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
