using UnityEngine;
using System.Collections;
using System;

public class Demo2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CQuark.AppDomain.Reset();
		InitAppDomain.RegisterFullnameType();

//		CQuark.AppDomain.RegisterType<Debug>("Debug");
//		CQuark.AppDomain.RegisterType<Vector3>("Vector3");
//		Execute1 ();

		//将函数Today()注册给脚本使用
        CQuark.AppDomain.RegisterType<Demo2>("Demo2");
		Execute2 ();
	}

	public static int Today(){
		return (int)DateTime.Now.DayOfWeek;
	}
	
	//这个函数展示了执行一个函数块，且函数块再调用Unity的Debug类
	void Execute1(){
		CQuarkParagraph block = new CQuarkParagraph();
		block.Execute (
			"int a = 2;\n" +
			"if(a == 0)\n" +
			"UnityEngine.Debug.Log(\"a is zero!\");\n" +
			"else\n" +
			"UnityEngine.Debug.Log(\"a is not zero!\");");
	}

	//这个函数展示了执行一个函数块，且函数块再调用一个方法
	void Execute2(){
		CQuarkParagraph block = new CQuarkParagraph();
		CQuark.CQ_Value ret = block.Execute (
			"UnityEngine.Debug.Log(\"V\" + (UnityEngine.Vector3.up + UnityEngine.Vector3.back));\n" +
			"UnityEngine.Debug.Log(UnityEngine.Vector3.up.ToString());\n" +
			//TODO 括号的识别有问题
//			"UnityEngine.Debug.Log((UnityEngine.Vector3.up + UnityEngine.Vector3.back).ToString());\n" +
			"UnityEngine.Debug.Log(\"Today is \" + Demo2.Today());\n" +
            "return Demo2.Today();");
//			"Debug.Log(\"V\" + (Vector3.up + Vector3.back));\n" +
//			"Debug.Log(\"Today is \" + Demo2.Today());\n" +
//			"return Demo2.Today();");
		Debug.Log("return = " + ret);
	}
}
