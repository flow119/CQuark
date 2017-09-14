using UnityEngine;
using System.Collections;
using System;

public class Demo2 : MonoBehaviour {

	// Use this for initialization
	void Start () {

//		Script.Init ();

		Type tt = typeof(UnityEngine.GameObject);
		Type t = Type.GetType ("UnityEngine.GameObject");

		//将函数Today()注册给脚本使用
		Script.env.RegFunction (new CSLE.RegHelper_Function ((deleToday)Today));
	
		//让脚本能使用UnityEngine.Debug
		Script.env.RegType (new CSLE.RegHelper_Type (typeof(UnityEngine.Debug)));
	
		ExecuteFile ();
	}

	delegate int deleToday();
	int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}

	// Update is called once per frame
	void ExecuteFile () {
		string script = (Resources.Load ("script02") as TextAsset).text;

		Script.ClearValue ();
		Script.SetValue ("Monday", 1);
		Script.SetValue ("Sunday", 0);
		Script.SetValue ("HP1", 200);
		Script.SetValue ("HP2", 300);
		object obj = Script.Execute (script);
		Debug.Log ("result = " + obj);
	}
}
