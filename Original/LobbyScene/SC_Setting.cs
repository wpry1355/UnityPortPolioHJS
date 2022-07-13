using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Setting : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider EffectSlider;
    [SerializeField] Image BGMMute;
    [SerializeField] Image EffectMute;
    [SerializeField] Sprite[] MuteImage = new Sprite[2];
    float SaveBGMVolum;
    float SaveEffectVolum;

    bool IsInit = false;
    void Awake()
    {
        initSetting();
    }

    void initSetting()
    {
        BGMSlider.value = SC_SoundControlManager._instance._BGMVolum;
        EffectSlider.value = SC_SoundControlManager._instance._EffectSoundVolume;
        SaveBGMVolum = SC_SoundControlManager._instance._saveBGMMuteVolume;
        SaveEffectVolum = SC_SoundControlManager._instance._saveEffectMuteVolume;
        ChageMuteImage();
        IsInit = true;
    }
    


    void ChageMuteImage()
    {
        if (BGMSlider.value > 0)
            BGMMute.sprite = MuteImage[0];
        else
            BGMMute.sprite = MuteImage[1];
        if (EffectSlider.value > 0)
            EffectMute.sprite = MuteImage[0];
        else
            EffectMute.sprite = MuteImage[1];
    }

    //SliderChange
    public void BGMSliderValueChange()
    {
        if(IsInit)
            SC_SoundControlManager._instance.BGMVolumeSetting(BGMSlider.value);
        ChageMuteImage();
    }

    public void EffectSliderValueChange()
    {
        if (IsInit)
            SC_SoundControlManager._instance.EffectSoundVolumeSetting(EffectSlider.value);
        ChageMuteImage();
    }


    //Mute
    public void BGMMuteOnOff()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        if (BGMSlider.value > 0)
        {
            SaveBGMVolum = BGMSlider.value;
            BGMSlider.value = 0;
        }
        else
        {
            BGMSlider.value = SaveBGMVolum;
        }
    }
    public void EffectMuteOnOff()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        if (EffectSlider.value > 0)
        {
            SaveEffectVolum = EffectSlider.value;
            EffectSlider.value = 0;

        }
        else
        {
            EffectSlider.value = SaveEffectVolum;
        }
    }

    public void SettingWindowClose()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        SC_SoundControlManager._instance._saveBGMMuteVolume = SaveBGMVolum;
        SC_SoundControlManager._instance._saveEffectMuteVolume = SaveEffectVolum;
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
}
