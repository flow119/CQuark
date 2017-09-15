using UnityEngine;
using System.Collections;
using System;

public class Demo4 : MonoBehaviour,ICoroutine {

	Script script = new Script();
	void Start(){
		script.RegTypes ();
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
	
	public object WaitForSecond(float time){
		return StartCoroutine(Timer (time));
	}

	IEnumerator Timer(float time){
		yield return new WaitForSeconds (time);
	}
}
