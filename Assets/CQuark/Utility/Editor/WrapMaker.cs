using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class WrapMaker : EditorWindow{

	protected class Property{
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
	
	protected class Method {
		/// <summary>
		/// "New, SCall, MCall, Op..."
		/// </summary>
		public string m_methodType;
		public string m_returnType;
		public string m_methodName;
		public string[] m_inType;
		public bool m_isGeneric = false; //是否是<T>
		public Method (string methodType, Type returnType, string methodName, System.Reflection.ParameterInfo[] param, int paramLength, bool isGeneric = false) {
			m_methodType = methodType;
			if(returnType != null)
				m_returnType = Type2String(returnType); ;
			m_methodName = methodName;
			m_inType = new string[paramLength];

			for(int i = 0; i < paramLength; i++) {
				m_inType[i] = Type2String(param[i].ParameterType);
			}
			m_isGeneric = isGeneric;
		}
	}

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
						
						nameSpace = "";
						type = assembly.GetType( assemblyName.ToString() + "." + TypeName );
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

	protected static string Type2String(Type type){
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

	protected List<Property> GetPropertys(Type type, ref string log){
		List<Property> savePropertys = new List<Property>();
		log += "\n字段\n";
		FieldInfo[] fis = type.GetFields();
		for(int i= 0; i < fis.Length; i++){
			log += "public ";
			string attributes = fis[i].Attributes.ToString();
			bool isStatic = attributes.Contains("Static");
			if(isStatic)
				log += "static ";
			bool isReadonly = attributes.Contains("Literal") && attributes.Contains("HasDefault") ; //const
			isReadonly = isReadonly || attributes.Contains("InitOnly");                             //readonly
			if(isReadonly)
				log += "readonly ";
			log += Type2String(fis[i].FieldType) + " " + fis[i].Name + ";\n";
			savePropertys.Add(new Property(fis[i].FieldType, isStatic, true, !isReadonly, fis[i].Name));
		}
		
		log += "\n静态属性\n";
		PropertyInfo[] spis = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
		for(int i= 0; i < spis.Length; i++){
			log += "public static ";
//			string attributes = spis[i].Attributes.ToString();
			log += Type2String(spis[i].PropertyType) + " " + spis[i].Name + "{";
			if(spis[i].CanRead) 
				log += "get;";
			if(spis[i].CanWrite)
				log += "set;";
			log += "}\n";

			savePropertys.Add(new Property(spis[i].PropertyType, true, spis[i].CanRead, spis[i].CanWrite, spis[i].Name));
		}

		log += "\n实例属性\n";
		PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		//检查是否有Index
		MethodInfo[] mis = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
		bool hasIndex = false;
		for(int j = 0; j < mis.Length; j++){
			if(mis[j].Name == "get_Item" || mis[j].Name == "set_Item"){
				hasIndex = true;
				break;
			}
		}
		
		for(int i = 0; i < pis.Length; i++){
			if(pis[i].Name == "Item" && hasIndex){
				continue;//如果有get_Item或者set_Item，表示是[Index]，否则表示一个属性
			}

			log += "public ";
//			string attributes = pis[i].Attributes.ToString();
			log += Type2String(pis[i].PropertyType) + " " + pis[i].Name + "{";
			if(pis[i].CanRead) 
				log += "get;";
			if(pis[i].CanWrite)
				log += "set;";
			log += "}\n";

				
			savePropertys.Add(new Property(pis[i].PropertyType, false, pis[i].CanRead, pis[i].CanWrite, pis[i].Name));
		}

		return savePropertys;
	}
	protected string[] Propertys2PartStr(string classFullName, List<Property> propertys){
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
                    wrapSVGet += "\t\t\t\treturnValue.m_type = typeof(" + propertys[i].m_type + ");\n";
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
                    wrapMVGet += "\t\t\t\treturnValue.m_type = typeof(" + propertys[i].m_type + ");\n";
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
		return new string[]{wrapSVGet, wrapSVSet, wrapMVGet, wrapMVSet};
	}

	protected List<Method> GetConstructor(Type type, ref string log){
		List<Method> saveMethods = new List<Method>();
		log += "\n构造函数\n";
		if (typeof(MonoBehaviour).IsAssignableFrom(type)) {
			log += "继承自MonoBehaviour\n";
			return saveMethods;
		}
		System.Reflection.ConstructorInfo[] construct = type.GetConstructors();
		for(int i = 0; i < construct.Length; i++){
			string s = "";
			s += type.ToString() + "(";
			System.Reflection.ParameterInfo[] param = construct[i].GetParameters();
			if(param.Length == 0){
				saveMethods.Add(new Method("New", null, "", param, 0));
			}else{
				if(param[0].IsOptional)
					saveMethods.Add(new Method("New", null, "", param, 0));
				
				for(int j = 0; j < param.Length; j++){
					s += param[j].ParameterType + " " +param[j].Name;
					if(j != param.Length - 1)
						s += ",";
					if(param[j].IsOptional)
						s += " = " + param[j].DefaultValue;
					
					if(j == param.Length - 1 || param[j + 1].IsOptional) {
						saveMethods.Add(new Method("New", null, "", param, j + 1));
					}
				}
			}
			s += ")";
			log += s + "\n";
		}

		return saveMethods;
	}
	protected string Constructor2PartStr(string classFullName, List<Method> constructor){
		string wrapNew = "";
		for(int i = 0; i < constructor.Count; i++) {
			if(constructor[i].m_inType.Length == 0)
				wrapNew += "\t\t\tif(param.Count == 0){\n";
			else{
				string typehash = GetHashCodeByTypes (constructor [i].m_inType);
				wrapNew += "\t\t\tif(param.Count == " + constructor[i].m_inType.Length + " && MatchType(param, " + typehash + ", mustEqual)){\n";
			}
			wrapNew += "\t\t\t\treturnValue = new CQ_Value();\n";
            wrapNew += "\t\t\t\treturnValue.m_type = typeof(" + classFullName + ");\n";
			wrapNew += "\t\t\t\treturnValue.value = new " + classFullName + "(";
			for(int j = 0; j < constructor[i].m_inType.Length; j++) {
				wrapNew += "(" + constructor[i].m_inType[j] + ")param[" + j + "].ConvertTo(typeof(" + constructor[i].m_inType[j] + "))";
				if(j != constructor[i].m_inType.Length - 1)
					wrapNew += ",";
			}
			wrapNew += ");\n";
			wrapNew += "\t\t\t\treturn true;\n\t\t\t}\n";
		}
		return wrapNew;
	}

	protected List<Method> GetStaticMethods(Type type, ref string log, List<Property> ignoreProperty){
		List<Method> saveMethods = new List<Method> ();
		//静态函数，成员函数
		log += "\n静态方法\n";

		//TODO 暂时不包含op，不包含index，不包含ref，in, out, 不包含IEnumrator
		System.Reflection.MethodInfo[] smis = type.GetMethods (BindingFlags.Public | BindingFlags.Static);//这里没有获取私有方法，因为即使获取了西瓜也没有办法调用
		//基类的方法一起导出，这样可以自动调用基类
		for (int i = 0; i < smis.Length; i++) {
			//属性上面已经获取过了 这里做个判断，是否包含这个属性，包含的话continue，否则表示有方法名是get_,set_开头
			if (ContainsProperty (ignoreProperty, smis [i].Name))
				continue;

			if (IsOp (smis [i]))
				continue;

			string retType = Type2String (smis [i].ReturnType);

			string s = "public static " + retType + " " ;
			s += smis [i].Name ;
			bool isGeneric = (smis[i].ContainsGenericParameters && smis[i].IsGenericMethod);
			if(isGeneric)
				s += "<T>";
            s += "(";
			System.Reflection.ParameterInfo[] param = smis [i].GetParameters ();
			if (param.Length == 0) {
				saveMethods.Add (new Method ("SCall", smis [i].ReturnType, smis [i].Name, param, 0, isGeneric));
			} else {
				if (param [0].IsOptional)
					saveMethods.Add (new Method ("SCall", smis [i].ReturnType, smis [i].Name, param, 0, isGeneric));
				for (int j = 0; j < param.Length; j++) {
					string paramType = Type2String (param [j].ParameterType);
					bool finish = true;
					if (paramType.EndsWith ("&")) {//ref，out
						s += "ref " + paramType.Substring (0, paramType.Length - 1) + " " + param [j].Name;
						finish = false;
					} else {
						s += paramType + " " + param [j].Name;
						if (param [j].IsOptional)
							s += " = " + param [j].DefaultValue;
					}
					if (j != param.Length - 1)
						s += ", ";
					
					if (finish) {
						if (j == param.Length - 1 || param [j + 1].IsOptional) {
							saveMethods.Add (new Method ("SCall", smis [i].ReturnType, smis [i].Name, param, j + 1, isGeneric));
						}
					}
				}
			}
			s += ")";
			log += s + "\n";
		}
		return saveMethods;
	}
	protected string SCall2PartStr(string classFullName, List<Method> staticMethods){
		string wrapSCall = "";
		for(int i = 0; i < staticMethods.Count; i++) {
			if(!Finish(staticMethods[i].m_returnType) || !Finish(staticMethods[i].m_inType))
				continue;

			if(staticMethods[i].m_inType.Length == 0)
				wrapSCall += "\t\t\tif(paramCount == 0 && functionName == \"" + staticMethods[i].m_methodName + "\"){\n";
			else {
				string typehash = GetHashCodeByTypes (staticMethods [i].m_inType);
				wrapSCall += "\t\t\tif(paramCount == " + staticMethods[i].m_inType.Length + " && functionName == \"" + staticMethods[i].m_methodName + "\" && MatchType(param, " + typehash + ", mustEqual)){\n";
			}
			if(staticMethods[i].m_returnType == "void"){
				wrapSCall += "\t\t\t\treturnValue = CQ_Value.Null;\n";
			}else{
				wrapSCall += "\t\t\t\treturnValue = new CQ_Value();\n";
                wrapSCall += "\t\t\t\treturnValue.m_type = typeof(" + staticMethods[i].m_returnType + ");\n";
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
		if (!string.IsNullOrEmpty (wrapSCall))
			wrapSCall = "\t\t\tint paramCount = param.Count;\n" + wrapSCall;
		return wrapSCall;
	}

	protected List<Method> GetInstanceMethods(Type type, ref string log, List<Property> ignoreProperty){
		List<Method> saveMethods = new List<Method> ();
		log += "\n成员方法\n";

		if (type.GetConstructors ().Length == 0 && !typeof(Component).IsAssignableFrom(type)) {	//是否是静态类，静态类没有构造函数
			log += "静态类没有成员方法";
			return saveMethods;			//静态类依然会反射出成员方法（比如ToString,GetType），但没法调用，我们不保存
		}

		System.Reflection.MethodInfo[] imis = type.GetMethods (BindingFlags.Public | BindingFlags.Instance);//这里没有获取私有方法，因为即使获取了西瓜也没有办法调用
		//基类的方法一起导出，这样可以自动调用基类

		PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		bool hasIndex = false;
		for (int i = 0; i < pis.Length; i++) {
			if(pis[i].Name == "Item"){
				hasIndex = true;
				break;
			}
		}
		for (int i = 0; i < imis.Length; i++) {
			//属性上面已经获取过了 这里做个判断，是否包含这个属性，包含的话continue，否则表示有方法名是get_,set_开头
			if(ContainsProperty(ignoreProperty, imis[i].Name))
				continue;

			if(imis[i].Name == "get_Item" || imis[i].Name == "set_Item"){
				if(hasIndex)
					continue;
			}

			string retType = Type2String (imis [i].ReturnType);

			string s = "public " + retType + " ";
			s += imis [i].Name ;
			bool isGeneric = (imis[i].ContainsGenericParameters && imis[i].IsGenericMethod);
			if(isGeneric)
				s += "<T>";

			s += "(";
			System.Reflection.ParameterInfo[] param = imis [i].GetParameters ();
			if (param.Length == 0) {
				saveMethods.Add (new Method ("MCall", imis [i].ReturnType, imis [i].Name, param, 0, isGeneric));
			} else {
				if (param [0].IsOptional)
					saveMethods.Add (new Method ("MCall", imis [i].ReturnType, imis [i].Name, param, 0, isGeneric));
				for (int j = 0; j < param.Length; j++) {
					string paramType = Type2String (param [j].ParameterType);
					bool finish = true;
					if (paramType.EndsWith ("&")) {//ref，out
						s += "ref " + paramType.Substring (0, paramType.Length - 1) + " " + param [j].Name;
						finish = false;
					} else {
						s += paramType + " " + param [j].Name;
						if (param [j].IsOptional)
							s += " = " + param [j].DefaultValue;
					}
					if (j != param.Length - 1)
						s += ", ";
					
					if (finish) {
						if (j == param.Length - 1 || param [j + 1].IsOptional) {
							saveMethods.Add (new Method ("MCall", imis [i].ReturnType, imis [i].Name, param, j + 1, isGeneric));
						}
					}
				}
			}
			s += ")";
			log += s + "\n";
		}
		return saveMethods;
	}
	protected string MCall2PartStr(string classFullName, List<Method> instanceMethods){
		string wrapMCall = "";
		for(int i = 0; i < instanceMethods.Count; i++) {
			if(!Finish(instanceMethods[i].m_returnType) || !Finish(instanceMethods[i].m_inType))
				continue;

			if(instanceMethods[i].m_inType.Length == 0)
				wrapMCall += "\t\t\tif(paramCount == 0 && functionName == \"" + instanceMethods[i].m_methodName + "\"){\n";
			else {
				string typehash = GetHashCodeByTypes (instanceMethods [i].m_inType);
				wrapMCall += "\t\t\tif(paramCount == " + instanceMethods[i].m_inType.Length + " && functionName == \"" + instanceMethods[i].m_methodName + "\" && MatchType(param, " + typehash + ", mustEqual)){\n";
			}
			if(instanceMethods[i].m_returnType == "void"){
				wrapMCall += "\t\t\t\treturnValue = CQ_Value.Null;\n\t\t\t\t";
			}else{
				wrapMCall += "\t\t\t\treturnValue = new CQ_Value();\n";
                wrapMCall += "\t\t\t\treturnValue.m_type = typeof(" + instanceMethods[i].m_returnType + ");\n";
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
				+ "\t\t\tint paramCount = param.Count;\n"
				+ wrapMCall;
		}
		return wrapMCall;
	}

	protected void CallTypes2TypeStr(List<Method> methods, List<string> typeDic){
		for (int i = 0; i < methods.Count; i++) {
			if (methods [i].m_inType.Length == 0)
				continue;

			if(!Finish(methods[i].m_inType))
				continue;

			string hashCode = GetHashCodeByTypes(methods [i].m_inType);
			string text = "static Type[] " + hashCode + " = new Type[]{";
			for (int j = 0; j < methods [i].m_inType.Length; j++) {
				text += "typeof(" + methods [i].m_inType [j] + ")";
				if (j != methods [i].m_inType.Length - 1)
					text += ", ";
			}
			text += "};";
			if (!typeDic.Contains (text)) {
				typeDic.Add (text);
			}
		}
	}

	static string GetHashCodeByTypes(string[] types){
		string hashkey = "";
		foreach(string s in types) {
			hashkey += s;
		}
		//返回类似 t75CC8236 这样的值
		return "t" + hashkey.GetHashCode ().ToString("x8");
	}

	protected List<Method> GetIndex(Type type, ref string log){
		if (type.GetConstructors ().Length == 0)	//是否是静态类，静态类没有构造函数
			return new List<Method> ();			//静态类依然会反射出成员方法（比如ToString,GetType），但没法调用，我们不保存
		
		List<Method> saveMethods = new List<Method> ();
		log += "\n索引\n";

		PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		bool hasIndex = false;
		for (int i = 0; i < pis.Length; i++) {
			if(pis[i].Name == "Item"){
				hasIndex = true;
				break;
			}
		}
		if (!hasIndex)
			return new List<Method> ();

		//检查是否有Index
		MethodInfo[] mis = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
		for (int i = 0; i < mis.Length; i++) {
			if(mis[i].Name == "get_Item"){
				ParameterInfo[] pais = mis[i].GetParameters();
				Method method = new Method("IndexGet", mis[i].ReturnType, mis[i].Name, pais, pais.Length);
				saveMethods.Add(method);
				log += "IndexGet " + mis[i].ReturnType + " [";
				for(int j = 0; j < pais.Length; j++){
					log += pais[j].ParameterType + " " + pais[j].Name;
					if(j != pais.Length - 1)
						log += ",";
				}
				log += "]\n";
			}else if(mis[i].Name == "set_Item"){
				ParameterInfo[] pais = mis[i].GetParameters();
				Method method = new Method("IndexSet", mis[i].ReturnType, mis[i].Name, mis[i].GetParameters(), mis[i].GetParameters().Length);
				saveMethods.Add(method);
				log += "IndexSet [";
				for(int j = 0; j < pais.Length - 1; j++){
					log += pais[j].ParameterType + " " + pais[j].Name;
					if(j != pais.Length - 2)
						log += ",";
				}
				log += "] = ";
				log += pais[pais.Length - 1].ParameterType + " " + pais[pais.Length - 1].Name + "\n";
			}
		}
		return saveMethods;
	}
	protected string[] Index2PartStr(string classFullName, List<Method> indexMethods){
		string wrapIGet = "";
		string wrapISet = "";
		for(int i = 0; i < indexMethods.Count; i++) {
			if(!Finish(indexMethods[i].m_returnType) || !Finish(indexMethods[i].m_inType))
				continue;
			//TODO 可能有多位数组this[x,y]
			if(indexMethods[i].m_methodType == "IndexGet"){
				wrapIGet += "\t\t\tif(key.EqualOrImplicateType(typeof(" + indexMethods[i].m_inType[0]+ "))){\n";
				wrapIGet += "\t\t\t\treturnValue = new CQ_Value();\n";
                wrapIGet += "\t\t\t\treturnValue.m_type = typeof(" + indexMethods[i].m_returnType + ");\n";
				wrapIGet += "\t\t\t\treturnValue.value = ";
				wrapIGet += "obj[(" + indexMethods[i].m_inType[0] + ")key.ConvertTo(typeof(" + indexMethods[i].m_inType[0] + "))];\n";
				wrapIGet += "\t\t\t\treturn true;\n\t\t\t}";
			}
			else if(indexMethods[i].m_methodType == "IndexSet"){
				wrapISet += "\t\t\tif(param.EqualOrImplicateType(typeof(" + indexMethods[i].m_inType[0]+ "))){\n";
				wrapISet += "\t\t\t\tobj[(" + indexMethods[i].m_inType[0] + ")key.ConvertTo(typeof(" + indexMethods[i].m_inType[0] + "))] = ";
				wrapISet += "(" + indexMethods[i].m_inType[indexMethods[i].m_inType.Length - 1] + ")param.ConvertTo(typeof(" + indexMethods[i].m_inType[indexMethods[i].m_inType.Length - 1] + "));\n";
				wrapISet += "\t\t\t\treturn true;\n\t\t\t}";
			}
		}
		if(!string.IsNullOrEmpty(wrapIGet)) {
			wrapIGet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
				+ wrapIGet;
		}
		if(!string.IsNullOrEmpty(wrapISet)) {
			wrapISet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
				+ wrapISet;
		}
		return new string[]{wrapIGet, wrapISet};
	}

	protected List<Method> GetOp(Type type, ref string log){
		List<Method> saveMethods = new List<Method> ();

		log += "\n运算符\n";

		System.Reflection.MethodInfo[] smis = type.GetMethods (BindingFlags.Public | BindingFlags.Static);//和获取静态方法一样
		for (int i = 0; i < smis.Length; i++) {
			if (!IsOp (smis [i]))
				continue;

			System.Reflection.ParameterInfo[] param = smis [i].GetParameters ();
            log += "public static " + smis[i].ReturnType + " " + smis[i].Name + "(";
            for(int j = 0; j < param.Length; j++) {
                log += param[j].ParameterType + " " + param[j].Name;
                if(j != param.Length - 1)
                    log += ",";
            }
            log += ")\n";

            Method method = new Method(smis[i].Name, smis[i].ReturnType, smis[i].Name, param, param.Length);
			saveMethods.Add(method);
		}
		return saveMethods;
	}
    protected string[] Op2PartStr (List<Method> opMethods) {
        string wrapAdd = "";
        string wrapSub = "";
        string wrapMul = "";
        string wrapDiv = "";
        string wrapMod = "";

        string wrapGreater = "";
        string wrapLess = "";
        string wrapEGreater = "";
        string wrapELess = "";
        string wrapEqual = "";
        string wrapInequal = "";

        string wrapImplicit = "";
        string wrapExplicit = "";
        string wrapNegation = "";

        for(int i = 0; i < opMethods.Count; i++) {
            if(opMethods[i].m_methodName == "op_Addition") {
                wrapAdd += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.m_type = typeof(" + opMethods[i].m_returnType + ");\n"
                + "\t\t\t\treturnValue.value = (" + opMethods[i].m_inType[0] + ")left.ConvertTo(typeof(" + opMethods[i].m_inType[0] + ")) + (" + opMethods[i].m_inType[1] + ")right.ConvertTo(typeof(" + opMethods[i].m_inType[1] + "));\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Subtraction") {
                wrapSub += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.m_type = typeof(" + opMethods[i].m_returnType + ");\n"
                + "\t\t\t\treturnValue.value = (" + opMethods[i].m_inType[0] + ")left.ConvertTo(typeof(" + opMethods[i].m_inType[0] + ")) - (" + opMethods[i].m_inType[1] + ")right.ConvertTo(typeof(" + opMethods[i].m_inType[1] + "));\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Multiply") {
                wrapMul += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.m_type = typeof(" + opMethods[i].m_returnType + ");\n"
                + "\t\t\t\treturnValue.value = (" + opMethods[i].m_inType[0] + ")left.ConvertTo(typeof(" + opMethods[i].m_inType[0] + ")) * (" + opMethods[i].m_inType[1] + ")right.ConvertTo(typeof(" + opMethods[i].m_inType[1] + "));\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Division") {
                wrapDiv += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.m_type = typeof(" + opMethods[i].m_returnType + ");\n"
                + "\t\t\t\treturnValue.value = (" + opMethods[i].m_inType[0] + ")left.ConvertTo(typeof(" + opMethods[i].m_inType[0] + ")) / (" + opMethods[i].m_inType[1] + ")right.ConvertTo(typeof(" + opMethods[i].m_inType[1] + "));\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Modulus") {
                wrapMod += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.m_type = typeof(" + opMethods[i].m_returnType + ");\n"
                + "\t\t\t\treturnValue.value = (" + opMethods[i].m_inType[0] + ")left.ConvertTo(typeof(" + opMethods[i].m_inType[0] + ")) % (" + opMethods[i].m_inType[1] + ")right.ConvertTo(typeof(" + opMethods[i].m_inType[1] + "));\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
        }

        return new string[]{
            wrapAdd,
            wrapSub,
            wrapMul,
            wrapDiv,
            wrapMod,

            wrapGreater, 
            wrapLess,
            wrapEGreater,
            wrapELess,
            wrapEqual, 
            wrapInequal, 

            wrapImplicit,
            wrapExplicit,
            wrapNegation,
        };
    }
	protected static bool ContainsProperty (List<Property> propertys, string methodName) {
		if (methodName.StartsWith ("get_")) {
			string propertyName = methodName.Substring (4);
			for (int i = 0; i < propertys.Count; i++) {
				if (propertys [i].m_canGet && propertys [i].m_name == propertyName)
					return true;
			}
		} 
		else if (methodName.StartsWith ("set_")) {
			string propertyName = methodName.Substring (4);
			for (int i = 0; i < propertys.Count; i++) {
				if (propertys [i].m_canSet && propertys [i].m_name == propertyName)
					return true;
			}
		}
		return false;
	}

	protected static bool IsOp(MethodInfo method){
        //if (method.GetParameters ().Length != 2)
        //    return false;
		string methodName = method.Name;
        int paramLength = method.GetParameters().Length;
        if(paramLength == 2) {
             return 
                 methodName == "op_Addition"            //a+b
            || methodName == "op_Subtraction"           //a-b
            || methodName == "op_Multiply"              //a*b
            || methodName == "op_Division"              //a/b
            || methodName == "op_Modulus"               //a%b

            || methodName == "op_GreaterThan"           //a>b
            || methodName == "op_LessThan"              //a<b
            || methodName == "op_GreaterThanOrEqual"    //a>=b
            || methodName == "op_LessThanOrEqual"       //a<=b

            || methodName == "op_Equality"              //a==b
            || methodName == "op_Inequality";           //a!=b
        }
        else if(paramLength == 1){
            return
                methodName == "op_Implicit"            //a 隐式转换成T
            || methodName == "op_Explicit"             //(T)a

            || methodName == "op_UnaryNegation" ;      //-a
        }
        return false;            
	}

	protected static bool Finish(string type){
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
	protected static bool Finish (string[] types) {
		for(int j = 0; j < types.Length; j++) {
			if(!Finish(types[j]))
				return false;
		}
		return true;
	}
}
