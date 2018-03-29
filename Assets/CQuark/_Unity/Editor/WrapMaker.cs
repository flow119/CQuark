using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMaker : EditorWindow {

	[MenuItem("CQuark/Wrap Maker", false, 9)]
	[MenuItem("Assets/CQuark/Wrap Maker", false, 0)]
	static public void OpenAtlasMaker ()
	{
		EditorWindow.GetWindow<WrapMaker>(false, "Wrap Maker", true).Show();
	}

	Vector2 mScroll = Vector2.zero;
	//命名空间，类
	Dictionary<string, List<string>> m_classes = new Dictionary<string, List<string>>();

	string _wrapFolder = "";
	bool _loaded = false;
	string _classInput = "";
	string _wrapCoreTemplate = "";
	string _wrapTemplate = "";
	string WrapFolder{
		get{
			return Application.dataPath + "/" + _wrapFolder;
		}
	}

	public static Type GetType( string TypeName, ref string nameSpace){
		Type type = null;
		if(string.IsNullOrEmpty(nameSpace)){
			type = Type.GetType( TypeName );
			if(type != null){
				return type;
			}else{
				AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
				foreach( var assemblyName in referencedAssemblies ){
					// Load the referenced assembly
					var assembly = Assembly.Load( assemblyName );
					if( assembly != null ){
						type = assembly.GetType(TypeName );
						//如DebugUtil
						if( type != null )
							return type;
						
						nameSpace = assemblyName.ToString();
						type = assembly.GetType( nameSpace + "." + TypeName );
						if( type != null )
							return type;
					}
				}
			}
		}else{
			if(nameSpace == "CQuark"){
				Debug.LogError("不允许对CQuark做Wrap");
				return null;
			}
			type = Type.GetType( nameSpace + "." + TypeName  );
			if(type != null){
				//如System.DateTime
				return type;
			}
			try{
				Assembly assembly = Assembly.Load( nameSpace );
				if( assembly != null ){
					type = assembly.GetType( nameSpace + "." + TypeName );
					if( type != null ){
						//如UnityEngine.Vector3
						return type;
					}
					type = assembly.GetType(TypeName );
					if( type != null )
						return type;
				}
			}catch (Exception){
				AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
				foreach( var assemblyName in referencedAssemblies ){
					Assembly assembly = Assembly.Load( assemblyName );
					if( assembly != null ){
						//如LitJson.JSONNode
						type = assembly.GetType( nameSpace + "." + TypeName );
						if( type != null )
							return type;
					}
				}
			}
		}
		return null;
	}

	void Reload(){
		
		if(string.IsNullOrEmpty(_wrapTemplate)){
//			_wrapTemplate = (Resources.Load("WrapTemplate") as TextAsset).text;
		}
		if(string.IsNullOrEmpty(_wrapFolder)){
			_wrapFolder = PlayerPrefs.GetString("WrapFolder", "CQuark/Wrap");
		}

		m_classes = new Dictionary<string, List<string>>();
		DirectoryInfo di = new DirectoryInfo(WrapFolder);
		FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
		bool findWrapCore = false;
		for(int i = 0; i < files.Length; i++){
			string classname = files[i].Name.Split('.')[0];
			if(files[i].Directory.ToString().Length == WrapFolder.Length){
				if(classname == "Wrap"){
					findWrapCore = true;
					continue;
				}
				if(!m_classes.ContainsKey(""))
					m_classes.Add("", new List<string>());
				m_classes[""].Add(classname);
			}else{
				string nameSpace = files[i].Directory.ToString().Substring(WrapFolder.Length + 1);
				if(!m_classes.ContainsKey(nameSpace))
					m_classes.Add(nameSpace, new List<string>());
				m_classes[nameSpace].Add(classname);
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

		if(!m_classes.ContainsKey(assemblyName)){
			m_classes.Add(assemblyName, new List<string>());
		}

		//导出内容：
		//构造函数
		//静态方法
		//静态成员set_
		//静态成员get_
		//静态操作op_ 	?
		//成员方法
		//成员set
		//成员get
		//成员操作
		//index
		//协程拿出去

		string text = "";
		text += "构造函数\n";
		System.Reflection.ConstructorInfo[] construct = type.GetConstructors();
		for(int i = 0; i < construct.Length; i++){
			string s = "";
			s += construct[i].IsPublic ? "public " : "private ";
			s += construct[i].IsStatic ? "static " : "";
			s += type.ToString() + "(";
			System.Reflection.ParameterInfo[] param = construct[i].GetParameters();
			for(int j = 0; j < param.Length; j++){
				s += param[j].ParameterType + " " +param[j].Name;
				if(j != param.Length - 1)
					s += ",";
			}
			s += ")";
			text += s + "\n";
		}
		text += "\n"; 
		text += "变量\n";
		List<string> property = new List<string>();
		System.Reflection.MemberInfo[] members = type.GetMembers();
		for(int i = 0; i < members.Length; i++){
			string s = "";
			string memberType = members[i].MemberType.ToString();
			if(memberType == "Property" && !property.Contains(members[i].Name)){
				property.Add(members[i].Name);
			}
			s += memberType + " " + members[i].Name;
			text += s + "\n";
		}

		text += "\n"; 
		text += "方法\n";
		System.Reflection.MethodInfo[] methods = type.GetMethods();//这里没有获取私有方法，因为即使获取了西瓜也没有办法调用
		//基类的方法一起导出，这样可以自动调基类
		for(int i = 0; i < methods.Length; i++){
			string s = "";
			s += methods[i].IsPublic ? "public " : "private ";
			s += methods[i].IsStatic ? "static " : "";
			string retType = Type2String(methods[i].ReturnType);

			s += retType + " ";

			//如果方法名是get_开头，那么就是get成员（如果本来就命名为get_X，反射出的就是get_get_X）
			//如果方法名是set_开头，那么就是set成员（如果本来就命名为set_X，反射出的就是set_set_X）
			s += methods[i].Name + "(";
			System.Reflection.ParameterInfo[] param = methods[i].GetParameters();
			for(int j = 0; j < param.Length; j++){
				string paramType = Type2String(param[j].ParameterType);
				if(paramType.EndsWith("&")){//ref
					s += "ref " + paramType.Substring(0, paramType.Length - 1) + " " + param[j].Name;
				}else{
					s += paramType + " " + param[j].Name;
				}
				if(j != param.Length - 1)
					s += ",";
			}
			s += ")";
			text += s + "\n";
		}



		System.IO.File.WriteAllText(Application.dataPath + "/" + type + ".txt", text);
		m_classes[assemblyName].Add(classname);
	}

	static string Type2String(Type type){
		string retType = type.ToString();
		retType = retType.Replace('+','.');//A+Enum实际上是A.Enum
		retType = retType.Replace("System.Boolean", "bool");
		retType = retType.Replace("System.Byte", "byte");
		retType = retType.Replace("System.Char", "char");
		retType = retType.Replace("System.Double", "double");
		retType = retType.Replace("System.Int16", "short");
		retType = retType.Replace("System.Int32", "int");
		retType = retType.Replace("System.Int64", "long");
		retType = retType.Replace("System.Object", "object");
		retType = retType.Replace("System.Single", "float");
		retType = retType.Replace("System.String", "string");
		retType = retType.Replace("System.UInt16", "ushort");
		retType = retType.Replace("System.UInt32", "uint");
		retType = retType.Replace("System.UInt64", "ulong");
		retType = retType.Replace("System.Void", "void");
		return retType;
	}

	void OnlyRemoveClass(string assemblyName, string classname){
		m_classes[assemblyName].Remove(classname);
		if(string.IsNullOrEmpty(assemblyName))
			File.Delete(WrapFolder + "/" + classname + ".cs");
		else
			File.Delete(WrapFolder + "/" + assemblyName + "/" + classname + ".cs");
	}


	void UpdateWrapCore(){
        if(string.IsNullOrEmpty(_wrapCoreTemplate)) {
            _wrapCoreTemplate = (Resources.Load("WrapCoreTemplate") as TextAsset).text;
        }

        string wrapNew = "";
        string wrapSVGet = "";
        string wrapSVSet = "";
        string wrapSCall = "";
        string wrapMVGet = "";
        string wrapMVSet = "";
        string wrapMCall = "";
        string wrapIGet = "";
        string wrapISet = "";


        foreach(KeyValuePair<string, List<string>> kvp in m_classes) {
            for(int i = 0; i < kvp.Value.Count; i++) {
                string classFullName = kvp.Key == "" ? kvp.Value[i] : kvp.Key + "." + kvp.Value[i]; //类似UnityEngine.Vector3，用来Wrap
                string classWrapName = kvp.Key + kvp.Value[i];                                      //类似UnityEngineVector3，不带点
                
                wrapNew += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapNew += "\t\t\t\treturn " + classWrapName + "New(param, out returnValue);\r\n";
                wrapNew += "\t\t\t}\n";

                wrapSVGet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSVGet += "\t\t\t\treturn " + classWrapName + "SGet(memberName, out returnValue);\r\n";
                wrapSVGet += "\t\t\t}\n";

                wrapSVSet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSVSet += "\t\t\t\treturn " + classWrapName + "SSet(memberName, param);\r\n";
                wrapSVSet += "\t\t\t}\n";

                wrapSCall += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapSCall += "\t\t\t\treturn " + classWrapName + "SCall(functionName, param, out returnValue);\r\n";
                wrapSCall += "\t\t\t}\n";

                wrapMVGet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMVGet += "\t\t\t\treturn " + classWrapName + "MGet(objSelf, memberName, out returnValue);\r\n";
                wrapMVGet += "\t\t\t}\n";

                wrapMVSet += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMVSet += "\t\t\t\treturn " + classWrapName + "MSet(objSelf, memberName, param);\r\n";
                wrapMVSet += "\t\t\t}\n";

                wrapMCall += "\t\t\tif(type == typeof(" + classFullName + ")){\r\n";
                wrapMCall += "\t\t\t\treturn " + classWrapName + "MCall(objSelf, functionName, param, out returnValue);\r\n";
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
        File.WriteAllText(WrapFolder + "/WrapCore.cs", text, System.Text.Encoding.UTF8);
	}

	void AddClass(string assemblyName, string classname){
		OnlyAddClass(assemblyName, classname);
		UpdateWrapCore();
		//Add完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
		Reload();
	}

	void RemoveClass(string assemblyName, string classname){
		OnlyRemoveClass(assemblyName, classname);
		UpdateWrapCore();
		//Remove完毕ReloadDataBase，会编译代码
		AssetDatabase.Refresh();
		Reload();
	}

	void UpdateClass(string assemblyName, string classname){
		OnlyRemoveClass(assemblyName, classname);
		OnlyAddClass(assemblyName, classname);
        UpdateWrapCore(); // 有可能WrapCore被改坏了，还是更新一下
		AssetDatabase.Refresh();
		Reload();
	}

	// Use this for initialization
	void OnGUI () {
		if(!_loaded){
			Reload();
			_loaded = true;
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("WrapFolder:", GUILayout.Width(100));
		_wrapFolder = GUILayout.TextField(_wrapFolder);
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

        GUILayout.BeginHorizontal();
		if(GUILayout.Button("Reload")){
			Reload();
		}
        if(GUILayout.Button("UpdateAll")) {
         //   Reload();
        }
        GUILayout.EndHorizontal();

		GUILayout.Space(5);

		mScroll = GUILayout.BeginScrollView(mScroll);
		GUILayout.BeginVertical();
		foreach(var kvp in m_classes){
			GUILayout.BeginHorizontal();
			GUI.contentColor = Color.cyan;
			GUILayout.Label("Namespace : ", GUILayout.Width(100));
			GUI.contentColor = Color.white;
			GUILayout.TextField(kvp.Key);
			GUILayout.EndHorizontal();

			for(int i = 0; i < kvp.Value.Count; i++){
				GUILayout.BeginHorizontal();
				GUILayout.TextField(kvp.Value[i]);
				GUI.backgroundColor = Color.green;
				if(GUILayout.Button("Update", GUILayout.Width(60))){
					UpdateClass(kvp.Key, kvp.Value[i]);
				}
				GUI.backgroundColor = Color.red;
				if(GUILayout.Button("X", GUILayout.Width(30))){
					RemoveClass(kvp.Key,kvp.Value[i]);
				}
				GUI.backgroundColor = Color.white;
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		_classInput = GUILayout.TextField(_classInput, GUILayout.MinWidth(100));
		GUI.enabled = !string.IsNullOrEmpty(_classInput);
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add/Update", GUILayout.Width(100))){
			string className = "";
			string assemblyName = "";
			string[] s = _classInput.Split('.');
			if(s.Length == 1){
				assemblyName = "";
				className = s[0];
			}else if(s.Length == 2){
				assemblyName = s[0];
				className = s[1];
			}

			if(m_classes.ContainsKey(assemblyName) && m_classes[assemblyName].Contains(className)){
				UpdateClass(assemblyName, className);
			}else{
				AddClass(assemblyName, className);
			}
			_classInput = "";
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
	}
}
