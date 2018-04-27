using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMakerGUI : EditorWindow {

    const string WRAP_CORE_NAME = "WrapCore";

    //是否忽略Obsolete的方法
    public static bool m_ignoreObsolete = true;


    string _classInput = "";

    static string _search = "";

    Vector2 mScroll = Vector2.zero;
    List<string> _folderNamespace = new List<string>();    //被折叠的wrapclass
    List<string> _selectedClasses = new List<string>();       //选中的类

    #region WrapClass
    //这个类只是编辑器使用的，便于预览创建的Wrap而已，不影响最终发布
    [System.Serializable]
    protected class WrapClass {
        public string m_nameSpace;
        public List<string> m_classes = new List<string>();
        public WrapClass (string nameSpace) {
            m_nameSpace = nameSpace;
        }
        public void AddClass (string s) {
            m_classes.Add(s);
            m_classes.Sort();
        }
    }
    protected List<WrapClass> m_wrapClasses = new List<WrapClass>();
    protected WrapClass GetWrapClass (string key) {
        for(int i = 0; i < m_wrapClasses.Count; i++) {
            if(m_wrapClasses[i].m_nameSpace == key)
                return m_wrapClasses[i];
        }
        return null;
    }
    #endregion


	string WrapGenFolder{
		get{
			return Application.dataPath + "/CQuark/Wrap/Generate";
		}
	}

	string WrapCoreFolder{
		get{
			return Application.dataPath + "/CQuark/Wrap/Base";
		}
	}

    string WrapConfigFolder {
        get {
            return Application.dataPath + "/CQuark/Wrap/Editor/Config";
        }
    }

    [MenuItem("CQuark/Wrap Maker", false, 9)]
    [MenuItem("Assets/CQuark/Wrap Maker", false, 0)]
    static public void OpenWrapMaker () {
        EditorWindow.GetWindow<WrapMakerGUI>(false, "Wrap Maker", true).Show();
    }

	

	void Reload(){
		//sset,sget,mset,mget,new,scall,mcall,op

        m_wrapClasses.Clear();
        if(!Directory.Exists(WrapGenFolder)){
            Directory.CreateDirectory(WrapGenFolder);
        }
        else {
            DirectoryInfo di = new DirectoryInfo(WrapGenFolder);

            FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
            for(int i = 0; i < files.Length; i++) {
                string classname = files[i].Name.Split('.')[0];
                if(files[i].Directory.ToString().Length == WrapGenFolder.Length) {
                    WrapClass wc = GetWrapClass("");
                    if(wc == null) {
                        wc = new WrapClass("");
                        m_wrapClasses.Add(wc);
                    }
                    wc.AddClass(classname);
                }
                else {
                    string nameSpace = files[i].Directory.ToString().Substring(WrapGenFolder.Length + 1);

                    WrapClass wc = GetWrapClass(nameSpace);
                    if(wc == null) {
                        wc = new WrapClass(nameSpace);
                        m_wrapClasses.Add(wc);
                    }
                    wc.AddClass(classname);
                }
            }
		}
	}

	void OnlyAddClass(Type type){
		OnlyAddClass(type.Namespace, type.Name);
	}
	//这里可以改为使用类
	void OnlyAddClass(string assemblyName, string classname){
        List<string> _typeDic = new List<string>();

		if(classname.Contains("`"))
			return;
		if(string.IsNullOrEmpty(assemblyName))
			assemblyName = "";
		
		Type type = WrapReflectionTools.GetType(classname, ref assemblyName);

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
        List<Property> savePropertys = WrapReflectionTools.GetPropertys(type, ref log);
        string[] propertyPartStr = WrapTextTools.Propertys2PartStr(classFullName, savePropertys);

		//构造函数，
        List<Method> constructMethods = WrapReflectionTools.GetConstructor(type, ref log);
        string wrapNew = WrapTextTools.Constructor2PartStr(classFullName, constructMethods);
        WrapTextTools.CallTypes2TypeStr(classFullName, constructMethods, _typeDic);

		//静态方法（最后的参数是忽略属性，因为属性也是一种方法）
        List<Method> staticMethods = WrapReflectionTools.GetStaticMethods(type, ref log, savePropertys);
        string wrapSCall = WrapTextTools.SCall2PartStr(classFullName, staticMethods);
        WrapTextTools.CallTypes2TypeStr(classFullName, staticMethods, _typeDic);

		//成员方法
        List<Method> memberMethods = WrapReflectionTools.GetInstanceMethods(type, ref log, savePropertys);
        string wrapMCall = WrapTextTools.MCall2PartStr(classFullName, memberMethods);
        WrapTextTools.CallTypes2TypeStr(classFullName, memberMethods, _typeDic);

		//索引
        List<Method> indexMethods = WrapReflectionTools.GetIndex(type, ref log);
        string[] wrapIndex = WrapTextTools.Index2PartStr(classFullName, indexMethods);

		//运算符（数学运算和逻辑运算）
        List<Method> opMethods = WrapReflectionTools.GetOp(type, ref log);
        string[] wrapOp = WrapTextTools.Op2PartStr(opMethods);

        //if(m_generateLog){
	        if(string.IsNullOrEmpty(assemblyName)) {
                WrapTextTools.WriteAllText(WrapGenFolder, classname + ".txt", log);
			}
			else {
                WrapTextTools.WriteAllText(WrapGenFolder + "/" + assemblyName, classname + ".txt", log);
			}
        //}

		UpdateWrapPart(assemblyName, classname, _typeDic, propertyPartStr,
            wrapNew, wrapSCall, wrapMCall, wrapIndex, wrapOp);
	}

	void OnlyRemoveClass(string assemblyName, string classname){
		if(string.IsNullOrEmpty(assemblyName))
			File.Delete(WrapGenFolder + "/" + classname + ".cs");
		else
			File.Delete(WrapGenFolder + "/" + assemblyName + "/" + classname + ".cs");
	}
		
	void UpdateWrapPart(string assemblyName, string classname, List<string> typeDic, string[] propertys, 
		string wrapNew, string wrapSCall, string wrapMCall, string[] wrapIndex, string[] wrapOp){

		string classWrapName = assemblyName.Replace(".","") + classname;   //类似UnityEngineVector3，不带点
        string text = File.ReadAllText(WrapConfigFolder + "/WrapPartTemplate.txt", System.Text.Encoding.UTF8);// (Resources.Load("WrapPartTemplate") as TextAsset).text;

        string types = "";
        for(int i = 0; i < typeDic.Count; i++) {
            types += "\t\t" + typeDic[i] + "\n";
        }
        text = text.Replace("{TypesArray}", types);

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
            WrapTextTools.WriteAllText(WrapGenFolder, classname + ".cs", text);
		}
		else {
            WrapTextTools.WriteAllText(WrapGenFolder + "/" + assemblyName, classname + ".cs", text);
		}
	}
		
	void UpdateWrapCore(){
//		//测试
//		return;
        string text = File.ReadAllText(WrapConfigFolder + "/WrapCoreTemplate.txt", System.Text.Encoding.UTF8);
        //string text = (Resources.Load("WrapCoreTemplate") as TextAsset).text;

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

	void AddClass(string assemblyName, string classname){
		OnlyAddClass(assemblyName, classname);
		Reload();
		UpdateWrapCore();
        
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
        if(WrapReflectionTools.GetType(className, ref assemblyName) != null) {
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
            Type[] types = WrapReflectionTools.GetTypesByNamespace(text);
			if(types != null) {
				for(int i = 0; i < types.Length; i++) {
//					Debug.Log(types[i].ToString());
					AddClass(types[i].Namespace, types[i].Name);
				}
			}
		}

		Reload();
		UpdateWrapCore();
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

		if(WrapGUITools.DrawHeader("Option")) {
			WrapGUITools.BeginContents();
            //GUILayout.BeginHorizontal();
            GUILayout.Label("WrapOption:", GUILayout.Width(100));
            m_ignoreObsolete = GUILayout.Toggle(m_ignoreObsolete, "Ignore Obsolete");
            //m_generateLog = GUILayout.Toggle(m_generateLog, "Generate Log");
            //GUILayout.EndHorizontal();

            //TODO 这里可以输入黑名单和自动添加名单

            GUILayout.Space(20);
			WrapGUITools.EndContents();
        }

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
        GUILayout.Label("  STEP 1 : Click the button on right side →", "BoldLabel");
        GUI.backgroundColor = Color.green;
       
		if(GUILayout.Button("Wrap Common", GUILayout.Width(100))) {
			WrapCommon();
        }

        GUILayout.EndHorizontal();
		GUILayout.Label("  to wrap UnityEngine's COMMON class and ALL non-namespace class");

		GUILayout.Space(10);

        GUI.backgroundColor = Color.white;
        GUILayout.Label("  If you'd like wrap other class or custom namespace, Then ");
        GUILayout.Label("  STEP 2 : input FULL class name in the box below and click \"Wrap Custom\"", "BoldLabel");
       

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
        EditorGUILayout.HelpBox("  eg. input \"LitJson.JSONNode\" for wrap a class. \n  eg. input \"LiJson\" for wrap all classes in this namespace. ", MessageType.Info);

        GUILayout.Space(10);

//		if(WrapGUITools.DrawHeader("Wraps")){

        GUILayout.BeginHorizontal();
        GUILayout.Space(80);
        GUILayout.FlexibleSpace();
        int fontSize = GUI.skin.label.fontSize;
        GUI.skin.label.fontSize = 30;
        GUILayout.Label("Wraps");
        GUI.skin.label.fontSize = fontSize;
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Reload", GUILayout.Width(80))) {
			Reload();
		}
        GUILayout.EndHorizontal();

//			NGUIEditorTools.BeginContents();
        //搜索框
        GUILayout.BeginHorizontal();
        GUILayout.Space(84f);
		string before = _search;
        string after = EditorGUILayout.TextField("", before, "SearchTextField");
        if(before != after) 
			_search = after;

        if(GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f))) {
			_search = "";
            GUIUtility.keyboardControl = 0;
        }
        GUILayout.Space(84f);
		
        GUILayout.EndHorizontal();
        //搜索结束

