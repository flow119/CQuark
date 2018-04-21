using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_LoopSwitchCase : ICQ_Expression {
        public CQ_Expression_LoopSwitchCase (int tbegin, int tend, int lbegin, int lend) {
            _expressions = new List<ICQ_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;

            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin {
            get;
            private set;
        }
        public int lineEnd {
            get;
            set;
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
            content.DepthAdd();
            ICQ_Expression expr_switch = _expressions[0] as ICQ_Expression;
            CQ_Value switchVal = CQ_Value.Null;
            //			CQ_Content.Value vrt = null;
            if(expr_switch != null)
                switchVal = expr_switch.ComputeValue(content);//switch//

            for(int i = 1; i < _expressions.Count - 1; i += 2) {
                if(_expressions[i] != null) {
                    //case xxx://
                    if(switchVal.GetObject().Equals(_expressions[i].ComputeValue(content).GetObject())) {
                        while(_expressions[i + 1] == null) {
                            i += 2;
                        }
                        //						content.InStack(_expressions[i+1]);
                        content.DepthAdd();
                        _expressions[i + 1].ComputeValue(content);
                        break;
                    }
                    else {
                        continue;
                    }
                }
                else {
                    //default:
                    //					content.InStack(_expressions[i+1]);
                    content.DepthAdd();
                    _expressions[i + 1].ComputeValue(content);
                    break;
                }
            }

            content.DepthRemove();
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return CQ_Value.Null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            content.DepthAdd();
            ICQ_Expression expr_switch = _expressions[0] as ICQ_Expression;
            CQ_Value switchVal = CQ_Value.Null;
            //			CQ_Content.Value vrt = null;
            if(expr_switch != null)
                switchVal = expr_switch.ComputeValue(content);//switch//

            for(int i = 1; i < _expressions.Count - 1; i += 2) {
                if(_expressions[i] != null) {
                    //case xxx://
                    if(switchVal.GetObject().Equals(_expressions[i].ComputeValue(content).GetObject())) {
                        while(_expressions[i + 1] == null) {
                            i += 2;
                        }
                        content.DepthAdd();
                        if(_expressions[i + 1].hasCoroutine) {
                            yield return coroutine.StartCoroutine(_expressions[i + 1].CoroutineCompute(content, coroutine));
                        }
                        else {
                            _expressions[i + 1].ComputeValue(content);
                        }
                        break;
                    }
                    else {
                        continue;
                    }
                }
                else {
                    //default:
                    content.DepthAdd();
                    if(_expressions[i + 1].hasCoroutine) {
                        yield return coroutine.StartCoroutine(_expressions[i + 1].CoroutineCompute(content, coroutine));
                    }
                    else {
                        _expressions[i + 1].ComputeValue(content);
                    }
                    break;
                }
            }

            content.DepthRemove();
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
        }

        public override string ToString () {
            return "SwitchCase|";
        }
    }
}