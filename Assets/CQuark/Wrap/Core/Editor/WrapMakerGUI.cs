using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMakerGUI : WrapMaker {

    List<string> _folderNamespace = new List<string>();    //被折叠的wrapclass
//    List<string> _deleteClasses = new List<string>();       //准备删除的类
    
    const string WRAP_CORE_NAME = "WrapCore";
	const string WRAP_TYPE_NAME = "WrapType";

	[MenuItem("CQuark/Wrap Maker", false, 9)]
	[MenuItem("Assets/CQuark/Wrap Maker", false, 0)]
	static public void OpenWrapMaker ()
	{
		EditorWindow.GetWindow<WrapMakerGUI>(false, "Wrap Maker", true).Show();
	}

	Vector2 mScroll = Vector2.zero;

	string _classInput = "";

	string WrapGenFolder{
		get{
			return Application.dataPath + "/CQuark/Wrap/Generate";
		}
	}

	string WrapCoreFolder{
		get{
			return Application.dataPath + "/CQuark/Wrap/Core";
		}
	}

	static void WriteAllText(string folder, string name, string content){
		if(!Directory.Exists(folder))
			Directory.CreateDirectory(folder);
		File.WriteAllText(folder + "/" + name, content, System.Text.Encoding.UTF8);
	}

	void Reload(){
		//TODO这里做一次MD5比较，记录原始的cs脚本的md5，然后刷新一下当前的md5,m5可以写到wrap的cs里
		//md5
		//sset,sget,mset,mget,new,scall,mcall,op

        m_wrapClasses.Clear();
		DirectoryInfo di = new DirectoryInfo(WrapGenFolder);
		FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
		for(int i = 0; i < files.Length; i++){
			string classname = files[i].Name.Split('.')[0];
			if(files[i].Directory.ToString().Length == WrapGenFolder.Length){
                WrapClass wc = GetWrapClass("");
                if(wc == null) {
                    wc = new WrapClass("");
                    m_wrapClasses.Add(wc);
                }
                wc.AddClass(classname);
			}else{
				string nameSpace = files[i].Directory.ToString().Substring(WrapGenFolder.Length + 1);
				
                WrapClass wc = GetWrapClass(nameSpace);
                if(wc == null) {
                    wc = new WrapClass(nameSpace);
                    m_wrapClasses.Add(wc);
                }
                wc.AddClass(classname);
			}
		}

//		_typeDic.Clear ();
		string wrapType = File.ReadAllText(WrapCoreFolder + "/" + WRAP_TYPE_NAME + ".cs", System.Text.Encoding.UTF8);
		int startIndex = wrapType.IndexOf ("#region Types") + "#region Types".Length;
		int endIndex = wrapType.IndexOf ("#endregion");
		if(startIndex < endIndex){
			string types = wrapType.Substring(startIndex, endIndex - startIndex);
			string[] typeLines = types.Replace("\t","").Replace("\r","").Split ('\n');
			for (int i = 0; i < typeLines.Length; i++) {
				if (!_typeDic.Contains (typeLines [i]))
					_typeDic.Add (typeLines [i]);
			}
		}
	}

	void OnlyAddClass(Type type){
		OnlyAddClass(type.Namespace, type.Name);
	}
	//这里可以改为使用类
	void OnlyAddClass(string assemblyName, string classname){
		if(classname.Contains("`"))
			return;
		if(string.IsNullOrEmpty(assemblyName))
			assemblyName = "";
		
		Type type = GetType(classname, ref assemblyName);

		if(type == null){
			if(string.IsNullOrEmpty(assemblyName))
				Debug.LogWarning("No Such Type : " + classname);
			else
				Debug.LogWarning("No Such Type : " + assemblyName + "." + classname);
			return;
		}

		string log = "";//一个记事本，用来保存哪些内容做了Wrap
		string classFullName = string.IsNullOrEmpty(assemblyName) ? classname : assemblyName + "." + classname;

		//变量或属性
		List<Property> savePropertys = GetPropertys(type, ref log);
		string[] propertyPartStr = Propertys2PartStr(classFullName, savePropertys);

		//构造函数，
		List<Method> constructMethods = GetConstructor(type, ref log);
		string wrapNew = Constructor2PartStr(classFullName, constructMethods);
		CallTypes2TypeStr (constructMethods, _typeDic);

		//静态方法（最后的参数是忽略属性，因为属性也是一种方法）
		List<Method> staticMethods = GetStaticMethods (type, ref log, savePropertys);
		string wrapSCall = SCall2PartStr(classFullName, staticMethods);
		CallTypes2TypeStr (staticMethods, _typeDic);

		//成员方法
		List<Method> memberMethods = GetInstanceMethods (type, ref log, savePropertys);
		string wrapMCall = MCall2PartStr(classFullName, memberMethods);
		CallTypes2TypeStr (memberMethods, _typeDic);

		//索引
		List<Method> indexMethods = GetIndex (type, ref log);
		string[] wrapIndex = Index2PartStr(classFullName, indexMethods);

		//运算符（数学运算和逻辑运算）
		List<Method> opMethods = GetOp (type, ref log);
        string[] wrapOp = Op2PartStr(opMethods);

		if(m_generateLog){
	        if(string.IsNullOrEmpty(assemblyName)) {
				WriteAllText(WrapGenFolder, classname + ".txt", log);
			}
			else {
				WriteAllText(WrapGenFolder + "/" + assemblyName, classname + ".txt", log);
			}
		}

		UpdateWrapPart(assemblyName, classname, propertyPartStr,
            wrapNew, wrapSCall, wrapMCall, wrapIndex, wrapOp);
	}

	void OnlyRemoveClass(string assemblyName, string classname){
		if(string.IsNullOrEmpty(assemblyName))
			File.Delete(WrapGenFolder + "/" + classname + ".cs");
		else
			File.Delete(WrapGenFolder + "/" + assemblyName + "/" + classname + ".cs");
	}
		
	void UpdateWrapPart(string assemblyName, string classname, string[] propertys, 
		string wrapNew, string wrapSCall, string wrapMCall, string[] wrapIndex, string[] wrapOp){

		string classWrapName = assemblyName.Replace(".","") + classname;                                      //类似UnityEngineVector3，不带点
		string text = (Resources.Load("WrapPartTemplate") as TextAsset).text;


		text = text.Replace("{WrapName}", classWrapName);
		text = text.Replace("{WrapSGet}", propertys[0]);  
		text = text.Replace("{WrapSSet}", propertys[1]);  
		text = text.Replace("{WrapMGet}", propertys[2]);  
		text = text.Replace("{WrapMSet}", propertys[3]);

		text = text.Replace("{WrapNew}", wrapNew);	
		text = text.Replace("{WrapSCall}", wrapSCall);	
		text = text.Replace("{WrapMCall}", wrapMCall);

		text = text.Replace("{WrapIGet}", wrapIndex[0]);
		text = text.Replace("{WrapISet}", wrapIndex[1]);

        text = text.Replace("{WrapAdd}", wrapOp[0]);
        text = text.Replace("{WrapSub}", wrapOp[1]);
        text = text.Replace("{WrapMul}", wrapOp[2]);
        text = text.Replace("{WrapDiv}", wrapOp[3]);
        text = text.Replace("{WrapMod}", wrapOp[4]);

		//TODO OP 逻辑比较
     //  (op_Addition,op_subtraction,op_Multiply,op_Division,op_Modulus,op_GreaterThan,op_LessThan,op_GreaterThanOrEqual,op_LessThanOrEqual,op_Equality,op_Inequality

		if(string.IsNullOrEmpty(assemblyName)) {
			WriteAllText(WrapGenFolder, classname + ".cs", text);
		}
		else {
			WriteAllText(WrapGenFolder + "/" + assemblyName, classname + ".cs", text);
		}
	}
		
	void UpdateWrapCore(){
//		//测试
//		return;

        string text = (Resources.Load("WrapCoreTemplate") as TextAsset).text;

        string wrapNew = "";
        string wrapSVGet = "";
        string wrapSVSet = "";
        string wrapSCall = "";
        string wrapMVGet = "";
        string wrapMVSet = "";
        string wrapMCall = "";
        string wrapIGet = "";
        string wrapISet = "";
		string wrapAdd = "";
		string wrapSub = "";
		string wrapMul = "";
		string wrapDiv = "";
		string wrapMod = "";

        foreach(WrapClass kvp in m_wrapClasses) {
            for(int i = 0; i < kvp.m_classes.Count; i++) {
                string classFullName = kvp.m_nameSpace == "" ? kvp.m_classes[i] : kvp.m_nameSpace + "." + kvp.m_classes[i]; 	//类似UnityEngine.Vector3，用来Wrap
                string classWrapName = kvp.m_nameSpace.Replace(".", "") + kvp.m_classes[i];                                      //类似UnityEngineVector3，不带点
                
                wrapNew += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapNew += "\t\t\t\treturn " + classWrapName + "New(param, out returnValue, true) || " 
                                                + classWrapName + "New(param, out returnValue, false);\r\n";
                wrapNew += "\t\t\t}\n";

                wrapSVGet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSVGet += "\t\t\t\treturn " + classWrapName + "SGet(memberName, out returnValue);\r\n";
                wrapSVGet += "\t\t\t}\n";

                wrapSVSet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSVSet += "\t\t\t\treturn " + classWrapName + "SSet(memberName, param);\r\n";
                wrapSVSet += "\t\t\t}\n";

                wrapSCall += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSCall += "\t\t\t\treturn " + classWrapName + "SCall(functionName, param, out returnValue, true) || " 
                                                + classWrapName + "SCall(functionName, param, out returnValue, false);\r\n";
                wrapSCall += "\t\t\t}\n";

                wrapMVGet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMVGet += "\t\t\t\treturn " + classWrapName + "MGet(objSelf, memberName, out returnValue);\r\n";
                wrapMVGet += "\t\t\t}\n";

                wrapMVSet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMVSet += "\t\t\t\treturn " + classWrapName + "MSet(objSelf, memberName, param);\r\n";
                wrapMVSet += "\t\t\t}\n";

                wrapMCall += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMCall += "\t\t\t\treturn " + classWrapName + "MCall(objSelf, functionName, param, out returnValue, true) || "
                                                + classWrapName + "MCall(objSelf, functionName, param, out returnValue, false);\r\n";
                wrapMCall += "\t\t\t}\n";

                wrapIGet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapIGet += "\t\t\t\treturn " + classWrapName + "IGet(objSelf, key, out returnValue);\r\n";
                wrapIGet += "\t\t\t}\n";

                wrapISet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapISet += "\t\t\t\treturn " + classWrapName + "ISet(objSelf, key, param);\r\n";
                wrapISet += "\t\t\t}\n";

               
                wrapAdd += "\t\t\t\tif(" + classWrapName + "Add(left, right, out returnValue, mustEqual))\n";
                wrapAdd += "\t\t\t\t\treturn true;\n";

                wrapSub += "\t\t\t\tif(" + classWrapName + "Sub(left, right, out returnValue, mustEqual))\n";
                wrapSub += "\t\t\t\t\treturn true;\n";

                wrapMul += "\t\t\t\tif(" + classWrapName + "Mul(left, right, out returnValue, mustEqual))\n";
                wrapMul += "\t\t\t\t\treturn true;\n";

                wrapDiv += "\t\t\t\tif(" + classWrapName + "Div(left, right, out returnValue, mustEqual))\n";
                wrapDiv += "\t\t\t\t\treturn true;\n";

                wrapMod += "\t\t\t\tif(" + classWrapName + "Mod(left, right, out returnValue, mustEqual))\n";
                wrapMod += "\t\t\t\t\treturn true;\n";
               
            }
        }

        wrapAdd = "\t\t\tfor(int t = 2; t > 0; t--){\n"
                            + "\t\t\t\tbool mustEqual = (t == 2);\n"
                            + wrapAdd
                            + "\t\t\t}\n";
        wrapSub = "\t\t\tfor(int t = 2; t > 0; t--){\n"
                           + "\t\t\t\tbool mustEqual = (t == 2);\n"
                           + wrapSub
                           + "\t\t\t}\n";
        wrapMul = "\t\t\tfor(int t = 2; t > 0; t--){\n"
                           + "\t\t\t\tbool mustEqual = (t == 2);\n"
                           + wrapMul
                           + "\t\t\t}\n";
        wrapDiv = "\t\t\tfor(int t = 2; t > 0; t--){\n"
                           + "\t\t\t\tbool mustEqual = (t == 2);\n"
                           + wrapDiv
                           + "\t\t\t}\n";
        wrapMod = "\t\t\tfor(int t = 2; t > 0; t--){\n"
                           + "\t\t\t\tbool mustEqual = (t == 2);\n"
                           + wrapMod
                           + "\t\t\t}\n";

        text = text.Replace("{wrapSVGet}", wrapSVGet);
        text = text.Replace("{wrapSVSet}", wrapSVSet);
        text = text.Replace("{wrapMVGet}", wrapMVGet);
        text = text.Replace("{wrapMVSet}", wrapMVSet);

        text = text.Replace("{wrapNew}", wrapNew);
        text = text.Replace("{wrapSCall}", wrapSCall);
        text = text.Replace("{wrapMCall}", wrapMCall);
        text = text.Replace("{wrapIGet}", wrapIGet);
        text = text.Replace("{wrapISet}", wrapISet);

        text = text.Replace("{wrapAdd}", wrapAdd);
        text = text.Replace("{wrapSub}", wrapSub);
        text = text.Replace("{wrapMul}", wrapMul);
        text = text.Replace("{wrapDiv}", wrapDiv);
        text = text.Replace("{wrapMod}", wrapMod);

        //string text = string.Format(_wrapCoreTemplate, wrapNew, wrapSVGet, wrapSVSet, wrapSCall, wrapMVGet, wrapMVSet, wrapMCall, wrapIGet, wrapISet);
        File.WriteAllText(WrapCoreFolder + "/" + WRAP_CORE_NAME + ".cs", text, System.Text.Encoding.UTF8);
	}

	List<string> _typeDic = new List<string> ();
	void UpdateWrapTypes(){
		//TODO 不要这个类，改为写在各个类文件里面
		string text = (Resources.Load("WrapTypeTemplate") as TextAsset).text;
		string types = "";
		for (int i = 0; i < _typeDic.Count; i++) {
			types += "\t\t\t" + _typeDic [i] + "\n";
		}
		text = text.Replace ("{wrapTypes}", types);
		File.WriteAllText(WrapCoreFolder + "/" + WRAP_TYPE_NAME + ".cs", text, System.Text.Encoding.UTF8);
	}

	void AddClass(string assemblyName, string classname){
		OnlyAddClass(assemblyName, classname);
		Reload();
		UpdateWrapCore();
		UpdateWrapTypes ();
        
		//Add完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
	}

	void RemoveClass(string assemblyName, string classname){
		OnlyRemoveClass(assemblyName, classname);
		Reload();
		UpdateWrapCore();
		//Remove完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
	}

    void RemoveClass (string assemblyName) {
        List<string> classes = GetWrapClass(assemblyName).m_classes;
        for(int i = classes.Count - 1; i >= 0; i--) {
            OnlyRemoveClass(assemblyName, classes[i]);
        }
        Reload();
        UpdateWrapCore();
        //Remove完毕ReloadDataBase，会编译代码
        AssetDatabase.Refresh();
    }

	void UpdateClass(string assemblyName, string classname){
		OnlyRemoveClass(assemblyName, classname);
		OnlyAddClass(assemblyName, classname);
        UpdateWrapCore(); // 有可能WrapCore被改坏了，还是更新一下
		AssetDatabase.Refresh();
	}

    void UpdateClass (string assemblyName) {
        List<string> classes = GetWrapClass(assemblyName).m_classes;
        for(int i = classes.Count - 1; i >= 0; i--) {
            OnlyRemoveClass(assemblyName, classes[i]);
            OnlyAddClass(assemblyName, classes[i]);
        }
        UpdateWrapCore(); // 有可能WrapCore被改坏了，还是更新一下
        AssetDatabase.Refresh();
    }


    void ClearAll () {
//        m_wrapClasses.Clear();
//		Reload();
//        UpdateWrapCore();
//		_typeDic.Clear ();
//		UpdateWrapTypes ();
//        AssetDatabase.Refresh();
    }

	public void WrapCommon(){
        //最后导出的内容是： BaseType + (All-Non-Namespace - BlackList) + WhiteList
        //导出的时候同时注册
        //基本类型（int,bool,string等）
//        OnlyAddClass("", "double");
//        OnlyAddClass("", "float");
//        OnlyAddClass("", "long");
//        OnlyAddClass("", "ulong");
//        OnlyAddClass("", "int");
//        OnlyAddClass("", "uint");
//        OnlyAddClass("", "short");
//        OnlyAddClass("", "ushort");
//        OnlyAddClass("", "byte");
//        OnlyAddClass("", "sbyte");
//        OnlyAddClass("", "char");
//        OnlyAddClass("", "object");
//        OnlyAddClass("", "bool");
//        OnlyAddClass("", "string");

        //项目里没有Namespace的所有类, 减去黑名单
//		Type[] types = GetTypesByNamespace("");
//		if(types != null) {
//			for(int i = 0; i < types.Length; i++) {
//				OnlyAddClass(types[i].Namespace, types[i].Name);
//			}
//		}

        //Config里的所有类，这些可以从外部配置,在Option里设置

        //System.IO.DirectoryInfo
        //System.IO.Directory
        //System.IO.File
        //System.IO.FileInfo
        

//        OnlyAddClass("System", "DateTime");
//        OnlyAddClass("System", "DayOfWeek");
//
//		
//
//        //TODO 这里的内容放到外部配置里，在Option里设置
//		//这里并没有直接获取UnityEngine里所有类，因为大部分类不常用。这里列出的类是我自己使用频率较高的
//		//如果需要，你可以在这里补充，或者在编辑器界面里手动输入单独添加
//
//        OnlyAddClass("UnityEngine", "Object");
//
//        OnlyAddClass("UnityEngine", "AssetBundle");
//        OnlyAddClass("UnityEngine", "Animation");
//        OnlyAddClass("UnityEngine", "AnimationCurve");
//        OnlyAddClass("UnityEngine", "AnimationClip");
//        OnlyAddClass("UnityEngine", "Animator");
//        OnlyAddClass("UnityEngine", "AnimatorStateInfo");
//        OnlyAddClass("UnityEngine", "Application");
//        OnlyAddClass("UnityEngine", "AudioSource");
//        OnlyAddClass("UnityEngine", "AudioClip");
//        OnlyAddClass("UnityEngine", "AudioListener");
//
//        OnlyAddClass("UnityEngine", "Bounds");
//        OnlyAddClass("UnityEngine", "Behaviour");
//
//        OnlyAddClass("UnityEngine", "Camera");
//        OnlyAddClass("UnityEngine", "Component");
//        OnlyAddClass("UnityEngine", "Color");
//        OnlyAddClass("UnityEngine", "Debug");
//        OnlyAddClass("UnityEngine", "GameObject");
//        OnlyAddClass("UnityEngine", "Input");
//
//        OnlyAddClass("UnityEngine", "KeyCode");
//        OnlyAddClass("UnityEngine", "KeyFrame");
//        OnlyAddClass("UnityEngine", "Light");
//        OnlyAddClass("UnityEngine", "Mathf");
//        OnlyAddClass("UnityEngine", "Material");
//        OnlyAddClass("UnityEngine", "Mesh");
//        OnlyAddClass("UnityEngine", "MeshRenderer");
//        OnlyAddClass("UnityEngine", "MeshFilter");
//        OnlyAddClass("UnityEngine", "MonoBehaviour");
//
//        OnlyAddClass("UnityEngine", "Physics");
//        OnlyAddClass("UnityEngine", "Physics2D");
//        OnlyAddClass("UnityEngine", "ParticleSystem");
//        OnlyAddClass("UnityEngine", "PlayerPrefs");
//        OnlyAddClass("UnityEngine", "Quaternion");
//
//        OnlyAddClass("UnityEngine", "Renderer");
//        OnlyAddClass("UnityEngine", "Resolution");
//        OnlyAddClass("UnityEngine", "Random");
//        OnlyAddClass("UnityEngine", "Ray");
//        OnlyAddClass("UnityEngine", "Ray2D");
//        OnlyAddClass("UnityEngine", "Resources");
//
//        OnlyAddClass("UnityEngine", "Screen");
//        OnlyAddClass("UnityEngine", "Shader");
//        OnlyAddClass("UnityEngine", "Texture");
//        OnlyAddClass("UnityEngine", "SkinnedMeshRenderer");
//        OnlyAddClass("UnityEngine", "Transform");
//        OnlyAddClass("UnityEngine", "Time");
//        OnlyAddClass("UnityEngine", "TextAsset");
//
//        OnlyAddClass("UnityEngine", "Vector2");
//        OnlyAddClass("UnityEngine", "Vector3");
//        OnlyAddClass("UnityEngine", "Vector4");
//
//        OnlyAddClass("UnityEngine", "WWW");
//        OnlyAddClass("UnityEngine", "WWWForm");
//        OnlyAddClass("UnityEngine", "WaitForSeconds");
//        OnlyAddClass("UnityEngine", "WaitForFixedUpdate");
//        OnlyAddClass("UnityEngine", "WaitForEndOfFrame");
//

		Reload();
		UpdateWrapCore();
		UpdateWrapTypes ();
		//Add完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
	}

	//Wrap一个自己输入的内容，需要判断是一个类还是一个命名空间
	public void WrapCustom(string text){
		string className = "";
		string assemblyName = "";
		int dotIndex = text.LastIndexOf('.');
		if(dotIndex == -1) {
			assemblyName = "";
			className = text;
		}
		else {
			assemblyName = text.Substring(0, dotIndex);
			className = text.Substring(dotIndex + 1);
		}

		//输入的是一个类
		if(GetType(className, ref assemblyName) != null){
			WrapClass wc = GetWrapClass(assemblyName);
			if(wc == null) {
				wc = new WrapClass(assemblyName);
				AddClass(assemblyName, className);
			}
			else {
				if(!wc.m_classes.Contains(className))
					AddClass(assemblyName, className);
				else
					UpdateClass(assemblyName, className);
			}
		}
		//输入的是一个命名空间
		else{
			Type[] types = GetTypesByNamespace(text);
			if(types != null) {
				for(int i = 0; i < types.Length; i++) {
//					Debug.Log(types[i].ToString());
					AddClass(types[i].Namespace, types[i].Name);
				}
			}
		}

		Reload();
		UpdateWrapCore();
		UpdateWrapTypes ();
		//Add完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
	}

	// Use this for initialization
	bool _isCompiling = true;
	bool _option = false;
    bool _pressAll = false;
	void Start(){
		Reload();
	}
	void OnGUI () {
		if(_isCompiling != EditorApplication.isCompiling){
			_isCompiling = EditorApplication.isCompiling;
			Reload();
		}

		GUILayout.BeginHorizontal();
		if(GUILayout.Button((_option ? "\u25BC" : "\u25BA"), GUILayout.Width(25))) {
			_option = !_option;
		}
		GUILayout.Label(" Option");
		GUILayout.EndHorizontal();
		if(_option){
			GUILayout.BeginHorizontal();
			GUILayout.Label("WrapOption:", GUILayout.Width(100));
			m_ignoreObsolete = GUILayout.Toggle(m_ignoreObsolete, "Ignore Obsolete");
			m_generateLog = GUILayout.Toggle(m_generateLog, "Generate Log");
			GUILayout.EndHorizontal();

			//TODO 这里可以输入黑名单和自动添加名单

			GUILayout.Space(20);
		}

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
        GUILayout.Label("  STEP 1 : Click the button on right side →");
        GUI.backgroundColor = Color.green;
       
		if(GUILayout.Button("Wrap Common", GUILayout.Width(100))) {
			WrapCommon();
        }
//        if(GUILayout.Button("Wrap Unity.UI Assembly")) {
//            Type[] types = GetTypesByNamespace("UnityEngine");
//            types = GetTypesByNamespace("UnityEngine.UI");
//            if(types != null) {
//                for(int i = 0; i < types.Length; i++) {
//                    Debug.Log(types[i].ToString());
//                }
//            }
//        }
//        if(GUILayout.Button("Wrap Assembly-CSharp")) {
//            Type[] types = GetTypesByNamespace("");
//            if(types != null) {
//                for(int i = 0; i < types.Length; i++) {
//                    Debug.Log(types[i].ToString());
//                }
//            }
//        }
        GUILayout.EndHorizontal();
		GUILayout.Label("  to wrap UnityEngine's COMMON class and ALL non-namespace class");

		GUILayout.Space(10);

        GUI.backgroundColor = Color.white;
        GUILayout.Label("  If you'd like wrap other class or custom namespace, Then ");
        GUILayout.Label("  STEP 2 : input FULL class name in the box below and click \"Wrap Custom\"");
       

        GUILayout.BeginHorizontal(); 
        GUI.backgroundColor = Color.green;
        _classInput = GUILayout.TextField(_classInput, GUILayout.MinWidth(100));
        GUI.enabled = !string.IsNullOrEmpty(_classInput);
        if(GUILayout.Button("Wrap Custom", GUILayout.Width(100))) {
			WrapCustom(_classInput);
            _classInput = "";
        }

        GUI.enabled = true;
//
//		if(GUILayout.Button("Wrap Namespace", GUILayout.Width(120))){
//			string assemblyName = _classInput;
//			Type[] types = GetTypesByNamespace(assemblyName);
//			if(types != null){
//				for(int i = 0; i < types.Length; i++){
//					Debug.Log(types[i].ToString());
//				}
//			}
//		}
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
		GUILayout.Label("  eg. input \"LitJson.JSONNode\" for wrap a class. ");
		GUILayout.Label("  eg. input \"LiJson\" for wrap all classes in this namespace. ");

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Wraps : ");

       
        if(GUILayout.Button(_pressAll ? "Cancel" : "Select All", GUILayout.Width(80))){
            _pressAll = !_pressAll;
        }
        GUI.enabled = _pressAll;
        GUI.backgroundColor = Color.cyan;
       if(GUILayout.Button("Update", GUILayout.Width(60))) {
            //   Reload();
        }
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("X", GUILayout.Width(25))) {
			ClearAll ();
        }
        GUI.backgroundColor = Color.white;
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        //if(GUILayout.Button("Clear", GUILayout.Width(60))) {
        //    ClearAll();
        //}
        //GUI.backgroundColor = Color.white;
        //GUILayout.EndHorizontal();


		mScroll = GUILayout.BeginScrollView(mScroll);
		GUILayout.BeginVertical();
		foreach(var kvp in m_wrapClasses){
			GUILayout.BeginHorizontal();
            if(_folderNamespace.Contains(kvp.m_nameSpace)) {
                if(GUILayout.Button("\u25BC", GUILayout.Width(25))) {
                    _folderNamespace.Remove(kvp.m_nameSpace);
                }
            }
            else {
                if(GUILayout.Button("\u25BA", GUILayout.Width(25))) {
                    _folderNamespace.Add(kvp.m_nameSpace);
                }
            }
            
            GUILayout.Label("Namespace", GUILayout.Width(80));

			GUILayout.TextField(kvp.m_nameSpace);
            GUILayout.Label("    " + kvp.m_classes.Count + " Classes", GUILayout.Width(80));
            GUI.backgroundColor = Color.cyan;
            if(GUILayout.Button("Update", GUILayout.Width(60))) {
                UpdateClass(kvp.m_nameSpace);
            }
            GUI.backgroundColor = Color.red;
            if(GUILayout.Button("X", GUILayout.Width(25))) {
                RemoveClass(kvp.m_nameSpace);
                return;
            }
            GUI.backgroundColor = Color.white;
            //GUILayout.Space(90);
			GUILayout.EndHorizontal();

            if(_folderNamespace.Contains(kvp.m_nameSpace)) {
                for(int i = 0; i < kvp.m_classes.Count; i++) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(60);
                    GUILayout.Label("  Class",GUILayout.Width(54));
                    GUILayout.TextField(kvp.m_classes[i]);
                    GUI.backgroundColor = Color.cyan;
                    if(GUILayout.Button("Update", GUILayout.Width(60))) {
                        UpdateClass(kvp.m_nameSpace, kvp.m_classes[i]);
                    }
                    GUI.backgroundColor = Color.red;
                    if(GUILayout.Button("X", GUILayout.Width(25))) {
                        RemoveClass(kvp.m_nameSpace, kvp.m_classes[i]);
                        return;
                    }
                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(5);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		GUILayout.Space(10);

        if(GUILayout.Button("Reload")) {
            Reload();
        }
        GUILayout.Space(10);
	}
}
