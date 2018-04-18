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
		public IClass _class
        {
            get { throw new NotImplementedException(); }
        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
            throw new NotImplementedException();
        }

        public CQ_Value Math2Value (char code, CQ_Value left, CQ_Value right) {
            throw new NotImplementedException();
        }

        public bool MathLogic(LogicToken code, CQ_Value left, CQ_Value right)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
