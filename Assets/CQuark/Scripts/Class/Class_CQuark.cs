using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
    //由西瓜脚本里获取的自定义Type
    public class Class_CQuark : IClass {
        public Class_CQuark (string keyword, string _namespace, string filename, bool bInterface) {
            this.Name = keyword;
            this.Namespace = _namespace;
            this.filename = filename;
            this.bInterface = bInterface;
        }
        public string filename {
            get;
            private set;
        }
        public bool bInterface {
            get;
            private set;
        }

        //dumplog用的
        public IList<Token> tokenlist {
            get;
            private set;
        }
        public void EmbDebugToken (IList<Token> tokens) {
            this.tokenlist = tokens;
        }

        #region impl type
        public string FullName {
            get { return Namespace + "." + Name; }
        }

        public string Namespace {
            get;
            private set;
        }

        public string Name {
            get;
            private set;

        }

        #endregion

        #region Script IMPL
        CQ_Content contentMemberCalc = null;
        public CQ_Value New (CQ_Content content, CQ_Value[] _params) {
            if(contentMemberCalc == null)
                contentMemberCalc = new CQ_Content();
            NewStatic();
            //CQ_Expression_Value_ScriptValue sv = new CQ_Expression_Value_ScriptValue();
            //sv.value_type = this;
            //sv.value_value = new CQClassInstance();
            //sv.value_value.type = this;


            

            
            CQ_ClassInstance c = new CQ_ClassInstance();
            c.type = this;
            
          


            foreach(KeyValuePair<string, Member> i in this.members) {
                if(i.Value.bStatic == false) {
                    if(i.Value.expr_defvalue == null) {
                        CQ_Value val = new CQ_Value();
                        val.SetCQType(i.Value.type.typeBridge);
                        val.m_value = i.Value.type.defaultValue;
                        c.member[i.Key] = val;
                        //sv.value_value.member[i.Key] = new CQ_Value();
                        //sv.value_value.member[i.Key].SetCQType(i.Value.type.cqType);
                        //sv.value_value.member[i.Key].value = i.Value.type.defaultValue;
                    }
                    else {
                        var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                        if(i.Value.type.typeBridge != value.typeBridge) {
                            CQ_Value val = new CQ_Value();
                            val.SetCQType(i.Value.type.typeBridge);
                            val.m_value = value.ConvertTo(i.Value.type.typeBridge);
                            c.member[i.Key] = val;
                            //sv.value_value.member[i.Key] = val;
                            //sv.value_value.member[i.Key] = new CQ_Value();
                            //sv.value_value.member[i.Key].SetCQType(i.Value.type.cqType);
                            //sv.value_value.member[i.Key].value = value.ConvertTo(i.Value.type.cqType);
                        }
                        else {
                            c.member[i.Key] = value;
                        }
                    }
                }
            }
            if(this.functions.ContainsKey(this.Name))//有同名函数就调用
            {
                MemberCall(content, c, this.Name, _params);
            }

            CQ_Value v = new CQ_Value();
            v.m_stype = this;
            v.m_value = c;
            return v;
        }
        void NewStatic () {
            if(contentMemberCalc == null)
                contentMemberCalc = new CQ_Content();
            if(this.staticMemberInstance == null) {
                staticMemberInstance = new Dictionary<string, CQ_Value>();
                foreach(var i in this.members) {
                    if(i.Value.bStatic == true) {
                        if(i.Value.expr_defvalue == null) {

                            CQ_Value val = new CQ_Value();
                            val.SetCQType(i.Value.type.typeBridge);
                            val.m_value = i.Value.type.defaultValue;
                            staticMemberInstance[i.Key] = val;

                        }
                        else {
                            var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                            if(i.Value.type.typeBridge != value.typeBridge) {

                                CQ_Value val = new CQ_Value();
                                val.SetCQType(i.Value.type.typeBridge);
                                val.m_value = value.ConvertTo(i.Value.type.typeBridge);
                                staticMemberInstance[i.Key] = val;

                                //staticMemberInstance[i.Key] = new CQ_Value();
                                //staticMemberInstance[i.Key].SetCQType( i.Value.type.cqType);
                                //staticMemberInstance[i.Key].value = value.ConvertTo(i.Value.type.cqType);
                            }
                            else {
                                staticMemberInstance[i.Key] = value;
                            }
                        }
                    }
                }
            }
        }
        public CQ_Value StaticCall (CQ_Content contentParent, string function, CQ_Value[] _params) {
            return StaticCall(contentParent, function, _params, null);
        }
        public CQ_Value StaticCall (CQ_Content contentParent, string function, CQ_Value[] _params, MethodCache cache) {
            if(cache != null) {
                cache.cachefail = true;
            }
            NewStatic();
            if(this.functions.ContainsKey(function)) {
                if(this.functions[function].bStatic == true) {
                    CQ_Content content = new CQ_Content();
                    content.CallType = this;
                    content.CallThis = null;
#if CQUARK_DEBUG
                    content.function = function;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    // int i = 0;
                    for(int i = 0; i < functions[function]._paramtypes.Count; i++)
                    //foreach (var p in this.functions[function]._params)
                    {
                        content.DefineAndSet(functions[function]._paramnames[i], functions[function]._paramtypes[i].typeBridge, _params[i].m_value);
                        //i++;
                    }
                    //var value = this.functions[function].expr_runtime.ComputeValue(content);
                    CQ_Value value = CQ_Value.Null;
                    if(this.functions[function].expr_runtime != null) {
                        value = this.functions[function].expr_runtime.ComputeValue(content);
                        //if(value != null)
                        //    value.breakBlock = BreakType.None;
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif

                    return value;
                }
            }
            else if(this.members.ContainsKey(function)) {
                if(this.members[function].bStatic == true) {
                    Delegate dele = this.staticMemberInstance[function].m_value as Delegate;
                    if(dele != null) {
                        CQ_Value value = new CQ_Value();
                        object[] objs = new object[_params.Length];
                        for(int i = 0; i < _params.Length; i++) {
                            objs[i] = _params[i].m_value;
                        }
                        value.m_value = dele.DynamicInvoke(objs);
                        if(value.m_value != null)
                            value.m_type = value.m_value.GetType();
                        //value.breakBlock = BreakType.None;
                        return value;
                    }
                }

            }
            throw new NotImplementedException();
        }

        public CQ_Value StaticValueGet (CQ_Content content, string valuename) {
            NewStatic();
            CQ_Value temp = CQ_Value.Null;
            if(this.staticMemberInstance.TryGetValue(valuename, out temp)) {
                CQ_Value v = new CQ_Value();
                v.m_type = temp.m_type;
                v.m_stype = temp.m_stype;
                v.m_value = temp.m_value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool StaticValueSet (CQ_Content content, string valuename, object value) {
            NewStatic();
            if(this.staticMemberInstance.ContainsKey(valuename)) {
                if(value != null && value.GetType() != (Type)this.members[valuename].type.typeBridge) {
                    if(value is CQ_ClassInstance) {
                        if((value as CQ_ClassInstance).type != (Class_CQuark)this.members[valuename].type.typeBridge) {
							value = CQuark.AppDomain.GetITypeByClassCQ((value as CQ_ClassInstance).type).ConvertTo(value, this.members[valuename].type.typeBridge);
                        }
                    }
                    else if(value is DeleEvent) {

                    }
                    else {
                        value = CQuark.AppDomain.ConvertTo(value, this.members[valuename].type.typeBridge);
                    }
                }
                CQ_Value val = this.staticMemberInstance[valuename];
                val.m_value = value;
				this.staticMemberInstance[valuename] = val;
                return true;
            }
            throw new NotImplementedException();
        }
        public CQ_Value MemberCall (CQ_Content contentParent, object object_this, string func, CQ_Value[] _params) {
            return MemberCall(contentParent, object_this, func, _params, null);
        }
        public CQ_Value MemberCall (CQ_Content contentParent, object object_this, string func, CQ_Value[] _params, MethodCache cache) {
            if(cache != null) {
                cache.cachefail = true;
            }
            Function funccache = null;
            if(this.functions.TryGetValue(func, out funccache)) {
                if(funccache.bStatic == false) {
                    CQ_Content content = new CQ_Content();

                    content.CallType = this;
                    content.CallThis = object_this as CQ_ClassInstance;
#if CQUARK_DEBUG
                    content.function = func;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    for(int i = 0; i < funccache._paramtypes.Count; i++) {
                        content.DefineAndSet(funccache._paramnames[i], funccache._paramtypes[i].typeBridge, _params[i].m_value);
                    }
                    CQ_Value value = CQ_Value.Null;
                    var funcobj = funccache;
                    if(this.bInterface) {
                        content.CallType = (object_this as CQ_ClassInstance).type;
                        funcobj = (object_this as CQ_ClassInstance).type.functions[func];
                    }
                    if(funcobj.expr_runtime != null) {
                        value = funcobj.expr_runtime.ComputeValue(content);
                        //if(value != null)
                        //    value.breakBlock = BreakType.None;
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif

                    return value;
                }
            }
            else if(this.members.ContainsKey(func)) {
                if(this.members[func].bStatic == false) {
                    Delegate dele = (object_this as CQ_ClassInstance).member[func].m_value as Delegate;
                    if(dele != null) {
                        CQ_Value value = new CQ_Value();
                        object[] objs = new object[_params.Length];
                        for(int i = 0; i < _params.Length; i++) {
                            objs[i] = _params[i].m_value;
                        }
                        value.m_value = dele.DynamicInvoke(objs);
                        if(value.m_value != null)
                            value.m_type = value.m_value.GetType();
                        //value.breakBlock = BreakType.None;
                        return value;
                    }
                }

            }
            throw new NotImplementedException();
        }


        public virtual IEnumerator CoroutineCall (CQ_Content contentParent, object object_this, string func, CQ_Value[] _params, UnityEngine.MonoBehaviour coroutine) {
            //TODO
            Function funccache = null;
            if(this.functions.TryGetValue(func, out funccache)) {
                if(funccache.bStatic == false) {
                    CQ_Content content = new CQ_Content();

                    content.CallType = this;
                    content.CallThis = object_this as CQ_ClassInstance;
#if CQUARK_DEBUG
                    content.function = func;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    for(int i = 0; i < funccache._paramtypes.Count; i++)
                    //int i = 0;
                    //foreach (var p in this.functions[func]._params)
                    {
                        content.DefineAndSet(funccache._paramnames[i], funccache._paramtypes[i].typeBridge, _params[i].m_value);
                        //i++;
                    }
                    //CQ_Content.Value value = null;
                    var funcobj = funccache;
                    if(this.bInterface) {
                        content.CallType = (object_this as CQ_ClassInstance).type;
                        funcobj = (object_this as CQ_ClassInstance).type.functions[func];
                    }
                    if(funcobj.expr_runtime != null) {
                        yield return coroutine.StartCoroutine(funcobj.expr_runtime.CoroutineCompute(content, coroutine));
                        //						if (value != null)
                        //							value.breakBlock = 0;
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif

                    //					return value;
                }
            }
            else
                yield return MemberCall(contentParent, object_this, func, _params, null);
        }

        public CQ_Value MemberValueGet (CQ_Content content, object object_this, string valuename) {
            CQ_ClassInstance sin = object_this as CQ_ClassInstance;
            CQ_Value temp = CQ_Value.Null;
            if(sin.member.TryGetValue(valuename, out temp)) {
                CQ_Value v = new CQ_Value();
                v.m_type = temp.m_type;
                v.m_stype = temp.m_stype;
                v.m_value = temp.m_value;
                return v;
            }
            throw new NotImplementedException();
        }

        public bool MemberValueSet (CQ_Content content, object object_this, string valuename, object value) {
            CQ_ClassInstance sin = object_this as CQ_ClassInstance;
            if(sin.member.ContainsKey(valuename)) {
                if(value != null && value.GetType() != (Type)this.members[valuename].type.typeBridge) {
                    if(value is CQ_ClassInstance) {
                        if((value as CQ_ClassInstance).type != (Class_CQuark)this.members[valuename].type.typeBridge) {
                            value = CQuark.AppDomain.GetITypeByClassCQ((value as CQ_ClassInstance).type).ConvertTo(value, this.members[valuename].type.typeBridge);
                        }
                    }
                    else if(value is DeleEvent) {

                    }
                    else {
                        value = CQuark.AppDomain.ConvertTo(value, this.members[valuename].type.typeBridge);
                    }
                }
                CQ_Value val = sin.member[valuename];
                val.m_value = value;
                sin.member[valuename] = val;
                return true;
            }
            throw new NotImplementedException();
        }

        public CQ_Value IndexGet (CQ_Content content, object object_this, object key) {
            throw new NotImplementedException();
        }

        public void IndexSet (CQ_Content content, object object_this, object key, object value) {
            throw new NotImplementedException();
        }
        #endregion

        public class Function {
            public bool bPublic;
            public bool bStatic;
            public List<string> _paramnames = new List<string>();
            public List<IType> _paramtypes = new List<IType>();
            public IType _returntype;
            public ICQ_Expression expr_runtime;
           
        }
        public class Member {
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




        public CQ_Value StaticCallCache (CQ_Content content, CQ_Value[] _params, MethodCache cache) {
            throw new NotImplementedException();
        }


        public CQ_Value MemberCallCache (CQ_Content content, object object_this, CQ_Value[] _params, MethodCache cache) {
            throw new NotImplementedException();
        }

    }
}
