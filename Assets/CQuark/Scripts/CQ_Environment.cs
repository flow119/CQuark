using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    //环境 增加本地代码的管理
    //环境 增加运行中的表达式查询
    public class CQ_Environment 
    {
		Dictionary<CQType, ICQ_Type> types = new Dictionary<CQType, ICQ_Type>();
		Dictionary<string, ICQ_Type> typess = new Dictionary<string, ICQ_Type>();
		Dictionary<string, ICQ_Function> calls = new Dictionary<string, ICQ_Function>();
		Dictionary<string, ICQ_Function> corouts = new Dictionary<string, ICQ_Function>();
		//Dictionary<string, ICQ_Type_Dele> deleTypes = new Dictionary<string, ICQ_Type_Dele>();

        CQ_Expression_Compiler compiler = null;

        public CQ_Environment()
        {
            //if(useNamespace==true)
            //{
            //    throw new Exception("使用命名空间还不能完全兼容，建议关闭");
            //}

            //this.useNamespace = useNamespace;

            compiler = new CQ_Expression_Compiler();
            RegType(new CQ_Type_Int());
            RegType(new CQ_Type_UInt());
            RegType(new CQ_Type_Float());
            RegType(new CQ_Type_Double());
            RegType(new CQ_Type_String());
            RegType(new CQ_Type_Var());
            RegType(new CQ_Type_Bool());
            RegType(new CQ_Type_Lambda());
            RegType(new CQ_Type_Delegate());
            RegType(new CQ_Type_Byte());
            RegType(new CQ_Type_Char());
            RegType(new CQ_Type_UShort());
            RegType(new CQ_Type_Sbyte());
            RegType(new CQ_Type_Short());
            RegType(new CQ_Type_Long());
            RegType(new CQ_Type_ULong());
			RegType (typeof(IEnumerator), "IEnumerator");

            RegType(typeof(object), "object");

			RegType (typeof(List<>), "List");	//模板类要独立注册
			RegType (typeof(Dictionary<,>), "Dictionary");
			RegType (typeof(Stack<>), "Stack");
			RegType (typeof(Queue<>), "Queue");

            typess["null"] = new CQ_Type_NULL();
            //contentGloabl = CreateContent();
            //if (!useNamespace)//命名空间模式不能直接用函数
            {
                RegFunction(new FunctionTrace());
            }
        }
        //public bool useNamespace
        //{
        //    get;
        //    private set;
        //}

        
		public void RegType(Type type, string keyword)
		{
			RegType(RegHelper_Type.MakeType(type, keyword));
		}
        public void RegType(ICQ_Type type)
        {
            types[type.type] = type;

            string typename = type.keyword;
            //if (useNamespace)
            //{

            //    if (string.IsNullOrEmpty(type._namespace) == false)
            //    {
            //        typename = type._namespace + "." + type.keyword;
            //    }
            //}
            if (string.IsNullOrEmpty(typename))
            {//匿名自动注册
            }
            else
            {
                typess[typename] = type;
                CQ_TokenParser.AddType(typename);
            }
        }

        public ICQ_Type GetType(CQType type)
        {
            if (type == null)
                return typess["null"];

            ICQ_Type ret = null;
            if (types.TryGetValue(type, out ret) == false)
            {
                DebugUtil.LogWarning("(CQcript)类型未注册,将自动注册一份匿名:" + type.ToString());
                ret = RegHelper_Type.MakeType(type, "");
                RegType(ret);
            }
            return ret;
        }
        //public ICQ_Type_Dele GetDeleTypeBySign(string sign)
        //{
        //    if (deleTypes.ContainsKey(sign) == false)
        //    {
        //        return null;
        //        //logger.Log_Error("(CQcript)类型未注册:" + sign);

        //    }
        //    return deleTypes[sign];

        //}
        public ICQ_Type GetTypeByKeyword(string keyword)
        {
            ICQ_Type ret = null;
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }
            if (typess.TryGetValue(keyword, out ret) == false)
            {
                if (keyword[keyword.Length - 1] == '>')
                {
                    int iis = keyword.IndexOf('<');
                    string func = keyword.Substring(0, iis);
                    List<string> _types = new List<string>();
                    int istart = iis + 1;
                    int inow = istart;
                    int dep = 0;
                    while (inow < keyword.Length)
                    {
                        if (keyword[inow] == '<')
                        {
                            dep++;
                        }
                        if (keyword[inow] == '>')
                        {
                            dep--;
                            if (dep < 0)
                            {
                                _types.Add(keyword.Substring(istart, inow - istart));
                                break;
                            }
                        }

                        if (keyword[inow] == ',' && dep == 0)
                        {
                            _types.Add(keyword.Substring(istart, inow - istart));
                            istart = inow + 1;
                            inow = istart;
                            continue; ;
                        }

                        inow++;
                    }

                    //var funk = keyword.Split(new char[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (typess.ContainsKey(func))
                    {
                        Type gentype = GetTypeByKeyword(func).type;
                        if (gentype.IsGenericTypeDefinition)
                        {
                            Type[] types = new Type[_types.Count];
                            for (int i = 0; i < types.Length; i++)
                            {
                                CQType t = GetTypeByKeyword(_types[i]).type;
                                Type rt = t;
                                if (rt == null && t != null)
                                {
                                    rt = typeof(object);
                                }
                                types[i] = rt;
                            }
                            Type IType = gentype.MakeGenericType(types);
                            RegType(CQuark.RegHelper_Type.MakeType(IType, keyword));
                            return GetTypeByKeyword(keyword);
                        }
                    }
                }
                DebugUtil.LogError("(CQcript)类型未注册:" + keyword);
            }

            return ret;
        }
        public ICQ_Type GetTypeByKeywordQuiet(string keyword)
        {
            ICQ_Type ret = null;
            if (typess.TryGetValue(keyword, out ret) == false)
            {
                return null;
            }
            return ret;
        }
        public void RegFunction(ICQ_Function func)
        {
            //if (useNamespace)
            //{
            //    throw new Exception("用命名空间时不能直接使用函数，必须直接定义在类里");
            //}
			if (func.returntype == typeof(IEnumerator))
				corouts [func.keyword] = func;
			else
				calls[func.keyword] = func;
        }
		public void RegFunction(Delegate dele)
		{
			RegFunction(new RegHelper_Function (dele));
		}

        public ICQ_Function GetFunction(string name)
        {
            ICQ_Function func = null;
            calls.TryGetValue(name, out func);
            if (func == null)
            {
				corouts.TryGetValue (name, out func);
				if(func == null)
	                throw new Exception("找不到函数:" + name);
            }
            return func;
        }
			
		//是否是一个协程方法
		public bool IsCoroutine(string name){
			return corouts.ContainsKey (name);
		}

		public IList<Token> ParserToken(string code)
		{
			if (code [0] == 0xFEFF) {
				//windows下用记事本写，会在文本第一个字符出现BOM（65279）
				code = code.Substring(1);
			}

			IList<Token> tokens = CQ_TokenParser.Parse(code);
			if (tokens == null)
				DebugUtil.LogWarning ("没有解析到代码");

			return tokens;
		}
        public ICQ_Expression Expr_CompileToken(IList<Token> listToken)
        {
            return compiler.Compile(listToken, this);
        }
        public ICQ_Expression Expr_CompilerToken(IList<Token> listToken)
        {
            return compiler.Compile(listToken, this);
        }
        public ICQ_Expression Expr_CompileToken(IList<Token> listToken, bool SimpleExpression)
        {
            return SimpleExpression ? compiler.Compile_NoBlock(listToken, this) : compiler.Compile(listToken, this);
        }
        public ICQ_Expression Expr_CompilerToken(IList<Token> listToken, bool SimpleExpression)
        {
            return SimpleExpression ? compiler.Compile_NoBlock(listToken, this) : compiler.Compile(listToken, this);
        }
        //CQ_Content contentGloabl = null;
        public ICQ_Expression Expr_Optimize(ICQ_Expression old)
        {
            return compiler.Optimize(old, this);
        }
        public CQ_Content CreateContent()
        {
            return new CQ_Content(this, true);
        }

        public CQ_Content.Value Expr_Execute(ICQ_Expression expr)
        {
            CQ_Content content = CreateContent();
            return expr.ComputeValue(content);
        }
        public CQ_Content.Value Expr_Execute(ICQ_Expression expr, CQ_Content content)
        {
            if (content == null) content = CreateContent();
            return expr.ComputeValue(content);
        }
		public IEnumerator Expr_Coroutine(ICQ_Expression expr, CQ_Content content, ICoroutine coroutine)
		{
			if (content == null)
				content = CreateContent();
			yield return coroutine.StartNewCoroutine(expr.CoroutineCompute(content, coroutine));
		}

        public void Project_Compile(Dictionary<string, IList<Token>> project, bool embDebugToken)
        {
            foreach (KeyValuePair<string, IList<Token>> f in project)
            {
                File_PreCompileToken(f.Key, f.Value);
            }
            foreach (KeyValuePair<string, IList<Token>> f in project)
            {
                //预处理符号
                for (int i = 0; i < f.Value.Count; i++)
                {
                    if (f.Value[i].type == TokenType.IDENTIFIER && CQ_TokenParser.ContainsType(f.Value[i].text))
                    {//有可能预处理导致新的类型
                        if (i > 0
                            &&
                            (f.Value[i - 1].type == TokenType.TYPE || f.Value[i - 1].text == "."))
                        {
                            continue;
                        }
                        Token rp = f.Value[i];
                        rp.type = TokenType.TYPE;
                        f.Value[i] = rp;
                    }
                }
                File_CompileToken(f.Key, f.Value, embDebugToken);
            }
        }
        public void File_PreCompileToken(string filename, IList<Token> listToken)
        {
            IList<ICQ_Type> types = compiler.FilePreCompile(this, filename, listToken);
            foreach (var type in types)
            {
                this.RegType(type);
            }
        }
        public void File_CompileToken(string filename, IList<Token> listToken, bool embDebugToken)
        {
            DebugUtil.Log("File_CompilerToken:" + filename);
            IList<ICQ_Type> types = compiler.FileCompile(this, filename, listToken, embDebugToken);
            foreach (var type in types)
            {
                if (this.GetTypeByKeywordQuiet(type.keyword) == null)
                    this.RegType(type);
            }
        }
    }
}
