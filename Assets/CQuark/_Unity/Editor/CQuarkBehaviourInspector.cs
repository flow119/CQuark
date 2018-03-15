using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using CQuark;

[CustomEditor(typeof(CQuarkBehaviour))]
public class ScriptMonoInspector : Editor {

	CQuarkBehaviour _script;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		_script = target as CQuarkBehaviour;

		_script.m_codeType = (CQuarkBehaviour.ECodeType)EditorGUILayout.EnumPopup ("CodeType", _script.m_codeType);

		switch (_script.m_codeType) {
		case CQuarkBehaviour.ECodeType.FileName:
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

		case CQuarkBehaviour.ECodeType.TextAsset:
			GUILayout.Label("TextAsset作为类");
			GUILayout.Space(5);
			_script.m_className = EditorGUILayout.TextField("类名", _script.m_className);
			_script.m_ta = EditorGUILayout.ObjectField(_script.m_ta, typeof(TextAsset), false) as TextAsset;
			break;

		case CQuarkBehaviour.ECodeType.Text:
			GUILayout.Label("文本作为类");
			GUILayout.Space(5);
			_script.m_className = EditorGUILayout.TextField("类名", _script.m_className);
			_script.m_codeText = EditorGUILayout.TextArea(_script.m_codeText);
			break;
		}
	}
}
