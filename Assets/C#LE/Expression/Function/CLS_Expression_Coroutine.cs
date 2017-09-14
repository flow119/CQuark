using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{
	public class CLS_Expression_Coroutine : ICLS_Expression
    {
		public CLS_Expression_Coroutine(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
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
		public bool hasCoroutine
		{
			get{
				return true;
			}
		}
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            List<CLS_Content.Value> list = new List<CLS_Content.Value>();
            foreach (ICLS_Expression p in listParam)
            {
                if (p != null)
                {
                    list.Add(p.ComputeValue(content));
                }
            }
			UnityEngine.Debug.Log("YieldWaitForSecond");
			if(funcname == "YieldWaitForSecond"){

				content.OutStack(this);
				return null;
			}else{
	            CLS_Content.Value v = null;

	            SType.Function retFunc = null;
	            bool bFind = false;
	            if (content.CallType != null)
	                bFind = content.CallType.functions.TryGetValue(funcname, out retFunc);

	            if (bFind)
	            {
	                if (retFunc.bStatic)
	                {
	                    v = content.CallType.StaticCall(content, funcname, list);

	                }
	                else
	                {
	                    v = content.CallType.MemberCall(content, content.CallThis, funcname, list);

	                }
	            }


	            else
	            {
	                v = content.GetQuiet(funcname);
	                if (v != null && v.value is Delegate)
	                {
	                    //if(v.value is Delegate)
	                    {
	                        Delegate d = v.value as Delegate;
	                        v = new CLS_Content.Value();
	                        object[] obja = new object[list.Count];
	                        for (int i = 0; i < list.Count; i++)
	                        {
	                            obja[i] = list[i].value;
	                        }
	                        v.value = d.DynamicInvoke(obja);
	                        if (v.value == null)
	                        {
	                            v.type = null;
	                        }
	                        else
	                        {
	                            v.type = v.value.GetType();
	                        }
	                    }
	                    //else
	                    //{
	                    //    throw new Exception(funcname + "不是函数");
	                    //}
	                }
	                else
	                {
	                    v = content.environment.GetFunction(funcname).Call(content, list);
	                }
	            }
	            //操作变量之
	            //做数学计算
	            //从上下文取值
	            //_value = null;
	            content.OutStack(this);
	            return v;
			}
        }

		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			List<CLS_Content.Value> list = new List<CLS_Content.Value>();
			foreach (ICLS_Expression p in listParam)
			{
				if (p != null)
				{
					//暂时不支持协程套用协程
					list.Add(p.ComputeValue(content));
				}
			}
			if(funcname == "YieldWaitForSecond"){
				if(list[0].type.Name == "Int32"){
					int delay = (int)list[0].value;
					yield return coroutine.WaitForSecond((float)delay);
				}else if(list[0].type.Name == "Single"){
					float delay = (float)list[0].value;
					yield return coroutine.WaitForSecond(delay);
				}else if(list[0].type.Name == "Double"){
					double delay = (double)list[0].value;
					yield return coroutine.WaitForSecond((float)delay);
				}else{
					//Unknow Number Type
				}

				content.OutStack(this);
			}else{
				CLS_Content.Value v = null;
				
				SType.Function retFunc = null;
				bool bFind = false;
				if (content.CallType != null)
					bFind = content.CallType.functions.TryGetValue(funcname, out retFunc);
				
				if (bFind)
				{
					if (retFunc.bStatic)
					{
						v = content.CallType.StaticCall(content, funcname, list);
						
					}
					else
					{
						v = content.CallType.MemberCall(content, content.CallThis, funcname, list);
						
					}
				}
				
				
				else
				{
					v = content.GetQuiet(funcname);
					if (v != null && v.value is Delegate)
					{
						//if(v.value is Delegate)
						{
							Delegate d = v.value as Delegate;
							v = new CLS_Content.Value();
							object[] obja = new object[list.Count];
							for (int i = 0; i < list.Count; i++)
							{
								obja[i] = list[i].value;
							}
							v.value = d.DynamicInvoke(obja);
							if (v.value == null)
							{
								v.type = null;
							}
							else
							{
								v.type = v.value.GetType();
							}
						}
						//else
						//{
						//    throw new Exception(funcname + "不是函数");
						//}
					}
					else
					{
						v = content.environment.GetFunction(funcname).Call(content, list);
					}
				}
				//操作变量之
				//做数学计算
				//从上下文取值
				//_value = null;
				content.OutStack(this);
			}
		}
        public string funcname;

        public override string ToString()
        {

            return "Call|" + funcname + "(params[" + listParam.Count + ")";
        }
    }
}