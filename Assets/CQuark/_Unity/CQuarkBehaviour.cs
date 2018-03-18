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
	CQuark.IType type;
	CQuark.CQClassInstance inst;//脚本实例
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
		content = new CQ_Content(true);

		//TODO 最好在编译的时候就做，不要在实例化的时候做
		RegisterMember("gameObject", typeof(GameObject));
		RegisterMember("transform", typeof(Transform));

		inst = type._class.New(content, null).value as CQuark.CQClassInstance;
		DefaultValue("gameObject", typeof(GameObject), this.gameObject);
		DefaultValue("transform", typeof(Transform), this.transform);
	}

	CQ_Value DefaultValue(string name, CQ_Type type, Object obj){
		CQ_Value val = new CQ_Value ();
		val.type = type;
		val.value = obj;
		inst.member[name] = val;
		return val;
	}

	void RegisterMember(string name, CQ_Type cqtype){
		Class_CQuark cclass = (Class_CQuark)type._class;
		if(!cclass.members.ContainsKey(name)){
			Class_CQuark.Member m = new Class_CQuark.Member();
			m.bPublic = true;
			m.bReadOnly = true;
			m.type = AppDomain.GetType(cqtype);
			cclass.members.Add(name, m);
		}
	}
//	void AddMember(string name, CQ_Type type, Object obj){
//		if(!inst.type.members.ContainsKey(name)){
//			Class_CQuark.Member m = new Class_CQuark.Member();
//			m.bPublic = true;
//			m.bReadOnly = true;
//			inst.type.members.Add(name, m);
//		}
//		CQ_Value val = new CQ_Value ();
//		val.type = type;
//		val.value = obj;
//		inst.member.Add (name, val);
//	}

	protected override void CallScript(string methodName, bool useCoroutine){
		Class_CQuark cclass = type._class as Class_CQuark;
		if (useCoroutine) {
			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
				this.StartNewCoroutine (type._class.CoroutineCall (content, inst, methodName, null, this));
		}
		else {
			if (cclass.functions.ContainsKey (methodName) || cclass.members.ContainsKey (methodName))
				type._class.MemberCall (content, inst, methodName, null);
		}
	}
}
