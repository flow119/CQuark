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
        AppDomain.RegisterType(typeof(UnityEngine.Object), "Object");

        AppDomain.RegisterType(typeof(AssetBundle), "AssetBundle");
        AppDomain.RegisterType(typeof(Animation), "Animation");
        AppDomain.RegisterType(typeof(AnimationCurve), "AnimationCurve");
        AppDomain.RegisterType(typeof(AnimationClip), "AnimationClip");
        AppDomain.RegisterType(typeof(Animator), "Animator");
        AppDomain.RegisterType(typeof(Application), "Application");
        AppDomain.RegisterType(typeof(AudioSource), "AudioSource");
        AppDomain.RegisterType(typeof(AudioClip), "AudioClip");
        AppDomain.RegisterType(typeof(AudioListener), "AudioListener");

        AppDomain.RegisterType(typeof(Camera), "Camera");
        AppDomain.RegisterType(typeof(Component), "Component");
        AppDomain.RegisterType(typeof(Color), "Color");
        AppDomain.RegisterType(typeof(Debug), "Debug");
        AppDomain.RegisterType(typeof(GameObject), "GameObject");
        AppDomain.RegisterType(typeof(Input), "Input");

        AppDomain.RegisterType(typeof(KeyCode), "KeyCode");
        AppDomain.RegisterType(typeof(Light), "Light");
        AppDomain.RegisterType(typeof(Mathf), "Mathf");
        AppDomain.RegisterType(typeof(Material), "Material");
        AppDomain.RegisterType(typeof(Mesh), "Mesh");
        AppDomain.RegisterType(typeof(MeshFilter), "MeshFilter");

        AppDomain.RegisterType(typeof(ParticleSystem), "ParticleSystem");
        AppDomain.RegisterType(typeof(PlayerPrefs), "PlayerPrefs");
        AppDomain.RegisterType(typeof(Quaternion), "Quaternion");
        AppDomain.RegisterType(typeof(Renderer), "Renderer");
        AppDomain.RegisterType(typeof(UnityEngine.Random), "Random");
        AppDomain.RegisterType(typeof(Ray), "Ray");
        AppDomain.RegisterType(typeof(Resources), "Resources");

        AppDomain.RegisterType(typeof(Screen), "Screen");
        AppDomain.RegisterType(typeof(Shader), "Shader");
        AppDomain.RegisterType(typeof(Texture), "Texture");
        AppDomain.RegisterType(typeof(Transform), "Transform");
        AppDomain.RegisterType(typeof(UnityEngine.Time), "Time");

        AppDomain.RegisterType(typeof(Vector2), "Vector2");
        AppDomain.RegisterType(typeof(Vector3), "Vector3");
        AppDomain.RegisterType(typeof(Vector4), "Vector4");
        AppDomain.RegisterType(typeof(WWW), "WWW");
        AppDomain.RegisterType(typeof(WWWForm), "WWWForm");

        //TODO 补充NGUI,LitJson
        //补充自己需要的类
    }
}
