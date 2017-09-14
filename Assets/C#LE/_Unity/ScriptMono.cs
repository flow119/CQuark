using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSLE;

public class ScriptMono : MonoBehaviour, ICoroutine {

	public enum ECodeType
	{
//		FunctionBlock,	//一个函数块（一般用来测试）
		FunctionsText,	//常用函数作为string
		FileName,		//在StreamingAssets下的cs文件名
		Text,			//cs文件作为string
		TextAsset,		//cs文件作为TextAsset
	}

	public ECodeType m_codeType = ECodeType.FunctionsText;

	//如果!m_useTextAsset，则纯文本
	public Script m_script = new Script();
	public string m_Awake = "";
	public string m_OnEnable = "";
	public string m_OnDisable = "";
	public string m_Start = "";
	public string m_Update = "";
	public string m_FixedUpdate = "";
	public string m_OnDestroy = "";
	Dictionary<string, CSLE.ICLS_Expression> dictExpr = new Dictionary<string, CSLE.ICLS_Expression>();
	//FileName
	public string m_fileName = "";
	static bool s_projectBuilded = false;
	ScriptInstanceHelper inst;
//	//整个文件作为string
//	public string m_codeText = "";
//	//如果m_useTextAsset，则用文件
//	public TextAsset m_ta;


	public object StartNewCoroutine(IEnumerator method){
		return StartCoroutine(method);
	}

	public object WaitForSecond(float time){
		return StartCoroutine(Timer (time));
	}

	IEnumerator Timer(float time){
		yield return new WaitForSeconds (time);
	}

	void Awake(){
		ReBuildScript ();
		if (m_codeType == ECodeType.FileName) {
			inst = new ScriptInstanceHelper(m_fileName);
			inst.gameObject = this.gameObject;
		}
		CallScript ("Awake");
	}

	void OnEnable(){
		CallScript ("OnEnable");
	}

	void OnDisable(){
		CallScript ("OnDisable");
	}

	// Use this for initialization
	void Start () {
		CallScript ("Start");
	}
	
	// Update is called once per frame
	void Update () {
		CallScript ("Update");
	}

	void FixedUpdate () {
		CallScript ("FixedUpdate");
	}

	void OnDestroy(){
		CallScript ("OnDestroy");
	}

	void CallScript(string key){
		switch (m_codeType) {
		case ECodeType.FunctionsText:
			if (!dictExpr.ContainsKey (key))
				return;
			var expr = dictExpr [key];
			if (expr == null)
				return;
			var content = m_script.env.CreateContent ();//创建上下文，并设置变量给脚本访问
			content.DefineAndSet ("gameObject", typeof(GameObject), this.gameObject);
			content.DefineAndSet ("transform", typeof(Transform), this.transform);
			try {
				expr.ComputeValue (content);
			} catch (System.Exception e) {
				string dumpValue = content.DumpValue ();//出错可以dump脚本上现存的变量
				string dumpStack = content.DumpStack (null);//dump脚本堆栈，如果保存着token就可以dump出具体代码
				string dumpException = e.ToString ();
				Debug.LogError ("callscript:" + key + " error\n" 
				                + dumpValue + "\n" + dumpStack + "\n" + dumpException);
			}
			break;
		case ECodeType.FileName:
			inst.MemberCall(key);
			break;
		}
	}

	public void ReBuildScript(){
		m_script.Reset ();
		m_script.RegTypes ();
		switch (m_codeType) {
		case ECodeType.FunctionsText:
			BuildScript ("Start", m_Start);
			BuildScript ("OnEnable", m_OnEnable);
			BuildScript ("OnDisable", m_OnDisable);
			BuildScript ("Start", m_Start);
			BuildScript ("Update", m_Update);
			BuildScript ("FixedUpdate", m_FixedUpdate);
			BuildScript ("OnDestroy", m_OnDestroy);
			break;
		case ECodeType.FileName:
			if(!s_projectBuilded){
				Script.Instance.BuildProject(Application.streamingAssetsPath, "*.txt");
				s_projectBuilded = true;
			}
			break;
		}

	}

	#region 文本函数
	void BuildScript(string key, string code){
		if (string.IsNullOrEmpty (code))
			return;
		try{
			var token = m_script.env.ParserToken(code);
			var expr = m_script.env.Expr_CompileToken(token);
			dictExpr[key] = expr;
		}catch(System.Exception e){
			Debug.LogError("BuildScript:" + key + " err\n" + e.ToString());
		}
	}

	#endregion

	#region 文件名执行
	public class ScriptInstanceHelper{
		CSLE.ICLS_Type type;
		CSLE.SInstance inst;//脚本实例
		CSLE.CLS_Content content;//操作上下文

		public ScriptInstanceHelper(string scriptTypeName){
			type = Script.Instance.env.GetTypeByKeywordQuiet(scriptTypeName);
			if(type == null){
				Debug.LogError("Type:" + scriptTypeName + "不存在与脚本项目中");
				return;
			}
			content = Script.Instance.env.CreateContent();
			inst = type.function.New(content, null).value as CSLE.SInstance;
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

		public void MemberCall(string methodName){
			SType cclass = type.function as SType;
			if(cclass.functions.ContainsKey(methodName) || cclass.members.ContainsKey(methodName))
				type.function.MemberCall(content, inst, methodName, null);
		}
	}
	public interface IScriptBehaviour
	{
		GameObject gameObject{ get; }
	}
	#endregion
}
