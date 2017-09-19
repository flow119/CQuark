using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_String : ICQ_Type
    {
        public CQ_Type_String()
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
        public CQType type
        {
            get { return typeof(string); }
        }

        public ICQ_Value MakeValue(object value)
        {
            CQ_Value_Value<string> v = new CQ_Value_Value<string>();
            v.value_value = (string)value;
            
            return v;

        }

        public object ConvertTo(CQ_Content env, object src, CQType targetType)
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

        public object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
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

        public bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right)
        {
            if (code == logictoken.equal)
            {
                return (string)left == (string)right.value;
            }
            else if(code== logictoken.not_equal)
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
        public object DefValue
        {
            get { return null; }
        }
    }
}
