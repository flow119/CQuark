using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;

/// <summary>
/// 在ScriptExecutionOrder里把Order设为-3000，确保AppDomain的注册最先执行
/// </summary>
public class InitAppDomain : MonoBehaviour {
    public static InitAppDomain Instance { get; private set; }
    static bool appDomainInit = false;

    //public List<string> m_buildFolder;
    public string m_folderPath = "/Demo07";
    public string m_pattern = "*.txt";

    void Awake()
    {
        Instance = this;
        if (!appDomainInit)
        {
            appDomainInit = true;
			//重置AppDomain(清空已注册类型，并且注册基本类型)
            CQuark.AppDomain.Initialize(true, true, true);
			//注册原生类型
        }
        CQuark.CQ_Compiler.CompileProject(Application.streamingAssetsPath + "/" + m_folderPath, m_pattern);
        Debug.Log("Project Compile Finished");
    }
}
