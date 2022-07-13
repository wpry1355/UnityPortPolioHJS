using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_UIFadeOut : MonoBehaviour
{
    Image TargetImg;
    float FadeOutPanel;
    float FadeOutSound;
    string SceneName;

    private void Awake()
    {
        TargetImg =gameObject.transform.GetChild(0).GetComponent<Image>();
        TargetImg.color = new Color(0, 0, 0, 0);
        FadeOutPanel = 0;
        FadeOutSound = 0;
    }
    public void initSceneName(string _SceneName)
    {
        SceneName = _SceneName;
    }
    private void Update()
    {
        FadeOutPanel = FadeOutPanel + Time.deltaTime;
        FadeOutSound = FadeOutSound + (Time.deltaTime / 255);
        TargetImg.color = new Color(TargetImg.color.r, TargetImg.color.g, TargetImg.color.b, FadeOutPanel);
        SC_SoundControlManager._instance.NowPlayBGM.GetComponent<AudioSource>().volume = SC_SoundControlManager._instance.NowPlayBGM.GetComponent<AudioSource>().volume - FadeOutSound;
        if(FadeOutPanel>2)
        {
            //로드 함수 실행기점
            SC_SceneControlManager._instance.LoadSceneAndinit(SceneName);
        }
    }

}
