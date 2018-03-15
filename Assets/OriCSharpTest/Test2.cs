using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : Test1 {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.down * 90 * Time.deltaTime);
	}
}
