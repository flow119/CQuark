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

	//TextAsset Or Text
	public TextAsset m_ta;
	public string m_codeText = "";

	protected override void Initialize(){
		switch(m_codeType){
		case ECodeType.FileName:
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
        SetMember("gameObject", typeof(GameObject), this.gameObject);
        SetMember("transform", typeof(Transform), this.transform);
	}

	CQ_Value SetMember(string name, CQ_Type type, Object obj){
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
