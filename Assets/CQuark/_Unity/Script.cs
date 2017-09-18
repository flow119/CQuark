using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using CSLE;
using System.Collections;

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
		env.RegType(RegHelper_Type.MakeType (typeof(UnityEngine.Object), "Object"));
		env.RegType(RegHelper_Type.MakeType (typeof(object), "object"));

		//大部分类型用RegHelper_Type提供即可
		env.RegType (RegHelper_Type.MakeType (typeof(AssetBundle), "AssetBundle"));
		env.RegType (RegHelper_Type.MakeType (typeof(Animation), "Animation"));
		env.RegType (RegHelper_Type.MakeType (typeof(AnimationCurve), "AnimationCurve"));
		env.RegType (RegHelper_Type.MakeType (typeof(AnimationClip), "AnimationClip"));
		env.RegType (RegHelper_Type.MakeType (typeof(Animator), "Animator"));
		env.RegType (RegHelper_Type.MakeType (typeof(Application), "Application"));
		env.RegType (RegHelper_Type.MakeType (typeof(AudioSource), "AudioSource"));
		env.RegType (RegHelper_Type.MakeType (typeof(AudioClip), "AudioClip"));
		env.RegType (RegHelper_Type.MakeType (typeof(AudioListener), "AudioListener"));

		env.RegType (RegHelper_Type.MakeType (typeof(Camera), "Camera"));
		env.RegType (RegHelper_Type.MakeType (typeof(Component), "Component"));
		env.RegType (RegHelper_Type.MakeType (typeof(Color), "Color"));
		env.RegType (RegHelper_Type.MakeType (typeof(Debug), "Debug"));
		env.RegType (RegHelper_Type.MakeType (typeof(GameObject), "GameObject"));
		env.RegType (RegHelper_Type.MakeType (typeof(Input), "Input"));

		env.RegType (RegHelper_Type.MakeType (typeof(Light), "Light"));
		env.RegType (RegHelper_Type.MakeType (typeof(Mathf), "Mathf"));
		env.RegType (RegHelper_Type.MakeType (typeof(Material), "Material"));
		env.RegType (RegHelper_Type.MakeType (typeof(Mesh), "Mesh"));
		env.RegType (RegHelper_Type.MakeType (typeof(MeshFilter), "MeshFilter"));
		env.RegType (RegHelper_Type.MakeType (typeof(Renderer), "Renderer"));

		env.RegType (RegHelper_Type.MakeType (typeof(ParticleSystem), "ParticleSystem"));
		env.RegType (RegHelper_Type.MakeType (typeof(PlayerPrefs), "PlayerPrefs"));
		env.RegType (RegHelper_Type.MakeType (typeof(Ray), "Ray"));
		env.RegType (RegHelper_Type.MakeType (typeof(Resources), "Resources"));

		env.RegType (RegHelper_Type.MakeType (typeof(Screen), "Screen"));
		env.RegType (RegHelper_Type.MakeType (typeof(Shader), "Shader"));
		env.RegType (RegHelper_Type.MakeType (typeof(Texture), "Texture"));
		env.RegType (RegHelper_Type.MakeType (typeof(Transform), "Transform"));
		env.RegType (RegHelper_Type.MakeType (typeof(UnityEngine.Time), "Time"));

		env.RegType (RegHelper_Type.MakeType (typeof(Vector2), "Vector2"));
		env.RegType (RegHelper_Type.MakeType (typeof(Vector3), "Vector3"));
		env.RegType (RegHelper_Type.MakeType (typeof(Vector4), "Vector4"));
		env.RegType (RegHelper_Type.MakeType (typeof(Quaternion), "Quaternion"));
		env.RegType (RegHelper_Type.MakeType (typeof(WWW), "WWW"));
		env.RegType (RegHelper_Type.MakeType (typeof(WWWForm), "WWWForm"));

		//对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
		env.RegType(RegHelper_Type.MakeType(typeof(int[]), "int[]"));	//数组要独立注册
		env.RegType(RegHelper_Type.MakeType(typeof(string[]), "string[]"));	
		env.RegType(RegHelper_Type.MakeType(typeof(bool[]), "bool[]"));	
		env.RegType(RegHelper_Type.MakeType(typeof(List<>), "List"));	//模板类要独立注册
		env.RegType(RegHelper_Type.MakeType(typeof(Dictionary<,>), "Dictionary"));

//		env.RegType(new CSLE.RegHelper_Type(typeof(Vector2)));
//		env.RegType(new CSLE.RegHelper_Type(typeof(Vector3)));
//		env.RegType(new CSLE.RegHelper_Type(typeof(Vector4)));
//
//
//		env.RegType(new CSLE.RegHelper_Type(typeof(GameObject)));
//		env.RegType(new CSLE.RegHelper_Type(typeof(Transform)));
//
//		env.RegType(new CSLE.RegHelper_Type(typeof(Component)));
//
//		env.RegType(new CSLE.RegHelper_Type(typeof(Debug)));
//		env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Object)));
//		env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Time)));
//
//
//		env.RegType(new CSLE.RegHelper_Type(typeof(int[]),"int[]"));
//		env.RegType(new CSLE.RegHelper_Type(typeof(List<int>), "List<int>"));
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