using UnityEngine;
using System.Collections;
using System;

public class Demo2 : MonoBehaviour {

	public string m_blockFilePath;

	// Use this for initialization
	void Start () {

//		Script.Init ();

//		Type tt = typeof(UnityEngine.GameObject);
//		Type t = Type.GetType ("UnityEngine.GameObject");

		//将函数Today()注册给脚本使用
		Script.Instance.env.RegFunction (new CSLE.RegHelper_Function ((deleToday)Today));
	

	
		ExecuteFile ();
	}

	delegate int deleToday();
	int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}

	// 这个函数展示了如何执行一个文件（作为函数块）
	void ExecuteFile () {

		Script.Instance.ClearValue ();
		Script.Instance.SetValue ("Monday", 1);
		Script.Instance.SetValue ("Sunday", 0);
		Script.Instance.SetValue ("HP1", 200);
		Script.Instance.SetValue ("HP2", 300);

		Action<WWW> cb = delegate(WWW www) {
			object obj = Script.Instance.Execute (www.text);
			Debug.Log ("result = " + obj);
		};
		LoadMgr.Instance.LoadFromStreaming (m_blockFilePath, cb);
	}
	
	//这个函数展示了如何执行一个文件(类)里某个函数
	void ExecuteFileFunction(){
		//TODO 执行文件里某个函数
	}
}
