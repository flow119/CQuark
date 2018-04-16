using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
    /// <summary>
    /// 相当于CQ_Value的集合
    /// </summary>
    public class CQ_Content {
        public Class_CQuark CallType;
        public CQClassInstance CallThis;
        //由于CQ_Content会频繁创建，而很多时候不需要values，所以lazy一下，只在使用时构造Stack和Dictionary

        Stack<string> tvalues;  //所有values的名字
        Stack<int> tvalueDepth; //每一层作用域里变量的数量
        public Dictionary<string, CQ_Value> values;//所有变量暂存

#if CQUARK_DEBUG
        private Stack<ICQ_Expression> stackExpr = new Stack<ICQ_Expression>();
        private Stack<CQ_Content> stackContent = new Stack<CQ_Content>();
        public string function;
#endif
        public string CallName {
            get {
                string strout = "";
                if(this.CallType != null) {
                    if(!string.IsNullOrEmpty(this.CallType.filename))
                        strout += "(" + this.CallType.filename + ")";
                    strout += this.CallType.Name + ":";
                }
#if CQUARK_DEBUG
                strout += this.function;
#endif
                return strout;
            }
        }


        public CQ_Content Clone () {
            CQ_Content con = new CQ_Content();
            if(values != null) {
                foreach(var c in this.values) {
                    con.values.Add(c.Key, c.Value);
                }
            }

            con.CallThis = this.CallThis;
            con.CallType = this.CallType;

            return con;
        }
#if CQUARK_DEBUG      
        public void InStack(CQ_Content expr)
        {

            if (stackContent.Count > 0 && stackContent.Peek() == expr)
            {
                throw new Exception("InStackContent error");
            }
            stackContent.Push(expr);
        }
        public void OutStack(CQ_Content expr)
        {        
            if (stackContent.Peek() != expr)
            {
                throw new Exception("OutStackContent error:" + expr.ToString() + " err:" + stackContent.Peek().ToString());
            }
            stackContent.Pop();
        }
        public void InStack(ICQ_Expression expr)
        {       
            if (stackExpr.Count > 0 && stackExpr.Peek() == expr)
            {
                throw new Exception("InStack error");
            }
            stackExpr.Push(expr);
        }
        public void OutStack(ICQ_Expression expr)
        {       
			if (stackExpr.Peek() != expr)
            {
				throw new Exception("OutStack error:" + expr.ToString() + " err:" + stackExpr.Peek().ToString());

            }
            stackExpr.Pop();
        }
#endif
        public int Record () {
            return tvalueDepth.Peek();
        }
        public void Restore (int depthCount, ICQ_Expression expr) {
            int newCount = tvalueDepth.Pop();
            tvalueDepth.Push(depthCount);
            int needRemove = newCount - depthCount;
            for(; needRemove > 0; needRemove--) {
                string name = tvalues.Pop();
                values.Remove(name);
            }

#if CQUARK_DEBUG
            while(stackExpr.Peek()!=expr)
            {
                stackExpr.Pop();
            }
#endif
        }
        public string DumpValue () {
            string strvalues = "";
#if CQUARK_DEBUG
            if (this.stackContent != null)
            {
                foreach (var subc in this.stackContent)
                {
                    strvalues += subc.DumpValue();
                }
            }
#endif
            strvalues += "DumpValue:" + this.CallName + "\n";
            if(values != null) {
                foreach(var v in this.values) {
                    strvalues += "V:" + v.Key + "=" + v.Value.ToString() + "\n";
                }
            }

            return strvalues;
        }
        public string DumpStack (IList<Token> tokenlist) {
            string strvalues = "";
#if CQUARK_DEBUG
                if(this.CallType!=null&&this.CallType.tokenlist!=null)
                {
                    tokenlist = this.CallType.tokenlist;
                }
                foreach(var subc in this.stackContent)
                {
                    strvalues += subc.DumpStack(tokenlist);
                }
                strvalues += "DumpStack:" + this.CallName + "\n";
                foreach(var s in stackExpr)
                {
                    if ((s.tokenBegin == 0 && s.tokenEnd == 0)||tokenlist==null)
                    {
                        strvalues += "<CQuark>:line(" + s.lineBegin + "-" + s.lineEnd + ")\n";
                    }
                    else
                    {
                        strvalues += "<CQuark>:line(" + s.lineBegin + "-" + s.lineEnd + ")";
                        
                        if (s.tokenEnd - s.tokenBegin >= 20)
                        {
                            for(int i=s.tokenBegin;i<s.tokenBegin+8;i++)
                            {
                                strvalues += tokenlist[i].text + " ";
                            }
                            strvalues += "...";
                            for (int i = s.tokenEnd-7; i <= s.tokenEnd; i++)
                            {
                                strvalues += tokenlist[i].text + " ";
                            }
                        }
                        else
                        {
                            for (int i = s.tokenBegin; i <= s.tokenEnd; i++)
                            {
                                strvalues += tokenlist[i].text + " ";
                            }
                        }
                        strvalues += "\n";

                    }
                   
                }
#endif
            return strvalues;
        }

        public string Dump () {
            string str = DumpValue();
            str += DumpStack(null);
            return str;
        }
        public string Dump (IList<Token> tokenlist) {
            string str = DumpValue();
            str += DumpStack(tokenlist);
            return str;
        }


        public void Define (string name, CQ_Type type) {
            if(values == null) {
                values = new Dictionary<string, CQ_Value>();
            }
            else if(values.ContainsKey(name)) {
                throw new Exception("已经定义过");
            }

            CQ_Value v = new CQ_Value();
            v.SetCQType(type);
            values[name] = v;

            int newdepth = tvalueDepth.Pop() + 1;
            tvalueDepth.Push(newdepth);
            tvalues.Push(name);
        }

        public void Set (string name, object value) {
            if(values == null) {
                values = new Dictionary<string, CQ_Value>();
            }
            CQ_Value retV = CQ_Value.Null;

            bool bFind = values.TryGetValue(name, out retV);
            if(!bFind) {
                if(CallType != null) {
                    Class_CQuark.Member retM = null;
                    bool bRet = CallType.members.TryGetValue(name, out retM);
                    if(bRet) {
                        if(retM.bStatic) {
                            CQ_Value val = CallType.staticMemberInstance[name];
                            val.value = value;
                            CallType.staticMemberInstance[name] = val;
                        }
                        else {
                            CQ_Value val = CallThis.member[name];
                            val.value = value;
                            CallThis.member[name] = val;
                        }
                        return;
                    }

                }
                string err = CallType.Name + "\n";
                foreach(var m in CallType.members) {
                    err += m.Key + ",";
                }
                throw new Exception("值没有定义过" + name + "," + err);

            }
            else {
                if(retV.m_type == typeof(Type_Var.var) && value != null)
                    retV.m_type = value.GetType();
                retV.value = value;
                values[name] = retV;
            }
        }
        public void DefineAndSet (string name, CQ_Type type, object value) {
            if(values == null) {
                values = new Dictionary<string, CQ_Value>();
            }
            else if(values.ContainsKey(name)) {
                throw new Exception(type.ToString() + ":" + name + "已经定义过");
            }

            CQ_Value v = new CQ_Value();
            v.SetCQType(type);
            v.value = value;
            values[name] = v;

            int newdepth = tvalueDepth.Pop() + 1;
            tvalueDepth.Push(newdepth);
            tvalues.Push(name);
        }
        public CQ_Value Get (string name) {
            CQ_Value v = GetQuiet(name);
            if(v == CQ_Value.Null)
                throw new Exception("值" + name + "没有定义过");
            return v;
        }
        public CQ_Value GetQuiet (string name) {
            CQ_Value retV = new CQ_Value();
            if(name == "this") {
                retV.m_stype = CallType;
                retV.value = CallThis;
                return retV;
            }

            if(values != null) {
                if(values.TryGetValue(name, out retV))//优先上下文变量
                    return retV;
            }

            if(CallType != null) {
                Class_CQuark.Member retM = null;
                if(CallType.members.TryGetValue(name, out retM)) {
                    if(retM.bStatic) {
                        return CallType.staticMemberInstance[name];
                    }
                    else {
                        return CallThis.member[name];
                    }
                }
                if(CallType.functions.ContainsKey(name)) {
                    //如果直接得到代理实例，
                    DeleFunction dele = new DeleFunction(CallType, this.CallThis, name);
                    retV.value = dele;
                    retV.m_type = typeof(DeleFunction);
                    return retV;

                }
            }
            return retV;//null
        }

        public void DepthAdd ()//控制变量作用域，深一层
        {
            if(tvalues == null)
                tvalues = new Stack<string>();
            if(tvalueDepth == null)
                tvalueDepth = new Stack<int>();

            tvalueDepth.Push(0);
        }
        public void DepthRemove ()//控制变量作用域，退出一层，上一层的变量都清除
        {
            if(tvalues == null)
                return;
            int depth = tvalueDepth.Pop();
            for(; depth > 0; depth--) {
                string name = tvalues.Pop();
                values.Remove(name);
            }
        }
    }
}
