using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SC_RPGUnitLevelUp : MonoBehaviour
{
    public int Index = 0;

    //Now Stat
    [SerializeField] Text Job;
    [SerializeField] Text Level;
    [SerializeField] Text HP;
    [SerializeField] Text Attack;
    [SerializeField] Text Defense;


    //After Stat
    [SerializeField] Text AfLevel;
    [SerializeField] Text AfHP;
    [SerializeField] Text AfAttack;
    [SerializeField] Text AfDefense;

    [SerializeField] Text[] CountMaterial = new Text[2];
    [SerializeField] Image[] MaterialImg = new Image[2];
    [SerializeField] Text NeededGold;

    [SerializeField] GameObject LevelUPPanel;
    [SerializeField] GameObject ItemPopUp;

    int NowGold;
    int CostGold;

    int MaterialCount1;
    int MaterialCount2;

    int CostMaterial1;
    int CostMaterial2;

    private void Awake()
    {
        NowGold = SC_UserInfoManager._instance.Gold;
        UpdateData(Index);
    }

    public void UpdateData(int nIndex)
    {
        Index = nIndex;
        List<Dictionary<string, object>> baseStat = SC_UserInfoManager._instance._unitidefaultInfo;
        List<Dictionary<string, object>> levelVar = SC_UserInfoManager._instance._unitlevelvar;
        List<Dictionary<string, object>> NeededMaterialTable = SC_CSVReader.Read("TableUnitLevelUpMaterial");

        int charLevel = (int)SC_UserInfoManager._instance._userunitlevel[Index]["Level"];


        //Value init
        Job.text = baseStat[Index]["UnitName"].ToString();
        Level.text = charLevel.ToString();
        HP.text = ((int)baseStat[Index]["HP"]+ ((int)levelVar[Index]["HP"] * (charLevel - 1))).ToString() ;
        Attack.text = ((int)baseStat[Index]["OffensePower"] + ((int)levelVar[Index]["OffensePower"] * (charLevel - 1))).ToString();
        Defense.text = ((int)baseStat[Index]["Defense"] + ((int)levelVar[Index]["Defense"] * (charLevel - 1))).ToString();


        AfLevel.text = (charLevel + 1).ToString();
        AfHP.text = ((int)baseStat[Index]["HP"] + ((int)levelVar[Index]["HP"] * charLevel)).ToString();
        AfAttack.text = ((int)baseStat[Index]["OffensePower"] + ((int)levelVar[Index]["OffensePower"] * charLevel)).ToString();
        AfDefense.text = ((int)baseStat[Index]["Defense"] + ((int)levelVar[Index]["Defense"] * charLevel)).ToString();
       
        CostGold = (int)NeededMaterialTable[charLevel + 1]["Gold"];
        NeededGold.text = SC_UIFunctionPool._instance.CheckCountAndColor(NowGold, (int)NeededMaterialTable[charLevel + 1]["Gold"]);


        CostMaterial1 = (int)NeededMaterialTable[charLevel + 1]["Count1"];
        CostMaterial2 = (int)NeededMaterialTable[charLevel + 1]["Count2"];


        // 필요 재료 init

        if (SC_UserInfoManager._instance.FindItemIndex
         (SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString()) != -1)
        {
            MaterialCount1 = (int)SC_UserInfoManager._instance._userInventory[SC_UserInfoManager._instance.FindItemIndex
           (SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString())]["Count"];
        }
        else
            MaterialCount1 = 0;

        CountMaterial[0].text = SC_UIFunctionPool._instance.CheckCountAndColor(MaterialCount1, CostMaterial1);


        if (SC_UserInfoManager._instance.FindItemIndex(SC_UserInfoManager._instance._userInventory,
            SC_UserInfoManager._instance._ItemTable[(int)(SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), Job.text) + 1]["Name"].ToString()) != -1)
        {
            MaterialCount2 = (int)SC_UserInfoManager._instance._userInventory[SC_UserInfoManager._instance.FindItemIndex
                (SC_UserInfoManager._instance._userInventory,
                SC_UserInfoManager._instance._ItemTable[(int)(SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), Job.text) + 1]["Name"].ToString())]["Count"];

            CountMaterial[1].text = SC_UIFunctionPool._instance.CheckCountAndColor(MaterialCount2, CostMaterial2);
        }
        else
        {
            MaterialCount2 = 0;
            CountMaterial[1].text = "<color=red>" + 0 + "</color>" + "/" + NeededMaterialTable[charLevel + 1]["Count2"].ToString();
        }
        MaterialImg[0].sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)20);
        MaterialImg[1].sprite = SC_ImagePool._Instance.getImage(((SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), Job.text) + 21));

        LevelUPInvisible();
    }

    public void LevelUPInvisible()
    {
        if (MaterialCount1 >= CostMaterial1 && MaterialCount2 >= CostMaterial2 && NowGold >= CostGold)
        {
            LevelUPPanel.SetActive(false);
        }
        else
            LevelUPPanel.SetActive(true);
    }

    public void LevelUPBtnTouch()
    {
        // RPG캐릭 레벨업 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.LevelUp);
        SC_UserInfoManager._instance._userunitlevel[Index]["Level"] = (int)SC_UserInfoManager._instance._userunitlevel[Index]["Level"] + 1;
        SC_UserInfoManager._instance.AddToInventory(SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString(), -CostMaterial1);
        SC_UserInfoManager._instance.AddToInventory(SC_UserInfoManager._instance._userInventory, SC_UserInfoManager._instance._ItemTable[(int)((SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), Job.text) + 1)]["Name"].ToString(), -CostMaterial2);
        NowGold = NowGold - CostGold;
        UpdateData(Index);
        SC_UserInfoManager._instance.Gold = NowGold;
        SC_TopUI._instance.UpdateInfo();
        //데이터 저장
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userunitlevel,"Level", Index,  SC_UserInfoManager._instance._userunitlevel[Index]["Level"], "UnitLevelData");
        //골드
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userinfodata, "Gold", 0,  NowGold, "UserInfomation");
        //인벤토리
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userInventory, "UserInventory");
    }

    public void OnTouchMaterial1()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        ItemPopUp.GetComponent<SC_ItemInfomation>()._InitCustom(SC_UserInfoManager._instance._ItemTable, SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString(), MaterialCount1.ToString(), SC_UserInfoManager._instance._ItemTable[0]["Detail"].ToString(), SC_UserInfoManager._instance._ItemTable[0]["FarmingArea"].ToString(), transform.parent.parent.parent.parent,0);
    }
    public void OnTouchMaterial2()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        int ItemTableIndex = Index + 1;
        ItemPopUp.GetComponent<SC_ItemInfomation>()._InitCustom(SC_UserInfoManager._instance._ItemTable, SC_UserInfoManager._instance._ItemTable[ItemTableIndex]["Name"].ToString(), MaterialCount2.ToString(), SC_UserInfoManager._instance._ItemTable[ItemTableIndex]["Detail"].ToString(), SC_UserInfoManager._instance._ItemTable[ItemTableIndex]["FarmingArea"].ToString(), transform.parent.parent.parent.parent, Index);
    }
}
