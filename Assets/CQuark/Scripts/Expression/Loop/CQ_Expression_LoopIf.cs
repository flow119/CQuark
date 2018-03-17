using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_LoopIf : ICQ_Expression
    {
        public CQ_Expression_LoopIf(int tbegin, int tend, int lbegin, int lend)
        {
            _expressions = new List<ICQ_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            set;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> _expressions
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
				if(_expressions == null || _expressions.Count == 0)
					return false;
				foreach(ICQ_Expression expr in _expressions){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CQ_Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            ICQ_Expression expr_if = _expressions[0];
            bool bif = (bool)expr_if.ComputeValue(content).value;
            //if (expr_init != null) expr_init.ComputeValue(content);
            ICQ_Expression expr_go1 = _expressions[1];
            ICQ_Expression expr_go2 = null;
            if(_expressions.Count>2)expr_go2= _expressions[2];
            CQ_Value value = null;
            if (bif && expr_go1 != null)
            {
                if (expr_go1 is CQ_Expression_Block)
                {
                    value = expr_go1.ComputeValue(content);
                }
                else
                {
                    content.DepthAdd();
                    value = expr_go1.ComputeValue(content);
                    content.DepthRemove();
                }

            }
            else if (!bif && expr_go2 != null)
            {

                if (expr_go2 is CQ_Expression_Block)
                {
                    value = expr_go2.ComputeValue(content);
                }
                else
                {
                    content.DepthAdd();
                    value = expr_go2.ComputeValue(content);
                    content.DepthRemove();
                }

            }

            //while((bool)expr_continue.value);

            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);
            return value;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			ICQ_Expression expr_if = _expressions[0];
			bool bif = (bool)expr_if.ComputeValue(content).value;
			//if (expr_init != null) expr_init.ComputeValue(content);
			ICQ_Expression expr_go1 = _expressions[1];
			ICQ_Expression expr_go2 = null;
			if(_expressions.Count>2)expr_go2= _expressions[2];
//			CQ_Content.Value value = null;
			if (bif && expr_go1 != null)
			{
				if (expr_go1 is CQ_Expression_Block)
				{
					if(expr_go1.hasCoroutine){
						yield return coroutine.StartNewCoroutine(expr_go1.CoroutineCompute(content, coroutine));
					}else{
						expr_go1.ComputeValue(content);
					}
				}
				else
				{
					content.DepthAdd();
					if(expr_go1.hasCoroutine){
						yield return coroutine.StartNewCoroutine(expr_go1.CoroutineCompute(content, coroutine));
					}else{
						expr_go1.ComputeValue(content);
					}
					content.DepthRemove();
				}
				
			}
			else if (!bif && expr_go2 != null)
			{
				
				if (expr_go2 is CQ_Expression_Block)
				{
					if(expr_go2.hasCoroutine){
						yield return coroutine.StartNewCoroutine(expr_go2.CoroutineCompute(content, coroutine));
					}else{
						expr_go2.ComputeValue(content);
					}
				}
				else
				{
					content.DepthAdd();
					if(expr_go2.hasCoroutine){
						yield return coroutine.StartNewCoroutine(expr_go2.CoroutineCompute(content, coroutine));
					}else{
						expr_go2.ComputeValue(content);
					}
					content.DepthRemove();
				}
				
			}
			
			//while((bool)expr_continue.value);
			
			//for 逻辑
			//做数学计算
			//从上下文取值
			//_value = null;
			content.OutStack(this);
		}


        public override string ToString()
        {
            return "If|";
        }
    }
}