//        GUILayout.BeginHorizontal();
//        GUILayout.Label("Wraps : ");
//        if(GUILayout.Button(_pressAll ? "Cancel" : "Select All", GUILayout.Width(80))){
//            _pressAll = !_pressAll;
//        }
//        GUI.enabled = _pressAll;
//        GUI.backgroundColor = Color.cyan;
//       if(GUILayout.Button("Update", GUILayout.Width(60))) {
//            //   Reload();
//        }
//        GUI.backgroundColor = Color.red;
//        if(GUILayout.Button("X", GUILayout.Width(25))) {
//			ClearAll ();
//        }
//        GUI.backgroundColor = Color.white;
//        GUI.enabled = true;
//        GUILayout.EndHorizontal();

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
				if(GUILayout.Button("  \u25BC", "PreToolbar2", GUILayout.Width(20))) {
                    _folderNamespace.Remove(kvp.m_nameSpace);
                }
            }
            else {
				if(GUILayout.Button("  \u25BA",  "PreToolbar2", GUILayout.Width(20))) {
                    _folderNamespace.Add(kvp.m_nameSpace);
                }
            }
            

			Action<string> selectAll = delegate(string ns) {
				for(int i = 0; i < m_wrapClasses.Count; i++){
					if(m_wrapClasses[i].m_nameSpace == ns){
						for(int j = 0; j < m_wrapClasses[i].m_classes.Count; j++){
							string fullName = string.IsNullOrEmpty(ns) ? "" : ns + ".";
							fullName += m_wrapClasses[i].m_classes[j];
							if(!_selectedClasses.Contains(fullName))
								_selectedClasses.Add(fullName);
						}
						break;
					}
				}
			};
			Action<string> deselectAll = delegate(string ns) {
				for(int i = 0; i < m_wrapClasses.Count; i++){
					if(m_wrapClasses[i].m_nameSpace == ns){
						for(int j = 0; j < m_wrapClasses[i].m_classes.Count; j++){
							string fullName = string.IsNullOrEmpty(ns) ? "" : ns + ".";
							fullName += m_wrapClasses[i].m_classes[j];
							if(_selectedClasses.Contains(fullName))
								_selectedClasses.Remove(fullName);
						}
						break;
					}
				}
			};
			if(IsAllSelect(kvp.m_nameSpace)){
				bool b = GUILayout.Toggle(true,"",GUILayout.Width(12));
				if(!b){
					deselectAll(kvp.m_nameSpace);
				}
			}else if(IsNoneSelect(kvp.m_nameSpace)){
				bool b = GUILayout.Toggle(false,"",GUILayout.Width(12));
				if(b){
					selectAll(kvp.m_nameSpace);
				}
			}else{
				bool b = GUILayout.Toggle(true, "", "ToggleMixed",GUILayout.Width(12));
				if(!b){
					deselectAll(kvp.m_nameSpace);
				}
			}

            GUILayout.Label("Ns", "sv_label_2", GUILayout.Width(38));
            EditorGUILayout.SelectableLabel(kvp.m_nameSpace, GUILayout.Height(16));	//EditorGUILayour可以把文字拷出来
			
