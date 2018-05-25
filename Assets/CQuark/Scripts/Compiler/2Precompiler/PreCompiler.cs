using System.Collections;
using System.Collections.Generic;
using CQuark;
using System;

namespace CQuark{
	public class PreCompiler  {

		//找到类型，写入AppDomain和CustomTypes
		//如果类里有类那么依次加入
		public static void RegisterCQClass(string fileName, IList<Token> tokens){
			string Namespace = "";
			List<string> classStack = new List<string> ();
			string outterClass = "";

			for (int i = 0; i < tokens.Count; i++) {
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "namespace"){
					Namespace = tokens[i].text;
				}
				if (tokens [i].type == TokenType.KEYWORD && (tokens [i].text == "class" || tokens [i].text == "interface" || tokens[i].text == "struct")) {
					string name = tokens [i + 1].text;
					outterClass = name;
					bool bInterface = (tokens [i].text == "interface");

					#if CQUARK_DEBUG
					DebugUtil.Log("(scriptPreParser)findclass:" + name + "(" + ibegin + "," + iend + ")");
					#endif                    

					string fullName = "";
					if(!string.IsNullOrEmpty(Namespace))
						fullName = Namespace + ".";
					foreach(string s in classStack){
						if(!string.IsNullOrEmpty(s))
							fullName += s + ".";
					}
					fullName += name;

					//把Token里的Type注册到AppDomain里
					if(CQuark.AppDomain.GetTypeByKeywordQuiet(fullName) == null){
						Type_Class typeClass = new Type_Class(fullName, bInterface, fileName);
						AppDomain.RegisterCQType(typeClass);
					}
				}
				if(tokens[i].type == TokenType.PUNCTUATION){
					if(tokens[i].text == "{"){
						if(string.IsNullOrEmpty(outterClass))
							classStack.Add("");
						else
						{
							classStack.Add(outterClass);
							outterClass = "";
						}
					}else if(tokens[i].text == "}"){
						classStack.RemoveAt(classStack.Count - 1);
					}
				}
			}
		}
		
		//再全部注册完毕后，把类名补足
		public static void FixNamespace(string fileName, IList<Token> tokens){
			
		}

		//把所有类的Identifier替换成Type
		public static void IdentifyClass(string fileName, IList<Token> tokens){

		}

