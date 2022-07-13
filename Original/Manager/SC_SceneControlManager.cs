using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_SceneControlManager : MonoBehaviour
{
    [SerializeField] GameObject PrefebFadeOut;
    [SerializeField] GameObject PrefebFadeIn;
    [SerializeField] GameObject LoaddingWindow;

    static SC_SceneControlManager _uniqueInstance;
    public AsyncOperation LoadingPer;

    public static SC_SceneControlManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void FadeIn()
    {
        GameObject go = Instantiate(PrefebFadeIn);
        go.AddComponent<SC_UIFadeIn>();
    }
    public void FadeOutAndLoadScene(string SceneName)
    {
        GameObject go = Instantiate(PrefebFadeOut);
        go.AddComponent<SC_UIFadeOut>();
        go.GetComponent<SC_UIFadeOut>().initSceneName(SceneName);
        
    }
    public void LoadSceneAndinit(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
