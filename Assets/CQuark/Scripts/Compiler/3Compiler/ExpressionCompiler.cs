using CQuark;
using System.Collections.Generic;
using System;

namespace CQuark.Compile{
	public class ExpressionCompiler {
		//编译一个CQuark，可包含类中类，多个类
		public static void CompileClass(string fileName, IList<Token> tokens){
			for(int i = 0; i < tokens.Count; i++) {
				if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ";")
					continue;
				
				if(tokens[i].type == TokenType.KEYWORD && (tokens[i].text == "class" || tokens[i].text == "interface")) {
					string name = tokens[i + 1].text;
					int ibegin = i + 2;
					
					if(tokens[ibegin].text == ":") {
						ibegin++;
					}
					while(tokens[ibegin].text != "{") {
						ibegin++;
					}
					
					int iend = FindBlock(tokens, ibegin);
					if(iend == -1) {
						DebugUtil.LogError("查找文件尾失败。");
						return;
					}
					
					CompilerSingleClass(name, (tokens[i].text == "interface"), fileName, tokens, ibegin, iend);
					i = iend;
					continue;
				}
			}
		}
		//编译一个类
		static IType CompilerSingleClass (string classname, bool bInterface, string filename, IList<Token> tokens, int ibegin, int iend) {
			
			Type_Class typeClass = CQuark.AppDomain.GetTypeByKeywordQuiet(classname) as Type_Class;
			
			if(typeClass == null)
				typeClass = new Type_Class(classname, bInterface, filename);
			
			(typeClass._class as Class_CQuark).functions.Clear();
			(typeClass._class as Class_CQuark).members.Clear();
			//搜寻成员定义和函数
			//定义语法            //Type id[= expr];
			//函数语法            //Type id([Type id,]){block};
			//属性语法            //Type id{get{},set{}};
			bool bPublic = false;
			bool bStatic = false;
			
			for(int i = ibegin; i <= iend; i++) {
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "public") {
					bPublic = true;
					continue;
				}
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "private") {
					bPublic = false;
					continue;
				}
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "protected") {
					bPublic = false;
					continue;
				}
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "static") {
					bStatic = true;
					continue;
				}
				if(tokens[i].type == TokenType.TYPE || (tokens[i].type == TokenType.IDENTIFIER && tokens[i].text == classname))//发现类型
				{
					IType idtype = CQuark.AppDomain.GetTypeByKeyword("null");
					bool bctor = false;
					if(tokens[i].type == TokenType.TYPE)//类型
					{
						if(tokens[i].text == classname && tokens[i + 1].text == "(") {//构造函数
							bctor = true;
							i--;
						}
						else if(tokens[i + 1].text == "[" && tokens[i + 2].text == "]") {
							idtype = CQuark.AppDomain.GetTypeByKeyword(tokens[i].text + "[]");
							i += 2;
						}
						else if(tokens[i].text == "void") {
							
						}
						else {
							idtype = CQuark.AppDomain.GetTypeByKeyword(tokens[i].text);
						}
					}
					
					if(tokens[i + 1].type == CQuark.TokenType.IDENTIFIER || bctor) //类型后面是名称
					{
						string idname = tokens[i + 1].text;
						if(tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && tokens[i + 2].text == "(")//参数开始,这是函数
						{
							#if CQUARK_DEBUG
							DebugUtil.Log("发现函数:" + idname);
							#endif
							Class_CQuark.Function func = new Class_CQuark.Function();
							func.bStatic = bStatic;
							func.bPublic = bPublic;
							
							if(tokens[i].text != "void")
								func._returntype = CQuark.AppDomain.GetTypeByKeyword(tokens[i].text);
							
							int funcparambegin = i + 2;
							int funcparamend = FindBlock(tokens, funcparambegin);
							if(funcparamend - funcparambegin > 1) {
								int start = funcparambegin + 1;
								
								for(int j = funcparambegin + 1; j <= funcparamend; j++) {
									if(tokens[j].text == "," || tokens[j].text == ")") {
										string ptype = "";
										for(int k = start; k <= j - 2; k++)
											ptype += tokens[k].text;
										string pid = tokens[j - 1].text;
										IType type = CQuark.AppDomain.GetTypeByKeyword(ptype);
										// _params[pid] = type;
										//func._params.Add(pid, type);
										if(type == null) {
											throw new Exception(filename + ":不可识别的函数头参数:" + tokens[funcparambegin].ToString() + tokens[funcparambegin].SourcePos());
										}
										func._paramnames.Add(pid);
										func._paramtypes.Add(type);
										start = j + 1;
									}
								}
							}
							
							int funcbegin = funcparamend + 1;
							if(tokens[funcbegin].text == "{") {
								int funcend = FindBlock(tokens, funcbegin);
								ExprCompileUtil.Compiler_Expression_Block(tokens, funcbegin, funcend, out func.expr_runtime);
								if(func.expr_runtime == null) {
									DebugUtil.LogWarning("警告，该函数编译为null，请检查");
								}
								(typeClass._class as Class_CQuark).functions.Add(idname, func);
								
								i = funcend;
							}
							else if(tokens[funcbegin].text == ";") {
								
								func.expr_runtime = null;
								(typeClass._class as Class_CQuark).functions.Add(idname, func);
								i = funcbegin;
							}
							else {
								throw new Exception(filename + ":不可识别的函数表达式:" + tokens[funcbegin].ToString() + tokens[funcbegin].SourcePos());
							}
						}
						else if(tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && tokens[i + 2].text == "{")//语句块开始，这是 getset属性
						{
							//get set 成员定义
							
							bool setpublic = true;
							bool haveset = false;
							for(int j = i + 3; j <= iend; j++) {
								if(tokens[j].text == "get") {
									setpublic = true;
								}
								if(tokens[j].text == "private") {
									setpublic = false;
								}
								if(tokens[j].text == "set") {
									haveset = true;
								}
								if(tokens[j].text == "}") {
									break;
								}
							}
							
							
							var member = new Class_CQuark.Member();
							member.bStatic = bStatic;
							member.bPublic = bPublic;
							member.bReadOnly = !(haveset && setpublic);
							member.m_itype = idtype;
							#if CQUARK_DEBUG
							DebugUtil.Log("发现Get/Set:" + idname);
							#endif
							//ICQ_Expression expr = null;
							
							if(tokens[i + 2].text == "=") {
								int jbegin = i + 3;
								int jdep;
								int jend = ExprCompileUtil.FindCodeAny(tokens, ref jbegin, out jdep);
								
								if(!ExprCompileUtil.Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue)) {
									DebugUtil.LogError("Get/Set定义错误");
								}
								i = jend;
							}
							(typeClass._class as Class_CQuark).members.Add(idname, member);
						}
						else if(tokens[i + 2].type == CQuark.TokenType.PUNCTUATION && (tokens[i + 2].text == "=" || tokens[i + 2].text == ";"))//这是成员定义
						{
							#if CQUARK_DEBUG
							DebugUtil.Log("发现成员定义:" + idname);
							#endif
							var member = new Class_CQuark.Member();
							member.bStatic = bStatic;
							member.bPublic = bPublic;
							member.bReadOnly = false;
							member.m_itype = idtype;
							
							//ICQ_Expression expr = null;
							
							if(tokens[i + 2].text == "=") {
								int posend = 0;
								for(int j = i; j < iend; j++) {
									if(tokens[j].text == ";") {
										posend = j - 1;
										break;
									}
								}
								
								int jbegin = i + 3;
								int jdep;
								int jend = ExprCompileUtil.FindCodeAny(tokens, ref jbegin, out jdep);
								if(jend < posend) {
									jend = posend;
								}
								if(!ExprCompileUtil.Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue)) {
									DebugUtil.LogError("成员定义错误");
								}
								i = jend;
							}
							(typeClass._class as Class_CQuark).members.Add(idname, member);
						}
						
						bPublic = false;
						bStatic = false;
						
						continue;
					}
					else {
						throw new Exception(filename + ":不可识别的表达式:" + tokens[i].ToString() + tokens[i].SourcePos());
					}
				}
			}
			return typeClass;
		}


		//TODO这2个方法移动到PreCompile
		//编译一个非类的CQ。调用到这个方法时，Token如果是TYPE，必须是全名
		public static ICQ_Expression CompileParagraph (IList<Token> tlist) {
			ICQ_Expression value;
			
			int expbegin = 0;
			int expend = FindCodeBlock(tlist, expbegin);
			if(expend != tlist.Count - 1) {
				DebugUtil.LogError(tlist, "CodeBlock 识别问题,异常结尾", expbegin, expend);
				return null;
			}
			bool succ = ExprCompileUtil.Compiler_Expression_Block(tlist, expbegin, expend, out value);
			if(succ) {
				if(value == null) {
					DebugUtil.LogWarning("编译为null:");
				}
				return value;
			}
			else {
				DebugUtil.LogError(tlist, "编译失败:", expbegin, expend);
				return null;
			}
		}
	
		static int FindCodeBlock (IList<Token> tokens, int pos) {
			int dep = 0;
			for(int i = pos; i < tokens.Count; i++) {
				
				if(tokens[i].type == TokenType.PUNCTUATION) {
					if(tokens[i].text == "{") {
						dep++;
					}
					if(tokens[i].text == "}") {
						dep--;
						if(dep < 0)
							return i - 1;
					}
				}
			}
			if(dep != 0)
				return -1;
			else
				return tokens.Count - 1;
		}

		static int FindBlock (IList<Token> tokens, int start) {
			if(tokens[start].type != CQuark.TokenType.PUNCTUATION) {
				DebugUtil.LogError("(script)FindBlock 没有从符号开始");
			}
			string left = tokens[start].text;
			string right = "}";
			if(left == "{") right = "}";
			if(left == "(") right = ")";
			if(left == "[") right = "]";
			int depth = 0;
			for(int i = start; i < tokens.Count; i++) {
				if(tokens[i].type == CQuark.TokenType.PUNCTUATION) {
					if(tokens[i].text == left) {
						depth++;
					}
					else if(tokens[i].text == right) {
						depth--;
						if(depth == 0) {
							return i;
						}
					}
				}
			}
			return -1;
		}
	}
}
