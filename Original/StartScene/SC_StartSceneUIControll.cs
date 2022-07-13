using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_StartSceneUIControll : MonoBehaviour
{
    
    [SerializeField] GameObject prefepSettingWindow;
    private void Awake()
    {

    }
    public void StartGame()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SceneControlManager._instance.FadeOutAndLoadScene("LobbyScene");       
    }
    public void ExitGame()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_UIFunctionPool._instance.exitGame();

    }

    public void SettingWindowOpen()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        SC_UIFunctionPool._instance.WindowOpen(prefepSettingWindow,transform.gameObject);
    }
}
