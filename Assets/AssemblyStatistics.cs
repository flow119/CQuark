using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class AssemblyStatistics  {

	// Use this for initialization
	[MenuItem("Assets/AssemblyStatistics")]
	public static void Start () {
		System.Reflection.Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
		for(int i = 0; i < assembly.Length; i++){
			Debug.Log(assembly[i].GetName());

			Type[] types = assembly[i].GetTypes();
			for(int j = 0; j < types.Length; j++){
				Debug.Log(types[j].FullName);
			}
		}
	}

}
