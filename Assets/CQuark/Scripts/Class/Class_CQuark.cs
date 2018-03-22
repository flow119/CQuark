using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    //由西瓜脚本里获取的自定义Type
    public class Class_CQuark : IClass
    {
        public Class_CQuark(string keyword, string _namespace, string filename, bool bInterface)
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

		//dumplog用的
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
        public CQ_Value New(CQ_Content content, IList<CQ_Value> _params)
        {
            if (contentMemberCalc == null)
                contentMemberCalc = new CQ_Content();
            NewStatic();
            CQ_Expression_Value_ScriptValue sv = new CQ_Expression_Value_ScriptValue();
            sv.value_type = this;
            sv.value_value = new CQClassInstance();
            sv.value_value.type = this;
            foreach (KeyValuePair<string, Member> i in this.members)
            {
                if (i.Value.bStatic == false)
                {
                    if (i.Value.expr_defvalue == null)
                    {
                        sv.value_value.member[i.Key] = new CQ_Value();
                        sv.value_value.member[i.Key].type = i.Value.type.typeBridge;
                        sv.value_value.member[i.Key].value = i.Value.type.defaultValue;
                    }
                    else
                    {
                        var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                        if (i.Value.type.typeBridge != value.type)
                        {
                            sv.value_value.member[i.Key] = new CQ_Value();
                            sv.value_value.member[i.Key].type = i.Value.type.typeBridge;
                            sv.value_value.member[i.Key].value = CQuark.AppDomain.GetType(value.type).ConvertTo(value.value, i.Value.type.typeBridge);
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
            return CQ_Value.FromICQ_Value(sv);
        }
        void NewStatic()
        {
            if (contentMemberCalc == null)
                contentMemberCalc = new CQ_Content();
            if (this.staticMemberInstance == null)
            {
                staticMemberInstance = new Dictionary<string, CQ_Value>();
                foreach (var i in this.members)
                {
                    if (i.Value.bStatic == true)
                    {
                        if (i.Value.expr_defvalue == null)
                        {
                            staticMemberInstance[i.Key] = new CQ_Value();

                            staticMemberInstance[i.Key].type = i.Value.type.typeBridge;
                            staticMemberInstance[i.Key].value = i.Value.type.defaultValue;
                        }
                        else
                        {
                            var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                            if (i.Value.type.typeBridge != value.type)
                            {
                                staticMemberInstance[i.Key] = new CQ_Value();
                                staticMemberInstance[i.Key].type = i.Value.type.typeBridge;
                                staticMemberInstance[i.Key].value = CQuark.AppDomain.GetType(value.type).ConvertTo(value.value, i.Value.type.typeBridge);
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
        public CQ_Value StaticCall(CQ_Content contentParent, string function, IList<CQ_Value> _params)
        {
            return StaticCall(contentParent, function, _params, null);
        }
        public CQ_Value StaticCall(CQ_Content contentParent, string function, IList<CQ_Value> _params, MethodCache cache)
        {
            if (cache != null)
            {
                cache.cachefail = true;
            }
            NewStatic();
            if (this.functions.ContainsKey(function))
            {
                if (this.functions[function].bStatic == true)
                {
                    CQ_Content content = new CQ_Content();

                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
                    content.CallType = this;
                    content.CallThis = null;
					#if CQUARK_DEBUG
                    content.function = function;
					#endif
                    // int i = 0;
                    for (int i = 0; i < functions[function]._paramtypes.Count; i++)
                    //foreach (var p in this.functions[function]._params)
                    {
                        content.DefineAndSet(functions[function]._paramnames[i], functions[function]._paramtypes[i].typeBridge, _params[i].value);
                        //i++;
                    }
                    //var value = this.functions[function].expr_runtime.ComputeValue(content);
                    CQ_Value value = null;
                    if (this.functions[function].expr_runtime != null)
                    {
                        value = this.functions[function].expr_runtime.ComputeValue(content);
                        if (value != null)
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
                        CQ_Value value = new CQ_Value();
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

        public CQ_Value StaticValueGet(CQ_Content content, string valuename)
        {
            NewStatic();

            if (this.staticMemberInstance.ContainsKey(valuename))
            {
                CQ_Value v = new CQ_Value();
                v.type = this.staticMemberInstance[valuename].type;
                v.value = this.staticMemberInstance[valuename].value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool StaticValueSet(CQ_Content content, string valuename, object value)
        {
            NewStatic();
            if (this.staticMemberInstance.ContainsKey(valuename))
            {
                if (value != null && value.GetType() != (Type)this.members[valuename].type.typeBridge)
                {
                    if (value is CQClassInstance)
                    {
                        if ((value as CQClassInstance).type != (Class_CQuark)this.members[valuename].type.typeBridge)
                        {
                            value = CQuark.AppDomain.GetType((value as CQClassInstance).type).ConvertTo(value, this.members[valuename].type.typeBridge);
                        }
                    }
                    else if (value is DeleEvent)
                    {

                    }
                    else
                    {
                        value = CQuark.AppDomain.GetType(value.GetType()).ConvertTo(value, this.members[valuename].type.typeBridge);
                    }
                }
                this.staticMemberInstance[valuename].value = value;
                return true;
            }
            throw new NotImplementedException();
        }
        public CQ_Value MemberCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Value> _params)
        {
            return MemberCall(contentParent, object_this, func, _params, null);
        }
        public CQ_Value MemberCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Value> _params, MethodCache cache)
        {
            if (cache != null)
            {
                cache.cachefail = true;
            }
            if (this.functions.ContainsKey(func))
            {
                if (this.functions[func].bStatic == false)
                {
                    CQ_Content content = new CQ_Content();

                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
                    content.CallType = this;
                    content.CallThis = object_this as CQClassInstance;
					#if CQUARK_DEBUG
                    content.function = func;
					#endif
                    for (int i = 0; i < this.functions[func]._paramtypes.Count; i++)
                    {
                        content.DefineAndSet(this.functions[func]._paramnames[i], this.functions[func]._paramtypes[i].typeBridge, _params[i].value);
                    }
                    CQ_Value value = null;
                    var funcobj = this.functions[func];
                    if (this.bInterface)
                    {
                        content.CallType = (object_this as CQClassInstance).type;
                        funcobj = (object_this as CQClassInstance).type.functions[func];
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
                    Delegate dele = (object_this as CQClassInstance).member[func].value as Delegate;
                    if (dele != null)
                    {
                        CQ_Value value = new CQ_Value();
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

        public bool HasFunction(string key)
        {
            return this.functions.ContainsKey(key) || this.members.ContainsKey(key);
        }

        public virtual IEnumerator CoroutineCall(CQ_Content contentParent, object object_this, string func, IList<CQ_Value> _params, ICoroutine coroutine)
        {
            //TODO
            if (this.functions.ContainsKey(func))
            {
                if (this.functions[func].bStatic == false)
                {
                    CQ_Content content = new CQ_Content();

                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
                    content.CallType = this;
                    content.CallThis = object_this as CQClassInstance;
					#if CQUARK_DEBUG
                    content.function = func;
					#endif
                    for (int i = 0; i < this.functions[func]._paramtypes.Count; i++)
                    //int i = 0;
                    //foreach (var p in this.functions[func]._params)
                    {
                        content.DefineAndSet(this.functions[func]._paramnames[i], this.functions[func]._paramtypes[i].typeBridge, _params[i].value);
                        //i++;
                    }
                    //CQ_Content.Value value = null;
                    var funcobj = this.functions[func];
                    if (this.bInterface)
                    {
                        content.CallType = (object_this as CQClassInstance).type;
                        funcobj = (object_this as CQClassInstance).type.functions[func];
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

        public CQ_Value MemberValueGet(CQ_Content content, object object_this, string valuename)
        {
            CQClassInstance sin = object_this as CQClassInstance;
            if (sin.member.ContainsKey(valuename))
            {
                CQ_Value v = new CQ_Value();
                v.type = sin.member[valuename].type;
                v.value = sin.member[valuename].value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool MemberValueSet(CQ_Content content, object object_this, string valuename, object value)
        {
            CQClassInstance sin = object_this as CQClassInstance;
            if (sin.member.ContainsKey(valuename))
            {
                if (value != null && value.GetType() != (Type)this.members[valuename].type.typeBridge)
                {
                    if (value is CQClassInstance)
                    {
                        if ((value as CQClassInstance).type != (Class_CQuark)this.members[valuename].type.typeBridge)
                        {
                            value = CQuark.AppDomain.GetType((value as CQClassInstance).type).ConvertTo(value, this.members[valuename].type.typeBridge);
                        }
                    }
                    else if (value is DeleEvent)
                    {

                    }
                    else
                    {
                        value = CQuark.AppDomain.GetType(value.GetType()).ConvertTo(value, this.members[valuename].type.typeBridge);
                    }
                }
                sin.member[valuename].value = value;
                return true;
            }
            throw new NotImplementedException();
        }

        public CQ_Value IndexGet(CQ_Content content, object object_this, object key)
        {
            throw new NotImplementedException();
        }

        public void IndexSet(CQ_Content content, object object_this, object key, object value)
        {
            throw new NotImplementedException();
        }
        #endregion

        public class Function
        {
            public bool bPublic;
            public bool bStatic;
            public List<string> _paramnames = new List<string>();
            public List<IType> _paramtypes = new List<IType>();
            //public Dictionary<string, ICQ_Type> _params = new Dictionary<string, ICQ_Type>();
            public IType _returntype;
            public ICQ_Expression expr_runtime;
            public string GetParamSign()
            {
                string sign = "";
                if (_returntype != null && _returntype.typeBridge != null && (Type)_returntype.typeBridge != typeof(void))
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
            public IType type;
            public bool bPublic;
            public bool bStatic;
            public bool bReadOnly;
            public ICQ_Expression expr_defvalue;
        }

        public Dictionary<string, Function> functions = new Dictionary<string, Function>();
        public Dictionary<string, Member> members = new Dictionary<string, Member>();
        public Dictionary<string, Dictionary<Type, Delegate>> deles = new Dictionary<string, Dictionary<Type, Delegate>>();
        public Dictionary<string, CQ_Value> staticMemberInstance = null;




        public CQ_Value StaticCallCache(CQ_Content content, IList<CQ_Value> _params, MethodCache cache)
        {
            throw new NotImplementedException();
        }


        public CQ_Value MemberCallCache(CQ_Content content, object object_this, IList<CQ_Value> _params, MethodCache cache)
        {
            throw new NotImplementedException();
        }

    }
}
