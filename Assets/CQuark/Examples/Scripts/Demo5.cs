using UnityEngine;
using System.Collections;
using System;

public class Demo5 : MonoBehaviour,ICoroutine {
	public string m_blockFilePath;

	CQuarkBlock script = new CQuarkBlock();
	void Start(){
		CQuark.AppDomain.Reset();
        CQuark.AppDomain.RegisterType(typeof(Debug),"Debug");
		CQuark.AppDomain.RegisterMethod ((eDelay)Wait);
		CQuark.AppDomain.RegisterMethod ((eDelay)YieldWaitForSecond);
		ExecuteFile ();
	}

	void ExecuteFile () {
		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		StartCoroutine (script.StartCoroutine (text, this));
	}

	public object StartNewCoroutine(IEnumerator method){
		return StartCoroutine(method);
	}

	delegate IEnumerator eDelay(float t);
	IEnumerator Wait(float time){
		yield return new WaitForSeconds (time);
	}
	IEnumerator YieldWaitForSecond(float time){
		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds (0.1f);
			Debug.Log (i.ToString());
		}
		yield return new WaitForSeconds (time);
	}
}
