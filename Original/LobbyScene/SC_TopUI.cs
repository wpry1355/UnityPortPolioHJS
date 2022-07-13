using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TopUI : MonoBehaviour
{
    [SerializeField] Image _profileImage;
    [SerializeField] Text _userName;
    [SerializeField] Text _gold;
    [SerializeField] Text _cash;
    [SerializeField] Text _playtoken;
    [SerializeField] GameObject _prefabSettingWindow;

    static SC_TopUI _uniqueInstance;
    public static SC_TopUI _instance
    {
        get { return _uniqueInstance; }
    }


    void Awake()
    {
        _uniqueInstance = this;
        UpdateInfo();
        SC_SoundControlManager._instance.BGMSoundPlay(SC_PublicDefine.eSoundTrack.LobbyBGM);
        SC_SceneControlManager._instance.FadeIn();

    }


    public void UpdateInfo()
    {
        _profileImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)SC_UserInfoManager._instance._userinfodata[0]["FrontPosUnit"]);
        _userName.text = SC_UserInfoManager._instance.UserName;
        _gold.text = SC_UserInfoManager._instance.Gold.ToString();
        _cash.text = SC_UserInfoManager._instance.Cash.ToString();
        PlayTokenUpdate();
    }

    public void PlayTokenUpdate()
    {
        _playtoken.text = SC_UserInfoManager._instance.Playtoken.ToString() + "/" + SC_UserInfoManager._instance.MaxPlayToken.ToString();
    }
    public void SettingWindowOpen()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        SC_UIFunctionPool._instance.WindowOpen(_prefabSettingWindow,transform.parent.gameObject);

    }
}
