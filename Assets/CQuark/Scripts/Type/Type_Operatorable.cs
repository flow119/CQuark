using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class Type_Operatorable : IType
    {
        public string keyword
        {
            get;
            protected set;
        }
        public string _namespace
        {
            get { return typeBridge.NameSpace; }
        }
        public TypeBridge typeBridge
        {
            get;
            protected set;
        }
        public virtual object defaultValue
        {
            get { return null; }
        }
        public IClass function
        {
            get;
            protected set;
        }

        public Type _type;

        public Type_Operatorable(Type type, string setkeyword, bool dele)
        {
            function = new Class_System(type);
            if (setkeyword != null)
            {
                keyword = setkeyword.Replace(" ", "");
            }
            else
            {
                keyword = type.Name;
            }
            this.typeBridge = type;
            this._type = type;
        }


        public virtual ICQ_Expression_Value MakeValue(object value) //这个方法可能存在AOT陷阱
        {
            //这个方法可能存在AOT陷阱
            //Type target = typeof(CQ_Value_Value<>).MakeGenericType(new Type[] { type }); 
            //return target.GetConstructor(new Type[] { }).Invoke(new object[0]) as ICQ_Value;

            CQ_Expression_Value_Object rvalue = new CQ_Expression_Value_Object(typeBridge);
            rvalue.value_value = value;
            return rvalue;
        }
        public virtual object ConvertTo(object src, TypeBridge targetType)
        {
            Type targettype = (Type)targetType;
            if (this._type == targettype) return src;

            //type.get

            if (_type.IsEnum)
            {
                if ((Type)targetType == typeof(int))
                    return System.Convert.ToInt32(src);
                else if ((Type)targetType == typeof(uint))
                    return System.Convert.ToUInt32(src);
                else if ((Type)targetType == typeof(short))
                    return System.Convert.ToInt16(src);
                else if ((Type)targetType == typeof(ushort))
                    return System.Convert.ToUInt16(src);
                else
                {
                    return System.Convert.ToInt32(src);
                }
            }
            else if (targettype != null && targettype.IsEnum)
            {
                return Enum.ToObject(targettype, src);

            }
            var ms = _type.GetMethods();
            foreach (var m in ms)
            {
                if ((m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == targettype)
                {
                    return m.Invoke(null, new object[] { src });
                }
            }
            if (targettype != null)
            {
                if (targettype.IsAssignableFrom(_type))
                    return src;
                if (src != null && targettype.IsInstanceOfType(src))
                    return src;
            }
            else
            {
                return src;
            }

            return null;
        }
		public virtual object Math2Value(char code, object left, CQ_Value right, out TypeBridge returntype)
        {
            returntype = typeBridge;
            System.Reflection.MethodInfo call = null;
            //var m = ((Type)type).GetMembers();
            if (code == '+')
            {
                if ((Type)right.type == typeof(string))
                {
                    returntype = typeof(string);
                    return left.ToString() + right.value as string;
                }
                call = _type.GetMethod("op_Addition", new Type[] { this.typeBridge, right.type });
            }
            else if (code == '-')//base = {CQcriptExt.Vector3 op_Subtraction(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Subtraction", new Type[] { this.typeBridge, right.type });
            else if (code == '*')//[2] = {CQcriptExt.Vector3 op_Multiply(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Multiply", new Type[] { this.typeBridge, right.type });
            else if (code == '/')//[3] = {CQcriptExt.Vector3 op_Division(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Division", new Type[] { this.typeBridge, right.type });
            else if (code == '%')//[4] = {CQcriptExt.Vector3 op_Modulus(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Modulus", new Type[] { this.typeBridge, right.type });

            var obj = call.Invoke(null, new object[] { left, right.value });
            //function.StaticCall(env,"op_Addtion",new List<ICL>{})
            return obj;
        }
		public virtual bool MathLogic(LogicToken code, object left, CQ_Value right)
        {
            System.Reflection.MethodInfo call = null;

            //var m = _type.GetMethods();
            if (code == LogicToken.greater)//[2] = {Boolean op_GreaterThan(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThan");
            else if (code == LogicToken.less)//[4] = {Boolean op_LessThan(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_LessThan");
            else if (code == LogicToken.greater_equal)//[3] = {Boolean op_GreaterThanOrEqual(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThanOrEqual");
            else if (code == LogicToken.less_equal)//[5] = {Boolean op_LessThanOrEqual(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_LessThanOrEqual");
            else if (code == LogicToken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left == right.value;
                }

                call = _type.GetMethod("op_Equality");
                if (call == null)
                {
                    return left.Equals(right.value);
                }
            }
            else if (code == LogicToken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left != right.value;
                }
                call = _type.GetMethod("op_Inequality");
                if (call == null)
                {
                    return !left.Equals(right.value);
                }
            }
            var obj = call.Invoke(null, new object[] { left, right.value });
            return (bool)obj;
        }

        
        public virtual Delegate CreateDelegate(DeleFunction lambda)
        {
            throw new Exception("");
        }
        public virtual Delegate CreateDelegate(DeleLambda lambda)
        {
            throw new Exception("");
        }
    }
}
