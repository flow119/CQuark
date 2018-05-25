using System.Collections.Generic;
using CQuark;

namespace CQuark{
	//外部调用编译功能只调用这一个类
	//如果是编译项目的话，那么编译出的结果是很多IType，他们会被直接写到AppDomain中

	//完整运行一个西瓜项目分以下几步
	//1注册所有原生类型（通过工具找）到AppDomain
	//2.1把整个项目里所有CQuark文件转成Token(此时还不去找类，不合并标识符),删除注释
	//2.2把所有Token里的class注册到AppDomain里					//如果是编译块的话，跳过这一步
	//所有文件都这么处理//针对Project
	//2.3a找命名空间，补足所有的类									//如果是编译块的话，跳过这一步
	//2.3b合并命名空间.类，类.类，标识为Type
	//3把所有Tokens再编成Expression，加到对应的IType里			
	//4执行需要的IType
	public class CQ_Compiler {
		const bool USE_NEW = false;
		//编译整个项目
		//如果不是特殊情况，这个函数一般只需要调用一次
		//因为你编译的CQuark引用到了别的类，而别的类没有编译过的话会报错
		//所以最好一次编译所有CQuark文件（整个项目）
		public static void CompileProject(string path, string pattern = "*.cs") {
			string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
			CompileProject(files);
		}
		public static void CompileProject (string[] filePaths) {
			if (USE_NEW) {

				Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
				//分割Token，注册所有类
				foreach(string filePath in filePaths) {
					//2.1 分割成Token(此时只有Keyword,Identifier,Punction)
					if(project.ContainsKey(filePath))
						continue;
					string text = System.IO.File.ReadAllText(filePath);
					List<Token> tokens = TokenSpliter.SplitToken(text);
					project.Add(filePath, tokens);

					//2.2把所有代码里的类注册一遍
					PreCompiler.RegisterCQClass(filePath, tokens);
				}

				foreach(KeyValuePair<string, IList<Token>> f in project) {
					//预处理符号
					for(int i = 0; i < f.Value.Count; i++) {
						if(f.Value[i].type == TokenType.IDENTIFIER && CQ_TokenParser.ContainsType(f.Value[i].text)) {//有可能预处理导致新的类型
							if(i > 0 && (f.Value[i - 1].type == TokenType.TYPE || f.Value[i - 1].text == ".")) {
								continue;
							}
							Token rp = f.Value[i];
							rp.type = TokenType.TYPE;
							f.Value[i] = rp;
						}
					}
					CompileOneFile(f.Key, f.Value);
				}
			} else {
				//2.1 分割成Token
				Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
				foreach(string filePath in filePaths) {
					if(project.ContainsKey(filePath))
						continue;
					string text = System.IO.File.ReadAllText(filePath);
					List<Token> tokens = CQ_TokenParser.Parse(text);
					project.Add(filePath, tokens);
				}
				//2.2把所有代码里的类注册一遍
				foreach(KeyValuePair<string, IList<Token>> f in project) {
					IList<IType> types = Precompile.FilePreCompile(f.Key, f.Value);
					foreach(var type in types) {
						AppDomain.RegisterCQType(type);
					}
				}
				foreach(KeyValuePair<string, IList<Token>> f in project) {
					//预处理符号
					for(int i = 0; i < f.Value.Count; i++) {
						if(f.Value[i].type == TokenType.IDENTIFIER && CQ_TokenParser.ContainsType(f.Value[i].text)) {//有可能预处理导致新的类型
							if(i > 0 && (f.Value[i - 1].type == TokenType.TYPE || f.Value[i - 1].text == ".")) {
								continue;
							}
							Token rp = f.Value[i];
							rp.type = TokenType.TYPE;
							f.Value[i] = rp;
						}
					}
					CompileOneFile(f.Key, f.Value);
				}
			}
		}
		//编译单个文件
		//如果调用这个函数，你必须确保在此之前已经注册了这个CQ需要引用的所有类
		//除非你完全理解了这个函数的意思，否则请调用CompileProject
		// 这里的filename只是为了编译时报错可以看到出错文件
		public static void CompileOneFile(string fileName, string text){
			if (USE_NEW) {
				//2.1 分割成Token
				List<Token> tokens = TokenSpliter.SplitToken(text);

				//2.2把所有代码里的类注册一遍
				PreCompiler.RegisterCQClass(fileName, tokens);

				CompileOneFile(fileName, tokens);
			} else {
				//2.1 分割成Token
				List<Token> tokens = CQ_TokenParser.Parse(text);
				CompileOneFile(fileName, tokens);
			}
		}
		private static void CompileOneFile(string fileName, IList<Token> tokens){
			if (USE_NEW) {
				//到这里，所有会用到的类都注册过了，可以用AppDomain判断类了
				//2.3 补足命名空间
				DebugUtil.Log ("File_CompilerToken:" + fileName);
				IList<IType> types = Precompile.FileCompile (fileName, tokens);
				foreach (var type in types) {
					if (AppDomain.GetTypeByKeywordQuiet (type.keyword) == null)
						AppDomain.RegisterCQType (type);
				}
				//2.3
			} else {
				DebugUtil.Log ("File_CompilerToken:" + fileName);
				IList<IType> types = Precompile.FileCompile (fileName, tokens);
				foreach (var type in types) {
					if (AppDomain.GetTypeByKeywordQuiet (type.keyword) == null)
						AppDomain.RegisterCQType (type);
				}

			}
		}

		//编译一个函数块
		public static ICQ_Expression CompileBlock(string text){
			if (USE_NEW) {
				List<Token> tokens = TokenSpliter.SplitToken (text);
				PreCompiler.IdentifyClass("", tokens);
				return CQ_Expression_Compiler.Compile (tokens);
			} else {
				List<Token> tokens = CQ_TokenParser.Parse (text);
				//改成:
				//Split
				//Compile到Type
				return CQ_Expression_Compiler.Compile (tokens);
			}
		}
	}
}
