using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CQuark;

namespace CQuark{
	//参考了ILRuntime，把以前Environment和Content整合到了一起。
	//整个项目只需要一个AppDomain,所以改成了全部静态
	public class AppDomain {

		static Dictionary<TypeBridge, IType> types = new Dictionary<TypeBridge, IType>();
		static Dictionary<string, IType> typess = new Dictionary<string, IType>();
		static Dictionary<string, IMethod> calls = new Dictionary<string, IMethod>();
		static Dictionary<string, IMethod> corouts = new Dictionary<string, IMethod>();

		public static void Reset(){
			DebugUtil.Log("Reset Domain");
			types.Clear();
			typess.Clear();
			calls.Clear();
			corouts.Clear();
			RegDefaultType();
		}

		public static void RegDefaultType(){
			//最好能默认
			RegType(new Type_Int());
			RegType(new Type_UInt());
			RegType(new Type_Float());
			RegType(new Type_Double());
			RegType(new Type_Byte());
			RegType(new Type_Char());
			RegType(new Type_UShort());
			RegType(new Type_Sbyte());
			RegType(new Type_Short());
			RegType(new Type_Long());
			RegType(new Type_ULong());

            RegType(new Type_String());
            RegType(new Type_Var());
            RegType(new Type_Bool());
            RegType(new Type_Lambda());
            RegType(new Type_Delegate());
			RegType (typeof(IEnumerator), "IEnumerator");

			RegType(typeof(object), "object");

			RegType (typeof(List<>), "List");	//模板类要独立注册
			RegType (typeof(Dictionary<,>), "Dictionary");
			RegType (typeof(Stack<>), "Stack");
			RegType (typeof(Queue<>), "Queue");

			typess["null"] = new Type_NULL();
			//contentGloabl = CreateContent();
			//if (!useNamespace)//命名空间模式不能直接用函数
			{
				RegisterFunction(new MethodTrace());
			}

			RegType(typeof(object), "object");
			//以下内容是Unity专用，如果非Unity平台可以直接主食掉
			RegType(typeof(UnityEngine.Object), "Object");

			//大部分类型用RegHelper_Type提供即可
			RegType (typeof(System.DateTime), "DateTime");
			RegType (typeof(AssetBundle), "AssetBundle");
			RegType (typeof(Animation), "Animation");
			RegType (typeof(AnimationCurve), "AnimationCurve");
			RegType (typeof(AnimationClip), "AnimationClip");
			RegType (typeof(Animator), "Animator");
			RegType (typeof(Application), "Application");
			RegType (typeof(AudioSource), "AudioSource");
			RegType (typeof(AudioClip), "AudioClip");
			RegType (typeof(AudioListener), "AudioListener");

			RegType (typeof(Camera), "Camera");
			RegType (typeof(Component), "Component");
			RegType (typeof(Color), "Color");
			RegType (typeof(Debug), "Debug");
			RegType (typeof(GameObject), "GameObject");
			RegType (typeof(Input), "Input");

			RegType (typeof(Light), "Light");
			RegType (typeof(Mathf), "Mathf");
			RegType (typeof(Material), "Material");
			RegType (typeof(Mesh), "Mesh");
			RegType (typeof(MeshFilter), "MeshFilter");
			RegType (typeof(Renderer), "Renderer");
			RegType (typeof(UnityEngine.Random), "Random");
			RegType(typeof(KeyCode),"KeyCode");

			RegType (typeof(ParticleSystem), "ParticleSystem");
			RegType (typeof(PlayerPrefs), "PlayerPrefs");
			RegType (typeof(Ray), "Ray");
			RegType (typeof(Resources), "Resources");

			RegType (typeof(Screen), "Screen");
			RegType (typeof(Shader), "Shader");
			RegType (typeof(Texture), "Texture");
			RegType (typeof(Transform), "Transform");
			RegType (typeof(UnityEngine.Time), "Time");

			RegType (typeof(Vector2), "Vector2");
			RegType (typeof(Vector3), "Vector3");
			RegType (typeof(Vector4), "Vector4");
			RegType (typeof(Quaternion), "Quaternion");
			RegType (typeof(WWW), "WWW");
			RegType (typeof(WWWForm), "WWWForm");

			//对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
			RegType(typeof(int[]), "int[]");	//数组要独立注册
			RegType(typeof(string[]), "string[]");	
			RegType(typeof(float[]), "float[]");	
			RegType(typeof(bool[]), "bool[]");	
			RegType (typeof(byte[]), "byte[]");
		}

