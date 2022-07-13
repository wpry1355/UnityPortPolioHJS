using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_CharacterInfo : MonoBehaviour
{
    [SerializeField] public int Index;
    [SerializeField] Image CharacterImage;
    [SerializeField] Text Job;
    [SerializeField] Text Level;
    [SerializeField] Text HP;
    [SerializeField] Text Attack;
    [SerializeField] Text Defense;
    [SerializeField] Text Critical;
    [SerializeField] Text Evasion;
    [SerializeField] GameObject SkillWindows;
    private void Awake()
    {
        //인게임인지 로비인지
        if (SC_RPGInGameManager._instance != null)
            initIngame(SC_RPGInGameManager._instance.PCUnitList);
        else
            initInLobby(Index);

    }
    private void initIngame(SC_RPGUnit[] ArrayPlayerCharacter)
    {
        CharacterImage.sprite = ArrayPlayerCharacter[Index]._modelProfile;
        Job.text = ArrayPlayerCharacter[Index]._unitName;
        Level.text = ArrayPlayerCharacter[Index]._level.ToString();
        HP.text = ArrayPlayerCharacter[Index]._nowHP + " / " + SC_RPGInGameManager._instance.PCUnitList[Index]._maxHP;
        Attack.text = ArrayPlayerCharacter[Index]._basePower.ToString();
        Defense.text = ArrayPlayerCharacter[Index]._baseDefense.ToString();
        Critical.text = ArrayPlayerCharacter[Index]._criticalRate.ToString();
        Evasion.text = ArrayPlayerCharacter[Index]._evasionRate.ToString();
        SkillWindows.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)(Index + 30));
        SkillWindows.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._SkillDetail[Index]["SkillName"].ToString();
        SkillWindows.transform.GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._SkillDetail[Index]["Detail"].ToString();
    }
    public void initInLobby(int Index)
    {
        List<Dictionary<string, object>> baseStat = SC_UserInfoManager._instance._unitidefaultInfo;
        List<Dictionary<string, object>> levelVar = SC_UserInfoManager._instance._unitlevelvar;
        int charLevel = (int)SC_UserInfoManager._instance._userunitlevel[Index]["Level"];
        CharacterImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)Index);
        Job.text = baseStat[Index]["UnitName"].ToString();
        Level.text = charLevel.ToString();
        HP.text = ((int)baseStat[Index]["HP"] + ((int)levelVar[Index]["HP"] * (charLevel - 1))).ToString();
        Attack.text = ((int)baseStat[Index]["OffensePower"] + ((int)levelVar[Index]["OffensePower"] * (charLevel - 1))).ToString();
        Defense.text = ((int)baseStat[Index]["Defense"] + ((int)levelVar[Index]["Defense"] * (charLevel - 1))).ToString(); ;
        Critical.text = baseStat[Index]["CriticalRate"].ToString();
        Evasion.text = baseStat[Index]["EvasionRate"].ToString();
        SkillWindows.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)(Index + 30));
        SkillWindows.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._SkillDetail[Index]["SkillName"].ToString();
        SkillWindows.transform.GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._SkillDetail[Index]["Detail"].ToString();
    }
}
