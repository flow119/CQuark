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
	CQuark.ICQ_Type type;
	CQuark.SInstance inst;//脚本实例
	public string m_className;

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

		type = CQuark.AppDomain.GetTypeByKeywordQuiet(m_className);
		if(type == null){
			Debug.LogError("Type:" + m_className + "不存在与脚本项目中");
			return;
		}
		content = new CQ_Content();
		inst = type.function.New(content, null).value as CQuark.SInstance;
		content.CallType = inst.type;
		content.CallThis = inst;
//		if (!inst.member.ContainsKey ("gameObject")){
//			CQ_Content.Value val = new CQ_Content.Value ();
//			val.type = typeof(GameObject);
//			val.value = this.gameObject;
//			inst.member.Add ("gameObject", val);
//		}
//		if (!inst.member.ContainsKey ("transform")) {
//			CQ_Content.Value val = new CQ_Content.Value ();
//			val.type = typeof(Transform);
//			val.value = this.transform;
//			inst.member.Add ("transform", val);
//		}
		inst.member["gameObject"].value = this.gameObject;
//		inst.member ["transform"].value = this.transform;
	}


	protected override void CallScript(string methodName, bool useCoroutine){
		if (useCoroutine) {
			SType cclass = type.function as SType;
			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
				this.StartNewCoroutine (type.function.CoroutineCall (content, inst, methodName, null, this));
		}
		else {
			SType cclass = type.function as SType;
			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
				type.function.MemberCall (content, inst, methodName, null);
		}
	}
}
