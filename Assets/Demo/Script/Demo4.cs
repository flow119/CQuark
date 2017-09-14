using UnityEngine;
using System.Collections;

public class Demo4 : ScriptMono {

	Script script = new Script();
	void Start(){
		script.RegTypes ();
		ExecuteFile ();
	}

	void ExecuteFile () {
		string text = (Resources.Load ("script04Coroutine") as TextAsset).text;

		StartCoroutine (script.StartCoroutine (text, this));
	}
}
