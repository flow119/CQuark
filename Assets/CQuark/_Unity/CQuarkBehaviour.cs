using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

public class CQuarkBehaviour : MonoBehaviourAdapter {

	public enum ECodeType
	{
		FileName,		//在StreamingAssets下的cs文件名
		TextAsset,		//cs文件作为TextAsset
		Text,			//cs文件作为string
	}

	public ECodeType m_codeType = ECodeType.FileName;

	public string m_className;
	ScriptInstanceHelper inst;
//	CQuark.ICQ_Type type;
//	CQuark.SInstance inst;//脚本实例

	//FileName
	static bool s_projectBuilded = false;
	public string m_folderPath = "/Demo07";
	public string m_pattern = "*.txt";

	//TextAsset Or Text
	public TextAsset m_ta;
	public string m_codeText = "";

	protected override void Initialize(){
		base.Initialize ();

		switch(m_codeType){
		case ECodeType.FileName:
			if(!s_projectBuilded){
				AppDomain.BuildProject(Application.streamingAssetsPath + m_folderPath, m_pattern);
				s_projectBuilded = true;
			}
			break;
		case ECodeType.TextAsset:
			AppDomain.BuildFile(m_className, m_ta.text);
			break;
		case ECodeType.Text:
			AppDomain.BuildFile(m_className, m_codeText);
			break;
		}
		inst = new ScriptInstanceHelper(m_className);
		inst.gameObject = this.gameObject;
//		type = CQuark.AppDomain.GetTypeByKeywordQuiet(m_className);
//		if(type == null){
//			Debug.LogError("Type:" + m_className + "不存在与脚本项目中");
//			return;
//		}
//		content = new CQ_Content ();
//		inst = type.function.New(content, null).value as CQuark.SInstance;
//		content.CallType = inst.type;
//		content.CallThis = inst;
//		content.DefineAndSet ("gameObject", typeof(GameObject), this.gameObject);
//		content.DefineAndSet ("transform", typeof(Transform), this.transform);
	}


	protected override void CallScript(string methodName, bool useCoroutine){
//		if (useCoroutine) {
//			SType cclass = type.function as SType;
//			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
//				this.StartNewCoroutine (type.function.CoroutineCall (content, inst, methodName, null, this));
//		}
//		else {
//			SType cclass = type.function as SType;
//			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
//				type.function.MemberCall (content, inst, methodName, null);
//		}
		if(useCoroutine)
			inst.CoroutineCall(methodName, this);
		else
			inst.MemberCall(methodName);
	}


	public class ScriptInstanceHelper{
		CQuark.ICQ_Type type;
		CQuark.SInstance inst;//脚本实例
		CQuark.CQ_Content content;//操作上下文

		public ScriptInstanceHelper(string scriptTypeName){
			type = CQuark.AppDomain.GetTypeByKeywordQuiet(scriptTypeName);
			if(type == null){
				Debug.LogError("Type:" + scriptTypeName + "不存在与脚本项目中");
				return;
			}
			content = new CQ_Content();
			inst = type.function.New(content, null).value as CQuark.SInstance;
			content.CallType = inst.type;
			content.CallThis = inst;
		}

		public GameObject gameObject{
			get{
				return inst.member["gameObject"].value as GameObject;
			}set{
				inst.member["gameObject"].value = value;
			}
		}

		public Transform transform{
			get{
				return inst.member["transform"].value as Transform;
			}set{
				inst.member["transform"].value = value;
			}
		}

		public void MemberCall(string methodName){
			SType cclass = type.function as SType;
			if(cclass.functions.ContainsKey(methodName) || cclass.members.ContainsKey(methodName))
				type.function.MemberCall(content, inst, methodName, null);
		}

		public void CoroutineCall(string methodName, ICoroutine coroutine){
			SType cclass = type.function as SType;
			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
				coroutine.StartNewCoroutine (type.function.CoroutineCall (content, inst, methodName, null, coroutine));
		}
	}
}