        public static Type_Operatorable MakeType(Type type, string keyword)
        {
            if (!type.IsSubclassOf(typeof(Delegate)))
            {
                return new Type_Operatorable(type, keyword, false);
            }
            var method = type.GetMethod("Invoke");
            var pp = method.GetParameters();
            if (method.ReturnType == typeof(void))
            {
                if (pp.Length == 0)
                {
                    return new Type_DeleAction(type, keyword);
                }
                else if (pp.Length == 1)
                {
                    var gtype = typeof(Type_DeleAction<>).MakeGenericType(new Type[] { pp[0].ParameterType });
                    return gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Operatorable;
                }
                else if (pp.Length == 2)
                {
                    var gtype = typeof(Type_DeleAction<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Operatorable);
                }
                else if (pp.Length == 3)
                {
                    var gtype = typeof(Type_DeleAction<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Operatorable);
                }
                else
                {
                    throw new Exception("还没有支持这么多参数的委托");
                }
            }
            else
            {
                Type gtype = null;
                if (pp.Length == 0)
                {
                    gtype = typeof(Type_DeleNonVoidAction<>).MakeGenericType(new Type[] { method.ReturnType });
                }
                else if (pp.Length == 1)
                {
                    gtype = typeof(Type_DeleNonVoidAction<,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType });
                }
                else if (pp.Length == 2)
                {
                    gtype = typeof(Type_DeleNonVoidAction<,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType });
                }
                else if (pp.Length == 3)
                {
                    gtype = typeof(Type_DeleNonVoidAction<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                }
                else
                {
                    throw new Exception("还没有支持这么多参数的委托");
                }
                return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Operatorable);
            }
        }

		public static void RegType(Type type, string keyword)
		{
			RegType(MakeType(type, keyword));
		}
		public static void RegType(IType type)
		{
			types[type.typeBridge] = type;

			string typename = type.keyword;
			//if (useNamespace)
			//{

			//    if (string.IsNullOrEmpty(type._namespace) == false)
			//    {
			//        typename = type._namespace + "." + type.keyword;
			//    }
			//}
			if (string.IsNullOrEmpty(typename))
			{//匿名自动注册
			}
			else
			{
				typess[typename] = type;
				CQ_TokenParser.AddType(typename);
			}
		}

		public static IType GetType(TypeBridge type)
		{
			if (type == null)
				return typess["null"];

			IType ret = null;
			if (types.TryGetValue(type, out ret) == false)
			{
				DebugUtil.LogWarning("(CQcript)类型未注册,将自动注册一份匿名:" + type.ToString());
				ret = MakeType(type, "");
				RegType(ret);
			}
			return ret;
		}
		public static IType GetTypeByKeyword(string keyword)
		{
			IType ret = null;
			if (string.IsNullOrEmpty(keyword))
			{
				return null;
			}
			if (typess.TryGetValue(keyword, out ret) == false)
			{
				if (keyword[keyword.Length - 1] == '>')
				{
					int iis = keyword.IndexOf('<');
					string func = keyword.Substring(0, iis);
					List<string> _types = new List<string>();
					int istart = iis + 1;
					int inow = istart;
					int dep = 0;
					while (inow < keyword.Length)
					{
						if (keyword[inow] == '<')
						{
							dep++;
						}
						if (keyword[inow] == '>')
						{
							dep--;
							if (dep < 0)
							{
								_types.Add(keyword.Substring(istart, inow - istart));
								break;
							}
						}

						if (keyword[inow] == ',' && dep == 0)
						{
							_types.Add(keyword.Substring(istart, inow - istart));
							istart = inow + 1;
							inow = istart;
							continue; ;
						}

						inow++;
					}

					//var funk = keyword.Split(new char[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
					if (typess.ContainsKey(func))
					{
						Type gentype = GetTypeByKeyword(func).typeBridge;
						if (gentype.IsGenericTypeDefinition)
						{
							Type[] types = new Type[_types.Count];
							for (int i = 0; i < types.Length; i++)
							{
								TypeBridge t = GetTypeByKeyword(_types[i]).typeBridge;
								Type rt = t;
								if (rt == null && t != null)
								{
									rt = typeof(object);
								}
								types[i] = rt;
							}
							Type IType = gentype.MakeGenericType(types);
							RegType(MakeType(IType, keyword));
							return GetTypeByKeyword(keyword);
						}
					}
				}
				DebugUtil.LogError("(CQcript)类型未注册:" + keyword);
			}

			return ret;
		}
		public static IType GetTypeByKeywordQuiet(string keyword)
		{
			IType ret = null;
			if (typess.TryGetValue(keyword, out ret) == false)
			{
				return null;
			}
			return ret;
		}
		public static void RegisterFunction(IMethod func)
		{
			//if (useNamespace)
			//{
			//    throw new Exception("用命名空间时不能直接使用函数，必须直接定义在类里");
			//}
			if (func.returntype == typeof(IEnumerator))
				corouts [func.keyword] = func;
			else
				calls[func.keyword] = func;
		}
		public static void RegisterFunction(Delegate dele)
		{
			RegisterFunction(new Method (dele));
		}

		public static IMethod GetFunction(string name)
		{
			IMethod func = null;
			calls.TryGetValue(name, out func);
			if (func == null)
			{
				corouts.TryGetValue (name, out func);
				if(func == null)
					throw new Exception("找不到函数:" + name);
			}
			return func;
		}

		//是否是一个协程方法
		//TODO 最好不要这么判断
		public static bool IsCoroutine(string name){
			return corouts.ContainsKey (name);
		}

		public static IList<Token> ParserToken(string code)
		{
			if (code [0] == 0xFEFF) {
				//windows下用记事本写，会在文本第一个字符出现BOM（65279）
				code = code.Substring(1);
			}

			IList<Token> tokens = CQ_TokenParser.Parse(code);
			if (tokens == null)
				DebugUtil.LogWarning ("没有解析到代码");

			return tokens;
		}
		public static ICQ_Expression Expr_CompileToken(IList<Token> listToken)
		{
			return CQ_Expression_Compiler.Compile(listToken);
		}

		public static ICQ_Expression Expr_CompileToken(IList<Token> listToken, bool SimpleExpression)
		{
			return SimpleExpression ? CQ_Expression_Compiler.Compile_NoBlock(listToken) : CQ_Expression_Compiler.Compile(listToken);
		}

		//CQ_Content contentGloabl = null;

		public static CQ_Value Expr_Execute(ICQ_Expression expr)
		{
			CQ_Content content = new CQ_Content (true);
			return expr.ComputeValue(content);
		}
		public static CQ_Value Expr_Execute(ICQ_Expression expr, CQ_Content content)
		{
			if (content == null)
                content = new CQ_Content(true);
			return expr.ComputeValue(content);
		}
		public static IEnumerator Expr_Coroutine(ICQ_Expression expr, CQ_Content content, ICoroutine coroutine)
		{
			if (content == null)
                content = new CQ_Content(true);
			yield return coroutine.StartNewCoroutine(expr.CoroutineCompute(content, coroutine));
		}

		public static void Project_Compile(Dictionary<string, IList<Token>> project, bool embDebugToken)
		{
			foreach (KeyValuePair<string, IList<Token>> f in project)
			{
				File_PreCompileToken(f.Key, f.Value);
			}
			foreach (KeyValuePair<string, IList<Token>> f in project)
			{
				//预处理符号
				for (int i = 0; i < f.Value.Count; i++)
				{
					if (f.Value[i].type == TokenType.IDENTIFIER && CQ_TokenParser.ContainsType(f.Value[i].text))
					{//有可能预处理导致新的类型
						if (i > 0
							&&
							(f.Value[i - 1].type == TokenType.TYPE || f.Value[i - 1].text == "."))
						{
							continue;
						}
						Token rp = f.Value[i];
						rp.type = TokenType.TYPE;
						f.Value[i] = rp;
					}
				}
				File_CompileToken(f.Key, f.Value, embDebugToken);
			}
		}
		public static void File_PreCompileToken(string filename, IList<Token> listToken)
		{
			IList<IType> types = CQ_Expression_Compiler.FilePreCompile(filename, listToken);
			foreach (var type in types)
			{
				RegType(type);
			}
		}
		public static void File_CompileToken(string filename, IList<Token> listToken, bool embDebugToken)
		{
			DebugUtil.Log("File_CompilerToken:" + filename);
			IList<IType> types = CQ_Expression_Compiler.FileCompile(filename, listToken, embDebugToken);
			foreach (var type in types)
			{
				if (GetTypeByKeywordQuiet(type.keyword) == null)
					RegType(type);
			}
		}

		/// <summary>
		/// 这里的filename只是为了编译时报错可以看到出错文件
		/// </summary>
		public static void BuildFile(string filename, string code){
			var token = ParserToken(code);//词法分析
			File_CompileToken(filename, token, false);
		}
			
		public static void BuildProject(string path, string pattern)
		{
			string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
			Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
			foreach (var file in files)
			{
				if (project.ContainsKey(file))
					continue;
				string text = System.IO.File.ReadAllText(file);
				var tokens = CQ_TokenParser.Parse(text);
				project.Add(file, tokens);
			}
			Project_Compile(project, true);
		}
	}
}