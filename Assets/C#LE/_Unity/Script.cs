using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using CSLE;

class ScriptLogger : ICLS_Logger
{
    public void Log(string str)
    {
        UnityEngine.Debug.Log(str);
    }

    public void Log_Error(string str)
    {
        Debug.LogError(str);
    }

    public void Log_Warn(string str)
    {
        Debug.LogWarning(str);
    }
}


public class Script
{
	public Script(){
		Reset ();
	}
	public CLS_Environment env {get; private set; }
	public CSLE.CLS_Content content = null;


	public static Script Instance{
		get{
			if(_instance == null)
				_instance = new Script();
			_instance.RegTypes();
			return _instance;
		}
	}
	static Script _instance;

	
	public void Reset(){
		env = new CLS_Environment (new ScriptLogger ());
//		env.logger.Log("CQuark Inited.Ver = "+ env.version);
	}
//	/// <summary>
//	/// 这里注册脚本有权访问的类型，大部分类型用RegHelper_Type提供即可
//	/// </summary>
	public void RegTypes(){
		//大部分类型用RegHelper_Type提供即可
		env.RegType(new CSLE.RegHelper_Type(typeof(Vector2)));
		env.RegType(new CSLE.RegHelper_Type(typeof(Vector3)));
		env.RegType(new CSLE.RegHelper_Type(typeof(Vector4)));
		
		env.RegType(new CSLE.RegHelper_Type(typeof(Debug)));
		env.RegType(new CSLE.RegHelper_Type(typeof(GameObject)));
		env.RegType(new CSLE.RegHelper_Type(typeof(Component)));
		env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Object)));
		env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Time)));
		env.RegType(new CSLE.RegHelper_Type(typeof(Transform)));
		//对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
		env.RegType(new CSLE.RegHelper_Type(typeof(int[]),"int[]"));//数组要独立注册
		env.RegType(new CSLE.RegHelper_Type(typeof(List<int>), "List<int>"));//模板类要独立注册
	}

	
    public object Eval(string script){
		var token = env.ParserToken(script);//词法分析
		var expr = env.Expr_CompilerToken(token, true);//语法分析,简单表达式，一句话
		var value = env.Expr_Execute(expr, content);//执行表达式
        if (value == null)
			return null;
        return value.value;
    }
	
    public object Execute(string script){
		var token = env.ParserToken(script);//词法分析
		var expr = env.Expr_CompilerToken(token, false);//语法分析，语法块
		var value = env.Expr_Execute(expr, content);//执行表达式
        if (value == null) return null;
        return value.value;
    }


//	public static void StartCoroutine(string script){
//		var token = Instance.env.ParserToken(script);//词法分析
//		var expr = Instance.env.Expr_CompilerToken(token, false);//语法分析，语法块
//		Instance.env.Expr_Coroutine(expr, content);//执行表达式
////		if (value == null) return null;
////		return value.value;
//	}
	

	public CLS_Content.Value GetValue(string name){
		if (content == null)
			content = env.CreateContent();
		return content.Get(name);
	}

	public void SetValue(string name, object v)
    {
        if (content == null)
			content = env.CreateContent();
        content.DefineAndSet(name, v.GetType(), v);
    }

	public void ClearValue(){
        content = null;
    }


	public void BuildFile(string filename, string code){
		var token = env.ParserToken(code);//词法分析
		env.File_CompileToken(filename, token, false);
	}


	static bool projectLoaded;
	public static void LoadProject(){
		if (projectLoaded)
			return;
		Instance.BuildProject (Application.streamingAssetsPath, "*.cs");
		projectLoaded = true;
	}
	public void BuildProject(string path, string pattern)
	{
		#if UNITY_STANDALONE
		string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
		Dictionary<string, IList<CSLE.Token>> project = new Dictionary<string, IList<CSLE.Token>>();
		foreach (var v in files)
		{
			var tokens = env.tokenParser.Parse(System.IO.File.ReadAllText(v));
			project.Add(v, tokens);
		}
		env.Project_Compile(project, true);
		#endif
	}
	//TODO 编译单个文件
//	public IList<Token> BuildTextAsset(TextAsset ta)
//	{
//		return BuildText (ta.text);
//	}
//	public IList<Token> BuildText(string text){
//		var tokens = env.tokenParser.Parse (text);
//		env.com
//	}
}