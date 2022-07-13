using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MSEarthSpirit : SC_RPGUnit
{
    Transform skillPos;

    void Awake()
    {

        InitData(SC_PublicDefine.eUnitName.EarthSpirit);
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
                if (_nowSkillTurn > 0)
                {
                    ChangeAniToAction(SC_PublicDefine.eActionState.ATTACK);
                }
                else
                {
                    ChangeAniToAction(SC_PublicDefine.eActionState.SKILL);
                    _nowSkillTurn = _skillTurn;
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
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.EarthSpritAttack);
                _aniCtrl.SetTrigger("Attack");
                break;
            case SC_PublicDefine.eActionState.SKILL:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.EarthSpritSkill);
                _aniCtrl.SetTrigger("Skill");
                break;
            case SC_PublicDefine.eActionState.HIT:
                _aniCtrl.SetTrigger("Hit");
                break;
            case SC_PublicDefine.eActionState.DEAD:
                _isdead = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.EarthSpritDeath);
                _aniCtrl.SetBool("IsDead", true);
                break;
        }
    }

    public override void FindTime(int SelectAction)
    {
        switch (SelectAction)
        {
            case 0:
                FindAniPlayTime("MSEarthSpirit_ATTACK");
                break;
            case 2:
                FindAniPlayTime("MSEarthSpirit_SKILL");
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

    IEnumerator SkillEvent()
    {
        SkillToDamage();
        ChangeAniToAction(SC_PublicDefine.eActionState.IDEL);
        yield return new WaitForSeconds(0.5f);
        transform.position = originPos;
    }
}
