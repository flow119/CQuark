using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Type_Bool : IType
    {
        public string keyword
        {
            get { return "bool"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public TypeBridge typeBridge
        {
            get { return (typeof(bool)); }
        }
        public object defaultValue
        {
            get { return false; }
        }
        public ICQ_Function function
        {
            get { throw new NotImplementedException(); }
        }

        public ICQ_Expression_Value MakeValue(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
            throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Value right, out TypeBridge returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            throw new NotImplementedException();
        }
    }
}
