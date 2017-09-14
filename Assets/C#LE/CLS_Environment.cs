/// C#Light/Evil
/// 原作者 疯光无限 版本见ICLS_Environment.version
/// https://github.com/lightszero/CSLightStudio
/// http://crazylights.cnblogs.com
/// 
/// CQuark
/// https://github.com/flow119/CQuark
/// 请勿删除此声明
using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    //环境 增加本地代码的管理
    //环境 增加运行中的表达式查询
    public class CLS_Environment : ICLS_Environment, ICLS_Environment_Compiler
    {

        public string version
        {
            get
            {
                return "0.7.0";
            }
        }
        public CLS_Environment(ICLS_Logger logger)
        {
            //if(useNamespace==true)
            //{
            //    throw new Exception("使用命名空间还不能完全兼容，建议关闭");
            //}
            this.logger = logger;
            //this.useNamespace = useNamespace;
            tokenParser = new CLS_TokenParser();
            compiler = new CLS_Expression_Compiler(logger);
            RegType(new CLS_Type_Int());
            RegType(new CLS_Type_UInt());
            RegType(new CLS_Type_Float());
            RegType(new CLS_Type_Double());
            RegType(new CLS_Type_String());
            RegType(new CLS_Type_Var());
            RegType(new CLS_Type_Bool());
            RegType(new CLS_Type_Lambda());
            RegType(new CLS_Type_Delegate());
            RegType(new CLS_Type_Byte());
            RegType(new CLS_Type_Char());
            RegType(new CLS_Type_UShort());
            RegType(new CLS_Type_Sbyte());
            RegType(new CLS_Type_Short());
            RegType(new CLS_Type_Long());
            RegType(new CLS_Type_ULong());

            RegType(RegHelper_Type.MakeType(typeof(object), "object"));
            RegType(RegHelper_Type.MakeType(typeof(List<>), "List"));
            RegType(RegHelper_Type.MakeType(typeof(Dictionary<,>), "Dictionary"));

            typess["null"] = new CLS_Type_NULL();
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

        Dictionary<CLType, ICLS_Type> types = new Dictionary<CLType, ICLS_Type>();
        Dictionary<string, ICLS_Type> typess = new Dictionary<string, ICLS_Type>();
        Dictionary<string, ICLS_Function> calls = new Dictionary<string, ICLS_Function>();
        //Dictionary<string, ICLS_Type_Dele> deleTypes = new Dictionary<string, ICLS_Type_Dele>();
        public void RegType(ICLS_Type type)
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
                if (tokenParser.types.Contains(typename) == false)
                {
                    tokenParser.types.Add(typename);
                }
            }
        }

        public ICLS_Type GetType(CLType type)
        {
            if (type == null)
                return typess["null"];

            ICLS_Type ret = null;
            if (types.TryGetValue(type, out ret) == false)
            {
                logger.Log_Warn("(CLScript)类型未注册,将自动注册一份匿名:" + type.ToString());
                ret = RegHelper_Type.MakeType(type, "");
                RegType(ret);
            }
            return ret;
        }
        //public ICLS_Type_Dele GetDeleTypeBySign(string sign)
        //{
        //    if (deleTypes.ContainsKey(sign) == false)
        //    {
        //        return null;
        //        //logger.Log_Error("(CLScript)类型未注册:" + sign);

        //    }
        //    return deleTypes[sign];

        //}
        public ICLS_Type GetTypeByKeyword(string keyword)
        {
            ICLS_Type ret = null;
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
                                CLType t = GetTypeByKeyword(_types[i]).type;
                                Type rt = t;
                                if (rt == null && t != null)
                                {
                                    rt = typeof(object);
                                }
                                types[i] = rt;
                            }
                            Type IType = gentype.MakeGenericType(types);
                            RegType(CSLE.RegHelper_Type.MakeType(IType, keyword));
                            return GetTypeByKeyword(keyword);
                        }

                    }
                }
                logger.Log_Error("(CLScript)类型未注册:" + keyword);

            }

            return ret;
        }
        public ICLS_Type GetTypeByKeywordQuiet(string keyword)
        {
            ICLS_Type ret = null;
            if (typess.TryGetValue(keyword, out ret) == false)
            {
                return null;
            }
            return ret;
        }
        public void RegFunction(ICLS_Function func)
        {
            //if (useNamespace)
            //{
            //    throw new Exception("用命名空间时不能直接使用函数，必须直接定义在类里");
            //}
            calls[func.keyword] = func;
        }
        public ICLS_Function GetFunction(string name)
        {
            ICLS_Function func = null;
            calls.TryGetValue(name, out func);
            if (func == null)
            {
                throw new Exception("找不到函数:" + name);
            }
            return func;
        }
        public ICLS_Logger logger
        {
            get;
            private set;
        }
        //public ICLS_Debugger debugger;
        public ICLS_TokenParser tokenParser
        {
            get;
            private set;
        }
        ICLS_Expression_Compiler compiler = null;
        public IList<Token> ParserToken(string code)
        {
            return tokenParser.Parse(code);
        }
        public ICLS_Expression Expr_CompileToken(IList<Token> listToken)
        {
            return compiler.Compile(listToken, this);
        }
        public ICLS_Expression Expr_CompilerToken(IList<Token> listToken)
        {
            return compiler.Compile(listToken, this);
        }
        public ICLS_Expression Expr_CompileToken(IList<Token> listToken, bool SimpleExpression)
        {
            return SimpleExpression ? compiler.Compile_NoBlock(listToken, this) : compiler.Compile(listToken, this);
        }
        public ICLS_Expression Expr_CompilerToken(IList<Token> listToken, bool SimpleExpression)
        {
            return SimpleExpression ? compiler.Compile_NoBlock(listToken, this) : compiler.Compile(listToken, this);
        }
        //CLS_Content contentGloabl = null;
        public ICLS_Expression Expr_Optimize(ICLS_Expression old)
        {
            return compiler.Optimize(old, this);
        }
        public CLS_Content CreateContent()
        {
            return new CLS_Content(this, true);
        }

        public CLS_Content.Value Expr_Execute(ICLS_Expression expr)
        {
            CLS_Content content = CreateContent();
            return expr.ComputeValue(content);
        }
        public CLS_Content.Value Expr_Execute(ICLS_Expression expr, CLS_Content content)
        {
            if (content == null) content = CreateContent();
            return expr.ComputeValue(content);
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
                    if (f.Value[i].type == TokenType.IDENTIFIER && this.tokenParser.types.Contains(f.Value[i].text))
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
            IList<ICLS_Type> types = compiler.FilePreCompile(this, filename, listToken);
            foreach (var type in types)
            {
                this.RegType(type);
            }
        }
        public void File_CompileToken(string filename, IList<Token> listToken, bool embDebugToken)
        {
            logger.Log("File_CompilerToken:" + filename);
            IList<ICLS_Type> types = compiler.FileCompile(this, filename, listToken, embDebugToken);
            foreach (var type in types)
            {
                if (this.GetTypeByKeywordQuiet(type.keyword) == null)
                    this.RegType(type);
            }
        }


        public void Project_PacketToStream(Dictionary<string, IList<Token>> project, System.IO.Stream outstream)
        {
            byte[] FileHead = System.Text.Encoding.UTF8.GetBytes("C#LE-DLL");
            outstream.Write(FileHead, 0, 8);
            UInt16 count = (UInt16)project.Count;
            outstream.Write(BitConverter.GetBytes(count), 0, 2);
            foreach (var p in project)
            {
                byte[] pname = System.Text.Encoding.UTF8.GetBytes(p.Key);
                outstream.WriteByte((byte)pname.Length);
                outstream.Write(pname, 0, pname.Length);
                this.tokenParser.SaveTokenList(p.Value, outstream);
            }
        }
        public Dictionary<string, IList<Token>> Project_FromPacketStream(System.IO.Stream instream)
        {
            Dictionary<string, IList<Token>> project = new Dictionary<string, IList<Token>>();
            byte[] buf = new byte[8];
            instream.Read(buf, 0, 8);
            string filehead = System.Text.Encoding.UTF8.GetString(buf, 0, 8);
            if (filehead != "C#LE-DLL") return null;
            instream.Read(buf, 0, 2);
            UInt16 count = BitConverter.ToUInt16(buf, 0);
            for (int i = 0; i < count; i++)
            {
                int slen = instream.ReadByte();
                byte[] buffilename = new byte[slen];
                instream.Read(buffilename, 0, slen);
                string key = System.Text.Encoding.UTF8.GetString(buffilename, 0, slen);
                var tlist = tokenParser.ReadTokenList(instream);
                project[key] = tlist;
            }
            return project;

        }
    }
}
