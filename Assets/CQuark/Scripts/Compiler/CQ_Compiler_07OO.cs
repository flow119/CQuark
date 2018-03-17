using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
    public partial class CQ_Expression_Compiler 
    {

        static IList<IType> _FileCompiler(string filename, IList<Token> tokens, bool embDeubgToken, bool onlyGotType)
        {
            List<IType> typelist = new List<IType>();

            List<string> usingList = new List<string>();
            //识别using

            //扫描token有没有要合并的类型
            //using的实现在token级别处理即可
            bool bJumpClass = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ";")
                    continue;
                if (tokens[i].type == TokenType.COMMENT)
                    continue;
                if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "using")
                {
                    int dep;
                    int pos = i;
                    int iend = FindCodeAny(tokens, ref pos, out dep);
                    List<string> list = Compiler_Using(tokens, pos, iend);
                    string useText = "";
                    for (int j = 0; j < list.Count; j++)
                    {
                        useText += list[j];
                        if (j != list.Count - 1)
                        {
                            useText += ".";
                        }
                    }
                    usingList.Add(useText);
                    i = iend;
                    continue;
                }

                if (tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "[")
                {
                    if (tokens[i + 1].text == "NotScipt" || (tokens[i + 1].text == "CQuark" && tokens[i + 3].text == "NotScipt"))
                    {
                        bJumpClass = true;
                        i = i + 2;
                        continue;
                    }
                }
                if (tokens[i].type == TokenType.KEYWORD && (tokens[i].text == "class" || tokens[i].text == "interface"))
                {
                    string name = tokens[i + 1].text;
                    //在这里检查继承
                    List<string> typebase = null;
                    int ibegin = i + 2;
                    if (onlyGotType)
                    {
                        while (tokens[ibegin].text != "{")
                        {
                            ibegin++;
                        }
                    }
                    else
                    {
                        if (tokens[ibegin].text == ":")
                        {
                            typebase = new List<string>();
                            ibegin++;
                        }
                        while (tokens[ibegin].text != "{")
                        {
                            if (tokens[ibegin].type == TokenType.TYPE)
                            {
                                typebase.Add(tokens[ibegin].text);
                            }
                            ibegin++;
                        }
                    }
                    int iend = FindBlock(tokens, ibegin);
                    if (iend == -1)
                    {
                        DebugUtil.LogError("查找文件尾失败。");
                        return null;
                    }
                    if (bJumpClass)
                    {
                        DebugUtil.Log("(NotScript)findclass:" + name + "(" + ibegin + "," + iend + ")");
                    }
                    else if (onlyGotType)
                    {
                        DebugUtil.Log("(scriptPreParser)findclass:" + name + "(" + ibegin + "," + iend + ")");
                    }
                    else
                    {
                        DebugUtil.Log("(scriptParser)findclass:" + name + "(" + ibegin + "," + iend + ")");
                    }
                    if (bJumpClass)
                    {//忽略这个Class
                        //ICQ_Type type = Compiler_Class(env, name, (tokens[i].text == "interface"), filename, tokens, ibegin, iend, embDeubgToken, true);
                        //bJumpClass = false;
                    }
                    else
                    {
                        IType type = Compiler_Class(name, (tokens[i].text == "interface"), typebase, filename, tokens, ibegin, iend, embDeubgToken, onlyGotType, usingList);
                        if (type != null)
                        {
                            typelist.Add(type);
                        }
                    }
                    i = iend;
                    continue;
                }
            }

            return typelist;
        }
        static IType Compiler_Class(string classname, bool bInterface, IList<string> basetype, string filename, IList<Token> tokens, int ibegin, int iend, bool EmbDebugToken, bool onlyGotType, IList<string> usinglist)
        {

			Type_Class typeClass = CQuark.AppDomain.GetTypeByKeywordQuiet(classname) as Type_Class;

            if (typeClass == null)
                typeClass = new Type_Class(classname, bInterface, filename);

            if (basetype != null && basetype.Count != 0 && onlyGotType == false)
            {
                List<IType> basetypess = new List<IType>();
                foreach (string t in basetype)
                {
					IType type = CQuark.AppDomain.GetTypeByKeyword(t);
                    basetypess.Add(type);
                }
                typeClass.SetBaseType(basetypess);
            }

            if (onlyGotType) 
				return typeClass;

            //if (env.useNamespace && usinglist != null)
            //{//使用命名空间,替换token

            //    List<Token> newTokens = new List<Token>();
            //    for (int i = ibegin; i <= iend; i++)
            //    {
            //        if (tokens[i].type == TokenType.IDENTIFIER)
            //        {
            //            string ntype = null;
            //            string shortname = tokens[i].text;
            //            int startpos = i;
            //            while (ntype == null)
            //            {

            //                foreach (var u in usinglist)
            //                {
            //                    string ttype = u + "." + shortname;
            //                    if (env.GetTypeByKeywordQuiet(ttype) != null)
            //                    {
            //                        ntype = ttype;

            //                        break;
            //                    }

            //                }
            //                if (ntype != null) break;
            //                if ((startpos + 2) <= iend && tokens[startpos + 1].text == "." && tokens[startpos + 2].type == TokenType.IDENTIFIER)
            //                {
            //                    shortname += "." + tokens[startpos + 2].text;

            //                    startpos += 2;
            //                    if (env.GetTypeByKeywordQuiet(shortname) != null)
            //                    {
            //                        ntype = shortname;

            //                        break;
            //                    }
            //                    continue;
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            if (ntype != null)
            //            {
            //                var t = tokens[i];
            //                t.text = ntype;
            //                t.type = TokenType.TYPE;
            //                newTokens.Add(t);
            //                i = startpos;
            //                continue;
            //            }
            //        }
            //        newTokens.Add(tokens[i]);
            //    }
            //    tokens = newTokens;
            //    ibegin = 0;
            //    iend = tokens.Count - 1;
            //}

            typeClass.compiled = false;
            (typeClass.function as Class_CQuark).functions.Clear();
            (typeClass.function as Class_CQuark).members.Clear();
            //搜寻成员定义和函数
            //定义语法            //Type id[= expr];
            //函数语法            //Type id([Type id,]){block};
            //属性语法            //Type id{get{},set{}};
            bool bPublic = false;
            bool bStatic = false;
            if (EmbDebugToken)//SType 嵌入Token
            {
                typeClass.EmbDebugToken(tokens);
            }
            for (int i = ibegin; i <= iend; i++)
            {

                if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "public")
                {
                    bPublic = true;
                    continue;
                }
                else if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "private")
                {
                    bPublic = false;
                    continue;
                }
                else if (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "static")
                {
                    bStatic = true;
                    continue;
                }
                else if (tokens[i].type == TokenType.TYPE || (tokens[i].type == TokenType.IDENTIFIER && tokens[i].text == classname))//发现类型
                {

					IType idtype = CQuark.AppDomain.GetTypeByKeyword("null");
                    bool bctor = false;
                    if (tokens[i].type == TokenType.TYPE)//类型
                    {

                        if (tokens[i].text == classname && tokens[i + 1].text == "(")
                        {//构造函数
                            bctor = true;
                            i--;
                        }
                        else if (tokens[i + 1].text == "[" && tokens[i + 2].text == "]")
                        {
							idtype = CQuark.AppDomain.GetTypeByKeyword(tokens[i].text + "[]");
                            i += 2;
                        }
						else if (tokens[i].text == "void")
                        {

                        }
                        else
                        {
							idtype = CQuark.AppDomain.GetTypeByKeyword(tokens[i].text);
                        }
                    }

                    if (tokens[i + 1].type == CQuark.TokenType.IDENTIFIER || bctor) //类型后面是名称
                    {
                        string idname = tokens[i + 1].text;
                        if (tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && tokens[i + 2].text == "(")//参数开始,这是函数
                        {
                            DebugUtil.Log("发现函数:" + idname);
                            Class_CQuark.Function func = new Class_CQuark.Function();
                            func.bStatic = bStatic;
                            func.bPublic = bPublic;

                            int funcparambegin = i + 2;
                            int funcparamend = FindBlock(tokens, funcparambegin);
                            if (funcparamend - funcparambegin > 1)
                            {


                                int start = funcparambegin + 1;
                                //Dictionary<string, ICQ_Type> _params = new Dictionary<string, ICQ_Type>();
                                for (int j = funcparambegin + 1; j <= funcparamend; j++)
                                {
                                    if (tokens[j].text == "," || tokens[j].text == ")")
                                    {
                                        string ptype = "";
                                        for (int k = start; k <= j - 2; k++)
                                            ptype += tokens[k].text;
                                        var pid = tokens[j - 1].text;
										var type = CQuark.AppDomain.GetTypeByKeyword(ptype);
                                        // _params[pid] = type;
                                        //func._params.Add(pid, type);
                                        if (type == null)
                                        {
                                            throw new Exception(filename + ":不可识别的函数头参数:" + tokens[funcparambegin].ToString() + tokens[funcparambegin].SourcePos());
                                        }
                                        func._paramnames.Add(pid);
                                        func._paramtypes.Add(type);
                                        start = j + 1;
                                    }
                                }
                            }

                            int funcbegin = funcparamend + 1;
                            if (tokens[funcbegin].text == "{")
                            {
                                int funcend = FindBlock(tokens, funcbegin);
                                Compiler_Expression_Block(tokens, funcbegin, funcend, out func.expr_runtime);
                                if (func.expr_runtime == null)
                                {
                                    DebugUtil.LogWarning("警告，该函数编译为null，请检查");
                                }
                                (typeClass.function as Class_CQuark).functions.Add(idname, func);

                                i = funcend;
                            }
                            else if (tokens[funcbegin].text == ";")
                            {

                                func.expr_runtime = null;
                                (typeClass.function as Class_CQuark).functions.Add(idname, func);
                                i = funcbegin;
                            }
                            else
                            {
                                throw new Exception(filename + ":不可识别的函数表达式:" + tokens[funcbegin].ToString() + tokens[funcbegin].SourcePos());
                            }
                        }
                        else if (tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && tokens[i + 2].text == "{")//语句块开始，这是 getset属性
                        {
                            //get set 成员定义

                            bool setpublic = true;
                            bool haveset = false;
                            for (int j = i + 3; j <= iend; j++)
                            {
                                if (tokens[j].text == "get")
                                {
                                    setpublic = true;
                                }
                                if (tokens[j].text == "private")
                                {
                                    setpublic = false;
                                }
                                if (tokens[j].text == "set")
                                {
                                    haveset = true;
                                }
                                if (tokens[j].text == "}")
                                {
                                    break;
                                }
                            }


                            var member = new Class_CQuark.Member();
                            member.bStatic = bStatic;
                            member.bPublic = bPublic;
                            member.bReadOnly = !(haveset && setpublic);
                            member.type = idtype;
                            DebugUtil.Log("发现Get/Set:" + idname);
                            //ICQ_Expression expr = null;

                            if (tokens[i + 2].text == "=")
                            {
                                int jbegin = i + 3;
                                int jdep;
                                int jend = FindCodeAny(tokens, ref jbegin, out jdep);

                                if (!Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue))
                                {
                                    DebugUtil.LogError("Get/Set定义错误");
                                }
                                i = jend;
                            }
                            (typeClass.function as Class_CQuark).members.Add(idname, member);
                        }
                        else if (tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && (tokens[i + 2].text == "=" || tokens[i + 2].text == ";"))//这是成员定义
                        {
                            DebugUtil.Log("发现成员定义:" + idname);

                            var member = new Class_CQuark.Member();
                            member.bStatic = bStatic;
                            member.bPublic = bPublic;
                            member.bReadOnly = false;
                            member.type = idtype;


                            //ICQ_Expression expr = null;

                            if (tokens[i + 2].text == "=")
                            {
                                int posend = 0;
                                for (int j = i; j < iend; j++)
                                {
                                    if (tokens[j].text == ";")
                                    {
                                        posend = j - 1;
                                        break;
                                    }
                                }

                                int jbegin = i + 3;
                                int jdep;
                                int jend = FindCodeAny(tokens, ref jbegin, out jdep);
                                if (jend < posend)
                                {
                                    jend = posend;
                                }
                                if (!Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue))
                                {
                                    DebugUtil.LogError("成员定义错误");
                                }
                                i = jend;
                            }
                            (typeClass.function as Class_CQuark).members.Add(idname, member);
                        }

                        bPublic = false;
                        bStatic = false;

                        continue;
                    }
                    else
                    {
                        throw new Exception(filename + ":不可识别的表达式:" + tokens[i].ToString() + tokens[i].SourcePos());
                    }
                }
            }
            typeClass.compiled = true;
            return typeClass;
        }

        static List<string> Compiler_Using(IList<Token> tokens, int pos, int posend)
        {
            List<string> _namespace = new List<string>();

            for (int i = pos + 1; i <= posend; i++)
            {
                if (tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ".")
                    continue;
                else
                    _namespace.Add(tokens[i].text);
            }
            return _namespace;
        }
        //Dictionary<string, functioninfo> funcs = new Dictionary<string, functioninfo>();

        static int FindBlock(IList<CQuark.Token> tokens, int start)
        {
            if (tokens[start].type != CQuark.TokenType.PUNCTUATION)
            {
                DebugUtil.LogError("(script)FindBlock 没有从符号开始");
            }
            string left = tokens[start].text;
            string right = "}";
            if (left == "{") right = "}";
            if (left == "(") right = ")";
            if (left == "[") right = "]";
            int depth = 0;
            for (int i = start; i < tokens.Count; i++)
            {
                if (tokens[i].type == CQuark.TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == left)
                    {
                        depth++;
                    }
                    else if (tokens[i].text == right)
                    {
                        depth--;
                        if (depth == 0)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
    }
}