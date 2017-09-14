using UnityEngine;
using System.Collections;

public class MonoCoroutine : MonoBehaviour, ICoroutine {

	public static MonoCoroutine Instance{
		get{
			if(_instance == null){
				GameObject go = new GameObject("MonoCoroutine");
				_instance = go.AddComponent<MonoCoroutine>();
			}
			return _instance;
		}
	}

	private static MonoCoroutine _instance;
	
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
