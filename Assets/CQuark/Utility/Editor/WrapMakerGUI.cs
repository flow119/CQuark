using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMakerGUI : WrapMaker {

    List<string> _folderNamespace = new List<string>();    //被折叠的wrapclass
    List<string> _deleteClasses = new List<string>();       //准备删除的类
    
    const string WRAP_CORE_NAME = "WrapCore";
    const string WRAP_UTIL_NAME = "WrapUtil";

	[MenuItem("CQuark/Wrap Maker", false, 9)]
	[MenuItem("Assets/CQuark/Wrap Maker", false, 0)]
	static public void OpenWrapMaker ()
	{
		EditorWindow.GetWindow<WrapMakerGUI>(false, "Wrap Maker", true).Show();
	}

	Vector2 mScroll = Vector2.zero;

	string _wrapFolder = "";
	string _classInput = "";

	string WrapFolder{
		get{
			return Application.dataPath + "/" + _wrapFolder;
		}
	}


	void Reload(){
		if(string.IsNullOrEmpty(_wrapFolder)){
			_wrapFolder = PlayerPrefs.GetString("WrapFolder", "CQuark/Wrap");
		}

        m_wrapClasses.Clear();
		DirectoryInfo di = new DirectoryInfo(WrapFolder);
		FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
		for(int i = 0; i < files.Length; i++){
			string classname = files[i].Name.Split('.')[0];
			if(files[i].Directory.ToString().Length == WrapFolder.Length){
                if(classname == WRAP_CORE_NAME || classname == WRAP_UTIL_NAME) {
					continue;
				}

                WrapClass wc = GetWrapClass("");
                if(wc == null) {
                    wc = new WrapClass("");
                    m_wrapClasses.Add(wc);
                }
                wc.AddClass(classname);
			}else{
				string nameSpace = files[i].Directory.ToString().Substring(WrapFolder.Length + 1);
				
                WrapClass wc = GetWrapClass(nameSpace);
                if(wc == null) {
                    wc = new WrapClass(nameSpace);
                    m_wrapClasses.Add(wc);
                }
                wc.AddClass(classname);
			}
		}
		PlayerPrefs.SetString("WrapFolder", _wrapFolder);
	}

	void OnlyAddClass(string assemblyName, string classname){
		Type type = GetType(classname, ref assemblyName);

		if(type == null){
			Debug.LogError("No Such Type : " + classname);
			return;
		}

		string log = "";//一个记事本，用来保存哪些内容做了Wrap

		//变量或属性
		List<Property> savePropertys = GetPropertys(type, ref log);

		//构造函数，
		List<Method> constructMethods = GetConstruction(type, ref log);

		//运算符（数学运算和逻辑运算）
		List<Method> opMethods = GetOp (type, ref log);

		//静态方法（最后的参数是忽略属性，因为属性也是一种方法）
		List<Method> staticMethods = GetStaticMethods (type, ref log, savePropertys);

		//成员方法
		List<Method> memberMethods = GetInstanceMethods (type, ref log, savePropertys);

		//Index
		List<Method> indexMethods = GetIndex (type, ref log);

        if(string.IsNullOrEmpty(assemblyName)) {
			WriteAllText(WrapFolder, classname + ".txt", log);
		}
		else {
			WriteAllText(WrapFolder + "/" + assemblyName, classname + ".txt", log);
		}

		UpdateWrapPart(assemblyName, classname, savePropertys, 
		               constructMethods, staticMethods, memberMethods, opMethods);
	}

	static void WriteAllText(string folder, string name, string content){
		if(!Directory.Exists(folder))
			Directory.CreateDirectory(folder);
		File.WriteAllText(folder + "/" + name, content, System.Text.Encoding.UTF8);
	}

   



	void OnlyRemoveClass(string assemblyName, string classname){
		if(string.IsNullOrEmpty(assemblyName))
			File.Delete(WrapFolder + "/" + classname + ".cs");
		else
			File.Delete(WrapFolder + "/" + assemblyName + "/" + classname + ".cs");
	}

	void UpdateWrapPart(string assemblyName, string classname, List<Property> propertys, 
	                    List<Method> contruction, List<Method> staticMethods, List<Method> instanceMethods, List<Method> opMethods){

		//测试
		return;

		string classWrapName = assemblyName.Replace(".","") + classname;                                      //类似UnityEngineVector3，不带点
		string classFullName = string.IsNullOrEmpty(assemblyName) ? classname : assemblyName + "." + classname;
		string _wrapPartTemplate = (Resources.Load("WrapPartTemplate") as TextAsset).text;

		string wrapSVGet = "";
		string wrapSVSet = "";
		string wrapMVGet = "";
		string wrapMVSet = "";

		for(int i = 0; i < propertys.Count; i++){
			if(!Finish(propertys[i].m_type))
				continue;
			if(propertys[i].m_isStatic){
				if(propertys[i].m_canGet){
					wrapSVGet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
					wrapSVGet += "\t\t\t\treturnValue = new CQ_Value();\n";
					wrapSVGet += "\t\t\t\treturnValue.type = typeof(" + propertys[i].m_type + ");\n";
					wrapSVGet += "\t\t\t\treturnValue.value = ";
					wrapSVGet += classFullName + "." + propertys[i].m_name + ";\n";
					wrapSVGet += "\t\t\t\treturn true;\n";
				}
				if(propertys[i].m_canSet){
                    wrapSVSet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapSVSet += "\t\t\t\tif(param.EqualOrImplicateType(typeof(" + propertys[i].m_type + "))){\n";
                    wrapSVSet += "\t\t\t\t\t" + classFullName + "." + propertys[i].m_name + " = (" + propertys[i].m_type + ")" + "param.ConvertTo(typeof(" + propertys[i].m_type + "));\n";
                    wrapSVSet += "\t\t\t\t\treturn true;\n";
                    wrapSVSet += "\t\t\t\t}\n\t\t\t\tbreak;\n";
				}
			}else{
				if(propertys[i].m_canGet){
					wrapMVGet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
					wrapMVGet += "\t\t\t\treturnValue = new CQ_Value();\n";
					wrapMVGet += "\t\t\t\treturnValue.type = typeof(" + propertys[i].m_type + ");\n";
					wrapMVGet += "\t\t\t\treturnValue.value = ";
					wrapMVGet += "obj." + propertys[i].m_name + ";\n";
					wrapMVGet += "\t\t\t\treturn true;\n";
				}
				if(propertys[i].m_canSet){
                    wrapMVSet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapMVSet += "\t\t\t\tif(param.EqualOrImplicateType(typeof(" + propertys[i].m_type + "))){\n";
                    wrapMVSet += "\t\t\t\t\tobj." + propertys[i].m_name + " = (" + propertys[i].m_type + ")" + "param.ConvertTo(typeof(" + propertys[i].m_type + "));\n";
                    wrapMVSet += "\t\t\t\t\treturn true;\n";
                    wrapMVSet += "\t\t\t\t}\n\t\t\t\tbreak;\n";
				}
			}
		}

		if(!string.IsNullOrEmpty(wrapSVGet)){
			wrapSVGet = "\t\t\tswitch(memberName) {\n" 
                + wrapSVGet + "\t\t\t}";
		}
        if(!string.IsNullOrEmpty(wrapSVSet)) {
            wrapSVSet = "\t\t\tswitch(memberName) {\n" 
                + wrapSVSet + "\t\t\t}";
        }
		if(!string.IsNullOrEmpty(wrapMVGet)){
			wrapMVGet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
				+"\t\t\tswitch(memberName) {\n" 
                + wrapMVGet + "\t\t\t}";
		}
        if(!string.IsNullOrEmpty(wrapMVSet)) {
            wrapMVSet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + "\t\t\tswitch(memberName) {\n" 
                + wrapMVSet + "\t\t\t}";
        }


		string wrapNew = "";
		for(int i = 0; i < contruction.Count; i++) {
			if(contruction[i].m_inType.Length == 0)
                wrapNew += "\t\t\tif(param.Count == 0){\n";
            else{
				wrapNew += "\t\t\tif(param.Count == " + contruction[i].m_inType.Length + " && MatchType(param, new Type[] {";
				for(int j = 0; j < contruction[i].m_inType.Length; j++) {
					wrapNew += "typeof(" + contruction[i].m_inType[j] + ")";
					if(j != contruction[i].m_inType.Length - 1)
                        wrapNew += ",";
                }
                wrapNew += "}, mustEqual)){\n";
            }
            wrapNew += "\t\t\t\treturnValue = new CQ_Value();\n";
			wrapNew += "\t\t\t\treturnValue.type = typeof(" + classFullName + ");\n";
			wrapNew += "\t\t\t\treturnValue.value = new " + classFullName + "(";
			for(int j = 0; j < contruction[i].m_inType.Length; j++) {
				wrapNew += "(" + contruction[i].m_inType[j] + ")param[" + j + "].ConvertTo(typeof(" + contruction[i].m_inType[j] + "))";
				if(j != contruction[i].m_inType.Length - 1)
                    wrapNew += ",";
            }
            wrapNew += ");\n";
            wrapNew += "\t\t\t\treturn true;\n\t\t\t}\n";
        }

		string wrapSCall = "";
        for(int i = 0; i < staticMethods.Count; i++) {
				if(!Finish(staticMethods[i].m_returnType) || !Finish(staticMethods[i].m_inType))
                    continue;

				if(staticMethods[i].m_inType.Length == 0)
					wrapSCall += "\t\t\tif(param.Count == 0 && functionName == \"" + staticMethods[i].m_methodName + "\"){\n";
                else {
					wrapSCall += "\t\t\tif(param.Count == " + staticMethods[i].m_inType.Length + " && functionName == \"" + staticMethods[i].m_methodName + "\" && MatchType(param, new Type[] {";
					for(int j = 0; j < staticMethods[i].m_inType.Length; j++) {
						wrapSCall += "typeof(" + staticMethods[i].m_inType[j] + ")";
						if(j != staticMethods[i].m_inType.Length - 1)
                            wrapSCall += ",";
                    }
                    wrapSCall += "}, mustEqual)){\n";
                }
				if(staticMethods[i].m_returnType == "void"){
                    wrapSCall += "\t\t\t\treturnValue = null;\n";
                }else{
                    wrapSCall += "\t\t\t\treturnValue = new CQ_Value();\n";
					wrapSCall += "\t\t\t\treturnValue.type = typeof(" + staticMethods[i].m_returnType + ");\n";
                    wrapSCall += "\t\t\t\treturnValue.value = ";
                }
				wrapSCall += classFullName + "." + staticMethods[i].m_methodName + "(";
				for(int j = 0; j < staticMethods[i].m_inType.Length; j++) {
					wrapSCall += "(" + staticMethods[i].m_inType[j] + ")param[" + j + "].ConvertTo(typeof(" + staticMethods[i].m_inType[j] + "))";
					if(j != staticMethods[i].m_inType.Length - 1)
                        wrapSCall += ",";
                }
                wrapSCall += ");\n";
                wrapSCall += "\t\t\t\treturn true;\n\t\t\t}\n";
        }

		string wrapMCall = "";
        for(int i = 0; i < instanceMethods.Count; i++) {
			if(!Finish(instanceMethods[i].m_returnType) || !Finish(instanceMethods[i].m_inType))
					continue;

			if(instanceMethods[i].m_inType.Length == 0)
				wrapMCall += "\t\t\tif(param.Count == 0 && functionName == \"" + instanceMethods[i].m_methodName + "\"){\n";
			else {
				wrapMCall += "\t\t\tif(param.Count == " + instanceMethods[i].m_inType.Length + " && functionName == \"" + instanceMethods[i].m_methodName + "\" && MatchType(param, new Type[] {";
				for(int j = 0; j < instanceMethods[i].m_inType.Length; j++) {
					wrapMCall += "typeof(" + instanceMethods[i].m_inType[j] + ")";
					if(j != instanceMethods[i].m_inType.Length - 1)
						wrapMCall += ",";
				}
				wrapMCall += "}, mustEqual)){\n";
			}
			if(instanceMethods[i].m_returnType == "void"){
				wrapMCall += "\t\t\t\treturnValue = null;\n\t\t\t\t";
			}else{
				wrapMCall += "\t\t\t\treturnValue = new CQ_Value();\n";
				wrapMCall += "\t\t\t\treturnValue.type = typeof(" + instanceMethods[i].m_returnType + ");\n";
				wrapMCall += "\t\t\t\treturnValue.value = ";
			}
			wrapMCall += "obj." + instanceMethods[i].m_methodName + "(";
			for(int j = 0; j < instanceMethods[i].m_inType.Length; j++) {
				wrapMCall += "(" + instanceMethods[i].m_inType[j] + ")param[" + j + "].ConvertTo(typeof(" + instanceMethods[i].m_inType[j] + "))";
				if(j != instanceMethods[i].m_inType.Length - 1)
					wrapMCall += ",";
			}
			wrapMCall += ");\n";
			wrapMCall += "\t\t\t\treturn true;\n\t\t\t}\n";
        }
		if(!string.IsNullOrEmpty(wrapMCall)) {
			wrapMCall = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
				+ wrapMCall + "\t\t\t";
		}

//        string wrapOp = "";
//		string wrapAdd = "";
//		string wrapSub = "";
//		string wrapMul = "";
//		string wrapDiv = "";
//		string wrapMod = "";

		string text = _wrapPartTemplate.Replace("{0}", classWrapName);
		text = text.Replace("{1}", wrapNew);	
		text = text.Replace("{2}", wrapSVGet);  
        text = text.Replace("{3}", wrapSVSet);  
		text = text.Replace("{4}", wrapSCall);	
        text = text.Replace("{5}", wrapMVGet);  
        text = text.Replace("{6}", wrapMVSet);  
		text = text.Replace("{7}", wrapMCall);
		text = text.Replace("{8}", "");//IndexGet 还没想好怎么做
		text = text.Replace("{9}", "");//IndexSet 还没想好怎么做
		//TODO OP text = text.OP
     //  (op_Addition,op_subtraction,op_Multiply,op_Division,op_Modulus,op_GreaterThan,op_LessThan,op_GreaterThanOrEqual,op_LessThanOrEqual,op_Equality,op_Inequality

		if(string.IsNullOrEmpty(assemblyName)) {
			WriteAllText(WrapFolder, classname + ".cs", text);
		}
		else {
			WriteAllText(WrapFolder + "/" + assemblyName, classname + ".cs", text);
		}
	}

	static bool Finish(string type){
		if(type.EndsWith("&"))	//ref
			return false;
		if(type.Contains("List`"))	//List
			return false;
		if(type.Contains("IEnumerable`"))
			return false;
		if(type == "T" || type == "[T]" || type == "T[]")	//T , 比如GetComponent<T>
			return false;
		if(type =="System.Collections.IEnumerator")			//
			return false;	
		return true;
	}

	static bool Finish (string[] types) {
		for(int j = 0; j < types.Length; j++) {
			if(!Finish(types[j]))
				return false;
		}
		return true;
	}

	void UpdateWrapCore(){
		//测试
		return;

        string _wrapCoreTemplate = (Resources.Load("WrapCoreTemplate") as TextAsset).text;

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
            }
        }

        string text = _wrapCoreTemplate.Replace("{0}", wrapNew);
        text = text.Replace("{1}", wrapSVGet);
        text = text.Replace("{2}", wrapSVSet);
        text = text.Replace("{3}", wrapSCall);
        text = text.Replace("{4}", wrapMVGet);
        text = text.Replace("{5}", wrapMVSet);
        text = text.Replace("{6}", wrapMCall);
        text = text.Replace("{7}", wrapIGet);
        text = text.Replace("{8}", wrapISet);
        //string text = string.Format(_wrapCoreTemplate, wrapNew, wrapSVGet, wrapSVSet, wrapSCall, wrapMVGet, wrapMVSet, wrapMCall, wrapIGet, wrapISet);
        File.WriteAllText(WrapFolder + "/" + WRAP_CORE_NAME + ".cs", text, System.Text.Encoding.UTF8);
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
        m_wrapClasses.Clear();
		Reload();
        UpdateWrapCore();
        AssetDatabase.Refresh();
    }

	// Use this for initialization
	bool _isCompiling = true;
	void Start(){
		Reload();
	}
	void OnGUI () {
		if(_isCompiling != EditorApplication.isCompiling){
			_isCompiling = EditorApplication.isCompiling;
			Reload();
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("WrapFolder:", GUILayout.Width(100));
		_wrapFolder = GUILayout.TextField(_wrapFolder);
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

        GUILayout.Label("    You can make Wrap with this tool simply.\n"
                            +"    Step 1 : Type Full class name in the box below（eg . UnityEngine.Vector3)\n" 
                            +"    Step 2 : Click \"Add/Update\" button");

        GUILayout.Space(5);

        GUILayout.BeginHorizontal(); 
        GUI.backgroundColor = Color.green;
        GUILayout.Label("Full Classname : ", GUILayout.Width(100));
        _classInput = GUILayout.TextField(_classInput, GUILayout.MinWidth(100));
        GUI.enabled = !string.IsNullOrEmpty(_classInput);
        if(GUILayout.Button("Add/Update", GUILayout.Width(100))) {
            string className = "";
            string assemblyName = "";

            int dotIndex = _classInput.LastIndexOf('.');
            if(dotIndex == -1) {
                assemblyName = "";
                className = _classInput;
            }
            else {
                assemblyName = _classInput.Substring(0, dotIndex);
                className = _classInput.Substring(dotIndex + 1);
            }

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

            _classInput = "";
        }
        GUI.enabled = true;
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();


        GUILayout.Label("    Or just click the button below");
        GUI.backgroundColor = Color.green;
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add/Update Full project")) {
            //   Reload();
        }
        if(GUILayout.Button("Update Registed wrap")) {
            //   Reload();
        }
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("Clear All wraps")) {
            //   Reload();
        }
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();

        //if(GUILayout.Button("Clear", GUILayout.Width(60))) {
        //    ClearAll();
        //}
        //GUI.backgroundColor = Color.white;
        //GUILayout.EndHorizontal();

		GUILayout.Space(10);

        GUILayout.Label("Wraps : ");

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

        GUILayout.Label("If the Serializable Field is missing, Click the button below ↓");
        GUILayout.Space(5);
        if(GUILayout.Button("Reload")) {
            Reload();
        }
        GUILayout.Space(10);
	}
}
