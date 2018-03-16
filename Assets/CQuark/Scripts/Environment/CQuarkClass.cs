using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using CQuark;
using System.Collections;


public class CQuarkClass
{
	public CQ_Content content = null;


	public static CQuarkClass Instance{
		get{
			if(_instance == null)
				_instance = new CQuarkClass();
			CQuark.AppDomain.RegDefaultType();
			return _instance;
		}
	}
	static CQuarkClass _instance;

    public object Eval(string script){
		var token = CQuark.AppDomain.ParserToken(script);//词法分析
		var expr = CQuark.AppDomain.Expr_CompileToken(token, true);//语法分析,简单表达式，一句话
		var value = CQuark.AppDomain.Expr_Execute(expr, content);//执行表达式
        if (value == null)
			return null;
        return value.value;
    }
	
    public object Execute(string script){
		var token = CQuark.AppDomain.ParserToken(script);//词法分析
		var expr = CQuark.AppDomain.Expr_CompileToken(token, false);//语法分析，语法块
		var value = CQuark.AppDomain.Expr_Execute(expr, content);//执行表达式
        if (value == null) return null;
        return value.value;
    }

	public IEnumerator StartCoroutine(string script, ICoroutine coroutine){
		var token = CQuark.AppDomain.ParserToken(script);//词法分析
		var expr = CQuark.AppDomain.Expr_CompileToken(token, false);//语法分析，语法块
		yield return coroutine.StartNewCoroutine(CQuark.AppDomain.Expr_Coroutine (expr, content, coroutine));
	}

	public CQ_Value GetValue(string name){
		if (content == null)
			content = new CQ_Content (true);
		return content.Get(name);
	}

	public void SetValue(string name, object v)
    {
		if (content == null)
            content = new CQ_Content(true);
        content.DefineAndSet(name, v.GetType(), v);
    }

	public void ClearValue(){
        content = null;
    }


	//TODO 编译单个文件
//	public IList<Token> BuildTextAsset(TextAsset ta)
//	{
//		return BuildText (ta.text);
//	}
//	public IList<Token> BuildText(string text){
//		var tokens = env.tokenParser.Parse (text);
//		env.com
//	}
}