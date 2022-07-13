using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_MiddleUI : MonoBehaviour
{
    [SerializeField] Image ProfileChar;
    [SerializeField] GameObject PrefebHeroManagement;
    [SerializeField] GameObject PrefebTDMagement;

    static SC_MiddleUI _uniqueInstance;
    public static SC_MiddleUI _instance
    {
        get { return _uniqueInstance; }
    }


    private void Start()
    {
        UpdateData();
        _uniqueInstance = this;
    }

    public void UpdateData()
    {
        if ((string)SC_UserInfoManager._instance._userinfodata[0]["ProfileImage"] != "null")
            ProfileChar.sprite = (Sprite)SC_UserInfoManager._instance._userinfodata[0]["ProfileImage"+10];
        else
            ProfileChar.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)SC_UserInfoManager._instance._userinfodata[0]["FrontPosUnit"]+10);
    }

    public void HeroManagementClick()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        Instantiate(PrefebHeroManagement, transform.position, Quaternion.identity);
    }
    public void TowerManagementClick()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        Instantiate(PrefebTDMagement, transform.position, Quaternion.identity);
    }
}

