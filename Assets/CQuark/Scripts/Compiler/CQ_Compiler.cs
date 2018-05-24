using System.Collections.Generic;
using CQuark;

namespace CQuark{
	//外部调用编译功能只调用这一个类
	//如果是编译项目的话，那么编译出的结果是很多IType，他们会被直接写到AppDomain中

	//完整运行一个西瓜项目分以下几步
	//注册所有原生类型（通过工具找）到AppDomain
	//把整个项目里所有CQuark文件转成Token(此时还不去找类，不合并标识符)
	//把所有Token里的class注册到AppDomain里					//如果是编译单个文件的话，直接这一步开始
	//分别编译所有的Token(合并命名空间，把Type替换成全名)变成新的Tokens
	//把所有Tokens再编成Expression，加到对应的IType里			//如果是编译Block的话，直接这一步开始
	//执行需要的IType
	public class CQ_Compiler {
		//编译整个项目
		//如果不是特殊情况，这个函数一般只需要调用一次
		//因为你编译的CQuark引用到了别的类，而别的类没有编译过的话会报错
		//所以最好一次编译所有CQuark文件（整个项目）
		public static void CompileProject(string path, string pattern = "*.cs") {
			string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
			CompileProject(files);
		}
		public static void CompileProject (string[] filePaths) {
			Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
			foreach(string filePath in filePaths) {
				if(project.ContainsKey(filePath))
					continue;
				string text = System.IO.File.ReadAllText(filePath);
				List<Token> tokens = CQ_TokenParser.Parse(text);
				project.Add(filePath, tokens);
			}
			
			foreach(KeyValuePair<string, IList<Token>> f in project) {
				//先把所有代码里的类注册一遍
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
		//编译单个文件
		//如果调用这个函数，你必须确保在此之前已经注册了这个CQ需要引用的所有类
		//除非你完全理解了这个函数的意思，否则请调用CompileProject
		// 这里的filename只是为了编译时报错可以看到出错文件
		public static void CompileOneFile(string fileName, string text){
			List<Token> tokens = CQ_TokenParser.Parse(text);
			CompileOneFile(fileName, tokens);
		}
		public static void CompileOneFile(string fileName, IList<Token> tokens){
			DebugUtil.Log("File_CompilerToken:" + fileName);
			IList<IType> types = Precompile.FileCompile(fileName, tokens);
			foreach(var type in types) {
				if(AppDomain.GetTypeByKeywordQuiet(type.keyword) == null)
					AppDomain.RegisterCQType(type);
			}
		}

		//编译一个函数块
		public static ICQ_Expression CompileBlock(string text){
			List<Token> tokens = CQ_TokenParser.Parse(text);
			return CompileBlock(tokens);
		}
		//编译一个函数块
		//调用这个方法的时候，你的tokens里的类名应该是全名
		public static ICQ_Expression CompileBlock(List<Token> tokens){
			return CQ_Expression_Compiler.Compile(tokens);
		}
	}
}
