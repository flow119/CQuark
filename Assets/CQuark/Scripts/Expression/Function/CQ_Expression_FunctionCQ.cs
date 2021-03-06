﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_FunctionCQ : ICQ_Expression {
        public CQ_Expression_FunctionCQ (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions {
            get;
            private set;
        }
        public int tokenBegin {
            get;
            private set;
        }
        public int tokenEnd {
            get;
            set;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            set;
        }
        public bool hasCoroutine {
            get {
                if(_expressions == null || _expressions.Count == 0)
                    return false;
                foreach(ICQ_Expression expr in _expressions) {
                    if(expr.hasCoroutine)
                        return true;
                }
                return false;
            }
        }
        public CQ_Value ComputeValue (CQ_Content content) {
#if CQUARK_DEBUG
            content.InStack(this);
#endif

            CQ_Value[] parameters = CQ_ObjPool.PopArray(_expressions.Count);
            for(int i = 0; i < _expressions.Count; i++) {
                parameters[i] = _expressions[i].ComputeValue(content);
            }

            CQ_Value v = CQ_Value.Null;

            Class_CQuark.Function retFunc = null;
            bool bFind = false;
            if(content.CallType != null)
                bFind = content.CallType.functions.TryGetValue(funcname, out retFunc);

            if(bFind) {
                if(retFunc.bStatic) {
                    v = content.CallType.StaticCall(content, funcname, parameters);
                }
                else {
                    v = content.CallType.MemberCall(content, content.CallThis, funcname, parameters);
                }
            }
            else {
                v = content.GetQuiet(funcname);
				if(v.IsDelegate) {
                    Delegate d = v.GetObject() as Delegate;
                    v = new CQ_Value();
                    object[] obja = new object[parameters.Length];
                    for(int i = 0; i < parameters.Length; i++) {
                        obja[i] = parameters[i].GetObject();
                    }
                    object obj = d.DynamicInvoke(obja);
                    
                    if(obj == null) {
                        v.SetNoneTypeObject(null);
                    }
                    else {
                        v.SetObject(obj.GetType(), obj);
                    }
                }
                else {
                    throw new Exception(funcname + "没有这样的方法");
                    //v = CQuark.AppDomain.GetMethod(funcname).Call(content, list);
                }
            }
            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            CQ_ObjPool.PushArray(parameters);
            return v;
        }

        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
            throw new Exception("不支持方法内套用协程");
        }
        public string funcname;

        public override string ToString () {
            return "Call|" + funcname + "(params[" + _expressions.Count + ")";
        }
    }
}