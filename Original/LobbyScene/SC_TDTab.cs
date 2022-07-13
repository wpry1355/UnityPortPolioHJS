using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TDTab : MonoBehaviour
{
    [SerializeField] Image[] TowerBG = new Image[2];
    [SerializeField] Image FirstTower;
    [SerializeField] Image SecondTower;

    [SerializeField] Text UnitName;
    //Before
    [SerializeField] Text BeforeLevel;
    [SerializeField] Text BeforeAttack;
    [SerializeField] Text BeforeAttackSpeed;
    [SerializeField] Text BeforeRange;
    //After
    [SerializeField] Text AfterLevel;
    [SerializeField] Text AfterAttack;
    [SerializeField] Text AfterAttackSpeed;
    [SerializeField] Text AfterRange;

    // Material
    [SerializeField] GameObject Material1;
    [SerializeField] GameObject Material2;
    [SerializeField] Text NeededGold;
    [SerializeField] GameObject PrefebMaterialInfo;

    [SerializeField] GameObject LevelUpBtnPanel;

    int NowSelectedTower;
    int CostGold;
    int NowGold;

    int MaterialCount1;
    int MaterialCount2;

    int CostMaterial1;
    int CostMaterial2;

    List<Dictionary<string, object>> TDUnitData;
    List<Dictionary<string, object>> TDUnitLevelVar;
    List<Dictionary<string, object>> NeededMaterialTable;
    List<Dictionary<string, object>> ItemTable;

    private void Awake()
    {
        InitTDTabUI();
    }

    void InitTDTabUI()
    {
        TDUnitData = SC_UserInfoManager._instance._TDUnitStat;
        TDUnitLevelVar = SC_UserInfoManager._instance._TDUnitLevelVar;
        NeededMaterialTable = SC_CSVReader.Read("TableUnitLevelUpMaterial");
        ItemTable = SC_UserInfoManager._instance._ItemTable;
        NowGold = SC_UserInfoManager._instance.Gold;
        FirstTower.sprite = SC_ImagePool._Instance.getImage(SC_PublicDefine.eUnitName.TDNormalTower);
        SecondTower.sprite = SC_ImagePool._Instance.getImage(SC_PublicDefine.eUnitName.TDAirTower);
        FirstTowerTouch();
    }


    void InitTDUnitStat()
    {
        int TargetIndex = FindIndexInList(TDUnitData, NowSelectedTower);
        int Level = (int)SC_UserInfoManager._instance._userunitlevel[FindIndexInList(SC_UserInfoManager._instance._userunitlevel, NowSelectedTower)]["Level"];
        CostGold = (int)NeededMaterialTable[Level + 1]["Gold"];
        //스탯창 init
        UnitName.text =TDUnitData[TargetIndex]["UnitName"].ToString();
        BeforeLevel.text = Level.ToString();
        BeforeAttack.text = ((int)TDUnitData[TargetIndex]["OffensePower"]+((int)TDUnitLevelVar[TargetIndex]["OffensePower"]*(Level-1))).ToString();
        BeforeAttackSpeed.text = TDUnitData[TargetIndex]["Cost"].ToString();
        BeforeRange.text = TDUnitData[TargetIndex]["sRange"].ToString();



        AfterLevel.text = (Level+1).ToString();
        AfterAttack.text = ((int)TDUnitData[TargetIndex]["OffensePower"] + (int)TDUnitLevelVar[TargetIndex]["OffensePower"]*Level).ToString();
        AfterAttackSpeed.text = TDUnitData[TargetIndex]["Cost"].ToString();
        AfterRange.text = TDUnitData[TargetIndex]["sRange"].ToString();


        //재료창 init
        Material1.transform.GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)20);
        Material2.transform.GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)NowSelectedTower+20);
        CostMaterial1 = (int)NeededMaterialTable[Level + 1]["Count1"];
        CostMaterial2 = (int)NeededMaterialTable[Level + 1]["Count2"];



        // 필요한 재료 개수 init
        if (SC_UserInfoManager._instance.FindItemIndex
            (SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString()) != -1)
        {
            MaterialCount1 = (int)SC_UserInfoManager._instance._userInventory[SC_UserInfoManager._instance.FindItemIndex
           (SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString())]["Count"];
        }
        else
            MaterialCount1 = 0;


        Material1.transform.GetChild(1).GetComponent<Text>().text = SC_UIFunctionPool._instance.CheckCountAndColor(MaterialCount1, CostMaterial1);


        if (SC_UserInfoManager._instance.FindItemIndex(SC_UserInfoManager._instance._userInventory,
                SC_UserInfoManager._instance._ItemTable[FindIndexInList(SC_UserInfoManager._instance._ItemTable,NowSelectedTower)]["Name"].ToString()) != -1)
        {
            MaterialCount2 = (int)SC_UserInfoManager._instance._userInventory[SC_UserInfoManager._instance.FindItemIndex
                (SC_UserInfoManager._instance._userInventory,
                SC_UserInfoManager._instance._ItemTable[FindIndexInList(SC_UserInfoManager._instance._ItemTable,NowSelectedTower)]["Name"].ToString())]["Count"];

            Material2.transform.GetChild(1).GetComponent<Text>().text = SC_UIFunctionPool._instance.CheckCountAndColor(MaterialCount2, CostMaterial2);
        }
        else
        {
            MaterialCount2 = 0;
            Material2.transform.GetChild(1).GetComponent<Text>().text = "<color=red>" + 0 + "</color>" + "/" + NeededMaterialTable[Level + 1]["Count2"].ToString();
        }

        NeededGold.text = SC_UIFunctionPool._instance.CheckCountAndColor(NowGold, CostGold);

    }

    //타워 아이콘 터치 기점.
    public void FirstTowerTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        NowSelectedTower = 110;
        TowerBG[0].color = Color.white;
        TowerBG[1].color = Color.black;
        InitTDUnitStat();
        LevelUpBtnOnOff();
    }

    public void SecondTowerTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        NowSelectedTower = 111;
        TowerBG[0].color = Color.black;
        TowerBG[1].color = Color.white;
        InitTDUnitStat();
        LevelUpBtnOnOff();
    }

    void LevelUpBtnOnOff()
    {
        if(CostGold>NowGold || CostMaterial1>MaterialCount1|| CostMaterial2>MaterialCount2)
            LevelUpBtnPanel.SetActive(true);
        else
            LevelUpBtnPanel.SetActive(false);
    }

    public void LevelUpBtnTouch()
    {
        //TD 레벨업 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.LevelUp);
        int NowLevel = (int)SC_UserInfoManager._instance._userunitlevel[FindIndexInList(SC_UserInfoManager._instance._userunitlevel, NowSelectedTower)]["Level"];
        SC_UserInfoManager._instance._userunitlevel[FindIndexInList(SC_UserInfoManager._instance._userunitlevel, NowSelectedTower)]["Level"] = NowLevel + 1;
        SC_UserInfoManager._instance.AddToInventory(SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString(), -CostMaterial1);
        SC_UserInfoManager._instance.AddToInventory(SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[FindIndexInList(SC_UserInfoManager._instance._ItemTable, NowSelectedTower)]["Name"].ToString(), -CostMaterial2);
        NowGold = NowGold - CostGold;
        SC_UserInfoManager._instance.Gold = NowGold;
        SC_TopUI._instance.UpdateInfo();
        //데이터 저장
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userunitlevel, "Level", FindIndexInList(SC_UserInfoManager._instance._userunitlevel, NowSelectedTower), SC_UserInfoManager._instance._userunitlevel[FindIndexInList(SC_UserInfoManager._instance._userunitlevel, NowSelectedTower)]["Level"], "UnitLevelData");
        //골드
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userinfodata, "Gold", 0, NowGold, "UserInfomation");
        //인벤토리
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userInventory, "UserInventory");
    
        InitTDUnitStat();
        LevelUpBtnOnOff();
    }

    int FindIndexInList(List<Dictionary<string, object>> DataTable, int UnitIndex)
    {
        int Index = 0;
        for(int i = 0; i<DataTable.Count; i++ )
        {
            if((int)DataTable[i]["Index"] ==UnitIndex)
            {
                Index = i;
            }
        }
        return Index;
    }

    public void CloseWindow()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        SC_TopUI._instance.UpdateInfo();
        SC_MiddleUI._instance.UpdateData();
        SC_UIFunctionPool._instance.WindowClose(gameObject);
    }

    public void OnTouchMaterial1()
    {
        
        PrefebMaterialInfo.GetComponent<SC_ItemInfomation>()._InitCustom(ItemTable, ItemTable[0]["Name"].ToString(), MaterialCount1.ToString(), ItemTable[0]["Detail"].ToString(), ItemTable[0]["FarmingArea"].ToString(), transform,0);
    }
    public void OnTouchMaterial2()
    {
        int TargetIndex = FindIndexInList(ItemTable, NowSelectedTower);
        PrefebMaterialInfo.GetComponent<SC_ItemInfomation>()._InitCustom(ItemTable, ItemTable[TargetIndex]["Name"].ToString(), MaterialCount2.ToString(), ItemTable[TargetIndex]["Detail"].ToString(), ItemTable[TargetIndex]["FarmingArea"].ToString(), transform,NowSelectedTower);
    }
}
