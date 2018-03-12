using UnityEngine;
using System.Collections;
using System;

public class Demo4 : MonoBehaviour {
	
	// Use this for initialization
	public string m_blockFilePath;
	void Start()
	{
		Script.Instance.env.RegType (typeof(System.DateTime),"DateTime");
		Script.Instance.env.RegType (typeof(System.DayOfWeek),"DayOfWeek");

		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		Script.Instance.BuildFile(m_blockFilePath, text);
	}

	string result = "";
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 200, 50), "Eval use String"))
		{
			Script.Instance.ClearValue();
			string callExpr ="ScriptClass4 sc =new ScriptClass4();\n"+
				"sc.defHP1=100;\n"+
					"sc.defHP2=200;\n"+
					"return sc.GetHP();";
			object i = Script.Instance.Execute(callExpr);
			result = "result=" + i;
		}

		if (GUI.Button(new Rect(200, 0, 200, 50), "Eval use Code"))
		{
			Script.Instance.ClearValue();
			CQuark.CQ_Content content = new CQuark.CQ_Content(Script.Instance.env);
			//得到脚本类型
			var typeOfScript = Script.Instance.env.GetTypeByKeyword("ScriptClass4");
			//调用脚本类构造创造一个实例
			var thisOfScript = typeOfScript.function.New(content, null).value;
			//调用脚本类成员变量赋值
			//Debug.LogWarning(thisOfScript+","+ typeOfScript+","+ typeOfScript.function);
			typeOfScript.function.MemberValueSet(content, thisOfScript, "defHP1", 200);
			typeOfScript.function.MemberValueSet(content, thisOfScript, "defHP2", 300);
			//调用脚本类成员函数
			var returnvalue = typeOfScript.function.MemberCall(content, thisOfScript, "GetHP", null);
			object i = returnvalue.value;
			result = "result=" + i;
		}

		GUI.Label(new Rect(0, 50, 200, 50), result);
	}
}

