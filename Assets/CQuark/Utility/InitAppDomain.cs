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
            CQuark.AppDomain.Reset();
            RegisterUnityType();
        }
        CQuark.AppDomain.BuildProject(Application.streamingAssetsPath + "/" + m_folderPath, m_pattern);
        Debug.Log("Project Compile Finished");
    }

    public static void RegisterUnityType()
    {
        //以下内容是Unity专用，如果非Unity平台可以直接主食掉
		AppDomain.RegisterType<UnityEngine.Object>("Object");

		AppDomain.RegisterType<AssetBundle>("AssetBundle");
		AppDomain.RegisterType<Animation>("Animation");
		AppDomain.RegisterType<AnimationCurve>("AnimationCurve");
		AppDomain.RegisterType<AnimationClip>("AnimationClip");
		AppDomain.RegisterType<Animator>("Animator");
		AppDomain.RegisterType<Application>("Application");
		AppDomain.RegisterType<AudioSource>("AudioSource");
		AppDomain.RegisterType<AudioClip>("AudioClip");
		AppDomain.RegisterType<AudioListener>("AudioListener");

		AppDomain.RegisterType<Camera>("Camera");
		AppDomain.RegisterType<Component>("Component");
		AppDomain.RegisterType<Color>("Color");
		AppDomain.RegisterType<Debug>("Debug");
		AppDomain.RegisterType<GameObject>("GameObject");
		AppDomain.RegisterType<Input>("Input");

		AppDomain.RegisterType<KeyCode>("KeyCode");
		AppDomain.RegisterType<Light>("Light");
		AppDomain.RegisterType<Mathf>("Mathf");
		AppDomain.RegisterType<Material>("Material");
		AppDomain.RegisterType<Mesh>("Mesh");
		AppDomain.RegisterType<MeshFilter>("MeshFilter");

        AppDomain.RegisterType<ParticleSystem>("ParticleSystem");
		AppDomain.RegisterType<PlayerPrefs>("PlayerPrefs");
        AppDomain.RegisterType<Quaternion>("Quaternion");
        AppDomain.RegisterType<Renderer>("Renderer");
        AppDomain.RegisterType<UnityEngine.Random>("Random");
        AppDomain.RegisterType<Ray>("Ray");
        AppDomain.RegisterType<Resources>("Resources");
							  
        AppDomain.RegisterType<Screen>("Screen");
        AppDomain.RegisterType<Shader>("Shader");
        AppDomain.RegisterType<Texture>("Texture");
        AppDomain.RegisterType<Transform>("Transform");
        AppDomain.RegisterType<UnityEngine.Time>("Time");

        AppDomain.RegisterType<Vector2>("Vector2");
        AppDomain.RegisterType<Vector3>("Vector3");
        AppDomain.RegisterType<Vector4>("Vector4");
        AppDomain.RegisterType<WWW>("WWW");
        AppDomain.RegisterType<WWWForm>("WWWForm");

        //TODO 补充NGUI,LitJson
        //补充自己需要的类
    }
}
