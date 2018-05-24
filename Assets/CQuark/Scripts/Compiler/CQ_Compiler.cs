using System.Collections.Generic;
using System.Collections;
using CQuark;

namespace CQuark{
	//外部调用编译功能只调用这一个类
	//如果是编译项目的话，那么编译出的结果是很多IType，他们会被直接写到AppDomain中
	public class CQ_Compiler {
		//编译整个项目
		//如果不是特殊情况，这个函数一般只需要调用一次
		//即使你只是编译几个脚本（甚至只有一个），也请调用这个函数
		//因为如果你编译的CQuark引用到了别的类，而别的类没有编译过的话会报错
		public static void CompileProject(string[] filePaths){

		}

		//编译单个文件
		//如果调用这个函数，你必须确保在此之前已经注册了这个CQ需要引用的所有类
		//除非你完全理解了这个函数的意思，否则请调用CompileProject
		public static void CompileOneFile(string fileName, string text){

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
