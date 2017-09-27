using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using CQuark;
using System.Collections;

class ScriptLogger : ICQ_Logger
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
	public CQ_Environment env {get; private set; }
	public CQuark.CQ_Content content = null;


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
		env = new CQ_Environment (new ScriptLogger ());
//		env.logger.Log("CQuark Inited.Ver = "+ env.version);
	}
//	/// <summary>
//	/// 这里注册脚本有权访问的类型，大部分类型用RegHelper_Type提供即可
//	/// </summary>
	public void RegTypes(){
		env.RegType(typeof(UnityEngine.Object), "Object");
		env.RegType(typeof(object), "object");

		//大部分类型用RegHelper_Type提供即可
		env.RegType (typeof(AssetBundle), "AssetBundle");
		env.RegType (typeof(Animation), "Animation");
		env.RegType (typeof(AnimationCurve), "AnimationCurve");
		env.RegType (typeof(AnimationClip), "AnimationClip");
		env.RegType (typeof(Animator), "Animator");
		env.RegType (typeof(Application), "Application");
		env.RegType (typeof(AudioSource), "AudioSource");
		env.RegType (typeof(AudioClip), "AudioClip");
		env.RegType (typeof(AudioListener), "AudioListener");

		env.RegType (typeof(Camera), "Camera");
		env.RegType (typeof(Component), "Component");
		env.RegType (typeof(Color), "Color");
		env.RegType (typeof(Debug), "Debug");
		env.RegType (typeof(GameObject), "GameObject");
		env.RegType (typeof(Input), "Input");

		env.RegType (typeof(Light), "Light");
		env.RegType (typeof(Mathf), "Mathf");
		env.RegType (typeof(Material), "Material");
		env.RegType (typeof(Mesh), "Mesh");
		env.RegType (typeof(MeshFilter), "MeshFilter");
		env.RegType (typeof(Renderer), "Renderer");
		env.RegType (typeof(UnityEngine.Random), "Random");

		env.RegType (typeof(ParticleSystem), "ParticleSystem");
		env.RegType (typeof(PlayerPrefs), "PlayerPrefs");
		env.RegType (typeof(Ray), "Ray");
		env.RegType (typeof(Resources), "Resources");

		env.RegType (typeof(Screen), "Screen");
		env.RegType (typeof(Shader), "Shader");
		env.RegType (typeof(Texture), "Texture");
		env.RegType (typeof(Transform), "Transform");
		env.RegType (typeof(UnityEngine.Time), "Time");

		env.RegType (typeof(Vector2), "Vector2");
		env.RegType (typeof(Vector3), "Vector3");
		env.RegType (typeof(Vector4), "Vector4");
		env.RegType (typeof(Quaternion), "Quaternion");
		env.RegType (typeof(WWW), "WWW");
		env.RegType (typeof(WWWForm), "WWWForm");

		//对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
		env.RegType(typeof(int[]), "int[]");	//数组要独立注册
		env.RegType(typeof(string[]), "string[]");	
		env.RegType(typeof(bool[]), "bool[]");	
		env.RegType(typeof(List<>), "List");	//模板类要独立注册
		env.RegType(typeof(Dictionary<,>), "Dictionary");

//		env.RegType(new CQuark.RegHelper_Type(typeof(Vector2)));
//		env.RegType(new CQuark.RegHelper_Type(typeof(Vector3)));
//		env.RegType(new CQuark.RegHelper_Type(typeof(Vector4)));
//
//
//		env.RegType(new CQuark.RegHelper_Type(typeof(GameObject)));
//		env.RegType(new CQuark.RegHelper_Type(typeof(Transform)));
//
//		env.RegType(new CQuark.RegHelper_Type(typeof(Component)));
//
//		env.RegType(new CQuark.RegHelper_Type(typeof(Debug)));
//		env.RegType(new CQuark.RegHelper_Type(typeof(UnityEngine.Object)));
//		env.RegType(new CQuark.RegHelper_Type(typeof(UnityEngine.Time)));
//
//
//		env.RegType(new CQuark.RegHelper_Type(typeof(int[]),"int[]"));
//		env.RegType(new CQuark.RegHelper_Type(typeof(List<int>), "List<int>"));
	}

	
    public object Eval(string script){
		var token = env.ParserToken(script);//词法分析
		var expr = env.Expr_CompileToken(token, true);//语法分析,简单表达式，一句话
		var value = env.Expr_Execute(expr, content);//执行表达式
        if (value == null)
			return null;
        return value.value;
    }
	
    public object Execute(string script){
		var token = env.ParserToken(script);//词法分析
		var expr = env.Expr_CompileToken(token, false);//语法分析，语法块
		var value = env.Expr_Execute(expr, content);//执行表达式
        if (value == null) return null;
        return value.value;
    }

	public IEnumerator StartCoroutine(string script, ICoroutine coroutine){
		var token = env.ParserToken(script);//词法分析
		var expr = env.Expr_CompileToken(token, false);//语法分析，语法块
		yield return coroutine.StartNewCoroutine(env.Expr_Coroutine (expr, content, coroutine));
	}

	public CQ_Content.Value GetValue(string name){
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


	public void BuildProject(string path, string pattern)
	{
		#if UNITY_STANDALONE
		string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
		Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
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