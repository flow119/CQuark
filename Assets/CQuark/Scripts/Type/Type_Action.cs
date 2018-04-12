using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Type_Action : IType
    {
        public string keyword
        {
            get;
            protected set;
        }
        public string _namespace
        {
            get { return cqType.NameSpace; }
        }
        public CQ_Type cqType
        {
            get;
            protected set;
        }
        public virtual object defaultValue
        {
            get { return null; }
        }
        public IClass _class
        {
            get;
            protected set;
        }

        public Type _type;

        public Type_Action(Type type, string setkeyword, bool dele)
        {
            _class = new Class_System(type);
            if (setkeyword != null)
            {
                keyword = setkeyword.Replace(" ", "");
            }
            else
            {
                keyword = type.Name;
            }
            this.cqType = type;
            this._type = type;
        }

        public virtual object ConvertTo(object src, CQ_Type targetType)
        {
			throw new NotImplementedException();
        }
		public virtual CQ_Value Math2Value(char code, object left, CQ_Value right)
        {
			throw new NotImplementedException();
        }
		public virtual bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
			throw new NotImplementedException();
        }
			
        public virtual Delegate CreateDelegate(DeleFunction lambda)
        {
            throw new Exception("方法没有被重载");
        }
        public virtual Delegate CreateDelegate(DeleLambda lambda)
        {
			throw new Exception("方法没有被重载");
        }
    }
}
