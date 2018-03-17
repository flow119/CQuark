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
        public TypeBridge typeBridge
        {
            get { return typeof(DeleLambda); }
        }
        public IClass function
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

        public object ConvertTo(object src, TypeBridge targetType)
        {
            Type_Operatorable dele = CQuark.AppDomain.GetType(targetType) as Type_Operatorable;
            return dele.CreateDelegate(src as DeleLambda);
            //throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Value right, out TypeBridge returntype)
        {

            throw new NotImplementedException("code:"+code +" right:+"+right.type.ToString()+"="+ right.value);
        }

        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {

            throw new NotImplementedException();
        }
    }
}
