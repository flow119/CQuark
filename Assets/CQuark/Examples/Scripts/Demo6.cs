using UnityEngine;
using System.Collections;
using System;

public class Demo6 : MonoBehaviour, ICoroutine {

	public string m_blockFilePath;

	// Use this for initialization
	void Start () {
		Script.Instance.RegTypes ();
		Script.Instance.env.RegFunction ((eDelay)Wait);

		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		Script.Instance.BuildFile(m_blockFilePath, text);
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
			Script.Instance.ClearValue();
			CQuark.CQ_Content content = new CQuark.CQ_Content(Script.Instance.env);
			//得到脚本类型
			var typeOfScript = Script.Instance.env.GetTypeByKeyword("ScriptClass6");
			//调用脚本类构造创造一个实例
			var thisOfScript = typeOfScript.function.New(content, null).value;

			//调用脚本类成员函数
			StartCoroutine(typeOfScript.function.CoroutineCall(content, thisOfScript, "GetHP", null, this));
		}
	}
}
