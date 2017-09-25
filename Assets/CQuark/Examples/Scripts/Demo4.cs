using UnityEngine;
using System.Collections;
using System;

public class Demo4 : MonoBehaviour,ICoroutine {

	Script script = new Script();
	void Start(){
		script.RegTypes ();
		script.env.RegFunction ((eDelay)Wait);
		script.env.RegFunction ((eDelay)YieldWaitForSecond);
		ExecuteFile ();
	}

	void ExecuteFile () {
		Action<WWW> cb = delegate(WWW www) {
			StartCoroutine (script.StartCoroutine (www.text, this));
		};
		LoadMgr.Instance.LoadFromStreaming ("Blocks/script04Coroutine.txt", cb);
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
