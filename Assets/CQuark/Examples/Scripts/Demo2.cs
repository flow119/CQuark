using UnityEngine;
using System.Collections;
using System;

public class Demo2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CQuark.AppDomain.Reset();
        CQuark.AppDomain.RegisterType(typeof(Debug), "Debug");
		Execute1 ();

		//将函数Today()注册给脚本使用
        CQuark.AppDomain.RegisterType(typeof(Demo2), "Demo2");
		Execute2 ();
	}

	public static int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}
	
	//这个函数展示了执行一个函数块，且函数块再调用Unity的Debug类
	void Execute1(){
		CQuarkBlock block = new CQuarkBlock();
		block.Execute (
			"int a = 2;\n" +
			"if(a == 0)\n" +
				"Debug.Log(\"a is zero!\");\n" +
			"else\n" +
				"Debug.Log(\"a is not zero!\");");
	}

	//这个函数展示了执行一个函数块，且函数块再调用一个方法
	void Execute2(){
		CQuarkBlock block = new CQuarkBlock();
		int ret = (int)block.Execute (
			"Debug.Log(\"Today is \" + Demo2.Today());\n" +
            "return Demo2.Today();");
		Debug.Log("return = " + ret);
	}
}