//	            GUILayout.Label("    " + kvp.m_classes.Count + " Classes", GUILayout.Width(80));
//	            GUI.backgroundColor = Color.cyan;
//	            if(GUILayout.Button("Update", GUILayout.Width(60))) {
//	                UpdateClass(kvp.m_nameSpace);
//	            }
//	            GUI.backgroundColor = Color.red;
//	            if(GUILayout.Button("X", GUILayout.Width(25))) {
//	                RemoveClass(kvp.m_nameSpace);
//	                return;
//	            }
//	            GUI.backgroundColor = Color.white;
            //GUILayout.Space(90);
			GUILayout.EndHorizontal();

            if(_folderNamespace.Contains(kvp.m_nameSpace)) {
                for(int i = 0; i < kvp.m_classes.Count; i++) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(34);

					string fullName = (string.IsNullOrEmpty(kvp.m_nameSpace) ? "" : kvp.m_nameSpace + ".") + kvp.m_classes[i];
					bool select = _selectedClasses.Contains(fullName);
					bool old = GUILayout.Toggle(select,"",GUILayout.Width(12));
					if(old != select){
						if(old)
							_selectedClasses.Add(fullName);
						else
							_selectedClasses.Remove(fullName);
					}
                    GUILayout.Label("C", "sv_label_1", GUILayout.Width(28));
					EditorGUILayout.SelectableLabel(kvp.m_classes[i], GUILayout.Height(16));
                   
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(5);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("All", GUILayout.Width(60))){
			_selectedClasses.Clear();
			for(int i = 0; i < m_wrapClasses.Count; i++){
				for(int j = 0; j < m_wrapClasses[i].m_classes.Count; j++){
					string fullName = string.IsNullOrEmpty(m_wrapClasses[i].m_nameSpace) ? "" : m_wrapClasses[i].m_nameSpace + ".";
					fullName += m_wrapClasses[i].m_classes[j];
					_selectedClasses.Add(fullName);
				}
			}
		}
		if(GUILayout.Button("None", GUILayout.Width(60))){
			_selectedClasses.Clear();
		}

		GUILayout.FlexibleSpace();

		GUI.backgroundColor = Color.cyan;
		if(GUILayout.Button("Update", GUILayout.Width(60))) {
			//	UpdateClass(kvp.m_nameSpace, kvp.m_classes[i]);
		}
		GUI.backgroundColor = Color.red;
		if(GUILayout.Button("Remove", GUILayout.Width(60))) {
			for(int i = 0; i < _selectedClasses.Count; i++){
			//	OnlyRemoveClass(WrapReflectionTools.GetType();
			}
			//	RemoveClass(kvp.m_nameSpace, kvp.m_classes[i]);
			return;
		}
		GUI.backgroundColor = Color.white;
		GUILayout.EndHorizontal();
