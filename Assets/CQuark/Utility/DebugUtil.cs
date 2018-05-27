/// <summary>
/// Copyright by csm, Update 2017.09.20
/// 2017.09.20 增加Log颜色
/// </summary>
#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
using UnityEngine;
#else
using System;
#endif
using System.Collections.Generic;
using CQuark;
//TODO 增加Log类型，数据备份
//public enum ELogType{
//	Info,		//灰色。	用于硬件信息，IP地址等。		每日备份，每日清空
//	Debug,		//蓝色。	测试调试功能，release中删除。	不备份，每日清空
//	Log,		//白色。	开服、公告信息。客户端调试。	每日备份，每日清空
//	Important,	//绿色。	充值订单验证。					立即备份，不清空
//	Warning,	//黄色。	已处理的异常。					每日备份，不清空
//	Error		//红色。	错误或异常。					立即备份，不清空
//}

public class DebugUtil {

	public static void LogError (IList<Token> tlist, string text, int pos, int posend) {
		string str = "";
		for(int i = pos; i <= posend; i++) {
			str += tlist[i].text + " ";
		}
		DebugUtil.LogError(text + ":" + str + "(" + pos + "-" + posend + ")");
	}

	public static void Log(string s){
#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
		Debug.Log (s);
#else
		Console.ForegroundColor = System.ConsoleColor.White;
		Console.WriteLine (s);
#endif
	}

	public static void LogWarning(string s){
#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
		Debug.LogWarning (s);
#else
		Console.ForegroundColor = System.ConsoleColor.Yellow;
		Console.WriteLine (s);
#endif
	}

	public static void LogError(string s){
#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
		Debug.LogError (s);
#else
		Console.ForegroundColor = System.ConsoleColor.Red;
		Console.WriteLine (s);
#endif
	}

#if DEBUG || UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE
	public static void Log(Color col, string text){
		Debug.Log ("<color=#" + ColorToHexStr (col) + ">" + text + "</color>");
	}
	public static string ColorToHexStr(Color32 col){
		string returnStr = "";
		returnStr += col.r.ToString ("x2");
		returnStr += col.g.ToString ("x2");
		returnStr += col.b.ToString ("x2");
		return returnStr;
	}
#else
	public static void Log(System.ConsoleColor col, string text){
		Console.ForegroundColor = col;
		Console.WriteLine (s);
	}
#endif

//	public static void Log(string text, ELogType logType){
//
//	}
}
