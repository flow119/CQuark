using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark {

    public class CQ_Expression_LoopWhile : ICQ_Expression {
        public CQ_Expression_LoopWhile (int tbegin, int tend, int lbegin, int lend) {
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
            ICQ_Expression expr_while = _expressions[0] as ICQ_Expression;
            ICQ_Expression expr_block = _expressions[1] as ICQ_Expression;
            CQ_Value vrt = CQ_Value.Null;
            while((bool)expr_while.ComputeValue(content).GetValue()) {
                if(expr_block != null) {
                    if(expr_block is CQ_Expression_Block) {
                        var v = expr_block.ComputeValue(content);
                        if(v != CQ_Value.Null) {
							if(v.m_breakBlock == BreakType.Return)
								vrt = v;
							if(v.m_breakBlock == BreakType.Return || v.m_breakBlock == BreakType.Break) 
								break;
                        }
                    }
                    else {
                        content.DepthAdd();
                        bool bbreak = false;
                        var v = expr_block.ComputeValue(content);
                        if(v != CQ_Value.Null) {
							if(v.m_breakBlock == BreakType.Return) 
								vrt = v;
							if(v.m_breakBlock == BreakType.Return || v.m_breakBlock == BreakType.Break)
								bbreak = true;
                        }
                        content.DepthRemove();
                        if(bbreak) break;
                    }
                    //if (v.breakBlock == 1) continue;
                    //if (v.breakBlock == 2) break;
                    //if (v.breakBlock == 10) return v;
                }
            }
            content.DepthRemove();
#if CQUARK_DEBUG
            content.OutStack(this);
#endif
            return vrt;
            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
        }
        public IEnumerator CoroutineCompute (CQ_Content content, UnityEngine.MonoBehaviour coroutine) {
#if CQUARK_DEBUG
			content.InStack(this);
#endif
            content.DepthAdd();
            ICQ_Expression expr_while = _expressions[0] as ICQ_Expression;
            ICQ_Expression expr_block = _expressions[1] as ICQ_Expression;
            //			CQ_Content.Value vrt = null;
            while((bool)expr_while.ComputeValue(content).GetValue()) {
                if(expr_block != null) {
                    if(expr_block is CQ_Expression_Block) {
                        if(expr_block.hasCoroutine) {
                            yield return coroutine.StartCoroutine(expr_block.CoroutineCompute(content, coroutine));
                        }
                        else {
                            var v = expr_block.ComputeValue(content);
                            if(v != CQ_Value.Null) {
                                //								if (v.breakBlock > 2) vrt = v;
								if(v.m_breakBlock == BreakType.Break || v.m_breakBlock == BreakType.Return)
									break;
                            }
                        }
                    }
                    else {
                        content.DepthAdd();
                        if(expr_block.hasCoroutine) {
                            yield return coroutine.StartCoroutine(expr_block.CoroutineCompute(content, coroutine));
                        }
                        else {
                            bool bbreak = false;
                            var v = expr_block.ComputeValue(content);
                            if(v != CQ_Value.Null) {
                                //								if (v.breakBlock > 2) vrt = v;
								if(v.m_breakBlock == BreakType.Break || v.m_breakBlock == BreakType.Return) bbreak = true;
                            }
                            content.DepthRemove();
                            if(bbreak) break;
                        }
                    }
                    //if (v.breakBlock == 1) continue;
                    //if (v.breakBlock == 2) break;
                    //if (v.breakBlock == 10) return v;
                }
            }
            content.DepthRemove();
#if CQUARK_DEBUG
			content.OutStack(this);
#endif
            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
        }

        public override string ToString () {
            return "While|";
        }
    }
}