using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class RegHelper_Type : ICQ_Type
    {
        public static RegHelper_Type MakeType(Type type, string keyword)
        {
            if (!type.IsSubclassOf(typeof(Delegate)))
            {
                return new RegHelper_Type(type, keyword, false);
            }
            var method = type.GetMethod("Invoke");
            var pp = method.GetParameters();
            if (method.ReturnType == typeof(void))
            {
                if (pp.Length == 0)
                {
                    return new RegHelper_DeleAction(type, keyword);
                }
                else if (pp.Length == 1)
                {
                    var gtype = typeof(RegHelper_DeleAction<>).MakeGenericType(new Type[] { pp[0].ParameterType });
                    return gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type;
                }
                else if (pp.Length == 2)
                {
                    var gtype = typeof(RegHelper_DeleAction<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);
                }
                else if (pp.Length == 3)
                {
                    var gtype = typeof(RegHelper_DeleAction<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);
                }
                else
                {
                    throw new Exception("还没有支持这么多参数的委托");
                }
            }
            else
            {
                Type gtype = null;
                if (pp.Length == 0)
                {
                    gtype = typeof(RegHelper_DeleNonVoidAction<>).MakeGenericType(new Type[] { method.ReturnType });
                }
                else if (pp.Length == 1)
                {
                    gtype = typeof(RegHelper_DeleNonVoidAction<,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType });
                }
                else if (pp.Length == 2)
                {
                    gtype = typeof(RegHelper_DeleNonVoidAction<,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType });
                }
                else if (pp.Length == 3)
                {
                    gtype = typeof(RegHelper_DeleNonVoidAction<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                }
                else
                {
                    throw new Exception("还没有支持这么多参数的委托");
                }
                return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);
            }
        }
        
        protected RegHelper_Type(Type type, string setkeyword, bool dele)
        {
            function = new RegHelper_TypeFunction(type);
            if (setkeyword != null)
            {
                keyword = setkeyword.Replace(" ", "");
            }
            else
            {
                keyword = type.Name;
            }
            this.type = type;
            this._type = type;
        }
        public string keyword
        {
            get;
            protected set;
        }
        public string _namespace
        {
            get { return type.NameSpace; }
        }
        public CQType type
        {
            get;
            protected set;
        }
        public Type _type;

        public virtual ICQ_Value MakeValue(object value) //这个方法可能存在AOT陷阱
        {
            //这个方法可能存在AOT陷阱
            //Type target = typeof(CQ_Value_Value<>).MakeGenericType(new Type[] { type }); 
            //return target.GetConstructor(new Type[] { }).Invoke(new object[0]) as ICQ_Value;

            CQ_Value_Object rvalue = new CQ_Value_Object(type);
            rvalue.value_value = value;
            return rvalue;
        }

        public virtual object ConvertTo(CQ_Content content, object src, CQType targetType)
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

		public virtual object Math2Value(CQ_Content content, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            returntype = type;
            System.Reflection.MethodInfo call = null;
            //var m = ((Type)type).GetMembers();
            if (code == '+')
            {
                if ((Type)right.type == typeof(string))
                {
                    returntype = typeof(string);
                    return left.ToString() + right.value as string;
                }
                call = _type.GetMethod("op_Addition", new Type[] { this.type, right.type });
            }
            else if (code == '-')//base = {CQcriptExt.Vector3 op_Subtraction(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Subtraction", new Type[] { this.type, right.type });
            else if (code == '*')//[2] = {CQcriptExt.Vector3 op_Multiply(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Multiply", new Type[] { this.type, right.type });
            else if (code == '/')//[3] = {CQcriptExt.Vector3 op_Division(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Division", new Type[] { this.type, right.type });
            else if (code == '%')//[4] = {CQcriptExt.Vector3 op_Modulus(CQcriptExt.Vector3, CQcriptExt.Vector3)}
                call = _type.GetMethod("op_Modulus", new Type[] { this.type, right.type });

            var obj = call.Invoke(null, new object[] { left, right.value });
            //function.StaticCall(env,"op_Addtion",new List<ICL>{})
            return obj;
        }

		public virtual bool MathLogic(CQ_Content content, LogicToken code, object left, CQ_Content.Value right)
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

        public ICQ_TypeFunction function
        {
            get;
            protected set;
        }

        public virtual object DefValue
        {
            get { return null; }
        }
    }
}
