using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CQuark;

public class CQuarkBehaviourSimple : CQuarkBehaviourBase {

	public string m_Awake = "";
	public string m_OnEnable = "";
	public string m_OnDisable = "";
	public string m_Start = "";
	public string m_Update = "";
	public string m_FixedUpdate = "";
	public string m_OnDestroy = "";
	Dictionary<string, CQuark.ICQ_Expression> dictExpr = new Dictionary<string, CQuark.ICQ_Expression>();

	protected override void Initialize(){
		BuildBlock ("Awake", m_Awake);
		BuildBlock ("OnEnable", m_OnEnable);
		BuildBlock ("OnDisable", m_OnDisable);
		BuildBlock ("Start", m_Start);
		BuildBlock ("Update", m_Update);
		BuildBlock ("FixedUpdate", m_FixedUpdate);
		BuildBlock ("OnDestroy", m_OnDestroy);
		content = new CQ_Content();//创建上下文，并设置变量给脚本访问
        content.DepthAdd();
        DefineAndSet("gameObject", typeof(GameObject), this.gameObject);
        DefineAndSet("transform", typeof(Transform), this.transform);
	}

    void DefineAndSet (string name, System.Type type, Object obj) {
        CQ_Value value = new CQ_Value();
        value.SetValue(type, obj);
        content.DefineAndSet(name, value);
    }


	protected override void CallScript(string key, bool useCoroutine){
		if (!dictExpr.ContainsKey (key))
			return;
		var expr = dictExpr [key];
		if (expr == null)
			return;

		if(useCoroutine){
            //StartNewCoroutine(expr.CoroutineCompute(content, this));
           StartCoroutine(expr.CoroutineCompute(content, this));
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
	}


	void BuildBlock(string key, string code){
		if (string.IsNullOrEmpty (code))
			return;
		try{
			var expr = CQuark.AppDomain.BuildBlock(code);
			dictExpr[key] = expr;
		}catch(System.Exception e){
			Debug.LogError("BuildScript:" + key + " err\n" + e.ToString());
		}
	}

	void Update () {
		CallScript ("Update");
	}

	void FixedUpdate () {
		CallScript ("FixedUpdate");
	}
}
