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
	List<string> mDelNames = new List<string>();
	bool m_loaded = false;
	string m_newClass = "";

	public static Type GetType( string TypeName )
	{
		// Try Type.GetType() first. This will work with types defined
		// by the Mono runtime, in the same assembly as the caller, etc.
		var type = Type.GetType( TypeName );

		// If it worked, then we're done here
		if( type != null )
			return type;

		// If the TypeName is a full name, then we can try loading the defining assembly directly
		if( TypeName.Contains( "." ) )
		{

			// Get the name of the assembly (Assumption is that we are using 
			// fully-qualified type names)
			string assemblyName = TypeName.Substring( 0, TypeName.IndexOf( '.' ) );

			if(assemblyName == "CQuark"){
				Debug.LogError("不允许对CQuark做Wrap");
				return null;
			}

			// Attempt to load the indicated Assembly
			Assembly assembly = Assembly.Load( assemblyName );
			if( assembly == null )
				return null;

			// Ask that assembly to return the proper Type
			type = assembly.GetType( TypeName );
			if( type != null )
				return type;

		}

		// If we still haven't found the proper type, we can enumerate all of the 
		// loaded assemblies and see if any of them define the type
		var currentAssembly = Assembly.GetExecutingAssembly();
		var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
		foreach( var assemblyName in referencedAssemblies )
		{

			// Load the referenced assembly
			var assembly = Assembly.Load( assemblyName );
			if( assembly != null )
			{
				// See if that assembly defines the named type
				type = assembly.GetType( TypeName );
				if( type != null )
					return type;
			}
		}

		// The type just couldn't be found...
		return null;
	}

	void Refresh(){

	}

	void UpdateClass(string classname){
		mDelNames.Remove(classname);
		Add(classname);
	}

	void Add(string classname){
		Type type = GetType(classname);
		
		if(type == null){
			Debug.LogError("No Such Type : " + classname);
			return;
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
		mDelNames.Add(classname);
		//Add完毕RefreshDataBase，会编译代码
		AssetDatabase.Refresh();
		Refresh();
	}

	void Remove(string classname){
		mDelNames.Remove(classname);
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
		for(int i = 0; i < mDelNames.Count; i++){
			GUILayout.BeginHorizontal();
			GUILayout.Label(mDelNames[i]);
			GUI.backgroundColor = Color.green;
			if(GUILayout.Button("Update", GUILayout.Width(60))){
				UpdateClass(mDelNames[i]);
			}
			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("X", GUILayout.Width(30))){
				Remove(mDelNames[i]);
			}
			GUI.backgroundColor = Color.white;
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		m_newClass = GUILayout.TextField(m_newClass, GUILayout.MinWidth(100));
		GUI.enabled = !string.IsNullOrEmpty(m_newClass);
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add/Update", GUILayout.Width(100))){
			if(mDelNames.Contains(m_newClass)){
				UpdateClass(m_newClass);
			}else{
				Add(m_newClass);
			}
			m_newClass = "";
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
	}
}
