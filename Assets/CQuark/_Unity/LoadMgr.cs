using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LoadMgr : MonoBehaviour {

	public static LoadMgr Instance{
		get{
			if(_instance == null){
				_instance = new GameObject("LoadManager").AddComponent<LoadMgr>();
			}
			return _instance;
		}
	}

	static LoadMgr _instance;

	public static string FixStreamingPath(string filename){  
		string pre = "file://";  
		#if UNITY_EDITOR  
		pre = "file://";  
		#elif UNITY_ANDROID  
		pre = "";  
		#elif UNITY_IPHONE  
		pre = "file://";  
		#endif  
		return pre + Application.streamingAssetsPath + "/" + filename;  
	}
	
	public static string FixPersistentPath(string filename){
		string pre = "file://";  
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN  
		pre = "file:///";  
		#elif UNITY_ANDROID  
		pre = "file://";  
		#elif UNITY_IPHONE  
		pre = "file://";  
		#endif  
		return pre + Application.persistentDataPath + "/" + filename;  
	}


	public void LoadFromStreaming(string filename, Action<WWW> loadCallback){
		string path = FixStreamingPath (filename);
		StartCoroutine (LoadIE (path, loadCallback));
	}

	public void LoadFromPersistent(string filename, Action<WWW> loadCallback){
		string path = FixPersistentPath (filename);
		StartCoroutine (LoadIE (path, loadCallback));
	}

	IEnumerator LoadIE(string path, Action<WWW> callBack){
		WWW www = new WWW (path);
		yield return www;
		callBack (www);
	}
}
