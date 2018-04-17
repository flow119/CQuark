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
		public IClass _class
        {
            get { throw new NotImplementedException(); }
        }
        public object defaultValue
        {
            get { return null; }
        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
            throw new NotImplementedException();
        }

        public CQ_Value Math2Value (char code, object left, CQ_Value right) {
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            throw new NotImplementedException();
        }
    }
}
