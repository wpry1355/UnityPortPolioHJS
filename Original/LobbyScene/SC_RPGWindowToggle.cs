using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SC_RPGWindowToggle : MonoBehaviour
{

    [SerializeField]
    Toggle[] arrToggle;
    [SerializeField]
    GameObject[] arrPanel;
    int SelectStage;
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    public void OnToggle()
    {
        //스테이지 클릭 기점

        if (arrToggle[0].isOn == true)
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
            SelectStage = 0;
            arrPanel[0].SetActive(true);
            arrPanel[1].SetActive(false);
        }
        else if (arrToggle[1].isOn == true)
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
            SelectStage = 1;
            arrPanel[0].SetActive(false);
            arrPanel[1].SetActive(true);
        }
        else
        {
            arrPanel[0].SetActive(false);
            arrPanel[1].SetActive(false);
        }
    }
    public void StartGame()
    {
        // 게임스타트, 씬이동 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_UserInfoManager._instance._isPlayingRPG = true;
        SC_UserInfoManager._instance._SelectedStageNumber = SelectStage;
        SC_SceneControlManager._instance.FadeOutAndLoadScene("RPGInGameScene");
    }
}
