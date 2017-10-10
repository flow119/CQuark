using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class SType : ICQ_TypeFunction
    {

        //{

        //}
        //public SType(string keyword, string _namespace = "")
        //{
        ////    this._Name = keyword;
        ////    this._Namespace = _namespace;
        ////    //this._GUID = Guid.Empty;

        //}
        public SType(string keyword, string _namespace , string filename , bool bInterface)
        {
            this.Name = keyword;
            this.Namespace = _namespace;
            this.filename = filename;
            this.bInterface = bInterface;
        }
        public string filename
        {
            get;
            private set;
        }
        public bool bInterface
        {
            get;
            private set;
        }
        public IList<Token> tokenlist
        {
            get;
            private set;
        }
        public void EmbDebugToken(IList<Token> tokens)
        {
            this.tokenlist = tokens;
        }
        #region impl type


        public string FullName
        {
            get { return Namespace + "." + Name; }
        }

        public string Namespace
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;

        }

        #endregion

        #region Script IMPL
        CQ_Content contentMemberCalc = null;
        public CQ_Content.Value New(CQ_Content content, IList<CQ_Content.Value> _params)
        {
            if (contentMemberCalc == null)
                contentMemberCalc = new CQ_Content(content.environment);
            NewStatic(content.environment);
            CQ_Value_ScriptValue sv = new CQ_Value_ScriptValue();
            sv.value_type = this;
            sv.value_value = new SInstance();
            sv.value_value.type = this;
            foreach (KeyValuePair<string,Member> i in this.members)
            {
                if (i.Value.bStatic == false)
                {
                    if (i.Value.expr_defvalue == null)
                    {
                        sv.value_value.member[i.Key] = new CQ_Content.Value();
                        sv.value_value.member[i.Key].type = i.Value.type.type;
                        sv.value_value.member[i.Key].value = i.Value.type.DefValue;
                    }
                    else
                    {
                        var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                        if (i.Value.type.type != value.type)
                        {
                            sv.value_value.member[i.Key] = new CQ_Content.Value();
                            sv.value_value.member[i.Key].type = i.Value.type.type;
                            sv.value_value.member[i.Key].value = content.environment.GetType(value.type).ConvertTo(content, value.value, i.Value.type.type);
                        }
                        else
                        {
                            sv.value_value.member[i.Key] = value;
                        }

                    }
                }
            }
            if (this.functions.ContainsKey(this.Name))//有同名函数就调用
            {
                MemberCall(content, sv.value_value, this.Name, _params);
            }
            return CQ_Content.Value.FromICQ_Value(sv);
        }
        void NewStatic(ICQ_Environment env)
        {
            if (contentMemberCalc == null)
                contentMemberCalc = new CQ_Content(env);
            if (this.staticMemberInstance == null)
            {
                staticMemberInstance = new Dictionary<string, CQ_Content.Value>();
                foreach (var i in this.members)
                {
                    if (i.Value.bStatic == true)
                    {
                        if (i.Value.expr_defvalue == null)
                        {
                            staticMemberInstance[i.Key] = new CQ_Content.Value();

                            staticMemberInstance[i.Key].type = i.Value.type.type;
                            staticMemberInstance[i.Key].value = i.Value.type.DefValue;
                        }
                        else
                        {
                            var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                            if (i.Value.type.type != value.type)
                            {
                                staticMemberInstance[i.Key] = new CQ_Content.Value();
                                staticMemberInstance[i.Key].type = i.Value.type.type;
                                staticMemberInstance[i.Key].value = env.GetType(value.type).ConvertTo(contentMemberCalc, value.value, i.Value.type.type);
                            }
                            else
                            {
                                staticMemberInstance[i.Key] = value;
                            }


                        }
                    }
                }
            }
        }
        public CQ_Content.Value StaticCall(CQ_Content contentParent, string function, IList<CQ_Content.Value> _params)
        {
            return StaticCall(contentParent, function, _params, null);
        }
        public CQ_Content.Value StaticCall(CQ_Content contentParent, string function, IList<CQ_Content.Value> _params, MethodCache cache)
        {
            if(cache!=null)
            {
                cache.cachefail = true;
            }
            NewStatic(contentParent.environment);
            if (this.functions.ContainsKey(function))
            {
                if (this.functions[function].bStatic == true)
                {
                    CQ_Content content = new CQ_Content(contentParent.environment, true);

                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
                    content.CallType = this;
                    content.CallThis = null;
                    content.function = function;
                    // int i = 0;
                    for (int i = 0; i < functions[function]._paramtypes.Count; i++)
                    //foreach (var p in this.functions[function]._params)
                    {
                        content.DefineAndSet(functions[function]._paramnames[i], functions[function]._paramtypes[i].type, _params[i].value);
                        //i++;
                    }
                    //var value = this.functions[function].expr_runtime.ComputeValue(content);
                    CQ_Content.Value value = null;
                    if (this.functions[function].expr_runtime != null)
                    {
                        value = this.functions[function].expr_runtime.ComputeValue(content);
                        if(value!=null)
                            value.breakBlock = 0;
                    }
                    else
                    {

                    }
                    contentParent.OutStack(content);

                    return value;
                }
            }
            else if (this.members.ContainsKey(function))
            {
                if (this.members[function].bStatic == true)
                {
                    Delegate dele = this.staticMemberInstance[function].value as Delegate;
                    if (dele != null)
                    {
                        CQ_Content.Value value = new CQ_Content.Value();
                        value.type = null;
                        object[] objs = new object[_params.Count];
                        for (int i = 0; i < _params.Count; i++)
                        {
                            objs[i] = _params[i].value;
                        }
                        value.value = dele.DynamicInvoke(objs);
                        if (value.value != null)
                            value.type = value.value.GetType();
                        value.breakBlock = 0;
                        return value;
                    }
                }

            }
            throw new NotImplementedException();
        }

        public CQ_Content.Value StaticValueGet(CQ_Content content, string valuename)
        {
            NewStatic(content.environment);

            if (this.staticMemberInstance.ContainsKey(valuename))
            {
                CQ_Content.Value v = new CQ_Content.Value();
                v.type = this.staticMemberInstance[valuename].type;
                v.value = this.staticMemberInstance[valuename].value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool StaticValueSet(CQ_Content content, string valuename, object value)
        {
            NewStatic(content.environment);
            if (this.staticMemberInstance.ContainsKey(valuename))
            {
                if (value != null && value.GetType() != (Type)this.members[valuename].type.type)
                {
                    if (value is SInstance)
                    {
                        if ((value as SInstance).type != (SType)this.members[valuename].type.type)
                        {
                            value = content.environment.GetType((value as SInstance).type).ConvertTo(content, value, this.members[valuename].type.type);
                        }
                    }
                    else if (value is DeleEvent)
                    {

                    }
                    else
                    {
                        value = content.environment.GetType(value.GetType()).ConvertTo(content, value, this.members[valuename].type.type);
                    }
                }
                this.staticMemberInstance[valuename].value = value;
                return true;
            }
            throw new NotImplementedException();
        }
        public CQ_Content.Value MemberCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Content.Value> _params)
        {
            return MemberCall(contentParent, object_this, func, _params, null);
        }
        public CQ_Content.Value MemberCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Content.Value> _params,MethodCache cache)
        {
            if (cache != null)
            {
                cache.cachefail = true;
            }
            if (this.functions.ContainsKey(func))
            {
                if (this.functions[func].bStatic == false)
                {
                    CQ_Content content = new CQ_Content(contentParent.environment, true);

                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
                    content.CallType = this;
                    content.CallThis = object_this as SInstance;
                    content.function = func;
                    for (int i = 0; i < this.functions[func]._paramtypes.Count; i++)
                    //int i = 0;
                    //foreach (var p in this.functions[func]._params)
                    {
                        content.DefineAndSet(this.functions[func]._paramnames[i], this.functions[func]._paramtypes[i].type, _params[i].value);
                        //i++;
                    }
                    CQ_Content.Value value = null;
                    var funcobj = this.functions[func];
                    if (this.bInterface)
                    {
                        content.CallType = (object_this as SInstance).type;
                        funcobj = (object_this as SInstance).type.functions[func];
                    }
                    if (funcobj.expr_runtime != null)
                    {
                        value = funcobj.expr_runtime.ComputeValue(content);
                        if (value != null)
                            value.breakBlock = 0;
                    }
                    contentParent.OutStack(content);

                    return value;
                }
            }
            else if (this.members.ContainsKey(func))
            {
                if (this.members[func].bStatic == false)
                {
                    Delegate dele = (object_this as SInstance).member[func].value as Delegate;
                    if (dele != null)
                    {
                        CQ_Content.Value value = new CQ_Content.Value();
                        value.type = null;
                        object[] objs = new object[_params.Count];
                        for (int i = 0; i < _params.Count; i++)
                        {
                            objs[i] = _params[i].value;
                        }
                        value.value = dele.DynamicInvoke(objs);
                        if (value.value != null)
                            value.type = value.value.GetType();
                        value.breakBlock = 0;
                        return value;
                    }
                }

            }
            throw new NotImplementedException();
        }

		public bool HasFunction(string key){
			return this.functions.ContainsKey (key) || this.members.ContainsKey (key);
		}

		public virtual IEnumerator CoroutineCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Content.Value> _params, ICoroutine coroutine){
			//TODO
			if (this.functions.ContainsKey(func))
			{
				if (this.functions[func].bStatic == false)
				{
					CQ_Content content = new CQ_Content(contentParent.environment, true);
					
					contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
					content.CallType = this;
					content.CallThis = object_this as SInstance;
					content.function = func;
					for (int i = 0; i < this.functions[func]._paramtypes.Count; i++)
						//int i = 0;
						//foreach (var p in this.functions[func]._params)
					{
						content.DefineAndSet(this.functions[func]._paramnames[i], this.functions[func]._paramtypes[i].type, _params[i].value);
						//i++;
					}
					CQ_Content.Value value = null;
					var funcobj = this.functions[func];
					if (this.bInterface)
					{
						content.CallType = (object_this as SInstance).type;
						funcobj = (object_this as SInstance).type.functions[func];
					}
					if (funcobj.expr_runtime != null)
					{
						yield return coroutine.StartNewCoroutine(funcobj.expr_runtime.CoroutineCompute(content, coroutine));
//						if (value != null)
//							value.breakBlock = 0;
					}
					contentParent.OutStack(content);
					
//					return value;
				}
			}
			else
				yield return MemberCall(contentParent, object_this, func, _params, null);
		}

        public CQ_Content.Value MemberValueGet(CQ_Content environment, object object_this, string valuename)
        {
            SInstance sin = object_this as SInstance;
            if (sin.member.ContainsKey(valuename))
            {
                CQ_Content.Value v = new CQ_Content.Value();
                v.type = sin.member[valuename].type;
                v.value = sin.member[valuename].value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool MemberValueSet(CQ_Content content, object object_this, string valuename, object value)
        {
            SInstance sin = object_this as SInstance;
            if (sin.member.ContainsKey(valuename))
            {
                if (value != null && value.GetType() != (Type)this.members[valuename].type.type)
                {
                    if (value is SInstance)
                    {
                        if ((value as SInstance).type != (SType)this.members[valuename].type.type)
                        {
                            value = content.environment.GetType((value as SInstance).type).ConvertTo(content, value, this.members[valuename].type.type);
                        }
                    }
                    else if (value is DeleEvent)
                    {

                    }
                    else
                    {
                        value = content.environment.GetType(value.GetType()).ConvertTo(content, value, this.members[valuename].type.type);
                    }
                }
                sin.member[valuename].value = value;
                return true;
            }
            throw new NotImplementedException();
        }

        public CQ_Content.Value IndexGet(CQ_Content environment, object object_this, object key)
        {
            throw new NotImplementedException();
        }

        public void IndexSet(CQ_Content environment, object object_this, object key, object value)
        {
            throw new NotImplementedException();
        }
        #endregion

        public class Function
        {
            public bool bPublic;
            public bool bStatic;
            public List<string> _paramnames = new List<string>();
            public List<ICQ_Type> _paramtypes = new List<ICQ_Type>();
            //public Dictionary<string, ICQ_Type> _params = new Dictionary<string, ICQ_Type>();
            public ICQ_Type _returntype;
            public ICQ_Expression expr_runtime;
            public string GetParamSign()
            {
                string sign = "";
                if (_returntype != null && _returntype.type != null && (Type)_returntype.type != typeof(void))
                    sign += _returntype.keyword;
                foreach (var p in _paramtypes)
                {
                    sign += "," + p.keyword;
                }
                return sign;
            }
        }
        public class Member
        {
            public ICQ_Type type;
            public bool bPublic;
            public bool bStatic;
            public bool bReadOnly;
            public ICQ_Expression expr_defvalue;
        }

        public Dictionary<string, Function> functions = new Dictionary<string, Function>();
        public Dictionary<string, Member> members = new Dictionary<string, Member>();
        public Dictionary<string, Dictionary<Type, Delegate>> deles = new Dictionary<string, Dictionary<Type, Delegate>>();
        public Dictionary<string, CQ_Content.Value> staticMemberInstance = null;




        public CQ_Content.Value StaticCallCache(CQ_Content environment, IList<CQ_Content.Value> _params, MethodCache cache)
        {
            throw new NotImplementedException();
        }


        public CQ_Content.Value MemberCallCache(CQ_Content environment, object object_this, IList<CQ_Content.Value> _params, MethodCache cache)
        {
            throw new NotImplementedException();
        }

    }

    public class SInstance
    {
        public SType type;
        public Dictionary<string, CQ_Content.Value> member = new Dictionary<string, CQ_Content.Value>();//成员
        public Dictionary<string, Dictionary<Type, Delegate>> deles = new Dictionary<string, Dictionary<Type, Delegate>>();
    }
    public class CQ_Type_Class : ICQ_Type_WithBase
    {
        public CQ_Type_Class(string keyword, bool bInterface, string filename)
        {
            this.keyword = keyword;
            this._namespace = "";
            type = new SType(keyword, "", filename, bInterface);
            compiled = false;
        }
        public void SetBaseType(IList<ICQ_Type> types)
        {
            this.types = types;
        }
        IList<ICQ_Type> types;
        public void EmbDebugToken(IList<Token> tokens)
        {
            ((SType)type).EmbDebugToken(tokens);
        }
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
        public bool compiled
        {
            get;
            set;
        }
        public CQType type
        {
            get;
            private set;
        }

        public ICQ_Value MakeValue(object value)
        {
            CQ_Value_ScriptValue svalue = new CQ_Value_ScriptValue();
            svalue.value_value = value as SInstance;
            svalue.value_type = type;
            return svalue;
        }

        public object ConvertTo(CQ_Content env, object src, CQType targetType)
        {
           
            var type = env.environment.GetType(targetType);
            if (this.type == type||(Type)targetType==typeof(object)) return src;
            if (this.types.Contains(type))
            {
                return src;
            }

            throw new NotImplementedException();
        }

        public object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            throw new NotImplementedException();
        }

        public bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right)
        {
            if (code == logictoken.equal)//[6] = {Boolean op_Equality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left == right.value;
                }
                else
                {
                    return left == right.value;
                }
            }
            else if (code == logictoken.not_equal)//[7] = {Boolean op_Inequality(CQcriptExt.Vector3, CQcriptExt.Vector3)}
            {
                if (left == null || right.type == null)
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

        public ICQ_TypeFunction function
        {
            get
            {
                return (SType)type as ICQ_TypeFunction;
            }
        }


        public object DefValue
        {
            get { return null; }
        }



    }
}
