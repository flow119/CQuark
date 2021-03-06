﻿using System;
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
//        public string _namespace
//        {
//            get;
//            private set;
//        }
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

        public Type_Class(string fullName, bool bInterface, string filename)
        {
			this.keyword = fullName;
//            this._namespace = "";
			typeBridge = new Class_CQuark(fullName, "", filename, bInterface);
        }
        public void SetBaseType(IList<IType> types)
        {
            this.types = types;
        }
#if CQUARK_DEBUG
        public void EmbDebugToken(IList<Token> tokens)
        {
            ((Class_CQuark)typeBridge).EmbDebugToken(tokens);
        }
#endif     
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
                if(left.GetObject() == null || right.TypeIsEmpty)
                {
                    return left.GetObject() == right.GetObject();
                }
                else
                {
                    return left.GetObject() == right.GetObject();
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if(left.GetObject() == null || right.TypeIsEmpty)
                {
                    return left.GetObject() != right.GetObject();
                }
                else
                {
                    return left.GetObject() != right.GetObject();
                }
            }
            throw new NotImplementedException();
        }
    }
}
