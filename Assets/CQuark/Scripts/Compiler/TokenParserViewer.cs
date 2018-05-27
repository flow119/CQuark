using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

//开发调试用的工具。正式产品不需要
public class TokenParserViewer : MonoBehaviour {

	public string m_text;
	public List<Token> m_tokens;

	public void RegistOriTypeNew(){
		AppDomain.Initialize (true, true, true);
	}

	public void CompileNew(){
		m_tokens = CQ_Compiler.CompileOneFile("", m_text) as List<Token>;
	}
}
