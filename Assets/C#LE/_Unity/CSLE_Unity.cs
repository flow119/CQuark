using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using CSLE;

class ScriptLogger : CSLE.ICLS_Logger
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

class Script
{
    public static CLS_Environment env{
		get{
			if(_env == null)
				Init ();
			return _env;
		}set{
			_env = value;
		}
	}
	private static CLS_Environment _env = null;

    public static void Init()
    {
		if (_env == null)
        {
			_env = new CSLE.CLS_Environment(new ScriptLogger());
			_env.logger.Log("CQuark Inited.Ver="+ _env.version);
        }
    }
    public static void Reset()
    {
		_env = null;
        Init();
    }


    public static object Eval(string script)
    {
		if (_env == null)
            Init();

		var token = _env.ParserToken(script);//词法分析
		var expr = _env.Expr_CompilerToken(token, true);//语法分析,简单表达式，一句话
		var value = _env.Expr_Execute(expr, content);//执行表达式
        if (value == null) return null;
        return value.value;
    }
    public static object Execute(string script)
    {
		var token = _env.ParserToken(script);//词法分析
		var expr = _env.Expr_CompilerToken(token, false);//语法分析，语法块
		var value = _env.Expr_Execute(expr, content);//执行表达式
        if (value == null) return null;
        return value.value;
    }
    


    public static CSLE.CLS_Content content = null;
	public static CLS_Content.Value GetValue(string name)
	{
		if (_env == null)
			Init();
		if (content == null)
			content = _env.CreateContent();
		return content.Get(name);
	}
    public static void SetValue(string name, object v)
    {
		if (_env == null)
            Init();
        if (content == null)
			content = _env.CreateContent();
        content.DefineAndSet(name, v.GetType(), v);
    }
    public static void ClearValue()
    {
        content = null;
    }

	
	public static void BuildProject(string path)
	{
		#if UNITY_STANDALONE
		if (_env == null)
			Init();
		string[] files = System.IO.Directory.GetFiles(path, "*.cs", System.IO.SearchOption.AllDirectories);
		Dictionary<string, IList<CSLE.Token>> project = new Dictionary<string, IList<CSLE.Token>>();
		foreach (var v in files)
		{
			var tokens = _env.tokenParser.Parse(System.IO.File.ReadAllText(v));
			project.Add(v, tokens);
		}
		_env.Project_Compile(project, true);
		#endif
	}
	public static void BuildFile(string filename, string code)
	{
		var token = _env.ParserToken(code);//词法分析
		_env.File_CompileToken(filename, token, false);
		
	}
}

