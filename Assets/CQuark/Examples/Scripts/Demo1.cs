using UnityEngine;
using System.Collections;

public class Demo1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Script.Init ();
		CQuark.AppDomain.Reset();
		Eval1 ();
		Eval2 ();
		ExecuteBlock ();

	}

	//这个函数展示最简单的计算
	void Eval1 () {
		CQuarkBlock block = new CQuarkBlock();
		int ret = (int)block.Execute ("1+2");
		Debug.Log ("return = " + ret);
	}

	//这个函数展示了先从外部向env赋值，再做计算
	void Eval2(){
		CQuarkBlock block = new CQuarkBlock();
		block.SetValue ("HP1", 200);
		block.SetValue ("HP2", 300);
		double d = (double)block.Execute ("HP1 + HP2 * 0.5");
		Debug.Log ("d = " + d);
		float f = (float)block.Execute ("HP1 + HP2 * 0.5f");
		Debug.Log ("f = " + f);
	}

	//这个函数展示了执行一个函数块
	void ExecuteBlock(){
		string method =
			"string ret = \"\";\n" +
			"for(int i = 0; i < 10; i++){\n" +
				"ret = ret + i;\n" +
			"};\n" +
			"return ret";
		CQuarkBlock block = new CQuarkBlock();
		string s = (string)block.Execute (method);
		Debug.Log (s);
	}
}
