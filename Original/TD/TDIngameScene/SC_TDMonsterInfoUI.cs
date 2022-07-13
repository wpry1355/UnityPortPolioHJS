using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TDMonsterInfoUI : MonoBehaviour
{
    [SerializeField] Text MonsterName;
    [SerializeField] Text MonsterDetail;
    [SerializeField] GameObject[] MonsterUI = new GameObject[4];
    List<string> MonsterInfo = new List<string>();
    List<string> StageMonsterName = new List<string>();
    List<int> ArrMonsterInfoIndex = new List<int>();
    int SelectMonster;

    private void Awake()
    {
        SelectMonster = 0;
        MonsterData();
        MonsterName.text = SC_UserInfoManager._instance._TDMonsterDetail[ArrMonsterInfoIndex[SelectMonster]]["UnitName"].ToString();
        MonsterDetail.text = SC_UserInfoManager._instance._TDMonsterDetail[ArrMonsterInfoIndex[SelectMonster]]["Detail"].ToString();
        if(ArrMonsterInfoIndex.Count< MonsterUI.Length)
        {
            for (int i = 0; i< MonsterUI.Length - ArrMonsterInfoIndex.Count;i++)
            {
                MonsterUI[MonsterUI.Length - 1 - i].SetActive(false);
            }
        }
    }


    void MonsterData()
    {
        List <Dictionary<string,object>>StageData = SC_TDInGameManager._instance._StageAndRoundData;
        
        for (int i = 0; i < StageData.Count; i++)
        {
            if((int)StageData[i]["Stage"] == SC_TDInGameManager._instance._stageNumIndex + 1)
            {
                StageMonsterName.Add(StageData[i]["Name"].ToString());
            }
        
        }
        StageMonsterName = StageMonsterName.Distinct().ToList();

        for(int i = 0; i<StageMonsterName.Count; i++)
        {
            ArrMonsterInfoIndex.Add((int)(SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), StageMonsterName[i]) - 120);
        }
        UpdateImg();
    }

    public void Monster1stTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectMonster = 0;
        UpdateMSName();
        UpdateDetail();

    }
    public void Monster2ndTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectMonster = 1;
        UpdateMSName();
        UpdateDetail();
    }
    public void Monster3rdTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectMonster = 2;
        UpdateMSName();
        UpdateDetail();
    }
    public void Monster4thTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SelectMonster = 3;
        UpdateMSName();
        UpdateDetail();
    }

    private void UpdateImg()
    {
        for (int i = 0; i < StageMonsterName.Count; i++)
        {
            MonsterUI[i].transform.GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)ArrMonsterInfoIndex[i]+120);
        }
    }
    private void UpdateMSName()
    {
        Debug.Log(SelectMonster);
        MonsterName.text = SC_UserInfoManager._instance._TDMonsterDetail[ArrMonsterInfoIndex[SelectMonster]]["UnitName"].ToString();
    }
    private void UpdateDetail()
    {
        MonsterDetail.text = SC_UserInfoManager._instance._TDMonsterDetail[ArrMonsterInfoIndex[SelectMonster]]["Detail"].ToString();
    }


    public void CloseBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        gameObject.SetActive(false);
    }
}