//			NGUIEditorTools.EndContents();
//		}




		GUILayout.Space(10);

//        GUILayout.BeginHorizontal();
//        GUILayout.Space(68);
//        if(GUILayout.Button("Reload")) {
//            Reload();
//        }
//        GUILayout.Space(68);
//        GUILayout.EndHorizontal();


	}

	bool IsAllSelect(string nameSpace){
		for(int i = 0; i < m_wrapClasses.Count; i++){
			if(m_wrapClasses[i].m_nameSpace == nameSpace){
				for(int j = 0; j < m_wrapClasses[i].m_classes.Count; j++){
					string fullName = string.IsNullOrEmpty(nameSpace) ? m_wrapClasses[i].m_classes[j] : nameSpace + "." + m_wrapClasses[i].m_classes[j];
					if(!_selectedClasses.Contains(fullName))
						return false;
				}
				return true;
			}
		}
		return false;
	}

	bool IsNoneSelect(string nameSpace){
		for(int i = 0; i < m_wrapClasses.Count; i++){
			if(m_wrapClasses[i].m_nameSpace == nameSpace){
				for(int j = 0; j < m_wrapClasses[i].m_classes.Count; j++){
					string fullName = string.IsNullOrEmpty(nameSpace) ? m_wrapClasses[i].m_classes[j] : nameSpace + "." + m_wrapClasses[i].m_classes[j];
					if(_selectedClasses.Contains(fullName))
						return false;
				}
				return true;
			}
		}
		return false;
	}
}
