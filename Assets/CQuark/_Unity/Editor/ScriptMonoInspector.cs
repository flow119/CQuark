using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using CQuark;

[CustomEditor(typeof(ScriptMono))]
public class ScriptMonoInspector : Editor {

	ScriptMono _script;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		_script = target as ScriptMono;

		_script.m_codeType = (ScriptMono.ECodeType)EditorGUILayout.EnumPopup ("CodeType", _script.m_codeType);

		switch (_script.m_codeType) {
		case ScriptMono.ECodeType.FileName:
			GUILayout.Label("编译StreamingAssets下所有脚本");
			GUILayout.Space(5);
			_script.m_folderPath = EditorGUILayout.TextField("编译文件夹", _script.m_folderPath);
			_script.m_pattern = EditorGUILayout.TextField("编译后缀过滤", _script.m_pattern);
			_script.m_className = EditorGUILayout.TextField("类名", _script.m_className);
			if(Application.isPlaying){
				if(GUILayout.Button("重新编译项目")){
					
				}
			}
			break;

		case ScriptMono.ECodeType.TextAsset:
			GUILayout.Label("TextAsset作为类");
			GUILayout.Space(5);
			_script.m_className = EditorGUILayout.TextField("类名", _script.m_className);
			_script.m_ta = EditorGUILayout.ObjectField(_script.m_ta, typeof(TextAsset), false) as TextAsset;
			break;

		case ScriptMono.ECodeType.Text:
			GUILayout.Label("文本作为类");
			GUILayout.Space(5);
			_script.m_className = EditorGUILayout.TextField("类名", _script.m_className);
			_script.m_codeText = EditorGUILayout.TextArea(_script.m_codeText);
			break;

		case ScriptMono.ECodeType.FunctionsText:
			GUILayout.Label("文本作为函数块");
			GUILayout.Space(5);
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
		}
	}
}
