using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SC_RPGUnit : MonoBehaviour
{
    string Name;
    bool CampType = true;

    int SkillPower;
    int NowSkillTurn;
    int MaxHP;
    int NowHP;
    int SkillTurn;
    int OffensePower;
    int Defense;
    int EvasionRate;
    int CriticalRate;
    int Level;
    bool AttackType;
    Sprite ModelProfile;

    bool isDead = false;
    bool isDefense = false;

    public int targetArrNum;
    public Vector2 originPos;
    protected Vector3 attPos;
    protected Vector3 targetPos;
    protected int rd
    protected bool _isAction = false;
    protected int NextActionNum;

    protected Animator _aniCtrl;
    protected RuntimeAnimatorController RAniC;
    protected float _aniPlayTimef;

    public float AniPlayTime
    {
        get { return _aniPlayTime; }
        set { _aniPlayTime = value; }
    }

    public string _unitName
    {
        get { return Name; }
    }
    public bool _campType
    {
        get { return CampType; }
    }
    public int _skillPower
    {
        get { return SkillPower; }
    }
    public int _level
    {
        get { return Level; }
    }
    public int _maxHP
    {
        get { return MaxHP; }
    }
    public int _nowHP
    {
        get { return NowHP; }
        set { NowHP = value; }
    }

    public float _HPRate
    {
        get { return (float)NowHP / (float)MaxHP; }
    }
    public int _skillTurn
    {
        get { return SkillTurn; }
    }

    public int _nowSkillTurn
    {
        get { return NowSkillTurn; }
        set { NowSkillTurn = value; }
    }

    public int _basePower
    {
        get { return OffensePower; }
    }
    public int _baseDefense
    {
        get { return Defense; }
    }
    protected float _DMDefense
    {
        get { return Defense * 2f; }
    }
    public int _evasionRate
    {
        get { return EvasionRate; }
    }
    public int _criticalRate
    {
        get { return CriticalRate; }
    }
    public Sprite _modelProfile
    {
        get { return ModelProfile; }
    }
    protected bool _attackType
    {
        get { return AttackType; }
    }
    public bool _isdead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    public bool _isDefense
    {
        get { return isDefense; }
        set { isDefense = value; }
    }

    //override Func
    public virtual void ChangeAniToAction(SC_PublicDefine.eActionState state) { }
    protected virtual void OverrideSkillPos() { }
    public virtual void FindTime(int SelectAction) { }

    //init
    protected void InitData(SC_PublicDefine.eUnitName Unitname)
    {
        
        List<Dictionary<string, object>> LevelData = SC_UserInfoManager._instance._userunitlevel;
        int Target = 0;

        for (int i = 0; i< LevelData.Count;i++)
        {
            if((int)LevelData[i]["Index"] == (int)Unitname)
            {
                Target = i;
            }
        }
        Level = (int)SC_UserInfoManager._instance._userunitlevel[Target]["Level"];

        List<Dictionary<string, object>> baseStat = SC_UserInfoManager._instance._unitidefaultInfo;
        List<Dictionary<string, object>> levelVar = SC_UserInfoManager._instance._unitlevelvar;


        Name = baseStat[Target]["UnitName"].ToString();
        if ("true" == (string)baseStat[Target]["CampType"])
            CampType = true;
        else
            CampType = false;
        SkillPower = (int)baseStat[Target]["SkillPower"];
        MaxHP = (int)baseStat[Target]["HP"] + ((int)levelVar[Target]["HP"]* (Level-1));
        NowHP = MaxHP;
        SkillTurn = (int)baseStat[Target]["SkillTurn"];
        NowSkillTurn = SkillTurn;
        OffensePower = (int)baseStat[Target]["OffensePower"] + ((int)levelVar[Target]["OffensePower"] * (Level - 1)); 
        Defense = (int)baseStat[Target]["Defense"] + ((int)levelVar[Target]["Defense"] * (Level - 1));
        if ("true" == (string)baseStat[Target]["AttackType"])
            AttackType = true;
        else
            AttackType = false;
        ModelProfile = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), Name));
        EvasionRate = (int)baseStat[Target]["EvasionRate"];
        CriticalRate = (int)baseStat[Target]["CriticalRate"];
    }




    //전투 Func
    //다음 액션 Setting
    public void SelectAction(int nextActionNum)
    {
        NextActionNum = nextActionNum;
        if (CampType)
        {
            switch (NextActionNum)
            {
                case 0:
                    Target();
                    return;
                case 1:
                    DefenseMode();
                    return;
                case 2:
                    Target();
                    return;
                default:
                    return;
            }
        }
    }
    //타겟 Setting
    public void Target()
    {
        if (CampType)
        {
            isDefense = false;
            if (SC_RPGBattle._instance.AliveEnemyUnit <= 0)
                return;

            if (_attackType)
            {
                // 전위 공격.
                if (SC_RPGInGameManager._instance.MSUnitList[0]._isdead == false)
                {
                    Action(0);
                }
                else
                {
                    if (SC_RPGInGameManager._instance.MSUnitList[1]._isdead == false && SC_RPGInGameManager._instance.MSUnitList[2]._isdead == false)
                    {
                        rd = Random.Range(1, 3);
                        Action(rd);
                    }
                    else
                    {
                        if (SC_RPGInGameManager._instance.MSUnitList[1]._isdead == false && SC_RPGInGameManager._instance.MSUnitList[2]._isdead == true)
                        {
                            Action(1);
                        }
                        else
                        {
                            Action(2);
                        }
                    }
                }
            }
            else
            {
                // 후위 공격
                if (SC_RPGInGameManager._instance.MSUnitList[1]._isdead == false || SC_RPGInGameManager._instance.MSUnitList[2]._isdead == false)
                {
                    if (SC_RPGInGameManager._instance.MSUnitList[1]._isdead == false && SC_RPGInGameManager._instance.MSUnitList[2]._isdead == false)
                    {
                        rd = Random.Range(1, 3);
                        Action(rd);
                    }
                    else
                    {
                        if (SC_RPGInGameManager._instance.MSUnitList[1]._isdead == false && SC_RPGInGameManager._instance.MSUnitList[2]._isdead == true)
                        {
                            Action(1);
                        }
                        else
                        {
                            Action(2);
                        }

                    }

                }
                else
                {
                    Action(0);
                }
            }
        }
        else
        {
            if (SC_RPGBattle._instance.AliveUserUnit <= 0)
                return;

            if (_attackType)
            {
                
                if (SC_RPGInGameManager._instance.PCUnitList[0]._isdead == false)
                {
                    Action(0);
                }
                else
                {
                    if (SC_RPGInGameManager._instance.PCUnitList[1]._isdead == false && SC_RPGInGameManager._instance.PCUnitList[2]._isdead == false)
                    {
                        rd = Random.Range(1, 3);
                        Action(rd);
                    }
                    else
                    {
                        if (SC_RPGInGameManager._instance.PCUnitList[1]._isdead == false && SC_RPGInGameManager._instance.PCUnitList[2]._isdead == true)
                        {
                            Action(1);
                        }
                        else
                        {
                            Action(2);
                        }
                    }
                }
            }
            else
            {

                if (SC_RPGInGameManager._instance.PCUnitList[1]._isdead == false || SC_RPGInGameManager._instance.PCUnitList[2]._isdead == false)
                {
                    if (SC_RPGInGameManager._instance.PCUnitList[1]._isdead == false && SC_RPGInGameManager._instance.PCUnitList[2]._isdead == false)
                    {
                        rd = Random.Range(1, 3);
                        Action(rd);
                    }
                    else
                    {
                        if (SC_RPGInGameManager._instance.PCUnitList[1]._isdead == false && SC_RPGInGameManager._instance.PCUnitList[2]._isdead == true)
                        {
                            Action(1);
                        }
                        else
                        {
                            Action(2);
                        }

                    }

                }
                else
                {
                    Action(0);
                }
            }
        }
    }

    //액션 실행
    protected void Action(int unitListNum)
    {
        targetArrNum = unitListNum;
        if (CampType)
        {
            targetPos = SC_RPGInGameManager._instance.MSUnitList[targetArrNum].gameObject.transform.position;
            if (NextActionNum == 0)
                attPos = new Vector3(targetPos.x - 2, targetPos.y, 0);
            else if (NextActionNum == 2)
                OverrideSkillPos();
        }
        else
        {
            targetPos = SC_RPGInGameManager._instance.PCUnitList[targetArrNum].gameObject.transform.position;
            if (_nowSkillTurn > 0)
                attPos = new Vector3(targetPos.x + 2, targetPos.y, 0);
            else
                OverrideSkillPos();
        }
        _isAction = true;
        if(!_isDefense)
            ChangeAniToAction(SC_PublicDefine.eActionState.RUN);
    }

    //애니메이션
    //데미지가 들어가는 기점
    //애니메이션 이벤트
    protected IEnumerator GoalEvent()
    {
        if (CampType)
        {
            if (NextActionNum == 0)
            {
                if (Evasion(SC_RPGInGameManager._instance.MSUnitList))
                {
                    SC_RPGBattleUI._instance.CreatTextHud(SC_RPGInGameManager._instance.MSUnitList[targetArrNum], "Evasion!! ", Color.gray);
                }
                else
                    AttackToDamage();
            }
            else if (NextActionNum == 2)
                SkillToDamage();
        }
        else
        {
            if (Evasion(SC_RPGInGameManager._instance.PCUnitList))
            {
                SC_RPGBattleUI._instance.CreatTextHud(SC_RPGInGameManager._instance.PCUnitList[targetArrNum], "Evasion!! ", Color.gray);
            }
            else
                AttackToDamage();
        }
        
        ChangeAniToAction(SC_PublicDefine.eActionState.IDEL);
        yield return new WaitForSeconds(0.5f);
        transform.position = originPos;
    }

    //애니메이션 시간 가져오기
    protected float FindAniPlayTime(string aniName)
    {
        for (int i = 0; i < RAniC.animationClips.Length; i++)
        {
            if (RAniC.animationClips[i].name == aniName)
            {
                AniPlayTime = RAniC.animationClips[i].length;
            }
        }
        return AniPlayTime;
    }


    // 데미지 연산

    //치명타, 회피율 세팅
    bool Evasion(SC_RPGUnit[] unitList)
    {
        int rd = Random.Range(1, 101);
        if (rd <= unitList[targetArrNum]._evasionRate)
            return true;
        else
            return false;
    }
    bool Critical()
    {
        int rd = Random.Range(1, 101);
        if (rd <= CriticalRate)
            return true;
        else
            return false;
    }
    // 일반 공격 데미지 연산
    protected void AttackToDamage()
    {
        int rdRange = (int)_basePower / 5;
        int rdDamage = Random.Range(-rdRange, rdRange + 1);
        int totalPower = _basePower + rdDamage;
        if (_campType)
        {
            Damage(SC_RPGInGameManager._instance.MSUnitList, totalPower);
            SC_RPGBattleUI._instance.UpdateMSHPBar(targetArrNum);
        }
        else
        {
            Damage(SC_RPGInGameManager._instance.PCUnitList, totalPower);
            SC_RPGBattleUI._instance.UpdateHPbar();
        }
    }
    // 스킬데미지 연산
    public void SkillToDamage()
    {
        int totalPower = _skillPower + (int)(_basePower * 0.8f);
        if (_campType)
        {
            Damage(SC_RPGInGameManager._instance.MSUnitList, totalPower);
            SC_RPGBattleUI._instance.UpdateMSHPBar(targetArrNum);
        }
        else
        {
            Damage(SC_RPGInGameManager._instance.PCUnitList, totalPower);
            SC_RPGBattleUI._instance.UpdateHPbar();
        }
    }
    // 최종데미지 연산
    void Damage(SC_RPGUnit[] unitList, int totalPower)
    {
        int finishDamage = 0;
        int nowHP = unitList[targetArrNum]._nowHP;
        bool IsCritical = Critical();
        if (IsCritical)
            totalPower = (int)(totalPower * 1.5f);

        if (unitList[targetArrNum]._isDefense)
        {
            if (totalPower <= (int)unitList[targetArrNum]._DMDefense)
                finishDamage = 1;
            else
                finishDamage = totalPower - (int)unitList[targetArrNum]._DMDefense;
        }
        else
        {
            if (totalPower <= unitList[targetArrNum]._baseDefense)
                finishDamage = 1;
            else
                finishDamage = totalPower - unitList[targetArrNum]._baseDefense;
        }

        if (IsCritical)
            SC_RPGBattleUI._instance.CreatTextHud(unitList[targetArrNum], "Critical!! " + finishDamage.ToString(), Color.red);
        else
            SC_RPGBattleUI._instance.CreatTextHud(unitList[targetArrNum], finishDamage.ToString(), Color.red);
        unitList[targetArrNum]._nowHP = nowHP - finishDamage;
        unitList[targetArrNum].ChangeAniToAction(SC_PublicDefine.eActionState.HIT);

        if (unitList[targetArrNum]._nowHP <= 0)
        {
            if (CampType)
            {
                SC_RPGBattleUI._instance.CreateReword(unitList[targetArrNum].transform);
                SC_RPGBattleUI._instance.UpdateReword();
                SC_RPGBattle._instance.AliveEnemyUnit -= 1;
            }
            else
            {
                SC_RPGBattle._instance.AliveUserUnit -= 1;
                SC_RPGBattleUI._instance.DeadUnitUI(targetArrNum);
                SC_RPGFieldHud._instance.FieldDeadPanel(targetArrNum);
            }
            unitList[targetArrNum].ChangeAniToAction(SC_PublicDefine.eActionState.DEAD);
        }
    }

    //방어모드
    protected void DefenseMode()
    {
        isDefense = true;
    }


}
       

