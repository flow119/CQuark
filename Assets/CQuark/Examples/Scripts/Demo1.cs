using UnityEngine;
using System.Collections;

public class Demo1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Script.Init ();

		Eval1 ();
		Eval2 ();
		ExecuteBlock ();

	}

	//这个函数展示最简单的计算
	void Eval1 () {
		int ret = (int)Script.Instance.Eval ("1+2");
		Debug.Log ("return = " + ret);
	}

	//这个函数展示了先从外部向env赋值，再做计算
	void Eval2(){
		Script.Instance.ClearValue ();
		Script.Instance.SetValue ("HP1", 200);
		Script.Instance.SetValue ("HP2", 300);
		double d = (double)Script.Instance.Eval ("HP1 + HP2 * 0.5");
		Debug.Log ("d = " + d);
		float f = (float)Script.Instance.Eval ("HP1 + HP2 * 0.5f");
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
		string s = (string)Script.Instance.Execute (method);
		Debug.Log (s);
	}
}
