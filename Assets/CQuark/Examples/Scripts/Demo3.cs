using UnityEngine;
using System.Collections;
using System;

public class Demo3 : MonoBehaviour {

	public string m_blockFilePath;

	// Use this for initialization
	void Start () {

//		Script.Init ();

//		Type tt = typeof(UnityEngine.GameObject);
//		Type t = Type.GetType ("UnityEngine.GameObject");

		//将函数Today()注册给脚本使用
		CQuark.AppDomain.RegFunction ((deleToday)Today);
	
		ExecuteFile ();
	}

	delegate int deleToday();
	int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}


	// 这个函数展示了如何执行一个文件（作为函数块）
	void ExecuteFile () {

		CQuarkClass.Instance.ClearValue ();
		CQuarkClass.Instance.SetValue ("Monday", 1);
		CQuarkClass.Instance.SetValue ("Sunday", 0);
		CQuarkClass.Instance.SetValue ("HP1", 200);
		CQuarkClass.Instance.SetValue ("HP2", 300);

		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		object obj = CQuarkClass.Instance.Execute (text);
		Debug.Log ("result = " + obj);
	}
}
