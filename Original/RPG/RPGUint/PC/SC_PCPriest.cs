using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PCPriest : SC_RPGUnit
{
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.Priest);
        _aniCtrl = transform.GetComponent<Animator>();
        originPos = transform.position;
        RAniC = _aniCtrl.runtimeAnimatorController;
    }
    void Update()
    {
        if (_isAction)
        {
            transform.position = Vector3.MoveTowards(transform.position, attPos, Time.deltaTime * 10f);

            if (attPos == transform.position)
            {
                switch (NextActionNum)
                {
                    case 0:
                        ChangeAniToAction(SC_PublicDefine.eActionState.ATTACK);
                        break;
                    case 2:
                        ChangeAniToAction(SC_PublicDefine.eActionState.SKILL);
                        break;
                    default:
                        break;
                }
                _isAction = false;
            }
        }
    }

    public override void ChangeAniToAction(SC_PublicDefine.eActionState state)
    {
        switch (state)
        {
            case SC_PublicDefine.eActionState.IDEL:
                _aniCtrl.SetBool("IsRun", false);
                break;
            case SC_PublicDefine.eActionState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case SC_PublicDefine.eActionState.ATTACK:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.PriestAttack);
                _aniCtrl.SetTrigger("Attack");
                break;
            case SC_PublicDefine.eActionState.SKILL:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.PriestSkill);
                _aniCtrl.SetTrigger("Skill");
                break;
            case SC_PublicDefine.eActionState.HIT:
                _aniCtrl.SetTrigger("Hit");
                break;
            case SC_PublicDefine.eActionState.DEAD:
                _isdead = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.PriestDeath);
                _aniCtrl.SetBool("IsDead", true);
                break;
        }
    }
    public override void FindTime(int SelectAction)
    {
        switch (SelectAction)
        {
            case 0:
                FindAniPlayTime("PCPriest_ATTACK");
                break;
            case 2:
                FindAniPlayTime("PCPriest_SKILL");
                break;
            default:
                AniPlayTime = 0.5f;
                break;
        }
    }

    protected override void OverrideSkillPos()
    {
        attPos = SC_RPGInGameManager._instance._skillPos.transform.position;
    }

    // 스킬
    void PriestSkill()
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            int HealValue;
            HealValue = _skillPower + (int)(SC_RPGInGameManager._instance.PCUnitList[i]._maxHP / 10);
            if(SC_RPGInGameManager._instance.PCUnitList[i]._nowHP + HealValue >= SC_RPGInGameManager._instance.PCUnitList[i]._maxHP)
                SC_RPGInGameManager._instance.PCUnitList[i]._nowHP = SC_RPGInGameManager._instance.PCUnitList[i]._maxHP;
            else
                SC_RPGInGameManager._instance.PCUnitList[i]._nowHP += HealValue;
            SC_RPGBattleUI._instance.UpdateHPbar();
        }
    }
    void SkillEvent()
    {
        PriestSkill();
        ChangeAniToAction(SC_PublicDefine.eActionState.IDEL);
        transform.position = originPos;
    }
}
