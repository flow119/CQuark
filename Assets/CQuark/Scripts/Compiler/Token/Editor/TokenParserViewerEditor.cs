using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using CQuark;

[CustomEditor(typeof(TokenParserViewer))]
public class TokenParserViewerEditor : Editor {

	TokenParserViewer _target;
	string _console = "";
	int _lastline = 0;

	static Dictionary<TokenType, Color> s_typeColor = new Dictionary<TokenType, Color>{
		{TokenType.COMMENT, new Color(0.5f,0.7f,0.6f)},
		{TokenType.KEYWORD, new Color(0.2f,0.9f,0.4f)},
		{TokenType.PUNCTUATION, new Color(1f,1f,1f)},
		{TokenType.STRING, new Color(1f,0.5f,0.1f)},
		{TokenType.VALUE, new Color(1f,0.7f,0.1f)},

		{TokenType.IDENTIFIER, new Color(0,0,0)},
		{TokenType.UNKNOWN, new Color(0.2f,0.2f,0.2f)},

		{TokenType.NAMESPACE, new Color(0,0.1f,0.6f)},
		{TokenType.CLASS, new Color(0,0.2f,0.7f)},
		{TokenType.TYPE, new Color(0.2f,0.1f,1f)},
		{TokenType.PROPERTY, new Color(1f,0.9f,0.1f)},
		{TokenType.FUNCTION, new Color(0.1f,0.8f,0.8f)},
	};

	public override void OnInspectorGUI (){
		_target = target as TokenParserViewer;

		GUILayout.Label("原文");
		_target.m_text		= EditorGUILayout.TextArea(_target.m_text);

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Compile Old")) {
			_target.CompileOld();
		}
		if (GUILayout.Button ("Compile New")) {
			_target.CompileNew();
		}
		GUILayout.EndHorizontal ();

		int depth = 0;
		if (_target.m_tokens != null) {
			GUILayout.BeginHorizontal();
			for(int i = 0; i < _target.m_tokens.Count; i++){
				GUI.contentColor = s_typeColor[_target.m_tokens[i].type];



				if(_target.m_tokens[i].type == CQuark.TokenType.PUNCTUATION && _target.m_tokens[i].text == "}"){
					depth--;
				}

				if(_target.m_tokens[i].line != _lastline){
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Space (depth * 20);
					_lastline = _target.m_tokens[i].line;
				}

				if(GUILayout.Button(_target.m_tokens[i].text))
					_console = _target.m_tokens[i].ToString();
				
				
				if(_target.m_tokens[i].type == CQuark.TokenType.PUNCTUATION && _target.m_tokens[i].text == "{"){
					depth++;
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.Label(_console);
	}
}
