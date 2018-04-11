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
        public CQ_Type cqType
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
                return (Class_CQuark)cqType as IClass;
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
            cqType = new Class_CQuark(keyword, "", filename, bInterface);
            compiled = false;
        }
        public void SetBaseType(IList<IType> types)
        {
            this.types = types;
        }
        public void EmbDebugToken(IList<Token> tokens)
        {
            ((Class_CQuark)cqType).EmbDebugToken(tokens);
        }
     
        public object ConvertTo(object src, CQ_Type targetType)
        {
			var type = CQuark.AppDomain.GetITypeByCQType(targetType);
            if (this.cqType == type||(Type)targetType==typeof(object)) 
				return src;
            if (this.types.Contains(type))
            {
                return src;
            }

            throw new NotImplementedException();
        }
        public object Math2Value(char code, object left, CQ_Value right, out CQ_Type returntype)
        {
            throw new NotImplementedException();
        }
        public bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            if (code == LogicToken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.cq_type == null)
                {
                    return left == right.value;
                }
                else
                {
                    return left == right.value;
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.cq_type == null)
                {
                    return left != right.value;
                }
                else
                {
                    return left != right.value;
                }
            }
            throw new NotImplementedException();
        }
    }
}
