using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TDHeroSelectUI : MonoBehaviour
{
    [SerializeField] GameObject[] ArrHeroSelectBG = new GameObject[4];
    [SerializeField] GameObject MonsterInfoUI;
    int SelectHeroIndex;
    private void Awake()
    {
        SelectHeroIndex = 0;
        initUI();
        updateSelectHeroCheck();
    }
    void initUI()
    {
        for (int i = 0; i < ArrHeroSelectBG.Length; i++)
        {
            ArrHeroSelectBG[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)i);
            ArrHeroSelectBG[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = SC_UserInfoManager._instance._TDUnitStat[i]["UnitName"] + " LV." + SC_UserInfoManager._instance._userunitlevel[i]["Level"];
            ArrHeroSelectBG[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = SC_UserInfoManager._instance._TDHeroSkillTable[i]["SkillName"].ToString();
            ArrHeroSelectBG[i].transform.GetChild(2).GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._TDHeroSkillTable[i]["SkillDetail"].ToString();
        }
    }

    void updateSelectHeroCheck()
    {
        for (int i = 0; i < ArrHeroSelectBG.Length; i++)
        {
            ArrHeroSelectBG[i].transform.GetChild(3).GetComponent<Image>().color = new Color(0.4352941f, 0.4352941f, 0.4352941f);
        }
        ArrHeroSelectBG[SelectHeroIndex].transform.GetChild(3).GetComponent<Image>().color = Color.yellow;
    }
    public void TouchHero1()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectHeroIndex = 0;
        updateSelectHeroCheck();
    }
    public void TouchHero2()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectHeroIndex = 1;
        updateSelectHeroCheck();
    }
    public void TouchHero3()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectHeroIndex = 2;
        updateSelectHeroCheck();
    }
    public void TouchHero4()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectHeroIndex = 3;
        updateSelectHeroCheck();
    }

    public void SelectBtnClick()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_TDInGameManager._instance._selectHeroIndex = SelectHeroIndex;
        SC_TDBattle._instance.GameStart();
        SC_UIFunctionPool._instance.WindowClose(gameObject);
        SC_TDBattleUI._Instance.BotUIInit();
    }

    public void MonsterInfoBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        SC_UIFunctionPool._instance.WindowOpen(MonsterInfoUI, transform.gameObject);
    }
}
