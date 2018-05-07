using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

//开发调试用的工具。正式产品不需要
public class TokenParserViewer : MonoBehaviour {

	public string m_text;
	public List<Token> m_tokens;

	public void CompileNew(){
		TokenParser.CleartType();
		AppDomain.Reset ();
		AppDomain.RegisterDefaultType ();
//		InitAppDomain.RegisterFullnameType();
		m_tokens = TokenParser.Parse (m_text);
	}

	public void CompileOld(){
		//TODO 注册类型
		AppDomain.Reset ();
		AppDomain.RegisterDefaultType ();
		InitAppDomain.RegisterUnityType ();
		m_tokens = CQ_TokenParser.Parse (m_text);
	}
}
