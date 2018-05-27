using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark {
	public static class TokenSpliter {
		#region Keywords
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

			"namespace",//180525新增
			"struct",//180525新增
			"partial",//180526新增
			"void", //180526新增
			
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
		#endregion

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

		//第一步，找出所有的token
		//此时只有string,namespace,标点,关键字，数值,Identifier。没有Type,Property,Function
		//所有注释不加入List（因为注释可以夹杂在表达式中导致解析错误）
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

	}
}