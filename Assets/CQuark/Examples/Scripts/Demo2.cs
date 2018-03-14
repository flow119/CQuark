using UnityEngine;
using System.Collections;
using System;

public class Demo2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Script.Init ();

		Execute1 ();

		//将函数Today()注册给脚本使用
		CQuark.AppDomain.RegFunction ((deleToday)Today);
		Execute2 ();
	}

	delegate int deleToday();
	int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}
	
	//这个函数展示了执行一个函数块，且函数块再调用Unity的Debug类
	void Execute1(){
		CQuarkClass.Instance.Execute (
			"int a = 2;\n" +
			"if(a == 0)\n" +
				"Debug.Log(\"a is zero!\");\n" +
			"else\n" +
				"Debug.Log(\"a is not zero!\");");
	}

	//这个函数展示了执行一个函数块，且函数块再调用一个方法
	void Execute2(){
		int ret = (int)CQuarkClass.Instance.Execute (
			"Debug.Log(\"Today is \" + Today());\n" +
			"return Today();");
		Debug.Log("return = " + ret);
	}
}
