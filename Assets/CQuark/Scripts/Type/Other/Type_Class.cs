using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Type_Class : IType
    {
        public string keyword
        {
            get;
            private set;
        }
        public string _namespace
        {
            get;
            private set;
        }
        public TypeBridge typeBridge
        {
            get;
            private set;
        }
        public object defaultValue
        {
            get { return null; }
        }
		public IClass _class
        {
            get
            {
                return (Class_CQuark)typeBridge as IClass;
            }
        }


        IList<IType> types;
        public bool compiled
        {
            get;
            set;
        }
        public Type_Class(string keyword, bool bInterface, string filename)
        {
            this.keyword = keyword;
            this._namespace = "";
            typeBridge = new Class_CQuark(keyword, "", filename, bInterface);
            compiled = false;
        }
        public void SetBaseType(IList<IType> types)
        {
            this.types = types;
        }
        public void EmbDebugToken(IList<Token> tokens)
        {
            ((Class_CQuark)typeBridge).EmbDebugToken(tokens);
        }
     
        public object ConvertTo(object src, TypeBridge targetType)
        {
			var type = CQuark.AppDomain.GetITypeByCQType(targetType);
            if (this.typeBridge == type||(Type)targetType==typeof(object)) 
				return src;
            if (this.types.Contains(type))
            {
                return src;
            }

            throw new NotImplementedException();
        }
        public CQ_Value Math2Value(char code, CQ_Value left, CQ_Value right)
        {
            throw new NotImplementedException();
        }
        public bool MathLogic (LogicToken code, CQ_Value left, CQ_Value right)
        {
            if (code == LogicToken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if(left.GetValue() == null || right.TypeIsEmpty)
                {
                    return left.GetValue() == right.GetValue();
                }
                else
                {
                    return left.GetValue() == right.GetValue();
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if(left.GetValue() == null || right.TypeIsEmpty)
                {
                    return left.GetValue() != right.GetValue();
                }
                else
                {
                    return left.GetValue() != right.GetValue();
                }
            }
            throw new NotImplementedException();
        }
    }
}
