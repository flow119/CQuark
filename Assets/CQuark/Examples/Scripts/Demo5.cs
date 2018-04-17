using UnityEngine;
using System.Collections;
using System;

public class Demo5 : MonoBehaviour {
	public string m_blockFilePath;

	CQuarkParagraph script = new CQuarkParagraph();
	void Start(){
		CQuark.AppDomain.Reset();
        CQuark.AppDomain.RegisterType(typeof(Debug),"Debug");
		ExecuteFile ();
	}

	void ExecuteFile () {
		string text = LoadMgr.LoadFromStreaming(m_blockFilePath);
		StartCoroutine (script.StartCoroutine (text, this));
	}

}
