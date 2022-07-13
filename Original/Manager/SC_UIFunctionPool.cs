using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_UIFunctionPool : MonoBehaviour
{
    static SC_UIFunctionPool _uniqueInstance;

    GameObject WindowTmp;
    public static SC_UIFunctionPool _instance
    {
        get { return _uniqueInstance; }
    }
    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void loadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void WindowOpen(GameObject _prefabWindow, GameObject parents)
    {
        Instantiate(_prefabWindow, parents.transform.position, Quaternion.identity, parents.transform);
    }
    public void WindowClose(GameObject CloseTarget)
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        Destroy(CloseTarget);
    }

    public string CheckCountAndColor(int Now,int Dst)
    {
        if(Now < Dst)
            return "<color=red>" + Now + "</color>" +"/"+ Dst;
        else
            return "<color=black>" + Now + "</color>" +"/"+ Dst;
    }

}
