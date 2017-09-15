using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using CSLE;

[CustomEditor(typeof(ScriptMono))]
public class ScriptMonoInspector : Editor {

	ScriptMono _script;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		_script = target as ScriptMono;

		_script.m_codeType = (ScriptMono.ECodeType)EditorGUILayout.EnumPopup ("CodeType", _script.m_codeType);

		switch (_script.m_codeType) {
		case ScriptMono.ECodeType.FunctionsText:
			GUILayout.Label("文本作为函数");
			_script.m_Start 		= EditorGUILayout.TextField("Start", _script.m_Start);
			_script.m_OnEnable 		= EditorGUILayout.TextField("OnEnable", _script.m_OnEnable);
			_script.m_OnDisable 	= EditorGUILayout.TextField("OnDisable", _script.m_OnDisable);
			_script.m_Start 		= EditorGUILayout.TextField("Start", _script.m_Start);
			_script.m_Update 		= EditorGUILayout.TextField("Update", _script.m_Update);
			_script.m_FixedUpdate 	= EditorGUILayout.TextField("FixedUpdate", _script.m_FixedUpdate);
			_script.m_OnDestroy 	= EditorGUILayout.TextField("OnDestroy", _script.m_OnDestroy);

			if(Application.isPlaying){
				if(GUILayout.Button("重新编译文本")){

				}
			}
			break;
		case ScriptMono.ECodeType.FileName:
			GUILayout.Label("从StreamingAssets加载");
			_script.m_fileName = EditorGUILayout.TextField("文件名", _script.m_fileName);
			if(Application.isPlaying){
				if(GUILayout.Button("重新编译项目")){
					
				}
			}
			break;
		default :
			GUILayout.Label("下个版本实现");
			break;
		}
	}
}
