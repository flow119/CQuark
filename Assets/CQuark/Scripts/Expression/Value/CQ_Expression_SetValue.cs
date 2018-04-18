using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {
    public class CQ_Expression_SetValue : ICQ_Expression {

        public CQ_Expression_SetValue (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            private set;
        }

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
            private set;
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
            {

                CQ_Value v = _expressions[0].ComputeValue(content);

                {
                    object val = v.GetValue();
                    TypeBridge value_type = null;
                    if(content.values != null && content.values.ContainsKey(value_name)) {
                        value_type = content.values[value_name].typeBridge;
                    }
                    else {
                        if(content.CallType != null) {
                            if(content.CallType.members.ContainsKey(value_name)) {
                                if(content.CallType.members[value_name].bStatic) {
                                    value_type = content.CallType.staticMemberInstance[value_name].typeBridge;
                                }
                                else {
                                    value_type = content.CallThis.member[value_name].typeBridge;
                                }

                            }

                        }
                    }
                    //val = v.value;
                    if((Type)value_type != typeof(Type_Var.var) && value_type != v.typeBridge) {
                        val = v.ConvertTo(value_type);
                    }


                    content.Set(value_name, val);
                }
            }
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return CQ_Value.Null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif

            if(_expressions[0].hasCoroutine) {
                yield return coroutine.StartCoroutine(_expressions[0].CoroutineCompute(content, coroutine));
            }
            else {
                CQ_Value v = _expressions[0].ComputeValue(content);

                {
                    object val = v.GetValue();
                    TypeBridge value_type = null;
                    if(content.values != null && content.values.ContainsKey(value_name)) {
                        value_type = content.values[value_name].typeBridge;
                    }
                    else {
                        if(content.CallType != null) {
                            if(content.CallType.members.ContainsKey(value_name)) {
                                if(content.CallType.members[value_name].bStatic) {
                                    value_type = content.CallType.staticMemberInstance[value_name].typeBridge;
                                }
                                else {
                                    value_type = content.CallThis.member[value_name].typeBridge;
                                }

                            }

                        }
                    }
                    //val = v.value;
                    if((Type)value_type != typeof(Type_Var.var) && value_type != v.typeBridge) {
                        val = v.ConvertTo(value_type);
                    }


                    content.Set(value_name, val);
                }
            }
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
        }

        public string value_name;

        public override string ToString () {
            return "SetValue|" + value_name + "=";
        }
    }
}