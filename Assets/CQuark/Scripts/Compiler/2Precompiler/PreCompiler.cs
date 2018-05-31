using System.Collections.Generic;
using System;
using CQuark.Compile;

namespace CQuark.Compile{
	public class PreCompiler  {
		class ClassHeader{
			public string fullName;
			public int depth;
			public string[] split{get; private set;}
			public ClassHeader(string _fullName){
				fullName = _fullName;
				depth = 0;
				string[] s = _fullName.Split('.');
				split = new string[s.Length];
				for(int i = 0; i < s.Length; i++){
					string combine = "";
					for(int j = 0; j < s.Length - i; j++){
						combine += s[j];
						if(j != s.Length - i -1)
							combine += ".";
					}
					split[i] = combine;
				}
			}
		}

		//找到类型，写入AppDomain和CustomTypes
		//如果类里有类那么依次加入
		public static void RegisterCQClass(string fileName, IList<Token> tokens){
			string Namespace = "";
			List<string> classStack = new List<string> ();
			string outterClass = "";

			for (int i = 0; i < tokens.Count; i++) {
				if(tokens[i].type == TokenType.KEYWORD ){
					if(tokens[i].text == "namespace"){
						int end = FindToken(tokens, i, "{");
						Namespace = CombineReplace(tokens, i + 1, end - i - 1, TokenType.NAMESPACE).text;
#if CQUARK_DEBUG
						DebugUtil.Log("Namespace = " + Namespace);
#endif
						continue;
					}else if(tokens[i].text == "using"){
						int end = FindToken(tokens, i, ";");
						string Using = CombineReplace(tokens, i + 1, end - i - 1, TokenType.NAMESPACE).text;
#if CQUARK_DEBUG
						DebugUtil.Log("Using = " + Using);
#endif
						continue;
					}
				}
				if (tokens [i].type == TokenType.KEYWORD && (tokens [i].text == "class" || tokens [i].text == "interface" || tokens[i].text == "struct")) {
					string name = tokens [i + 1].text;
					outterClass = name;

					#if CQUARK_DEBUG
					DebugUtil.Log("(PreCompiler)findclass:" + name + "(" + i + ")");
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
						Type_Class typeClass = new Type_Class(fullName, tokens [i].text == "interface", fileName);
						AppDomain.RegisterType(typeClass);
					}

					//这个Token的名字补足
					Token tok = tokens[i+1];
					tok.text = fullName;
					tok.type = TokenType.CLASS;
					tokens[i+1] = tok;
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
		
		//类全部注册完毕后，把类名补足,Identifier替换成Type
		public static void IdentifyType(string fileName, IList<Token> tokens){
			DefineAttributes(tokens);

			List<string> usingNamespace = null;
			for(int i = 0; i < tokens.Count; i++){
				if(tokens[i].type == TokenType.NAMESPACE){
					if(usingNamespace == null)
						usingNamespace = new List<string>();

					//C#中命名空间如果using System.Collections.Generic。也会同时using了System,System.Collections
					ClassHeader header = new ClassHeader(tokens[i].text);
					for(int j = 0; j < header.split.Length; j++){
						if(!usingNamespace.Contains(header.split[j]))
							usingNamespace.Add(header.split[j]);
					}
				}
			}
			string[] usingNamespaceArray = usingNamespace != null ? usingNamespace.ToArray() : null;

			Stack<ClassHeader> classHeaders = null;

			//目前处理方法是找Identifier是否是注册过
			//注意，可能造成错误 eg:
			//GameObject Debug; 
			//Debug.Translate();//这里的Debug实际是变量，但是会被当做类来看待，暂时没想到好的解决方法
			//所以类命名尽量大写，变量命名尽量小写
			int begin = 0;
			for(; begin < tokens.Count; begin++){
				if(tokens[begin].type == TokenType.IDENTIFIER){
					if(begin > 0){
						if(tokens[begin-1].type == TokenType.TYPE)
							continue; //Type Type后面的一定是变量
						if(tokens[begin-1].text == ".")
							continue;//X.b b一定是变量或方法
					}
					//找到连着的A.B.C
					int end = begin;
					while(end < tokens.Count - 2){
						if(tokens[end + 1].text == "." && tokens[end + 2].type == TokenType.IDENTIFIER)
							end += 2;
						else
							break;
					}
					//从后往前，直到是类就设置为Type
					while(end >= begin){
						string name = "";
						for(int i = begin; i <= end; i++){
							name += tokens[i].text;
						}

						string fullName = "";
						//先判断外层类.比如类是Namespace.A.B.C 那么C类里的类应该先判断Namespace.A.B+X, Namespace.A.+X, Namespace.+X, X
						if(classHeaders != null && classHeaders.Count > 0 && IsType(name, classHeaders.Peek().split, out fullName)){
							CombineReplace(tokens, begin, end - begin + 1, TokenType.TYPE, fullName);
							break;
						}

						//在判断name是否是类
						if(AppDomain.ContainsType(name)){
							CombineReplace(tokens, begin, end - begin + 1, TokenType.TYPE);
							break;
						}
						//最后加命名空间判断Namespace1.X,Namespace2.X
						if(usingNamespace != null && IsType(name, usingNamespaceArray, out fullName)){
							CombineReplace(tokens, begin, end - begin + 1, TokenType.TYPE, fullName);
							break;
						}

						end -= 2;
					}
				}
				if(tokens[begin].type == TokenType.CLASS){
					if(classHeaders == null)
						classHeaders = new Stack<ClassHeader>();
					classHeaders.Push(new ClassHeader(tokens[begin].text));
				}
				if(tokens[begin].type == TokenType.PUNCTUATION){
					if(classHeaders == null || classHeaders.Count == 0)//对paragraph来说，这里是空
						continue;
					if(tokens[begin].text == "{"){
						classHeaders.Peek().depth++;
					}else if(tokens[begin].text == "}"){
						classHeaders.Peek().depth--;
						if(classHeaders.Peek().depth <= 0)
							classHeaders.Pop();
					}
				}
			}

			//如果有基类的话，指定基类
			DefineBaseTypes(tokens);

			//Identifier<Type ?? >合并成Identifier
			//Type<??>合并成Type
			CombineTempletType(tokens);

			//为了后面解析表达式方便，把一些类型做调整
			for(int i = 0; i < tokens.Count; i++){
				Token tok = tokens[i];
				if(tok.type == TokenType.NAMESPACE){
					tok.type = TokenType.IDENTIFIER;
					tokens[i] = tok;
				}
				else if(tok.type == TokenType.CLASS){
					tok.type = TokenType.TYPE;
					tokens[i] = tok;
				}
				else if(tok.type == TokenType.KEYWORD && tok.text == "void"){
					tok.type = TokenType.TYPE;
					tokens[i] = tok;
				}
			}
		}
		//定义Attribute
		static void DefineAttributes(IList<Token> tokens){
			for (int left = 0; left < tokens.Count - 1; left++) {
				if(tokens[left].text == "["){
					if(left > 0 && tokens[left-1].type != TokenType.PUNCTUATION || tokens[left - 1].type != TokenType.ATTRIBUTE)
						continue;
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
		//指向基类
		static void DefineBaseTypes(IList<Token> tokens){
			for(int i = 0; i < tokens.Count; i++){
				if(tokens[i].type == TokenType.CLASS){//如果是paragraph走到这里也无所谓
					//基类
					List<string> strBase = null;
					int ibegin = i + 1;
					
					if(tokens[ibegin].text == ":") {
						strBase = new List<string>();
						ibegin++;
					}
					while(tokens[ibegin].text != "{") {
						//此时类已经合并起来了
						if(tokens[ibegin].type == TokenType.TYPE) {
							strBase.Add(tokens[ibegin].text);
						}
						ibegin++;
					}
					
					if(strBase != null && strBase.Count > 0) {
						Type_Class typeClass = CQuark.AppDomain.GetTypeByKeywordQuiet(tokens[i].text) as Type_Class;
						List<IType> baseTypes = new List<IType>();
						foreach(string t in strBase) {
							IType type = CQuark.AppDomain.GetTypeByKeyword(t);
							baseTypes.Add(type);
						}
						typeClass.SetBaseType(baseTypes);
					}
					
					//					Token tok = tokens[i];
					//					tok.type = TokenType.TYPE;
					//					tokens[i] = tok;
				}
			}
		}
		//合并数组、模板
		static void CombineTempletType(IList<Token> tokens){
			//先合并数组，因为[]里不能套<>或[]，而<>里可以套[]
			for(int start = 1; start < tokens.Count - 1; start++){
				if(tokens[start - 1].type == TokenType.TYPE && tokens[start].text == "["){
					bool isArray = true;
					int end = start + 1;
					int dimension = 1;

					string s = tokens[start - 1].text + tokens[start].text;

					for(; end < tokens.Count - 1; ){
						s += tokens[end].text;

						if (tokens[end].text == ","){//多维数组
							end += 1;
							dimension ++;
						}
						else if (tokens[end].text == "]"){//直接break即可，如果有int[][]，那么下一个循环会走到，即：int[]是一个类，（int[]）[]是一个类
							break;
						}
						else{
							isArray = false;
							break;
						}
					}
					if(isArray){
						Type t = AppDomain.GetTypeByKeywordQuiet(tokens[start - 1].text).typeBridge.type;
//						CombineReplace(tokens, start - 1, end - start + 2, TokenType.TYPE);//XX[,]
						string keywords = Combine(tokens, start - 1, end - start + 2);
						if(!AppDomain.ContainsType(keywords)){
							Type tArray = dimension == 1 ? t.MakeArrayType() : t.MakeArrayType(dimension);
							AppDomain.RegisterType(tArray, keywords);
							DebugUtil.Log(tArray.ToString() + keywords);
						}
					}
				}
			}
			//再合并List<>,Queue<>,Stack<>,Dictionary<,>等
			bool hasGeneric = false;
			List<Type> types = new List<Type>();//避免GC，放外面
			do {
				for (int start = 1; start < tokens.Count - 1; start++) {
					if (tokens [start].text == "<") {
						bool isTemplet = true;
						int end = start + 1;
						types.Clear();
						for (; end < tokens.Count - 1;) {
							if (tokens [end].type == TokenType.TYPE) {// || tokens[end].type == TokenType.IDENTIFIER){
								if(tokens [start - 1].type == TokenType.TYPE)//如果是List，Queue,Dictionary之类的模版类型，需要再次注册
									types.Add(AppDomain.GetTypeByKeyword(tokens[end].text).typeBridge.type);
								if (tokens [end + 1].text == ",")//<A, B>
									end += 2;
								else if (tokens [end + 1].text == ">")
									break;
								else {
									isTemplet = false;
									break;
								}
							} else {
								isTemplet = false;
								break;
							}
						}
						if (isTemplet) {
							if(tokens [start - 1].type == TokenType.TYPE){
								Type func = AppDomain.GetTypeByKeyword(tokens [start - 1].text).typeBridge.type;
								Type genType = func.MakeGenericType(types.ToArray());
								CombineReplace (tokens, start - 1, end - start + 3, TokenType.TYPE);//Type看前一个token。比如List<x,y>是类型
								AppDomain.RegisterType(genType, tokens[start - 1].text);
								DebugUtil.Log(genType.ToString());
							}else{
								CombineReplace (tokens, start - 1, end - start + 3, TokenType.IDENTIFIER);//Type看前一个token。比如GetComponent<x>是标识符
							}
							hasGeneric = true;
						}
					}
				}
			} while(hasGeneric);//如果有<>，那么可能一次脱不完，比如Dictionary<int, List<int>>
		}

#region Utility
		static bool IsType(string name, string[] header, out string fullName){
			fullName = "";
			for(int i = 0; i < header.Length; i++){
				fullName = header[i] + "." + name;
				if(AppDomain.ContainsType(fullName)){
					return true;
				}
			}
			return false;
		}

		static int FindToken(IList<Token> tokens, int npos, TokenType type){
			for(int i = npos; i < tokens.Count; i++) {
				if(tokens[i].type == type)
					return i;
			}
			return -1;
		}
		
		static int FindToken(IList<Token> tokens, int npos, string text){
			for(int i = npos; i < tokens.Count; i++) {
				if(tokens[i].text == text)
					return i;
			}
			return -1;
		}
		
		//把tokens的start位开始合并到一个token里
		static Token CombineReplace(IList<Token> tokens, int start, int length, TokenType type){
			Token t = tokens [start];//line和pos继承
			for (int i = 1; i < length; i++) {
				t.text += tokens[start + 1].text;
				tokens.RemoveAt(start + 1);
			}
			t.type = type;
			tokens [start] = t;
			return t;
		}

		static Token CombineReplace(IList<Token> tokens, int start, int length, TokenType type, string overwriteText){
			Token t = tokens [start];//line和pos继承
			for (int i = 1; i < length; i++) {
				tokens.RemoveAt(start + 1);
			}
			t.type = type;
			t.text = overwriteText;
			tokens [start] = t;
			return t;
		}
		
		static string Combine(IList<Token> tokens, int start, int length){
			string ret = tokens[start].text;
			for (int i = 1; i < length; i++) {
				ret += tokens[start + i].text;
			}
			return ret;
		}
#endregion
	}
}
