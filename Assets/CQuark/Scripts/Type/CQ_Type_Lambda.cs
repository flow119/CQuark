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

        public object ConvertTo(CQ_Content env, object src, CQType targetType)
        {
            ICQ_Type_Dele dele =  env.environment.GetType(targetType) as ICQ_Type_Dele;
            return dele.CreateDelegate(env.environment, src as DeleLambda);
            //throw new NotImplementedException();
        }

        public object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {

            throw new NotImplementedException("code:"+code +" right:+"+right.type.ToString()+"="+ right.value);
        }

        public bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right)
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
