using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SC_TDUnit : MonoBehaviour
{

    string UnitName;
    int Cost;
    int OffensePower;
    int SkillOffensePower;
    int sRange;
    int AttackSpeed;
    int SkillAttackSpeed;
    int CriticalRate;
    int SkillCriticalRate;
    bool IsAirAttack;
    bool IsGroundAttack;
    int Level;
    SC_PublicDefine.eSoundTrack SoundNumber;
    Collider2D[] TargetCol2DCheck;
    protected Vector2 MyPos;
    protected Animator _aniCtrl;
    SC_TDEnemy Target;
    float AttackTime;
    GameObject prafebsRoundRing;
    GameObject sRangeRound;

    public GameObject _sRangeRound
    {
        get{ return sRangeRound; }
    }
    public string _unitName
    {
        get { return UnitName; }
    }
    public int _cost
    {
        get { return Cost; }
    }
    public int _offensePower
    {
        get { return OffensePower; }
    }
    public int _finishOffensePower
    {
        get { return OffensePower + SkillOffensePower; }
    }
    public float _sRange
    {
        get { return sRange; }
    }
    public float _attackSpeed
    {
        get { return AttackSpeed; }
    }
    public float _criticalRate
    {
        get { return CriticalRate; }
    }
    public float _finishCriticalRate
    {
        get { return CriticalRate + SkillCriticalRate; }
    }

    public bool _isAirAttack
    {
        get { return IsAirAttack; }
    }
    public bool _isGroundAttack
    {
        get { return IsGroundAttack; }
    }
    public int _level
    {
        get { return Level; }
    }

    //override Func
    protected virtual void ChangeAniToAction(SC_PublicDefine.eTDTowerActionState state) { }
    protected virtual void AttackEvent() { }

    void Update()
    {
        if (AttackTime < AttackSpeed)
            AttackTime += Time.deltaTime;

        if (AttackTime >= AttackSpeed)
        {
            // 공격 기점
            if (FindAttackTarget() != null)
            {
                ChangeAniToAction(SC_PublicDefine.eTDTowerActionState.ATTACK);
                if (FindAttackTarget()._isDead == false)
                    FindAttackTarget().TakeHit(Damage());
                AttackTime = 0;
            }
        }
    }

    //init
    protected void InitData(SC_PublicDefine.eUnitName Unitname)
    {
        List<Dictionary<string, object>> LevelData = SC_UserInfoManager._instance._userunitlevel;
        int Target = 0;
        List<Dictionary<string, object>> baseStat = SC_UserInfoManager._instance._TDUnitStat;
        List<Dictionary<string, object>> levelVar = SC_UserInfoManager._instance._TDUnitLevelVar;

        for (int i = 0; i < baseStat.Count; i++)
        {
            if ((int)baseStat[i]["Index"] == (int)Unitname)
            {
                Target = i;
            }
        }
        Level = (int)SC_UserInfoManager._instance._userunitlevel[Target]["Level"];
        UnitName = baseStat[Target]["UnitName"].ToString();
        Cost = (int)baseStat[Target]["Cost"];
        OffensePower = (int)baseStat[Target]["OffensePower"] + ((int)levelVar[Target]["OffensePower"] * (Level - 1));
        sRange = (int)baseStat[Target]["sRange"];
        AttackSpeed = (int)baseStat[Target]["AttackSpeed"];
        CriticalRate = (int)baseStat[Target]["CriticalRate"];
        SoundNumber = (SC_PublicDefine.eSoundTrack)Unitname;
        if ("true" == (string)baseStat[Target]["IsAirAttack"])
            IsAirAttack = true;
        else
            IsAirAttack = false;
        if ("true" == (string)baseStat[Target]["IsGroundAttack"])
            IsGroundAttack = true;
        else
            IsGroundAttack = false;
        SkillCheck();
        prafebsRoundRing = SC_TDBattleUI._Instance.sRangeRing;
        sRangeRound = Instantiate(prafebsRoundRing, transform.position + Vector3.up, Quaternion.identity);
        sRangeRound.transform.localScale = sRangeRound.transform.localScale * sRange;
        sRangeRound.SetActive(false);
    }


    //스킬 관련 func
    protected void ApplyHeroSkill()
    {
        SC_TDBattle._instance.IsBuildedHero = true;
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 0:
            case 1:
                for (int i = 0; i < SC_TDBattle._instance.BuildedTowerList.Count; i++)
                {
                    SC_TDBattle._instance.BuildedTowerList[i].GetComponent<SC_TDUnit>().SkillOn();
                }
                break;
            case 2:
            case 3:
                for (int i = 0; i < SC_TDBattle._instance.SpawnEnemyList.Count; i++)
                {
                    if (SC_TDBattle._instance.SpawnEnemyList[i] != null)
                        SC_TDBattle._instance.SpawnEnemyList[i].GetComponent<SC_TDEnemy>().SkillOn();
                }
                break;
            default:
                break;
        }
    }
    public void UnApplyHeroSkill()
    {
        SC_TDBattle._instance.IsBuildedHero = true;
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 0:
            case 1:
                for (int i = 0; i < SC_TDBattle._instance.BuildedTowerList.Count; i++)
                {
                    SC_TDBattle._instance.BuildedTowerList[i].GetComponent<SC_TDUnit>().SkillOff();
                }
                break;
            case 2:
            case 3:
                for (int i = 0; i < SC_TDBattle._instance.SpawnEnemyList.Count; i++)
                {
                    SC_TDBattle._instance.SpawnEnemyList[i].GetComponent<SC_TDUnit>().SkillOff();
                }
                break;
            default:
                break;
        }
    }
    void SkillOn()
    {
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 0:
                SkillOffensePower = (int)(OffensePower * 0.2f);
                break;
            case 1:
                SkillCriticalRate = 5;
                break;
            default:
                break;
        }
    }
    void SkillOff()
    {
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 0:
                SkillOffensePower = 0;
                break;
            case 1:
                SkillCriticalRate = 0;
                break;
            default:
                break;
        }
    }
    public void SkillCheck()
    {
        if (SC_TDBattle._instance.IsBuildedHero)
            SkillOn();
        else
            SkillOff();
    }

    

    // Tower 공격 관련 Func
    int Damage()
    {
        //치명타
        int Damage = 0;
        int rd = Random.Range(1, 101);
        if (rd <= _finishCriticalRate)
        {
            Damage = (int)(_finishOffensePower * 1.5f);
        }
        else
            Damage = _finishOffensePower;
        return Damage;
    }

    public SC_TDEnemy FindAttackTarget()
    {
        TargetCol2DCheck = Physics2D.OverlapCircleAll(MyPos + new Vector2(0, 1), sRange); // sRange 수정.
        List<Collider2D> ListCol2D = new List<Collider2D>();
        int index = 0;
        for (int i = 0; i < TargetCol2DCheck.Length; i++)
        {
            if (TargetCol2DCheck[i].tag == "TDEnemy")
            {
                if (TargetCol2DCheck[i].gameObject.GetComponent<SC_TDEnemy>()._isFlying)
                {
                    if (IsAirAttack)
                        ListCol2D.Add(TargetCol2DCheck[i]);
                }
                else
                {
                    if(IsGroundAttack)
                        ListCol2D.Add(TargetCol2DCheck[i]);
                }
            }
        }
        if(ListCol2D.Count != 0)
        {
            float NearEnemy = Vector3.Distance(ListCol2D[0].gameObject.transform.position, transform.position);
            for (int i = 1; i < ListCol2D.Count; i++)
            {
                if (NearEnemy > Vector3.Distance(ListCol2D[i].gameObject.transform.position, transform.position))
                {
                    NearEnemy = Vector3.Distance(ListCol2D[i].gameObject.transform.position, transform.position);
                    index = i;
                }
            }
            Target = ListCol2D[index].gameObject.GetComponent<SC_TDEnemy>();
            return Target;
        }
        else
            Target = null;
        return Target;
    }
    public void CheckAttackRotation()
    {
        float x = FindAttackTarget().transform.position.x - transform.position.x;
        if (x < 0)
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else
            transform.gameObject.GetComponent<SpriteRenderer>().flipX = false;

    }
    protected void playAttackSound()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SoundNumber);
    }
}
