using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

//开发调试用的工具。正式产品不需要
public class TokenParserViewer : MonoBehaviour {

	public string m_text;
	public List<Token> m_tokens;

	public void RegistOriTypeNew(){
		
		AppDomain.Reset ();
		//		AppDomain.RegisterDefaultType ();
		InitAppDomain.RegisterFullnameType();

	}

	public void CompileNew(){

		//2.1 分割成Token
		m_tokens = TokenSpliter.SplitToken(m_text);
//		
//		//2.2把所有代码里的类注册一遍
		PreCompiler.RegisterCQClass("", m_tokens);
//		
		CQ_Compiler.CompileOneFile("", m_tokens);
	}

	public void CompileOld(){
		//TODO 注册类型
		AppDomain.Reset ();
//		AppDomain.RegisterDefaultType ();
		InitAppDomain.RegisterOriType ();
		AppDomain.RegisterType (typeof(GameObject[]), "GameObject[]");
		m_tokens = CQ_TokenParser.Parse (m_text);
	}
}
