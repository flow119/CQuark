using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class Type_Lambda: IType
    {
        public Type_Lambda()
        {

            function = null;
        }
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

        public IValue MakeValue(object value)
        {
            throw new NotSupportedException();

        }

        public object ConvertTo(object src, TypeBridge targetType)
        {
            RegHelper_Type dele = CQuark.AppDomain.GetType(targetType) as RegHelper_Type;
            return dele.CreateDelegate(src as DeleLambda);
            //throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Content.Value right, out TypeBridge returntype)
        {

            throw new NotImplementedException("code:"+code +" right:+"+right.type.ToString()+"="+ right.value);
        }

        public bool MathLogic(LogicToken code, object left, CQ_Content.Value right)
        {

            throw new NotImplementedException();
        }



        public ICQ_TypeFunction function
        {
            get;
            private set;
        }
        public object defaultValue
        {
            get { return null; }
        }
    }
}
