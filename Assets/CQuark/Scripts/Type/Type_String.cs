using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_String : IType
    {
        public Type_String()
        {
            function = new RegHelper_TypeFunction(typeof(string));
        }
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

        public IValue MakeValue(object value)
        {
            CQ_Value_Value<string> v = new CQ_Value_Value<string>();
            v.value_value = (string)value;
            
            return v;

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

        public object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype)
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

        public bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
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


        public ICQ_TypeFunction function
        {
            get;
            private set;
        }
        public object defaultValue
        {
            get { return null; }
        }
    }
}
