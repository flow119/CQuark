using UnityEngine;
using System.Collections;
using System;

public class Demo4 : MonoBehaviour {
	
	// Use this for initialization
	public string m_blockFilePath;

	void Start()
	{
        CQuark.AppDomain.Initialize(false, true, true);
//        InitAppDomain.RegisterOriType();
//        CQuark.AppDomain.RegisterType (typeof(System.DateTime),"DateTime");
//		CQuark.AppDomain.RegisterType (typeof(System.DayOfWeek),"DayOfWeek");
		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		CQuark.CQ_Compiler.CompileOneFile(m_blockFilePath, text);
	}

	string result = "";
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 200, 50), "Eval use String"))
		{
			string callExpr ="ScriptClass4 sc =new ScriptClass4();\n"+
				"sc.defHP1=100;\n"+
					"sc.defHP2=200;\n"+
					"return sc.GetHP();";
			CQuarkParagraph block = new CQuarkParagraph();
			CQuark.CQ_Value i = block.Execute(callExpr);
			result = "result=" + i;
		}

		if (GUI.Button(new Rect(200, 0, 200, 50), "Eval use Code"))
		{
			//得到类型
			CQuark.IType typeOfScript = CQuark.AppDomain.GetTypeByKeyword("ScriptClass4");

			CQuark.CQ_Content content = new CQuark.CQ_Content();
			//调用脚本类构造创造一个实例
			var thisOfScript = typeOfScript._class.New(content, new CQuark.CQ_Value[0]).GetObject();

			//调用脚本类成员变量赋值
			//Debug.LogWarning(thisOfScript+","+ typeOfScript+","+ typeOfScript.function);
            CQuark.CQ_Value v1 = new CQuark.CQ_Value();
			v1.SetNumber(typeof(int), 150);
            CQuark.CQ_Value v2 = new CQuark.CQ_Value();
			v2.SetNumber(typeof(int), 300);
            typeOfScript._class.MemberValueSet(content, thisOfScript, "defHP1", v1);
            typeOfScript._class.MemberValueSet(content, thisOfScript, "defHP2", v2);
			//调用脚本类成员函数
            var returnvalue = typeOfScript._class.MemberCall(content, thisOfScript, "GetHP", new CQuark.CQ_Value[0]);
			result = "result=" + returnvalue;
		}

		GUI.Label(new Rect(0, 50, 200, 50), result);
	}
}

