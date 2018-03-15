using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour {

	void OnEnable(){
		Debug.Log ("Enable");
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("AA");
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * 180 * Time.deltaTime);
	}
}
