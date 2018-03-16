using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Type_Var : IType
    {
        public class var
        {

        }
        public string keyword
        {
            get { return "var"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public TypeBridge typeBridge
        {
            get { return (typeof(var)); }
        }

        public IValue MakeValue(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
            throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
        {
            throw new NotImplementedException();
        }

        public ICQ_TypeFunction function
        {
            get { throw new NotImplementedException(); }
        }
        public object defaultValue
        {
            get { return null; }
        }
    }
}
