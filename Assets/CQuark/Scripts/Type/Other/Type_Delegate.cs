using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Delegate: IType
    {
        public string keyword
        {
            get { return "(){}"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQ_Type cqType
        {
            get { return typeof(DeleFunction); }
        }
        public object defaultValue
        {
            get { return null; }
        }
		public IClass _class
        {
            get { return null; }
        }

        public object ConvertTo(object src, CQ_Type targetType)
        {
			Type_Action dele = CQuark.AppDomain.GetITypeByCQType(targetType) as Type_Action;
            return dele.CreateDelegate(src as DeleFunction);
            //throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {

            throw new NotImplementedException("code:"+code +" right:+"+right.type.ToString()+"="+ right.value);
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {

            throw new NotImplementedException();
        }
    }
}
