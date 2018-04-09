using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using CQuark;

[CustomEditor(typeof(CQuarkBehaviourSimple))]
public class CQuarkBehaviourSimpleInspector : Editor {

	CQuarkBehaviourSimple _script;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		_script = target as CQuarkBehaviourSimple;

		GUILayout.Label("文本作为函数块");
		GUILayout.Space(5);

		GUILayout.Label("Awake");
		_script.m_Awake 		= EditorGUILayout.TextArea(_script.m_Awake);
		GUILayout.Label("OnEnable");
		_script.m_OnEnable 		= EditorGUILayout.TextArea(_script.m_OnEnable);
		GUILayout.Label("OnDisable");
		_script.m_OnDisable 	= EditorGUILayout.TextArea(_script.m_OnDisable);
		GUILayout.Label("Start");
		_script.m_Start 		= EditorGUILayout.TextArea(_script.m_Start);
		GUILayout.Label("Update");
		_script.m_Update 		= EditorGUILayout.TextArea(_script.m_Update);
		GUILayout.Label("FixedUpdate");
		_script.m_FixedUpdate 	= EditorGUILayout.TextArea(_script.m_FixedUpdate);
		GUILayout.Label("OnDestroy");
		_script.m_OnDestroy 	= EditorGUILayout.TextArea(_script.m_OnDestroy);

		if(Application.isPlaying){
			if(GUILayout.Button("重新编译文本")){

			}
		}
	}
}
