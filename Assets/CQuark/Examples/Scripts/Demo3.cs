using UnityEngine;
using System.Collections;
using System;

public class Demo3 : MonoBehaviour {

	public string m_blockFilePath;

	// Use this for initialization
	void Start () {

		CQuark.AppDomain.Reset();

//		Type tt = typeof(UnityEngine.GameObject);
//		Type t = Type.GetType ("UnityEngine.GameObject");
		CQuark.AppDomain.RegisterType<Debug>("Debug");
        CQuark.AppDomain.RegisterType<Demo3>("Demo3");
		//将函数Today()注册给脚本使用
	
		ExecuteFile ();
	}

	public static int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}


	// 这个函数展示了如何执行一个文件（作为函数块）
	void ExecuteFile () {
		CQuarkParagraph block = new CQuarkParagraph();

        block.SetValue("HP1", 200);
        block.SetValue("HP2", 300);

		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		object obj = block.Execute (text);
		Debug.Log ("result = " + obj);
	}
}