		static List<string> Compiler_Using (IList<Token> tokens, int pos, int posend) {
			List<string> _namespace = new List<string>();
			
			for(int i = pos + 1; i <= posend; i++) {
				if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ".")
					continue;
				else
					_namespace.Add(tokens[i].text);
			}
			return _namespace;
		}
		static IType Compiler_Class (string classname, bool bInterface, IList<string> basetype, string filename, IList<Token> tokens, int ibegin, int iend, bool onlyGotType, IList<string> usinglist) {
			
			Type_Class typeClass = CQuark.AppDomain.GetTypeByKeywordQuiet(classname) as Type_Class;
			
			if(typeClass == null)
				typeClass = new Type_Class(classname, bInterface, filename);
			
			if(basetype != null && basetype.Count != 0 && onlyGotType == false) {
				List<IType> basetypess = new List<IType>();
				foreach(string t in basetype) {
					IType type = CQuark.AppDomain.GetTypeByKeyword(t);
					basetypess.Add(type);
				}
				typeClass.SetBaseType(basetypess);
			}
			
			if(onlyGotType)
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
			(typeClass._class as Class_CQuark).functions.Clear();
			(typeClass._class as Class_CQuark).members.Clear();
			//搜寻成员定义和函数
			//定义语法            //Type id[= expr];
			//函数语法            //Type id([Type id,]){block};
			//属性语法            //Type id{get{},set{}};
			bool bPublic = false;
			bool bStatic = false;
			
			#if CQUARK_DEBUG
			//嵌入Token 到Class_CQuark 
			typeClass.EmbDebugToken(tokens);
			#endif
			
			for(int i = ibegin; i <= iend; i++) {
				if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "public") {
					bPublic = true;
					continue;
				}
				else if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "private") {
					bPublic = false;
					continue;
				}
				else if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "protected") {
					bPublic = false;
					continue;
				}
				else if(tokens[i].type == TokenType.KEYWORD && tokens[i].text == "static") {
					bStatic = true;
					continue;
				}
				else if(tokens[i].type == TokenType.TYPE || (tokens[i].type == TokenType.IDENTIFIER && tokens[i].text == classname))//发现类型
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
										var pid = tokens[j - 1].text;
										var type = CQuark.AppDomain.GetTypeByKeyword(ptype);
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
								CQ_Expression_Compiler.Compiler_Expression_Block(tokens, funcbegin, funcend, out func.expr_runtime);
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
								int jend = CQ_Expression_Compiler.FindCodeAny(tokens, ref jbegin, out jdep);
								
								if(!CQ_Expression_Compiler.Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue)) {
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
								int jend = CQ_Expression_Compiler.FindCodeAny(tokens, ref jbegin, out jdep);
								if(jend < posend) {
									jend = posend;
								}
								if(!CQ_Expression_Compiler.Compiler_Expression(tokens, jbegin, jend, out member.expr_defvalue)) {
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
			typeClass.compiled = true;
			return typeClass;
		}
		static int FindBlock (IList<CQuark.Token> tokens, int start) {
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


		
		static int FindToken(List<Token> tokens, int npos, TokenType type){
			for(int i = npos; i < tokens.Count; i++) {
				if(tokens[i].type == type)
					return i;
			}
			return -1;
		}
		
		static int FindToken(List<Token> tokens, int npos, string text){
			for(int i = npos; i < tokens.Count; i++) {
				if(tokens[i].text == text)
					return i;
			}
			return -1;
		}
		
		//把tokens的start位开始合并到一个token里
		static Token CombineReplace(List<Token> tokens, int start, int length, TokenType type){
			Token t = tokens [start];//line和pos继承
			for (int i = 1; i < length; i++) {
				t.text += tokens[start + 1].text;
				tokens.RemoveAt(start + 1);
			}
			t.type = type;
			tokens [start] = t;
			return t;
		}
		
		static string Combine(List<Token> tokens, int start, int length){
			string ret = tokens[start].text;
			for (int i = 1; i < length; i++) {
				ret += tokens[start + i].text;
			}
			return ret;
		}
		
		//判断x.y到底是类.方法还是命名空间.类，类.类
		//		[Obsolete]
		//		static void ReplaceIdentifier(List<Token> tokens, List<string> usingNamespace){
		//			for(int start = 0; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.IDENTIFIER){
		//					for(int end = start; end < tokens.Count;){
		//						string fullname =  Combine(tokens, start, end - start + 1);
		//						if(IsType(fullname, usingNamespace, out fullname)){
		//							Token t = CombineReplace(tokens, start, end - start + 1, TokenType.TYPE);
		//							t.text = fullname;
		//							tokens[start] = t;
		//							start--;//先不判断下一个，因为要合并类中类
		//							break;
		//						}
		//
		//						if(end + 2 < tokens.Count && tokens[end + 1].text == "." && tokens[end+2].type == TokenType.IDENTIFIER){
		//							end += 2;
		//						}else{
		//							break;
		//						}
		//					}
		//				}else if(tokens[start].type == TokenType.TYPE){
		//					//合并类中类
		//					if(start + 2 < tokens.Count && tokens[start + 1].text == "." && tokens[start + 2].type == TokenType.IDENTIFIER){
		//						string fullname =  Combine(tokens, start, 3);
		//						if(IsType(fullname, usingNamespace, out fullname)){
		//							Token t = CombineReplace(tokens, start, 3, TokenType.TYPE);
		//							t.text = fullname;
		//							tokens[start] = t;
		//							start--;
		//						}
		//					}
		//				}
		//			}
		//
		//
		////			if(ContainsType(t.text)) { //foreach (string s in types)
		////				while(line[i] == ' ' && i < line.Length) {
		////					i++;
		////				}
		////				if(line[i] == '<') { /*  || line[i] == '['*/
		////					int dep = 0;
		////					string text = t.text;
		////					while(i < line.Length) {
		////						if(line[i] == '<')
		////							dep++;
		////						if(line[i] == '>') 
		////							dep--;
		////						if(line[i] == ';' || line[i] == '(' || line[i] == '{') {
		////							break;
		////						}
		////						if(line[i] != ' ') 
		////							text += line[i];
		////						i++;
		////						if(dep == 0) {
		////							t.text = text;
		////							break;
		////						}
		////					}
		////					//if (types.Contains(t.text))//自动注册
		////					{
		////						t.type = TokenType.TYPE;
		////						return i;
		////					}
		////				}
		////				else {
		////					t.type = TokenType.TYPE;
		////					return nstart + t.text.Length;
		////				}
		////			}
		////			while(i < line.Length && line[i] == ' ') {
		////				i++;
		////			}
		////			if(i < line.Length && (line[i] == '<'/* || line[i] == '['*/)) {//检查特别类型
		////				int dep = 0;
		////				string text = t.text;
		////				while(i < line.Length) {
		////					if(line[i] == '<') {
		////						dep++;
		////						i++;
		////						text += '<';
		////						continue;
		////					}
		////					if(line[i] == '>') {
		////						dep--;
		////						i++;
		////						if(dep == 0) {
		////							t.text = text + '>';
		////							break;
		////						}
		////						continue;
		////					}
		////					Token tt;
		////					int nnstart = FindStart(line, i, ref lineIndex);
		////					i = GetToken(line, nnstart, out tt, ref lineIndex);
		////					if(tt.type != TokenType.IDENTIFIER && tt.type != TokenType.TYPE && tt.text != ",") {
		////						break;
		////					}
		////					text += tt.text;
		////				}
		////				if(ContainsType(t.text)) {
		////					t.type = TokenType.TYPE;
		////					return i;
		////					
		////				}
		////				else if(dep == 0) {
		////					t.type = TokenType.IDENTIFIER;
		////					return i;
		////				}
		////			}
		////			if(tokens.Count >= 3 && t.type == TokenType.PUNCTUATION && t.text == ">"
		////			   && tokens[tokens.Count - 1].type == TokenType.TYPE
		////			   && tokens[tokens.Count - 2].type == TokenType.PUNCTUATION && tokens[tokens.Count - 2].text == "<"
		////			   && tokens[tokens.Count - 3].type == TokenType.IDENTIFIER) {//模板函数调用,合并之
		////				string ntype = tokens[tokens.Count - 3].text + tokens[tokens.Count - 2].text + tokens[tokens.Count - 1].text + t.text;
		////				t.type = TokenType.IDENTIFIER;
		////				t.text = ntype;
		////				t.pos = tokens[tokens.Count - 2].pos;
		////				t.line = tokens[tokens.Count - 2].line;
		////				tokens.RemoveAt(tokens.Count - 1);
		////				tokens.RemoveAt(tokens.Count - 1);
		////				tokens.RemoveAt(tokens.Count - 1);
		////				tokens.Add(t);
		////				continue;
		////			}
		//
		//
		//			//TODO 用作用域（Stack)去判断
		//
		//			//C# 允许这种写法 A A = new A();//fuck
		//			//但是int int这种又不行
		//			//1A B, A一定是Type，B一定是Property
		//			//2typeof(A)，A一定是Type
		//			//3<A>，<A,B>等 A一定是Type
		//			//4A[] A一定是Type
		//			//A.B，A.B.C，如果上面有A的Property（B A;），那么A一定是Property，否则A是类或者命名空间
		//			//new XX.X()一定是构造函数
		//
		//			//A.B到底是类中类还是A.成员没有办法知道，必须在Expression级处理
		////			if(tokens.Count >= 1 && t.type == TokenType.TYPE && tokens[tokens.Count - 1].type == TokenType.TYPE) {//Type Type 不可能，为重名
		////				t.type = TokenType.IDENTIFIER;
		////				tokens.Add(t);
		////				continue;
		////			}
		//
		//			//编译分几步
		//
		//			//2，把所有类提取出来，连同其namespace写到注册字典里
		//			//3，列举出所有的类（保留unknown.unknow），所有Property,所有function(必须作用域处理)
		//			//4，通过字典和反射确定 类名及所有Unknown
		//			//5,由compile expression（也需要重构）处理
		//		}
		//		[Obsolete]
		//		public static bool IsType(List<Token> tokens, int index){
		//			//typeof()
		//
		//			//构造函数
		//
		//			//后接Identifier
		//			if(index == 0)
		//				return true;
		//			if(tokens[index - 1].type == TokenType.PUNCTUATION){
		//				if(tokens[index - 1].text == ";" ||tokens[index - 1].text == "}" 
		//					||tokens[index - 1].text == "{" || tokens[index - 1].text == "=>")
		//					return true;
		//			}
		//			if(tokens[index - 1].type == TokenType.KEYWORD){
		//				if(tokens[index - 1].text == "public" ||tokens[index - 1].text == "private"
		//					||tokens[index - 1].text == "protected" || tokens[index - 1].text == "static")
		//					return true;
		//			}
		//			return false;
		//		}
		
//		public static List<string> DefineNamespace(List<Token> tokens, out string currentNamespace){
//			List<string> usingNamespace = new List<string>();
//			for(int start = 0; start < tokens.Count; start++){
//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "using"){
//					//using有3种用法。
//					//1控制Dispose
//					if(tokens[start + 1].type == TokenType.PUNCTUATION && tokens[start + 1].text == "("){//TODO 暂时不处理
//						
//					}
//					//2命名空间 using System.IO;//后面是命名空间
//					//3别名 using Project = PC.MyCompany.Project;//后面是Identifier
//					else{
//						//由于此时还不确定后面的类型是Type还是Namespace或者是未定义的Identifier，所以判断方式是先出现=还是;
//						int nextEqual = FindToken(tokens, start, "=");
//						int nextSemicolon = FindToken(tokens, start, ";");
//						if(nextEqual > 0 && nextEqual < nextSemicolon){
//							//别名
//							//TODO 暂时不处理
//						}else{
//							//命名空间
//							string nameSpace = CombineReplace(tokens, start + 1, nextSemicolon - start - 1, TokenType.NAMESPACE).text;
//							if(!usingNamespace.Contains(nameSpace))
//								usingNamespace.Add(nameSpace);
//						}
//					}
//				}
//			}
//			
//			currentNamespace = "";
//			for(int start = 0; start < tokens.Count; start++){
//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "namespace"){//namespace后接命名空间，可以包含多个.
//					for(int i = start + 1; i < tokens.Count; i++){
//						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
//							string nameSpace = CombineReplace(tokens, start, i - start - 1, TokenType.NAMESPACE).text;
//							currentNamespace = nameSpace;
//							if(!usingNamespace.Contains(nameSpace))
//								usingNamespace.Add(nameSpace);
//							break;
//						}
//					}
//				}
//			}
//			return usingNamespace;
//		}

//		public static void DefineClass(List<Token> tokens){
//			
//			for(int start = 0; start < tokens.Count; start++){
//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "class"){//class后面的一定是类，且后面只能接{或:xxx{
//					Token token = tokens[start + 1];
//					token.type = TokenType.CLASS;
//					tokens[start+1] = token;
//
//					if(start + 3 < tokens.Count && tokens[start + 2].text == ":"){
//						for(int i = start + 3; i < tokens.Count; i++){
//							if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
//								string baseClass = CombineReplace(tokens, start + 3, i - start - 3, TokenType.CLASS).text;
//								start += 3;
//								break;
//							}
//						}
//					}
//				}
//				else if(tokens[start].text == "typeof" && tokens[start + 1].text == "("){
//					//TODO 注意要区分T模板
//					for(int i = start + 2; i < tokens.Count; i++){
//						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ")"){
//							string baseClass = CombineReplace(tokens, start + 2, i - start - 2, TokenType.CLASS).text;
//							start = i + 1;
//							break;
//						}
//					}
//				}
//				else if(tokens[start].text == "as" || tokens[start].text == "is"){
//					if(tokens[start - 1].type == TokenType.IDENTIFIER ){
//						Token t = tokens[start - 1];
//						t.type = TokenType.PROPERTY;
//						tokens[start - 1] = t;
//					}
//					for(int i = start + 1; i < tokens.Count; i++){
//						if(tokens[i].type == TokenType.IDENTIFIER ){
//							if(tokens[i + 1].text == ".")
//								i += 2;
//						}else{
//							CombineReplace(tokens, start + 1, i - start - 1, TokenType.TYPE);
//							start = i;
//							break;
//						}
//					}
//				}
//			}
//
//			for (int start = 0; start < tokens.Count; start++) {
//				//(I)xxx强转
//			}
//		}

//		public static void DefineProperty(List<Token> tokens){
//			//三个函数顺序执行
//			for(int start = 1; start < tokens.Count; start++){
//				if(tokens[start].type == TokenType.IDENTIFIER && basicTypes.Contains(tokens[start].text)){//class后面的一定是类，且后面只能接{或:xxx{
//					Token token = tokens[start];
//					token.type = TokenType.TYPE;
//					tokens[start] = token;
//				}
//			}
//
//			for(int start = 1; start < tokens.Count - 1; start++){
//				if(tokens[start].type == TokenType.IDENTIFIER && (tokens[start - 1].type == TokenType.IDENTIFIER || tokens[start - 1].type == TokenType.TYPE)
//				   && tokens[start + 1].text != "("){//TYPE 后一定是Property
//					Token token = tokens[start];
//					token.type = TokenType.PROPERTY;
//					tokens[start] = token;
//				}
//			}
//			for(int start = 0; start < tokens.Count - 4; start++){
//				if(tokens[start].text == "[" && tokens[start + 1].text == "]" && tokens[start + 2].type != TokenType.PUNCTUATION){
//					Token t = tokens[start + 2];
//					t.type = TokenType.PROPERTY;
//					tokens[start + 2] = t;
//				}
//			}
//			//必须放在模版<>判断之后)
//			for (int start = 1; start < tokens.Count; start++) {
//				if(tokens[start].type == TokenType.PUNCTUATION){
//					if (tokens [start].text == ")" || tokens [start].text == "," || tokens [start].text == "]" || tokens[start].text == ";"
//					    || tokens[start].text == ">" || tokens[start].text == "<" || tokens[start].text == ">=" || tokens[start].text == "<=" || tokens[start].text == "==" || tokens[start].text == "!=" 
//					    || tokens[start].text == "+=" || tokens[start].text == "-=" || tokens[start].text == "*=" || tokens[start].text == "/=" || tokens[start].text == "%=" 
//					    || tokens[start].text == "=" || tokens[start].text == "&&" || tokens[start].text == "||" || tokens[start].text == "&=" || tokens[start].text == "|=") {
//						if(tokens[start - 1].type == TokenType.IDENTIFIER){
//							Token t = tokens[start - 1];
//							t.type = TokenType.PROPERTY;
//							tokens[start - 1] = t;
//						}
//					}
//				}
//			}
//			//TODO 作用域，并且获取基类
//
//			for (int end = tokens.Count - 1; end >= 0; end--) {
//				if(tokens[end].type == TokenType.PROPERTY && tokens[end - 1].type == TokenType.IDENTIFIER){//Property前面的Identifier一定是Type，可能是x.y.z a
//					for(int start = end - 1; start >= 0; ){
//						if(tokens[start - 1].text == "." && tokens[start - 2].type == TokenType.IDENTIFIER){
//							start -= 2;
//						}else{
//							CombineReplace(tokens, start, end - start, TokenType.TYPE);
//							break;
//						}
//					}
//				}
//			}
//		}

//		public static void DefineFunction(List<Token> tokens){
//			for(int start = 0; start < tokens.Count - 1; start++){
//				if(tokens[start + 1].text == "(" && tokens[start].type == TokenType.IDENTIFIER ){
//					//public
//					Token token = tokens[start];
//					token.type = TokenType.FUNCTION;
//					tokens[start] = token;
//				}
//			}
//		}

//		public static void DefineAttributes(List<Token> tokens){
//			for (int left = 1; left < tokens.Count - 1; left++) {
//				if(tokens[left].text == "[" && 
//				   (tokens[left-1].type == TokenType.PUNCTUATION || tokens[left - 1].type == TokenType.COMMENT)){
//					int depth = 0;
//					int right = left + 1;
//					for(; right < tokens.Count; right++){
//						if(tokens[right].text == "[")
//							depth ++;
//						else if(tokens[right].text == "]"){
//							if(depth > 0)
//								depth --;
//							else
//								break;
//						}
//					}
//					CombineReplace(tokens, left, right - left + 1, TokenType.ATTRIBUTE);
//					left = right;
//				}
//			}
//		}

//		public static void DefineTempletType(List<Token> tokens){
//			//模板类
//			for(int start = 0; start < tokens.Count - 1; start++){
//				if(tokens[start].text == "<"){
//					bool isTemplet = true;
//					int end = start + 1;
//					for(; end < tokens.Count - 1; ){
//						if(tokens[end].type == TokenType.TYPE || tokens[end].type == TokenType.NAMESPACE || tokens[end].type == TokenType.CLASS || tokens[end].type == TokenType.IDENTIFIER){
//							if (tokens[end + 1].text == ".")//<A.b>
//								end += 2;
//							else if (tokens[end + 1].text == ",")//<A, B>
//								end += 2;
//							else if (tokens[end + 1].text == ">")
//								break;
//							else{
//								isTemplet = false;
//								break;
//							}
//						}else{
//							isTemplet = false;
//							break;
//						}
//					}
//					if(isTemplet){
//						CombineReplace(tokens, start - 1, end - start + 3, TokenType.IDENTIFIER);
//					}
//				}
//			}
//
//			//数组
//			for(int left = 1; left < tokens.Count - 1; left++){
//				if(tokens[left].text == "[" ){
//					if(tokens[left+1].text == "]"){
//						for(int start = left - 1; start >= 0;){
//							if(tokens[start].type == TokenType.CLASS || tokens[start].type == TokenType.NAMESPACE ||
//								tokens[start].type == TokenType.TYPE || tokens[start].type == TokenType.IDENTIFIER){
//								if(start - 1 > 0 && tokens[start - 1].text == "."){
//									start -= 2;
//								}else{
//									CombineReplace(tokens, start, left - start, TokenType.TYPE);
//									break;
//								}
//							}
//						}
//					}else{
//						if(tokens[left - 1].type == TokenType.IDENTIFIER){
//							Token t = tokens[left - 1];
//							t.type = TokenType.PROPERTY;
//							tokens[left - 1] = t;
//						}
//					}
//				}
//			}
//
//		}

//		public static List<string> FindUsingNamespace(List<Token> tokens){
//			for(int start = 0; start < tokens.Count; start++){
//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "class"){//class后面的一定是类，且后面只能接{
//					string typeName = tokens[start + 1].text;
//					Token t = tokens[start + 1];
//					if(!string.IsNullOrEmpty(currentNamespace)){
//						typeName = currentNamespace + "." + typeName;
//						t.text = typeName;
//					}
//					//TODO 类中类的注册
//					RegisterNewType(typeName, currentNamespace);
//					t.type = TokenType.TYPE;
//					tokens[start + 1] = t;
//				}
//			}
//			return usingNamespace;
//		}

		//Parse的流程应该修改：
		//拆解出Token(不指名是Type还是Identifier)
		//对Tokens第二次处理，看是Namespace.Type还是Identifier(解决Namespace，类中类，类.方法，x.x.x)
	

			//找到类，注意，此时是根据C#规范来找的,因此不知道类的全名
//			DefineClass (tokens);	//所有的类注册完毕
//			DefineTempletType(tokens);//模板类会导致新的Type和新的构造函数，因此放在Property和Function之前
//			DefineProperty (tokens);
//			DefineFunction (tokens);
//			DefineAttributes (tokens);

			//第三步。对所有脚本编译（因为需要知道基类的property）

			//第四步，所有Property根据作用域来确定。不确定的保留Identifier.Identifier。

//			//第三步，列举出所有的类，所有Property,所有function(必须作用域处理)。不确定的保留（Identifier.Identifier），
//
//
//			//把Token区分出Type还是Namespace还是Method
//			ReplaceIdentifier (tokens, usingNamespace);


		
		//		public static List<string> DefineNamespace(List<Token> tokens, out string currentNamespace){
		//			List<string> usingNamespace = new List<string>();
		//			for(int start = 0; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "using"){
		//					//using有3种用法。
		//					//1控制Dispose
		//					if(tokens[start + 1].type == TokenType.PUNCTUATION && tokens[start + 1].text == "("){//TODO 暂时不处理
		//						
		//					}
		//					//2命名空间 using System.IO;//后面是命名空间
		//					//3别名 using Project = PC.MyCompany.Project;//后面是Identifier
		//					else{
		//						//由于此时还不确定后面的类型是Type还是Namespace或者是未定义的Identifier，所以判断方式是先出现=还是;
		//						int nextEqual = FindToken(tokens, start, "=");
		//						int nextSemicolon = FindToken(tokens, start, ";");
		//						if(nextEqual > 0 && nextEqual < nextSemicolon){
		//							//别名
		//							//TODO 暂时不处理
		//						}else{
		//							//命名空间
		//							string nameSpace = CombineReplace(tokens, start + 1, nextSemicolon - start - 1, TokenType.NAMESPACE).text;
		//							if(!usingNamespace.Contains(nameSpace))
		//								usingNamespace.Add(nameSpace);
		//						}
		//					}
		//				}
		//			}
		//			
		//			currentNamespace = "";
		//			for(int start = 0; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "namespace"){//namespace后接命名空间，可以包含多个.
		//					for(int i = start + 1; i < tokens.Count; i++){
		//						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
		//							string nameSpace = CombineReplace(tokens, start, i - start - 1, TokenType.NAMESPACE).text;
		//							currentNamespace = nameSpace;
		//							if(!usingNamespace.Contains(nameSpace))
		//								usingNamespace.Add(nameSpace);
		//							break;
		//						}
		//					}
		//				}
		//			}
		//			return usingNamespace;
		//		}
		
		//		public static void DefineClass(List<Token> tokens){
		//			
		//			for(int start = 0; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "class"){//class后面的一定是类，且后面只能接{或:xxx{
		//					Token token = tokens[start + 1];
		//					token.type = TokenType.CLASS;
		//					tokens[start+1] = token;
		//
		//					if(start + 3 < tokens.Count && tokens[start + 2].text == ":"){
		//						for(int i = start + 3; i < tokens.Count; i++){
		//							if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
		//								string baseClass = CombineReplace(tokens, start + 3, i - start - 3, TokenType.CLASS).text;
		//								start += 3;
		//								break;
		//							}
		//						}
		//					}
		//				}
		//				else if(tokens[start].text == "typeof" && tokens[start + 1].text == "("){
		//					//TODO 注意要区分T模板
		//					for(int i = start + 2; i < tokens.Count; i++){
		//						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ")"){
		//							string baseClass = CombineReplace(tokens, start + 2, i - start - 2, TokenType.CLASS).text;
		//							start = i + 1;
		//							break;
		//						}
		//					}
		//				}
		//				else if(tokens[start].text == "as" || tokens[start].text == "is"){
		//					if(tokens[start - 1].type == TokenType.IDENTIFIER ){
		//						Token t = tokens[start - 1];
		//						t.type = TokenType.PROPERTY;
		//						tokens[start - 1] = t;
		//					}
		//					for(int i = start + 1; i < tokens.Count; i++){
		//						if(tokens[i].type == TokenType.IDENTIFIER ){
		//							if(tokens[i + 1].text == ".")
		//								i += 2;
		//						}else{
		//							CombineReplace(tokens, start + 1, i - start - 1, TokenType.TYPE);
		//							start = i;
		//							break;
		//						}
		//					}
		//				}
		//			}
		//
		//			for (int start = 0; start < tokens.Count; start++) {
		//				//(I)xxx强转
		//			}
		//		}
		
		//		public static void DefineProperty(List<Token> tokens){
		//			//三个函数顺序执行
		//			for(int start = 1; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.IDENTIFIER && basicTypes.Contains(tokens[start].text)){//class后面的一定是类，且后面只能接{或:xxx{
		//					Token token = tokens[start];
		//					token.type = TokenType.TYPE;
		//					tokens[start] = token;
		//				}
		//			}
		//
		//			for(int start = 1; start < tokens.Count - 1; start++){
		//				if(tokens[start].type == TokenType.IDENTIFIER && (tokens[start - 1].type == TokenType.IDENTIFIER || tokens[start - 1].type == TokenType.TYPE)
		//				   && tokens[start + 1].text != "("){//TYPE 后一定是Property
		//					Token token = tokens[start];
		//					token.type = TokenType.PROPERTY;
		//					tokens[start] = token;
		//				}
		//			}
		//			for(int start = 0; start < tokens.Count - 4; start++){
		//				if(tokens[start].text == "[" && tokens[start + 1].text == "]" && tokens[start + 2].type != TokenType.PUNCTUATION){
		//					Token t = tokens[start + 2];
		//					t.type = TokenType.PROPERTY;
		//					tokens[start + 2] = t;
		//				}
		//			}
		//			//必须放在模版<>判断之后)
		//			for (int start = 1; start < tokens.Count; start++) {
		//				if(tokens[start].type == TokenType.PUNCTUATION){
		//					if (tokens [start].text == ")" || tokens [start].text == "," || tokens [start].text == "]" || tokens[start].text == ";"
		//					    || tokens[start].text == ">" || tokens[start].text == "<" || tokens[start].text == ">=" || tokens[start].text == "<=" || tokens[start].text == "==" || tokens[start].text == "!=" 
		//					    || tokens[start].text == "+=" || tokens[start].text == "-=" || tokens[start].text == "*=" || tokens[start].text == "/=" || tokens[start].text == "%=" 
		//					    || tokens[start].text == "=" || tokens[start].text == "&&" || tokens[start].text == "||" || tokens[start].text == "&=" || tokens[start].text == "|=") {
		//						if(tokens[start - 1].type == TokenType.IDENTIFIER){
		//							Token t = tokens[start - 1];
		//							t.type = TokenType.PROPERTY;
		//							tokens[start - 1] = t;
		//						}
		//					}
		//				}
		//			}
		//			//TODO 作用域，并且获取基类
		//
		//			for (int end = tokens.Count - 1; end >= 0; end--) {
		//				if(tokens[end].type == TokenType.PROPERTY && tokens[end - 1].type == TokenType.IDENTIFIER){//Property前面的Identifier一定是Type，可能是x.y.z a
		//					for(int start = end - 1; start >= 0; ){
		//						if(tokens[start - 1].text == "." && tokens[start - 2].type == TokenType.IDENTIFIER){
		//							start -= 2;
		//						}else{
		//							CombineReplace(tokens, start, end - start, TokenType.TYPE);
		//							break;
		//						}
		//					}
		//				}
		//			}
		//		}
		
		//		public static void DefineFunction(List<Token> tokens){
		//			for(int start = 0; start < tokens.Count - 1; start++){
		//				if(tokens[start + 1].text == "(" && tokens[start].type == TokenType.IDENTIFIER ){
		//					//public
		//					Token token = tokens[start];
		//					token.type = TokenType.FUNCTION;
		//					tokens[start] = token;
		//				}
		//			}
		//		}
		
		//		public static void DefineAttributes(List<Token> tokens){
		//			for (int left = 1; left < tokens.Count - 1; left++) {
		//				if(tokens[left].text == "[" && 
		//				   (tokens[left-1].type == TokenType.PUNCTUATION || tokens[left - 1].type == TokenType.COMMENT)){
		//					int depth = 0;
		//					int right = left + 1;
		//					for(; right < tokens.Count; right++){
		//						if(tokens[right].text == "[")
		//							depth ++;
		//						else if(tokens[right].text == "]"){
		//							if(depth > 0)
		//								depth --;
		//							else
		//								break;
		//						}
		//					}
		//					CombineReplace(tokens, left, right - left + 1, TokenType.ATTRIBUTE);
		//					left = right;
		//				}
		//			}
		//		}
		
		//		public static void DefineTempletType(List<Token> tokens){
		//			//模板类
		//			for(int start = 0; start < tokens.Count - 1; start++){
		//				if(tokens[start].text == "<"){
		//					bool isTemplet = true;
		//					int end = start + 1;
		//					for(; end < tokens.Count - 1; ){
		//						if(tokens[end].type == TokenType.TYPE || tokens[end].type == TokenType.NAMESPACE || tokens[end].type == TokenType.CLASS || tokens[end].type == TokenType.IDENTIFIER){
		//							if (tokens[end + 1].text == ".")//<A.b>
		//								end += 2;
		//							else if (tokens[end + 1].text == ",")//<A, B>
		//								end += 2;
		//							else if (tokens[end + 1].text == ">")
		//								break;
		//							else{
		//								isTemplet = false;
		//								break;
		//							}
		//						}else{
		//							isTemplet = false;
		//							break;
		//						}
		//					}
		//					if(isTemplet){
		//						CombineReplace(tokens, start - 1, end - start + 3, TokenType.IDENTIFIER);
		//					}
		//				}
		//			}
		//
		//			//数组
		//			for(int left = 1; left < tokens.Count - 1; left++){
		//				if(tokens[left].text == "[" ){
		//					if(tokens[left+1].text == "]"){
		//						for(int start = left - 1; start >= 0;){
		//							if(tokens[start].type == TokenType.CLASS || tokens[start].type == TokenType.NAMESPACE ||
		//								tokens[start].type == TokenType.TYPE || tokens[start].type == TokenType.IDENTIFIER){
		//								if(start - 1 > 0 && tokens[start - 1].text == "."){
		//									start -= 2;
		//								}else{
		//									CombineReplace(tokens, start, left - start, TokenType.TYPE);
		//									break;
		//								}
		//							}
		//						}
		//					}else{
		//						if(tokens[left - 1].type == TokenType.IDENTIFIER){
		//							Token t = tokens[left - 1];
		//							t.type = TokenType.PROPERTY;
		//							tokens[left - 1] = t;
		//						}
		//					}
		//				}
		//			}
		//
		//		}
		
		//		public static List<string> FindUsingNamespace(List<Token> tokens){
		//			for(int start = 0; start < tokens.Count; start++){
		//				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "class"){//class后面的一定是类，且后面只能接{
		//					string typeName = tokens[start + 1].text;
		//					Token t = tokens[start + 1];
		//					if(!string.IsNullOrEmpty(currentNamespace)){
		//						typeName = currentNamespace + "." + typeName;
		//						t.text = typeName;
		//					}
		//					//TODO 类中类的注册
		//					RegisterNewType(typeName, currentNamespace);
		//					t.type = TokenType.TYPE;
		//					tokens[start + 1] = t;
		//				}
		//			}
		//			return usingNamespace;
		//		}
		
		//Parse的流程应该修改：
		//拆解出Token(不指名是Type还是Identifier)
		//对Tokens第二次处理，看是Namespace.Type还是Identifier(解决Namespace，类中类，类.方法，x.x.x)
		
		
		//找到类，注意，此时是根据C#规范来找的,因此不知道类的全名
		//			DefineClass (tokens);	//所有的类注册完毕
		//			DefineTempletType(tokens);//模板类会导致新的Type和新的构造函数，因此放在Property和Function之前
		//			DefineProperty (tokens);
		//			DefineFunction (tokens);
		//			DefineAttributes (tokens);
		
		//第三步。对所有脚本编译（因为需要知道基类的property）
		
		//第四步，所有Property根据作用域来确定。不确定的保留Identifier.Identifier。
		
		//			//第三步，列举出所有的类，所有Property,所有function(必须作用域处理)。不确定的保留（Identifier.Identifier），
		//
		//
		//			//把Token区分出Type还是Namespace还是Method
		//			ReplaceIdentifier (tokens, usingNamespace);

	}
}
