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
        public TypeBridge typeBridge
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

        public object ConvertTo(object src, TypeBridge targetType)
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

        public CQ_Value Math2Value (char code, CQ_Value left, CQ_Value right) {
            if (code == '+')
            {
                CQ_Value returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), left.ToString() + right.ToString());
                return returnValue;
            }
            throw new NotImplementedException();
        }

        public bool MathLogic (LogicToken code, CQ_Value left, CQ_Value right)
        {
            if (code == LogicToken.equal)
            {
				return left.ToString() == right.ToString();
            }
            else if(code== LogicToken.not_equal)
            {
				return left.ToString() != right.ToString();
            }
            throw new NotImplementedException();
        }
    }
}
