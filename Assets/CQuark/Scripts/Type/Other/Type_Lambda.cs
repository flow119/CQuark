using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Lambda: IType
    {
        public string keyword
        {
            get { return "()=>"; }
        }
        public string _namespace
        {
            get { return ""; }
        }
        public CQ_Type cqType
        {
            get { return typeof(DeleLambda); }
        }
		public IClass _class
        {
            get { return null; }
        }
        public object defaultValue
        {
            get { return null; }
        }

        public object ConvertTo(object src, CQ_Type targetType)
        {
			Type_Action dele = CQuark.AppDomain.GetITypeByCQType(targetType) as Type_Action;
            return dele.CreateDelegate(src as DeleLambda);
            //throw new NotImplementedException();
        }

        public CQ_Value Math2Value (char code, object left, CQ_Value right) {
            throw new NotImplementedException("code:"+code +" right:+"+"="+ right.m_value);
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {

            throw new NotImplementedException();
        }
    }
}
