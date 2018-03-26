using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class WrapMaker : EditorWindow {

	[MenuItem("CQuark/Wrap Maker", false, 9)]
	[MenuItem("Assets/CQuark/Wrap Maker", false, 0)]
	static public void OpenAtlasMaker ()
	{
		EditorWindow.GetWindow<WrapMaker>(false, "Wrap Maker", true).Show();
	}

	Vector2 mScroll = Vector2.zero;
	Dictionary<string, List<string>> mDelNames = new Dictionary<string, List<string>>();
	bool m_loaded = false;
	string m_newClass = "";

	public static Type GetType( string TypeName, ref string nameSpace)
	{
		Type type = null;
		if(string.IsNullOrEmpty(nameSpace)){
			type = Type.GetType( TypeName );
			if(type != null){
				return type;
			}else{
				AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
				foreach( var assemblyName in referencedAssemblies )
				{
					// Load the referenced assembly
					var assembly = Assembly.Load( assemblyName );
					if( assembly != null )
					{
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

	void Refresh(){

	}

	void AddClass(string assemblyName, string classname){
		Type type = GetType(classname, ref assemblyName);
		
		if(type == null){
			Debug.LogError("No Such Type : " + classname);
			return;
		}

		if(!mDelNames.ContainsKey(assemblyName)){
			mDelNames.Add(assemblyName, new List<string>());
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
		text += "方法\n";
		System.Reflection.MethodInfo[] methods = type.GetMethods();//这里没有获取私有方法，因为即使获取了西瓜也没有办法调用
		//基类的方法一起导出，这样可以自动调基类
		for(int i = 0; i < methods.Length; i++){
			string s = "";
			s += methods[i].IsPublic ? "public " : "private ";
			s += methods[i].IsStatic ? "static " : "";
			string retType = methods[i].ReturnType.ToString();
			retType = retType.Replace('+','.');//A+Enum实际上是A.Enum
			retType = retType.Replace("System.Void", "void");
			s += retType + " ";

			//如果方法名是get_开头，那么就是get成员（如果本来就命名为get_X，反射出的就是get_get_X）
			//如果方法名是set_开头，那么就是set成员（如果本来就命名为set_X，反射出的就是set_set_X）
			s += methods[i].Name + "(";
			System.Reflection.ParameterInfo[] param = methods[i].GetParameters();
			for(int j = 0; j < param.Length; j++){
				string paramType = param[j].ParameterType.ToString();
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

		text += "\n"; 
		text += "变量\n";
		System.Reflection.MemberInfo[] members = type.GetMembers();
		for(int i = 0; i < members.Length; i++){
			string s = "";
			string memberType = members[i].MemberType.ToString();
			s += memberType + " " + members[i].Name;
			text += s + "\n";
		}

		System.IO.File.WriteAllText(Application.dataPath + "/" + type + ".txt", text);
		mDelNames[assemblyName].Add(classname);
		//Add完毕RefreshDataBase，会编译代码
		AssetDatabase.Refresh();
		Refresh();
	}

	void RemoveClass(string assemblyName, string classname){
		mDelNames[assemblyName].Remove(classname);
	}

	void UpdateClass(string assemblyName, string classname){
		RemoveClass(assemblyName, classname);
		AddClass(assemblyName, classname);
	}

	// Use this for initialization
	void OnGUI () {
		if(!m_loaded){
			Refresh();
			m_loaded = true;
		}

		if(GUILayout.Button("Refresh")){
			Refresh();
		}

		GUILayout.Space(10);

		mScroll = GUILayout.BeginScrollView(mScroll);
		GUILayout.BeginVertical();
		foreach(var kvp in mDelNames){
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
		m_newClass = GUILayout.TextField(m_newClass, GUILayout.MinWidth(100));
		GUI.enabled = !string.IsNullOrEmpty(m_newClass);
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add/Update", GUILayout.Width(100))){
			string className = "";
			string assemblyName = "";
			string[] s = m_newClass.Split('.');
			if(s.Length == 1){
				assemblyName = "";
				className = s[0];
			}else if(s.Length == 2){
				assemblyName = s[0];
				className = s[1];
			}

			if(mDelNames.ContainsKey(assemblyName) && mDelNames[assemblyName].Contains(className)){
				UpdateClass(assemblyName, className);
			}else{
				AddClass(assemblyName, className);
			}
			m_newClass = "";
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
	}
}
