using System; 
using System.Reflection; 
//using System.Globalization; 
using Microsoft.CSharp;
//using System.CodeDom; 
using System.CodeDom.Compiler;
//using System.Text; 
using UnityEngine;

public class CSCom : MonoBehaviour {

	public string strSourceCode;
	public string className;
	object objClass;

	void Compile(){
		// 1.Create a new CSharpCodePrivoder instance
		
		CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
		
		
		
		// 2.Sets the runtime compiling parameters by crating a new CompilerParameters instance
		
		CompilerParameters objCompilerParameters = new CompilerParameters();
		
		objCompilerParameters.ReferencedAssemblies.Add("System.dll");

		objCompilerParameters.ReferencedAssemblies.Add("D:/Unity472/Editor/Data/Managed/" + "UnityEngine.dll");
		objCompilerParameters.GenerateInMemory = true;
		
		
		
		// 3.CompilerResults: Complile the code snippet by calling a method from the provider
		
		CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, strSourceCode);
		
		
		
		if (cr.Errors.HasErrors)
			
		{
			
			string strErrorMsg = cr.Errors.Count.ToString() + " Errors:";
			
			Debug.LogError(strErrorMsg);
			
			for (int x = 0; x < cr.Errors.Count; x++)
				
			{
				
				strErrorMsg = strErrorMsg + "\r\nLine: " +
					
					cr.Errors[x].Line.ToString() + " - " +
						
						cr.Errors[x].ErrorText;
				Debug.LogError(strErrorMsg);
				
			}

			return;
			
		}

		// 4. Invoke the method by using Reflection
		
		Assembly objAssembly = cr.CompiledAssembly;
		
		objClass = objAssembly.CreateInstance(className);
	}

	void Awake(){
		Compile();
	}

	// Use this for initialization
	void Start () {
		
		objClass.GetType().InvokeMember("Start", BindingFlags.InvokeMethod, null, objClass, null);

	}
	
	// Update is called once per frame
//	void Update () {
//	
//		objClass.GetType().InvokeMember("Update", BindingFlags.InvokeMethod, null, objClass, null);
//
//	}
}
