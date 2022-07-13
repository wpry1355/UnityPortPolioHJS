using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TDWindowToggle : MonoBehaviour
{
    [SerializeField]
    Toggle[] arrToggle;
    [SerializeField]
    GameObject[] arrPanel;
    int StageNumber = 0;
    public void OnToggle()
    {
        //TD 스테이지 터치 기점.
        if (arrToggle[0].isOn == true)
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
            StageNumber = 0;
            arrPanel[0].SetActive(true);
            arrPanel[1].SetActive(false);
            arrPanel[2].SetActive(false);
        }
        else if (arrToggle[1].isOn == true)
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
            StageNumber = 1;
            arrPanel[0].SetActive(false);
            arrPanel[1].SetActive(true);
            arrPanel[2].SetActive(false);
        }
        else
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
            StageNumber = 2;
            arrPanel[0].SetActive(false);
            arrPanel[1].SetActive(false);
            arrPanel[2].SetActive(true);
        }
    }
    public void StartGame()
    {
        //TD 게임 스타트(씬이동) 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_UserInfoManager._instance._SelectedStageNumber = StageNumber;
        SC_UserInfoManager._instance._isPlayingTD = true;
        SC_SceneControlManager._instance.FadeOutAndLoadScene("TDInGameScene");
    }
}