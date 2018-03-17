using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
	//一个西瓜Class
    public class CQ_Content
    {
		public Class_CQuark CallType;
		public Stack<List<string>> tvalues = new Stack<List<string>>();
		public SInstance CallThis;
		public Dictionary<string, CQ_Value> values = new Dictionary<string, CQ_Value>();

        public CQ_Content()
        {
            this.useDebug = false;
        }
        public CQ_Content(bool useDebug)
        {
            this.useDebug = useDebug;
            if (useDebug)
            {
                stackExpr = new Stack<ICQ_Expression>();
                stackContent = new Stack<CQ_Content>();
            }
        }

        public CQ_Content Clone()
        {
            CQ_Content con = new CQ_Content(useDebug);
            foreach(var c in this.values)
            {
                con.values.Add(c.Key, c.Value);
            }
            con.CallThis = this.CallThis;
            con.CallType = this.CallType;

            return con;
        }
        
        public string function
        {
            get;
            set;
        }
        public string CallName
        {
            get
            {
                string strout = "";
                if (this.CallType != null)
                {
                    if (string.IsNullOrEmpty(this.CallType.filename) == false)
                        strout += "(" + this.CallType.filename + ")";
                    strout += this.CallType.Name + ":";
                }
                strout += this.function;
                return strout;
            }

        }
        public bool useDebug
        {
            get;
            private set;
        }
        public Stack<ICQ_Expression> stackExpr
        {
            get;
            private set;
        }
        public Stack<CQ_Content> stackContent
        {
            get;
            private set;
        }
        public void InStack(CQ_Content expr)
        {
            if (!useDebug) return;
            if (stackContent.Count > 0 && stackContent.Peek() == expr)
            {
                throw new Exception("InStackContent error");
            }
            stackContent.Push(expr);
        }
        public void OutStack(CQ_Content expr)
        {
            if (!useDebug) return;
            if (stackContent.Peek() != expr)
            {
                throw new Exception("OutStackContent error:" + expr.ToString() + " err:" + stackContent.Peek().ToString());
            }
            stackContent.Pop();
        }
        public void InStack(ICQ_Expression expr)
        {
            if (!useDebug) return;
            if (stackExpr.Count > 0 && stackExpr.Peek() == expr)
            {
                throw new Exception("InStack error");
            }
            stackExpr.Push(expr);
        }
        public void OutStack(ICQ_Expression expr)
        {
            if (!useDebug) return;

			if (stackExpr.Peek() != expr)
            {
				if (expr.hasCoroutine) {
					DepthRemove ();
				}else {
					throw new Exception("OutStack error:" + expr.ToString() + " err:" + stackExpr.Peek().ToString());
				}
            }
            stackExpr.Pop();
        }
        public void Record(out List<string> depth)
        {
            depth = tvalues.Peek();
        }
        public void Restore(List<string> depth, ICQ_Expression expr)
        {
            while(tvalues.Peek()!=depth)
            {
                tvalues.Pop();
            }
            while(stackExpr.Peek()!=expr)
            {
                stackExpr.Pop();
            }
        }
		public string DumpValue()
		{
			string svalues = "";
            if (this.stackContent != null)
            {
                foreach (var subc in this.stackContent)
                {
                    svalues += subc.DumpValue();
                }
            }
            svalues += "DumpValue:" + this.CallName + "\n";
            foreach(var v in this.values)
            {
                svalues += "V:" + v.Key + "=" + v.Value.ToString()+"\n";
            }
			return svalues;
		}
		public string DumpStack(IList<Token> tokenlist)
        {
			string svalues = "";
            if (useDebug)
            {
                if(this.CallType!=null&&this.CallType.tokenlist!=null)
                {
                    tokenlist = this.CallType.tokenlist;
                }
                foreach(var subc in this.stackContent)
                {
                    svalues += subc.DumpStack(tokenlist);
                }
                svalues += "DumpStack:" + this.CallName + "\n";
                foreach(var s in stackExpr)
                {
                    if ((s.tokenBegin == 0 && s.tokenEnd == 0)||tokenlist==null)
                    {
						svalues += "<CQuark>:line(" + s.lineBegin + "-" + s.lineEnd + ")\n";
                    }
                    else
                    {
						svalues += "<CQuark>:line(" + s.lineBegin + "-" + s.lineEnd + ")";
                        
                        if (s.tokenEnd - s.tokenBegin >= 20)
                        {
                            for(int i=s.tokenBegin;i<s.tokenBegin+8;i++)
                            {
                                svalues += tokenlist[i].text + " ";
                            }
                            svalues += "...";
                            for (int i = s.tokenEnd-7; i <= s.tokenEnd; i++)
                            {
                                svalues += tokenlist[i].text + " ";
                            }
                        }
                        else
                        {
                            for (int i = s.tokenBegin; i <= s.tokenEnd; i++)
                            {
                                svalues += tokenlist[i].text + " ";
                            }
                        }
                        svalues += "\n";

                    }
                   
                }
            }
            return svalues;

        }
        public string Dump()
        {
            string str = DumpValue();
            str += DumpStack(null);
            return str;
        }
		public string Dump(IList<Token> tokenlist)
		{
			string str = DumpValue();
			str += DumpStack(tokenlist);
			return str;
		}
        

        public void Define(string name,TypeBridge type)
        {
            if (values.ContainsKey(name))
				throw new Exception("已经定义过");
            CQ_Value v = new CQ_Value();
            v.type = type;
            values[name] = v;
            if (tvalues.Count > 0)
            {
                tvalues.Peek().Add(name);//暂存临时变量
            }
        }
        public void Set(string name,object value)
        {
            CQ_Value retV = null;
            bool bFind = values.TryGetValue(name, out retV);
            if (!bFind)
            {
                if (CallType != null)
                {
                    Class_CQuark.Member retM = null;
                    bool bRet = CallType.members.TryGetValue(name, out retM);
                    if (bRet)
                    {
                        if (retM.bStatic)
                        {
                            CallType.staticMemberInstance[name].value=value;
                        }
                        else
                        {
                            CallThis.member[name].value=value;
                        }
                        return;
                    }

                }
                string err = CallType.Name + "\n";
                foreach(var m in CallType.members)
                {
                    err += m.Key + ",";
                }
                throw new Exception("值没有定义过" + name + "," + err);

            }
            if ((Type)retV.type == typeof(Type_Var.var) && value != null)
                retV.type = value.GetType();
            retV.value = value;
        }
        public void DefineAndSet(string name,TypeBridge type,object value)
        {
            if (values.ContainsKey(name)) 
                throw new Exception(type.ToString()+":"+name+"已经定义过");
            CQ_Value v = new CQ_Value();
            v.type = type;
            v.value = value;
            values[name] = v;
            if(tvalues.Count>0)
            {
                tvalues.Peek().Add(name);//暂存临时变量
            }
        }
        public CQ_Value Get(string name)
        {
            CQ_Value v = GetQuiet(name);
            if(v==null)
                throw new Exception("值"+name+"没有定义过");
            return v;
        }
        public CQ_Value GetQuiet(string name)
        {
            if (name == "this")
            {
                CQ_Value v = new CQ_Value();
                v.type = CallType;
                v.value = CallThis;
                return v;
            }

            CQ_Value retV = null;
            bool bFind = values.TryGetValue(name, out retV);
            if (bFind)//优先上下文变量
                return retV;

            if (CallType != null)
            {
                Class_CQuark.Member retM = null;
                bFind = CallType.members.TryGetValue(name, out retM);
                if (bFind)
                {
                    if (retM.bStatic)
                    {
                        return CallType.staticMemberInstance[name];
                    }
                    else
                    {
                        return CallThis.member[name];
                    }
                }
                if (CallType.functions.ContainsKey(name))
                {
                    CQ_Value v = new CQ_Value();
                    //如果直接得到代理实例，
                    DeleFunction dele = new DeleFunction(CallType,this.CallThis,name);


                    //DeleScript dele =new DeleScript();
                    //dele.function = name;
                    //dele.calltype = CallType;
                    //dele.callthis = CallThis;
                    v.value = dele;
                    v.type = typeof(DeleFunction);
                    return v;

                }
            }
            return null;
        }

        public void DepthAdd()//控制变量作用域，深一层
        {
            tvalues.Push(new List<string>());
        }
        public void DepthRemove()//控制变量作用域，退出一层，上一层的变量都清除
        {
			if (tvalues.Count == 0)
				return;
            List<string> list = tvalues.Pop();
            foreach(var v in list)
            {
                values.Remove(v);
            }
        }
    }
}
