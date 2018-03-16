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
        public TypeBridge typeBridge
        {
            get { return null; }
        }
        public object defaultValue
        {
            get { return null; }
        }
        public ICQ_Function function
        {
            get { throw new NotImplementedException(); }
        }
        

        public ICQ_Expression_Value MakeValue(object value)
        {
            CQ_Expression_Value_Null v = new CQ_Expression_Value_Null();
            return v;
        }
        public object ConvertTo(object src, TypeBridge targetType)
        {
            return null;
        }
        public object Math2Value(char code, object left, CQ_Value right, out TypeBridge returntype)
        {
           
            if ((Type)right.type == typeof(string))
            {
                returntype = typeof(String);
                return "null" + right.value;
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
