using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

public class CQuarkBehaviour : MonoBehaviour, ICoroutine {

	public enum ECodeType
	{
//		FunctionBlock,	//一个函数块（一般用来测试）
		FileName,		//在StreamingAssets下的cs文件名
		TextAsset,		//cs文件作为TextAsset
		Text,			//cs文件作为string
		FunctionsText,	//常用函数作为string
	}

	public ECodeType m_codeType = ECodeType.FunctionsText;

	CQ_Content content;

	//FileName or TextAsset or Text
	public string m_className;

	//FileName
	static bool s_projectBuilded = false;
	ScriptInstanceHelper inst;
	public string m_folderPath = "/Demo07";
	public string m_pattern = "*.txt";

	//TextAsset Or Text
	ICQ_Type typeOfScript;////得到脚本类型
	object thisOfScript; //调用脚本类构造创造一个实例
	public TextAsset m_ta;
	public string m_codeText = "";

	//FunctionsText
	public CQuarkClass m_script = new CQuarkClass();
	public string m_Awake = "";
	public string m_OnEnable = "";
	public string m_OnDisable = "";
	public string m_Start = "";
	public string m_Update = "";
	public string m_FixedUpdate = "";
	public string m_OnDestroy = "";
	Dictionary<string, CQuark.ICQ_Expression> dictExpr = new Dictionary<string, CQuark.ICQ_Expression>();


	public object StartNewCoroutine(IEnumerator method){
		return StartCoroutine(method);
	}
	
	delegate IEnumerator eDelay(float t);
	IEnumerator Wait(float time){
		yield return new WaitForSeconds (time);
	}


	void Awake(){
		Initialize ();
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
		CallScript (key, false);
	}

	void CallScript(string key, bool useCoroutine){
		switch (m_codeType) {
		case ECodeType.Text:
		case ECodeType.TextAsset:
		case ECodeType.FileName:
			//			content.DefineAndSet ("gameObject", typeof(GameObject), this.gameObject);
			//			content.DefineAndSet ("transform", typeof(Transform), this.transform);
			if(useCoroutine)
				inst.CoroutineCall(key, this);
			else
				inst.MemberCall(key);
			break;

//		case ECodeType.Text:
//		case ECodeType.TextAsset:
//
//			if(typeOfScript.function.HasFunction(key)){
//				if(useCoroutine)
//					StartCoroutine(typeOfScript.function.CoroutineCall(content, thisOfScript, key, null, this));
//				else
//					typeOfScript.function.MemberCall(content, thisOfScript, key, null);
//			}
//			break;

		case ECodeType.FunctionsText:
			if (!dictExpr.ContainsKey (key))
				return;
			var expr = dictExpr [key];
			if (expr == null)
				return;

			if(useCoroutine){
				StartNewCoroutine(expr.CoroutineCompute(content, this));
			}
			else{
				try {
					expr.ComputeValue (content);
				} catch (System.Exception e) {
					string dumpValue = content.DumpValue ();//出错可以dump脚本上现存的变量
					string dumpStack = content.DumpStack (null);//dump脚本堆栈，如果保存着token就可以dump出具体代码
					string dumpException = e.ToString ();
					Debug.LogError ("callscript:" + key + " error\n" 
					                + dumpValue + "\n" + dumpStack + "\n" + dumpException);
				}
			}
			break;
		}
	}

	static bool appDomainInit = false;
	public void Initialize(){
		if(!appDomainInit){
			appDomainInit = true;
			AppDomain.Reset();
		}
		CQuark.AppDomain.RegFunction ((eDelay)Wait);

		if (m_codeType != ECodeType.FunctionsText) {
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

		} else {
			BuildBlock ("Start", m_Start);
			BuildBlock ("OnEnable", m_OnEnable);
			BuildBlock ("OnDisable", m_OnDisable);
			BuildBlock ("Start", m_Start);
			BuildBlock ("Update", m_Update);
			BuildBlock ("FixedUpdate", m_FixedUpdate);
			BuildBlock ("OnDestroy", m_OnDestroy);
			content = CQuark.AppDomain.CreateContent ();//创建上下文，并设置变量给脚本访问
			content.DefineAndSet ("gameObject", typeof(GameObject), this.gameObject);
			content.DefineAndSet ("transform", typeof(Transform), this.transform);
            
		}
	}

	#region 文件名执行
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
			content = CQuark.AppDomain.CreateContent();
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

	#endregion

	#region TextAsset or Text

	#endregion

	#region 文本函数
	void BuildBlock(string key, string code){
		if (string.IsNullOrEmpty (code))
			return;
		try{
			var token = CQuark.AppDomain.ParserToken(code);
			var expr = CQuark.AppDomain.Expr_CompileToken(token);
			dictExpr[key] = expr;
		}catch(System.Exception e){
			Debug.LogError("BuildScript:" + key + " err\n" + e.ToString());
		}
	}

	#endregion


}
