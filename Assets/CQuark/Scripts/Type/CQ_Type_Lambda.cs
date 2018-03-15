using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_Lambda: ICQ_Type
    {
        public CQ_Type_Lambda()
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
        public CQType type
        {
            get { return typeof(DeleLambda); }
        }

        public ICQ_Value MakeValue(object value)
        {
            throw new NotSupportedException();

        }

        public object ConvertTo(object src, CQType targetType)
        {
            RegHelper_Type dele = CQuark.AppDomain.GetType(targetType) as RegHelper_Type;
            return dele.CreateDelegate(src as DeleLambda);
            //throw new NotImplementedException();
        }

        public object Math2Value(char code, object left, CQ_Content.Value right, out CQType returntype)
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
        public object DefValue
        {
            get { return null; }
        }
    }
}
