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
        public CQ_Type typeBridge
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


        public ICQ_Expression_Value MakeValue(object value)
        {
            throw new NotSupportedException();

        }

        public object ConvertTo(object src, CQ_Type targetType)
        {
            Type_Operatable dele = CQuark.AppDomain.GetType(targetType) as Type_Operatable;
            return dele.CreateDelegate(src as DeleLambda);
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
