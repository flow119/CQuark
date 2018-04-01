using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMaker : EditorWindow {

	class Property{
		public string m_type;
		public bool m_isStatic;
		public bool m_canGet;
		public bool m_canSet;
		public string m_name;

		public Property(Type type, bool isStatic, bool canGet, bool canSet, string name){
			m_type = Type2String(type);
			m_isStatic = isStatic;
			m_canGet = canGet;
			m_canSet = canSet;
			m_name = name;
		}
	}

    const string WRAP_CORE_NAME = "WrapCore";
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
                if(classname == WRAP_CORE_NAME) {
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
			
		//TODO 暂时不包含op，不包含index，不包含ref，in, out, 不包含IEnumrator
		List<Property> propertys = new List<Property>();

		string note = "";
#region 变量或属性
		note += "变量\n";
		FieldInfo[] fis = type.GetFields();
		for(int i= 0; i < fis.Length; i++){
			string attributes = fis[i].Attributes.ToString();
			bool isStatic = attributes.Contains("Static");
			if(isStatic)
				note += "static ";
			bool isReadonly = attributes.Contains("Literal") && attributes.Contains("HasDefault");
			if(isReadonly)
				note += "readonly ";
			note += Type2String(fis[i].FieldType) + " " + fis[i].Name + ";\n";
			propertys.Add(new Property(fis[i].FieldType, isStatic, true, !isReadonly, fis[i].Name));
		}

		note += "属性\n";
		PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
		for(int i= 0; i < pis.Length; i++){
			note += "public ";
			string attributes = pis[i].Attributes.ToString();

			note += "static " + Type2String(pis[i].PropertyType) + " " + pis[i].Name + "{";
			if(pis[i].CanRead) 
				note += "get;";
			if(pis[i].CanWrite)
				note += "set;";
			note += "}\n";

			propertys.Add(new Property(pis[i].PropertyType, true, pis[i].CanRead, pis[i].CanWrite, pis[i].Name));
		}
		pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		for(int i= 0; i < pis.Length; i++){
			note += "public ";
			string attributes = pis[i].Attributes.ToString();
			note += Type2String(pis[i].PropertyType) + " " + pis[i].Name + "{";
			if(pis[i].CanRead) 
				note += "get;";
			if(pis[i].CanWrite)
				note += "set;";
			note += "}\n";
			if(pis[i].Name == "Item")
				continue;//这里有个问题，实例的Item无法确认是属性还是[index]
			if(pis[i].PropertyType.ToString().Contains("IEnumerable`"))
				continue;//暂时先不处理枚举器
			propertys.Add(new Property(pis[i].PropertyType, false, pis[i].CanRead, pis[i].CanWrite, pis[i].Name));

		}
#endregion

		note += "\n";

        note += "构造函数\n";
		System.Reflection.ConstructorInfo[] construct = type.GetConstructors();
		for(int i = 0; i < construct.Length; i++){
			string s = "";
			s += type.ToString() + "(";
			System.Reflection.ParameterInfo[] param = construct[i].GetParameters();
			for(int j = 0; j < param.Length; j++){
				s += param[j].ParameterType + " " +param[j].Name;
				if(j != param.Length - 1)
					s += ",";
				if(param[j].IsOptional)
					s += " = " + param[j].DefaultValue;
			}
			s += ")";
            note += s + "\n";
		}
        note += "\n";
//        note += "成员\n";
//		List<string> property = new List<string>();
//		System.Reflection.MemberInfo[] members = type.GetMembers();
//		for(int i = 0; i < members.Length; i++){
//			string memberType = members[i].MemberType.ToString();
//			if(memberType == "Property"){
//				property.Add(members[i].Name);
//                note += memberType + " " + members[i].Name + "\n";
//			}else if(memberType == "Field"){
//                note += memberType + " " + members[i].Name + "\n";
//            }
//		}



        note += "\n";
        note += "方法\n";
		//TODO 默认参， ref in out
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
					s += paramType + " " + param[j].Name ;
					if(param[j].IsOptional)
						s += " = " + param[j].DefaultValue;
				}
				if(j != param.Length - 1)
					s += ", ";
			}
			s += ")";
            note += s + "\n";
		}

		if(string.IsNullOrEmpty(assemblyName)) {
			WriteAllText(WrapFolder, classname + ".txt", note);
		}
		else {
			WriteAllText(WrapFolder + "/" + assemblyName, classname + ".txt", note);
		}

		UpdateWrapPart(assemblyName, classname, propertys);
	}

	static void WriteAllText(string folder, string name, string content){
//		folder = folder.Replace("/","\\");
		if(!Directory.Exists(folder))
			Directory.CreateDirectory(folder);
		File.WriteAllText(folder + "/" + name, content, System.Text.Encoding.UTF8);
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
		if(string.IsNullOrEmpty(assemblyName))
			File.Delete(WrapFolder + "/" + classname + ".cs");
		else
			File.Delete(WrapFolder + "/" + assemblyName + "/" + classname + ".cs");
	}

	void UpdateWrapPart(string assemblyName, string classname, List<Property> propertys){
		List<string> newc = new List<string>();
		List<string> scall = new List<string>();
		List<string> mcall = new List<string>();

		string classWrapName = assemblyName + classname;                                      //类似UnityEngineVector3，不带点
		string classFullName = string.IsNullOrEmpty(assemblyName) ? classname : assemblyName + "." + classname;
		string _wrapPartTemplate = (Resources.Load("WrapPartTemplate") as TextAsset).text;

		string wrapSVGet = "";
		string wrapSVSet = "";
		string wrapMVGet = "";
		string wrapMVSet = "";

		for(int i = 0; i < propertys.Count; i++){
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

				}
			}
		}

		if(!string.IsNullOrEmpty(wrapSVGet)){
			wrapSVGet = "\t\t\tswitch(memberName) {\n" + wrapSVGet + "\t\t\t}";
		}
		if(!string.IsNullOrEmpty(wrapMVGet)){
			wrapMVGet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
				+"\t\t\tswitch(memberName) {\n" + wrapMVGet + "\t\t\t}";
		}

		string wrapNew = "";
		string wrapSCall = "";
		string wrapMCall = "";

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

		if(string.IsNullOrEmpty(assemblyName)) {
			WriteAllText(WrapFolder, classname + ".cs", text);
		}
		else {
			WriteAllText(WrapFolder + "/" + assemblyName, classname + ".cs", text);
		}
	}

	void UpdateWrapCore(){
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

	void UpdateClass(string assemblyName, string classname){
		OnlyRemoveClass(assemblyName, classname);
		OnlyAddClass(assemblyName, classname);
        UpdateWrapCore(); // 有可能WrapCore被改坏了，还是更新一下
		AssetDatabase.Refresh();
	}

    void ClearAll () {
        m_classes.Clear();
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

        GUILayout.BeginHorizontal();
		if(GUILayout.Button("Reload")){
			Reload();
		}
        if(GUILayout.Button("UpdateAll")) {
         //   Reload();
        }
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("Clear", GUILayout.Width(60))) {
            ClearAll();
        }
        GUI.backgroundColor = Color.white;
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
