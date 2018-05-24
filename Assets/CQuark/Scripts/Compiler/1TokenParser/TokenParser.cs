using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
	public static class TokenParser {
		#region Keywords And BasicTypes
		private static readonly List<string> keywords = new List<string>(){
			//https://msdn.microsoft.com/zh-cn/library/x53a06bb(VS.80).aspx
			"if",
			"as",
			"is",
			"else",
			"break",
			//2017-09-15 0.7.1 补充协程
			"continue",
			"for",
			"do",
			"while",
			"trace",
			"return",
			"true",
			"false",
			"null",
			"new",
			"foreach",
			"in",
			//OO支持 新增关键字
			"class",
			"interface",
			
			"using",
			"public",
			"private",
			"static",
			"protected",//1.0.1补充
			"internal",//1.0.1补充
			"const",
			
			"try",
			"catch",
			"throw",
			
			//0.7.2 补充switch case
			"switch",
			"case",
			"default",
			
			//0.8.4 补充
			"yield",

			//1.0.1
			"typeof",

			//1.0.2
			"override",
			"get",
			"set",

			"ref",//TODO
			"out",//TODO
		};

		static readonly List<string> basicTypes = new List<string>(){
			"double",
			"long",
			"ulong",
			"float",
			"int",
			"uint",
			"short",
			"ushort",
			"byte",
			"sbyte",
			"char",
			
			"bool",
			"string",
			"void",//虽然是关键字，但还是作为Type处理
			"object",
			//2017-09-15 0.7.1 补充协程
			"IEnumerator",
			//1.0.1新增
			"Type",
		};
		#endregion

		static List<string> oriTypes = new List<string>();//原生类型，比如UnityEngine.xxx,读取生成工具

		static List<string> customTypes = new List<string>();//注册类型,西瓜声明的类型
		public static void Reload(bool reloadOrigion){
			customTypes.Clear();
			if (reloadOrigion)
				oriTypes.Clear ();
		}
		//初始化时由AppDomain调用
		public static void RegisterOriType (string type) {
			if(oriTypes.Contains(type))
				return;
			oriTypes.Add(type);
		}
		//实时编译调用
		public static void RegisterCustomType(string type){
			if(customTypes.Contains(type))
				return;
			customTypes.Add(type);
		}

		public static bool IsType (string type, List<string> nameSpace, out string fullName) {
			if(basicTypes.Contains(type) || customTypes.Contains(type) || oriTypes.Contains(type)){
				fullName = type;
				return true;
			}

			if(nameSpace != null){
				for(int i = 0; i < nameSpace.Count; i++){
					fullName = nameSpace[i] + "." + type;
					if(/*basicTypes.Contains(fullName) || */customTypes.Contains(fullName) || oriTypes.Contains(fullName)){
						return true;
					}
				}
			}
			fullName = "";
			return false;
		}

		static int FindStart (string lines, int npos, ref int lineIndex) {
			int n = npos;
			for(int i = n; i < lines.Length; i++) {
				if(lines[i] == '\n')
					lineIndex++;
				if(!char.IsSeparator(lines, i) && lines[i] != '\n' && lines[i] != '\r' && lines[i] != '\t') {
					return i;
				}
			}
			return -1;
		}

		//单纯的把所有token提出来，不区分x.y到底是namespace.class还是class.function还是class.class
		static int GetToken (string line, int nstart, out Token t, ref int lineIndex) {
			//符号解析参照:https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/namespace-alias-qualifer
			t.pos = nstart;
			t.line = lineIndex;
			t.text = " ";
			t.type = TokenType.UNKNOWN;
			if(nstart < 0) 
				return -1;
			//string
			//TODO @"
			if (line [nstart] == '\"') {
				t.text = "\"";
				int pos = nstart + 1;
				bool bend = false;
				while (pos < line.Length) {
					char c = line [pos];
					if (c == '\n') {
						throw new Exception ("查找字符串失败");
					}
					if (c == '\"') {
						t.type = TokenType.STRING;
						bend = true;
						//break;
					}
					if (c == '\\') {
						pos++;
						c = line [pos];
						if (c == '\\') {
							t.text += '\\';
							pos++;
							continue;
						} else if (c == '"') {
							t.text += '\"';
							pos++;
							continue;
						} else if (c == '\'') {
							t.text += '\'';
							pos++;
							continue;
						} else if (c == '0') {
							t.text += '\0';
							pos++;
							continue;
						} else if (c == 'a') {
							t.text += '\a';
							pos++;
							continue;
						} else if (c == 'b') {
							t.text += '\b';
							pos++;
							continue;
						} else if (c == 'f') {
							t.text += '\f';
							pos++;
							continue;
						} else if (c == 'n') {
							t.text += '\n';
							pos++;
							continue;
						} else if (c == 'r') {
							t.text += '\r';
							pos++;
							continue;
						} else if (c == 't') {
							t.text += '\t';
							pos++;
							continue;
						} else if (c == 'v') {
							t.text += '\v';
							pos++;
							continue;
						} else {
							throw new Exception ("不可识别的转义序列:" + t.text);
						}
					}
					t.text += line [pos];
					pos++;
					if (bend)
						return pos;
				}
				throw new Exception ("查找字符串失败");
			}
			//char
			else if (line [nstart] == '\'') {
				int nend = line.IndexOf ('\'', nstart + 1);
				int nsub = line.IndexOf ('\\', nstart + 1);
				while (nsub > 0 && nsub < nend) {
					nend = line.IndexOf ('\'', nsub + 2);
					nsub = line.IndexOf ('\\', nsub + 2);
				}
				if (nend - nstart + 1 < 1)
					throw new Exception ("查找字符失败");
				t.type = TokenType.VALUE;
				int pos = nend + 1;
				t.text = line.Substring (nstart, nend - nstart + 1);
				t.text = t.text.Replace ("\\\"", "\"");
				t.text = t.text.Replace ("\\\'", "\'");
				t.text = t.text.Replace ("\\\\", "\\");
				t.text = t.text.Replace ("\\0", "\0");
				t.text = t.text.Replace ("\\a", "\a");
				t.text = t.text.Replace ("\\b", "\b");
				t.text = t.text.Replace ("\\f", "\f");
				t.text = t.text.Replace ("\\n", "\n");
				t.text = t.text.Replace ("\\r", "\r");
				t.text = t.text.Replace ("\\t", "\t");
				t.text = t.text.Replace ("\\v", "\v");
				int sp = t.text.IndexOf ('\\');
				if (sp > 0) {
					throw new Exception ("不可识别的转义序列:" + t.text.Substring (sp));
				}
				if (t.text.Length > 3) {
					throw new Exception ("char 不可超过一个字节(" + t.line + ")");
				}
				return pos;
			}
			// //注释
			else if (line [nstart] == '/' && nstart < line.Length - 1 && line [nstart + 1] == '/') {
				t.type = TokenType.COMMENT;
				int enterpos = line.IndexOf ('\n', nstart + 2);
				if (enterpos < 0)
					t.text = line.Substring (nstart);
				else
					t.text = line.Substring (nstart, enterpos - nstart);
			}
			// /*注释
			else if (line [nstart] == '/' && nstart < line.Length - 1 && line [nstart + 1] == '*') {
				t.type = TokenType.COMMENT;
				int enterpos = line.IndexOf ("*/", nstart + 2);
				if (enterpos < 0)
					t.text = line.Substring (nstart);
				else
					t.text = line.Substring (nstart, enterpos - nstart + 2);
			} 
			//= == =>
			else if (line [nstart] == '=') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')//==
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '>')//=>
					t.text = line.Substring (nstart, 2);
				else//=
					t.text = line.Substring (nstart, 1);
			} 
			// !, !=
			else if (line [nstart] == '!') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else
					t.text = line.Substring (nstart, 1);
			} 
			//+ += ++
			else if (line [nstart] == '+') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '+')
					t.text = line.Substring (nstart, 2);
				else
					t.text = line.Substring (nstart, 1);
			}
			//- -= -- ->
			else if (line [nstart] == '-') {
				//负数也先作为符号处理
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '-')
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '>')
					t.text = line.Substring (nstart, 2);//TODO ->
				else
					t.text = line.Substring (nstart, 1);
			}
			//* *=
			else if (line [nstart] == '*') {
				//暂时不处理指针
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else
					t.text = line.Substring (nstart, 1);
			}
			// / /= 
			else if (line [nstart] == '/') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else
					t.text = line.Substring (nstart, 1);
			} 
			//  % %=
			else if (line [nstart] == '%') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=')
					t.text = line.Substring (nstart, 2);
				else
					t.text = line.Substring (nstart, 1);
			} 
			// > >= >> >>=
			else if (line [nstart] == '>') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=') // >=
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '>') {// >>
					if (nstart < line.Length - 2 && line [nstart + 2] == '=') 
						t.text = line.Substring (nstart, 3);//TODO >>=
					else
						t.text = line.Substring (nstart, 2);//TODO >>
				} else 
					t.text = line.Substring (nstart, 1);
			} 
			// < <= << <<=
			else if (line [nstart] == '<') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=') // <=
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '<') {// <<
					if (nstart < line.Length - 2 && line [nstart + 2] == '=') 
						t.text = line.Substring (nstart, 3);//TODO <<=
					else 
						t.text = line.Substring (nstart, 2);//TODO <<
				}
				else 
					t.text = line.Substring (nstart, 1);
			} 
			//  &,&&,&=
			else if (line [nstart] == '&') {
				//暂时不处理指针
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '&') // &&
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '=') 
					t.text = line.Substring (nstart, 2);// TODO &=
				else 
					t.text = line.Substring (nstart, 1);// TODO &
			}
			// |,||,|=
			else if (line [nstart] == '|') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '|')// ||
					t.text = line.Substring (nstart, 2);
				else if (nstart < line.Length - 1 && line [nstart + 1] == '=') 
					t.text = line.Substring (nstart, 2);// TODO |=
				else 
					t.text = line.Substring (nstart, 1);// TODO |
			}
			// ?,??
			else if (line [nstart] == '?') {
				//不支持?.和?[]，那是C#6的功能
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '?')
					t.text = line.Substring (nstart, 2);// TODO ??
				else 
					t.text = line.Substring (nstart, 1);
			} 
			//^ ^=
			else if (line [nstart] == '^') {
				//TODO
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == '=') 
					t.text = line.Substring (nstart, 2);// TODO ^=
				else
					t.text = line.Substring (nstart, 1);// TODO ^
			}
			//: ::
			else if (line [nstart] == ':') {
				t.type = TokenType.PUNCTUATION;
				if (nstart < line.Length - 1 && line [nstart + 1] == ':') 
					t.text = line.Substring (nstart, 2);// TODO::
				else 
					t.text = line.Substring (nstart, 1);// TODO 继承,三元表达式已完成
			}
			//字母，可能是类，命名空间，方法
			else if(char.IsLetter(line, nstart) || line[nstart] == '_') {
				//判断完整性
				int i = nstart + 1;
				while(i < line.Length && (char.IsLetterOrDigit(line, i) || line[i] == '_')) {
					i++;
				}
				t.text = line.Substring(nstart, i - nstart);
				//判断字母类型： 关键字 还是标识符（这里不区分Type）
				if(keywords.Contains(t.text)) {
					t.type = TokenType.KEYWORD;
					return nstart + t.text.Length;
				}
				t.type = TokenType.IDENTIFIER;
				return nstart + t.text.Length;
			}
			//其他符号 .;()[]{}~
			else if(char.IsPunctuation(line, nstart)) {
				t.type = TokenType.PUNCTUATION;
				t.text = line.Substring(nstart, 1);
				return nstart + t.text.Length;
			}
			//数字 包括0x..., 1.2, 1.2f
			else if(char.IsNumber(line, nstart)) {
				//判断数字合法性
				if(line[nstart] == '0' && line[nstart + 1] == 'x') {//0x....
					int iend = nstart + 2;
					for(int i = nstart + 2; i < line.Length; i++) {
						if(char.IsNumber(line, i)) {
							iend = i;
						}
						else {
							break;
						}
					}
					t.type = TokenType.VALUE;
					t.text = line.Substring(nstart, iend - nstart + 1);
				}
				else {
					//纯数字
					int iend = nstart;
					for(int i = nstart + 1; i < line.Length; i++) {
						if(char.IsNumber(line, i)) {
							iend = i;
						}
						else {
							break;
						}
					}
					t.type = TokenType.VALUE;
					int dend = iend + 1;
					if(dend < line.Length && line[dend] == '.') {
						int fend = dend;
						for(int i = dend + 1; i < line.Length; i++) {
							if(char.IsNumber(line, i)) {
								fend = i;
							}
							else {
								break;
							}
						}
						if(fend + 1 < line.Length && line[fend + 1] == 'f') {
							t.text = line.Substring(nstart, fend + 2 - nstart);
							
						}
						else {
							t.text = line.Substring(nstart, fend + 1 - nstart);
						}
						//.111
						//.123f
					}
					else {
						if(dend < line.Length && line[dend] == 'f') {
							t.text = line.Substring(nstart, dend - nstart + 1);
						}
						else {
							t.text = line.Substring(nstart, dend - nstart);
						}
					}
					
				}
				return nstart + t.text.Length;
			}
			//不可识别逻辑
			else {
				int i = nstart + 1;
				while(i < line.Length - 1 && char.IsSeparator(line, i) == false && line[i] != '\n' && line[i] != '\r' && line[i] != '\t') {
					i++;
				}
				t.text = line.Substring(nstart, i - nstart);
				return nstart + t.text.Length;
			}
			
			return nstart + t.text.Length;
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
		[Obsolete]
		static void ReplaceIdentifier(List<Token> tokens, List<string> usingNamespace){
			for(int start = 0; start < tokens.Count; start++){
				if(tokens[start].type == TokenType.IDENTIFIER){
					for(int end = start; end < tokens.Count;){
						string fullname =  Combine(tokens, start, end - start + 1);
						if(IsType(fullname, usingNamespace, out fullname)){
							Token t = CombineReplace(tokens, start, end - start + 1, TokenType.TYPE);
							t.text = fullname;
							tokens[start] = t;
							start--;//先不判断下一个，因为要合并类中类
							break;
						}

						if(end + 2 < tokens.Count && tokens[end + 1].text == "." && tokens[end+2].type == TokenType.IDENTIFIER){
							end += 2;
						}else{
							break;
						}
					}
				}else if(tokens[start].type == TokenType.TYPE){
					//合并类中类
					if(start + 2 < tokens.Count && tokens[start + 1].text == "." && tokens[start + 2].type == TokenType.IDENTIFIER){
						string fullname =  Combine(tokens, start, 3);
						if(IsType(fullname, usingNamespace, out fullname)){
							Token t = CombineReplace(tokens, start, 3, TokenType.TYPE);
							t.text = fullname;
							tokens[start] = t;
							start--;
						}
					}
				}
			}


//			if(ContainsType(t.text)) { //foreach (string s in types)
//				while(line[i] == ' ' && i < line.Length) {
//					i++;
//				}
//				if(line[i] == '<') { /*  || line[i] == '['*/
//					int dep = 0;
//					string text = t.text;
//					while(i < line.Length) {
//						if(line[i] == '<')
//							dep++;
//						if(line[i] == '>') 
//							dep--;
//						if(line[i] == ';' || line[i] == '(' || line[i] == '{') {
//							break;
//						}
//						if(line[i] != ' ') 
//							text += line[i];
//						i++;
//						if(dep == 0) {
//							t.text = text;
//							break;
//						}
//					}
//					//if (types.Contains(t.text))//自动注册
//					{
//						t.type = TokenType.TYPE;
//						return i;
//					}
//				}
//				else {
//					t.type = TokenType.TYPE;
//					return nstart + t.text.Length;
//				}
//			}
//			while(i < line.Length && line[i] == ' ') {
//				i++;
//			}
//			if(i < line.Length && (line[i] == '<'/* || line[i] == '['*/)) {//检查特别类型
//				int dep = 0;
//				string text = t.text;
//				while(i < line.Length) {
//					if(line[i] == '<') {
//						dep++;
//						i++;
//						text += '<';
//						continue;
//					}
//					if(line[i] == '>') {
//						dep--;
//						i++;
//						if(dep == 0) {
//							t.text = text + '>';
//							break;
//						}
//						continue;
//					}
//					Token tt;
//					int nnstart = FindStart(line, i, ref lineIndex);
//					i = GetToken(line, nnstart, out tt, ref lineIndex);
//					if(tt.type != TokenType.IDENTIFIER && tt.type != TokenType.TYPE && tt.text != ",") {
//						break;
//					}
//					text += tt.text;
//				}
//				if(ContainsType(t.text)) {
//					t.type = TokenType.TYPE;
//					return i;
//					
//				}
//				else if(dep == 0) {
//					t.type = TokenType.IDENTIFIER;
//					return i;
//				}
//			}
//			if(tokens.Count >= 3 && t.type == TokenType.PUNCTUATION && t.text == ">"
//			   && tokens[tokens.Count - 1].type == TokenType.TYPE
//			   && tokens[tokens.Count - 2].type == TokenType.PUNCTUATION && tokens[tokens.Count - 2].text == "<"
//			   && tokens[tokens.Count - 3].type == TokenType.IDENTIFIER) {//模板函数调用,合并之
//				string ntype = tokens[tokens.Count - 3].text + tokens[tokens.Count - 2].text + tokens[tokens.Count - 1].text + t.text;
//				t.type = TokenType.IDENTIFIER;
//				t.text = ntype;
//				t.pos = tokens[tokens.Count - 2].pos;
//				t.line = tokens[tokens.Count - 2].line;
//				tokens.RemoveAt(tokens.Count - 1);
//				tokens.RemoveAt(tokens.Count - 1);
//				tokens.RemoveAt(tokens.Count - 1);
//				tokens.Add(t);
//				continue;
//			}


			//TODO 用作用域（Stack)去判断

			//C# 允许这种写法 A A = new A();//fuck
			//但是int int这种又不行
			//1A B, A一定是Type，B一定是Property
			//2typeof(A)，A一定是Type
			//3<A>，<A,B>等 A一定是Type
			//4A[] A一定是Type
			//A.B，A.B.C，如果上面有A的Property（B A;），那么A一定是Property，否则A是类或者命名空间
			//new XX.X()一定是构造函数

			//A.B到底是类中类还是A.成员没有办法知道，必须在Expression级处理
//			if(tokens.Count >= 1 && t.type == TokenType.TYPE && tokens[tokens.Count - 1].type == TokenType.TYPE) {//Type Type 不可能，为重名
//				t.type = TokenType.IDENTIFIER;
//				tokens.Add(t);
//				continue;
//			}

			//编译分几步

			//2，把所有类提取出来，连同其namespace写到注册字典里
			//3，列举出所有的类（保留unknown.unknow），所有Property,所有function(必须作用域处理)
			//4，通过字典和反射确定 类名及所有Unknown
			//5,由compile expression（也需要重构）处理
		}
		[Obsolete]
		public static bool IsType(List<Token> tokens, int index){
			//typeof()

			//构造函数

			//后接Identifier
			if(index == 0)
				return true;
			if(tokens[index - 1].type == TokenType.PUNCTUATION){
				if(tokens[index - 1].text == ";" ||tokens[index - 1].text == "}" 
					||tokens[index - 1].text == "{" || tokens[index - 1].text == "=>")
					return true;
			}
			if(tokens[index - 1].type == TokenType.KEYWORD){
				if(tokens[index - 1].text == "public" ||tokens[index - 1].text == "private"
					||tokens[index - 1].text == "protected" || tokens[index - 1].text == "static")
					return true;
			}
			return false;
		}


		public static List<Token> SplitToken(string lines){
			if(lines.Length > 0 && lines[0] == 0xFEFF) {
				//windows下用记事本写，会在文本第一个字符出现BOM（65279）
				lines = lines.Substring(1);
			}

			int lineIndex = 1;
			List<Token> tokens = new List<Token>();
			int n = 0;
			while(n >= 0) {
				Token t;
				t.line = lineIndex;
				
				int nstart = FindStart(lines, n, ref lineIndex);
				t.line = lineIndex;
				int nend = GetToken(lines, nstart, out t, ref lineIndex);
				if(nend >= 0) {
					for(int i = nstart; i < nend; i++) {
						if(lines[i] == '\n')
							lineIndex++;
					}
				}
				n = nend;
				if(t.pos == -1 && t.type == TokenType.UNKNOWN)
					continue;
				if(t.type == TokenType.COMMENT)//所有注释不加入List（因为注释可以夹杂在表达式中导致解析错误）
					continue;
				tokens.Add(t);
			}
			return tokens;
		}

		public static List<string> DefineNamespace(List<Token> tokens, out string currentNamespace){
			List<string> usingNamespace = new List<string>();
			for(int start = 0; start < tokens.Count; start++){
				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "using"){
					//using有3种用法。
					//1控制Dispose
					if(tokens[start + 1].type == TokenType.PUNCTUATION && tokens[start + 1].text == "("){//TODO 暂时不处理
						
					}
					//2命名空间 using System.IO;//后面是命名空间
					//3别名 using Project = PC.MyCompany.Project;//后面是Identifier
					else{
						//由于此时还不确定后面的类型是Type还是Namespace或者是未定义的Identifier，所以判断方式是先出现=还是;
						int nextEqual = FindToken(tokens, start, "=");
						int nextSemicolon = FindToken(tokens, start, ";");
						if(nextEqual > 0 && nextEqual < nextSemicolon){
							//别名
							//TODO 暂时不处理
						}else{
							//命名空间
							string nameSpace = CombineReplace(tokens, start + 1, nextSemicolon - start - 1, TokenType.NAMESPACE).text;
							if(!usingNamespace.Contains(nameSpace))
								usingNamespace.Add(nameSpace);
						}
					}
				}
			}
			
			currentNamespace = "";
			for(int start = 0; start < tokens.Count; start++){
				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "namespace"){//namespace后接命名空间，可以包含多个.
					for(int i = start + 1; i < tokens.Count; i++){
						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
							string nameSpace = CombineReplace(tokens, start, i - start - 1, TokenType.NAMESPACE).text;
							currentNamespace = nameSpace;
							if(!usingNamespace.Contains(nameSpace))
								usingNamespace.Add(nameSpace);
							break;
						}
					}
				}
			}
			return usingNamespace;
		}

		public static void DefineClass(List<Token> tokens){
			
			for(int start = 0; start < tokens.Count; start++){
				if(tokens[start].type == TokenType.KEYWORD && tokens[start].text == "class"){//class后面的一定是类，且后面只能接{或:xxx{
					Token token = tokens[start + 1];
					token.type = TokenType.CLASS;
					tokens[start+1] = token;

					if(start + 3 < tokens.Count && tokens[start + 2].text == ":"){
						for(int i = start + 3; i < tokens.Count; i++){
							if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == "{"){
								string baseClass = CombineReplace(tokens, start + 3, i - start - 3, TokenType.CLASS).text;
								start += 3;
								break;
							}
						}
					}
				}
				else if(tokens[start].text == "typeof" && tokens[start + 1].text == "("){
					//TODO 注意要区分T模板
					for(int i = start + 2; i < tokens.Count; i++){
						if(tokens[i].type == TokenType.PUNCTUATION && tokens[i].text == ")"){
							string baseClass = CombineReplace(tokens, start + 2, i - start - 2, TokenType.CLASS).text;
							start = i + 1;
							break;
						}
					}
				}
				else if(tokens[start].text == "as" || tokens[start].text == "is"){
					if(tokens[start - 1].type == TokenType.IDENTIFIER ){
						Token t = tokens[start - 1];
						t.type = TokenType.PROPERTY;
						tokens[start - 1] = t;
					}
					for(int i = start + 1; i < tokens.Count; i++){
						if(tokens[i].type == TokenType.IDENTIFIER ){
							if(tokens[i + 1].text == ".")
								i += 2;
						}else{
							CombineReplace(tokens, start + 1, i - start - 1, TokenType.TYPE);
							start = i;
							break;
						}
					}
				}
			}

			for (int start = 0; start < tokens.Count; start++) {
				//(I)xxx强转
			}
		}

		public static void DefineProperty(List<Token> tokens){
			//三个函数顺序执行
			for(int start = 1; start < tokens.Count; start++){
				if(tokens[start].type == TokenType.IDENTIFIER && basicTypes.Contains(tokens[start].text)){//class后面的一定是类，且后面只能接{或:xxx{
					Token token = tokens[start];
					token.type = TokenType.TYPE;
					tokens[start] = token;
				}
			}

			for(int start = 1; start < tokens.Count - 1; start++){
				if(tokens[start].type == TokenType.IDENTIFIER && (tokens[start - 1].type == TokenType.IDENTIFIER || tokens[start - 1].type == TokenType.TYPE)
				   && tokens[start + 1].text != "("){//TYPE 后一定是Property
					Token token = tokens[start];
					token.type = TokenType.PROPERTY;
					tokens[start] = token;
				}
			}
			for(int start = 0; start < tokens.Count - 4; start++){
				if(tokens[start].text == "[" && tokens[start + 1].text == "]" && tokens[start + 2].type != TokenType.PUNCTUATION){
					Token t = tokens[start + 2];
					t.type = TokenType.PROPERTY;
					tokens[start + 2] = t;
				}
			}
			//必须放在模版<>判断之后)
			for (int start = 1; start < tokens.Count; start++) {
				if(tokens[start].type == TokenType.PUNCTUATION){
					if (tokens [start].text == ")" || tokens [start].text == "," || tokens [start].text == "]" || tokens[start].text == ";"
					    || tokens[start].text == ">" || tokens[start].text == "<" || tokens[start].text == ">=" || tokens[start].text == "<=" || tokens[start].text == "==" || tokens[start].text == "!=" 
					    || tokens[start].text == "+=" || tokens[start].text == "-=" || tokens[start].text == "*=" || tokens[start].text == "/=" || tokens[start].text == "%=" 
					    || tokens[start].text == "=" || tokens[start].text == "&&" || tokens[start].text == "||" || tokens[start].text == "&=" || tokens[start].text == "|=") {
						if(tokens[start - 1].type == TokenType.IDENTIFIER){
							Token t = tokens[start - 1];
							t.type = TokenType.PROPERTY;
							tokens[start - 1] = t;
						}
					}
				}
			}
			//TODO 作用域，并且获取基类

			for (int end = tokens.Count - 1; end >= 0; end--) {
				if(tokens[end].type == TokenType.PROPERTY && tokens[end - 1].type == TokenType.IDENTIFIER){//Property前面的Identifier一定是Type，可能是x.y.z a
					for(int start = end - 1; start >= 0; ){
						if(tokens[start - 1].text == "." && tokens[start - 2].type == TokenType.IDENTIFIER){
							start -= 2;
						}else{
							CombineReplace(tokens, start, end - start, TokenType.TYPE);
							break;
						}
					}
				}
			}
		}

		public static void DefineFunction(List<Token> tokens){
			for(int start = 0; start < tokens.Count - 1; start++){
				if(tokens[start + 1].text == "(" && tokens[start].type == TokenType.IDENTIFIER ){
					//public
					Token token = tokens[start];
					token.type = TokenType.FUNCTION;
					tokens[start] = token;
				}
			}
		}

		public static void DefineAttributes(List<Token> tokens){
			for (int left = 1; left < tokens.Count - 1; left++) {
				if(tokens[left].text == "[" && 
				   (tokens[left-1].type == TokenType.PUNCTUATION || tokens[left - 1].type == TokenType.COMMENT)){
					int depth = 0;
					int right = left + 1;
					for(; right < tokens.Count; right++){
						if(tokens[right].text == "[")
							depth ++;
						else if(tokens[right].text == "]"){
							if(depth > 0)
								depth --;
							else
								break;
						}
					}
					CombineReplace(tokens, left, right - left + 1, TokenType.ATTRIBUTE);
					left = right;
				}
			}
		}

		public static void DefineTempletType(List<Token> tokens){
			//模板类
			for(int start = 0; start < tokens.Count - 1; start++){
				if(tokens[start].text == "<"){
					bool isTemplet = true;
					int end = start + 1;
					for(; end < tokens.Count - 1; ){
						if(tokens[end].type == TokenType.TYPE || tokens[end].type == TokenType.NAMESPACE || tokens[end].type == TokenType.CLASS || tokens[end].type == TokenType.IDENTIFIER){
							if (tokens[end + 1].text == ".")//<A.b>
								end += 2;
							else if (tokens[end + 1].text == ",")//<A, B>
								end += 2;
							else if (tokens[end + 1].text == ">")
								break;
							else{
								isTemplet = false;
								break;
							}
						}else{
							isTemplet = false;
							break;
						}
					}
					if(isTemplet){
						CombineReplace(tokens, start - 1, end - start + 3, TokenType.IDENTIFIER);
					}
				}
			}

			//数组
			for(int left = 1; left < tokens.Count - 1; left++){
				if(tokens[left].text == "[" ){
					if(tokens[left+1].text == "]"){
						for(int start = left - 1; start >= 0;){
							if(tokens[start].type == TokenType.CLASS || tokens[start].type == TokenType.NAMESPACE ||
								tokens[start].type == TokenType.TYPE || tokens[start].type == TokenType.IDENTIFIER){
								if(start - 1 > 0 && tokens[start - 1].text == "."){
									start -= 2;
								}else{
									CombineReplace(tokens, start, left - start, TokenType.TYPE);
									break;
								}
							}
						}
					}else{
						if(tokens[left - 1].type == TokenType.IDENTIFIER){
							Token t = tokens[left - 1];
							t.type = TokenType.PROPERTY;
							tokens[left - 1] = t;
						}
					}
				}
			}

		}

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
		public static List<Token> Parse (string lines) {
			//第一步，找出所有的token
			//此时只有string,namespace,标点,关键字，数值,Identifier。没有Type,Property,Function
			//所有注释不加入List（因为注释可以夹杂在表达式中导致解析错误）
			List<Token> tokens = SplitToken (lines);

			if (tokens == null) {
				DebugUtil.LogWarning ("没有解析到代码");
				return null;
			}


//			//第二步，找到引用的命名空间，及当前的命名空间。注册到字典
//			List<string> usingNamespace = FindUsingNamespace (tokens);
			string currentNamespace = "";
			List<string> usingNamespace = DefineNamespace (tokens, out currentNamespace);
			//找到类，注意，此时是根据C#规范来找的,因此不知道类的全名
			DefineClass (tokens);	//所有的类注册完毕
			DefineTempletType(tokens);//模板类会导致新的Type和新的构造函数，因此放在Property和Function之前
			DefineProperty (tokens);
			DefineFunction (tokens);
			DefineAttributes (tokens);

			//第三步。对所有脚本编译（因为需要知道基类的property）

			//第四步，所有Property根据作用域来确定。不确定的保留Identifier.Identifier。

//			//第三步，列举出所有的类，所有Property,所有function(必须作用域处理)。不确定的保留（Identifier.Identifier），
//
//
//			//把Token区分出Type还是Namespace还是Method
//			ReplaceIdentifier (tokens, usingNamespace);

			return tokens;
		}

	}
}