using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PCThief : SC_RPGUnit
{
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.Thief);
        _aniCtrl = transform.GetComponent<Animator>();
        originPos = transform.position;
        RAniC = _aniCtrl.runtimeAnimatorController;
    }
    void Update()
    {
        if (_isAction)
        {
            if(NextActionNum == 0)
                transform.position = Vector3.MoveTowards(transform.position, attPos, Time.deltaTime * 10f);
            if (NextActionNum == 2)
                transform.position = attPos;

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
                transform.GetComponent<SpriteRenderer>().flipX = false;
                _aniCtrl.SetBool("IsRun", false);
                break;
            case SC_PublicDefine.eActionState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case SC_PublicDefine.eActionState.ATTACK:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.ThiefAttack);
                _aniCtrl.SetTrigger("Attack");
                break;
            case SC_PublicDefine.eActionState.SKILL:
                transform.GetComponent<SpriteRenderer>().flipX = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.ThiefSkill);
                _aniCtrl.SetTrigger("Skill");
                break;
            case SC_PublicDefine.eActionState.HIT:
                _aniCtrl.SetTrigger("Hit");
                break;
            case SC_PublicDefine.eActionState.DEAD:
                _isdead = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.ThiefDeath);
                _aniCtrl.SetBool("IsDead", true);
                break;
        }
    }
    public override void FindTime(int SelectAction)
    {
        switch (SelectAction)
        {
            case 0:
                FindAniPlayTime("PCThief_ATTACK");
                break;
            case 2:
                FindAniPlayTime("PCThief_SKILL");
                break;
            default:
                AniPlayTime = 0.5f;
                break;
        }
    }
    protected override void OverrideSkillPos()
    {
        attPos = new Vector3(targetPos.x + 2, targetPos.y, 0);
    }

    void SkillEvent()
    {
        SkillToDamage();
        ChangeAniToAction(SC_PublicDefine.eActionState.IDEL);
        transform.position = originPos;
    }
}
