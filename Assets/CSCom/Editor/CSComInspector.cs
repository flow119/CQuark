using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;


[CustomEditor(typeof(CSCom))]
public class CSComInspector : Editor {

	CSCom _script;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		_script = target as CSCom;

		GUILayout.Label("文本作为类");
		GUILayout.Space(5);
		_script.className = EditorGUILayout.TextField("类名", _script.className);
		_script.strSourceCode = EditorGUILayout.TextArea(_script.strSourceCode);

	}
}
