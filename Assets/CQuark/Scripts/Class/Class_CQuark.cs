using System;
using System.Collections;
using System.Collections.Generic;

namespace CQuark {
    //由西瓜脚本里获取的自定义Type
    public class Class_CQuark : IClass {
        public Class_CQuark (string keyword, string _namespace, string filename, bool bInterface) {
            this.Name = keyword;
//            this.Namespace = _namespace;
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
		//因为一些特殊需要，attributes我也保存下来
		public List<string> attributes;
        //dumplog用的
        public IList<Token> tokenlist {
            get;
            private set;
        }
        public void EmbDebugToken (IList<Token> tokens) {
            this.tokenlist = tokens;
        }

        #region impl type
//        public string FullName {
//            get { return Namespace + "." + Name; }
//        }

//        public string Namespace {
//            get;
//            private set;
//        }

        public string Name {
            get;
            private set;

        }

        #endregion

        #region Script IMPL
        CQ_Content contentMemberCalc = null;
        public CQ_Value New (CQ_Content content, CQ_Value[] _params) {
            if(contentMemberCalc == null) {
				contentMemberCalc = CQ_ObjPool.PopContent();
            }
               
            NewStatic();

            
            CQ_ClassInstance c = new CQ_ClassInstance();
            c.type = this;
 
            foreach(KeyValuePair<string, Member> i in this.members) {
                if(i.Value.bStatic == false) {
                    if(i.Value.expr_defvalue == null) {
                        CQ_Value val = new CQ_Value();
                        val.SetObject(i.Value.m_itype.typeBridge, i.Value.m_itype.defaultValue);
                        c.member[i.Key] = val;
                        //sv.value_value.member[i.Key] = new CQ_Value();
                        //sv.value_value.member[i.Key].SetCQType(i.Value.type.cqType);
                        //sv.value_value.member[i.Key].value = i.Value.type.defaultValue;
                    }
                    else {
                        var value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                        if(i.Value.m_itype.typeBridge != value.typeBridge) {
                            CQ_Value val = new CQ_Value();
                            val.SetObject(i.Value.m_itype.typeBridge, value.ConvertTo(i.Value.m_itype.typeBridge));
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
            v.SetObject(this, c);
            return v;
        }
        void NewStatic () {
            if(this.staticMemberInstance == null) {
                staticMemberInstance = new Dictionary<string, CQ_Value>();
                foreach(var i in this.members) {
                    if(i.Value.bStatic == true) {
                        if(i.Value.expr_defvalue == null) {

                            CQ_Value val = new CQ_Value();
                            val.SetObject(i.Value.m_itype.typeBridge, i.Value.m_itype.defaultValue);
                            staticMemberInstance[i.Key] = val;

                        }
                        else {
							CQ_Value value = i.Value.expr_defvalue.ComputeValue(contentMemberCalc);
                            if(i.Value.m_itype.typeBridge != value.typeBridge) {

                                CQ_Value val = new CQ_Value();
                                val.SetObject(i.Value.m_itype.typeBridge, value.ConvertTo(i.Value.m_itype.typeBridge));
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
					CQ_Content content = CQ_ObjPool.PopContent();
                    content.CallType = this;
                    content.CallThis = null;

#if CQUARK_DEBUG
                    content.function = function;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    // int i = 0;
                    for(int i = 0; i < functions[function]._paramtypes.Count; i++)
                    {
                        //content.DefineAndSet(functions[function]._paramnames[i], functions[function]._paramtypes[i].typeBridge, _params[i].GetValue());
						content.DefineAndSet(functions[function]._paramnames[i], _params[i].typeBridge, _params[i]);
                    }
                    CQ_Value value = CQ_Value.Null;
                    if(this.functions[function].expr_runtime != null) {
                        value = this.functions[function].expr_runtime.ComputeValue(content);
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif
					CQ_ObjPool.PushContent(content);
                    return value;
                }
            }
            else if(this.members.ContainsKey(function)) {
                if(this.members[function].bStatic == true) {
                    Delegate dele = this.staticMemberInstance[function].GetObject() as Delegate;
                    if(dele != null) {
                        CQ_Value value = new CQ_Value();
                        object[] objs = new object[_params.Length];
                        for(int i = 0; i < _params.Length; i++) {
                            objs[i] = _params[i].GetObject();
                        }
                        object obj = dele.DynamicInvoke(objs);
                        if(obj == null) {
                            return CQ_Value.Null;
                        }
                        else {
                            value.SetObject(obj.GetType(), obj);
                            return value;
                        }
                        //value.breakBlock = BreakType.None;
                    }
                }

            }
            throw new NotImplementedException();
        }

        public CQ_Value StaticValueGet (CQ_Content content, string valuename) {
            NewStatic();
            CQ_Value temp = CQ_Value.Null;
            if(this.staticMemberInstance.TryGetValue(valuename, out temp)) {
				return temp;
            }
            throw new NotImplementedException();
        }

        public bool StaticValueSet (CQ_Content content, string valuename, CQ_Value value) {
            NewStatic();
           
            if(this.staticMemberInstance.ContainsKey(valuename)) {
                CQ_Value oldVal = this.staticMemberInstance[valuename];
                oldVal.UsingValue(value);
                this.staticMemberInstance[valuename] = oldVal;
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
					CQ_Content content = CQ_ObjPool.PopContent();
                    content.CallType = this;
                    content.CallThis = object_this as CQ_ClassInstance;
                    
#if CQUARK_DEBUG
                    content.function = func;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    for(int i = 0; i < funccache._paramtypes.Count; i++) {
                        //content.DefineAndSet(funccache._paramnames[i], funccache._paramtypes[i].typeBridge, _params[i].GetValue());
						content.DefineAndSet(funccache._paramnames[i], _params[i].typeBridge, _params[i]);
                    }
					
					//如果返回值是IEnumerator的话，这里把方法返回出来
					if(funccache._returntype != null && funccache._returntype.typeBridge.type == typeof(IEnumerator)){
						CQ_Value ienumerator = new CQ_Value();
						CQ_Expression_Block funcCQ = funccache.expr_runtime as CQ_Expression_Block;
						funcCQ.callObj = content;
						ienumerator.SetObject(typeof(CQ_Expression_Block), funcCQ);
						return ienumerator;
					}
					

                    CQ_Value value = CQ_Value.Null;
                    var funcobj = funccache;
                    if(this.bInterface) {
                        content.CallType = (object_this as CQ_ClassInstance).type;
                        funcobj = (object_this as CQ_ClassInstance).type.functions[func];
                    }
                    if(funcobj.expr_runtime != null) {
                        value = funcobj.expr_runtime.ComputeValue(content);
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif
					CQ_ObjPool.PushContent(content);
                    return value;
                }
            }
            else if(this.members.ContainsKey(func)) {
                if(this.members[func].bStatic == false) {
                    Delegate dele = (object_this as CQ_ClassInstance).member[func].GetObject() as Delegate;
                    if(dele != null) {
                        CQ_Value value = new CQ_Value();
                        object[] objs = new object[_params.Length];
                        for(int i = 0; i < _params.Length; i++) {
                            objs[i] = _params[i].GetObject();
                        }
                        object obj = dele.DynamicInvoke(objs);
                        if(obj == null) {
                            return CQ_Value.Null;
                        }
                        else {
                            value.SetObject(obj.GetType(), obj);
                            return value;
                        }
                    }
                }

            }
            throw new NotImplementedException();
        }


        public virtual IEnumerator CoroutineCall (CQ_Content contentParent, object object_this, string func, CQ_Value[] _params, UnityEngine.MonoBehaviour coroutine) {
            Function funccache = null;
            if(this.functions.TryGetValue(func, out funccache)) {
                if(!funccache.bStatic) {
					CQ_Content content = CQ_ObjPool.PopContent();
                    content.CallType = this;
                    content.CallThis = object_this as CQ_ClassInstance;
#if CQUARK_DEBUG
                    content.function = func;
                    contentParent.InStack(content);//把这个上下文推给上层的上下文，这样如果崩溃是可以一层层找到原因的
#endif
                    for(int i = 0; i < funccache._paramtypes.Count; i++)
                    {
						content.DefineAndSet(funccache._paramnames[i], _params[i].typeBridge, _params[i]);
                    }

                    var funcobj = funccache;
                    if(this.bInterface) {
                        content.CallType = (object_this as CQ_ClassInstance).type;
                        funcobj = (object_this as CQ_ClassInstance).type.functions[func];
                    }
                    if(funcobj.expr_runtime != null) {
                       yield return coroutine.StartCoroutine(funcobj.expr_runtime.CoroutineCompute(content, coroutine));
                    }
#if CQUARK_DEBUG
                    contentParent.OutStack(content);
#endif
					CQ_ObjPool.PushContent(content);
                }
            }
            else
                yield return MemberCall(contentParent, object_this, func, _params, null);
        }
		
		public virtual IEnumerator CoroutineCall (ICQ_Expression expression, CQ_Content param, UnityEngine.MonoBehaviour coroutine) {
			yield return coroutine.StartCoroutine(expression.CoroutineCompute(param, coroutine));
		}

        public CQ_Value MemberValueGet (CQ_Content content, object object_this, string valuename) {
            CQ_ClassInstance sin = object_this as CQ_ClassInstance;
            CQ_Value temp = CQ_Value.Null;
            if(sin.member.TryGetValue(valuename, out temp)) {
                return temp;
            }
            throw new NotImplementedException();
        }

        public bool MemberValueSet (CQ_Content content, object object_this, string valuename, CQ_Value value) {
            CQ_ClassInstance sin = object_this as CQ_ClassInstance;
           
            if(sin.member.ContainsKey(valuename)) {
                CQ_Value oldVal = sin.member[valuename];
                oldVal.UsingValue(value);
                sin.member[valuename] = oldVal;
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
        public struct Member {
			public List<string> attributes;
            public IType m_itype;
            public bool bPublic;
            public bool bStatic;
            public bool bReadOnly;
            public ICQ_Expression expr_defvalue;

            public static Member Null {
                get {
                    return new Member();
                }
            }
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
