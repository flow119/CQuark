using UnityEngine;
using System.Collections;
using System;

public class Demo6 : MonoBehaviour, ICoroutine {

	public string m_blockFilePath;
    public string m_className = "ScriptClass6B";

	// Use this for initialization
	void Start () {
		CQuark.AppDomain.Reset();
		CQuark.AppDomain.RegisterMethod ((eDelay)Wait);

		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		CQuark.AppDomain.BuildFile(m_blockFilePath, text);
	}

	delegate IEnumerator eDelay(float t);
	IEnumerator Wait(float time){
		yield return new WaitForSeconds (time);
	}

	public object StartNewCoroutine(IEnumerator method){
		return StartCoroutine(method);
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 200, 50), "Coroutine Call"))
		{
			CQuark.CQ_Content content = new CQuark.CQ_Content();
			//得到脚本类型
			var typeOfScript = CQuark.AppDomain.GetTypeByKeyword(m_className);
			//调用脚本类构造创造一个实例
			var thisOfScript = typeOfScript._class.New(content, null).value;

			//调用脚本类成员函数
			StartCoroutine(typeOfScript._class.CoroutineCall(content, thisOfScript, "GetHP", null, this));
		}
	}
}
